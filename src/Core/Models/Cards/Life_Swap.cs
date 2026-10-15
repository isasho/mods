using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 以命换命 — 2费：失去6点生命，目标敌人失去15点生命（无视格挡）。升级后失去4点，敌人失去20点。
    public sealed class Life_Swap : CardModel
    {
        public Life_Swap()
            : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var target = cardPlay.Target;
            if (target == null) return;

            int selfDmg = IsUpgraded ? 4 : 6;
            int enemyDmg = IsUpgraded ? 20 : 15;

            await CreatureCmd.Damage(choiceContext, Owner.Creature, selfDmg,
                ValueProp.Unblockable | ValueProp.Unpowered, this);

            await CreatureCmd.Damage(choiceContext, target, enemyDmg,
                ValueProp.Unblockable | ValueProp.Unpowered, this);
        }
    }
}
