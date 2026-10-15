using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Relics
{
    // 凝血绷带（普通）— 每回合第一次失去生命时抽1张牌。
    public sealed class CoagulationBandage : RelicModel
    {
        public override RelicRarity Rarity => RelicRarity.Common;

        private bool _triggeredThisTurn;

        public override async Task BeforeSideTurnStart(
            PlayerChoiceContext choiceContext,
            CombatSide side,
            IReadOnlyList<Creature> creatures,
            ICombatState combatState)
        {
            if (side != base.Owner.Creature.Side)
                return;
            _triggeredThisTurn = false;
            await Task.CompletedTask;
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
            if (_triggeredThisTurn)
                return;
            if (result.TotalDamage <= 0)
                return;
            _triggeredThisTurn = true;
            Flash();
            await CardPileCmd.Draw(choiceContext, 1, base.Owner);
        }
    }
}
