using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Field : MonoBehaviour
{
    public int row, column;
    public GameObject tilePrefab;
    public Material[] materials;
    public List<List<Tile>> tiles = new List<List<Tile>>();
    public List<Tile> activateTiles = new List<Tile>();
    
    public void CreateField()
    {
        //행열의 길이 만큼 체스판 생성
        for (int i = 0; i < row; i++)
        {
            tiles.Add(new List<Tile>());
            for (int j = 0; j < column; j++)
            {
                int materialNum = (i + j) % materials.Length;
                Vector3 pos = new Vector3((j - column / 2) * 10 + 5, 0, (i - row / 2) * 10 + 5);
                var tileObj = Instantiate(tilePrefab, transform);
                tileObj.transform.localPosition = pos;
                //흑백 색 바꾸기(디버그용)
                var tile = tileObj.GetComponent<Tile>();
                tileObj.GetComponent<MeshRenderer>().material = materials[materialNum];
                tile.SetField(this, j, i);
                tiles[i].Add(tile);
                activateTiles.Add(tile);
            }
        }
    }
    //해당 위치가 필드내에 존재하는 위치인지 확인
    public bool IsValidCellPos(intVector2 pos)
    {
        if(pos.x>=column || pos.x < 0)
        {
            return false;
        }
        if (pos.y >= row || pos.y < 0)
        {
            return false;
        }
        return true;
    }
    public bool IsMovable(intVector2 pos)
    {
        if (!IsValidCellPos(pos)) return false;
        var tile = GetTile(pos);
        return !tile.isOccupied;
    }
    //해당 위치의 셀을 반환
    public Tile GetTile(intVector2 pos)
    {
        if (!IsValidCellPos(pos)) return null;
        return tiles[pos.y][pos.x];
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
            list.AddRange(tiles[i]);
        }
        return list;
    }

}
