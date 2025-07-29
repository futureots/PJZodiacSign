using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class Field : MonoBehaviour
{
    public int row, column;
    public GameObject tilePrefab;

    Tile[,] _tiles;
    public Tile[,] tiles
    {
        get
        {
            if(_tiles != null) return _tiles;
            _tiles = new Tile[row, column];
            for(int i = 0; i < row; i++)
            {
                for(int j=0;j< column; j++)
                {
                    _tiles[i, j] = _tileList[i].list[j];
                }
            }
            return tiles;
        }
    }
    
    [ContextMenuItem("CreateField","CreateField")]
    [ContextMenuItem("DestroyField", "DestroyField")]
    public List<Row<Tile>> _tileList;

    #region Field
    /// <summary>
    /// 필드 생성
    /// </summary>
    public void CreateField()
    {
        _tileList = new List<Row<Tile>>();
        //행,열의 길이 만큼 체스판 생성
        //tiles = new Tile[row, column];
        for (int i = 0; i < row; i++)
        {
            var temp = new Row<Tile>();
            _tileList.Add(temp);
            for (int j = 0; j < column; j++)
            {
                Vector3 pos = new Vector3((j - column / 2) * 10 + 5, 0, (i - row / 2) * 10 + 5);
                var tileObj = Instantiate(tilePrefab, transform);
                tileObj.transform.localPosition = pos;
                var tile = tileObj.GetComponent<Tile>();
                tile.SetField(this, j, i);
                temp.list.Add(tile);
            }
        }
        Debug.Log(tiles.Length);
    }

    /// <summary>
    /// 필드 제거
    /// </summary>
    public void DestroyField()
    {
        foreach (Row<Tile> tile in _tileList)
        {
            foreach (var item in tile.list)
            {
                DestroyImmediate(item.gameObject);
            }
            tile.list.Clear();
        }
        _tileList.Clear();
    }
    /// <summary>
    /// 필드위의 모든 기물 제거(장애물 포함)
    /// </summary>
    public void EraseField()
    {
        foreach (var tile in tiles)
        {
            if (tile.isEmpty) continue;
            tile.ClearBufferedObjects();
            tile.DestroyOccupiedObject();
        }
    }
    /// <summary>
    /// 사망한 오브젝트 제거(장애물 포함)
    /// </summary>
    public void CleanField()
    {
        foreach (var tile in tiles)
        {
            if (tile.isEmpty) continue;
            // 타일에 존재하는 기물의 수가 1개 이상이면 마지막에 들어온 객체 제외하고 전부 삭제
            var obj = tile.occupiedObject.GetComponent<IDamageable>();
            if (obj.isZero())
            {
                tile.OccupyObject(null);
                obj.Dead();
            }
            tile.ClearBufferedObjects();
        }
    }

    #endregion

    #region Tile
    // 해당 위치가 필드내에 존재하는 위치인지 확인
    public bool IsValidCellPos(intVector2 pos)
    {
        if(pos.x>=column || pos.x < 0 || pos.y >= row || pos.y < 0 || tiles[pos.y,pos.x] == null)
        {
            return false;
        }
        return true;
    }
    // 해당 위치의 셀을 반환
    public Tile GetTile(intVector2 pos, bool isReflect = false)
    {
        var fieldPos = pos;
        if (isReflect)
        {
            fieldPos = new intVector2(row - pos.x - 1, column - pos.y - 1);
        }
        
        if (!IsValidCellPos(fieldPos)) return null;
        return tiles[fieldPos.y, fieldPos.x];
    }
    /// <summary>
    /// 필드에 있는 모든 타일 가져오기
    /// </summary>
    public List<Tile> GetTiles()
    {
        var list = new List<Tile>();
        for(int i = 0; i < row; i++)
        {
            for(int j = 0; j < column; j++)
            {
                list.Add(tiles[i, j]);
            }
        }
        return list;
    }

    //해당 위치 타일들 가져오기
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

    /// <summary>
    /// 맨 절반의 타일을 가져오기
    /// </summary>
    /// <param name="isReflect">true = 적 측, false = 플레이어 측</param>
    /// <returns></returns>
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
    #endregion

    //공격 가능한 오브젝트 가져오기
    public List<GameObject> GetOccupiedObjects()
    {
        List<GameObject> list = new();
        foreach(var tile in tiles)
        {
            if (tile.isEmpty) continue;
            var occupiedObj = tile.occupiedObject;
            if (occupiedObj != null)
            {
                list.Add(occupiedObj);
            }
        }
        return list;
    }

    public int[,] GetFieldInfo()
    {
        var field = new int[row, column];
        for(int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++) 
            {
                var t = tiles[i,j];
                if (t.isEmpty) continue;
                else
                {
                    var team = t.occupiedObject.GetComponent<Team>();
                    if (team == null) field[i, j] = -1;
                    else
                    {
                        field[i, j] = team.teamNumber;
                    }
                }
            }
        }
        return field;
    }

    public int[,] GetOtherTileValues(int[,] fieldInfo, int teamNum = 0)
    {
        var field = new int[row, column];
        for(int i = 0; i < row; i++)
        {
            for(int j = 0; j < column; j++)
            {
                if (fieldInfo[i, j] == 0) continue;
                var obj = tiles[i, j].occupiedObject;
                var entity = obj.GetComponent<Entity>();
                if (entity == null) continue;
                if (entity.team.teamNumber == teamNum) continue;
                foreach(var vec in entity.GetAttackVector(fieldInfo,new intVector2(j,i)))
                {
                    field[vec.y, vec.x] -= entity.power;
                }

            }
        }
        return field;
    }
    public static bool isValidPos(int[,] info, intVector2 pos)
    {
        var height = info.GetLength(0);
        var width = info.GetLength(1);

        return pos.y >= 0 && pos.y < height && pos.x >= 0 && pos.x < width;
    }

    public static List<Tile> GetEmptyTile(List<Tile> list)
    {
        var emptyTiles = new List<Tile>();
        foreach (var tile in list)
        {
            if (tile.isEmpty)
            {
                emptyTiles.Add(tile);
            }
        }
        return emptyTiles;
    }
}
[System.Serializable]
public class Row<T>
{
    public Row()
    {
        list = new List<T>();
    }
    public List<T> list;
}
