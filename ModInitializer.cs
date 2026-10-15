using System;

using HarmonyLib;

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;

using sts2mod.Patches;

namespace sts2mod
{
	[ModInitializer(nameof(Initialize))]
	public static class ModInitializer
	{
		public static void Initialize()
		{
			var harmony = new Harmony("sts2mod.yourname");
			CharacterPatch.Apply(harmony);
			CritPatch.Apply(harmony);
			PowerIconPreloadPatch.Apply(harmony);
			ArchitectPatch.Apply(harmony);
			AncientDialoguePatch.Apply(harmony);
			CardDescriptionPatch.Apply(harmony);
			CardKeywordHoverTipPatch.Apply(harmony);
			TouchOfOrobasPatch.Apply(harmony);
			ArchaicToothPatch.Apply(harmony);
			harmony.PatchAll();

			Log.Info("绯血者模组加载成功！");
		}
	}
}
