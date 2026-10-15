using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

using System.Threading.Tasks;

namespace sts2mod.Relics
{
    // 血色护符（罕见）— 每失去1点生命获得1点格挡。
    public sealed class BloodAmulet : RelicModel
    {
        public override RelicRarity Rarity => RelicRarity.Uncommon;

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
            if (result.TotalDamage <= 0)
                return;
            Flash();
            await CreatureCmd.GainBlock(
                base.Owner.Creature,
                (decimal)result.TotalDamage,
                ValueProp.Move,
                null);
        }
    }
}
