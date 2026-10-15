using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Powers
{
    // 以血易血 — Buff：每回合开始失去生命并获得能量。每张牌提供失血（2/1）和1点能量，多张同名牌效果叠加。
    public sealed class BloodForBloodPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        // 单张牌提供的失血量（2 未升级 / 1 升级），由打出时设置。
        public int HpLossPerCopy { get; set; } = 2;

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new DynamicVar("Energy", 1m)
        ];

        // 刷新描述变量（能量数），使其随同名牌数量实时更新。
        public void RefreshDescription()
        {
            int hpLossPerCopy = HpLossPerCopy > 0 ? HpLossPerCopy : 2;
            int copies = hpLossPerCopy > 0 ? (int)base.Amount / hpLossPerCopy : 0;

            base.DynamicVars["Energy"].BaseValue = copies;
        }

        public override async Task AfterPlayerTurnStart(
            PlayerChoiceContext choiceContext,
            Player player)
        {
            if (player.Creature != base.Owner)
                return;

            int hpLossPerCopy = HpLossPerCopy > 0 ? HpLossPerCopy : 2;
            int copies = hpLossPerCopy > 0 ? (int)base.Amount / hpLossPerCopy : 0;
            int hpLoss = (int)base.Amount;
            int energy = copies;

            await CreatureCmd.Damage(
                choiceContext,
                base.Owner,
                hpLoss,
                ValueProp.Unblockable | ValueProp.Unpowered,
                base.Owner,
                null);

            if (energy > 0)
                await PlayerCmd.GainEnergy(energy, player);
        }
    }
}
