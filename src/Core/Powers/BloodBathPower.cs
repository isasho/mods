using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Powers
{
    // 浴血苏生 — Buff：回合开始时，若本场战斗已失去生命，回复已损失生命的百分比。Amount 表示回复百分比（20/25）。
    public sealed class BloodBathPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterPlayerTurnStart(
            PlayerChoiceContext choiceContext,
            Player player)
        {
            if (player.Creature != base.Owner)
                return;

            decimal missingHp = base.Owner.MaxHp - base.Owner.CurrentHp;
            if (missingHp <= 0)
                return;

            decimal pct = base.Amount;
            await CreatureCmd.Heal(base.Owner, missingHp * pct / 100m);
        }
    }
}
