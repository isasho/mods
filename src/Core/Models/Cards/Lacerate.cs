using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Core;
using sts2mod.Powers;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 撕裂 — 1费：造成5点伤害，施加5层流血。若目标已有流血，额外施加其流血层数50%的流血。升级后+3伤害+3流血。
    public sealed class Lacerate : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(5m, ValueProp.Move),
            new BleedVar(5m)
        ];

        public Lacerate()
            : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

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

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this).Targeting(target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            int baseBleed = (int)DynamicVars["Bleed"].BaseValue;
            int extraBleed = 0;

            var bleed = target.GetPower<BleedPower>();
            if (bleed != null)
                extraBleed = (int)decimal.Floor(bleed.Amount * 0.5m);

            await PowerCmd.Apply<BleedPower>(choiceContext, target, baseBleed + extraBleed, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
            DynamicVars["Bleed"].UpgradeValueBy(3m);
        }
    }
}
