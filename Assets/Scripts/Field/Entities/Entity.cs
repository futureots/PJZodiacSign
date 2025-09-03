using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;



[RequireComponent(typeof(BuffManager))]
public class Entity : MonoBehaviour, IDamageable, IAttackable
{

    private void Awake()
    {
        buffList = GetComponent<BuffManager>();
        statusEffects = new();
    }

    #region sourceField
    public Field sourceField;

    // 기물이 메인필드에서 온 것인지 확인
    public bool IsFromMainField()
    {
        return sourceField == GameManager.Instance.field;
    }

    // 기물이 인스턴트필드에서 온 것인지 확인
    public bool IsFromInstantField()
    {
        return sourceField != GameManager.Instance.field;
    }
    #endregion

    public Tile curTile { get; private set; }

    
    /// <summary>기물의 고유 id(이름)</summary>
    public string id;

    #region Skill
    /// <summary>기물 스킬 데이터</summary>
    public BaseSkillData skillData;
    /// <summary>
    /// 기물의 스킬 설정
    /// </summary>
    /// <param name="skillData">기물 스킬 데이터</param>
    public void SetupSkill(BaseSkillData skillData)
    {
        this.skillData = skillData;
    }
    /// <summary>skillData의 인스턴스</summary>
    public IActive GetSkillInstance()
    {
        if (skillData == null) return null;
        var skillInstance = skillData.CreateInstance();
        skillInstance.AddCallback(x => {
            if (x)
            {
                CurEnergy = 0;
            }
        });
        if (skillInstance is IOwnable entitySkill)
        {
            entitySkill.Owner = this;
        }
        return skillInstance;
    }


    #endregion
    
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
    public EntityData data;


    /// <summary>
    /// 기물 초기화, 스킬 설정
    /// </summary>
    /// <param name="data">기물 데이터</param>
    /// <param name="level">기물의 레벨</param>
    public void InitializeEntity(EntityData data, int level =0)
    {
        this.data = data;
        id = data.id;
        this.Level = level;

        UpdateEntity();

        // 기물 스킬 설정(스킬이 스크립트 기반 스킬이면 자동으로 할당, 아니면 할당X)
        SetupSkill(data.skill);
        SkillCost = data.skillCost;
    }

    void UpdateEntity()
    {
        // 기물 스탯 계산
        Power = data.power + data.bonusPower * Level;
        MaxHp = data.maxHp + data.bonusHp * Level;
        CurHp = MaxHp;
        CurEnergy = 0;
    }

    /// <summary>죽음 시 호출</summary>
    public Action onDead;

    #region Status
    /// <summary>기물의 레벨</summary>
    [SerializeField] int _level;
    public int Level { 
        get { return _level; }
        set
        {
            _level = value;
            onLevelChanged?.Invoke(_level);
            UpdateEntity();
        }
    }
    
    public Action<int> onLevelChanged;
    /// <summary>공격력</summary>
    [SerializeField] int _power;
    public int Power
    {
        get { return _power; }
        set
        {
            _power = value;
            onPowerChanged?.Invoke(_power);
        }
    }
    public Action<int> onPowerChanged;
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
    /// <summary>현재 에너지</summary>
    [SerializeField] int _curEnergy;
    public int CurEnergy
    {
        get { return _curEnergy; }
        set
        {
            _curEnergy = value;
            onEnergyChanged?.Invoke(_curEnergy, SkillCost);
        }
    }
    /// <summary>스킬 비용</summary>
    [SerializeField] int _skillCost;
    public int SkillCost
    {
        get { return _skillCost; }
        set
        {
            _skillCost = value;
            onEnergyChanged?.Invoke(CurEnergy, _skillCost);
        }
    }
    public Action<int, int> onEnergyChanged;
    #endregion

    #region Buff

    public void OnTurnStart()
    {
        buffList.UpdateBuff();
        buffList.RemoveBuff();
        CurEnergy = Mathf.Min(CurEnergy + 1, SkillCost);
    }

    public BuffManager buffList;

    Dictionary<string, int> statusEffects;
    public void AddEffect(string effectName)
    {
        if (statusEffects.ContainsKey(effectName))
        {
            statusEffects[effectName] += 1;
        }
        else
        {
            statusEffects.Add(effectName, 1);
        }
    }
    public void SubtractEffect(string effectName)
    {
        if (statusEffects.ContainsKey(effectName))
        {
            statusEffects[effectName] -= 1;
            if(statusEffects[effectName] <= 0)
            {
                statusEffects.Remove(effectName);
            }
        }
        else return;
    }

    bool isSlienced
    {
        get
        {
            return statusEffects.ContainsKey("slience");
        }
    }
    bool isRooted
    {
        get
        {
            return statusEffects.ContainsKey("root");
        }
    }
    bool isProtected
    {
        get
        {
            return statusEffects.ContainsKey("protect");
        }
    }

    #endregion

    public void Attack()
    {
        if (isSlienced) return;
        var list = GetAttackArea();
        int damage = Power;
        
        foreach (var item in list)
        {
            if (item.isEmpty) continue;
            var target = item.occupiedObject;
            var targetTeam = target.GetComponent<Team>();
            if(!team.IsAlly(targetTeam))
            {
                target.GetComponent<IDamageable>()?.Damaged(Power);
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
    /// <param name="ignoreArea">false : 이동 영역 내에서만 이동, true : 영역 무시하고 이동</param>
    /// <param name="ignoreOccupy">false : 타일이 비어있을 때만 이동, true : 이동 시 타일의 기물 제거</param>
    /// <returns></returns>
    public bool MoveSequence(Tile tile, bool ignoreArea = false, bool ignoreOccupy = false)
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
    /// 해당 타일로 이동(해당 타일의 기물 제거, 이동 제한 X)
    /// </summary>
    /// <param name="tile">이동할 타일</param>
    public void MoveTo(Tile tile)
    {
        if (curTile != null)
        {
            curTile.SetOccupant();
        }
        tile.SetOccupant(gameObject);
        curTile = tile;
        transform.position = tile.transform.position;
    }


    public bool isReflect;

    #region Area
    /// <summary>
    /// 기물의 이동 가능 좌표를 반환
    /// </summary>
    /// <returns>이동 가능 좌표의 배열</returns>
    public List<intVector2> GetMoveVector()
    {
        var moveArea = new List<intVector2>();
        if (!isRooted)
        {
            var fieldInfo = curTile.field.GetFieldState();
            fieldInfo[curTile.fieldPos.y, curTile.fieldPos.x] = 0;
            var list = GetComponents<IMoveArea>();
            
            foreach (var area in list)
            {
                var vectors = area.GetMoveVector(fieldInfo, curTile.fieldPos, isReflect);
                moveArea.AddRange(vectors);
            }
        }
        
        moveArea.Add(curTile.fieldPos);
        return moveArea;
    }
    /// <summary>
    /// 기물의 이동 영역 반환
    /// </summary>
    /// <returns>기물이 이동가능한 타일들</returns>
    public List<Tile> GetMoveArea()
    {
        var list = GetMoveVector();
        var tiles = curTile.field.GetTiles(list);
        return tiles;
    }

    public List<intVector2> GetAttackVector(int[,] field, intVector2 pos)
    {
        var attackArea = new List<intVector2>();
        if (isSlienced)
        {
            return attackArea;
        }
        var list = GetComponents<IAttackArea>();

        
        foreach (var area in list)
        {
            var vectors = area.GetAttackVector(field, pos, isReflect);
            attackArea.AddRange(vectors);
        }
        return attackArea;
    }

    public List<Tile> GetAttackArea(Tile tile)
    {
        var field = tile.field.GetFieldState();
        if(tile.field == curTile.field) field[curTile.fieldPos.y, curTile.fieldPos.x] = 0;
        var list = GetAttackVector(field, tile.fieldPos);
        var tiles = tile.field.GetTiles(list);
        return tiles;
    }

    /// <summary>
    /// 기물의 공격 영역 반환
    /// </summary>
    /// <returns>기물이 공격할 수 있는 타일</returns>
    public List<Tile> GetAttackArea()
    {
        return GetAttackArea(curTile);
    }
    public List<intVector2> GetAttackVector()
    {
        return GetAttackVector(curTile.field.GetFieldState(), curTile.fieldPos);
    }

    #endregion

    #region AICalculate
    /// <summary>
    /// 해당 기물의 이동 후 공격 가능한 위치 반환
    /// </summary>
    /// <param name="field">현재 필드 상태</param>
    /// <param name="tileValues">타일 위치별 가치</param>
    /// <returns></returns>
    public (int,intVector2) GetBestMove(int[,] field, int[,] tileValues)
    {
        var list = GetMoveVector();
        
        int max = tileValues[curTile.fieldPos.y, curTile.fieldPos.x];
        
        List<intVector2> pos = new();
        foreach (var area in list)
        {
            // 이동할 수 없는 타일은 제외
            if (field[area.y, area.x] != 0) continue;

            // 죽음 위험 체크(이동 후 체력이 0 이하면 -9999)
            var value = tileValues[area.y, area.x];
            if (value + CurHp <= 0) value = -9999;

            // 공격 가능 체크
            field[curTile.fieldPos.y, curTile.fieldPos.x] = 0;
            var plusArea = GetAttackVector(field,area);
            field[curTile.fieldPos.y, curTile.fieldPos.x] = team.teamNumber;
            foreach (var plus in plusArea)
            {
                if (field[plus.y, plus.x] == 0) continue;
                if (field[plus.y, plus.x] == team.teamNumber) continue;
                tileValues[area.y,area.x] += Power;
            }

            if (value > max || pos.Count == 0)
            {
                pos.Clear();
                max = value;
                pos.Add(area);
            }
            else if(value == max)
            {
                pos.Add(area);
            }
        }
        return (max, pos[UnityEngine.Random.Range(0,pos.Count)]);
    }


    #endregion


    


    


    
 

}