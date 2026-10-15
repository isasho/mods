using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using System.Threading.Tasks;

namespace sts2mod.Powers
{
	// 血宴不息 — Buff：每回合开始时回复生命。升级后回复更多。
	public sealed class BloodFeastPower : PowerModel
	{
		public override PowerType Type => PowerType.Buff;

		public override PowerStackType StackType => PowerStackType.Counter;


		public override async Task AfterPlayerTurnStart(
			PlayerChoiceContext choiceContext,
			Player player)
		{
			if (player.Creature != base.Owner)
				return;

			await CreatureCmd.Heal(base.Owner, base.Amount);
		}
	}
}
