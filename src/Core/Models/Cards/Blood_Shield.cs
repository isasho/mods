using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 鲜血护盾 — 1费：失去3点生命，获得等同于已损失生命值50%的格挡。升级后75%。
    public sealed class Blood_Shield : CardModel
    {
        public Blood_Shield()
            : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var creature = Owner.Creature;

            await CreatureCmd.Damage(choiceContext, creature, 3,
                ValueProp.Unblockable | ValueProp.Unpowered, this);

            decimal missingHp = creature.MaxHp - creature.CurrentHp;
            decimal ratio = IsUpgraded ? 0.75m : 0.5m;
            decimal block = decimal.Floor(missingHp * ratio);

            if (block > 0)
                await CreatureCmd.GainBlock(creature, block, ValueProp.Move, cardPlay);
        }
    }
}
