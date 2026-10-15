using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 死愿 — 2费能力：生命值低于50%时，所有攻击伤害翻倍。升级后耗能-1。
    public sealed class Death_Wish : CardModel
    {
        public Death_Wish()
            : base(2, CardType.Power, CardRarity.Rare, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<DeathDesirePower>(choiceContext,
                Owner.Creature, 1, Owner.Creature, this, false);

            var power = Owner.Creature.GetPower<DeathDesirePower>();
            if (power != null)
                power.RefreshDescription();
        }

        protected override void OnUpgrade()
        {
            base.EnergyCost.UpgradeBy(-1);
        }
    }
}
