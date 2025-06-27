using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class S_Warp : BaseSkillInstance
{
    [SkillTarget("대상 기물을 선택하세요.")]
    public Entity entity;
    [SkillTarget("대상 타일을 선택하세요.")]
    public Tile tile;

    public S_Warp(BaseSkillData data) : base(data)
    {
    }

    //스킬 발동
    public override void Activate()
    {
        entity.MoveSequence(tile,true,true);
    }

    public override bool IsActable()
    {
        Debug.Log(entity + "  " + tile);
        if (entity == null || tile == null) return false;
        return true;
    }
    public override bool IsValidInput(FieldInfo field)
    {
        Debug.Log(field.Name);
        switch (field.Name)
        {
            case nameof(entity):
                return IsValidEntity();
            case nameof(tile):
                return IsValidTile();
            default:
                return false;
        } 
    }

    bool IsValidEntity()
    {
        if (entity == null) return false;
        return true;
    }
    bool IsValidTile()
    {
        if (tile == null) return false;
        //if (!tile.isEmpty) return false;
        return true;
    }
    public override void Reinitialize()
    {
        Debug.Log("Reinitialize");
        entity = null;
        tile = null;
    }
}
