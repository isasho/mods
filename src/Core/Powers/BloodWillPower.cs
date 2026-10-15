using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

using System.Threading.Tasks;

namespace sts2mod.Powers
{
    // 血之意志 — Buff：每次因卡牌失去生命时获得1点力量。
    public sealed class BloodWillPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterDamageReceived(
            PlayerChoiceContext choiceContext,
            Creature target,
            DamageResult result,
            ValueProp props,
            Creature? dealer,
            CardModel? cardSource)
        {
            if (target != base.Owner)
                return;
            if (cardSource == null)
                return;
            if (result.UnblockedDamage <= 0)
                return;

            await PowerCmd.Apply<StrengthPower>(choiceContext, 
                base.Owner, (int)base.Amount, base.Owner, null, false);
        }
    }
}
