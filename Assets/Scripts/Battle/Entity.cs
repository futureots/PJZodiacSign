using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.PlayerSettings;

public class Entity : MonoBehaviour
{
    //필드와 연관되어있는 변수들
    public bool isReflect;
    bool isAllocated;
    public Field field;
    public Tile curTile { get; private set; }
    public intVector2 curPos
    {
        get
        {
            if (curTile == null) return new intVector2(-1, -1);
            return curTile.fieldPos;
        }
    }

    //엔티티 라이프 사이클에 필요한 변수들
    public EntityDataSO data;
    public Element elementType;
    public Jodiac jodiacType;

    public Health health;
    public Action<Entity> OnDestroyed;
    public int originPower
    {
        get
        {
            return data.power;
        }
    }
    public int power;
    public int teamNum;
    public int TeamNum { get => teamNum; set => teamNum = value; }
    
    
    //public float properHeight => transform.lossyScale.y;
    public intVector2[] area
    {
        get
        {
            return data.area.ToArray();
        }
    }
    public void SetOriginPower()
    {
        power = originPower;
    }
    private void Start()
    {
        //hp = maxHp;
        health.Dead += Dead;
        SetOriginPower();
    }

    private void OnMouseDown()
    {
        InputManager.Instance.OnEntityDown(this);
    }
    private void OnMouseDrag()
    {
        InputManager.Instance.OnEntityDrag(this);
    }
    private void OnMouseUp()
    {
        InputManager.Instance.OnEntityUp(this);
    }


    public void Dead()
    {
        OnDestroyed?.Invoke(this);
        Destroy(gameObject);
    }

    public void MoveToTile(Tile tile)
    {
        var scale = transform.localScale;
        //원래 있던 위치 연결 제거
        if (field != null)
        {
            if (field.IsValidCellPos(curPos))
            {
                var preCell = field.GetTile(curPos);
                preCell.entityObj = null;
            }
        }
        if (tile.isOccupied)
        {
            return;
        }
        tile.SetEntity(gameObject);
        curTile = tile;
        Vector3 entityPos = tile.transform.position;
        entityPos.y = 0.25f;
        transform.localScale = scale;
        transform.DOMove(entityPos, 1f);
        field = tile.field;
    }
    /// <summary>
    /// 타일 위치를 기준으로 엔티티의 공격(이동)범위 리스트로 반환
    /// </summary>
    /// <param name="entityPos"></param>
    /// <param name="hasOriginTile">엔티티 기존 위치 반환 여부</param>
    /// <returns></returns>
    public List<intVector2> GetArea(intVector2 entityPos, bool hasOriginTile = false)
    {
        if (field == null) return new List<intVector2>();
        List<intVector2> absArea = new List<intVector2>();
        foreach (intVector2 pos in area)
        {
            var absPos = entityPos + pos * (isReflect ? -1 : 1);
            if (field.IsValidCellPos(absPos))
            {
                absArea.Add(absPos);
            }
        }
        if (hasOriginTile) absArea.Add(entityPos);
        return absArea;
    }
    public List<intVector2> GetArea(bool hasOriginTile = false)
    {
        return GetArea(curPos, hasOriginTile);
    }
    public void Attack()
    {
        var targets = new List<IDamageable>();
        foreach (var pos in GetArea())
        {
            var tile = field.GetTile(pos);
            if (tile == null) continue;
            if (tile.isOccupied)
            {
                if (tile.entityObj.tag == tag) continue; 
                var damageable = tile.entityObj.GetComponent<IDamageable>();
                if (damageable == null) continue;
                targets.Add(damageable);
            }
        }
        foreach (var target in targets)
        {
            target.Damaged(power, elementType);
        }
    }
}
