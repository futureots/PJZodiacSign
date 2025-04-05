using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Outline;
using static UnityEngine.EventSystems.EventTrigger;



public class Field : MonoBehaviour
{
    public int row, column;
    public GameObject tilePrefab;
    public Tile[,] tiles;

    private void Start()
    {
        CreateField();
    }
    [ContextMenu("CreateField")]
    public void CreateField()
    {
        //행,열의 길이 만큼 체스판 생성
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
        Debug.Log(tiles.Length);
    }


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
    // 사망한 엔티티 제거(장애물 포함)
    public void CleanField()
    {
        foreach (var tile in tiles)
        {
            if (!tile.isOccupied) continue;
            var health = tile.occupiedObject.GetComponent<Health>();
            Debug.Log("health : " + health.hp);
            if (health.isZero())
            {
                health.OnDead?.Invoke();
            }
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
