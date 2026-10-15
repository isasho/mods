using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 战斗狂热 — 1费能力：每回合开始获得1力量，可叠加。升级后耗能为0。
    public sealed class Combat_Frenzy : CardModel
    {
        public Combat_Frenzy()
            : base(1, CardType.Power, CardRarity.Uncommon, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<CombatFrenzyPower>(choiceContext, 
                Owner.Creature, 1, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            base.EnergyCost.UpgradeBy(-1);
        }
    }
}
