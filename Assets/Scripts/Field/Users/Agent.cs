using System;
using System.Collections.Generic;
using UnityEngine;


public class Agent : MonoBehaviour
{
    // 플레이어 번호
    public PlayerID id;
    // 팀 번호
    public intVector2 Direction { get; private set; }

    private static Agent _localPlayer;
    public static Agent LocalPlayer
    {
        get
        {
            return _localPlayer;
        }
        set
        {
            _localPlayer = value;
            OnLocalPlayerChanged?.Invoke();
        }
    }
    public static Action OnLocalPlayerChanged;

    public FieldController fieldController { get; protected set; }
    

    // 행동 포인트
    public int actionCount;
    
    private int _currentActionCount;

    public int CurrentActionCount
    {
        get
        {
            return  _currentActionCount;
        }
        private set
        {
            _currentActionCount = value;
            onActionCountChanged?.Invoke(_currentActionCount);
        }
    }
    
    //public List<Entity> actionAbleEntities=new List<Entity>();
    
    public event Action<int> onActionCountChanged;

    private int _credit;
    
    public int Credit
    {
        get
        {
            return _credit;
        }
        set
        {
            _credit = value;
            OnCreditChanged?.Invoke(Credit);
        }
    }
    public event Action<int> OnCreditChanged;

    public Inventory inventory;

    public List<Entity> fieldEntities;
    public List<Entity> resourceEntities;
    
    public void Init(FieldController fieldController, PlayerID teamId,AgentData data, intVector2 direction)
    {
        // Get Data
        Direction = direction;
        id = teamId;
        Credit = data.credit;
        
        // Inject FieldController
        this.fieldController = fieldController;
        fieldController.OnPhaseStarted += OnPhaseChange;
        fieldController.OnTurnStarted += OnTurnChange;
        
        // Inventory
        if (data.items == null) data.items = new();
        inventory.SetItem(data.items);

        // Resource Field
        resourceEntities = new();
        var list = StageManager.Instance.agentField[teamId].GetTiles().GetEmptyTiles();
        foreach (var entityData in data.handEntities)
        {
            if (list.Count <= 0) break;
            GetEntity(entityData.data, entityData.level);
        }

        // On Field
        fieldEntities = new();
        foreach (var entityData in data.fieldEntities)
        {
            DeployEntity(entityData.Value.data, entityData.Key, entityData.Value.level);
        }
    }

    /// <summary>
    /// 엔티티 새로 획득
    /// </summary>
    /// <param name="newEntity">획득할 엔티티 정보</param>
    /// <param name="level">초기 레벨</param>
    /// <remarks>리소스 필드로 할당</remarks>
    public Entity GetEntity(EntityData newEntity, int level = 0)
    {
        var resourceField = StageManager.Instance.agentField[id];
        var list = resourceField.GetTiles().GetEmptyTiles();
        var entity = EntityFactory.Instance.Request(newEntity, Direction, list[0], id, level);

        list.RemoveAt(0);
        resourceEntities.Add(entity);

        return entity;
    }

    /// <summary>
    /// 엔티티 새로 배치
    /// </summary>
    /// <param name="newEntity">획득할 엔티티 정보</param>
    /// <param name="tile">배치 위치</param>
    /// <param name="level">초기 레벨</param>
    /// <remarks>온필드에 바로 배치</remarks>
    public void DeployEntity(EntityData newEntity, intVector2 tile, int level = 0)
    {
        var fieldTile = StageManager.Instance.field.GetTile(tile);
        if (!fieldTile) return;
        var entity = EntityFactory.Instance.Request(newEntity, Direction, fieldTile, id, level);
        // 기물 사망(강화) 시 리스트에서 제거(이후 페이즈 변화 시 리스트 갱신 및 액션 변경)
        fieldEntities.Add(entity);
    }
    
    
    void OnTurnChange(Turn turn, uint count)
    {
        fieldEntities.ForEach(entity => entity.IsControllable = false);
        resourceEntities.ForEach(entity => entity.IsControllable = false);
        if(turn.agentID == id)
        {
            
            switch (turn.type)
            {
                case TurnType.ACTION:
                    CurrentActionCount = actionCount;
                    fieldEntities.ForEach(entity => entity.IsControllable = true);
                    break;
                case TurnType.ATTACK:
                    CurrentActionCount = -1;
                    break;
                case TurnType.REPAIR:
                    fieldEntities.ForEach(entity => entity.IsControllable = true);
                    resourceEntities.ForEach(entity => entity.IsControllable = true);
                    CurrentActionCount = -1;
                    break;
            }
        }
    }

    void OnPhaseChange(Phase phase)
    {
        foreach (var entity in resourceEntities)
        {
            entity.onDead -= RemoveOnResourceList;
        }
        var resources = StageManager.Instance.agentField[id].GetEntities(id);
        resourceEntities.Clear();
        foreach (var resource in resources)
        {
            resourceEntities.Add(resource);
            resource.onDead += RemoveOnResourceList;
        }
        
        foreach (var entity in fieldEntities)
        {
            entity.onDead -= RemoveOnFieldList;
        }
        var fields =  StageManager.Instance.field.GetEntities(id);
        fieldEntities.Clear();
        foreach (var entity in fields)
        {
            fieldEntities.Add(entity);
            entity.onDead += RemoveOnFieldList;
        }
        if (phase.phaseName == PhaseType.Battle)
        {
            var _entityLevelData = new List<EntityLevelData>();
            foreach (var entity in resourceEntities)
            {
                _entityLevelData.Add(new EntityLevelData(entity));
            }
            entityLevelData = _entityLevelData;
            
            var _fieldEntityData = new Dictionary<intVector2, EntityLevelData>();
            foreach (var entity in fieldEntities)
            {
                _fieldEntityData.Add(entity.CurTile.fieldPos, new EntityLevelData(entity));
                entity.onEntitySpawn += SetSpawnedByEntity;
            }
            fieldEntityData = _fieldEntityData;
        }
    }

    void RemoveOnFieldList(Entity entity)
    {
        fieldEntities.Remove(entity);
    }

    void RemoveOnResourceList(Entity entity)
    {
        resourceEntities.Remove(entity);
    }

    void SetSpawnedByEntity(Entity entity)
    {
        fieldEntities.Add(entity);
        entity.IsControllable = true;
        entity.onDead += RemoveOnFieldList;
        entity.onEntitySpawn += SetSpawnedByEntity;
    }
    
    
    public Command CreateMoveCommand(Entity entity, Tile tile, bool isWarp = false)
    {
        CurrentActionCount -= 1;
        var cmd = new MoveCommand(entity, tile, isWarp);
        // 해당 기물의 이동명령이 있으면 제거 후 추가
        fieldController.ReceiveCommands(cmd);
        return cmd;
    }

    public Command CreateSkillCommand(SkillComponent skill)
    {
        CurrentActionCount -= 1;
        var cmd = new SkillCommand(skill);
        fieldController.ReceiveCommands(cmd);
        return cmd;
    }

    public Command CreateAttackCommand(Entity entity)
    {
        CurrentActionCount -= 1;
        var cmd = new AttackCommand(entity);
        fieldController.ReceiveCommands(cmd);
        return cmd;
    }

    public Command CreateEnhanceCommand(Entity target, Entity source)
    {
        CurrentActionCount -= 1;
        var cmd = new EnhanceCommand(target, source);
        fieldController.ReceiveCommands(cmd);
        return cmd;
    }

    public Command CreateEndCommand()
    {
        var cmd = new EndCommand(fieldController);
        fieldController.ReceiveCommands(cmd);
        return cmd;
    }


    List<EntityLevelData> entityLevelData;
    Dictionary<intVector2,EntityLevelData> fieldEntityData;
    public AgentData getData() {
        var data = new AgentData(Credit,entityLevelData,fieldEntityData,inventory.GetInventoryData());
        return data; 
    }

}
