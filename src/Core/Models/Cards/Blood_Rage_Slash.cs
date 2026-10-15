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
    // 血怒斩杀 — 2费：造成14点伤害。暴击时引爆流血×2（有鲜血渴望则引爆但不消耗流血）。升级后+4伤害。
    public sealed class Blood_Rage_Slash : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(14m, ValueProp.Move)
        ];

        public Blood_Rage_Slash()
            : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(
            PlayerChoiceContext choiceContext,
            CardPlay cardPlay)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            if (!CritManager.ConsumeCritFlag(Owner.Creature))
                return;

            int bleed = BleedManager.GetBleed(cardPlay.Target);
            if (bleed <= 0)
                return;

            await BleedManager.Detonate(choiceContext, cardPlay.Target, 2m, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(4m);
        }
    }
}
