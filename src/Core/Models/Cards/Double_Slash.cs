using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Core;
using sts2mod.Powers;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 无情切割 — 1费：造成5点伤害两次。若目标有流血，每次命中以(暴击率+流血层数)%概率追加5×2伤害。升级后+2伤害。
    public sealed class Double_Slash : CardModel, IRepeatAttackInfo
    {
        // 流血追加的额外 2 段为条件触发，不纳入静态显示。
        public int HitsPerTarget => 2;

        public int BleedAppliesPerTarget => 0;

        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(5m, ValueProp.Move)
        ];

        public Double_Slash()
            : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var target = cardPlay.Target;
            if (target == null) return;

            for (int i = 0; i < 2; i++)
                await DoHit(choiceContext, target);
        }

        private async Task DoHit(PlayerChoiceContext choiceContext, Creature target)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this).Targeting(target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            var bleed = target.GetPower<BleedPower>();
            if (bleed == null) return;

            int critRate = CritManager.GetCritRate(Owner.Creature);
            int bleedStacks = (int)bleed.Amount;
            int chance = critRate + bleedStacks;
            if (chance > 100) chance = 100;

            bool triggered = Owner.RunState.Rng.CombatCardGeneration.NextInt(100) < chance;
            if (triggered)
            {
                for (int i = 0; i < 2; i++)
                {
                    await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                        .FromCard(this).Targeting(target)
                        .WithHitFx("vfx/vfx_attack_slash")
                        .Execute(choiceContext);
                }
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(2m);
        }
    }
}
