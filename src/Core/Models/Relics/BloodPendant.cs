using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Core;
using sts2mod.Powers;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Relics
{
    // 鲜血吊坠（初始）— 战斗开始时获得3层痛楚，生命低于50%时获得25%暴击率。
    public sealed class BloodPendant : RelicModel
    {
        public override RelicRarity Rarity => RelicRarity.Starter;

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new PowerVar<CrimsonRagePower>(3m)
        ];

        public override async Task BeforeCombatStart()
        {
            Flash();
            await PowerCmd.Apply<CrimsonRagePower>(new ThrowingPlayerChoiceContext(),
                base.Owner.Creature,
                DynamicVars["CrimsonRagePower"].BaseValue,
                base.Owner.Creature,
                null, false);
            // 累计失去生命值追踪：挂载 1 作为激活标记（框架不挂载 amount=0 的 power），读取时减 1。
            await PowerCmd.Apply<CombatHpLossPower>(new ThrowingPlayerChoiceContext(),
                base.Owner.Creature, 1, base.Owner.Creature, null, false);
            await CritManager.EnsureCritDisplay(base.Owner.Creature);
            await CritManager.EnsureCritSystem(base.Owner.Creature);
        }

        public override async Task AfterPlayerTurnStart(
            PlayerChoiceContext choiceContext,
            Player player)
        {
            if (player != base.Owner)
                return;
            await CheckCritCondition();
        }

        public override async Task AfterDamageReceived(
            PlayerChoiceContext choiceContext,
            Creature target,
            DamageResult result,
            ValueProp props,
            Creature? dealer,
            CardModel? cardSource)
        {
            if (target != base.Owner.Creature)
                return;
            await CheckCritCondition();
        }

        private async Task CheckCritCondition()
        {
            var creature = base.Owner.Creature;
            float hpPercent = (float)creature.CurrentHp / creature.MaxHp;
            bool hasCrit = creature.GetPower<BloodPendantCritPower>() != null;

            if (hpPercent < 0.5f && !hasCrit)
            {
                await PowerCmd.Apply<BloodPendantCritPower>(new ThrowingPlayerChoiceContext(), 
                    creature, 25m, creature, null, false);
                await CritManager.RefreshCritDisplay(creature);
            }
            else if (hpPercent >= 0.5f && hasCrit)
            {
                var power = creature.GetPower<BloodPendantCritPower>();
                if (power != null)
                    await PowerCmd.Remove(power);
                await CritManager.RefreshCritDisplay(creature);
            }
        }
    }
}
