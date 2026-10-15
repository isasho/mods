using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Core;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 血神裁决 — 2费：引爆目标流血，造成层数×4伤害并清空流血。升级后×5。
    public sealed class Blood_God_Judgment : CardModel
    {
        public Blood_God_Judgment()
            : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(
            PlayerChoiceContext choiceContext,
            CardPlay cardPlay)
        {
            decimal multi = IsUpgraded ? 5m : 4m;
            await BleedManager.Detonate(choiceContext, cardPlay.Target, multi, this);
        }
    }
}
