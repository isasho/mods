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
    // 流血药剂（普通，仅战斗，任意敌人）— 施加20层流血。
    public sealed class BleedPotion : PotionModel
    {
        public override PotionRarity Rarity => PotionRarity.Common;
        public override PotionUsage Usage => PotionUsage.CombatOnly;
        public override TargetType TargetType => TargetType.AnyEnemy;

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new DynamicVar("Bleed", 20m)
        ];

        protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
        {
            AssertValidForTargetedPotion(target);
            await PowerCmd.Apply<BleedPower>(choiceContext, 
                target,
                (int)base.DynamicVars["Bleed"].BaseValue,
                base.Owner.Creature,
                null, false);
        }
    }
}
