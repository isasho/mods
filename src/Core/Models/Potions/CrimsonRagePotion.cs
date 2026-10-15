using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using sts2mod.Powers;

namespace sts2mod.Potions
{
    // 痛楚药剂（普通，仅战斗，自身）— 获得20点痛楚。
    public sealed class CrimsonRagePotion : PotionModel
    {
        public override PotionRarity Rarity => PotionRarity.Common;
        public override PotionUsage Usage => PotionUsage.CombatOnly;
        public override TargetType TargetType => TargetType.Self;

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new DynamicVar("CrimsonRage", 20m)
        ];

        protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
        {
            await PowerCmd.Apply<CrimsonRagePower>(choiceContext, 
                base.Owner.Creature,
                (int)base.DynamicVars["CrimsonRage"].BaseValue,
                base.Owner.Creature,
                null, false);
        }
    }
}
