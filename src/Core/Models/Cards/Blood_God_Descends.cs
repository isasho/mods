using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Cards
{
	// 血神降世 — 3费能力：每回合开始失去生命，获得力量+敏捷（敏捷=力量-1）。多张同名牌效果叠加。
	public sealed class Blood_God_Descends : CardModel
	{
		public Blood_God_Descends()
			: base(
				3,
				CardType.Power,
				CardRarity.Rare,
				TargetType.None)
		{
		}

		protected override async Task OnPlay(
			PlayerChoiceContext choiceContext,
			CardPlay cardPlay)
		{
			int strength = IsUpgraded ? 3 : 2;
			await PowerCmd.Apply<BloodGodPower>(choiceContext,
				Owner.Creature,
				strength,
				Owner.Creature,
				this, false);

			var power = Owner.Creature.GetPower<BloodGodPower>();
			if (power != null)
			{
				power.StrengthPerCopy = strength;
				power.RefreshDescription();
			}
		}
	}
}
