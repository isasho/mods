using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 挑衅 — 0费：施加1层易伤和1层虚弱。升级后各+1。
    public sealed class Provoke : CardModel
    {
        public Provoke()
            : base(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int amount = IsUpgraded ? 2 : 1;
            await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, amount, Owner.Creature, this, false);
            await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, amount, Owner.Creature, this, false);
        }
    }
}
