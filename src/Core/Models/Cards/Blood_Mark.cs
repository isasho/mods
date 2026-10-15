using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Core;
using sts2mod.Powers;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 血印 — 0费：施加5层流血。升级后+3层。
    public sealed class Blood_Mark : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new BleedVar(5m)
        ];

        public Blood_Mark()
            : base(0, CardType.Skill, CardRarity.Basic, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(
            PlayerChoiceContext choiceContext,
            CardPlay cardPlay)
        {
            int bleedAmount = (int)DynamicVars["Bleed"].BaseValue;
            await PowerCmd.Apply<BleedPower>(choiceContext, 
                cardPlay.Target, bleedAmount, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Bleed"].UpgradeValueBy(3m);
        }
    }
}
