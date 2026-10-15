using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 猩红永恒 — 2费能力：受到致命伤害时阻止死亡并恢复50%最大生命，仅触发一次。升级后耗能-1。
    public sealed class Crimson_Eternity : CardModel
    {
        public Crimson_Eternity()
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
            await PowerCmd.Apply<CrimsonEternityPower>(choiceContext, 
                Owner.Creature, 1, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            base.EnergyCost.UpgradeBy(-1);
        }
    }
}
