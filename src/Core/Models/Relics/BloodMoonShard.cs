using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

using sts2mod.Powers;

namespace sts2mod.Relics
{
    // 血月碎片（稀有）— 造成的流血层数翻倍（×2）。
    public sealed class BloodMoonShard : RelicModel
    {
        public override RelicRarity Rarity => RelicRarity.Rare;

        public override decimal ModifyPowerAmountGivenMultiplicative(
            PowerModel power,
            Creature giver,
            decimal amount,
            Creature target,
            CardModel cardSource)
        {
            if (power is BleedPower && giver == base.Owner.Creature)
                return 2m;
            return 1m;
        }
    }
}
