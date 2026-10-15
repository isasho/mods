using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using System.Threading.Tasks;

namespace sts2mod.Powers
{
    // 歃血盟约 — Buff：每次造成未被格挡的攻击伤害时回复生命。
    public sealed class BloodPactPower : PowerModel
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
            if (result.UnblockedDamage <= 0)
                return;
            if (base.Owner.IsDead)
                return;
            if (base.Amount <= 0)
                return;

            await CreatureCmd.Heal(base.Owner, base.Amount);
        }
    }
}
