using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace sts2mod.Powers
{
    // 血坠暴击标记：血坠内部用，不在UI显示。
    public sealed class BloodPendantCritPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.None;

        protected override bool IsVisibleInternal => false;
    }
}
