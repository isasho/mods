using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Powers
{
    // 失控 — Buff：每回合开始获得力量和痛楚（每层2点），失去等同于力量层数的生命。多张同名牌效果叠加。
    public sealed class LossOfControlPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new DynamicVar("StatGain", 2m)
        ];

        // 刷新描述变量（每回合获得的力量/痛楚），使其随叠加实时更新。
        public void RefreshDescription()
        {
            int stacks = Math.Max(0, (int)base.Amount);
            base.DynamicVars["StatGain"].BaseValue = 2 * stacks;
        }

        public override async Task AfterPlayerTurnStart(
            PlayerChoiceContext choiceContext,
            Player player)
        {
            if (player.Creature != base.Owner)
                return;

            int stacks = Math.Max(0, (int)base.Amount);
            int statGain = 2 * stacks;

            await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, statGain, base.Owner, null, false);
            await PowerCmd.Apply<CrimsonRagePower>(choiceContext, base.Owner, statGain, base.Owner, null, false);

            var str = base.Owner.GetPower<StrengthPower>();
            int hpLoss = str != null ? (int)str.Amount : 0;
            if (hpLoss > 0)
            {
                await CreatureCmd.Damage(choiceContext, base.Owner, hpLoss,
                    ValueProp.Unblockable | ValueProp.Unpowered, base.Owner, null);
            }
        }
    }
}
