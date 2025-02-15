using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{
    //이 타일이 있는 필드 리스트
    public Field field { get; private set; }
    public intVector2 fieldPos;
    public void SetField(Field f,int column, int row)
    {
        field = f;
        fieldPos.x = column;
        fieldPos.y = row;
    }
    //이 칸에 있는 엔티티(적 또는 아군 또는 장애물)
    public GameObject entityObj;
    public IDamageable entityBody
    {
        get
        {
            if (entityObj == null) return null;
            return entityObj.GetComponent<IDamageable>();
        }
    }
    public bool isOccupied => entityObj != null;
    //0번은 중립 -1은 빈칸
    public int entityTeam
    {
        get
        {
            if (!isOccupied) return -1;
            var team = entityObj.GetComponent<ITeam>();
            if(team == null) return 0;
            return entityObj.GetComponent<ITeam>().TeamNum;
        }
    }
    public Entity GetEntity()
    {
        if (!isOccupied) return null;
        Entity entity = entityObj.GetComponent<Entity>();
        return entity;
    }
    public void SetEntity(GameObject e)
    {
        entityObj = e.gameObject;
        e.transform.SetParent(transform);
    }
    private void OnMouseUp()
    {
        Debug.Log("Mouse Up");
    }

}
