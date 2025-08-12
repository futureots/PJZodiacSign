using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : Singleton<GameManager>
{
    public Agent[] agents;

    public int level {  get; private set; }
    
    [SerializeField] Field _field;
    public Field field
    {
        get { return _field; }
    }

    DataManager dataManager;
    public HpPanelManager hpManager;

    private void Awake()
    {
        dataManager = this.GetOrAddComponent<DataManager>();
    }
    private void Start()
    {
        GameStart();
    }
    #region GameStart

    /// <summary>
    /// 게임 시작 또는 다음 레벨(레벨이 증가했을 때)
    /// </summary>
    public void GameStart()
    {
        // 에이전트에 필요한 데이터를 설정하거나 로드
        dataManager.LoadAllData("data");
        var list = dataManager.GetData();
        for(int i = 0; i < list.Length; i++)
        {
            agents[i].SetData(list[i]);
        }
        level = dataManager.playerData.stageLevel;
        
        // 새로운 페이즈 시스템 사용
        var phaseManager = GetComponent<PhaseManager>();
        if (phaseManager == null) return;
        
        // 첫 번째 레벨 시작
        phaseManager.NextLevel(level);
    }

    #endregion

    #region GameEnd
    /// <summary>
    /// 게임 종료 시 처리
    /// </summary>
    public void GameEnd()
    {
        Debug.Log("게임 종료");
        // 게임 종료 처리 로직 추가
    }
    
    /// <summary>
    /// 다음 레벨로 진행하는 메서드
    /// </summary>
    public void GoToNextLevel()
    {
        StartCoroutine(GoToNextLevelCoroutine());
    }
    
    private IEnumerator GoToNextLevelCoroutine()
    {
        // 체력바 UI 정리
        if(hpManager != null) hpManager.ClearHpBar();
        
        // 승리 효과 표시 시간 (3초)
        yield return new WaitForSeconds(3f);
        
        // 레벨 증가
        level += 1;
        
        // 새로운 레벨의 페이즈 시작
        var phaseManager = GetComponent<PhaseManager>();
        if (phaseManager != null)
        {
            phaseManager.NextLevel(level);
        }
    }
    
    public static Action<int> OnNextLevel;
    
    public bool CheckGameEnd()
    {
        if (IsGameEnd(out int winner))
        {
            Agent winAgent = null;
            for(int i = 0; i < agents.Length; i++)
            {
                if (agents[i].team.teamNumber == winner) winAgent = agents[i];
            }
            if(winAgent == null) return false;
            if (winAgent is InputManager)
            {
                Debug.Log("승리");
                // 플레이어 데이터 저장
                dataManager.SetData(winAgent.GetAgentData(), level);
                dataManager.SaveAllData("Data");
            }
            else
            {
                Debug.Log("패배...");
                GameEnd();
            }
            return true;
        }
        return false;
    }
    
    public bool IsGameEnd(out int winner)
    {
        var tiles = _field.GetTiles();
        bool isEnd = false;
        List<int> teams = new List<int>();
        foreach (var tile in tiles)
        {
            if (tile.isEmpty) continue;
            var entityTeam = tile.occupiedObject.GetComponent<Team>();
            if (entityTeam == null) continue;
            if (!teams.Contains(entityTeam.teamNumber))
            {
                teams.Add(entityTeam.teamNumber);
            }
        }
        if (teams.Count == 1)
        {
            winner = teams[0];
            isEnd = true;
        }
        else
        {
            winner = -1;
        }
        return isEnd;
    }
    

    public void SetEntityHpBar()
    {
        if (hpManager == null) return;
        foreach (var item in agents)
        {
            foreach(var entity in item.controller.entities)
            {
                hpManager.CreateHpBar(entity);
            }
        }
    }
    #endregion

}
