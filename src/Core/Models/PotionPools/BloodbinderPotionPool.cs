using System.Collections.Generic;

using MegaCrit.Sts2.Core.Models;

using sts2mod.Potions;

namespace sts2mod.PotionPools
{
    public sealed class BloodbinderPotionPool : PotionPoolModel
    {
        public override string EnergyColorName => "ironclad";

        protected override IEnumerable<PotionModel> GenerateAllPotions()
        {
            return new PotionModel[]
            {
                ModelDb.Potion<BleedPotion>(),
                ModelDb.Potion<SacrificePotion>(),
                ModelDb.Potion<CrimsonRagePotion>(),
                ModelDb.Potion<BloodRestorePotion>(),
                ModelDb.Potion<BloodBurstPotion>(),
            };
        }
    }
}
