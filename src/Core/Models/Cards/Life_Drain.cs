using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Powers;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 生命榨取 — 2费：造成8点伤害。伤害的50%转化为生命回复和目标等额生命流失。若目标有流血，转化率提升至100%。升级后+3伤害。
    public sealed class Life_Drain : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(8m, ValueProp.Move)
        ];

        public Life_Drain()
            : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        protected override bool ShouldGlowGoldInternal
        {
            get
            {
                if (CombatState == null) return false;
                foreach (var enemy in CombatState.Enemies)
                    if (enemy.GetPower<BleedPower>() != null)
                        return true;
                return false;
            }
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var target = cardPlay.Target;
            if (target == null) return;

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this).Targeting(target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            decimal ratio = target.GetPower<BleedPower>() != null ? 1m : 0.5m;
            int drain = (int)decimal.Floor(DynamicVars.Damage.BaseValue * ratio);
            if (drain > 0)
            {
                await CreatureCmd.Damage(choiceContext, target, drain,
                    ValueProp.Unblockable | ValueProp.Unpowered, this);
                await CreatureCmd.Heal(Owner.Creature, drain);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
