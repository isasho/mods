using System;

using HarmonyLib;

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Core;
using sts2mod.Powers;

namespace sts2mod.Patches
{
    // 暴击统一补丁 — 在框架伤害计算入口 Hook.ModifyDamage 结算完成后，按暴击率统一掷骰并放大伤害。
    public static class CritPatch
    {
        public static void Apply(Harmony harmony)
        {
            var method = AccessTools.Method(typeof(Hook), nameof(Hook.ModifyDamage));
            if (method != null)
                harmony.Patch(method, postfix: new HarmonyMethod(typeof(CritPatch), nameof(ModifyDamage_Postfix)));
        }

        public static void ModifyDamage_Postfix(
            Creature dealer,
            ValueProp props,
            CardModel cardSource,
            CardPreviewMode previewMode,
            ref decimal __result)
        {
            if (!props.IsPoweredAttack())
                return;
            if (dealer == null)
                return;

            // 死愿：生命值低于50%时攻击伤害翻倍（可叠加）。确定性效果，预览时也实时显示。
            var deathDesire = dealer.GetPower<DeathDesirePower>();
            if (deathDesire != null && dealer.MaxHp > 0 && (float)dealer.CurrentHp / dealer.MaxHp < 0.5f)
                __result *= (decimal)Math.Pow(2, (int)deathDesire.Amount);

            // 暴击是随机事件，只在真实结算（None）时掷骰，预览不显示。
            if (previewMode != CardPreviewMode.None)
                return;

            var critAttack = cardSource as ICritAttack;
            if (critAttack != null && !critAttack.CanCrit)
                return;

            int critRate = CritManager.GetCritRate(dealer);
            if (critAttack != null)
                critRate += critAttack.CritBonusRate;
            if (critRate <= 0)
                return;

            var combatState = dealer.CombatState;
            if (combatState == null)
                return;

            bool crit = combatState.RunState.Rng.CombatCardGeneration.NextInt(100)
                < Math.Min(critRate, 100);
            if (!crit)
                return;

            CritManager.NotifyCrit(dealer);
            decimal multiplier = critAttack?.CritMultiplier(critRate)
                ?? CritManager.GetCritMultiplier(critRate);
            __result *= multiplier;
        }
    }
}
