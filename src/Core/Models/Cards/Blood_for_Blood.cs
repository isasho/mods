using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 以血换血 — 2费能力：每回合开始失去2生命，获得1能量。升级后失去1生命。
    public sealed class Blood_for_Blood : CardModel
    {
        public Blood_for_Blood()
            : base(2, CardType.Power, CardRarity.Uncommon, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int hpLoss = IsUpgraded ? 1 : 2;
            await PowerCmd.Apply<BloodForBloodPower>(choiceContext,
                Owner.Creature, hpLoss, Owner.Creature, this, false);

            var power = Owner.Creature.GetPower<BloodForBloodPower>();
            if (power != null)
            {
                power.HpLossPerCopy = hpLoss;
                power.RefreshDescription();
            }
        }
    }
}
