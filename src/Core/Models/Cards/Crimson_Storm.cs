using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Core;
using sts2mod.Powers;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 猩红风暴 — 2费：对所有敌人造成4点伤害，获得1层痛楚。重复3次。升级后+2伤害。
    public sealed class Crimson_Storm : CardModel, IRepeatAttackInfo
    {
        public int HitsPerTarget => 3;

        public int BleedAppliesPerTarget => 0;

        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(4m, ValueProp.Move)
        ];

        public Crimson_Storm()
            : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var combat = cardPlay.Card?.CombatState;
            if (combat == null) return;

            for (int i = 0; i < 3; i++)
            {
                foreach (var enemy in combat.Enemies.ToList())
                {
                    if (enemy.IsDead) continue;
                    await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                        .FromCard(this).Targeting(enemy)
                        .WithHitFx("vfx/vfx_attack_slash")
                        .Execute(choiceContext);
                }

                await PowerCmd.Apply<CrimsonRagePower>(choiceContext, Owner.Creature, 1, Owner.Creature, this, false);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(2m);
        }
    }
}
