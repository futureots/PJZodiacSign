using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public EntityController controller;
    public Field field;
    private void Awake()
    {
        field.CreateField();
        var data = PartyData.LoadPartyData();
        controller.SetEntities(1, data);
    }
    public void TurnStart()
    {
        controller.StartMovePhase();
    }

    public void TurnEnd()
    {
        //이동 행동
        controller.OperActivate();
        //공격 행동
        field.FieldAction();
    }
    public void SaveCurrentState(int team)
    {
        List<PartyEntity> list = new List<PartyEntity>();
        foreach (var i in field.tiles)
        {
            foreach (var j in i)
            {
                var entity = j.GetEntity();
                if (entity != null)
                {
                    if (entity.TeamNum != team) continue;
                    PartyEntity temp = new PartyEntity(j.fieldPos, entity.name);
                    list.Add(temp);
                }
            }
        }
        PartyData data = new PartyData();
        data.Entities = list.ToArray();
        data.SavePartyData();
    }
}
