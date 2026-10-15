using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

using sts2mod.Core;
using sts2mod.Powers;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 血之共鸣 — 1费：对所有敌人施加6层流血，抽1张牌。升级后+3流血。
    public sealed class Blood_Resonance : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new BleedVar(6m)
        ];

        public Blood_Resonance()
            : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var combat = cardPlay.Card?.CombatState;
            if (combat == null) return;

            int bleed = (int)DynamicVars["Bleed"].BaseValue;
            foreach (var enemy in combat.Enemies.ToList())
            {
                await PowerCmd.Apply<BleedPower>(choiceContext, enemy, bleed, Owner.Creature, this, false);
            }

            await CardPileCmd.Draw(choiceContext, 1, Owner);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Bleed"].UpgradeValueBy(3m);
        }
    }
}
