using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Powers;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 生命转换 — 1费：失去4点生命，获得2点力量。若你有痛楚，改为3点。消耗。升级后不消耗。
    public sealed class Life_Conversion : CardModel
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            IsUpgraded ? [] : [CardKeyword.Exhaust];

        public Life_Conversion()
            : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None) { }

        protected override void OnUpgrade()
        {
            RemoveKeyword(CardKeyword.Exhaust);
        }

        protected override bool ShouldGlowGoldInternal
        {
            get
            {
                return Owner?.Creature?.GetPower<CrimsonRagePower>() != null;
            }
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.Damage(choiceContext, Owner.Creature, 4,
                ValueProp.Unblockable | ValueProp.Unpowered, this);

            int str = Owner.Creature.GetPower<CrimsonRagePower>() != null ? 3 : 2;
            await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, str, Owner.Creature, this, false);
        }
    }
}
