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
    /// 게임 시작 또는 재개하기(데이터 불러오기)
    /// </summary>
    public void GameStart()
    {
        // 게임에 필요한 데이터 가져오거나 생성
        dataManager.LoadAllData("data");
        var list = dataManager.GetData();
        for(int i = 0; i < list.Length; i++)
        {
            agents[i].SetData(list[i]);
        }
        level = dataManager.playerData.stageLevel;
        // 게임 시작용 턴 생성
        var turnManager = GetComponent<TurnManager>();
        if (turnManager == null) return;
        turnManager.turns.AddLast(new RepairTurn(level));
        turnManager.StartTurn();
    }

    #endregion

    #region GameEnd
    /// <summary>
    /// 게임 종료 및 메인화면으로 이동
    /// </summary>
    void GameEnd()
    {

    }
    IEnumerator GoNextLevel()
    {
        level += 1;
        yield return new WaitForSeconds(10);

        // 이거도 개별 Camera마다 필요할 수 있음
        if(hpManager != null) hpManager.ClearHpBar();

        OnNextLevel?.Invoke(level);
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
                // 데이터 저장
                dataManager.SetData(winAgent.GetAgentData(), level);
                dataManager.SaveAllData("Data");

                StartCoroutine(GoNextLevel());
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
    
    /// <summary>
    /// 기물의 체력바 UI 세팅, 임시로 main카메라에만 세팅 나중에 카메라 별 세팅 추가 예정(멀티 일때만)
    /// </summary>
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
