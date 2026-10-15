using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
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
    // 血债清算 — 2费：造成本场战斗中累计失去生命值的伤害。可暴击。升级后费用-1。
    public sealed class Blood_Debt : CardModel, ICritAttack
    {
        public int CritBonusRate => 0;

        protected override List<DynamicVar> CanonicalVars => [
            new CalculationBaseVar(0m),
            new ExtraDamageVar(1m),
            new CalculatedDamageVar(ValueProp.Move)
                .WithMultiplier((card, _) => (decimal)System.Math.Max(0, card.Owner.Creature.GetPowerAmount<CombatHpLossPower>() - 1)),
        ];

        public Blood_Debt()
            : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
        {
        }

        protected override async Task OnPlay(
            PlayerChoiceContext choiceContext,
            CardPlay cardPlay)
        {
            int damage = System.Math.Max(0, Owner.Creature.GetPowerAmount<CombatHpLossPower>() - 1);

            if (damage > 0)
            {
                await CreatureCmd.Damage(
                    choiceContext,
                    cardPlay.Target,
                    damage,
                    ValueProp.Move,
                    Owner.Creature);

                VfxCmd.PlayOnCreature(cardPlay.Target, "vfx/vfx_bloody_impact");
            }
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}
