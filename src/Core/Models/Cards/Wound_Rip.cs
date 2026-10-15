using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Core;
using sts2mod.Powers;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 割裂伤口 — 1费：造成目标流血层数的伤害，再施加4层流血。升级后+3流血。
    public sealed class Wound_Rip : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new BleedVar(4m)
        ];

        public Wound_Rip()
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
                await DamageCmd.Attack(bleed)
                    .FromCard(this).Targeting(cardPlay.Target)
                    .WithHitFx("vfx/vfx_bloody_impact")
                    .Execute(choiceContext);
            }

            int newBleed = (int)DynamicVars["Bleed"].BaseValue;
            await PowerCmd.Apply<BleedPower>(choiceContext, cardPlay.Target, newBleed, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Bleed"].UpgradeValueBy(3m);
        }
    }
}
