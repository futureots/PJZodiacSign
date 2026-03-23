using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    /*
     * 스테이지 레벨 1개 관리
     * - 필요 데이터 전달, 의존성 주입
     */
    [SerializeField] public Shop shop;
    private EntityFactory entityFactory => EntityFactory.Instance;
    [SerializeField] private UIMapper uiMapper;
    public Field field;
    
    // key = teamNum, value = ResourceField
    [SerializeField] List<Field> resourceFields;
    public Dictionary<PlayerID, Field> agentField;
    
    public event Action<PlayerID> OnStageEnded;

    public void EndStage(PlayerID winner)
    {
        OnStageEnded?.Invoke(winner);
    }
    /// <summary>
    /// Init Model with Data
    /// </summary>
    /// <param name="stageData">stage Data to set</param>
    /// <remarks>Injected data by Controller</remarks>
    public void Init(StageData stageData)
    {
        // Entity Pooling
        // TODO: pooling 비동기로 예외
        // Shop Entities
        List<EntityData> entityList = stageData.shopTable.entityList.ConvertAll(x => x.data);
        
        // Agent Entities
        List<AgentData> agentList = stageData.agents;
        agentList.Add(stageData.player);
        List<EntityLevelData> agentEntityList = agentList
            .SelectMany(a => a.handEntities.Concat(a.fieldEntities.Values))
            .ToList();
        entityFactory.SetPool(entityList, agentEntityList);
        
        // TODO: Item Pooling
        // List<ItemData> itemList = stageData.shopTable.itemList.ConvertAll(x => x.data);

        
        // Set Shop
        shop.Init(stageData.shopTable);
        
        // Create Agents
        // NOTE: Single Player 기준 -1부터 카운트
        agentField = new Dictionary<PlayerID, Field>();
        for (int i = 0; i < resourceFields.Count; i++)
        {
            agentField.Add((PlayerID)(i - 1), resourceFields[i]);
        }

        // TODO: stageData와 Field 내 최대 Agent 개수 조절 필요
        // for (int i = 0; i < stageData.agents.Count; i++)
        // NOTE: Single Player 기준 0부터 카운트
        for (int i = 0; i < agentField.Count - 1; i++)
        {
            SetAgentField((PlayerID)(i), stageData.agents[i], new intVector2(-1, -1));
        }
        SetAgentField(PlayerID.P0, stageData.player, new intVector2(1, 1));             // LocalPlayer
    }

    void SetAgentField(PlayerID teamId, AgentData data, intVector2 direction)
    {
        var list = agentField[teamId].GetTiles().GetEmptyTiles();
        foreach (var entityData in data.handEntities)
        {
            if (list.Count <= 0) break;
            var entity = entityFactory.RequestEntity(entityData.data, direction,list[0], entityData.level);
            entity.team.teamNumber = teamId;
            list.RemoveAt(0);
        }
        foreach (var entityData in data.fieldEntities)
        {
            var tile = field.GetTile(entityData.Key);
            if (!tile) continue;
            var entity = entityFactory.RequestEntity(entityData.Value.data, direction, tile, entityData.Value.level);
            entity.team.teamNumber = teamId;
        }
    }

    /// <summary>
    /// Set Model to Current Phase
    /// </summary>
    /// <param name="newPhase">new Phase Info</param>
    public void SetPhase(Phase newPhase)
    {
        uiMapper.SetUI(newPhase.useUIType);

        // TODO: 페이즈 설정
    }

    /// <summary>
    /// Set Model to Current Phase
    /// </summary>
    /// <param name="newTurn">new Turn Info</param>
    public void SetTurn(Turn newTurn)
    {
        if(newTurn.type == TurnType.ACTION)
        {
            foreach(var entity in field.GetEntities())
            {
                if (entity.team.IsAlly(newTurn.agentID))
                {
                    if(entity.TryGetComponent<EnergyComponent>(out var energy))
                    {
                        energy.CurEnergy += 1;
                    }
                    
                }
            }
        }
    }

    #region CommandLogic

    private int _count;
    public int Count
    {
        get
        {
            return _count;
        }
        set
        {
            _count = value;
            isSequencing?.Invoke(_count);
        }
    }
    public Action<int> isSequencing;
    
    Queue<IExecute> commandQueue = new Queue<IExecute>();
    private bool _isWaiting;
    
    public void ReceiveCommand(IExecute command)
    {
        if (_isWaiting)
        {
            commandQueue.Enqueue(command);
            return;
        }
        if (command is CheckCommand cmd)
        {
            StartCoroutine(WaitForCommand(cmd));
        }
        else
        {
            ExecuteCommand(command);
        }
    }

    void ExecuteCommand(IExecute command)
    {
        void Callback() => Count--;
        Count++;
        this.RunWithCallback(command.Execute(), Callback);
    }

    IEnumerator WaitForCommand(IExecute command)
    {
        _isWaiting = true;
        yield return new WaitUntil(() => Count == 0);
        _isWaiting = false;
        yield return StartCoroutine(command.Execute());
        

        if (command is EndCommand)
        {
            foreach(var cmd in commandQueue) cmd.Delete();
            commandQueue.Clear();
        }
        else
        {
            List<IExecute> commands = new List<IExecute>(commandQueue);
            commandQueue.Clear();
            foreach (var cmd in commands)
            {
                ReceiveCommand(cmd);
            }
            
        }
    }

    
    #endregion
}
