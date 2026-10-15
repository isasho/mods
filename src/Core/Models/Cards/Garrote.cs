using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Core;
using sts2mod.Powers;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 绞杀 — 2费：造成8点伤害。若目标有虚弱，移除虚弱并2倍伤害。否则施加2层虚弱和8层流血。升级后+3伤害+4流血。
    public sealed class Garrote : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(8m, ValueProp.Move),
            new BleedVar(8m)
        ];

        public Garrote()
            : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        protected override bool ShouldGlowGoldInternal
        {
            get
            {
                if (CombatState == null) return false;
                foreach (var enemy in CombatState.Enemies)
                    if (enemy.GetPower<WeakPower>() != null)
                        return true;
                return false;
            }
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var target = cardPlay.Target;
            if (target == null) return;

            var weak = target.GetPower<WeakPower>();
            decimal damage = DynamicVars.Damage.BaseValue;

            if (weak != null)
            {
                await PowerCmd.Remove(weak);
                damage *= 2;
            }

            await DamageCmd.Attack(damage)
                .FromCard(this).Targeting(target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            if (weak == null && !target.IsDead)
            {
                int bleed = (int)DynamicVars["Bleed"].BaseValue;
                await PowerCmd.Apply<WeakPower>(choiceContext, target, 2, Owner.Creature, this, false);
                await PowerCmd.Apply<BleedPower>(choiceContext, target, bleed, Owner.Creature, this, false);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
            DynamicVars["Bleed"].UpgradeValueBy(4m);
        }
    }
}
