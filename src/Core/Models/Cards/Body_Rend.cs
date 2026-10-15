using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Powers;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 裂体斩 — 2费：造成10点伤害。若目标有流血，改为造成20点伤害。升级后+5伤害。
    public sealed class Body_Rend : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(10m, ValueProp.Move)
        ];

        public Body_Rend()
            : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

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
            var target = cardPlay.Target;
            if (target == null) return;

            decimal damage = DynamicVars.Damage.BaseValue;
            if (target.GetPower<BleedPower>() != null)
                damage *= 2;

            await DamageCmd.Attack(damage)
                .FromCard(this).Targeting(target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(5m);
        }
    }
}
