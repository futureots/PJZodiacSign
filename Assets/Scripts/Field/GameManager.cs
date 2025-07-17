using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : Singleton<GameManager>
{
    public Agent[] teams;

    public EntityController[] controllers
    {
        get
        {
            var list = new List<EntityController>();
            foreach (var team in teams)
            {
                var controller = team.GetComponent<EntityController>();
                if (controller != null)
                {
                    list.Add(controller);
                }
            }
            return list.ToArray();
        }
    }

    [SerializeField] Field _field;
    public Field field
    {
        get { return _field; }
    }


    // 턴 조작
    public TurnManager turnManager;




    #region GameEnd
    public bool CheckGameEnd()
    {
        int winner;
        if (!IsGameEnd(out winner)) return false;
        if (teams[winner] is InputManager)
        {
            Debug.Log("승리");
            // 데이터 저장
            DataManager.Instance.SaveAllData("Data");
        }
        else
        {
            Debug.Log("패배...");
        }
        return true;
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
    public bool IsGameEnd()
    {
        int dummy;
        return IsGameEnd(out dummy);
    }
    
    #endregion

}
