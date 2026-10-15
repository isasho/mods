using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
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
    // 血祭 — 1费：失去6点生命，对所有敌人施加10层流血。升级后失去生命减少3点。
    public sealed class Blood_Sacrifice : CardModel
    {
        protected override List<DynamicVar> CanonicalVars => [
            new DynamicVar("HpLoss", 6m),
            new BleedVar(10m)
        ];

        public Blood_Sacrifice()
            : base(
                1,
                CardType.Skill,
                CardRarity.Uncommon,
                TargetType.None)
        {
        }

        protected override async Task OnPlay(
            PlayerChoiceContext choiceContext,
            CardPlay cardPlay)
        {
            decimal hpLoss = DynamicVars["HpLoss"].BaseValue;

            await CreatureCmd.Damage(
                choiceContext,
                Owner.Creature,
                (int)hpLoss,
                ValueProp.Unblockable |
                ValueProp.Unpowered,
                this);

            var combat = cardPlay.Card?.CombatState;
            if (combat == null)
                return;

            foreach (var enemy in combat.Enemies.ToList())
            {
                await PowerCmd.Apply<BleedPower>(choiceContext,
                    enemy,
                    (int)DynamicVars["Bleed"].BaseValue,
                    Owner.Creature,
                    this, false);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["HpLoss"].UpgradeValueBy(-3m);
        }
    }
}
