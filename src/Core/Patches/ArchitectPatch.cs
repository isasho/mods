using HarmonyLib;

using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;

using sts2mod.Characters;

namespace sts2mod.Patches
{
    // 建筑师结局对话补丁 — 给绯血者（Bloodbinder）注册专属对话结构。
    // 对话内容在 localization/zhs/ancients.json（THE_ARCHITECT.talk.BLOODBINDER.*）。
    public static class ArchitectPatch
    {
        public static void Apply(Harmony harmony)
        {
            var defineMethod = AccessTools.Method(typeof(TheArchitect), "DefineDialogues");
            if (defineMethod != null)
                harmony.Patch(defineMethod, postfix: new HarmonyMethod(typeof(ArchitectPatch), nameof(DefineDialogues_Postfix)));
        }

        public static void DefineDialogues_Postfix(ref AncientDialogueSet __result)
        {
            var bloodbinder = ModelDb.Character<Bloodbinder>();
            if (bloodbinder == null)
                return;

            __result.CharacterDialogues[bloodbinder.Id.Entry] = new AncientDialogue[]
            {
                new AncientDialogue("", "")
                {
                    VisitIndex = 0,
                    EndAttackers = ArchitectAttackers.Both
                },
                new AncientDialogue("", "")
                {
                    VisitIndex = 1,
                    EndAttackers = ArchitectAttackers.Both
                },
                new AncientDialogue("", "")
                {
                    VisitIndex = 2,
                    EndAttackers = ArchitectAttackers.Both
                },
                new AncientDialogue("", "", "")
                {
                    VisitIndex = 3,
                    EndAttackers = ArchitectAttackers.Both
                },
            };
        }
    }
}
