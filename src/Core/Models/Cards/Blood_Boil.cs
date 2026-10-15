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
    // 血沸 — 1费：失去3点生命，对一名敌人造成5点伤害5次。升级后每次+3。
    public sealed class Blood_Boil : CardModel, IRepeatAttackInfo
    {
        public int HitsPerTarget => 5;

        public int BleedAppliesPerTarget => 0;

        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(5m, ValueProp.Move)
        ];

        public Blood_Boil()
            : base(1, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.Damage(choiceContext, Owner.Creature, 3,
                ValueProp.Unblockable | ValueProp.Unpowered, this);

            var target = cardPlay.Target;
            if (target == null) return;

            for (int i = 0; i < 5; i++)
            {
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .FromCard(this).Targeting(target)
                    .WithHitFx("vfx/vfx_bloody_impact")
                    .Execute(choiceContext);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
