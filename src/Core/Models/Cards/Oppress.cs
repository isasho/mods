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
    // 压迫 — 0费：施加1层易伤和3层流血。升级后+1易伤+2流血。
    public sealed class Oppress : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new BleedVar(3m)
        ];

        public Oppress()
            : base(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int vuln = IsUpgraded ? 2 : 1;
            int bleed = (int)DynamicVars["Bleed"].BaseValue;

            await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, vuln, Owner.Creature, this, false);
            await PowerCmd.Apply<BleedPower>(choiceContext, cardPlay.Target, bleed, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Bleed"].UpgradeValueBy(2m);
        }
    }
}
