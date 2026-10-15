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
    // 狂怒重击 — 2费：造成8点伤害，每有1层痛楚+2伤害。升级后基础伤害+2。
    public sealed class Rage_Blow : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(8m, ValueProp.Move)
        ];

        public Rage_Blow()
            : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int rageStacks = Owner.Creature.GetPowerAmount<CrimsonRagePower>();
            decimal damage = DynamicVars.Damage.BaseValue + (rageStacks * 2m);

            await DamageCmd.Attack(damage)
                .FromCard(this).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_bloody_impact")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(2m);
        }
    }
}
