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
    // 鲜血契约 — 0费：失去10生命，抽5张牌，获得3点能量。消耗。升级后失去7生命。
    public sealed class Blood_Contract : CardModel
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        protected override List<DynamicVar> CanonicalVars => [
            new EnergyVar(3)
        ];

        public Blood_Contract()
            : base(
                0,
                CardType.Skill,
                CardRarity.Rare,
                TargetType.None)
        {
        }

        protected override async Task OnPlay(
            PlayerChoiceContext choiceContext,
            CardPlay cardPlay)
        {
            int hpLoss = IsUpgraded ? 7 : 10;
            await CreatureCmd.Damage(choiceContext, Owner.Creature, hpLoss,
                ValueProp.Unblockable | ValueProp.Unpowered, this);

            await CardPileCmd.Draw(choiceContext, 5, Owner);

            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        }
    }
}
