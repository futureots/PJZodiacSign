using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Entity : MonoBehaviour, IDamageable, IAttackable
{
    public Tile curTile { get; private set; }

    
    /// <summary>기물의 고유한 id(이름)</summary>
    public string id;

    #region Skill
    /// <summary>기물 스킬 데이터</summary>
    public BaseSkillData skillData;
    /// <summary>skillData의 인스턴스</summary>
    public IActive skillInstance;
    /// <summary>
    /// 기물의 스킬 세팅
    /// </summary>
    /// <param name="skillData">기물 스킬 데이터</param>
    public void SetSkill(BaseSkillData skillData)
    {
        this.skillData = skillData;
        skillInstance = skillData.CreateInstance();
        if(skillInstance is IOwnable entitySkill)
        {
            entitySkill.Owner = this;
        }

    }

    #endregion
    
    Team _team;
    /// <summary>기물이 속한 팀</summary>
    public Team team
    {
        get
        {
            if (_team == null)
            {
                _team = GetComponent<Team>();
            }
            return _team;
        }
    }
    // 기물의 고유 데이터
    public EntityData data;
    /// <summary>
    /// 기물의 스탯, 스킬값 세팅
    /// </summary>
    /// <param name="data">기물 데이터</param>
    /// <param name="level">기물의 레벨</param>
    public void SetEntity(EntityData data, int level =0)
    {
        this.data = data;
        id = data.id;
        this.level = level;

        // 기물 스탯 세팅
        power = data.power + data.bonusPower * level;
        maxHp = data.maxHp + data.bonusHp * level;
        curHp = maxHp;
        curEnergy = 0;

        // 기물 스킬 세팅(스킬이 엔티티 전용 스킬이면 시전자 할당, 아니면 할당X)
        SetSkill(data.skill);
        skillCost = data.skillCost;
    }

    /// <summary>사망 시 호출</summary>
    public Action OnDead;

    #region Status
    /// <summary>기물의 레벨</summary>
    public int level { get; private set; }
    /// <summary>공격력</summary>
    public int power;
    /// <summary>최대 체력</summary>
    public int maxHp;
    /// <summary>현재 체력</summary>
    public int curHp;
    /// <summary>현재 마나</summary>
    public int curEnergy;
    /// <summary>스킬 마나 소모량</summary>
    public int skillCost;
    

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
    /// <summary>
    /// 버프 추가
    /// </summary>
    /// <param name="buff">버프 종류</param>
    /// <param name="count">버프 중첩 수</param>
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
        
    }
    /// <summary>
    /// 버프 갱신
    /// </summary>
    public void UpdateBuff()
    {
        if (buffList == null) return;
        foreach (var buff in buffList)
        {
            buff.UpdateBuff(this);
        }
    }

    /// <summary>
    /// 버프 제거
    /// </summary>
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
        curEnergy += 1;
        var list = GetAttackArea();
        int damage = power;
        
        foreach (var item in list)
        {
            if (item.isEmpty) continue;
            var target = item.occupiedObject;
            var targetTeam = target.GetComponent<Team>();
            if(!team.isAlly(targetTeam))
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
        //Debug.Log(gameObject+"Dead");
        OnDead?.Invoke();
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


    public bool isReflect;

    #region Area
    /// <summary>
    /// 기물의 이동 범위 반환
    /// </summary>
    /// <returns>기물의 이동범위에 포함되는 타일</returns>
    public List<Tile> GetMoveArea()
    {

        // 다른 방식
        /*
         * 메인필드에서 fieldInfo를 가져옴(int2차원 배열)
         * 내 현재 위치를 0으로 전환
         * GetComponents로 MoveArea가져옴
         * intVector 배열에 각 이동범위 벡터 가져오기(매개변수로 fieldInfo 넣기)
         * 각 이동범위를 해당 위치의 타일로 전환
         */

        var fieldInfo = curTile.field.GetFieldInfo();
        fieldInfo[curTile.fieldPos.y, curTile.fieldPos.x] = 0;
        var list = GetComponents<IMoveArea>();

        var moveArea = new List<Tile>();
        foreach (var area in list)
        {
            var vectors = area.GetMoveVector(fieldInfo, curTile.fieldPos, isReflect);
            var tiles = curTile.field.GetTiles(vectors);
            moveArea.AddRange(tiles);
        }
        moveArea.Add(curTile);
        return moveArea;
    }

   
    public List<Tile> GetAttackArea(Tile tile)
    {
        var fieldInfo = tile.field.GetFieldInfo();
        if (tile.field == curTile.field)
        {
            fieldInfo[curTile.fieldPos.y, curTile.fieldPos.x] = 0;
        }
        var list = GetComponents<IAttackArea>();

        var moveArea = new List<Tile>();
        foreach (var area in list)
        {
            var vectors = area.GetAttackVector(fieldInfo, tile.fieldPos, isReflect);
            var tiles = tile.field.GetTiles(vectors);
            moveArea.AddRange(tiles);
        }
        return moveArea;
    }
    /// <summary>
    /// 기물의 공격 범위 반환
    /// </summary>
    /// <returns>기물의 공격 범위에 포함되는 타일</returns>
    public List<Tile> GetAttackArea()
    {
        return GetAttackArea(curTile);
    }


    #endregion

    #region AICalc
    /// <summary>
    /// 이동 범위를 intVector2 배열로 반환
    /// </summary>
    /// <returns></returns>
    public List<intVector2> GetMoveVector()
    {
        List<intVector2> list = new List<intVector2>();
        foreach (var tile in GetMoveArea())
        {
            list.Add(tile.fieldPos);
        }
        return list;
    }
    public List<intVector2> GetAttackVector(Tile start)
    {
        var list = new List<intVector2>();
        foreach (var tile in GetAttackArea(start))
        {
            list.Add(tile.fieldPos);
        }
        return list;
    }
    public List<intVector2> GetAttackVector()
    {
        return GetAttackVector(curTile);
    }

    /// <summary>
    /// 해당 기물이 이동 시 가장 좋은 위치 반환
    /// </summary>
    /// <param name="field">현재 필드 상황</param>
    /// <param name="tileValues">각 위치의 예상 가치</param>
    /// <returns></returns>
    public (int,intVector2) GetBestMove(int[,] field, int[,] tileValues)
    {
        var list = GetMoveVector();
        
        int max = tileValues[curTile.fieldPos.y, curTile.fieldPos.x];
        intVector2 pos = new intVector2(-1, -1);
        foreach (var tile in list)
        {
            // 이동이 불가능한 타일일 경우
            if (field[tile.y, tile.x] != 0) continue;

            // 공격 점수 계산


            // 피격 점수 계산
            var value = tileValues[tile.y, tile.x];
            if (value + curHp <= 0) value = -9999;
            if(value > max || pos.y == -1)
            {
                max = value;
                pos = tile;
            }
        }
        return (max, pos);
    }


    #endregion

}