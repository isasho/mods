using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 血之打击 — 1费：造成6点伤害。升级后+3伤害。
    public sealed class Blood_Strike : CardModel
    {
        protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];

        protected override List<DynamicVar> CanonicalVars => [
            new DamageVar(6m, ValueProp.Move)
        ];

        public Blood_Strike()
            : base(
                1,
                CardType.Attack,
                CardRarity.Basic,
                TargetType.AnyEnemy)
        {
        }

        protected override async Task OnPlay(
            PlayerChoiceContext choiceContext,
            CardPlay cardPlay)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
