using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace sts2mod.Powers
{
    // 吸血词条 — 仅作为卡面关键词的悬停说明载体，不实际施加到任何生物。
    // 效果：回复X点生命，并对目标造成等量生命流失。
    public sealed class LifestealPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.None;

        protected override bool IsVisibleInternal => false;
    }
}
