using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 猎杀 — 1费：造成7点伤害，目标生命高于50%时+7伤害。升级后+3基础伤害。
    public sealed class Hunt : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(7m, ValueProp.Move)
        ];

        public Hunt()
            : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

        protected override bool ShouldGlowGoldInternal
        {
            get
            {
                if (CombatState == null) return false;
                foreach (var enemy in CombatState.Enemies)
                    if (enemy.CurrentHp > 0 && (float)enemy.CurrentHp / enemy.MaxHp >= 0.5f)
                        return true;
                return false;
            }
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            decimal damage = DynamicVars.Damage.BaseValue;
            if ((float)cardPlay.Target.CurrentHp / cardPlay.Target.MaxHp >= 0.5f)
                damage += 7;

            await DamageCmd.Attack(damage)
                .FromCard(this).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
