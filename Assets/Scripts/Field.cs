using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                Vector3 pos = new Vector3(j * 10, 0, i * 10);
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
    //필드 공격 행동
    public void FieldAction()
    {
        List<Tile> entityTile = new List<Tile>();
        foreach (var cells in tiles)
        {
            foreach (var cell in cells)
            {
                if (cell.isOccupied)
                {
                    //계산이 필요한 셀 추가
                    entityTile.Add(cell);
                    //셀 세팅(공격, 강화, 힐)
                    cell.CellSetting();
                }
            }
        }
        //순서대로 강화, 힐, 공격 순
        Entity.ActionType[] triggers = {Entity.ActionType.Enhance, Entity.ActionType.Heal, Entity.ActionType.Attack };
        foreach (var trigger in triggers)
        {
            foreach (var cell in entityTile)
            {
                cell.CellActivate(trigger);
            }
        }
        foreach (var cell in entityTile)
        {
            cell.CheckCell();
            if (cell.isOccupied)
            {
                cell.SetOriginStat();
            }
        }
    }
}
