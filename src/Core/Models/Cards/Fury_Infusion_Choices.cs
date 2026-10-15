using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace sts2mod.Cards
{
    // 痛楚兑现 — 0费抉择牌：获得等同于痛楚层数的力量。消耗。
    public sealed class Fury_Infusion_Convert : CardModel
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        public override CardPoolModel Pool => ModelDb.CardPool<TokenCardPool>();

        public Fury_Infusion_Convert()
            : base(0, CardType.Skill, CardRarity.Token, TargetType.None) { }

        protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
            => Task.CompletedTask;
    }

    // 暴击盛宴 — 0费抉择牌：本回合每次暴击获得1点力量。消耗。
    public sealed class Fury_Infusion_Feast : CardModel
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        public override CardPoolModel Pool => ModelDb.CardPool<TokenCardPool>();

        public Fury_Infusion_Feast()
            : base(0, CardType.Skill, CardRarity.Token, TargetType.None) { }

        protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
            => Task.CompletedTask;
    }

    // 痛楚催化 — 0费抉择牌：本回合获得的痛楚层数翻倍。消耗。
    public sealed class Fury_Infusion_Catalyst : CardModel
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        public override CardPoolModel Pool => ModelDb.CardPool<TokenCardPool>();

        public Fury_Infusion_Catalyst()
            : base(0, CardType.Skill, CardRarity.Token, TargetType.None) { }

        protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
            => Task.CompletedTask;
    }
}
