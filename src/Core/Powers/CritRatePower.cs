using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Core;

using System.Threading.Tasks;

namespace sts2mod.Powers
{
    // 暴击率显示 — Buff：当前暴击率百分比。每回合开始自动刷新。
    public sealed class CritRatePower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterPlayerTurnStart(
            PlayerChoiceContext choiceContext,
            Player player)
        {
            if (player.Creature != base.Owner)
                return;

            await CritManager.RefreshCritDisplay(base.Owner);
        }
    }
}
