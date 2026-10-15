using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Powers
{
    // 鲜血王座 — Buff：每当你对敌人施加流血时，额外施加3(5)层。
    public sealed class BloodThronePower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;


        private bool _applyingBonus;

        public override async Task AfterPowerAmountChanged(
            PlayerChoiceContext choiceContext,
            PowerModel power,
            decimal amountChange,
            Creature applier,
            CardModel cardSource)
        {
            if (power is not BleedPower)
                return;
            if (amountChange <= 0)
                return;
            if (_applyingBonus)
                return;

            int extra = (int)base.Amount;
            _applyingBonus = true;
            await PowerCmd.Apply<BleedPower>(choiceContext, power.Owner, extra, base.Owner, null, false);
            _applyingBonus = false;
        }
    }
}
