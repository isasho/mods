using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 浴血 — 3费能力：回合开始时，若本场战斗已失去生命，回复已失生命的20%。升级后回复25%。
    public sealed class Blood_Bath : CardModel
    {
        public Blood_Bath()
            : base(3, CardType.Power, CardRarity.Rare, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int pct = IsUpgraded ? 25 : 20;
            await PowerCmd.Apply<BloodBathPower>(choiceContext, Owner.Creature, pct, Owner.Creature, this, false);
        }
    }
}
