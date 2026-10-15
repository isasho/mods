using System.Collections.Generic;
using System.Linq;

using HarmonyLib;

using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Models;

namespace sts2mod.Patches
{
    // 预加载能力大图标 — 消除战斗中能力图标按需加载导致的卡顿（log 里的 Asset not cached）。
    // 游戏本身也不会预加载全部能力图标（敌人中途新出现的能力会按需加载），所以这里连同游戏能力一起预加载。
    public static class PowerIconPreloadPatch
    {
        public static void Apply(Harmony harmony)
        {
            var method = AccessTools.Method(typeof(PreloadManager), "GetCombatAssetPaths");
            if (method != null)
                harmony.Patch(method, postfix: new HarmonyMethod(typeof(PowerIconPreloadPatch), nameof(GetCombatAssetPaths_Postfix)));
        }

        public static void GetCombatAssetPaths_Postfix(ref IEnumerable<string> __result)
        {
            __result = __result.Concat(GetPowerIconPaths());
        }

        private static IEnumerable<string> GetPowerIconPaths()
        {
            return ModelDb.AllPowers
                .Select(p => p.ResolvedBigIconPath)
                .Where(path => !path.EndsWith("missing_power.png"))
                .Distinct();
        }
    }
}
