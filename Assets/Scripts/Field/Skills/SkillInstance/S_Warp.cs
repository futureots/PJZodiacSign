using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class S_Warp : BaseSkillInstance<SD_Warp>
{
    [SkillTarget("대상 기물을 선택하세요.")]
    public Entity entity;
    [SkillTarget("대상 타일을 선택하세요.")]
    public Tile tile;

    public S_Warp(SD_Warp data) : base(data)
    {
    }



    public override void Activate()
    {
        entity.Move(tile);
    }

    public override bool IsValidInput(FieldInfo field)
    {
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
        if (!tile.isEmpty) return false;
        return true;
    }
    

    public override bool CanSkillInput(Field field)
    {
        var entities = field.GetOccupiedObjects();
        var list = field.GetFieldState();
        foreach (var item in list)
        {
            if (item == 0) return true;
        }
        return false;
    }

    public override bool SetSkillInput(Field field)
    {
        var tiles = field.GetTiles();
        var moveArea = tiles.Where(x => x.isEmpty).ToArray();

        var entities = field.GetOccupiedObjects();
        var moveEntities = new List<Entity>();
        foreach (var item in entities)
        {
            var entity = item.GetComponent<Entity>();
            if(entity == null) continue;
            moveEntities.Add(entity);
        }

        if (moveArea.Length == 0 || moveEntities.Count == 0) return false;

        entity = moveEntities[UnityEngine.Random.Range(0, moveEntities.Count)];
        tile = moveArea[UnityEngine.Random.Range(0, moveArea.Length)];
        return true;
    }
}
