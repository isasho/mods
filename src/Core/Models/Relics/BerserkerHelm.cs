using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

using System.Threading.Tasks;

namespace sts2mod.Relics
{
    // 狂战士头盔（罕见）— 生命值低于50%时获得3点力量（每场战斗一次）。
    public sealed class BerserkerHelm : RelicModel
    {
        public override RelicRarity Rarity => RelicRarity.Uncommon;

        private bool _triggered;

        public override async Task AfterPlayerTurnStart(
            PlayerChoiceContext choiceContext,
            Player player)
        {
            if (player != base.Owner)
                return;
            await CheckBerserkerCondition();
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
            await CheckBerserkerCondition();
        }

        private async Task CheckBerserkerCondition()
        {
            if (_triggered)
                return;
            var creature = base.Owner.Creature;
            float hpPercent = (float)creature.CurrentHp / creature.MaxHp;
            if (hpPercent < 0.5f)
            {
                _triggered = true;
                Flash();
                await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), 
                    creature,
                    3m,
                    creature,
                    null, false);
            }
        }
    }
}
