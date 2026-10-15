using HarmonyLib;

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

using sts2mod.Relics;

namespace sts2mod.Patches
{
    // 让欧洛巴斯之触（TouchOfOrobas）也能把鲜血吊坠升级替换为血族之心。
    // 仅当玩家持有鲜血吊坠（即绯血者）时，Orobas 的选项才会出现，天然只对绯血者可见。
    public static class TouchOfOrobasPatch
    {
        public static void Apply(Harmony harmony)
        {
            var method = AccessTools.Method(typeof(TouchOfOrobas), nameof(TouchOfOrobas.GetUpgradedStarterRelic));
            if (method != null)
            {
                harmony.Patch(method,
                    prefix: new HarmonyMethod(typeof(TouchOfOrobasPatch), nameof(GetUpgradedStarterRelic_Prefix)));
            }
        }

        public static bool GetUpgradedStarterRelic_Prefix(RelicModel starterRelic, ref RelicModel __result)
        {
            if (starterRelic is BloodPendant)
            {
                __result = ModelDb.Relic<BloodHeart>();
                return false;
            }
            return true;
        }
    }
}
