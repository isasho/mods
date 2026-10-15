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
    // 凝血 — 1费：失去2点生命，获得8格挡，对所有敌人施加2层流血。升级后+3格挡+1流血。
    public sealed class Coagulate : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new BlockVar(8m, ValueProp.Move),
            new BleedVar(2m)
        ];

        public Coagulate()
            : base(1, CardType.Skill, CardRarity.Common, TargetType.Self) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.Damage(choiceContext, Owner.Creature, 2,
                ValueProp.Unblockable | ValueProp.Unpowered, this);

            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

            var combat = cardPlay.Card?.CombatState;
            if (combat == null) return;

            int bleed = (int)DynamicVars["Bleed"].BaseValue;
            foreach (var enemy in combat.Enemies.ToList())
                await PowerCmd.Apply<BleedPower>(choiceContext, enemy, bleed, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(3m);
            DynamicVars["Bleed"].UpgradeValueBy(1m);
        }
    }
}
