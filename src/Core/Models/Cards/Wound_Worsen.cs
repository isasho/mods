using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Core;
using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 伤口恶化 — 1费：目标流血层数翻倍。升级后耗能-1。
    public sealed class Wound_Worsen : CardModel
    {
        public Wound_Worsen()
            : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        protected override bool ShouldGlowGoldInternal
        {
            get
            {
                if (CombatState == null) return false;
                foreach (var enemy in CombatState.Enemies)
                    if (enemy.GetPower<BleedPower>() != null)
                        return true;
                return false;
            }
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int bleed = BleedManager.GetBleed(cardPlay.Target);
            if (bleed > 0)
            {
                await PowerCmd.Apply<BleedPower>(choiceContext, 
                    cardPlay.Target, bleed, Owner.Creature, this, false);
            }
        }

        protected override void OnUpgrade()
        {
            base.EnergyCost.UpgradeBy(-1);
        }
    }
}
