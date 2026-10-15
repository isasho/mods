using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 鲜血王座 — 1费能力：每当你对敌人施加流血时，额外施加3(5)层。
    public sealed class Blood_Throne : CardModel
    {
        public Blood_Throne()
            : base(1, CardType.Power, CardRarity.Rare, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<BloodThronePower>(choiceContext, 
                Owner.Creature, IsUpgraded ? 5 : 3, Owner.Creature, this, false);
        }
    }
}
