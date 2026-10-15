using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Core;

using System.Threading.Tasks;

namespace sts2mod.Powers
{
    // 绯色光轮 — Buff：暴击时对目标施加8(12)层流血。
    public sealed class CrimsonAuraPower : PowerModel
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

            int bleed = (int)base.Amount;
            await PowerCmd.Apply<BleedPower>(choiceContext, target, bleed, base.Owner, null, false);
        }
    }
}
