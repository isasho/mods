using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Powers;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 献祭 — 0费：失去4点生命。获得1点能量和3层痛楚。消耗。升级后不消耗。
    public sealed class Blood_Offering : CardModel
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            IsUpgraded ? [] : [CardKeyword.Exhaust];

        protected override List<DynamicVar> CanonicalVars => [
            new EnergyVar(1)
        ];

        public Blood_Offering()
            : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.Damage(choiceContext, Owner.Creature, 4,
                ValueProp.Unblockable | ValueProp.Unpowered, this);

            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
            await PowerCmd.Apply<CrimsonRagePower>(choiceContext, Owner.Creature, 3, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            RemoveKeyword(CardKeyword.Exhaust);
        }
    }
}
