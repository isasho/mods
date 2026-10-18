using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using sts2mod.Powers;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Core
{
    // 暴击管理器 — 基础暴击率0%。痛楚+5%/层，血涌+25%/50%，嗜血狂乱双倍。暴击率无上限。
    public static class CritManager
    {
        public const int BaseCritRate = 0;

        // 暴击伤害倍率：基础 50% 额外伤害；暴击率超过 100% 的部分继续累加（125%→1.75，250%→3.0）
        public static decimal GetCritMultiplier(int critRate)
        {
            decimal bonus = 50m + Math.Max(0, critRate - 100);
            return 1m + bonus / 100m;
        }

        private static readonly Dictionary<Creature, bool> _lastCritFlags = new();

        public static int GetCritRate(Creature creature)
        {
            int rate = BaseCritRate;

            var rush = creature.GetPower<BloodRushPower>();
            if (rush != null)
                rate += rush.BonusCritRate;

            var pain = creature.GetPower<PainEmpowerPower>();
            if (pain != null)
                rate += (int)pain.Amount;

            var rage = creature.GetPower<CrimsonRagePower>();
            if (rage != null)
                rate += (int)rage.Amount * 5;

            var pendantCrit = creature.GetPower<BloodPendantCritPower>();
            if (pendantCrit != null)
                rate += (int)pendantCrit.Amount;

            var heartCrit = creature.GetPower<BloodHeartCritPower>();
            if (heartCrit != null)
                rate += (int)heartCrit.Amount;

            var altarCrit = creature.GetPower<SacrificialAltarCritPower>();
            if (altarCrit != null)
                rate += (int)altarCrit.Amount;

            var frenzy = creature.GetPower<BloodFrenzyPower>();
            if (frenzy != null)
                rate = (int)(rate * Math.Pow(2, (int)frenzy.Amount));

            return rate;
        }

        public static void NotifyCrit(Creature creature)
        {
            _lastCritFlags[creature] = true;
        }

        public static bool ConsumeCritFlag(Creature creature)
        {
            if (_lastCritFlags.TryGetValue(creature, out bool hasCrit) && hasCrit)
            {
                _lastCritFlags[creature] = false;
                return true;
            }
            return false;
        }

        public static bool PeekCritFlag(Creature creature)
        {
            return _lastCritFlags.TryGetValue(creature, out bool hasCrit) && hasCrit;
        }

        public static async Task RefreshCritDisplay(Creature creature)
        {
            int rate = GetCritRate(creature);
            var existing = creature.GetPower<CritRatePower>();
            var ctx = new ThrowingPlayerChoiceContext();
            if (existing != null)
            {
                await PowerCmd.ModifyAmount(ctx, existing, rate - existing.Amount, creature, null, false);
            }
            else
            {
                await PowerCmd.Apply<CritRatePower>(ctx, creature, rate, creature, null, false);
            }
        }

        public static async Task EnsureCritDisplay(Creature creature)
        {
            if (creature.GetPower<CritRatePower>() != null)
                return;
            int rate = GetCritRate(creature);
            await PowerCmd.Apply<CritRatePower>(new ThrowingPlayerChoiceContext(), creature, rate, creature, null, false);
        }

        public static async Task EnsureCritSystem(Creature creature)
        {
            if (creature.GetPower<CritSystemPower>() != null)
                return;
            await PowerCmd.Apply<CritSystemPower>(new ThrowingPlayerChoiceContext(), creature, 0, creature, null, false);
        }
    }
}
