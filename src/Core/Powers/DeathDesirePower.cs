using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

using System;
using System.Collections.Generic;

namespace sts2mod.Powers
{
    // 死之祈愿 — Buff：生命值低于50%时，攻击伤害翻倍（每层 ×2，即 2^层数）。Amount 表示翻倍次数（可叠加）。
    // 实际伤害翻倍由 CritPatch 统一处理。
    public sealed class DeathDesirePower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new DynamicVar("Multiplier", 2m)
        ];

        // 刷新描述变量（倍率 = 2^层数），使其随叠加实时更新。
        public void RefreshDescription()
        {
            int amount = Math.Max(0, (int)base.Amount);
            base.DynamicVars["Multiplier"].BaseValue = (decimal)Math.Pow(2, amount);
        }
    }
}
