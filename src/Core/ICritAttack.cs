namespace sts2mod.Core
{
    // 可暴击攻击卡接口 — 单卡额外暴击率叠加在基础暴击率（能力）之上。
    public interface ICritAttack
    {
        // 单卡额外暴击率（如 Savage_Blow +35）
        int CritBonusRate { get; }

        // 是否可暴击（如 Blood_Burst 未升级不可暴击）
        bool CanCrit => true;

        // 暴击伤害倍率（如 Critical_Blow 三倍）
        decimal CritMultiplier(int critRate) => CritManager.GetCritMultiplier(critRate);
    }
}
