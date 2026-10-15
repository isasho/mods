using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Core;
using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 血涌 — 1费：本回合+50%暴击率，抽1张牌。升级后抽2张牌。
    public sealed class Blood_Rush : CardModel
    {
        public Blood_Rush()
            : base(1, CardType.Skill, CardRarity.Common, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<BloodRushPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this, false);
            var power = Owner.Creature.GetPower<BloodRushPower>();
            if (power != null)
                power.BonusCritRate = 50;

            int draw = IsUpgraded ? 2 : 1;
            await CardPileCmd.Draw(choiceContext, draw, Owner);

            await CritManager.RefreshCritDisplay(Owner.Creature);
        }
    }
}
