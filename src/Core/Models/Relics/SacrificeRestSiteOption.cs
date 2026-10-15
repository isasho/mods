using System.Threading.Tasks;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace sts2mod.Relics
{
    public enum SacrificeType
    {
        Pain,
        Bleed,
        Crit,
    }

    // 献祭 — 火堆选项：永久失去6点最大生命值，从三项增益中选一项叠加。
    public sealed class SacrificeRestSiteOption : RestSiteOption
    {
        private readonly SacrificialAltar _altar;
        private readonly SacrificeType _type;

        public override string OptionId => "SACRIFICE_" + _type.ToString().ToUpperInvariant();

        public override bool IsEnabled => Owner.Creature.MaxHp > SacrificialAltar.SacrificeHpCost;

        public SacrificeRestSiteOption(Player owner, SacrificialAltar altar, SacrificeType type)
            : base(owner)
        {
            _altar = altar;
            _type = type;
        }

        public override async Task<bool> OnSelect()
        {
            if (Owner.Creature.MaxHp <= SacrificialAltar.SacrificeHpCost)
                return false;

            await CreatureCmd.LoseMaxHp(
                new ThrowingPlayerChoiceContext(),
                Owner.Creature,
                SacrificialAltar.SacrificeHpCost,
                isFromCard: false);

            switch (_type)
            {
                case SacrificeType.Pain:
                    _altar.PainSacrificeCount++;
                    break;
                case SacrificeType.Bleed:
                    _altar.BleedSacrificeCount++;
                    break;
                case SacrificeType.Crit:
                    _altar.CritSacrificeCount++;
                    break;
            }

            return true;
        }
    }
}
