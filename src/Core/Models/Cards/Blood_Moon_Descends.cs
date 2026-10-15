using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 血月降临 — 2费能力：每回合开始失去生命并对所有敌人施加流血。多张同名牌效果叠加。
    public sealed class Blood_Moon_Descends : CardModel
    {
        public Blood_Moon_Descends()
            : base(2, CardType.Power, CardRarity.Uncommon, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int bleed = IsUpgraded ? 8 : 5;
            await PowerCmd.Apply<BloodMoonPower>(choiceContext,
                Owner.Creature, bleed, Owner.Creature, this, false);

            var power = Owner.Creature.GetPower<BloodMoonPower>();
            if (power != null)
            {
                power.BleedPerCopy = bleed;
                power.RefreshDescription();
            }
        }
    }
}
