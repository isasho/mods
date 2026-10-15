using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Core;
using sts2mod.Powers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 收割 — 2费：造成12点伤害。若目标有流血，伤害的30%溅射到全体敌人，回复溅射总量生命。升级后+4伤害，溅射比例+15%。
    public sealed class Blood_Reap : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(12m, ValueProp.Move)
        ];

        public Blood_Reap()
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
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this).Targeting(target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            if (BleedManager.GetBleed(target) <= 0)
                return;

            decimal splashRatio = IsUpgraded ? 0.45m : 0.30m;
            int splashDamage = (int)decimal.Floor(DynamicVars.Damage.BaseValue * splashRatio);
            if (splashDamage <= 0)
                return;

            var combat = cardPlay.Card?.CombatState;
            if (combat == null) return;

            int totalSplash = 0;
            foreach (var enemy in combat.Enemies.ToList())
            {
                if (enemy == target)
                    continue;
                await CreatureCmd.Damage(choiceContext, enemy, splashDamage,
                    ValueProp.Unblockable | ValueProp.Unpowered, this);
                totalSplash += splashDamage;
            }

            if (totalSplash > 0)
                await CreatureCmd.Heal(Owner.Creature, totalSplash);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(4m);
        }
    }
}
