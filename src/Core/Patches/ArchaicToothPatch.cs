using System.Collections.Generic;

using HarmonyLib;

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

using sts2mod.Cards;

namespace sts2mod.Patches
{
    // 让古老牙齿（ArchaicTooth）也能把血之攻击（Blood_Attack）升华替换为血之裂（Blood_Rift）。
    // 原版映射是私有静态属性（每次访问新建字典），故对 getter 打 postfix 追加绯血者条目。
    public static class ArchaicToothPatch
    {
        public static void Apply(Harmony harmony)
        {
            var method = AccessTools.PropertyGetter(typeof(ArchaicTooth), "TranscendenceUpgrades");
            if (method != null)
            {
                harmony.Patch(method,
                    postfix: new HarmonyMethod(typeof(ArchaicToothPatch), nameof(TranscendenceUpgrades_Postfix)));
            }
        }

        public static void TranscendenceUpgrades_Postfix(Dictionary<ModelId, CardModel> __result)
        {
            __result[ModelDb.Card<Blood_Attack>().Id] = ModelDb.Card<Blood_Rift>();
        }
    }
}
