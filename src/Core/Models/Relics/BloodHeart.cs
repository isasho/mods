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
    // 血族之心（初始升级版）— 战斗开始时获得6层痛楚，生命低于50%时获得40%暴击率。
    // 由欧洛巴斯之触（TouchOfOrobas）替换鲜血吊坠获得。
    public sealed class BloodHeart : RelicModel
    {
        public override RelicRarity Rarity => RelicRarity.Starter;

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new PowerVar<CrimsonRagePower>(6m)
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
            bool hasCrit = creature.GetPower<BloodHeartCritPower>() != null;

            if (hpPercent < 0.5f && !hasCrit)
            {
                await PowerCmd.Apply<BloodHeartCritPower>(new ThrowingPlayerChoiceContext(),
                    creature, 40m, creature, null, false);
                await CritManager.RefreshCritDisplay(creature);
            }
            else if (hpPercent >= 0.5f && hasCrit)
            {
                var power = creature.GetPower<BloodHeartCritPower>();
                if (power != null)
                    await PowerCmd.Remove(power);
                await CritManager.RefreshCritDisplay(creature);
            }
        }
    }
}
