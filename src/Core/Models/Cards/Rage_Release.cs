using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Powers;

using System.Linq;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 狂怒释放 — 1费：移除所有痛楚层数，对全体敌人每层造成8点伤害。升级后每层12伤。
    public sealed class Rage_Release : CardModel
    {
        public Rage_Release()
            : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies) { }

        protected override bool ShouldGlowGoldInternal
        {
            get
            {
                return Owner?.Creature?.GetPowerAmount<CrimsonRagePower>() > 0;
            }
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int stacks = Owner.Creature.GetPowerAmount<CrimsonRagePower>();
            if (stacks <= 0)
                return;

            await PowerCmd.Remove<CrimsonRagePower>(Owner.Creature);

            decimal dmgPerStack = IsUpgraded ? 12m : 8m;
            decimal damage = stacks * dmgPerStack;

            var combat = cardPlay.Card?.CombatState;
            if (combat == null) return;

            foreach (var enemy in combat.Enemies.ToList())
            {
                await DamageCmd.Attack(damage)
                    .FromCard(this).Targeting(enemy)
                    .WithHitFx("vfx/vfx_bloody_impact")
                    .Execute(choiceContext);
            }
        }
    }
}
