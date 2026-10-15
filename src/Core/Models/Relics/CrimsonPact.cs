using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

using sts2mod.Powers;

namespace sts2mod.Relics
{
    // 猩红契约（普通）— 每当你获得痛楚时，额外获得1点。
    public sealed class CrimsonPact : RelicModel
    {
        public override RelicRarity Rarity => RelicRarity.Common;

        public override decimal ModifyPowerAmountGivenAdditive(
            PowerModel power,
            Creature giver,
            decimal amount,
            Creature target,
            CardModel cardSource)
        {
            if (power is CrimsonRagePower && target == base.Owner.Creature)
                return 1m;
            return 0m;
        }
    }
}
