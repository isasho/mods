using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Powers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 生命虹吸 — 2费：对所有敌人造成8点伤害。每个敌人吸取2点生命，流血敌人改为吸取2+流血层数20%。吸取的生命同时造成等额无格挡伤害。升级后+3伤害。
    public sealed class Life_Siphon : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(8m, ValueProp.Move)
        ];

        public Life_Siphon()
            : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies) { }

        protected override bool ShouldGlowGoldInternal
        {
            get
            {
                if (CombatState == null) return false;
                foreach (var enemy in CombatState.Enemies)
                    if (enemy.GetPower<BleedPower>() != null)
                        return true;
                return false;
            }
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var combat = cardPlay.Card?.CombatState;
            if (combat == null) return;

            foreach (var enemy in combat.Enemies.ToList())
            {
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .FromCard(this).Targeting(enemy)
                    .WithHitFx("vfx/vfx_attack_slash")
                    .Execute(choiceContext);
            }

            int totalDrain = 0;
            foreach (var enemy in combat.Enemies.ToList())
            {
                int drain = 2;
                var bleed = enemy.GetPower<BleedPower>();
                if (bleed != null)
                    drain += (int)decimal.Floor(bleed.Amount * 0.2m);

                if (drain > 0)
                {
                    await CreatureCmd.Damage(choiceContext, enemy, drain,
                        ValueProp.Unblockable | ValueProp.Unpowered, this);
                    totalDrain += drain;
                }
            }

            if (totalDrain > 0)
                await CreatureCmd.Heal(Owner.Creature, totalDrain);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
