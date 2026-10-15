using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 血之契约 — 2费能力：每次造成攻击伤害时回复2生命。升级后回复3。
    public sealed class Blood_Pact : CardModel
    {
        public Blood_Pact()
            : base(2, CardType.Power, CardRarity.Uncommon, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int healAmount = IsUpgraded ? 3 : 2;
            await PowerCmd.Apply<BloodPactPower>(choiceContext, Owner.Creature, healAmount, Owner.Creature, this, false);
        }
    }
}
