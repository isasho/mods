using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Core;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Powers
{
    // 不灭狂怒 — Buff：每回合开始获得痛楚。
    public sealed class EndlessRagePower : PowerModel
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;


        public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> creatures, ICombatState combatState)
        {
            if (side != base.Owner.Side)
                return;
            int rage = (int)base.Amount;
            await PowerCmd.Apply<CrimsonRagePower>(choiceContext, base.Owner, rage, base.Owner, null, false);
        }
    }
}
