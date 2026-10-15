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
    // 血之裂 — 0费：失去3点生命，获得2层痛楚，对一名敌人造成22点伤害。升级后+7伤害、+1痛楚。
    public sealed class Blood_Rift : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(22m, ValueProp.Move),
            new HpLossVar(3m),
            new PowerVar<CrimsonRagePower>(2m)
        ];

        public Blood_Rift()
            : base(0, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.Damage(choiceContext, Owner.Creature, DynamicVars.HpLoss.BaseValue,
                ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);

            int rage = (int)DynamicVars["CrimsonRagePower"].BaseValue;
            await PowerCmd.Apply<CrimsonRagePower>(choiceContext, Owner.Creature, rage, Owner.Creature, this, false);

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(7m);
            DynamicVars["CrimsonRagePower"].UpgradeValueBy(1m);
        }
    }
}
