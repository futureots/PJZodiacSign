using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkillComponent))]
[RequireComponent(typeof(EnergyComponent))]     // NOTE: Skill 내 Energy 스탯 종속 시 컴포넌트 병합
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

    private bool isControllable = false;

    public bool IsControllable
    {
        get => isControllable;
        set
        {
            isControllable = value;
            OnControllableChanged?.Invoke(isControllable);
        }
    }

    public Action<bool> OnControllableChanged;

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
    
    public Action<Entity> onDead;
    public Action<Entity> onEntitySpawn;

    [SerializeField] public AreaComponent area;
    
    [SerializeField] public EnergyComponent energy;
    
    [SerializeField] public SkillComponent skill;

    #endregion

    private void Awake()
    {
        team = new();
    }

    [SerializeField] Material white;
    [SerializeField] Material black;
    /// <summary>
    /// Initialize Setting when Load
    /// </summary>
    /// <param name="data">Entity Data</param>
    /// <param name="level">Initial Level</param>
    /// <param name="team"></param>
    public void Init(EntityData data, intVector2 direction, int level = 0, PlayerID team =  PlayerID.None)
    {
        baseData = data;
        Level = level; // NOTE: 초기화 시 레벨 변화 이벤트 발생중
        this.direction = direction;
        if (direction.y < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        this.team.teamNumber = team;
        if (TryGetComponent<Renderer>(out var render))
        {
            render.materials = team == Agent.LocalPlayer.id ? new []{white} : new []{black};
        }
        
        
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
        PlayEffect(EffectType.LevelUp);
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

        bool isAttacked = false;
        foreach (var item in list)
        {
            if (item.IsEmpty) continue;
            var target = item.occupiedEntity;
            if (target.TryGetComponent<Entity>(out var entity))
            {
                if (!team.IsAlly(entity.team))
                {
                    var effect = EffectFactory.Instance.Request("AttackEffect", transform.position + Vector3.up * 7, transform.lossyScale,5f);
                    effect.GetComponent<BasicAttackEffect>()?.Initialize(target.gameObject, ()=> entity.Damaged(damage));
                    isAttacked = true;
                }
            }
        }
        if(isAttacked) yield return new WaitForSeconds(1.5f);
        yield break;
    }

    #endregion

    #region Health
    
    private int _defense;
    
    /// <summary>
    /// 기물의 방어력(받는 피해를 감소 시킨다.)
    /// </summary>
    public int Defense { 
        get => _defense;
        set
        {
            _defense = value;
            OnDefenseChanged?.Invoke(_defense);
        }
    }
    public event Action<int> OnDefenseChanged;
    
    private int _curHealth;
    
    /// <summary>
    /// 기물의 현재 체력
    /// </summary>
    public int CurHealth { 
        get => _curHealth;
        set
        {
            _curHealth = Math.Min(MaxHealth,value);
            OnHealthChanged?.Invoke(_curHealth, _maxHealth);
        }

    }
    private int _maxHealth;
    public int MaxHealth { 
        get => _maxHealth;
        set 
        {
            _maxHealth = value;
            OnHealthChanged?.Invoke(_curHealth, _maxHealth);
        }
    }
    public event Action<int, int> OnHealthChanged;

    public void Damaged(int damage)
    {
        var value = Math.Max(damage - _defense,0);
        EffectFactory.Instance.Request("HitEffect",transform.position + Vector3.up * 7, transform.lossyScale*2f,1f);

        CurHealth -= value;
    }

    public void Dead()
    {
        if (TryGetComponent(out Collider collider))
        {
            collider.enabled = false;
        }
        onDead?.Invoke(this);
        PlayEffect(EffectType.Dissolve);
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
            var isOccupied = !tile.IsEmpty;
            if (isOccupied) return false;
        }

        if (CurTile != null)
        {
            CurTile.UnsetOccupant();
        }

        tile.SetOccupant(this, true);
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
        var field = CurTile.field.GetFieldState(this);

        var list = area.GetMoveVector(field, CurTile.fieldPos, direction);
        var tiles = CurTile.field.GetTiles(list).GetEmptyTiles();

        tiles.Add(CurTile);

        return tiles;
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

    #region Effect

    public enum EffectType
    {
        LevelUp,
        Dissolve
    }

    /// <summary>
    /// 기물에 대한 이펙트 재생
    /// </summary>
    /// <param name="effectType"></param>
    public void PlayEffect(EffectType effectType)
    {
        switch (effectType)
        {
            case EffectType.Dissolve:
                var dissolveEffect = EffectFactory.Instance.Request("Dissolve", transform.position, transform.lossyScale,3f);
                if (dissolveEffect.TryGetComponent<DissolveEffect>(out var dissolve))
                {
                    if (TryGetComponent<MeshFilter>(out var mesh) && TryGetComponent<MeshRenderer>(out var render))
                    {
                        dissolve.Initialize(mesh.mesh, render);
                        dissolve.PlayEffect(2f);
                    }
                    Destroy(gameObject, 2f);
                }
                break;
            case EffectType.LevelUp:
                var levelUpEffect = EffectFactory.Instance.Request("LevelUp", transform.position, transform.lossyScale);
                if (levelUpEffect.TryGetComponent<GlowEffect>(out var levelUp))
                {
                    if (TryGetComponent<MeshFilter>(out var mesh))
                    {
                        levelUp.Init(mesh.mesh);
                        levelUp.Play();
                    }
                }
                break;
        }
    }

    #endregion
    
    #region Indicator

    [SerializeField] Outline indicatorEffect;
    public void ApplyHighlight()
    {
        indicatorEffect.enabled = true;
    }
    public void RemoveHighlight()
    {
        indicatorEffect.enabled = false;
    }
    #endregion
    
}