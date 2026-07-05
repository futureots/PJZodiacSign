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
    public Field field;
    public Timer timer;
    public int level;
    public bool isLastLevel;
    public int point;
    public int prevPoint;
    public int GetTotalPoint() => point + prevPoint;
    
    
    // key = teamNum, value = ResourceField
    [SerializeField] List<Field> resourceFields;
    public Dictionary<PlayerID, Field> agentField;
    
    public event Action<PlayerID> OnStageEnded;

    public void EndStage(PlayerID winner)
    {
        // 시간 저장
        timer.Pause();
        
        if (winner == Agent.LocalPlayer.id)
        {
            // 스테이지 클리어 점수 제공
            const float decayRate = 1000;
            const int levelMultiplier = 10;
            point = Mathf.RoundToInt(level * 1000 * Mathf.Exp(-timer.GetElapsedTime()/decayRate));
            
            // 살아있는 기물 수 + 강화단계 합
            List<Entity> data = field.GetEntities(Agent.LocalPlayer.id);
            data.ForEach(entity => point += (entity.Level + 1) * (entity.Level + 1) * levelMultiplier);
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
        prevPoint = stageData.point;
    }

    /// <summary>
    /// Set Model to Current Phase
    /// </summary>
    /// <param name="newPhase">new Phase Info</param>
    public void SetPhase(Phase newPhase)
    {

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
            get { return _count; }
            set
            {
                _count = value;
                isSequencing?.Invoke(_count);
            }
        }
        public Action<int> isSequencing;

        Queue<IExecute> commandQueue = new Queue<IExecute>();

    // 현재 큐를 처리하는 단일 루프가 실행 중인지 확인하는 플래그
        private bool _isProcessing = false; 

        public void ReceiveCommand(IExecute command)
        {
            commandQueue.Enqueue(command);
        
            // 처리가 진행 중이 아닐 때만 단일 처리 코루틴을 시작합니다.
            if (!_isProcessing)
            {
                StartCoroutine(ProcessQueueRoutine());
            }
        }

    // 상호 재귀 없이 단일 루프로 모든 큐를 기획 의도대로 처리하는 핵심 코루틴
        IEnumerator ProcessQueueRoutine()
        {
            _isProcessing = true;

            // 큐에 커맨드가 존재하는 동안 루프를 계속 돕니다.
            while (commandQueue.Count > 0)
            {
                IExecute nextCommand = commandQueue.Peek();

                if (nextCommand is CheckCommand cmd)
                {
                    // 중요 커맨드: 큐에서 꺼낸 뒤 대기 로직 수행
                    commandQueue.Dequeue();
                
                    // 1. 실행 중인 일반 커맨드(Count)가 0이 될 때까지 대기
                    yield return new WaitUntil(() => Count == 0);
                
                    // 2. 중요 커맨드 실행 완료까지 대기
                    yield return StartCoroutine(cmd.Execute());

                    // 3. EndCommand 로직이 필요하다면 여기에 위치
                    /*
                    if (cmd is EndCommand)
                    {
                        foreach (var c in commandQueue) c.Delete();
                        commandQueue.Clear();
                    }
                    */
                }
                else
                {
                    // 일반 커맨드: 큐에서 꺼낸 뒤 즉시 실행 (yield가 없으므로 프레임 소모 없이 동시 실행됨)
                    IExecute normalCmd = commandQueue.Dequeue();
                    ExecuteCommand(normalCmd);
                }
            }

            // 큐가 완전히 비워지면 처리 상태를 해제합니다.
            _isProcessing = false;
        }

        void ExecuteCommand(IExecute command)
        {
            void Callback() => Count--;
            Count++;
            this.RunWithCallback(command.Execute(), Callback);
        }

    #endregion
}
