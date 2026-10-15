using System.Collections.Generic;
using System.Threading.Tasks;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;

namespace sts2mod.Relics
{
    // 先祖之血（稀有）— 能量上限+1，无法在休息站点回复生命；每场战斗结束后回复10%最大生命。
    public sealed class AncestorsBlood : RelicModel
    {
        public override RelicRarity Rarity => RelicRarity.Rare;

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new HealVar(10m)
        ];

        public override decimal ModifyMaxEnergy(Player player, decimal amount)
        {
            if (player == base.Owner)
                return amount + 1m;
            return amount;
        }

        public override decimal ModifyRestSiteHealAmount(Creature creature, decimal amount)
        {
            if (creature == base.Owner.Creature)
                return 0m;
            return amount;
        }

        public override async Task AfterCombatVictory(CombatRoom _)
        {
            if (!base.Owner.Creature.IsDead)
            {
                Flash();
                await CreatureCmd.Heal(base.Owner.Creature,
                    (decimal)base.Owner.Creature.MaxHp * (base.DynamicVars.Heal.BaseValue / 100m));
            }
        }
    }
}
