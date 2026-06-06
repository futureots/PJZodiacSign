using GlobalManage;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    /** FieldController
     * 레벨 기믹 수행
     * 페이즈-턴 운영
     * Agent 생성 및 필드 시스템과 Command 통신
     */
    
    [SerializeField] private StageData stageData;
    
    [Header("Dependency")]
    protected StageManager StageManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private InputUIContainer inputUI;
    private EnemyAI _enemyAI = null;
    
    [Header("Agents")]
    public Agent localPlayer;
    public List<Agent> agents;
    
    [Header("Extra Rules")]
    [SerializeField] private List<ExSystem> exSystems;
    
    /// <summary>
    /// Initiate Controller
    /// </summary>
    /// <remarks>Load Model and set Command, Special Rule, and Reset Phase</remarks>
    public virtual void Init(StageData newData)
    {
        /* 레벨 데이터로 씬 로드 준비
         * - 에이전트 목록 확인 및 생성
         * - 페이즈 데이터 주입
         * - 특수 기믹 
         */
        
        stageData = newData;
        
        // Load Field
        try
        {
            StageManager = StageManager.Instance;
            StageManager.Init(stageData);
        }
        catch(Exception e)
        {
            EditorLogger.PrintError($"StageManager Not Found : {e.Message}");
            GameManager.Instance.EndGame();
        }

        // Set Enemy
        _enemyAI = Instantiate(stageData.aiPrefab, transform);
        Agent.LocalPlayer = localPlayer;
        localPlayer.Init(this,PlayerID.P0,stageData.player, new intVector2(1,1));
        for (int i = 0; i < agents.Count && i < stageData.agents.Count; i++)
        {
            agents[i].Init(this,(PlayerID)i, stageData.agents[i], new intVector2(-1,-1));
        }
        inputManager.Init(localPlayer);
        _enemyAI.Init(agents[0]);
        inputUI.Init(inputManager);
        
        // Init Extra Trigger
        foreach (ExSystem exSystem in exSystems)
        {
            exSystem.Init(this, stageData);
        }

        // 전투 시작
        StartLevel(stageData);
    }
    
    #region CycleManage
    /**
     * 페이즈 - 턴 관리 시스템
     * - 페이즈별로 턴 순회
     * - 페이즈 전환, 턴 전환 Event
     */
    
    private int _phaseIndex;
    private int _turnIndex;
    [SerializeField] protected uint turnCount;       // turn Count in current Phase

    private List<Phase> PhaseData => stageData.phases;     
    
    public Phase CurrentPhase => PhaseData[_phaseIndex];

    public Turn CurrentTurn => CurrentPhase.turnList[_turnIndex];

    public event Action<StageData> OnLevelStart;
    public event Action<Phase> OnPhaseStart;
    public event Action<Turn> OnTurnStarted;

    public Action<string> OnBattleEnd;
    
    public virtual void StartLevel(StageData data)
    {
        // 새 레벨 시작
        OnLevelStart?.Invoke(data);
        // 증강 추가 자체를 하나의 증강으로 처리
        
        // NOTE: 게임 시작 전 행동
        
        // 페이즈 시작
        SetPhase(0);
    }

    /// <summary>
    /// Phase Start and Reset Turn
    /// </summary>
    /// <param name="index">Phase index for set (-1 for Next Phase)</param>
    public virtual void SetPhase(int index = -1)
    {
        // Next Phase
        if (index == -1)
        {
            index = _phaseIndex + 1;
        }
        
        // Invalid Phase Count 
        if (index >= PhaseData.Count)
        {
            // Check Battle End
            if (IsBattleEnd(out PlayerID winner))
            {
                EndStage(winner);
            }
            return;
        }
        
        // Change Phase
        _phaseIndex = index;
        turnCount = 0;
        
        // Set Model
        StageManager.SetPhase(CurrentPhase);
        OnPhaseStart?.Invoke(CurrentPhase);
        
        // Reset Turn
        SetTurn(0);
    }

    /// <summary>
    /// Turn Start
    /// </summary>
    /// <param name="index">Turn index for Set, -1 for Next Turn</param>
    public virtual void SetTurn(int index = -1)
    {
        // Next Turn
        if (index == -1)
        {
            index = _turnIndex + 1;
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
                SetPhase();         // Move to Next Phase When all Turn Ends
                return;
            }
        }

        // Set Turn
        _turnIndex = index;
        turnCount++;
        
        // Set Model
        StageManager.SetTurn(CurrentTurn);
        OnTurnStarted?.Invoke(CurrentTurn);
    }
    
    public bool IsBattleEnd(out PlayerID winTeam)
    {
        List<PlayerID> surviveTeam = new();
        
        // Get All Teams with Any Entity
        foreach (var tile in StageManager.field.GetTiles())
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
        
        // Check Win Team
        if(surviveTeam.Count == 1)
        {
            winTeam = surviveTeam[0];
            return true;
        }
        
        // Continue Battle
        winTeam = PlayerID.None;
        return false;
    }

    public virtual void EndStage(PlayerID winner)
    {
        StageManager.EndStage(winner);
        DataManager.Instance.playData.time = StageManager.timer.GetTime();
        DataManager.Instance.playData.point = StageManager.GetTotalPoint();
    }

    public virtual void EndGame()
    {
        // 패배 시 데이터 삭제 및 메인 화면으로 이동
        OnBattleEnd?.Invoke("FAIL");
    }

    public virtual void ContinueGame()
    {
        var credit = Math.Min(localPlayer.Credit,200);
        localPlayer.Credit += 100 + Mathf.RoundToInt(credit*0.2f);
        OnBattleEnd?.Invoke("CLEAR");
    }
    #endregion

    #region AgentCommands
    
    /// <summary>
    /// 각 Agent에게서 실행할 command를 입력 받음.(모든 커맨드는 즉시 StageManager로 전송됨)
    /// </summary>
    /// <param name="cmd"></param>
    public void ReceiveCommands(Command cmd)
    {
        // 해당 커맨드에 대한 전처리 후 전송
        StageManager.ReceiveCommand(cmd);
        if (CurrentPhase.phaseName == PhaseType.Battle)
        {
            if (cmd is SkillCommand)
            {
                var checkCmd = new CheckCommand(this);
                StageManager.ReceiveCommand(checkCmd);
            }
        }
        
    }
    
    #endregion
}
