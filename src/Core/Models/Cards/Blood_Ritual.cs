using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Powers;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 鲜血仪式 — 1费：获得5层痛楚。消耗。升级后获得7层。
    public sealed class Blood_Ritual : CardModel
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        public Blood_Ritual()
            : base(1, CardType.Skill, CardRarity.Rare, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int rage = IsUpgraded ? 7 : 5;
            await PowerCmd.Apply<CrimsonRagePower>(choiceContext, Owner.Creature, rage, Owner.Creature, this, false);
        }
    }
}
