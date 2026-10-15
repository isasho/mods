using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace sts2mod.Powers
{
	// 流血 — 负面能力：宿主阵营回合开始时受到等于层数的不可减免伤害，随后层数减半。
	// 渴血之欲（BloodDesirePower）会使其额外触发。可被引爆类卡牌一次性清算全部层数造成爆发伤害。
	public sealed class BleedPower : PowerModel
	{
		public override PowerType Type => PowerType.Debuff;

		public override PowerStackType StackType => PowerStackType.Counter;

		protected override IEnumerable<DynamicVar> CanonicalVars => [
			new DynamicVar("Bleed", 0m)
		];

		// 触发次数 = 1 + 对手身上的渴血之欲层数，但不超过流血层数。
		private int TriggerCount
		{
			get
			{
				IEnumerable<Creature> source = from c in base.Owner.CombatState.GetOpponentsOf(base.Owner)
					where c.IsAlive
					select c;
				return Math.Min(base.Amount, 1 + source.Sum((Creature a) => a.GetPowerAmount<BloodDesirePower>()));
			}
		}

		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> creatures, ICombatState combatState)
		{
			if (side != base.Owner.Side)
				return;

			int iterations = TriggerCount;
			for (int i = 0; i < iterations; i++)
			{
				if (!base.Owner.IsAlive)
					break;

				int amount = base.Amount;
				if (amount <= 0)
					break;

				await CreatureCmd.Damage(
					new ThrowingPlayerChoiceContext(),
					base.Owner,
					amount,
					ValueProp.Unblockable | ValueProp.Unpowered,
					null, null);

				if (!base.Owner.IsAlive)
					break;

				int newAmount = amount / 2;
				int delta = newAmount - amount;
				if (delta < 0)
					await PowerCmd.ModifyAmount(new ThrowingPlayerChoiceContext(), this, delta, null, null, false);
			}
		}
	}
}
