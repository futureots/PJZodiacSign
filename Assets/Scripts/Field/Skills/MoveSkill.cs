using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MoveSkill : MonoBehaviour,ISkill
{
    [SkillTarget("대상 기물을 선택하세요.")]
    public Entity entity;
    [SkillTarget("대상 타일을 선택하세요.")]
    public Tile tile;

    //스킬 발동
    public void Activate()
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
    public bool IsValidInput(FieldInfo field)
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
    public void Reinitialize()
    {
        entity = null;
        tile = null;
    }
}
