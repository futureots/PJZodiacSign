using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Entity : MonoBehaviour, IDamageable, IAttackable
{
    public Tile curTile;
    
    
    [SerializeField] BaseSkillData skillData;
    public S_BaseEntity skillInstance { get; private set; }

    private void Start()
    {
        //entitySkill = GetComponentInChildren<Skill>();
        skillInstance = (S_BaseEntity)skillData.CreateInstance();
    }

    #region Status
    public int level { get; private set; }
    public int power;
    public int maxHp;
    public int curHp;
    public int curEnergy;
    

    #region Buff
    bool isSlienced
    {
        get
        {
            if (buffList == null) return false;
            return buffList.Exists((buff) => buff.buffData is Silence);
        }
    }
    bool isRooted
    {
        get
        {
            if (buffList == null) return false;
            return buffList.Exists((buff) => buff.buffData is Root);
        }
    }
    bool isProtected 
    {
        get
        {
            if (buffList == null) return false;
            return buffList.Exists((buff) => buff.buffData is Protect);
        }
    }


    List<BuffInstance> buffList;
    public void AddBuff(BuffData buff, int count)
    {
        if (buffList == null)
        {
            buffList = new List<BuffInstance>();
        }
        var existBuff = buffList.Find((x) => x.buffData.GetType() == buff.GetType());
        if (existBuff != null)
        {
            existBuff.ExtendBuff(this, count);
        }
        else
        {
            var instance = new BuffInstance(count, buff);
            buffList.Add(instance);
            instance.ApplyBuff(this);
        }
        //Debug.Log(buffList.Count);
        
    }
    public void UpdateBuff()
    {
        if (buffList == null) return;
        foreach (var buff in buffList)
        {
            buff.UpdateBuff(this);
        }
    }
    public void RemoveBuff()
    {
        if (buffList == null) return;
        var list = buffList.Where((buff) => buff.IsExpired()).ToList();
        foreach (var buff in list)
        {
            buff.RemoveBuff(this);
            buffList.Remove(buff);
        }
    }

    #endregion

    
    public void Attack()
    {
        if (isSlienced) return;
        ///Debug.Log($"{name}이 공격");
        var list = GetAttackArea();
        int damage = power;
        
        foreach (var item in list)
        {
            if (item.isEmpty) continue;
            var target = item.occupiedObject;
            if(target.tag != tag || target.tag == "Obstacle")
            {
                target.GetComponent<IDamageable>()?.Damaged(power);
            }
        }
    }
    public void Damaged(int damage)
    {
        var value = damage;
        // 데미지 경감
        if (isProtected) value /= 2;

        curHp -= value;
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

    /// <summary>
    /// 기물 이동(속박 적용 시 이동 X)
    /// </summary>
    /// <param name="tile">이동할 지점</param>
    /// <param name="ignoreArea">false : 이동 범위 내 지점만 이동, true : 범위 상관 없이 이동</param>
    /// <param name="ignoreOccupy">false : 도착 지점이 비었을 경우에만 이동, true : 이동 및 점거중인 객체 파괴</param>
    /// <returns></returns>
    public bool MoveSequence(Tile tile , bool ignoreArea = false, bool ignoreOccupy = false)
    {
        if(isRooted) return false;
        if (!ignoreArea)
        {
            var isInArea = GetMoveArea().Contains(tile);
            if (!isInArea) return false;
        }
        if (!ignoreOccupy)
        {
            var isOccupied = !tile.isEmpty;
            if (isOccupied) return false;
        }
        MoveTo(tile);
        return true;
    }

    /// <summary>
    /// 해당 타일로 이동(해당 타일의 점령 여부, 기물의 이동 범위 고려 X)
    /// </summary>
    /// <param name="tile">이동할 지점</param>
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


}