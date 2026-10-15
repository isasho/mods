using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 血之代价 — 1费：造成已损失生命值 ÷ 3（升级后 ÷ 2）的伤害。消耗。
    public sealed class Blood_Price : CardModel
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        protected override List<DynamicVar> CanonicalVars => [
            new CalculationBaseVar(0m),
            new ExtraDamageVar(1m),
            new CalculatedDamageVar(ValueProp.Move)
                .WithMultiplier((card, _) =>
                {
                    var creature = card.Owner.Creature;
                    int divisor = card.IsUpgraded ? 2 : 3;
                    return (decimal)((creature.MaxHp - creature.CurrentHp) / divisor);
                }),
        ];

        public Blood_Price()
            : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int missingHp = Owner.Creature.MaxHp - Owner.Creature.CurrentHp;
            int divisor = IsUpgraded ? 2 : 3;
            int damage = missingHp / divisor;
            if (damage <= 0) return;

            await DamageCmd.Attack(damage)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_bloody_impact")
                .Execute(choiceContext);
        }
    }
}
