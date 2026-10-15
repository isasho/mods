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
    // 血河 — 2费：所有敌人获得12层流血。升级后16层。
    public sealed class Blood_River : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new BleedVar(12m)
        ];

        public Blood_River()
            : base(
                2,
                CardType.Skill,
                CardRarity.Rare,
                TargetType.AllEnemies)
        {
        }

        protected override async Task OnPlay(
            PlayerChoiceContext choiceContext,
            CardPlay cardPlay)
        {
            var combat = cardPlay.Card?.CombatState;
            if (combat == null)
                return;

            int bleed = (int)DynamicVars["Bleed"].BaseValue;
            foreach (var enemy in combat.Enemies.ToList())
            {
                await PowerCmd.Apply<BleedPower>(choiceContext, enemy, bleed, Owner.Creature, this, false);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Bleed"].UpgradeValueBy(4m);
        }
    }
}
