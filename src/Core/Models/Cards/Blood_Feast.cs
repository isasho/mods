using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 鲜血盛宴 — 2费能力：每回合开始回复3生命。升级后回复5生命。
    public sealed class Blood_Feast : CardModel
    {
        public Blood_Feast()
            : base(2, CardType.Power, CardRarity.Uncommon, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<BloodFeastPower>(choiceContext, 
                Owner.Creature, IsUpgraded ? 5 : 3, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            base.EnergyCost.UpgradeBy(-1);
        }
    }
}
