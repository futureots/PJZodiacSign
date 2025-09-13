using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(BuffManager))]
public class Entity : MonoBehaviour, IDamageable, IAttackable, IMovable
{
    // 사망 시 해당 엔티티 
    private void Awake()
    {
        buffList = GetComponent<BuffManager>();
        statusEffects = new();
    }

    public Tile curTile { get; set; }
    
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
    /// 기물 초기화, 스킬 설정
    /// </summary>
    /// <param name="data">기물 데이터</param>
    /// <param name="level">기물의 레벨</param>
    public void InitializeEntity(EntityData data, int level =0)
    {
        this.baseData = data;
        this.Level = level;

        UpdateEntity();

        // 기물 스킬 설정(나중에 엔티티 데이터 팩토리 패턴 적용 시 해당 세팅은 인스턴스 생성 시로 변경 예정)
        if (data.skill != null)
        {
            var _skill = gameObject.GetOrAddComponent<SkillComponent>();
            
            _skill.SetupSkill(data.skill, data.skillCost);
        }
    }

    void UpdateEntity()
    {
        // 기물 스탯 계산
        Power = baseData.power + baseData.bonusPower * Level;
        MaxHp = baseData.maxHp + baseData.bonusHp * Level;
        CurHp = MaxHp;
    }


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
    #endregion

    #region Buff

    public void OnTurnStart()
    {
        buffList.UpdateBuff();
        buffList.RemoveBuff();
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
    public static Action<Entity> onEntityDead;
    public Action onDead;
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
        if(isRooted) return false;
        if (!ignoreOccupy)
        {
            var isOccupied = !tile.isEmpty;
            if (isOccupied) return false;
        }

        if (curTile != null)
        {
            curTile.UnsetOccupant();
        }
        tile.SetOccupant(gameObject, true);
        curTile = tile;

        return true;
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
    public bool GetBestMove(int[,] field, int[,] tileValues, out int value, out intVector2 pos)
    {
        var list = GetMoveVector();
        
        int max = tileValues[curTile.fieldPos.y, curTile.fieldPos.x];
        
        List<intVector2> valuablePos = new();
        foreach (var area in list)
        {
            
            // 이동할 수 없는 타일은 제외
            if (field[area.y, area.x] != 0 || curTile.fieldPos == area) continue;
            Debug.Log($"Best Entity : {baseData.productName} , CurPos : {curTile.fieldPos} , Expect : {area} ");
            // 죽음 위험 체크(이동 후 체력이 0 이하면 가중치 부여)
            var damage = tileValues[area.y, area.x];
            if (damage + CurHp <= 0) damage -= 5;

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

            if (damage > max || valuablePos.Count == 0)
            {
                valuablePos.Clear();
                max = damage;
                valuablePos.Add(area);
            }
            else if(damage == max)
            {
                valuablePos.Add(area);
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