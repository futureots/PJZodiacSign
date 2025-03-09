using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Field : MonoBehaviour
{
    public int row, column;
    public GameObject tilePrefab;
    public Tile[,] tiles = null;
    
    public void CreateField()
    {
        //행열의 길이 만큼 체스판 생성
        tiles = new Tile[row, column];
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                Vector3 pos = new Vector3((j - column / 2) * 10 + 5, 0, (i - row / 2) * 10 + 5);
                var tileObj = Instantiate(tilePrefab, transform);
                tileObj.transform.localPosition = pos;
                var tile = tileObj.GetComponent<Tile>();
                tile.SetField(this, j, i);
                tiles[i, j] = tile;
            }
        }
    }


    //해당 위치가 필드내에 존재하는 위치인지 확인
    public bool IsValidCellPos(intVector2 pos)
    {
        if(pos.x>=column || pos.x < 0 || pos.y >= row || pos.y < 0 || tiles[pos.y,pos.x] == null)
        {
            return false;
        }
        return true;
    }
    //해당 위치의 셀을 반환
    public Tile GetTile(intVector2 pos)
    {
        if (!IsValidCellPos(pos)) return null;
        return tiles[pos.y, pos.x];
    }
    public List<Tile> GetTiles(List<intVector2> positions)
    {
        var list = new List<Tile>();
        foreach (var pos in positions)
        {
            var tile = GetTile(pos);
            if (tile == null) continue;
            list.Add(tile);
        }
        return list;
    }
    public List<Tile> GetHalfTiles(bool isReflect)
    {
        List<Tile> list = new List<Tile>();
        var start = 0;
        int end = row / 2;
        if (isReflect)
        {
            start = end;
            end = row;
        }
        for (int i = start; i < end; i++) 
        {
            for(int j = 0; j< column; j++)
            {
                list.Add(tiles[i,j]);
            }
        }
        return list;
    }


    public void AddFieldColor(Material material, params Tile[] tiles)
    {
        foreach (var tile in tiles)
        {
            if(tile != null)
            {
                List<Material> materials = tile.renderer.sharedMaterials.ToList();
                materials.Add(material);
                tile.renderer.materials = materials.ToArray(); 
            }
        }
    }
    public void RemoveFieldColor(Material material,params Tile[] tiles)
    {
        foreach (var tile in tiles)
        {
            if (tile != null)
            {
                List<Material> materials = tile.renderer.sharedMaterials.ToList();
                materials.Remove(material);
                tile.renderer.materials = materials.ToArray();
            }
        }
    }

    public void CleanField()
    {
        foreach (var tile in tiles)
        {
            if (!tile.isOccupied) continue;
            var health = tile.entityObj.GetComponent<Health>();
            Debug.Log("health : " + health.hp);
            if (health.isZero())
            {
                health.Dead?.Invoke();
            }
        }
    }
}
