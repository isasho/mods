using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 痛觉强化 — 1费：失去4点生命，获得20%暴击率。升级后35%暴击率。
    public sealed class Pain_Empower : CardModel
    {
        public Pain_Empower()
            : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.Damage(choiceContext, Owner.Creature, 4,
                ValueProp.Unblockable | ValueProp.Unpowered, this);

            int critRate = IsUpgraded ? 35 : 20;
            await PowerCmd.Apply<PainEmpowerPower>(choiceContext, Owner.Creature, critRate, Owner.Creature, this, false);
        }
    }
}
