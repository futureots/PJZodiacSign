using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(PowerComponent))]
[RequireComponent(typeof(HealthComponent))]
public class Entity : Occupant, IDamageable, IAttackable
{

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
    

    /// <summary>기물의 레벨</summary>
    [SerializeField] int _level;

    public int Level
    {
        get { return _level; }
        set
        {
            int t = _level;
            _level = value;
            onLevelChanged?.Invoke(_level,t);
            
        }
    }

    public Action<int, int> onLevelChanged;

    #region Status

    public PowerComponent power;
    public HealthComponent health;
    
    #endregion

    // 나중에 스탯 계산용 핸들러 추가하면서 빼기
    public bool isProtected;

    public static Action<Entity> onEntityDead;
    public Action onDead;

    public void Initialize(bool isReflect, Tile tile)
    {
        this.IsReflect = isReflect;
        Move(tile, true);
    }

    /// <summary>
    /// 기물 초기 스탯 세팅
    /// </summary>
    /// <param name="data">기물 데이터</param>
    /// <param name="level">기물의 레벨</param>
    public void InitializeEntity(EntityData data, int level = 0)
    {
        this.baseData = data;
        this.Level = level;

        //체력 분리
        health = GetComponent<HealthComponent>();
        health.Initialize(baseData.maxHp + baseData.bonusHp * level);

        power = GetComponent<PowerComponent>();
        power.Init(baseData.power + baseData.bonusPower * level);
        onLevelChanged += UpdateEntity;
    }

    void UpdateEntity(int cur, int prev)
    {
        health.MaxHealth += baseData.bonusHp * (cur - prev);
        health.CurHealth += baseData.bonusHp * (cur - prev);
        power.Power += baseData.bonusPower * (cur - prev);
    }

    public IEnumerator Attack()
    {
        var list = GetAttackArea();
        int damage = power.Power;

        foreach (var item in list)
        {
            if (item.isEmpty) continue;
            var target = item.occupiedObject;
            var targetTeam = target.GetComponent<Team>();
            if(!team.IsAlly(targetTeam))
            {
                var effect = Instantiate(baseData.basicAttackEffect, transform.position + Vector3.up*7,Utils.QI);
                effect.GetComponent<BasicAttackEffect>()?.Initialize(target, damage);
                //target.GetComponent<IDamageable>()?.Damaged(damage);
            }
        }
        yield return null;
    }

    public void Damaged(int damage)
    {
        var value = damage;
        // 보호막 계산
        if (isProtected) value /= 2;

        health.CurHealth -= value;
    }
    public void Dead()
    {
        onEntityDead?.Invoke(this);
        onDead?.Invoke();
        var effect = Instantiate(baseData.dissolveEffect, transform);
        if(effect.TryGetComponent<DissolveEffect>(out var dissolve))
        {
            if(TryGetComponent<MeshFilter>(out var mesh))
            {
                dissolve.Initialize(mesh.mesh, GetComponent<MeshRenderer>());
                dissolve.PlayEffect(2f);
            }
        }
        
        
    }

    public void Healed(int amount)
    {
        health.CurHealth += amount;
    }

    public bool isZero()
    {
        if (health.CurHealth > 0) return false;
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
            var field = CurTile.field.GetFieldState(this);

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
        var field = tile.field.GetFieldState(this);
        
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

}