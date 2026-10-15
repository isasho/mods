using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Godot;

using HarmonyLib;

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using MegaCrit.Sts2.Core.Saves.Managers;

using sts2mod.Characters;

namespace sts2mod.Patches
{
    // 绯血者角色选择界面 Harmony 补丁 — 通过 ModInitializer 手动注册。
    public static class CharacterPatch
    {
        public static void Apply(Harmony harmony)
        {
            // ModelDb.AllCharacters — 注入 Bloodbinder
            harmony.Patch(
                AccessTools.PropertyGetter(typeof(ModelDb), nameof(ModelDb.AllCharacters)),
                postfix: new HarmonyMethod(typeof(CharacterPatch), nameof(AllCharacters_Postfix)));

            // 非 virtual 的场景/材质/图片路径 — 重定向到 Ironclad 占位
            PatchPath(harmony, "get_CharacterSelectBg", nameof(RedirectToIroncladBg));
            PatchPath(harmony, "get_VisualsPath", nameof(RedirectToIroncladVisuals));
            PatchPath(harmony, "get_EnergyCounterPath", nameof(RedirectToIroncladEnergy));
            PatchPath(harmony, "get_MerchantAnimPath", nameof(RedirectToIroncladMerchant));
            PatchPath(harmony, "get_RestSiteAnimPath", nameof(RedirectToIroncladRest));
            PatchPath(harmony, "get_TrailPath", nameof(RedirectToIroncladTrail));
            PatchPath(harmony, "get_CharacterSelectTransitionPath", nameof(RedirectToIroncladTransition));
            PatchPath(harmony, "get_IconTexturePath", nameof(RedirectToIroncladIcon));
            PatchPath(harmony, "get_IconOutlineTexturePath", nameof(RedirectToIroncladIconOutline));
            PatchPath(harmony, "get_ArmPointingTexturePath", nameof(RedirectToIroncladPoint));
            PatchPath(harmony, "get_ArmRockTexturePath", nameof(RedirectToIroncladRock));
            PatchPath(harmony, "get_ArmPaperTexturePath", nameof(RedirectToIroncladPaper));
            PatchPath(harmony, "get_ArmScissorsTexturePath", nameof(RedirectToIroncladScissors));

            // 音频 — AttackSfx/CastSfx/DeathSfx 非 virtual，强制重定向
            PatchPath(harmony, "get_AttackSfx", nameof(RedirectToIroncladAttackSfx));
            PatchPath(harmony, "get_CastSfx", nameof(RedirectToIroncladCastSfx));
            PatchPath(harmony, "get_DeathSfx", nameof(RedirectToIroncladDeathSfx));

            // Relic 轮廓精灵 — mod 遗物没有 outline atlas，重定向到 BurningBlood 占位
            harmony.Patch(
                AccessTools.PropertyGetter(typeof(RelicModel), "PackedIconOutlinePath"),
                postfix: new HarmonyMethod(typeof(CharacterPatch), nameof(RelicOutline_Postfix)));

            // 百科大全 — 注入 Bloodbinder 卡池到 Ironclad 过滤器，并为 Bloodbinder 角色建立映射
            harmony.Patch(
                AccessTools.Method(typeof(NCardLibrary), "_Ready"),
                postfix: new HarmonyMethod(typeof(CharacterPatch), nameof(CardLibrary_Ready_Postfix)));

            // CheckFifteenElitesDefeatedEpoch — Bloodbinder 不在角色白名单，跳过
            var checkElites = AccessTools.Method(typeof(ProgressSaveManager),
                "CheckFifteenElitesDefeatedEpoch");
            if (checkElites != null)
            {
                harmony.Patch(checkElites,
                    prefix: new HarmonyMethod(typeof(CharacterPatch),
                        nameof(CheckFifteenElites_Prefix)));
            }

            // ObtainCharUnlockEpoch — Bloodbinder 没有对应 Epoch，跳过
            var obtainEpoch = AccessTools.Method(typeof(ProgressSaveManager),
                "ObtainCharUnlockEpoch");
            if (obtainEpoch != null)
            {
                harmony.Patch(obtainEpoch,
                    prefix: new HarmonyMethod(typeof(CharacterPatch),
                        nameof(ObtainCharUnlockEpoch_Prefix)));
            }

            // CheckFifteenBossesDefeatedEpoch — 同上，跳过避免角色枚举越界导致 boss 奖励界面卡死
            var checkBosses = AccessTools.Method(typeof(ProgressSaveManager),
                "CheckFifteenBossesDefeatedEpoch");
            if (checkBosses != null)
            {
                harmony.Patch(checkBosses,
                    prefix: new HarmonyMethod(typeof(CharacterPatch),
                        nameof(CheckFifteenBosses_Prefix)));
            }
        }

        private static void PatchPath(Harmony harmony, string getterName, string postfixName)
        {
            var method = typeof(CharacterModel).GetProperty(getterName.Substring(4),
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetMethod;
            if (method != null)
            {
                harmony.Patch(method,
                    postfix: new HarmonyMethod(typeof(CharacterPatch), postfixName));
            }
        }

        public static void AllCharacters_Postfix(ref IEnumerable<CharacterModel> __result)
        {
            __result = __result.Append(ModelDb.Character<Bloodbinder>());
        }

        public static void RedirectToIroncladBg(CharacterModel __instance, ref string __result)
        {
            if (__instance is Bloodbinder)
                __result = SceneHelper.GetScenePath("screens/char_select/char_select_bg_ironclad");
        }

        public static void RedirectToIroncladVisuals(CharacterModel __instance, ref string __result)
        {
            if (__instance is Bloodbinder)
                __result = SceneHelper.GetScenePath("creature_visuals/ironclad");
        }

        public static void RedirectToIroncladEnergy(CharacterModel __instance, ref string __result)
        {
            if (__instance is Bloodbinder)
                __result = SceneHelper.GetScenePath("combat/energy_counters/ironclad_energy_counter");
        }

        public static void RedirectToIroncladMerchant(CharacterModel __instance, ref string __result)
        {
            if (__instance is Bloodbinder)
                __result = SceneHelper.GetScenePath("merchant/characters/ironclad_merchant");
        }

        public static void RedirectToIroncladRest(CharacterModel __instance, ref string __result)
        {
            if (__instance is Bloodbinder)
                __result = SceneHelper.GetScenePath("rest_site/characters/ironclad_rest_site");
        }

        public static void RedirectToIroncladTrail(CharacterModel __instance, ref string __result)
        {
            if (__instance is Bloodbinder)
                __result = SceneHelper.GetScenePath("vfx/card_trail_ironclad");
        }

        public static void RedirectToIroncladTransition(CharacterModel __instance, ref string __result)
        {
            if (__instance is Bloodbinder)
                __result = "res://materials/transitions/ironclad_transition_mat.tres";
        }

        public static void RedirectToIroncladIcon(CharacterModel __instance, ref string __result)
        {
            if (__instance is Bloodbinder)
                __result = ImageHelper.GetImagePath("ui/top_panel/character_icon_bloodbinder.png");
        }

        public static void RedirectToIroncladIconOutline(CharacterModel __instance, ref string __result)
        {
            if (__instance is Bloodbinder)
                __result = ImageHelper.GetImagePath("ui/top_panel/character_icon_bloodbinder_outline.png");
        }

        public static void RedirectToIroncladPoint(CharacterModel __instance, ref string __result)
        {
            if (__instance is Bloodbinder)
                __result = ImageHelper.GetImagePath("ui/hands/multiplayer_hand_ironclad_point.png");
        }

        public static void RedirectToIroncladRock(CharacterModel __instance, ref string __result)
        {
            if (__instance is Bloodbinder)
                __result = ImageHelper.GetImagePath("ui/hands/multiplayer_hand_ironclad_rock.png");
        }

        public static void RedirectToIroncladPaper(CharacterModel __instance, ref string __result)
        {
            if (__instance is Bloodbinder)
                __result = ImageHelper.GetImagePath("ui/hands/multiplayer_hand_ironclad_paper.png");
        }

        public static void RedirectToIroncladScissors(CharacterModel __instance, ref string __result)
        {
            if (__instance is Bloodbinder)
                __result = ImageHelper.GetImagePath("ui/hands/multiplayer_hand_ironclad_scissors.png");
        }

        public static void RedirectToIroncladAttackSfx(CharacterModel __instance, ref string __result)
        {
            if (__instance is Bloodbinder)
                __result = "event:/sfx/characters/ironclad/ironclad_attack";
        }

        public static void RedirectToIroncladCastSfx(CharacterModel __instance, ref string __result)
        {
            if (__instance is Bloodbinder)
                __result = "event:/sfx/characters/ironclad/ironclad_cast";
        }

        public static void RedirectToIroncladDeathSfx(CharacterModel __instance, ref string __result)
        {
            if (__instance is Bloodbinder)
                __result = "event:/sfx/characters/ironclad/ironclad_die";
        }

        public static void RelicOutline_Postfix(RelicModel __instance, ref string __result)
        {
            // mod 遗物若自带轮廓图（relic_outline_atlas.sprites/*.tres）则保留，否则回退到燃烧之血占位。
            if (__instance.GetType().Assembly == typeof(CharacterPatch).Assembly
                && !ResourceLoader.Exists(__result))
                __result = ImageHelper.GetImagePath("atlases/relic_outline_atlas.sprites/burning_blood.tres");
        }

        public static void CardLibrary_Ready_Postfix(NCardLibrary __instance)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;

            var poolFiltersField = typeof(NCardLibrary).GetField("_poolFilters", flags);
            var cardPoolFiltersField = typeof(NCardLibrary).GetField("_cardPoolFilters", flags);
            var necrobinderFilterField = typeof(NCardLibrary).GetField("_necrobinderFilter", flags);
            var ironcladFilterField = typeof(NCardLibrary).GetField("_ironcladFilter", flags);

            if (poolFiltersField == null || cardPoolFiltersField == null
                || necrobinderFilterField == null || ironcladFilterField == null)
                return;

            var poolFilters = (Dictionary<NCardPoolFilter, Func<CardModel, bool>>)poolFiltersField.GetValue(__instance);
            var cardPoolFilters = (Dictionary<CharacterModel, NCardPoolFilter>)cardPoolFiltersField.GetValue(__instance);
            var necrobinderFilter = (NCardPoolFilter)necrobinderFilterField.GetValue(__instance);
            var ironcladFilter = (NCardPoolFilter)ironcladFilterField.GetValue(__instance);

            if (poolFilters == null || cardPoolFilters == null
                || necrobinderFilter == null || ironcladFilter == null)
                return;

            // 克隆 Necrobinder 过滤器节点创建 Bloodbinder 独立标签
            var bloodbinderFilter = (NCardPoolFilter)necrobinderFilter.Duplicate(11);

            // 使用绯血者自己的小头像，缺失则回退到 Ironclad 占位
            var ironcladImage = ironcladFilter.GetNode<TextureRect>("Image");
            var bloodbinderImage = bloodbinderFilter.GetNode<TextureRect>("Image");
            if (bloodbinderImage != null)
            {
                string iconPath = ImageHelper.GetImagePath("ui/top_panel/character_icon_bloodbinder.png");
                Texture2D icon = ResourceLoader.Exists(iconPath)
                    ? ResourceLoader.Load<Texture2D>(iconPath)
                    : (ironcladImage != null ? ironcladImage.Texture : null);
                if (icon != null)
                {
                    bloodbinderImage.Texture = icon;
                    var bloodbinderShadow = bloodbinderImage.GetNode<TextureRect>("Shadow");
                    if (bloodbinderShadow != null)
                        bloodbinderShadow.Texture = icon;
                }
            }

            // 添加到 PoolFilters 网格容器
            var parent = necrobinderFilter.GetParent();
            if (parent != null)
                parent.AddChild(bloodbinderFilter);

            // 连接 Toggled 信号到 UpdateCardPoolFilter
            var updateMethod = typeof(NCardLibrary).GetMethod("UpdateCardPoolFilter", flags);
            if (updateMethod != null)
            {
                var action = (Action<NCardPoolFilter>)Delegate.CreateDelegate(
                    typeof(Action<NCardPoolFilter>), __instance, updateMethod);
                bloodbinderFilter.Connect("Toggled", Callable.From<NCardPoolFilter>(action));
            }

            // 注册 Bloodbinder 卡池过滤器
            poolFilters[bloodbinderFilter] = (CardModel c) =>
                c.Pool is BloodbinderCardPool;

            // 注册角色到过滤器的映射（用于游戏中打开百科大全时自动选中）
            var bloodbinder = ModelDb.Character<Bloodbinder>();
            if (bloodbinder != null)
                cardPoolFilters[bloodbinder] = bloodbinderFilter;

            // 确保过滤器可见（不受解锁状态影响）
            bloodbinderFilter.Visible = true;
        }

        public static bool CheckFifteenElites_Prefix(Player localPlayer)
        {
            return localPlayer?.Character is not Bloodbinder;
        }

        public static bool CheckFifteenBosses_Prefix(Player localPlayer)
        {
            return localPlayer?.Character is not Bloodbinder;
        }

        public static bool ObtainCharUnlockEpoch_Prefix(Player localPlayer, int act)
        {
            return localPlayer?.Character is not Bloodbinder;
        }
    }
}
