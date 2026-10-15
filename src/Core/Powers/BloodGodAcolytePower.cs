using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using System.Threading.Tasks;

namespace sts2mod.Powers
{
    // 血神信众 — Buff：每回合开始对所有敌人施加流血。
    public sealed class BloodGodAcolytePower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterPlayerTurnStart(
            PlayerChoiceContext choiceContext,
            Player player)
        {
            if (player.Creature != base.Owner)
                return;

            int bleed = (int)base.Amount;

            var combat = base.Owner.CombatState;
            if (combat == null)
                return;

            foreach (var enemy in combat.Enemies)
                await PowerCmd.Apply<BleedPower>(choiceContext, enemy, bleed, base.Owner, null, false);
        }
    }
}
