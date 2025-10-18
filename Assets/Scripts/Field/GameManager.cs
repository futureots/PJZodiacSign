using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static Action<int> onNextLevel;
    // 0번은 플레이어 1번은 적AI
    public Agent[] agents;


    //public EntityFactory entityInstaller;

    DataManager dataManager;
    public HpPanelManager hpManager;
    public TeamColorTable teamColorTable;

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        dataManager = DataManager.Instance;
    }
    private void Start()
    {
        StartGame();
    }
    public Agent GetOppositeAgent(Agent agent)
    {
        return agent == agents[0] ? agents[1] : agents[0];
    }
    public int level { get; private set; }

    [SerializeField] Field _field;
    public Field field
    {
        get { return _field; }
    }

    #region GameStart

    /// <summary>
    /// 게임 시작 또는 다음 레벨(레벨이 증가했을 때)
    /// </summary>
    public void StartGame()
    {
        // 에이전트에 필요한 데이터를 설정하거나 로드
        dataManager.LoadAllData("data");
        var list = dataManager.GetData();

        
        for(int i = 0; i < list.Length; i++)
        {
            agents[i].SetData(list[i]);
        }
        level = dataManager.playerData.stageLevel;

        // 해당 레벨의 정비 페이즈 부터 시작(없을 경우 0레벨부터 시작)
        var phaseManager = GetComponent<PhaseManager>();
        if (phaseManager == null) return;
        onNextLevel?.Invoke(level);
        //phaseManager.NextLevel(dataManager.playerData.stageLevel);
    }

    #endregion

    #region GameEnd
    /// <summary>
    /// 게임 종료 시 처리
    /// </summary>
    void EndGame()
    {
        Debug.Log("게임 종료");
        // 게임 종료 처리 로직 추가
    }
    
    IEnumerator GoNextLevel()
    {
        level += 1;
        yield return new WaitForSeconds(3);

        // 이벤트 발생 전 Camera 관련 처리
        if(hpManager != null) hpManager.ClearHpBar();

        onNextLevel?.Invoke(level);
    }
    
    
    

    
    public bool HasGameEnded(out Agent winner)
    {
        var tiles = _field.GetTiles();
        
        // LINQ를 사용해서 필드에 남아있는 팀 번호들을 수집
        var teams = tiles
            .Where(t => !t.isEmpty)
            .Select(t => t.occupiedObject.GetComponent<Team>())
            .Where(t => t != null)
            .Select(t => t.teamNumber)
            .Distinct()
            .ToList();
        
        // 팀이 하나만 남아있다면 승리 조건
        if (teams.Count == 1)
        {
            // LINQ FirstOrDefault를 사용해서 해당 팀의 에이전트를 찾기
            winner = agents.FirstOrDefault(a => a.team.teamNumber == teams[0]);
            return winner != null;
        }

        winner = null;
        return false;
    }
    
    /// <summary>
    /// 전투 승리 시 다음 레벨로 진행 (BattlePhase용)
    /// </summary>
    public bool HandleBattleVictory(Agent winner)
    {
        if (winner is InputManager)
        {
            Debug.Log("전투 승리! 다음 레벨로 진행합니다.");
            
            // 플레이어 데이터 저장
            dataManager.SetData(winner.UpdateAgentData(), level);
            dataManager.SaveAllData("Data");
            
            // 다음 레벨로 진행
            StartCoroutine(GoNextLevel());
            return true;
        }
        else
        {
            Debug.Log("게임 오버...");
            EndGame();
            return false;
        }
    }
    

    public void SetEntityHpBar()
    {
        if (hpManager == null) return;
        foreach (var item in agents)
        {
            foreach(var entity in item.controller.fieldEntities)
            {
                hpManager.CreateHpBar(entity);
            }
        }
    }
    #endregion

}
