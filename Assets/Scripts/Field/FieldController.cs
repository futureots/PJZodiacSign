using GlobalManage;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FieldController : MonoBehaviour
{
    /** FieldController
     * 레벨 기믹 수행
     * 페이즈-턴 운영
     * Agent 생성 및 필드 시스템과 Command 통신
     */
    [Header("Stage Operate")]
    protected StageManager stageManager;

    protected List<string> SpecialRule;   // TODO: 특수 기믹 DTO로 변경
    public Action<string> OnBattleEnd;    // NOTE: 전투 종료 플래그 (단순 string)
    
    [Header("Dependency")]
    [SerializeField] InputManager inputManager;
    [SerializeField] InputUIContainer inputUI;
    private EnemyAI _enemyAI = null;
    
    /// <summary>
    /// Initiate Controller
    /// </summary>
    /// <remarks>Load Model and set Command, Special Rule, and Reset Phase</remarks>
    public virtual void Init(StageData data)
    {
        /* 레벨 데이터로 씬 로드 준비
         * - 에이전트 목록 확인 및 생성
         */
        
        // Load Field
        stageManager = StageManager.Instance;
        if (!stageManager)
        {
            throw new Exception("StageManager not found");
        }
        stageManager.Init(data);

        // Set Agents
        _enemyAI = Instantiate(data.aiPrefab, transform);
        Agent.LocalPlayer = localPlayer;
        localPlayer.Init(this,PlayerID.P0,data.player, new intVector2(1,1));
        for (int i = 0; i < agents.Count && i < data.agents.Count; i++)
        {
            agents[i].Init(this,(PlayerID)i, data.agents[i], new intVector2(-1,-1));
        }
        inputManager.Init(localPlayer);
        _enemyAI.Init(agents[0]);
        inputUI.Init(inputManager);
        commandSystem = new CommandSystem();
        
        // Set Rules
        SpecialRule = data.specialRule;
        phases = data.phases;
        
        // Reset Phase
        turnCount = 0;
        SetPhase(0);
    }
    
    #region PhaseManage
    /**
     * 페이즈 - 턴 관리 시스템
     * - 페이즈별로 턴 순회
     * - 페이즈 전환, 턴 전환 Event
     */
    private int phaseIndex;
    private int turnIndex;
    
    [SerializeField] protected uint turnCount;       // loopCount
    public List<Phase> phases;     
    public Phase CurrentPhase => phases[phaseIndex];

    public Turn CurrentTurn => CurrentPhase.turnList[turnIndex];
    
    public event Action<Phase> OnPhaseStarted;
    public event Action<Turn, uint> OnTurnStarted;


    /// <summary>
    /// Phase Set and Reset Turn
    /// </summary>
    /// <param name="index">Phase index for set (-1 for Next Phase)</param>
    public virtual void SetPhase(int index = -1)
    {
        
        // Next Phase
        if (index == -1)
        {
            index = phaseIndex + 1;
        }
        
        // Invalid Phase Count
        if (index >= phases.Count)
        {
            if (IsBattleEnd(out PlayerID winner))
            {
                EndStage(winner);
            }
            return;
        }
        
        phaseIndex = index;
        // Set Model
        stageManager.SetPhase(CurrentPhase);
        OnPhaseStarted?.Invoke(CurrentPhase);
        
        // Reset Turn
        SetTurn(0);
    }

    /// <summary>
    /// Turn Set
    /// </summary>
    /// <param name="index">Turn index for Set, -1 for Next Turn</param>
    public virtual void SetTurn(int index = -1)
    {
        // Next Turn
        if (index == -1)
        {
            index = turnIndex + 1;
        }
        
        // Invalid Phase Count
        if (index >= CurrentPhase.turnList.Count)
        {
            // Loop Phase
            if (CurrentPhase.isLoop)
            {
                index = 0;

            }
            else
            {
                SetPhase();         // NOTE: Move to Next Phase When all Turn Ends
                return;
            }
        }

        // Set Turn
        turnIndex = index;
        if (index == 0)
        {
            turnCount++;
        }
        
        // Set Model
        stageManager.SetTurn(CurrentTurn);
        OnTurnStarted?.Invoke(CurrentTurn, turnCount);
    }
    
    public bool IsBattleEnd(out PlayerID winTeam)
    {
        List<PlayerID> surviveTeam = new();
        foreach (var tile in stageManager.field.GetTiles())
        {
            if (tile.IsEmpty) continue;
            if (tile.occupiedEntity.TryGetComponent<Entity>(out var entity))
            {
                if (entity.team.teamNumber == PlayerID.None) continue;
                if (!surviveTeam.Contains(entity.team.teamNumber))
                {
                    surviveTeam.Add(entity.team.teamNumber);
                }
            }
        }
        if(surviveTeam.Count == 1)
        {
            winTeam = surviveTeam[0];
            return true;
        }
        else
        {
            winTeam = PlayerID.None;
            return false;
        }
    }

    public virtual void EndStage(PlayerID winner)
    {
        stageManager.EndStage(winner);
        DataManager.Instance.playData.time = stageManager.timer.GetTime();
        DataManager.Instance.playData.point = stageManager.GetTotalPoint();
    }

    public virtual void EndGame()
    {
        // 패배 시 데이터 삭제 및 메인 화면으로 이동
        OnBattleEnd?.Invoke("FAIL");
    }

    public virtual void ContinueGame()
    {
        var credit = Math.Min(localPlayer.Credit,200);
        localPlayer.Credit += 150 + Mathf.RoundToInt(credit*0.2f);
        OnBattleEnd?.Invoke("CLEAR");
    }
    #endregion

    #region Commands
    /**
     * 
     */
    
    protected CommandSystem commandSystem;    // Attach
    
    /// <summary>
    /// 각 Agent에게서 실행할 command를 입력 받음.(모든 커맨드는 즉시 StageManager로 전송됨)
    /// </summary>
    /// <param name="cmd"></param>
    public void ReceiveCommands(Command cmd)
    {
        // 해당 커맨드에 대한 전처리 후 전송
        stageManager.ReceiveCommand(cmd);
        if (CurrentPhase.phaseName == PhaseType.Battle)
        {
            if (cmd is SkillCommand)
            {
                var checkCmd = new CheckCommand(this);
                stageManager.ReceiveCommand(checkCmd);
            }
        }
        
    }

    
    #endregion
    
    #region Agents
    
    // TODO: Agent 운영
    
    public Agent localPlayer;
    public List<Agent> agents;

    #endregion
    
    #region Debug

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            if (Input.GetKey(KeyCode.Alpha2))
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    EndStage(Agent.LocalPlayer.id);
                }
            }
        }
    }

    #endregion
}
