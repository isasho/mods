namespace sts2mod.Core
{
    // 多段攻击信息 — 供卡面追加行（CardDescriptionPatch）精确计算流血总量。
    // 祭坛·流血经 AfterDamageGiven 按「伤害段数」触发，血冕王座对「每一次」流血施加额外触发，
    // 二者在多段攻击中都会按段数累计，故单卡需声明这两项。
    public interface IRepeatAttackInfo
    {
        // 对单个目标造成的伤害段数。
        int HitsPerTarget { get; }

        // 卡牌自身对单个目标施加流血的次数（无流血变量的卡为 0）。
        int BleedAppliesPerTarget { get; }
    }
}
