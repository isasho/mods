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
    // 汲血 — 1费：造成5点伤害。若目标有流血，移除4层流血回复等量生命，并对目标造成等额生命流失。升级后+3伤害，移除回复+2。
    public sealed class Drain_Blood : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(5m, ValueProp.Move)
        ];

        public Drain_Blood()
            : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

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

            var bleed = target.GetPower<BleedPower>();
            if (bleed == null) return;

            int consume = IsUpgraded ? 6 : 4;
            int available = (int)bleed.Amount;
            int actual = Math.Min(available, consume);
            if (actual <= 0) return;

            if (actual >= available)
                await PowerCmd.Remove(bleed);
            else
                await PowerCmd.Apply<BleedPower>(choiceContext, target, -actual, Owner.Creature, this, false);

            await CreatureCmd.Damage(choiceContext, target, actual,
                ValueProp.Unblockable | ValueProp.Unpowered, this);
            await CreatureCmd.Heal(Owner.Creature, actual);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
