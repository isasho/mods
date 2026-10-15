using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Powers;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 鲜血回旋 — 1费：对所有敌人造成3点伤害。每有一个流血的敌人，重复1次。升级后+2伤害。
    public sealed class Blood_Whirl : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(3m, ValueProp.Move)
        ];

        public Blood_Whirl()
            : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies) { }

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

            int bleedingCount = combat.Enemies.Count(e => e.GetPower<BleedPower>() != null);
            int repeat = 1 + bleedingCount;

            for (int i = 0; i < repeat; i++)
            {
                foreach (var enemy in combat.Enemies.ToList())
                {
                    await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                        .FromCard(this).Targeting(enemy)
                        .WithHitFx("vfx/vfx_attack_slash")
                        .Execute(choiceContext);
                }
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(2m);
        }
    }
}
