using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 大放血 — 1费：失去8生命，获得3力量和3敏捷。升级后失去5生命。
    public sealed class Major_Bloodletting : CardModel
    {
        public Major_Bloodletting()
            : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int hpLoss = IsUpgraded ? 5 : 8;
            await CreatureCmd.Damage(choiceContext, Owner.Creature, hpLoss,
                ValueProp.Unblockable | ValueProp.Unpowered, this);

            await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, 3, Owner.Creature, this, false);
            await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, 3, Owner.Creature, this, false);
        }
    }
}
