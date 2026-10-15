using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Core;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Powers
{
    // 血涌奔流 — Buff：临时增加暴击率，回合结束时移除。
    public sealed class BloodRushPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.None;

        public int BonusCritRate { get; set; } = 25;

        public override async Task AfterPowerAmountChanged(
            PlayerChoiceContext choiceContext,
            PowerModel power,
            decimal amountChange,
            Creature applier,
            CardModel cardSource)
        {
            if (power is not BloodRushPower || power.Owner != base.Owner)
                return;
            await CritManager.RefreshCritDisplay(base.Owner);
        }

        public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> creatures)
        {
            if (side != base.Owner.Side)
                return;
            await PowerCmd.Remove(this);
            await CritManager.RefreshCritDisplay(base.Owner);
        }
    }
}
