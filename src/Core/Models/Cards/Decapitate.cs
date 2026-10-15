using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Core;
using sts2mod.Powers;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 断首斩 — 2费：造成12点伤害，目标流血≥10时改为三倍伤害。升级后基础伤害+3。
    public sealed class Decapitate : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(12m, ValueProp.Move)
        ];

        public Decapitate()
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
            decimal damage = DynamicVars.Damage.BaseValue;
            if (BleedManager.GetBleed(cardPlay.Target) >= 10)
                damage *= 3;

            await DamageCmd.Attack(damage)
                .FromCard(this).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_bloody_impact")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
