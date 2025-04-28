using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Entity : MonoBehaviour, IDamageable,IAttackable
{
    public Tile curTile;

    #region status
    public EntityStatus status;
    public int level;
    public int Power
    {
        get { return status.power.GetStatus(); }
    }
    public int maxHp
    {
        get { return status.maxHp.GetStatus(); }
    }
    public int curHp;
    public int curEnergy;




    public void Attack()
    {
        Debug.Log($"{name}ÀÌ °ø°Ý");
        var list = GetAttackArea();
        foreach (var item in list)
        {
            if (item.isEmpty) continue;
            var target = item.occupiedObject;
            if(target.tag != tag || target.tag == "Obstacle")
            {
                target.GetComponent<IDamageable>()?.Damaged(Power);
            }
        }
    }
    public void Damaged(int damage)
    {
        curHp -= damage;
        Debug.Log($"Damaged : {damage} , CurrentHp : {curHp}");
    }

    public void Dead()
    {
        Debug.Log(gameObject+"Dead");
        Destroy(gameObject);
    }

    public void Healed(int amount)
    {
        curHp += amount;
    }

    public bool isZero()
    {
        if (curHp > 0) return false;
        return true;
    }
    #endregion


    public bool MoveSequence(Tile tile)
    {
        var isMovable = GetMoveArea().Contains(tile) && !tile.isEmpty;
        if (isMovable)
        {
            MoveTo(tile);
        }
        return true;
    }
    public void MoveTo(Tile tile)
    {
        if (curTile != null)
        {
            curTile.OccupyObject();
        }
        tile.OccupyObject(gameObject);
        curTile = tile;
        transform.position = tile.transform.position;
    }

    public List<Tile> GetMoveArea()
    {
        var list = GetComponents<IMoveArea>();
        var tiles = new List<Tile>();
        foreach (var area in list)
        {
            tiles.AddRange(area.GetMoveArea(curTile));
        }
        tiles.Add(curTile);
        return tiles;
    }
    public List<Tile> GetAttackArea(Tile tile)
    {
        var list = GetComponents<IAttackArea>();
        var tiles = new List<Tile>();
        foreach (var area in list)
        {
            tiles.AddRange(area.GetAttackArea(tile));
        }
        return tiles;
    }
    public List<Tile> GetAttackArea()
    {
        return GetAttackArea(curTile);
    }


    #region Input
    private void OnMouseDown()
    {
        Debug.Log("Mouse DOWN");
        InputManager.Instance.OnGameObjectDown(gameObject);
    }
    private void OnMouseUp()
    {
        Debug.Log("Mouse UP");
        InputManager.Instance.OnGameObjectUp();
    }

    #endregion
}