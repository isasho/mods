using System.Collections.Generic;

using MegaCrit.Sts2.Core.Models;

using sts2mod.Relics;

namespace sts2mod.RelicPools
{
    public sealed class BloodbinderRelicPool : RelicPoolModel
    {
        public override string EnergyColorName => "ironclad";

        protected override IEnumerable<RelicModel> GenerateAllRelics()
        {
            return new RelicModel[]
            {
                ModelDb.Relic<BloodPendant>(),
                ModelDb.Relic<CrimsonPact>(),
                ModelDb.Relic<BloodThorn>(),
                ModelDb.Relic<BloodAmulet>(),
                ModelDb.Relic<CoagulationBandage>(),
                ModelDb.Relic<CrimsonRing>(),
                ModelDb.Relic<BerserkerHelm>(),
                ModelDb.Relic<BloodBible>(),
                ModelDb.Relic<BloodMoonShard>(),
                ModelDb.Relic<SacrificialAltar>(),
                ModelDb.Relic<AncestorsBlood>(),
                ModelDb.Relic<BloodHeart>(),
            };
        }
    }
}
