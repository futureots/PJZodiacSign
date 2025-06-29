using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class Field : MonoBehaviour
{
    public int row, column;
    public GameObject tilePrefab;
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
                    _tiles[i, j] = tileList[i].list[j];
                }
            }
            return tiles;
        }
    }
    Tile[,] _tiles;
    [ContextMenuItem("CreateField","CreateField")]
    [ContextMenuItem("ClearField", "EraseField")]
    public List<Row<Tile>> tileList;

    public void CreateField()
    {
        tileList = new List<Row<Tile>>();
        //행,열의 길이 만큼 체스판 생성
        //tiles = new Tile[row, column];
        for (int i = 0; i < row; i++)
        {
            var temp = new Row<Tile>();
            tileList.Add(temp);
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
    public void EraseField()
    {
        foreach (Row<Tile> tile in tileList)
        {
            foreach (var item in tile.list)
            {
                DestroyImmediate(item.gameObject);
            }
            tile.list.Clear();
        }
        tileList.Clear();
    }

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
    public Tile GetTile(intVector2 pos)
    {
        if (!IsValidCellPos(pos)) return null;
        return tiles[pos.y, pos.x];
    }

    public Tile GetTile(int x, int y)
    {
        var pos = new intVector2(x, y);
        return GetTile(pos);

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
            // 밀려난 오브젝트(파괴 예정 기물, 장애물 등) 삭제
            foreach (var item in tile.occupiedObjects)
            {
                var component = item.GetComponent<IDamageable>();
                if(component != null)
                {
                    component.Dead();
                }
            }
            tile.occupiedObjects.Clear();
        }
    }
    #region Visualize

    /// <summary>
    /// 예상 적 공격 범위 표시 메테리얼
    /// </summary>
    public Material dangerTileMaterial;

    /// <summary>
    /// 선택한 엔티티 예상 공격범위 표시 메테리얼
    /// </summary>
    public Material expectAttackMaterial;

    /// <summary>
    /// 선택한 엔티티 이동 가능범위 표시 메테리얼
    /// </summary>
    public Material expectMoveMaterial;

    List<Tile> entityMovableArea = new List<Tile>();
    List<Tile> entityAttackArea = new List<Tile>();
    List<Tile> enemiesAttackArea = new List<Tile>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type">0 : 이동 범위, 1 : 공격 범위, 2 : 피격범위</param>
    /// <param name="tiles"></param>
    public void AddFieldColor(int type, params Tile[] tiles)
    {
        List<Tile> list;
        Material material;
        switch (type)
        {
            case 0:
                list = entityMovableArea;
                material = expectMoveMaterial;
                break;
            case 1:
                list = entityAttackArea;
                material = expectAttackMaterial;
                break;
            case 2:
                list = enemiesAttackArea;
                material = dangerTileMaterial;
                break;
            default:
                return;
        }
        list.AddRange(tiles);
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
    public void RemoveFieldColor(int type)
    {
        List<Tile> list;
        Material material;
        switch (type)
        {
            case 0:
                list = entityMovableArea;
                material = expectMoveMaterial;
                break;
            case 1:
                list = entityAttackArea;
                material = expectAttackMaterial;
                break;
            case 2:
                list = enemiesAttackArea;
                material = dangerTileMaterial;
                break;
            default:
                return;
        }
        foreach (var tile in list)
        {
            if (tile != null)
            {
                List<Material> materials = tile.renderer.sharedMaterials.ToList();
                materials.Remove(material);
                tile.renderer.materials = materials.ToArray();
            }
        }
        list.Clear();
    }
    #endregion
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
