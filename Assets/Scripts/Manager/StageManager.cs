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
    public Timer timer;
    public int level;
    public bool isLastLevel;
    public int point;
    
    // key = teamNum, value = ResourceField
    [SerializeField] List<Field> resourceFields;
    public Dictionary<PlayerID, Field> agentField;
    
    public event Action<PlayerID> OnStageEnded;

    public void EndStage(PlayerID winner)
    {
        // 시간 저장
        timer.Pause();
        DataManager.Instance.playData.time = timer.GetTime();
        if (winner == Agent.LocalPlayer.id)
        {
            // 스테이지 클리어 점수 제공
            const float decayRate = 1000;
            const int levelMultiplier = 10;
            point = Mathf.RoundToInt(level * 1000 * Mathf.Exp(-timer.GetElapsedTime()/decayRate));
            
            // 살아있는 기물 수 + 강화단계 합
            List<Entity> data = field.GetEntities(Agent.LocalPlayer.id);
            data.ForEach(entity => point += (entity.Level + 1) * (entity.Level + 1) * levelMultiplier);
            DataManager.Instance.playData.point += point;
        }
        
        OnStageEnded?.Invoke(winner);
    }
    /// <summary>
    /// Init Model with Data
    /// </summary>
    /// <param name="stageData">stage Data to set</param>
    /// <remarks>Injected data by Controller</remarks>
    public void Init(StageData stageData)
    {
        // Get Level
        level = stageData.level;
        isLastLevel = stageData.isLastLevel;
        
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
        
        timer.Init(stageData.time);
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
