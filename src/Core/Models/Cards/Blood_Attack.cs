using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
	// 血之攻击 — 0费：失去5点生命，对一名敌人造成15点伤害。升级后+5伤害。
	public sealed class Blood_Attack : CardModel
	{
		protected override List<DynamicVar> CanonicalVars => [
			new DamageVar(15m, ValueProp.Move),
			new HpLossVar(5m)
		];

		public Blood_Attack()
			: base(
				0,
				CardType.Attack,
				CardRarity.Basic,
				TargetType.AnyEnemy)
		{
		}

		protected override async Task OnPlay(
			PlayerChoiceContext choiceContext,
			CardPlay cardPlay)
		{
			// 失去5点生命
			await CreatureCmd.Damage(
				choiceContext,
				Owner.Creature,
				DynamicVars.HpLoss.BaseValue,
				ValueProp.Unblockable |
				ValueProp.Unpowered |
				ValueProp.Move,
				this);

			// 对敌人造成15点伤害
			await DamageCmd.Attack(
					DynamicVars.Damage.BaseValue)
				.FromCard(this)
				.Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash")
				.Execute(choiceContext);
		}

		// 升级后 +5伤害
		protected override void OnUpgrade()
		{
			DynamicVars.Damage.UpgradeValueBy(5m);
		}
	}
}
