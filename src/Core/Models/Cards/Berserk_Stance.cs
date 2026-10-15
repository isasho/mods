using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 狂暴姿态 — 1费：获得2力量，失去2敏捷。升级后+3力量。
    public sealed class Berserk_Stance : CardModel
    {
        public Berserk_Stance()
            : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int str = IsUpgraded ? 3 : 2;
            await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, str, Owner.Creature, this, false);
            await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, -2, Owner.Creature, this, false);
        }
    }
}
