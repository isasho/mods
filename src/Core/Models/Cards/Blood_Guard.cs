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
    // 血之防御 — 1费：获得5点格挡。升级后+3格挡。
    public sealed class Blood_Guard : CardModel
    {
        protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];

        protected override List<DynamicVar> CanonicalVars => [
            new BlockVar(5m, ValueProp.Move)
        ];

        public Blood_Guard()
            : base(
                1,
                CardType.Skill,
                CardRarity.Basic,
                TargetType.Self)
        {
        }

        protected override async Task OnPlay(
            PlayerChoiceContext choiceContext,
            CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(
                base.Owner.Creature,
                base.DynamicVars.Block,
                cardPlay);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(3m);
        }
    }
}
