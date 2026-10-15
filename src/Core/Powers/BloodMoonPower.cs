using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Powers;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Powers
{
    // 血月临空 — Buff：每回合开始失去生命并对所有敌人施加流血。每张牌提供1点失血、5(8)层流血，多张同名牌效果叠加。
    public sealed class BloodMoonPower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        // 单张牌提供的流血层数（5 未升级 / 8 升级），由打出时设置。
        public int BleedPerCopy { get; set; } = 5;

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new DynamicVar("HpLoss", 1m)
        ];

        // 刷新描述变量（失血），使其随同名牌数量实时更新。
        public void RefreshDescription()
        {
            int bleedPerCopy = BleedPerCopy > 0 ? BleedPerCopy : 5;
            int copies = bleedPerCopy > 0 ? (int)base.Amount / bleedPerCopy : 0;

            base.DynamicVars["HpLoss"].BaseValue = copies;
        }

        public override async Task AfterPlayerTurnStart(
            PlayerChoiceContext choiceContext,
            Player player)
        {
            if (player.Creature != base.Owner)
                return;

            int bleedPerCopy = BleedPerCopy > 0 ? BleedPerCopy : 5;
            int copies = bleedPerCopy > 0 ? (int)base.Amount / bleedPerCopy : 0;
            int hpLoss = copies;
            int bleed = (int)base.Amount;

            await CreatureCmd.Damage(
                choiceContext,
                base.Owner,
                hpLoss,
                ValueProp.Unblockable | ValueProp.Unpowered,
                base.Owner,
                null);

            var combat = base.Owner.CombatState;
            if (combat == null)
                return;

            foreach (var enemy in combat.Enemies)
                await PowerCmd.Apply<BleedPower>(choiceContext, enemy, bleed, base.Owner, null, false);
        }
    }
}
