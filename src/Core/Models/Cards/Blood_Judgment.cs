using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Core;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 鲜血审判 — 2费：造成10点伤害，目标每有1层流血+2伤害。升级后基础伤害+4。
    public sealed class Blood_Judgment : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(10m, ValueProp.Move)
        ];

        public Blood_Judgment()
            : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            decimal damage = DynamicVars.Damage.BaseValue;
            damage += BleedManager.GetBleed(cardPlay.Target) * 2;

            await DamageCmd.Attack(damage)
                .FromCard(this).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_bloody_impact")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(4m);
        }
    }
}
