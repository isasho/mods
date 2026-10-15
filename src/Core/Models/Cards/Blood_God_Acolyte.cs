using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 血神信徒 — 2费能力：每回合开始对所有敌人施加2层流血。升级后3层。
    public sealed class Blood_God_Acolyte : CardModel
    {
        public Blood_God_Acolyte()
            : base(2, CardType.Power, CardRarity.Uncommon, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<BloodGodAcolytePower>(choiceContext, 
                Owner.Creature, IsUpgraded ? 3 : 2, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            base.EnergyCost.UpgradeBy(-1);
        }
    }
}
