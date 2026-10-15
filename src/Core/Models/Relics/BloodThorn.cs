using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Powers;

using System.Threading.Tasks;

namespace sts2mod.Relics
{
    // 血棘（普通）— 受到敌人攻击伤害时，对攻击者施加3层流血。
    public sealed class BloodThorn : RelicModel
    {
        public override RelicRarity Rarity => RelicRarity.Common;

        public override async Task AfterDamageReceived(
            PlayerChoiceContext choiceContext,
            Creature target,
            DamageResult result,
            ValueProp props,
            Creature? dealer,
            CardModel? cardSource)
        {
            if (target != base.Owner.Creature || dealer == null || !dealer.IsEnemy || result.TotalDamage <= 0)
                return;
            Flash();
            await PowerCmd.Apply<BleedPower>(choiceContext, 
                dealer,
                3,
                base.Owner.Creature,
                null, false);
        }
    }
}
