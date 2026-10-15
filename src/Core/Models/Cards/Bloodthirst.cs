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
    // 嗜血斩 — 1费：造成7点伤害。暴击时回复3生命，并对目标造成等额生命流失。升级后+3伤害回复5。
    public sealed class Bloodthirst : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(7m, ValueProp.Move)
        ];

        public Bloodthirst()
            : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            if (CritManager.ConsumeCritFlag(Owner.Creature))
            {
                int heal = IsUpgraded ? 5 : 3;
                await CreatureCmd.Heal(Owner.Creature, heal);
                await CreatureCmd.Damage(choiceContext, cardPlay.Target, heal,
                    ValueProp.Unblockable | ValueProp.Unpowered, this);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
