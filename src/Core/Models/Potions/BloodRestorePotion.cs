using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace sts2mod.Potions
{
    // 鲜血恢复剂（罕见，任意时间，任意玩家）— 回复25%最大生命值。
    public sealed class BloodRestorePotion : PotionModel
    {
        public override PotionRarity Rarity => PotionRarity.Uncommon;
        public override PotionUsage Usage => PotionUsage.AnyTime;
        public override TargetType TargetType => TargetType.AnyPlayer;

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new DynamicVar("HealPercent", 25m)
        ];

        protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
        {
            AssertValidForTargetedPotion(target);
            await CreatureCmd.Heal(
                target,
                (decimal)target.MaxHp * base.DynamicVars["HealPercent"].BaseValue / 100m);
        }
    }
}
