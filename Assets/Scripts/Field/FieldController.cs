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
    
    #region PhaseManager 
    /**
     * 페이즈 - 턴 관리 시스템
     * - 
     */
    [SerializeField] protected uint turnCount;       // turn Count in current Phase
    [SerializeField] public List<Phase> phases;     
    public Phase currentPhase { get; private set; }
    public event Action<Phase> OnPhaseChanged;
    public Turn currentTurn { get; private set; }
    public event Action<Turn> OnTurnChanged;
    
    public virtual void SetPhase(int index)
    {
        if (index >= phases.Count)
        {
            Debug.LogError($"Invalid Phase index");
            return;
        }

        currentPhase = phases[index];
        OnPhaseChanged?.Invoke(currentPhase);

        SetTurn(0);
    }

    public virtual void SetTurn(int index)
    {

    }
    
    #endregion
    
    #region Commands
    
    public List<Command> commandList;
    protected CommandSystem commandSystem;    // Attach
    
    public virtual void SendCommands()
    {
        //field에 커맨드 전송 및 콜백 함수 설정
    }
    public virtual void OnCommandExecuted()
    {

    }
    
    #endregion
    
    #region Agents

    public Agent localPlayer;
    public List<Agent> agents;
    
    #endregion

    /// <summary>
    /// Initiate Controller
    /// </summary>
    /// <remarks>필드, 상점 주입받고 전투 Model을 초기화</remarks>
    public virtual void Init(StageData data)
    {
        /* 레벨 데이터로 씬 로드 준비
         * - 에이전트 목록 확인 및 생성
         */
        
        // Load Field
        stageManager = StageManager.Instance;
        stageManager.Init(data);
        
        // 에이전트 생성 및 초기화
        // localPlayer.SetData(data.player);
        for (int i = 0; i < agents.Count || i < data.agents.Count; i++)
        {
            // agents[i].SetData(data.agents[i]);
        }
        commandSystem = new();
        commandList = new();
        
        // TODO: 기믹 세팅
        SpecialRule = data.specialRule;
        
        // TODO: 페이즈 초기화
        turnCount = 0;
        // currentPhase = phases[0];
    }
}
