using HarmonyLib;

using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Characters;

namespace sts2mod.Patches
{
    // 先古之民对话补丁 — 给绯血者（Bloodbinder）在原版先古之民处注册专属对话结构。
    // 对话内容在 localization/zhs/ancients.json（{ANCIENT}.talk.BLOODBINDER.*）。
    // 仅为本模组实际提供对话的先古之民注入；非原版（其他 mod）先古之民跳过，保持其自身对话。
    // 注：建筑师（TheArchitect）继承自 EventModel 而非 AncientEventModel，由 ArchitectPatch 单独处理。
    public static class AncientDialoguePatch
    {
        public static void Apply(Harmony harmony)
        {
            var method = AccessTools.PropertyGetter(typeof(AncientEventModel), nameof(AncientEventModel.DialogueSet));
            if (method != null)
                harmony.Patch(method, postfix: new HarmonyMethod(typeof(AncientDialoguePatch), nameof(DialogueSet_Postfix)));
        }

        public static void DialogueSet_Postfix(AncientEventModel __instance, ref AncientDialogueSet __result)
        {
            var bloodbinder = ModelDb.Character<Bloodbinder>();
            if (bloodbinder == null || __result.CharacterDialogues == null)
                return;

            // 仅为本模组实际提供对话的原版先古之民注入；其余（非原版/其他 mod）保持其自身对话。
            string entry = __instance.Id.Entry;
            if (!LocString.Exists("ancients", entry + ".talk.BLOODBINDER.0-0.ancient")
                && !LocString.Exists("ancients", entry + ".talk.BLOODBINDER.0-0.char"))
                return;

            __result.CharacterDialogues[bloodbinder.Id.Entry] = new AncientDialogue[]
            {
                new AncientDialogue("", "") { VisitIndex = 0 },
                new AncientDialogue("", "") { VisitIndex = 1 },
                new AncientDialogue("", "") { VisitIndex = 2 },
            };

            // 重新填充本地化 key（幂等），让新增对话的台词与按钮文字正确解析。
            __result.PopulateLocKeys(__instance.Id.Entry);
        }
    }
}
