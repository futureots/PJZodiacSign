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
    public event Action<Turn> OnTurnStarted;


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
            if (IsBattleEnd(out var winner))
            {
                GameManager.Instance.ExitBattle(winner);
            }
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
        OnTurnStarted?.Invoke(CurrentTurn);
    }
    
    public bool IsBattleEnd(out PlayerID winTeam)
    {
        List<PlayerID> surviveTeam = new List<PlayerID>();
        foreach (var tile in stageManager.field.GetTiles())
        {
            if (tile.isEmpty) continue;
            if (tile.occupiedObject.TryGetComponent<Entity>(out var entity))
            {
                if (entity.team.teamNumber == PlayerID.None) continue;
                if (!surviveTeam.Contains(entity.team.teamNumber))
                {
                    surviveTeam.Add(entity.team.teamNumber);
                }
            }
        }
        EditorLogger.Print(surviveTeam.Count);
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
    #endregion

    #region Commands
    /**
     * 
     */

    public int capacity;
    public List<Command> inputCommands;
    public event Action<List<Command>> OnListUpdated;
    
    protected CommandSystem commandSystem;    // Attach
    
    public void ReceiveCommands(Command cmd)
    {
        for (int i = inputCommands.Count - 1; i >= 0; i--)
        {
            if (cmd.IsOverlap(inputCommands[i]))
            {
                inputCommands[i].Delete();
                inputCommands.RemoveAt(i);
            }
        }

        inputCommands.Add(cmd);
        if(cmd is not EndCommand)
        {
            if (capacity == -1) { }
            else if(inputCommands.Count > capacity)
            {
                var trashCmd = inputCommands[0];
                inputCommands.RemoveAt(0);
                trashCmd.Delete();
            }
        }
        OnListUpdated?.Invoke(inputCommands);

        switch (CurrentPhase.phaseName)
        {
            case PhaseType.Repair:
                SendCommands();
                break;
            case PhaseType.Battle:
                if (cmd is EndCommand)
                {
                    SendCommands();
                }
                break;
        }
    }


    public virtual void SendCommands()
    {
        List<IExecute> ExecuteCommands = new List<IExecute>();
        foreach(var cmd in inputCommands)
        {
            ExecuteCommands.Add(cmd);
        }
        stageManager.ReceiveCommands(ExecuteCommands);
        inputCommands.Clear();
        OnListUpdated?.Invoke(inputCommands);
    }

    public void DeleteCommand(int index)
    {
        if (index < 0 || index >= inputCommands.Count) return;
        var cmd = inputCommands[index];
        inputCommands.RemoveAt(index);
        cmd?.Delete();
    }

    
    #endregion
    
    #region Agents
    /**
     * 
     */
    public Agent localPlayer;
    public List<Agent> agents;

    #endregion

    [Header("Dependency")]
    [SerializeField] InputManager inputManager;
    [SerializeField] EnemyAI enemyAI;
    [SerializeField] InputUIContainer inputUI;

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
        localPlayer.Init(this,PlayerID.P0,data.player.credit);
        for (int i = 0; i < agents.Count || i < data.agents.Count; i++)
        {
            agents[i].Init(this,(PlayerID)(i), data.agents[i].credit);
        }
        inputManager.Init(localPlayer);
        // TODO : 여러개면 for문 내부에서 돌리기(AI도 여러개로 세팅)
        enemyAI.Init(agents[0]);
        inputUI.Init(inputManager);
        commandSystem = new();
        inputCommands = new();
        
        // TODO: 기믹 세팅
        SpecialRule = data.specialRule;
        
        // Reset Phase
        turnCount = 0;
        SetPhase(0);
    }
}
