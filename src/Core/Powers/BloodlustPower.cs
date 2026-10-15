using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using System.Threading.Tasks;

namespace sts2mod.Powers
{
    // 噬血本能 — Buff：每次施加流血时获得2点格挡。
    public sealed class BloodlustPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;


        public override async Task AfterPowerAmountChanged(
            PlayerChoiceContext choiceContext,
            PowerModel power,
            decimal amountChange,
            Creature creature,
            CardModel cardSource)
        {
            if (power is not BleedPower)
                return;
            if (amountChange <= 0)
                return;

            await CreatureCmd.GainBlock(base.Owner, base.Amount, ValueProp.Move, null);
        }
    }
}
