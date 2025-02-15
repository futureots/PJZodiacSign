using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour, IAttackable, ITeam
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
    //전투와 관련된 변수들
    public enum ActionType
    {
        Attack,
        Heal,
        Enhance
    }
    //이거 나중에 리스트로 만들면 한번에 복수 행동 가능할듯(타수증가나 동시에 힐이랑 강화)
    public ActionType actionToEnemy;
    public ActionType actionToAlly;
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
    //엔티티 공격범위
    public List<intVector2> GetEntityArea()
    {
        if (field == null) return new List<intVector2>();
        List<intVector2> absArea = new List<intVector2>();
        foreach (intVector2 pos in area)
        {
            var absPos = curPos + pos * (isReflect ? -1 : 1);
            if (field.IsValidCellPos(absPos))
            {
                absArea.Add(absPos);
            }
        }
        return absArea;
    }
    public bool IsInArea(intVector2 pos)
    {
        var absArea = GetEntityArea();
        foreach (intVector2 pos2 in absArea)
        {
            if (pos == pos2) return true;
        }
        return false;
    }
    public void Attack(GameObject target = null)
    {
        if (target == null) return;
        var body = target.GetComponent<IDamageable>();
        if (body == null) return;
        body.Damaged(power, elementType);
    }
    public void Enhance(GameObject target = null)
    {
        if (target == null) return;
        var body = target.GetComponent<Entity>();
        if (body == null) return;
        body.power += originPower;
        Debug.Log(target.name + " Enhanced : " + body.power);
    }
    public void Heal(GameObject target = null)
    {
        if(target == null) return;
        var body = target.GetComponent<IDamageable>();
        if (body == null) return;
        body.Healed(power);
    }
}
