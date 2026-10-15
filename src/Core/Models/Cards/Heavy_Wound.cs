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
    // 重创 — 2费：造成15点伤害（附加已损失生命值20%），施加8层流血。升级后+4伤害+4流血。
    public sealed class Heavy_Wound : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(15m, ValueProp.Move),
            new BleedVar(8m)
        ];

        public Heavy_Wound()
            : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var target = cardPlay.Target;
            if (target == null) return;

            decimal missingHp = Owner.Creature.MaxHp - Owner.Creature.CurrentHp;
            decimal bonus = decimal.Floor(missingHp * 0.2m);

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue + bonus)
                .FromCard(this).Targeting(target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            int bleed = (int)DynamicVars["Bleed"].BaseValue;
            await PowerCmd.Apply<BleedPower>(choiceContext, target, bleed, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(4m);
            DynamicVars["Bleed"].UpgradeValueBy(4m);
        }
    }
}
