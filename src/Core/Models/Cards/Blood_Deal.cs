using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Core;
using sts2mod.Powers;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 鲜血交易 — 1费：失去3生命，获得6格挡，对随机敌人施加3层流血。升级后格挡+3流血+2。
    public sealed class Blood_Deal : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new BlockVar(6m, ValueProp.Move),
            new BleedVar(3m)
        ];

        public Blood_Deal()
            : base(1, CardType.Skill, CardRarity.Common, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.Damage(choiceContext, Owner.Creature, 3,
                ValueProp.Unblockable | ValueProp.Unpowered, this);

            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

            int bleed = (int)DynamicVars["Bleed"].BaseValue;
            var combat = cardPlay.Card?.CombatState;
            if (combat != null && combat.Enemies.Count > 0)
            {
                var target = combat.Enemies[Owner.RunState.Rng.CombatCardGeneration.NextInt(combat.Enemies.Count)];
                await PowerCmd.Apply<BleedPower>(choiceContext, target, bleed, Owner.Creature, this, false);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(3m);
            DynamicVars["Bleed"].UpgradeValueBy(2m);
        }
    }
}
