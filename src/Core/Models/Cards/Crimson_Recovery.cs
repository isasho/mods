using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 猩红恢复 — 1费：回复5生命，获得1层痛楚。升级后+2回复+1痛楚。
    public sealed class Crimson_Recovery : CardModel
    {
        public Crimson_Recovery()
            : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int heal = IsUpgraded ? 7 : 5;
            await CreatureCmd.Heal(Owner.Creature, heal);

            int rage = IsUpgraded ? 2 : 1;
            await PowerCmd.Apply<CrimsonRagePower>(choiceContext, Owner.Creature, rage, Owner.Creature, this, false);
        }
    }
}
