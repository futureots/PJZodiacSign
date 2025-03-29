using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{
    public new Renderer renderer
    {
        get
        {
            return GetComponent<Renderer>();
        }
    }
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
    public Entity OccupiedEntity
    {
        get
        {
            if (!isOccupied) return null;
            return entityObj.GetComponent<Entity>();
        }
    }
    public bool isOccupied => entityObj != null;
    public void SetEntity(GameObject e)
    {
        entityObj = e.gameObject;
        e.transform.SetParent(transform);
    }

}
