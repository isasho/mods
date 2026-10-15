using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

using sts2mod.Powers;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 狂怒注入 — 1费技能：从3个选项中选择1个执行。升级后不消耗。
    public sealed class Fury_Infusion : CardModel
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            IsUpgraded ? [] : [CardKeyword.Exhaust];

        public Fury_Infusion()
            : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None) { }

        protected override void OnUpgrade()
        {
            RemoveKeyword(CardKeyword.Exhaust);
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ICombatState combatState = Owner.Creature.CombatState;
            var choiceCards = new List<CardModel>
            {
                combatState.CreateCard(ModelDb.Card<Fury_Infusion_Feast>(), Owner),
                combatState.CreateCard(ModelDb.Card<Fury_Infusion_Catalyst>(), Owner),
            };

            // 有痛楚时才提供"痛楚兑现"，避免 0 痛楚时选了空放。
            if (Owner.Creature.GetPowerAmount<CrimsonRagePower>() > 0)
                choiceCards.Insert(0, combatState.CreateCard(ModelDb.Card<Fury_Infusion_Convert>(), Owner));

            var chosen = await CardSelectCmd.FromChooseACardScreen(choiceContext, choiceCards, Owner, false);

            if (chosen is Fury_Infusion_Convert)
                await ExecuteConvert(choiceContext);
            else if (chosen is Fury_Infusion_Feast)
                await ExecuteFeast(choiceContext);
            else if (chosen is Fury_Infusion_Catalyst)
                await ExecuteCatalyst(choiceContext);
        }

        private async Task ExecuteConvert(PlayerChoiceContext choiceContext)
        {
            var rage = Owner.Creature.GetPower<CrimsonRagePower>();
            int stacks = rage != null ? (int)rage.Amount : 0;
            if (stacks > 0)
                await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, stacks, Owner.Creature, this, false);
        }

        private async Task ExecuteFeast(PlayerChoiceContext choiceContext)
        {
            await PowerCmd.Apply<CritFeastPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this, false);
        }

        private async Task ExecuteCatalyst(PlayerChoiceContext choiceContext)
        {
            await PowerCmd.Apply<CrimsonCatalystPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this, false);

            var power = Owner.Creature.GetPower<CrimsonCatalystPower>();
            if (power != null)
                power.RefreshDescription();
        }
    }
}
