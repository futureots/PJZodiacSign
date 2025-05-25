using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MoveSkill : Skill
{
    [SkillTarget("대상 기물을 선택하세요.")]
    public Entity entity;
    [SkillTarget("대상 타일을 선택하세요.")]
    public Tile tile;

    //스킬 발동
    public override void Activate()
    {
        entity.MoveSequence(tile);
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
        if (entity.CompareTag("Player"))
        {
            return true;
        }
        return false;
    }
    bool IsValidTile()
    {
        if (!tile.isEmpty) return false;
        var list = entity.GetMoveArea();
        if (list.Contains(tile))
        {
            return true;
        }
        return false;
    }
    public override void Reinitialize()
    {
        Debug.Log("Reinitialize");
        entity = null;
        tile = null;
    }
}
