using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : Singleton<GameManager>
{
    public EntityController[] controllers;
    public Field field
    {
        get
        {
            return Field.Instance;
        }
    }

    public List<Entity> objects;

    #region Turn
    public TurnManager turnManager;
    public Button turnEndButton;
    #endregion
    private void Awake()
    {
        //데이터 기반 엔티티 불러오기 및 필드 생성
        /*//field.CreateField();
        var data = new PlayerData();//PartyData.LoadPartyData("CurrentPlayerParty");
        data.entities.Add(new EntityData(Jodiac.Mouse, Element.Water, 1));
        data.location.Add("Elite", 1);
        data.location["Elite"] += 1;
        data.SavePlayerData("Test");

        for(int i = 0; i < controllers.Length; i++)
        {
            //controllers[i].SetEntities(i+1, data);
        }
        //StartGame();*/
    }
    void Start()
    {
        int i = 0;
        foreach (var item in objects)
        {
            item.MoveTo(field.GetTile(i,i));
            i += 2;
        }
        
        turnManager = this.AddComponent<TurnManager>();
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
        var tiles = field.GetTiles();
        bool isPlayerAlive = false;
        bool isEnemyAlive = false;
        foreach (var tile in tiles)
        {
            if (tile.isEmpty) continue;
            var entityTag = tile.occupiedObject.tag;
            if (entityTag == "Player")
            {
                isPlayerAlive = true;
                Debug.Log($"{tile.occupiedObject} player is Alive");
            }
            else if (entityTag == "Enemy") {
                isEnemyAlive = true;
                Debug.Log($"{tile.occupiedObject} enemy is Alive");
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
