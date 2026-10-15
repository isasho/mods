using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

using System.Threading.Tasks;

namespace sts2mod.Powers
{
    // 绯色永劫 — Buff：受到致命伤害时阻止死亡，恢复50%最大生命，仅触发一次后移除。
    public sealed class CrimsonEternityPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.None;

        public override bool ShouldDie(Creature creature)
        {
            if (creature == base.Owner)
                return false;
            return true;
        }

        public override async Task AfterPreventingDeath(Creature creature)
        {
            if (creature != base.Owner)
                return;

            await CreatureCmd.Heal(
                base.Owner,
                (decimal)base.Owner.MaxHp * 0.5m);

            await PowerCmd.Remove(this);
        }
    }
}
