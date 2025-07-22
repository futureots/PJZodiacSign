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
        turnManager.turns.AddLast(new RepairTurn());
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

    public bool CheckGameEnd()
    {
        if (IsGameEnd(out int winner))
        {
            if (agents[winner] is InputManager)
            {
                Debug.Log("승리");
                // 데이터 저장
                dataManager.SetData(agents[winner].agentData, level);
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
            Debug.Log(entityTeam.teamNumber);
            if (!teams.Contains(entityTeam.teamNumber))
            {
                Debug.Log("count : " + teams.Count);
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
    public bool IsGameEnd()
    {
        int dummy;
        return IsGameEnd(out dummy);
    }
    
    #endregion

}
