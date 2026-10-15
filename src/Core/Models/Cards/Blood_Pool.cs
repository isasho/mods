using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Core;
using sts2mod.Powers;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 血池 — 2费：获得12格挡，对所有敌人施加4层流血。升级后+5格挡+3流血。
    public sealed class Blood_Pool : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new BlockVar(12m, ValueProp.Move),
            new BleedVar(4m)
        ];

        public Blood_Pool()
            : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

            var combat = cardPlay.Card?.CombatState;
            if (combat == null) return;

            int bleed = (int)DynamicVars["Bleed"].BaseValue;
            foreach (var enemy in combat.Enemies.ToList())
            {
                await PowerCmd.Apply<BleedPower>(choiceContext, enemy, bleed, Owner.Creature, this, false);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(5m);
            DynamicVars["Bleed"].UpgradeValueBy(3m);
        }
    }
}
