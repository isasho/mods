using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace sts2mod.Powers
{
    // 暴击系统标记 — Buff：仅作 UI 显示，实际暴击判定与放大由 CritPatch 统一处理。
    public sealed class CritSystemPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.None;

        protected override bool IsVisibleInternal => true;
    }
}
