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
    // 凝血 — 1费：获得8点格挡，回复4点生命。消耗。升级后+4格挡+3回复。
    public sealed class Quick_Clot : CardModel
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        protected override List<DynamicVar> CanonicalVars => [
            new BlockVar(8m, ValueProp.Move)
        ];

        public Quick_Clot()
            : base(1, CardType.Skill, CardRarity.Common, TargetType.Self) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

            int heal = IsUpgraded ? 7 : 4;
            await CreatureCmd.Heal(Owner.Creature, heal);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(4m);
        }
    }
}
