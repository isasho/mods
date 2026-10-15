using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Powers
{
    // 痛楚催化 — Buff：本回合中获得的痛楚层数翻倍（倍率 = 1 + 层数）。回合结束时移除。
    public sealed class CrimsonCatalystPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        private bool _applyingBonus;

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new DynamicVar("Multiplier", 2m)
        ];

        // 刷新描述变量（倍率 = 1 + 层数），使其随叠加实时更新。
        public void RefreshDescription()
        {
            base.DynamicVars["Multiplier"].BaseValue = 1m + (decimal)base.Amount;
        }

        public override async Task AfterPowerAmountChanged(
            PlayerChoiceContext choiceContext,
            PowerModel power,
            decimal amountChange,
            Creature creature,
            CardModel cardSource)
        {
            if (power is not CrimsonRagePower)
                return;
            if (amountChange <= 0)
                return;
            if (_applyingBonus)
                return;

            _applyingBonus = true;
            await PowerCmd.Apply<CrimsonRagePower>(choiceContext, creature, (int)amountChange * (int)base.Amount, base.Owner, null, false);
            _applyingBonus = false;
        }

        public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> creatures)
        {
            if (side != base.Owner.Side)
                return;
            await PowerCmd.Remove(this);
        }
    }
}
