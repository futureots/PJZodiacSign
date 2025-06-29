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
    //public Button turnEndButton;
    public Field instantField;


    private void Awake()
    {
        // 필드 생성
        //field.CreateField();

        // 플레이어 데이터 불러오기
        var playerData = DataManager.Instance.playerData;

        Debug.Log(playerData.entities.Count);
        foreach (var item in playerData.entities)
        {
            var resource = ResourceManager.GetEntityResource(item.name);
            var instance = Instantiate(resource);
            var entity = instance.GetComponent<Entity>();
            controllers[0].SetEntity(entity);
        }

        // 적 데이터 불러오기
        var enemyData = DataManager.Instance.curLocation;

        foreach (var item in enemyData.enemyList)
        {
            var resource = ResourceManager.GetEntityResource(item.name);
            var instance = Instantiate(resource);
            var entity = instance.GetComponent<Entity>();
            controllers[1].SetEntity(entity);
        }
        DataManager.Instance.SaveAllData("Data");
    }

    #region GameEnd
    public bool CheckGameEnd()
    {
        bool isWin;
        if (!IsGameEnd(out isWin)) return false;
        if (isWin)
        {
            Debug.Log("승리");
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
