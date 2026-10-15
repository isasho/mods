using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

using sts2mod.Core;
using sts2mod.Powers;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 猩红刻印 — 1费：施加10层流血和1层易伤。升级后+5流血+1易伤。
    public sealed class Crimson_Imprint : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new BleedVar(10m)
        ];

        public Crimson_Imprint()
            : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int bleed = (int)DynamicVars["Bleed"].BaseValue;
            int vuln = IsUpgraded ? 2 : 1;
            await PowerCmd.Apply<BleedPower>(choiceContext, cardPlay.Target, bleed, Owner.Creature, this, false);
            await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, vuln, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Bleed"].UpgradeValueBy(5m);
        }
    }
}
