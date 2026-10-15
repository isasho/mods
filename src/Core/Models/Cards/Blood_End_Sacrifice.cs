using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using System.Linq;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 终末献祭 — 2费：失去20生命，对所有敌人造成100点伤害。升级后130伤害。
    public sealed class Blood_End_Sacrifice : CardModel
    {
        public Blood_End_Sacrifice()
            : base(
                2,
                CardType.Attack,
                CardRarity.Rare,
                TargetType.AllEnemies)
        {
        }

        protected override async Task OnPlay(
            PlayerChoiceContext choiceContext,
            CardPlay cardPlay)
        {
            await CreatureCmd.Damage(
                choiceContext,
                Owner.Creature,
                20,
                ValueProp.Unblockable | ValueProp.Unpowered,
                this);

            var combat = cardPlay.Card?.CombatState;
            if (combat == null)
                return;

            int damage = IsUpgraded ? 130 : 100;
            foreach (var enemy in combat.Enemies.ToList())
            {
                await DamageCmd.Attack(damage)
                    .FromCard(this)
                    .Targeting(enemy)
                    .WithHitFx("vfx/vfx_bloody_impact")
                    .Execute(choiceContext);
            }
        }
    }
}
