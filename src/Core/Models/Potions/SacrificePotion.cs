using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace sts2mod.Potions
{
    // 献祭药剂（普通，仅战斗，自身）— 失去5点生命，获得2点能量。
    public sealed class SacrificePotion : PotionModel
    {
        public override PotionRarity Rarity => PotionRarity.Common;
        public override PotionUsage Usage => PotionUsage.CombatOnly;
        public override TargetType TargetType => TargetType.Self;

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new DynamicVar("HpLoss", 5m),
            new EnergyVar(2)
        ];

        protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
        {
            await CreatureCmd.Damage(
                choiceContext,
                base.Owner.Creature,
                base.DynamicVars["HpLoss"].BaseValue,
                ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move,
                base.Owner.Creature,
                null);
            await PlayerCmd.GainEnergy(
                base.DynamicVars.Energy.BaseValue,
                base.Owner);
        }
    }
}
