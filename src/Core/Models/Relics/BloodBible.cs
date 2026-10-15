using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Relics
{
    // 血之圣经（罕见）— 失去生命时获得1点痛楚。
    public sealed class BloodBible : RelicModel
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
            await PowerCmd.Apply<CrimsonRagePower>(choiceContext, 
                base.Owner.Creature,
                1m,
                base.Owner.Creature,
                null, false);
        }
    }
}
