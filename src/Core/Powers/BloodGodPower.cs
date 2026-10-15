using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Powers
{
    // 血神临凡 — Buff：每回合开始失去生命并获得力量+敏捷。每张牌提供1点失血、2(3)力量、1(2)敏捷，多张同名牌效果叠加。
    public sealed class BloodGodPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        // 单张牌提供的力量值（2 未升级 / 3 升级），由打出时设置。
        public int StrengthPerCopy { get; set; } = 2;

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new DynamicVar("HpLoss", 1m),
            new DynamicVar("Dexterity", 1m)
        ];

        // 下标显示每回合开始获得的敏捷层数。
        public override int DisplayAmount
        {
            get
            {
                int strengthPerCopy = StrengthPerCopy > 0 ? StrengthPerCopy : 2;
                int copies = strengthPerCopy > 0 ? (int)base.Amount / strengthPerCopy : 0;
                return copies * (strengthPerCopy - 1);
            }
        }

        // 刷新描述变量（失血、敏捷），使其随同名牌数量实时更新。
        public void RefreshDescription()
        {
            int strengthPerCopy = StrengthPerCopy > 0 ? StrengthPerCopy : 2;
            int copies = strengthPerCopy > 0 ? (int)base.Amount / strengthPerCopy : 0;

            base.DynamicVars["HpLoss"].BaseValue = copies;
            base.DynamicVars["Dexterity"].BaseValue = copies * (strengthPerCopy - 1);
        }

        public override async Task AfterPlayerTurnStart(
            PlayerChoiceContext choiceContext,
            Player player)
        {
            if (player.Creature != base.Owner)
                return;

            int strengthPerCopy = StrengthPerCopy > 0 ? StrengthPerCopy : 2;
            int copies = strengthPerCopy > 0 ? (int)base.Amount / strengthPerCopy : 0;
            int hpLoss = copies;
            int statGain = (int)base.Amount;
            int dexGain = copies * (strengthPerCopy - 1);

            await CreatureCmd.Damage(
                choiceContext,
                base.Owner,
                hpLoss,
                ValueProp.Unblockable | ValueProp.Unpowered,
                base.Owner,
                null);

            await PowerCmd.Apply<StrengthPower>(choiceContext,
                base.Owner, statGain, base.Owner, null, false);

            await PowerCmd.Apply<DexterityPower>(choiceContext,
                base.Owner, dexGain, base.Owner, null, false);
        }
    }
}
