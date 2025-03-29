using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    public EntityController[] controllers;
    public Field field;

    int turnCount;
    public Action turnEnd;
    private void Awake()
    {
        field.CreateField();
        var data = PartyData.LoadPartyData("CurrentPlayerParty");
        for(int i = 0; i < controllers.Length; i++)
        {
            controllers[i].SetEntities(i+1, data);
        }
        StartGame();
    }
    public void StartGame()
    {
        InputManager.Instance.inputMode = InputManager.InputMode.Set;
        turnCount = 0;
    }
    public void StartTurn()
    {
        InputManager.Instance.inputMode = InputManager.InputMode.Move;
    }
    public void EndTurn()
    {
        turnEnd?.Invoke();
        StartCoroutine(EndTurnCoroutine());
    }

    private IEnumerator EndTurnCoroutine()
    {
        InputManager.Instance.inputMode = InputManager.InputMode.None;
        //이동, 스킬 사용
        if (turnCount !=0)
        {
            List<Command> commands = new List<Command>();
            foreach (EntityController controller in controllers)
            {
                var cmd = controller.GetCommand();
                if (cmd == null) cmd = controller.GetRandomCommand();
                commands.Add(cmd);
            }
            foreach(var cmd in commands)
            {
                cmd.Execute();
                yield return new WaitForSeconds(1f);
            }
        }
        //공격
        if (turnCount >= 2)
        {

            foreach (var controller in controllers)
            {
                foreach (var entity in controller.entities)
                {
                    entity.Activate();
                }
            }
            turnCount = 0;
            controllers = controllers.Reverse().ToArray();
            turnEnd?.Invoke();
        }
        
        turnCount++;

        // 죽은 기물 제거
        field.CleanEntity();

        //한쪽 기물 전부 사망 시 게임 종료
        
        StartTurn();
    }

    #region Viewer
    [SerializeField] Material attackMaterial;
    public void ViewAttackArea(int teamNum)
    {
        List<EntityController> enemy = new List<EntityController>();
        foreach (var item in controllers)
        {
            if(item.teamNum != teamNum)
            {
                enemy.Add(item);
            }
        }
        foreach (var controller  in enemy)
        {
            foreach (var item in controller.entities)
            {
                var area = item.GetAttackArea(item.curPos);
                field.AddFieldColor(2, area.ToArray());
            }
        }
    }
    public void ClearAttackArea()
    {
        field.RemoveFieldColor(2);
    }
    #endregion

    #region Save
    public void SaveCurrentState(int team)
    {
        List<PartyEntity> list = new List<PartyEntity>();
        foreach (var tile in field.tiles)
        {
            var entity = tile.OccupiedEntity;
            if (entity != null)
            {
                if (entity.TeamNum != team) continue;
                PartyEntity temp = new PartyEntity(entity.jodiacType, entity.elementType);
                Debug.Log(entity.jodiacType + " : " + entity.elementType);
                list.Add(temp);
            }
        }
        PartyData data = new PartyData();
        data.Entities = list.ToArray();
        data.SavePartyData("CurrentPlayerParty");
    }
    #endregion
}
