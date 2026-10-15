using System.Linq;
using System.Threading.Tasks;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

using sts2mod.Powers;

namespace sts2mod.Relics
{
    // 猩红指环（罕见）— 回合开始时，对生命值最高的敌人施加4点流血。
    public sealed class CrimsonRing : RelicModel
    {
        public override RelicRarity Rarity => RelicRarity.Uncommon;

        public override async Task AfterPlayerTurnStart(
            PlayerChoiceContext choiceContext,
            Player player)
        {
            if (player != base.Owner)
                return;
            var enemies = base.Owner.Creature.CombatState.Enemies
                .Where(e => e.IsAlive)
                .ToList();
            if (enemies.Count == 0)
                return;
            var target = enemies.MaxBy(e => e.CurrentHp);
            Flash();
            await PowerCmd.Apply<BleedPower>(choiceContext, 
                target,
                4,
                base.Owner.Creature,
                null, false);
        }
    }
}
