using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Powers;
using sts2mod.Relics;

namespace sts2mod.Core
{
	// 流血动态变量 — 用于卡牌描述中显示流血数值。血冕王座额外施加的层数、血月碎片的翻倍，
	// 都实时反映在卡面（{Bleed:diff()} 绿色高亮）。献祭祭坛·流血由卡面追加行（CardDescriptionPatch）显示总量。
	public sealed class BleedVar : DynamicVar
	{
		public BleedVar(decimal value)
			: base("Bleed", value)
		{
		}

		public override void UpdateCardPreview(
			CardModel card,
			CardPreviewMode previewMode,
			Creature? target,
			bool runGlobalHooks)
		{
			decimal bleed = BaseValue;

			var throne = card.Owner?.Creature?.GetPower<BloodThronePower>();
			if (throne != null)
				bleed += throne.Amount;

			if (card.Owner?.GetRelic<BloodMoonShard>() != null)
				bleed *= 2;

			PreviewValue = bleed;
			EnchantedValue = BaseValue;
		}
	}
}
