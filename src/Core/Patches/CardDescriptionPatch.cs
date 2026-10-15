using System;

using HarmonyLib;

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Core;
using sts2mod.Powers;
using sts2mod.Relics;

namespace sts2mod.Patches
{
    // 卡面动态追加「施加流血」行 — 持有献祭祭坛·流血时，对所有攻击牌在描述末尾追加一行，
    // 显示该攻击对单个目标最终会施加的流血总量（再乘血月碎片倍率）：
    //   (卡牌自身流血×施加次数 + 祭坛流血×伤害段数 + 血冕王座×(施加次数+伤害段数)) × 血月碎片倍率。
    // 其中血冕王座对「每一次」流血施加都额外触发（卡牌与祭坛是两次独立施加），
    // 祭坛流血经 AfterDamageGiven 按「伤害段数」触发，多段攻击需通过 IRepeatAttackInfo 声明段数。
    // 数值随各种加成实时变化；未持有祭坛·流血时不显示，避免与卡牌原有流血描述重复。
    public static class CardDescriptionPatch
    {
        public static void Apply(Harmony harmony)
        {
            // 注意：私有方法 GetDescriptionForPile 的第 2 个参数是 CardModel 私有的嵌套枚举 DescriptionPreviewType，
            // 不是公开的 CardPreviewMode（两者不同，用错会导致反射匹配失败、补丁静默失效）。
            var previewType = AccessTools.Inner(typeof(CardModel), "DescriptionPreviewType");
            var method = previewType == null ? null : AccessTools.Method(
                typeof(CardModel),
                "GetDescriptionForPile",
                new Type[]
                {
                    typeof(PileType),
                    previewType,
                    typeof(Creature)
                });
            if (method != null)
                harmony.Patch(method, postfix: new HarmonyMethod(typeof(CardDescriptionPatch), nameof(Description_Postfix)));
        }

        public static void Description_Postfix(CardModel __instance, ref string __result)
        {
            try
            {
                if (__result == null || __instance.Type != CardType.Attack)
                    return;

                var creature = GetOwnerCreature(__instance);
                if (creature == null)
                    return;

                int altar = (int)creature.GetPowerAmount<SacrificialAltarBleedPower>();
                // 仅在持有献祭祭坛·流血时显示，避免与卡牌原有流血描述重复。
                if (altar <= 0)
                    return;

                int total = ComputeBleedTotal(__instance, creature, altar);
                if (total <= 0)
                    return;

                var line = new LocString("cards", "BLOODBINDER_EXTRA_BLEED");
                line.Add("Bleed", total);
                string text = line.GetFormattedText();
                if (!string.IsNullOrEmpty(text))
                    __result = __result + "\n" + text;
            }
            catch
            {
            }
        }

        // 卡牌自身流血基础层数（不含血冕王座加成）。无 Bleed 变量的攻击牌（如赤击）为 0。
        private static int GetRawCardBleed(CardModel card)
        {
            return card.DynamicVars.ContainsKey("Bleed")
                ? (int)card.DynamicVars["Bleed"].BaseValue
                : 0;
        }

        private static int ComputeBleedTotal(CardModel card, Creature creature, int altar)
        {
            int cardBleed = GetRawCardBleed(card);
            int throne = (int)creature.GetPowerAmount<BloodThronePower>();

            int hits = 1;
            int cardApplies = cardBleed > 0 ? 1 : 0;
            if (card is IRepeatAttackInfo repeat)
            {
                hits = repeat.HitsPerTarget;
                cardApplies = repeat.BleedAppliesPerTarget;
            }

            int total = cardBleed * cardApplies + altar * hits + throne * (cardApplies + hits);

            if (creature.Player?.GetRelic<BloodMoonShard>() != null)
                total *= 2;

            return total;
        }

        private static Creature? GetOwnerCreature(CardModel card)
        {
            if (!card.IsMutable)
                return null;
            return card.Owner?.Creature;
        }
    }
}
