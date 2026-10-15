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
    // 割腕 — 0费：失去3点生命，获得2层痛楚，造成6点伤害。升级后+1痛楚+3伤害。
    public sealed class Wrist_Cut : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(6m, ValueProp.Move)
        ];

        public Wrist_Cut()
            : base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.Damage(choiceContext, Owner.Creature, 3,
                ValueProp.Unblockable | ValueProp.Unpowered, this);

            int rage = IsUpgraded ? 3 : 2;
            await PowerCmd.Apply<CrimsonRagePower>(choiceContext, Owner.Creature, rage, Owner.Creature, this, false);

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
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
