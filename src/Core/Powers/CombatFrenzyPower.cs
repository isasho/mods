using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

using System.Threading.Tasks;

namespace sts2mod.Powers
{
    // 战意狂燃 — Buff（可叠加）：每回合开始时获得1点力量。
    public sealed class CombatFrenzyPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterPlayerTurnStart(
            PlayerChoiceContext choiceContext,
            Player player)
        {
            if (player.Creature != base.Owner)
                return;

            await PowerCmd.Apply<StrengthPower>(choiceContext, 
                base.Owner, base.Amount, base.Owner, null, false);
        }
    }
}
