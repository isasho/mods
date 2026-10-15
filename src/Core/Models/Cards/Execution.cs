using MegaCrit.Sts2.Core.Combat;
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
    // 处决 — 2费：造成12点伤害，目标生命低于20%时伤害+500%。升级后目标生命低于35%时伤害+500%。
    public sealed class Execution : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(12m, ValueProp.Move)
        ];

        public Execution()
            : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        // 卡牌高亮：存在可斩杀的敌人时金光
        protected override bool ShouldGlowGoldInternal
        {
            get
            {
                if (CombatState == null)
                    return false;
                float threshold = IsUpgraded ? 0.35f : 0.20f;
                foreach (var enemy in CombatState.Enemies)
                {
                    if (enemy.CurrentHp > 0 &&
                        (float)enemy.CurrentHp / enemy.MaxHp < threshold)
                        return true;
                }
                return false;
            }
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            decimal damage = DynamicVars.Damage.BaseValue;
            float threshold = IsUpgraded ? 0.35f : 0.20f;

            if ((float)cardPlay.Target.CurrentHp / cardPlay.Target.MaxHp < threshold)
                damage += damage * 5; // +500%

            await DamageCmd.Attack(damage)
                .FromCard(this).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            // upgrade only changes threshold, base damage stays 12
        }
    }
}
