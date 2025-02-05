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
    public bool isOccupied => entityBody != null;
    //0번은 중립 -1은 빈칸
    public int entityTeam
    {
        get
        {
            if (entityObj == null) return -1;
            var team = entityObj.GetComponent<ITeam>();
            if(team == null) return 0;
            return entityObj.GetComponent<ITeam>().TeamNum;
        }
    }
    
    public UnityAction<GameObject> cellAttack;
    public UnityAction<GameObject> cellEnhance;
    public UnityAction<GameObject> cellHeal;

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
    public void CellSetting()
    {
        var entity = GetEntity();
        if (entity == null) return;
        entity.SetAction();
    }
    //셀의 지역 효과 발동
    public void CellActivate(Entity.ActionType type)
    {
        if (isOccupied)
        {
            switch (type)
            {
                case Entity.ActionType.Attack:
                    cellAttack?.Invoke(entityObj);
                    cellAttack = null;
                    break;
                case Entity.ActionType.Heal:
                    cellHeal?.Invoke(entityObj);
                    cellHeal = null;
                    break;
                case Entity.ActionType.Enhance:
                    cellEnhance?.Invoke(entityObj);
                    cellEnhance = null;
                    break;
                default:
                    break;
            }
        }
    }
    //셀에 있는 오브젝트 사망 확인후 제거
    public void CheckCell()
    {
        if (isOccupied)
        {
            if (entityBody.isZero())
            {
                entityBody.Dead();
                entityObj = null;
            }
        }
    }
    //한턴 종료 후 원래 공격력으로 되돌리기
    public void SetOriginStat()
    {
        if (isOccupied)
        {
            var entity = GetEntity();
            if (entity == null) return;
            entity.SetOriginPower();
        }
    }

    private void OnMouseDown()
    {
        InputManager.Instance.TileInput(this);
    }
}
