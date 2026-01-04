
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
    protected StageManager stageManager;

    protected List<string> SpecialRule;   // TODO: 특수 기믹 DTO로 변경
    
    #region PhaseManage
    /**
     * 페이즈 - 턴 관리 시스템
     * - 페이즈별로 턴 순회
     * - 페이즈 전환, 턴 전환 Event
     */
    private int phaseIndex;
    private int turnIndex;
    
    [SerializeField] protected uint turnCount;       // turn Count in current Phase
    [SerializeField] public List<Phase> phases;     
    public Phase CurrentPhase => phases[phaseIndex];

    public Turn CurrentTurn => CurrentPhase.turnList[turnIndex];
    
    public event Action<Phase> onPhaseStarted;
    public event Action<Turn> onTurnStarted;

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
            Debug.LogError($"Invalid Phase index");
            return;
        }
        
        phaseIndex = index;

        // Set Model
        stageManager.SetPhase(CurrentPhase);
        onPhaseStarted?.Invoke(CurrentPhase);
        
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
        turnCount++;
        
        // Set Model
        stageManager.SetTurn(CurrentTurn);
        onTurnStarted?.Invoke(CurrentTurn);
    }
    
    #endregion
    
    #region Commands
    /**
     * 
     */
    
    public List<Command> commandList;
    protected CommandSystem commandSystem;    // Attach
    
    public virtual void SendCommands()
    {
        //TODO: field에 커맨드 전송 및 콜백 함수 설정
    }
    public virtual void OnCommandExecuted()
    {

    }
    
    #endregion
    
    #region Agents
    /**
     * 
     */
    public Agent localPlayer;
    public List<Agent> agents;
    
    #endregion

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
        stageManager.Init(data);
        
        // TODO: 에이전트 생성 및 초기화
        // localPlayer.SetData(data.player);
        for (int i = 0; i < agents.Count || i < data.agents.Count; i++)
        {
            // agents[i].SetData(data.agents[i]);
        }
        commandSystem = new();
        commandList = new();
        
        // TODO: 기믹 세팅
        SpecialRule = data.specialRule;
        
        // Reset Phase
        turnCount = 0;
        SetPhase(0);
    }
}
