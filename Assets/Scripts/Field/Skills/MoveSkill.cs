using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSkill : MonoBehaviour//,ISkill
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
        }
        catch
        {
            throw new Exception("Skill values arenot completed");
        }

    }

    public bool IsActable()
    {
        if (entity == null || tile == null) return false;
        return true;
    }

    public void Reinitialize()
    {
        entity = null;
        tile = null;
    }
}
