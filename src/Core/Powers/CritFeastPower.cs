using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Core;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Powers
{
    // 暴击盛宴 — Buff：本回合中每次暴击获得1点力量。回合结束时移除。
    public sealed class CritFeastPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterDamageGiven(
            PlayerChoiceContext choiceContext,
            Creature? dealer,
            DamageResult result,
            ValueProp props,
            Creature target,
            CardModel? cardSource)
        {
            if (dealer != base.Owner)
                return;
            if (!props.IsPoweredAttack())
                return;
            if (!CritManager.ConsumeCritFlag(base.Owner))
                return;

            await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, (int)base.Amount, base.Owner, null, false);
        }

        public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> creatures)
        {
            if (side != base.Owner.Side)
                return;
            await PowerCmd.Remove(this);
        }
    }
}
