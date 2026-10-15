using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace sts2mod.Powers
{
    // 祭坛暴击标记：献祭祭坛提供的基础暴击率，不在 UI 显示。
    public sealed class SacrificialAltarCritPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.None;

        protected override bool IsVisibleInternal => false;
    }
}
