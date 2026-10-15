using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Core;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
	// 屠杀盛宴 — 2费技能：引爆所有敌人流血×2并清空。升级后×3。
	public sealed class Slaughter_Feast : CardModel
	{
		public Slaughter_Feast()
			: base(2, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies) { }

		protected override async Task OnPlay(
			PlayerChoiceContext choiceContext,
			CardPlay cardPlay)
		{
			var combat = cardPlay.Card?.CombatState;
			if (combat == null)
				return;

			var enemies = new List<Creature>(combat.Enemies);

			decimal multi = IsUpgraded ? 3m : 2m;
			foreach (var enemy in enemies)
			{
				if (enemy == null)
					continue;
				await BleedManager.Detonate(choiceContext, enemy, multi, this);
			}
		}

	}
}
