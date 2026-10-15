using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Powers;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 鲜血回收 — 1费：移除目标所有流血，每层回复1点生命并造成等额生命流失。消耗。升级后每层回复2点，不消耗。
    public sealed class Blood_Recycle : CardModel
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            IsUpgraded ? [] : [CardKeyword.Exhaust];

        public Blood_Recycle()
            : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        protected override void OnUpgrade()
        {
            RemoveKeyword(CardKeyword.Exhaust);
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var target = cardPlay.Target;
            if (target == null) return;

            var bleed = target.GetPower<BleedPower>();
            if (bleed == null) return;

            int healPerStack = IsUpgraded ? 2 : 1;
            int stacks = (int)bleed.Amount;

            await PowerCmd.Remove(bleed);

            int total = stacks * healPerStack;
            await CreatureCmd.Damage(choiceContext, target, total,
                ValueProp.Unblockable | ValueProp.Unpowered, this);
            await CreatureCmd.Heal(Owner.Creature, total);
        }
    }
}
