using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Powers;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
	// 暴虐 — 2费：造成15点伤害。目标流血0-10层暴击，11-20层2倍，21+层3倍。升级后+5伤害。
	public sealed class Brutality : CardModel
	{
		protected override List<DynamicVar> CanonicalVars => [
			new DamageVar(15m, ValueProp.Move)
		];

		public Brutality()
			: base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

		protected override bool ShouldGlowGoldInternal
		{
			get
			{
				if (CombatState == null) return false;
				foreach (var enemy in CombatState.Enemies)
					if (enemy.GetPower<BleedPower>() != null)
						return true;
				return false;
			}
		}

		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			var target = cardPlay.Target;
			if (target == null) return;

			decimal damage = DynamicVars.Damage.BaseValue;
			var bleed = target.GetPower<BleedPower>();
			int bleedStacks = bleed != null ? (int)bleed.Amount : 0;

			decimal multiplier = bleedStacks switch
			{
				>= 21 => 3m,
				>= 11 => 2m,
				_     => 1.5m,
			};

			await DamageCmd.Attack(damage * multiplier)
				.FromCard(this).Targeting(target)
				.WithHitFx("vfx/vfx_bloody_impact")
				.Execute(choiceContext);
		}

		protected override void OnUpgrade()
		{
			DynamicVars.Damage.UpgradeValueBy(5m);
		}
	}
}
