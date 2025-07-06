using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : Singleton<GameManager>
{
    public EntityController[] controllers;

    [SerializeField] Field _field;
    public Field field
    {
        get { return _field; }
    }


    // 턴 조작
    public TurnManager turnManager;

    public EntityTable enemyTable;
    public StageCostData stageData;


    private void Start()
    {
        /*var playerData = DataManager.Instance.playerData;
        var enemyData = stageData.GetStageCost(playerData.stageLevel);
        var enemylist = new List<string>();
        while(enemyData.cost1 > 0)
        {
            enemyTable.GetEntityData(0, ref enemyData.cost1);
            Debug.Log(enemyData.cost1);
        }*/

    }

    #region GameEnd
    public bool CheckGameEnd()
    {
        bool isWin;
        if (!IsGameEnd(out isWin)) return false;
        if (isWin)
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
    public bool IsGameEnd(out bool isWin)
    {
        var tiles = _field.GetTiles();
        bool isPlayerAlive = false;
        bool isEnemyAlive = false;
        foreach (var tile in tiles)
        {
            if (tile.isEmpty) continue;
            var entityTag = tile.occupiedObject.tag;
            if (entityTag == "Player")
            {
                isPlayerAlive = true;
                //Debug.Log($"{tile.occupiedObject} player is Alive");
            }
            else if (entityTag == "Enemy") {
                isEnemyAlive = true;
                //Debug.Log($"{tile.occupiedObject} enemy is Alive");
            }
            if (isPlayerAlive && isEnemyAlive) break;
        }
        if (!isEnemyAlive)
        {
            isWin = true;
        }
        else
        {
            isWin = false;
        }
        return !(isPlayerAlive && isEnemyAlive);
    }
    public bool IsGameEnd()
    {
        bool dummy;
        return IsGameEnd(out dummy);
    }
    
    #endregion

}
