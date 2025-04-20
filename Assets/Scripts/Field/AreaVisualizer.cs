using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AreaVisualizer : MonoBehaviour
{
    Field field
    {
        get
        {
            return Field.Instance;
        }
    }
    /// <summary>
    /// 예상 적 공격 범위 표시 메테리얼
    /// </summary>
    public Material enemyAttackMaterial;

    /// <summary>
    /// 선택한 엔티티 예상 공격범위 표시 메테리얼
    /// </summary>
    public Material attackMaterial;

    /// <summary>
    /// 선택한 엔티티 이동 가능범위 표시 메테리얼
    /// </summary>
    public Material moveMaterial;

    List<Tile> highlightedTiles = new List<Tile>();

    public void ShowMoveArea(List<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            tile.AddColor(moveMaterial);
            highlightedTiles.Add(tile);
        }
    }
    public void RemoveMoveArea(List<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            tile.RemoveColor(moveMaterial);
            highlightedTiles.Remove(tile);
        }
    }
    public void ShowAttackArea(List<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            tile.AddColor(attackMaterial);
            highlightedTiles.Add(tile);
        }
    }
    public void RemoveAttackArea(List<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            tile.RemoveColor(attackMaterial);
            highlightedTiles.Remove(tile);
        }
    }

}

