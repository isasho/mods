using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 生命交换 — 2费：恢复至满血，移除全部力量。消耗。升级后失去消耗。
    public sealed class Blood_Life_Exchange : CardModel
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            IsUpgraded ? [] : [CardKeyword.Exhaust];

        public Blood_Life_Exchange()
            : base(
                2,
                CardType.Skill,
                CardRarity.Rare,
                TargetType.Self)
        {
        }

        protected override void OnUpgrade()
        {
            RemoveKeyword(CardKeyword.Exhaust);
        }

        protected override async Task OnPlay(
            PlayerChoiceContext choiceContext,
            CardPlay cardPlay)
        {
            var creature = Owner.Creature;
            decimal missingHp = creature.MaxHp - creature.CurrentHp;

            if (missingHp > 0)
                await CreatureCmd.Heal(creature, missingHp);

            var strength = creature.GetPower<StrengthPower>();
            if (strength != null)
                await PowerCmd.Remove(strength);
        }
    }
}
