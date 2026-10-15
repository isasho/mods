using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Core;

using System;
using System.Threading.Tasks;

namespace sts2mod.Powers
{
    // 痛楚 — Buff（可叠加）：每层+5%暴击率。暴击时额外造成等同于层数的伤害，消耗1层。击杀敌人时回复等同于剩余层数的生命，获得1层。
    public sealed class CrimsonRagePower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        [NonSerialized]
        private bool _busy;

        public override async Task AfterPowerAmountChanged(
            PlayerChoiceContext choiceContext,
            PowerModel power,
            decimal amountChange,
            Creature applier,
            CardModel cardSource)
        {
            if (power is not CrimsonRagePower || power.Owner != base.Owner)
                return;
            await CritManager.RefreshCritDisplay(base.Owner);
        }

        public override async Task AfterDamageGiven(
            PlayerChoiceContext choiceContext,
            Creature? dealer,
            DamageResult result,
            ValueProp props,
            Creature target,
            CardModel? cardSource)
        {
            if (_busy || dealer != base.Owner || !props.IsPoweredAttack() || base.Amount <= 0)
                return;

            int stacks = (int)base.Amount;
            bool isCrit = CritManager.PeekCritFlag(dealer);

            if (isCrit && result.UnblockedDamage > 0)
            {
                _busy = true;
                await CreatureCmd.Damage(
                    choiceContext,
                    target,
                    stacks,
                    ValueProp.Unblockable | ValueProp.Unpowered,
                    dealer,
                    null);

                await PowerCmd.ModifyAmount(choiceContext, this, -1, dealer, null, false);
                _busy = false;
            }

            if (target.IsDead)
            {
                int remaining = (int)base.Amount;
                if (remaining > 0)
                    await CreatureCmd.Heal(base.Owner, remaining);

                await PowerCmd.Apply<CrimsonRagePower>(choiceContext, 
                    base.Owner, 1, base.Owner, null, false);
            }
        }
    }
}
