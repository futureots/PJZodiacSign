using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class DebugSkillLogic : BaseSkillLogic
{
    public DebugSkillLogic()
    {
        tiles = new List<Tile>();
    }
    List<Tile> tiles;
    public override IEnumerator ExecuteSkill()
    {
        EditorLogger.Print($"SkillStart : {tiles.Count}");
        yield return new WaitForSeconds(1);
        foreach (Tile tile in tiles)
        {
            EditorLogger.Print($"tile : {tile.fieldPos}");
        }
        EditorLogger.Print("SkillEnd");
        tiles.Clear();
    }

    public override IEnumerator InputSkill(IInput input, Action<bool> callback)
    {
        var fieldTiles = StageManager.Instance.field.GetTiles();
        fieldTiles = fieldTiles.Where((_tile) => _tile.fieldPos.x % 2 == 0).ToList();
        List<Tile> _tiles = new List<Tile>();
        bool isContinued = true;
        Action<Tile> action = (tile) => { 
            _tiles.Add(tile);
            EditorLogger.Print(_tiles.Count);
        };
        Action<bool> conti = (flag) => { isContinued = flag; };
        yield return component.StartCoroutine(input.InputTile(fieldTiles, action, conti, 2));
        if (!isContinued)
        {
            callback?.Invoke(false);
            yield break;
        }


        EditorLogger.Print("InputComplete");
        tiles = _tiles;
        callback?.Invoke(true);
        yield break;
    }

    public override BaseSkillLogic Clone()
    {
        return new DebugSkillLogic();
    }
}
