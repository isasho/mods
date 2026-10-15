using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 无尽狂怒 — 2费能力：每回合开始获得1层痛楚。升级后获得2层。
    public sealed class Endless_Rage : CardModel
    {
        public Endless_Rage()
            : base(2, CardType.Power, CardRarity.Uncommon, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<EndlessRagePower>(choiceContext, Owner.Creature, IsUpgraded ? 2 : 1, Owner.Creature, this, false);
        }
    }
}
