using System;
using System.Collections.Generic;
using System.Linq;

using HarmonyLib;

using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

using sts2mod.Powers;

namespace sts2mod.Patches
{
    // 卡面黄色词条悬停提示 — 官方角色在每张卡里重写 ExtraHoverTips 挂词条说明，
    // 绯血者卡牌众多，改为统一补丁：解析卡面描述里的 [gold]词条[/gold]，
    // 为出现的每个词条自动追加对应的 HoverTip（流血/痛楚/吸血/力量/格挡/虚弱/易伤/敏捷/能量）。
    public static class CardKeywordHoverTipPatch
    {
        public static void Apply(Harmony harmony)
        {
            var getter = AccessTools.PropertyGetter(typeof(CardModel), nameof(CardModel.HoverTips));
            if (getter != null)
                harmony.Patch(getter, postfix: new HarmonyMethod(typeof(CardKeywordHoverTipPatch), nameof(HoverTips_Postfix)));
        }

        public static void HoverTips_Postfix(CardModel __instance, ref IEnumerable<IHoverTip> __result)
        {
            try
            {
                // 仅处理本模组的卡牌（官方卡已各自挂好词条）。
                if (__instance.GetType().Assembly != typeof(CardKeywordHoverTipPatch).Assembly)
                    return;

                string desc = __instance.Description.GetRawText();
                if (string.IsNullOrEmpty(desc))
                    return;

                var extra = new List<IHoverTip>();
                foreach (var keyword in Keywords)
                {
                    if (desc.Contains("[gold]" + keyword.Title() + "[/gold]"))
                        extra.Add(keyword.Tip());
                }

                if (extra.Count > 0)
                    __result = __result.Concat(extra);
            }
            catch
            {
            }
        }

        private sealed class KeywordEntry
        {
            public Func<string> Title;
            public Func<IHoverTip> Tip;
        }

        private static readonly KeywordEntry[] Keywords =
        {
            Power<BleedPower>(),
            Power<CrimsonRagePower>(),
            Power<LifestealPower>(),
            Power<StrengthPower>(),
            Power<DexterityPower>(),
            Power<WeakPower>(),
            Power<VulnerablePower>(),
            Static(StaticHoverTip.Block, "BLOCK"),
            Static(StaticHoverTip.Energy, "ENERGY"),
        };

        private static KeywordEntry Power<T>() where T : PowerModel
        {
            return new KeywordEntry
            {
                Title = () => ModelDb.Power<T>().Title.GetFormattedText(),
                Tip = () => HoverTipFactory.FromPower<T>()
            };
        }

        private static KeywordEntry Static(StaticHoverTip tip, string key)
        {
            return new KeywordEntry
            {
                Title = () => new LocString("static_hover_tips", key + ".title").GetFormattedText(),
                Tip = () => HoverTipFactory.Static(tip)
            };
        }
    }
}
