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
    // 血华爆裂 — 2费：对所有敌人造成10点伤害，施加4层流血。保留。升级后+4伤害+1流血，可暴击。
    public sealed class Blood_Burst : CardModel, ICritAttack
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];

        public int CritBonusRate => 0;

        public bool CanCrit => IsUpgraded;

        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(10m, ValueProp.Move),
            new BleedVar(4m)
        ];

        public Blood_Burst()
            : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var combat = cardPlay.Card?.CombatState;
            if (combat == null) return;

            int bleed = (int)DynamicVars["Bleed"].BaseValue;

            foreach (var enemy in combat.Enemies.ToList())
            {
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .FromCard(this).Targeting(enemy)
                    .WithHitFx("vfx/vfx_attack_slash")
                    .Execute(choiceContext);

                await PowerCmd.Apply<BleedPower>(choiceContext, enemy, bleed, Owner.Creature, this, false);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(4m);
            DynamicVars["Bleed"].UpgradeValueBy(1m);
        }
    }
}
