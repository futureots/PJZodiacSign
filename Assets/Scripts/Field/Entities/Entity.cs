using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Tile;

[RequireComponent(typeof(SkillComponent))]
[RequireComponent(typeof(EnergyComponent))]     // NOTE: Skill 내 Energy 스탯 종속 시 컴포넌트 병합
[RequireComponent(typeof(BuffManager))]
[RequireComponent(typeof(AreaComponent))]
public class Entity : Occupant, IDamageable, IAttackable
{
    // 기물의 기본 데이터
    public EntityData baseData;

    /// 기물의 팀 번호

    public Team team;

    public intVector2 direction;
    #region Status

    // Level of current Entity
    [SerializeField] int level;
    public Action<int, int> OnLevelChanged;

    public int Level
    {
        get => level;
        set
        {
            int before = level;
            level = value;
            OnLevelChanged?.Invoke(level, before);
        }
    }

    public static Action<Entity> onEntityDead;

    public Action onDead;

    [SerializeField] public AreaComponent area;
    
    [SerializeField] private EnergyComponent energy;
    
    [SerializeField] private SkillComponent skill;

    #endregion

    // TODO: 나중에 스탯 계산용 핸들러 추가하면서 빼기
    public bool isProtected;

    private void Awake()
    {
        team = new();
    }

    /// <summary>
    /// Initialize Setting when Load
    /// </summary>
    /// <param name="data">Entity Data</param>
    /// <param name="level">Initial Level</param>
    public void Init(EntityData data, intVector2 direction, int level = 0)
    {
        baseData = data;
        Level = level; // NOTE: 초기화 시 레벨 변화 이벤트 발생중
        this.direction = direction;
        //Get Status Component
        MaxHealth = baseData.maxHp + baseData.hpMultiplier * level;
        CurHealth = MaxHealth;
        // Set Attack
        Power = baseData.power + baseData.powerMultiplier * level;
        OnLevelChanged += UpdateEntity;
        
        // Set Skill
        energy = GetComponent<EnergyComponent>();
        energy.Initialize(data.maxEnergy);
        skill.Init(baseData.skill);
        
        // Set Area
        area.SetArea(data.moveArea, data.attackArea);

         IsReflect = false;
    }

    /// Update Status with Level-Up
    void UpdateEntity(int newLevel, int prevLevel)
    {
        int upLevel = newLevel - prevLevel;
        MaxHealth += baseData.hpMultiplier * upLevel;
        CurHealth += baseData.hpMultiplier * upLevel;
        Power += baseData.powerMultiplier * upLevel;
    }

    #region Attack

    private int _power;

    public int Power
    {
        get => _power;
        set
        {
            _power = value;
            OnPowerChanged?.Invoke(_power);
        }
    }

    public event Action<int> OnPowerChanged;

    public IEnumerator Attack(int multiplier = 100)
    {
        int damage = Power * multiplier / IAttackable.Scale;
        
        var list = GetAttackArea();

        foreach (var item in list)
        {
            if (item.isEmpty) continue;
            var target = item.occupiedObject;
            if(target.TryGetComponent<Entity>(out var entity))
            {
                if (!team.IsAlly(entity.team))
                {
                    var effect = Instantiate(baseData.basicAttackEffect, transform.position + Vector3.up * 7, Utils.QI);
                    effect.GetComponent<BasicAttackEffect>()?.Initialize(target, damage);
                    yield return new WaitForSeconds(1.5f);
                }
            }
        }
        yield return null;
    }

    #endregion

    #region Health

    // TODO: Health 변경 로직
    private int _curHealth;
    public int CurHealth { 
        get
        {
            return _curHealth;
        }
        set
        {
            _curHealth = Math.Min(MaxHealth,value);
            OnHealthChanged?.Invoke(_curHealth, _maxHealth);
        }

    }
    private int _maxHealth;
    public int MaxHealth { 
        get
        {
            return _maxHealth;
        }
        set 
        {
            _maxHealth = value;
            OnHealthChanged?.Invoke(_curHealth, _maxHealth);
        }
    }
    public event Action<int, int> OnHealthChanged;

    public void Damaged(int damage)
    {
        var value = damage;
        // 보호막 계산
        if (isProtected) value /= 2;

        CurHealth -= value;
    }

    public void Dead()
    {
        onEntityDead?.Invoke(this);
        onDead?.Invoke();
        var effect = Instantiate(baseData.dissolveEffect, transform);
        if (effect.TryGetComponent<DissolveEffect>(out var dissolve))
        {
            if (TryGetComponent<MeshFilter>(out var mesh))
            {
                dissolve.Initialize(mesh.mesh, GetComponent<MeshRenderer>());
                dissolve.PlayEffect(2f);
                Destroy(gameObject, 2f);
            }
        }
    }

    public void Healed(int amount)
    {
        CurHealth += amount;
        // NOTE: 별도의 힐 이벤트 추가
    }

    public bool IsZero()
    {
        return CurHealth <= 0;
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

    #region MoveArea

    /// <summary>
    /// 기물의 이동 영역 반환
    /// </summary>
    /// <returns>기물이 이동가능한 타일들</returns>
    public List<Tile> GetMoveArea()
    {
        if (TryGetComponent<AreaComponent>(out var area))
        {
            var field = CurTile.field.GetFieldState(this);

            var list = area.GetMoveVector(field, CurTile.fieldPos, direction);
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

        var list = area.GetAttackVector(field, tile.fieldPos, direction);
        var tiles = tile.field.GetTiles(list);
        return tiles;
    }

    #endregion

    #region Indicator

    [SerializeField] GameObject indicatorEffect;
    public void ApplyHighlight()
    {
        indicatorEffect.SetActive(true);
    }
    public void RemoveHighlight()
    {
        indicatorEffect.SetActive(false);
    }
    #endregion
}