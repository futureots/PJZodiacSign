using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AreaVisualizer : MonoBehaviour
{

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
            tile.ApplyHighlight(moveMaterial);
            highlightedTiles.Add(tile);
        }
    }
    public void RemoveMoveArea(List<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            tile.RemoveHighlight(moveMaterial);
            highlightedTiles.Remove(tile);
        }
    }
    public void ShowAttackArea(List<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            tile.ApplyHighlight(attackMaterial);
            highlightedTiles.Add(tile);
        }
    }
    public void RemoveAttackArea(List<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            tile.RemoveHighlight(attackMaterial);
            highlightedTiles.Remove(tile);
        }
    }
    public void RemoveAllArea()
    {
        var tiles = highlightedTiles.Distinct().ToList();
        foreach (Tile tile in tiles)
        {
            tile.RemoveHighlight(attackMaterial);
            tile.RemoveHighlight(moveMaterial);
        }
        highlightedTiles.Clear();
    }
}

