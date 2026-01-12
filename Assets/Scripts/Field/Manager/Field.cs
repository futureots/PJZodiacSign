using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Field : MonoBehaviour
{
    [Header("Field Shape")]
    public GameObject[] tilePrefab;
    /**
     * 필드 1개를 관리
     * - 타일 머터리얼
     * - 타일 크기 및 각 객체
     */
    
    [Header("Materials")]
    public List<Material> tileMaterial;

    public int row, column;
    [SerializeField] float tileDistance;
    

    private Tile[,] _tiles;
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

        float halfDistance = tileDistance / 2;
        for (int i = 0; i < row; i++)
        {
            var temp = new Row<Tile>();
            _tileList.Add(temp);
            for (int j = 0; j < column; j++)
            {
                Vector3 pos = new Vector3((j - column / 2) * tileDistance + halfDistance, 0, (i - row / 2) * tileDistance + halfDistance);
                var tileObj = Instantiate(tilePrefab[(j + i) % tilePrefab.Length], transform);
                tileObj.transform.localPosition = pos;
                var tile = tileObj.GetComponent<Tile>();
                tile.InitializeTile(this, j, i);
                temp.list.Add(tile);
            }
        }
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
    public void ResetField()
    {
        foreach (var tile in tiles)
        {
            if (tile.isEmpty) continue;
            tile.ClearOccupant();
        }
    }
    /// <summary>
    /// 사망한 오브젝트 제거(장애물 포함)
    /// </summary>
    public void RemoveDeadEntities()
    {
        foreach (var tile in tiles)
        {
            if (tile.isEmpty) continue;
            // 타일에 존재하는 기물의 수가 1개 이상이면 마지막에 들어온 객체 제외하고 전부 삭제
            var obj = tile.occupiedObject.GetComponent<IDamageable>();
            if (obj.IsZero())
            {
                tile.UnsetOccupant();
                obj.Dead();
            }
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
    /// 기준점에서 상대위치 타일 가져오기
    /// </summary>
    public List<Tile> GetTiles(intVector2 origin, List<intVector2> vectors,  Func<Tile,bool> query = null)
    {
        query ??= _ => true;
        var list = new List<Tile>();
        foreach (var pos in vectors)
        {
            var tile = GetTile(origin + pos);
            if (tile == null) continue; 
            list.Add(tile);
        }
        return list.Where(query).ToList();
        
    }

    /// <summary>
    /// 기준점에서 해당 방향 타일 가져오기
    /// </summary>
    public List<Tile> GetTiles(intVector2 origin, List<intVector2> directions, bool isPierce, Func<Tile, bool> query = null)
    {
        query ??= _ => true;
        var list = new List<Tile>();
        foreach (var direction in directions)
        {
            var vector = intVector2.Zero;
            while (true)
            {
                vector += direction;
                var tile = GetTile(origin + vector);
                if (tile == null) break;
                if (tile.isEmpty || isPierce)
                {
                    list.Add(tile);
                }
                else
                {
                    break;
                }
            }

        }
        return list.Where(query).ToList();
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

    //점거 중인 오브젝트 가져오기
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

    /// <summary>
    /// 현재 필드 상태 가져오기
    /// </summary>
    /// <returns>필드 크기에 맞는 2차원 배열 반환 빈 타일은 0, 중립은 -1, 나머지는 팀 번호</returns>
    public int[,] GetFieldState()
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

    public int[,] GetFieldState(Occupant occupant)
    {
        var list = GetFieldState();
        if(occupant.CurTile.field == this)
        {
            list[occupant.CurTile.fieldPos.y, occupant.CurTile.fieldPos.x] = 0;
        }
        return list;
    }

    /// <summary>
    /// 상대(중립 포함)의 공격범위 및 예상 데미지 계산 후 반환
    /// </summary>
    /// <param name="fieldInfo">2차원 배열</param>
    /// <param name="teamNum">팀 번호</param>
    /// <returns>해당 팀의 상대(중립 포함)의 공격 범위, 데미지 반환</returns>
    public int[,] CalculateEnemyThreat(int[,] fieldInfo, int teamNum = 0)
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

                int entityNum = fieldInfo[entity.CurTile.fieldPos.y, entity.CurTile.fieldPos.x];
                fieldInfo[entity.CurTile.fieldPos.y, entity.CurTile.fieldPos.x] = 0;

                // 기물의 공격력 반환
                int power = 0;
                if(entity.TryGetComponent<PowerComponent>(out var component))
                {
                    power = component.Power;
                }

                if(entity.TryGetComponent<AreaComponent>(out var area))
                {
                    foreach (var vec in area.GetAttackVector(fieldInfo, new intVector2(j, i), entity.IsReflect))
                    {
                        field[vec.y, vec.x] -= power;
                    }
                }
                fieldInfo[entity.CurTile.fieldPos.y, entity.CurTile.fieldPos.x] = entityNum;
            }
        }
        return field;
    }

    /// <summary>
    /// 해당 Vector값이 이 필드에 존재하는 값인지 확인
    /// </summary>
    /// <param name="info">타일 2차원 배열</param>
    /// <param name="pos">위치</param>
    /// <returns>배열에 존재하면 true, 배열 밖 값이면 false 반환</returns>
    public static bool IsPositionValid(int[,] info, intVector2 pos)
    {
        var height = info.GetLength(0);
        var width = info.GetLength(1);

        return pos.y >= 0 && pos.y < height && pos.x >= 0 && pos.x < width;
    }

    /// <summary>
    /// 타일 리스트의 점거되지 않은 빈 타일 리스트 반환
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<Tile> GetEmptyTiles(List<Tile> list)
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
