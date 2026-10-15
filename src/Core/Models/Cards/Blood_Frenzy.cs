using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Core;
using sts2mod.Powers;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 鲜血狂热 — 1费技能牌：获得鲜血狂热——暴击率翻倍，回合结束移除。消耗。升级后变为永久。
    public sealed class Blood_Frenzy : CardModel
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        public Blood_Frenzy()
            : base(
                1,
                CardType.Skill,
                CardRarity.Uncommon,
                TargetType.None)
        {
        }

        protected override async Task OnPlay(
            PlayerChoiceContext choiceContext,
            CardPlay cardPlay)
        {
            await PowerCmd.Apply<BloodFrenzyPower>(choiceContext, 
                Owner.Creature,
                1,
                Owner.Creature,
                this, false);

            var frenzy = Owner.Creature.GetPower<BloodFrenzyPower>();
            if (frenzy != null)
            {
                if (IsUpgraded)
                    frenzy.IsPermanent = true;
                frenzy.RefreshDescription();
            }

            await CritManager.RefreshCritDisplay(Owner.Creature);
        }

    }
}
