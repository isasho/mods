using System;
using System.Runtime.CompilerServices;

using Godot;

using HarmonyLib;

using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;

using sts2mod.Powers;

namespace sts2mod.Patches
{
    // 流血血条可视化 — 让敌人血条上出现代表「下一回合将受到的流血伤害」的深红色覆盖段，
    // 并在此伤害足以致死时把血条数字染红。机制与中毒（绿色覆盖）/ 灾厄（紫色覆盖）一致。
    // 覆盖段节点在 _Ready 时克隆 PoisonForeground 得到，复用其贴图与九宫格边距。
    public static class BleedHealthBarPatch
    {
        private static readonly ConditionalWeakTable<NHealthBar, NinePatchRect> _bleedFg = new();

        private static readonly Color _bleedForegroundColor = new Color(0.6f, 0.07f, 0.1f);
        private static readonly Color _bleedLethalTextColor = new Color(1f, 0.32f, 0.32f);
        private static readonly Color _bleedLethalOutlineColor = new Color(0.3f, 0f, 0f);

        private static AccessTools.FieldRef<NHealthBar, Creature>? _creatureRef;
        private static AccessTools.FieldRef<NHealthBar, Control>? _hpForegroundRef;
        private static AccessTools.FieldRef<NHealthBar, Control>? _poisonForegroundRef;
        private static AccessTools.FieldRef<NHealthBar, Control>? _hpForegroundContainerRef;
        private static AccessTools.FieldRef<NHealthBar, MegaLabel>? _hpLabelRef;

        public static void Apply(Harmony harmony)
        {
            try
            {
                _creatureRef = AccessTools.FieldRefAccess<NHealthBar, Creature>("_creature");
                _hpForegroundRef = AccessTools.FieldRefAccess<NHealthBar, Control>("_hpForeground");
                _poisonForegroundRef = AccessTools.FieldRefAccess<NHealthBar, Control>("_poisonForeground");
                _hpForegroundContainerRef = AccessTools.FieldRefAccess<NHealthBar, Control>("_hpForegroundContainer");
                _hpLabelRef = AccessTools.FieldRefAccess<NHealthBar, MegaLabel>("_hpLabel");
            }
            catch
            {
                return;
            }

            var ready = AccessTools.Method(typeof(NHealthBar), "_Ready");
            if (ready != null)
                harmony.Patch(ready, postfix: new HarmonyMethod(typeof(BleedHealthBarPatch), nameof(Ready_Postfix)));

            var refreshFg = AccessTools.Method(typeof(NHealthBar), "RefreshForeground");
            if (refreshFg != null)
                harmony.Patch(refreshFg, postfix: new HarmonyMethod(typeof(BleedHealthBarPatch), nameof(RefreshForeground_Postfix)));

            var refreshText = AccessTools.Method(typeof(NHealthBar), "RefreshText");
            if (refreshText != null)
                harmony.Patch(refreshText, postfix: new HarmonyMethod(typeof(BleedHealthBarPatch), nameof(RefreshText_Postfix)));
        }

        public static void Ready_Postfix(NHealthBar __instance)
        {
            try
            {
                Control poison = _poisonForegroundRef!(__instance);
                Node parent = poison?.GetParent();
                if (poison == null || parent == null)
                    return;

                var bleed = (NinePatchRect)poison.Duplicate();
                bleed.Name = "BleedForeground";
                bleed.SelfModulate = _bleedForegroundColor;
                bleed.Visible = false;
                parent.AddChild(bleed);
                _bleedFg.Add(__instance, bleed);
            }
            catch
            {
            }
        }

        public static void RefreshForeground_Postfix(NHealthBar __instance)
        {
            try
            {
                Creature creature = _creatureRef!(__instance);
                NinePatchRect bleed = GetBleedFg(__instance);
                if (creature == null || bleed == null)
                    return;

                int bleedDamage = creature.GetPower<BleedPower>()?.CalculateTotalDamageNextTurn() ?? 0;
                if (creature.CurrentHp <= 0 || creature.HpDisplay.IsInfinite() || bleedDamage <= 0)
                {
                    bleed.Visible = false;
                    return;
                }

                Control fgContainer = _hpForegroundContainerRef!(__instance);
                float maxFgWidth = fgContainer?.Size.X ?? 0f;
                if (maxFgWidth <= 0f)
                {
                    bleed.Visible = false;
                    return;
                }

                int poisonDamage = creature.GetPower<PoisonPower>()?.CalculateTotalDamageNextTurn() ?? 0;

                // 中毒已致命时，游戏已用绿色铺满整条，流血不再显示，避免重叠。
                if (poisonDamage >= creature.CurrentHp)
                {
                    bleed.Visible = false;
                    return;
                }

                // 中毒段左侧边缘（即流血段的右边缘）。
                float poisonLeft = GetFgWidth(creature, creature.CurrentHp - poisonDamage, maxFgWidth);
                int patchMarginLeft = bleed.PatchMarginLeft;
                Control hpFg = _hpForegroundRef!(__instance);

                bleed.Visible = true;
                int remaining = creature.CurrentHp - poisonDamage - bleedDamage;
                if (remaining <= 0)
                {
                    // 流血（单独或与中毒合计）足以致死：隐藏血量条，流血段铺满中毒段左侧。
                    bleed.OffsetLeft = 0f;
                    bleed.OffsetRight = poisonLeft - maxFgWidth;
                    if (hpFg != null)
                        hpFg.Visible = false;
                }
                else
                {
                    float bleedLeft = GetFgWidth(creature, remaining, maxFgWidth);
                    bleed.OffsetLeft = Math.Max(0f, bleedLeft - patchMarginLeft);
                    bleed.OffsetRight = poisonLeft - maxFgWidth;
                    if (hpFg != null)
                        hpFg.OffsetRight = bleedLeft - maxFgWidth;
                }
            }
            catch
            {
            }
        }

        public static void RefreshText_Postfix(NHealthBar __instance)
        {
            try
            {
                Creature creature = _creatureRef!(__instance);
                if (creature == null || creature.CurrentHp <= 0)
                    return;

                int bleedDamage = creature.GetPower<BleedPower>()?.CalculateTotalDamageNextTurn() ?? 0;
                if (bleedDamage <= 0 || bleedDamage < creature.CurrentHp)
                    return;

                MegaLabel label = _hpLabelRef!(__instance);
                if (label == null)
                    return;

                label.AddThemeColorOverride(ThemeConstants.Label.FontColor, _bleedLethalTextColor);
                label.AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, _bleedLethalOutlineColor);
            }
            catch
            {
            }
        }

        private static NinePatchRect? GetBleedFg(NHealthBar bar)
        {
            return _bleedFg.TryGetValue(bar, out NinePatchRect bleed) ? bleed : null;
        }

        private static float GetFgWidth(Creature creature, int amount, float maxFgWidth)
        {
            if (creature.MaxHp <= 0)
                return 0f;
            float val = (float)amount / creature.MaxHp * maxFgWidth;
            return Math.Max(val, (creature.CurrentHp > 0) ? 12f : 0f);
        }
    }
}
