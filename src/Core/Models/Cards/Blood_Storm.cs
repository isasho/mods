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
    // 血岚 — 2费：对所有敌人造成5点伤害，施加5层流血。重复2次。升级后+5伤害+5流血。
    public sealed class Blood_Storm : CardModel, IRepeatAttackInfo
    {
        public int HitsPerTarget => 2;

        public int BleedAppliesPerTarget => 2;

        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(5m, ValueProp.Move),
            new BleedVar(5m)
        ];

        public Blood_Storm()
            : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var combat = cardPlay.Card?.CombatState;
            if (combat == null) return;

            int bleed = (int)DynamicVars["Bleed"].BaseValue;

            for (int i = 0; i < 2; i++)
            {
                foreach (var enemy in combat.Enemies.ToList())
                {
                    await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                        .FromCard(this).Targeting(enemy)
                        .WithHitFx("vfx/vfx_attack_slash")
                        .Execute(choiceContext);

                    await PowerCmd.Apply<BleedPower>(choiceContext, enemy, bleed, Owner.Creature, this, false);
                }
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(5m);
            DynamicVars["Bleed"].UpgradeValueBy(5m);
        }
    }
}
