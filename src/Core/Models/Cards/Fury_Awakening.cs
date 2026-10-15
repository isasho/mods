using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using sts2mod.Powers;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 狂怒觉醒 — 1费技能：失去3点生命，获得5层痛楚。消耗。升级后+2层。
    public sealed class Fury_Awakening : CardModel
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        public Fury_Awakening()
            : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.Damage(choiceContext, Owner.Creature, 3,
                ValueProp.Unblockable | ValueProp.Unpowered, this);

            int stacks = IsUpgraded ? 7 : 5;
            await PowerCmd.Apply<CrimsonRagePower>(choiceContext, Owner.Creature, stacks, Owner.Creature, this, false);
        }
    }
}
