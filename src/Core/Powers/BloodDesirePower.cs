using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace sts2mod.Powers
{
    // 渴血之欲 — 正面能力：使流血额外触发。本身不执行逻辑，由 BleedPower 检测并重触发自身。
    // 参考官方 AccelerantPower：能力本身只是标记，由 PoisonPower 检测对手身上的层数决定触发次数。
    public sealed class BloodDesirePower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;
    }
}
