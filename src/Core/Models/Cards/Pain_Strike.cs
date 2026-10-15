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
    // 痛苦打击 — 1费：造成7点伤害，获得1层痛楚。升级后+3伤害+1痛楚。
    public sealed class Pain_Strike : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(7m, ValueProp.Move)
        ];

        public Pain_Strike()
            : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            int rage = IsUpgraded ? 2 : 1;
            await PowerCmd.Apply<CrimsonRagePower>(choiceContext, Owner.Creature, rage, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
