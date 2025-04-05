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

    public Battle.Entity obj;
    int turnCount;
    public Action turnEnd;
    private void Awake()
    {
        StartTurn();
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
    IEnumerator Start()
    {
        obj.MoveTo(field.GetTile(0, 0));
        yield return new WaitForSeconds(1);
        //obj.MoveTo(field.GetTile(5, 5));
    }
    public void StartTurn()
    {
        InputManager.Instance.AllocateMoveCommand();
    }
    public void EndTurn()
    {
        turnEnd?.Invoke();
        StartCoroutine(EndTurnCoroutine());
    }

    private IEnumerator EndTurnCoroutine()
    {
        // 입력 금지
        InputManager.Instance.isInputStop = true;

        // 각 컨트롤러의 입력한 커맨드 가져오기
        List<Command> commands = new List<Command>();
        foreach (EntityController controller in controllers)
        {
            var cmd = controller.curCmd;
            //if (cmd == null) cmd = controller.GetRandomCommand();
            commands.Add(cmd);
        }
        // 커맨드 실행
        foreach(var cmd in commands)
        {
            cmd.Execute();
            yield return new WaitForSeconds(1f);
        }
        /*
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
        */
        // 죽은 기물 제거
        field.CleanField();

        // 입력 재개
        InputManager.Instance.isInputStop = false;
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

}
