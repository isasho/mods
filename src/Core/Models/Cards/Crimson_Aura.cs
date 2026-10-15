using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 猩红光环 — 2费能力：每次暴击时对目标施加8层流血。升级后12层。
    public sealed class Crimson_Aura : CardModel
    {
        public Crimson_Aura()
            : base(2, CardType.Power, CardRarity.Uncommon, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<CrimsonAuraPower>(choiceContext, 
                Owner.Creature, IsUpgraded ? 12 : 8, Owner.Creature, this, false);
        }
    }
}
