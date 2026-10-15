using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 渴血之欲 — 2费能力：流血效果额外触发一次。升级后耗能-1。
    public sealed class Blood_Desire : CardModel
    {
        public Blood_Desire()
            : base(
                2,
                CardType.Power,
                CardRarity.Rare,
                TargetType.None)
        {
        }

        protected override async Task OnPlay(
            PlayerChoiceContext choiceContext,
            CardPlay cardPlay)
        {
            await PowerCmd.Apply<BloodDesirePower>(choiceContext, 
                Owner.Creature, 1, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            base.EnergyCost.UpgradeBy(-1);
        }
    }
}
