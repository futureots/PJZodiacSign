using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(BuffManager))]
[RequireComponent (typeof(PowerComponent))]
public class Entity : MonoBehaviour, IDamageable, IAttackable, IOccupant
{
    public Tile CurTile { get; private set; }

    public bool IsReflect { get; set; }

    Team _team;

    /// <summary>기물의 팀 번호</summary>
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

    // 기물의 기본 데이터
    public EntityData baseData;

    /// <summary>
    /// 기물 초기 스탯 세팅
    /// </summary>
    /// <param name="data">기물 데이터</param>
    /// <param name="level">기물의 레벨</param>
    public void InitializeEntity(EntityData data, int level =0)
    {
        this.baseData = data;
        this.Level = level;

        UpdateEntity();
    }

    void UpdateEntity()
    {
        // 기물 스탯 계산
        if(TryGetComponent<PowerComponent>(out var power))
        {
            power.Power = baseData.power + baseData.bonusPower * Level;
        }
        MaxHp = baseData.maxHp + baseData.bonusHp * Level;
        CurHp = MaxHp;
    }

    /// <summary>기물의 레벨</summary>
    [SerializeField] int _level;

    public int Level
    {
        get { return _level; }
        set
        {
            _level = value;
            onLevelChanged?.Invoke(_level);
            UpdateEntity();
        }
    }

    public Action<int> onLevelChanged;

    #region Status

    /// <summary>최대 체력</summary>
    [SerializeField] int _maxHp;

    public int MaxHp
    {
        get { return _maxHp; }
        set
        {
            _maxHp = value;
            onHpChanged?.Invoke(CurHp, _maxHp);
        }
    }

    /// <summary>현재 체력</summary>
    [SerializeField] int _curHp;

    public int CurHp
    {
        get { return _curHp; }
        set
        {
            _curHp = value;
            onHpChanged?.Invoke(_curHp, MaxHp);
        }
    }

    public Action<int, int> onHpChanged;
    #endregion

    // 나중에 스탯 계산용 핸들러 추가하면서 빼기
    public bool isProtected;

    public static Action<Entity> onEntityDead;
    public Action onDead;
    public void Attack()
    {
        var list = GetAttackArea();
        int damage = 0;
        if (TryGetComponent<PowerComponent>(out var component))
        {
            damage = component.Power;
        }
        
        
        foreach (var item in list)
        {
            if (item.isEmpty) continue;
            var target = item.occupiedObject;
            var targetTeam = target.GetComponent<Team>();
            if(!team.IsAlly(targetTeam))
            {
                target.GetComponent<IDamageable>()?.Damaged(damage);
            }
        }
    }
    public void Damaged(int damage)
    {
        var value = damage;
        // 보호막 계산
        if (isProtected) value /= 2;

        CurHp -= value;
    }

    public void Dead()
    {
        onEntityDead?.Invoke(this);
        onDead?.Invoke();
        Destroy(gameObject);
    }

    public void Healed(int amount)
    {
        CurHp += amount;
        CurHp = Mathf.Min(MaxHp,CurHp);
    }

    public bool isZero()
    {
        if (CurHp > 0) return false;
        return true;
    }
    
    /// <summary>
    /// 기물 이동(이동 제한 X)
    /// </summary>
    /// <param name="tile">이동할 타일</param>
    /// <param name="ignoreOccupy">false : 타일이 비어있을 때만 이동, true : 이동 시 타일의 기물 제거</param>
    /// <returns></returns>
    public bool Move(Tile tile, bool ignoreOccupy = false)
    {
        if (!ignoreOccupy)
        {
            var isOccupied = !tile.isEmpty;
            if (isOccupied) return false;
        }

        if (CurTile != null)
        {
            CurTile.UnsetOccupant();
        }
        tile.SetOccupant(gameObject, true);
        CurTile = tile;

        return true;
    }

    #region Area

    /// <summary>
    /// 기물의 이동 영역 반환
    /// </summary>
    /// <returns>기물이 이동가능한 타일들</returns>
    public List<Tile> GetMoveArea()
    {
        if(TryGetComponent<AreaComponent>(out var area))
        {
            var field = CurTile.field.GetFieldState();
            field[CurTile.fieldPos.y, CurTile.fieldPos.x] = 0;

            var list = area.GetMoveVector(field,CurTile.fieldPos, IsReflect);
            var tiles = CurTile.field.GetTiles(list).Where(value => value.isEmpty).ToList();

            tiles.Add(CurTile);

            return tiles;
        }
        return new List<Tile>();
    }

    /// <summary>
    /// 기물의 공격 영역 반환
    /// </summary>
    /// <returns>기물이 공격할 수 있는 타일</returns>
    public List<Tile> GetAttackArea()
    {
        return GetAttackArea(CurTile);
    }

    public List<Tile> GetAttackArea(Tile tile)
    {
        // IOccupant 인터페이스 사용해서 해당 함수도 AreaComponent로 빼기
        var field = tile.field.GetFieldState();
        if(tile.field == CurTile.field) field[CurTile.fieldPos.y, CurTile.fieldPos.x] = 0;
        
        if(TryGetComponent<AreaComponent>(out var component))
        {
            var list = component.GetAttackVector(field, tile.fieldPos, IsReflect);
            var tiles = tile.field.GetTiles(list);
            return tiles;
        }
        else
        {
            return new List<Tile>();
        }
    }

    #endregion

    #region AICalculate
    /// <summary>
    /// 해당 기물의 이동 후 공격 가능한 위치 반환
    /// </summary>
    /// <param name="field">현재 필드 상태</param>
    /// <param name="tileValues">타일 위치별 가치</param>
    /// <returns></returns>
    public bool GetBestMove(int[,] field, int[,] tileValues, out int value, out intVector2 pos)
    {
        
        int power = 0;
        if(TryGetComponent<PowerComponent>(out var component))
        {
            power = component.Power;
        }
        int max = tileValues[CurTile.fieldPos.y, CurTile.fieldPos.x];

        List<intVector2> valuablePos = new();
        if(TryGetComponent<AreaComponent>(out var area))
        {
            var list = area.GetMoveVector(field, CurTile.fieldPos, IsReflect);

            foreach (var item in list)
            {
                // 이동할 수 없는 타일은 제외
                if (field[item.y, item.x] != 0 || CurTile.fieldPos == item) continue;
                Debug.Log($"Best Entity : {baseData.productName} , CurPos : {CurTile.fieldPos} , Expect : {item} ");
                // 죽음 위험 체크(이동 후 체력이 0 이하면 가중치 부여)
                var damage = tileValues[item.y, item.x];
                if (damage + CurHp <= 0) damage -= 5;

                // 공격 가능 체크
                field[CurTile.fieldPos.y, CurTile.fieldPos.x] = 0;
                var plusArea = area.GetAttackVector(field, item, IsReflect);
                field[CurTile.fieldPos.y, CurTile.fieldPos.x] = team.teamNumber;
                foreach (var plus in plusArea)
                {
                    if (field[plus.y, plus.x] == 0) continue;
                    if (field[plus.y, plus.x] == team.teamNumber) continue;
                    tileValues[item.y, item.x] += power;
                }



                if (damage > max || valuablePos.Count == 0)
                {
                    valuablePos.Clear();
                    max = damage;
                    valuablePos.Add(item);
                }
                else if (damage == max)
                {
                    valuablePos.Add(item);
                }
            }
        }
        if(valuablePos.Count <= 0)
        {
            value = 0;
            pos = intVector2.Zero;

            return false;
        }
        value = max;
        pos = valuablePos[UnityEngine.Random.Range(0, valuablePos.Count)];
        return true;
    }


    #endregion
}