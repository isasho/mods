using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Core
{
    // 流血管理器 — 获取流血层数、引爆流血。
    public static class BleedManager
    {
        public static int GetBleed(Creature target)
        {
            if (target == null)
                return 0;
            return target.GetPowerAmount<BleedPower>();
        }

        // 引爆：消耗目标全部流血层数，造成 层数 × multiplier 真实伤害。
        public static async Task Detonate(
            PlayerChoiceContext choiceContext,
            Creature target,
            decimal multiplier,
            CardModel? source,
            Creature? dealer = null)
        {
            int bleed = GetBleed(target);
            if (bleed <= 0)
                return;

            await PowerCmd.Remove<BleedPower>(target);

            var attacker = dealer ?? source?.Owner?.Creature;
            await CreatureCmd.Damage(
                choiceContext,
                target,
                (decimal)bleed * multiplier,
                ValueProp.Unblockable | ValueProp.Move,
                attacker);

            VfxCmd.PlayOnCreature(target, "vfx/vfx_bloody_impact");
        }
    }
}
