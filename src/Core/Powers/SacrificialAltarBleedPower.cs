using System.Threading.Tasks;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace sts2mod.Powers
{
    // 祭坛放血 — 献祭祭坛提供的隐形 Buff：攻击命中时对目标施加等同于层数的流血。
    public sealed class SacrificialAltarBleedPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.None;

        protected override bool IsVisibleInternal => false;

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

            int bleed = (int)base.Amount;
            await PowerCmd.Apply<BleedPower>(choiceContext, target, bleed, base.Owner, null, false);
        }
    }
}
