using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PowerComponent))]
[RequireComponent(typeof(HealthComponent))]
public class Entity : Occupant, IDamageable, IAttackable
{
    // 기물의 기본 데이터
    public EntityData baseData;
    
    /// 기물의 팀 번호
    public Team team
    {
        get;
        private set;
    }
    


    #region Status
    
    //Level of current Entity
    [SerializeField] int level;
    public Action<int, int> OnLevelChanged;
        
    public int Level
    {
        get => level;
        set
        {
            int before = level;
            level = value;
            OnLevelChanged?.Invoke(level,before);
            
        }
    }
    
    // Damagable Power and Health Component
    public PowerComponent power;
    public HealthComponent health;
    
    public static Action<Entity> onEntityDead;
    public Action onDead;
    
    #endregion

    // TODO: 나중에 스탯 계산용 핸들러 추가하면서 빼기
    public bool isProtected;

    public void Init(bool isReflect, Tile tile)
    {
        this.IsReflect = isReflect;
        Move(tile, true);
    }

    /// <summary>
    /// Initialize Setting when Load
    /// </summary>
    /// <param name="data">Entity Data</param>
    /// <param name="level">Initial Level</param>
    public void InitializeEntity(EntityData data, int level = 0)
    {
        baseData = data;
        Level = level;      // NOTE: 초기화 시 레벨 변화 이벤트 발생중

        //Get Status Component
        health = GetComponent<HealthComponent>();
        health.Initialize(baseData.maxHp + baseData.bonusHp * level);

        power = GetComponent<PowerComponent>();
        power.Init(baseData.power + baseData.bonusPower * level);
        OnLevelChanged += UpdateEntity;
    }

    /// Update Status with Level-Up
    void UpdateEntity(int newLevel, int prevLevel)
    {
        int upLevel = newLevel - prevLevel;
        health.MaxHealth += baseData.bonusHp * upLevel;
        health.CurHealth += baseData.bonusHp * upLevel;
        power.Power += baseData.bonusPower * upLevel;
    }

    #region Attack
    
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
    
    #endregion
    
    #region Health

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

    public bool IsZero()
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

    #endregion
    
    #region NoveArea

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