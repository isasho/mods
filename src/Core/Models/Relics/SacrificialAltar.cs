using System.Collections.Generic;
using System.Threading.Tasks;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Saves.Runs;

using sts2mod.Core;
using sts2mod.Powers;

namespace sts2mod.Relics
{
    // 献祭祭坛（稀有）— 火堆处多出三个「献祭」选项（痛楚/流血/暴击），每次永久失去6点最大生命值，
    // 从三项中选一项获得本局永久增益，可重复叠加：
    //   痛楚：每场战斗开始+2层；流血：攻击命中+3层；暴击：基础暴击率+10%。
    public sealed class SacrificialAltar : RelicModel
    {
        public const int SacrificeHpCost = 6;
        public const decimal PainPerSacrifice = 2m;
        public const decimal BleedPerSacrifice = 3m;
        public const decimal CritPerSacrifice = 10m;

        private int _painCount;
        private int _bleedCount;
        private int _critCount;

        public override RelicRarity Rarity => RelicRarity.Rare;

        public override bool ShowCounter => true;

        public override int DisplayAmount => PainSacrificeCount + BleedSacrificeCount + CritSacrificeCount;

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new DynamicVar("PainSac", 0m),
            new DynamicVar("BleedSac", 0m),
            new DynamicVar("CritSac", 0m)
        ];

        [SavedProperty]
        public int PainSacrificeCount
        {
            get => _painCount;
            set { AssertMutable(); _painCount = value; SyncVars(); InvokeDisplayAmountChanged(); }
        }

        [SavedProperty]
        public int BleedSacrificeCount
        {
            get => _bleedCount;
            set { AssertMutable(); _bleedCount = value; SyncVars(); InvokeDisplayAmountChanged(); }
        }

        [SavedProperty]
        public int CritSacrificeCount
        {
            get => _critCount;
            set { AssertMutable(); _critCount = value; SyncVars(); InvokeDisplayAmountChanged(); }
        }

        private void SyncVars()
        {
            SyncVar("PainSac", PainPerSacrifice * PainSacrificeCount);
            SyncVar("BleedSac", BleedPerSacrifice * BleedSacrificeCount);
            SyncVar("CritSac", CritPerSacrifice * CritSacrificeCount);
        }

        private void SyncVar(string name, decimal target)
        {
            var dynVar = DynamicVars[name];
            decimal delta = target - dynVar.BaseValue;
            if (delta != 0m)
            {
                dynVar.UpgradeValueBy(delta);
                dynVar.FinalizeUpgrade();
            }
        }

        public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
        {
            if (player != base.Owner)
                return false;

            options.Add(new SacrificeRestSiteOption(player, this, SacrificeType.Pain));
            options.Add(new SacrificeRestSiteOption(player, this, SacrificeType.Bleed));
            options.Add(new SacrificeRestSiteOption(player, this, SacrificeType.Crit));
            return true;
        }

        public override async Task BeforeCombatStart()
        {
            if (PainSacrificeCount <= 0 && BleedSacrificeCount <= 0 && CritSacrificeCount <= 0)
                return;

            var creature = base.Owner.Creature;
            var ctx = new ThrowingPlayerChoiceContext();

            Flash();

            if (PainSacrificeCount > 0)
                await PowerCmd.Apply<CrimsonRagePower>(ctx, creature, PainPerSacrifice * PainSacrificeCount, creature, null, false);
            if (BleedSacrificeCount > 0)
                await PowerCmd.Apply<SacrificialAltarBleedPower>(ctx, creature, BleedPerSacrifice * BleedSacrificeCount, creature, null, false);
            if (CritSacrificeCount > 0)
                await PowerCmd.Apply<SacrificialAltarCritPower>(ctx, creature, CritPerSacrifice * CritSacrificeCount, creature, null, false);

            await CritManager.EnsureCritDisplay(creature);
            await CritManager.EnsureCritSystem(creature);
        }
    }
}
