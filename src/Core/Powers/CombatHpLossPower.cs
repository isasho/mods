using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using System.Threading.Tasks;

namespace sts2mod.Powers
{
    // 累计失去生命值追踪 — 隐藏 Buff，Counter 类型：每次失去生命（未格挡部分）累加层数。
    // 血债清算读取此层数作为伤害。初始挂载 1 作为激活标记（框架不挂载 amount=0 的 power），读取时减 1。
    public sealed class CombatHpLossPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        protected override bool IsVisibleInternal => false;

        public override async Task AfterDamageReceived(
            PlayerChoiceContext choiceContext,
            Creature target,
            DamageResult result,
            ValueProp props,
            Creature? dealer,
            CardModel? cardSource)
        {
            if (target != base.Owner)
                return;

            int hpLost = result.UnblockedDamage;
            if (hpLost > 0)
                await PowerCmd.Apply<CombatHpLossPower>(choiceContext,
                    base.Owner, hpLost, base.Owner, null, false);
        }
    }
}
