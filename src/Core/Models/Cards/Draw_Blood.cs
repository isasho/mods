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
    // 抽血 — 1费：造成3点伤害，抽1张牌，施加3层流血。升级后+2伤害+2流血。
    public sealed class Draw_Blood : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(3m, ValueProp.Move),
            new BleedVar(3m)
        ];

        public Draw_Blood()
            : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            await CardPileCmd.Draw(choiceContext, 1, Owner);

            int bleed = (int)DynamicVars["Bleed"].BaseValue;
            await PowerCmd.Apply<BleedPower>(choiceContext, cardPlay.Target, bleed, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(2m);
            DynamicVars["Bleed"].UpgradeValueBy(2m);
        }
    }
}
