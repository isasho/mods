using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 狂战冲锋 — 2费：失去4生命，对所有敌人造成14点伤害。升级后+5伤害。
    public sealed class Berserk_Charge : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(14m, ValueProp.Move)
        ];

        public Berserk_Charge()
            : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var combat = cardPlay.Card?.CombatState;
            if (combat == null) return;

            await CreatureCmd.Damage(choiceContext, Owner.Creature, 4,
                ValueProp.Unblockable | ValueProp.Unpowered, this);

            foreach (var enemy in combat.Enemies.ToList())
            {
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .FromCard(this).Targeting(enemy)
                    .WithHitFx("vfx/vfx_attack_slash")
                    .Execute(choiceContext);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(5m);
        }
    }
}
