using Battle;

using System;
using UnityEngine;

public class Skill : MonoBehaviour,ISkill
{
    [SkillTarget("대상 기물을 선택하세요.")]
    public Entity entity;
    [SkillTarget("대상 타일을 선택하세요.")]
    public Tile tile;

    //스킬 발동
    public virtual void Activate()
    {
        try
        {
            entity.MoveTo(tile);
            Debug.Log(tile.name + " Skill Active");
        }
        catch 
        {
            throw new Exception("Skill values arenot completed");
        }
        
    }

    public bool IsActable()
    {
        if(entity == null || tile == null) return false;
        return true;
    }

    public void Reinitialize()
    {
        entity = null;
        tile = null;
    }
}
