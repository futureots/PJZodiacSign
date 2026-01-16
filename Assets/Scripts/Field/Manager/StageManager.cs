
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    /*
     * 스테이지 레벨 1개 관리
     * - 필요 데이터 전달, 의존성 주입
     */
    [SerializeField] public Shop shop;
    [SerializeField] private EntityFactory entityFactory;
    [SerializeField] private UIMapper uiMapper;
    public Field field;
    // key = teamNum, value = ResourceField
    [SerializeField] List<Field> resourceFields;
    public Dictionary<PlayerID, Field> agentField;

    /// <summary>
    /// Init Model with Data
    /// </summary>
    /// <param name="stageData">stage Data to set</param>
    /// <remarks>Injected data by Controller</remarks>
    public void Init(StageData stageData)
    {
        shop.Init(stageData.shopTable);
        agentField = new Dictionary<PlayerID, Field>();
        for (int i = 0; i < resourceFields.Count; i++)
        {
            agentField.Add((PlayerID)(i-1), resourceFields[i]);
        }

        for (int i = 0; i < stageData.agents.Count; i++)
        {
            SetAgentField((PlayerID)(i), stageData.agents[i], new intVector2(-1, -1));
        }
        SetAgentField(PlayerID.P0, stageData.player, new intVector2(1, 1));
    }

    void SetAgentField(PlayerID teamId, AgentData data, intVector2 direction)
    {
        var list = Field.GetEmptyTiles(agentField[teamId].GetTiles());
        foreach (var entityData in data.handEntities)
        {
            if (list.Count <= 0) break;
            var entity = EntityFactory.RequestEntity(entityData.data, direction,list[0], entityData.level);
            entity.team.teamNumber = teamId;
            list.RemoveAt(0);
        }
        foreach (var entityData in data.fieldEntities)
        {
            var tile = field.GetTile(entityData.Key);
            if (!tile) continue;
            var entity = EntityFactory.RequestEntity(entityData.Value.data, direction, tile, entityData.Value.level);
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

    IExecute curCmd;
    bool isSequencing = false;
    Queue<IExecute> commandList = new Queue<IExecute>();
    public void ReceiveCommands(List<IExecute> commands)
    {
        foreach (var item in commands)
        {
            commandList.Enqueue(item);
        }
        ExecuteCommands();
    }
    public void ExecuteCommands()
    {
        if (!isSequencing)
        {
            isSequencing = true;
            OnCommandExecuted();
        }
    }
    public virtual void OnCommandExecuted()
    {
        EditorLogger.Print($"count : {commandList.Count}");
        if (commandList.Count > 0)
        {
            curCmd = commandList.Dequeue();
            Action callback = OnCommandExecuted;
            StartCoroutine(curCmd.Execute(callback));
        }
        else
        {
            isSequencing = false;
            EditorLogger.Print("NoMore Command");
        }
    }
    #endregion
}
