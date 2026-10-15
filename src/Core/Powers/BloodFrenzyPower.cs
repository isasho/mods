using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Core;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Powers
{
    // 嗜血狂乱 — 正面能力：暴击率翻倍（每层 ×2，即 2^层数）。回合结束时移除（升级后永久）。
    public sealed class BloodFrenzyPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public bool IsPermanent { get; set; }

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new DynamicVar("Multiplier", 2m)
        ];

        // 刷新描述变量（倍率 = 2^层数），使其随叠加实时更新。
        public void RefreshDescription()
        {
            int amount = Math.Max(0, (int)base.Amount);
            base.DynamicVars["Multiplier"].BaseValue = (decimal)Math.Pow(2, amount);
        }

        public override async Task AfterPowerAmountChanged(
            PlayerChoiceContext choiceContext,
            PowerModel power,
            decimal amountChange,
            Creature applier,
            CardModel cardSource)
        {
            if (power is not BloodFrenzyPower || power.Owner != base.Owner)
                return;
            await CritManager.RefreshCritDisplay(base.Owner);
        }

        public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> creatures)
        {
            if (IsPermanent)
                return;
            if (side != base.Owner.Side)
                return;
            await PowerCmd.Remove(this);
            await CritManager.RefreshCritDisplay(base.Owner);
        }
    }
}
