using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 失控 — 1费能力：每回合开始获得2点力量和2层痛楚。失去等同于力量层数的生命。升级后耗能为0。
    public sealed class Loss_of_Control : CardModel
    {
        public Loss_of_Control()
            : base(1, CardType.Power, CardRarity.Uncommon, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<LossOfControlPower>(choiceContext,
                Owner.Creature, 1, Owner.Creature, this, false);

            var power = Owner.Creature.GetPower<LossOfControlPower>();
            if (power != null)
                power.RefreshDescription();
        }

        protected override void OnUpgrade()
        {
            base.EnergyCost.UpgradeBy(-1);
        }
    }
}
