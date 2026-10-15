using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Core;
using sts2mod.Powers;

namespace sts2mod.Potions
{
    // 血爆药剂（罕见，仅战斗）— 引爆所有敌人流血，每层×2伤害并清空。
    public sealed class BloodBurstPotion : PotionModel
    {
        public override PotionRarity Rarity => PotionRarity.Uncommon;
        public override PotionUsage Usage => PotionUsage.CombatOnly;
        public override TargetType TargetType => TargetType.Self;

        protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
        {
            var enemies = base.Owner.Creature.CombatState.Enemies
                .Where(e => e.IsAlive && e.GetPowerAmount<BleedPower>() > 0)
                .ToList();
            foreach (var enemy in enemies)
            {
                await BleedManager.Detonate(choiceContext, enemy, 2m, null, base.Owner.Creature);
            }
        }
    }
}
