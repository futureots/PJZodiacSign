using NUnit.Framework.Constraints;
using System.Dynamic;
using UnityEngine;

public static class SkillFactory
{
    /// <summary>
    /// 스킬 인스턴스 반환(시전자 없음)
    /// </summary>
    /// <param name="key">스킬 종류</param>
    /// <param name="data">스킬 데이터</param>
    /// <returns>스킬 인스턴스</returns>
    public static IActive CreateInstance(BaseSkillData data)
    {
        var key = data.skillKey;
        var instance = CreateInstance(data,null);
        if (instance != null) return instance;
        switch (key)
        {
            case SkillKey.Warp:
                return new S_Warp(data);
                
        }
        return null;
    }

    /// <summary>
    /// 스킬 인스턴스 반환(시전자 명시)
    /// </summary>
    /// <param name="key">스킬 종류</param>
    /// <param name="data">스킬 데이터</param>
    /// <param name="owner">스킬 시전자</param>
    /// <returns>스킬 인스턴스</returns>
    public static IActive CreateInstance(BaseSkillData data, Entity owner)
    {
        var key = data.skillKey;
        switch (key)
        {
            case SkillKey.Enpassant:
                return new S_Enpassant(data, owner);
            case SkillKey.StaleMate:
                return new S_StaleMate(data, owner);

        }
        return null;
    }
}
public enum SkillKey
{

    Enpassant = 51,
    Castling = 52,
    Tour = 53,
    Heal = 54,
    StaleMate = 55,
    CheckMate = 56,

    // 아이템 스킬
    Promotion = 1,
    Upgrade = 2,
    Relocation = 3,
    Chestron = 4,
    Warp = 5,
    Attack = 6,
}
