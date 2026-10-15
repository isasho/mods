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
    // 血刃 — 1费：造成9点伤害，本场战斗累计失去10+生命时+5伤害。升级后基础伤害+3。
    public sealed class Blood_Blade : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(9m, ValueProp.Move)
        ];

        public Blood_Blade()
            : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

        protected override bool ShouldGlowGoldInternal
        {
            get
            {
                var creature = Owner?.Creature;
                return creature != null && (creature.MaxHp - creature.CurrentHp) >= 10;
            }
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            decimal damage = DynamicVars.Damage.BaseValue;
            int missingHp = Owner.Creature.MaxHp - Owner.Creature.CurrentHp;
            if (missingHp >= 10)
                damage += 5;

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
