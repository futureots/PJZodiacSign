using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour, IDamageable, IAttackable, ITeam
{
    //필드와 연관되어있는 변수들
    public bool isReflect;
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
    public ColorType type;
    public int maxHp
    {
        get
        {
            if (data == null) return 0;
            return data.hp;
        }
    }
    public int hp;
    public int originPower
    {
        get
        {
            return data.power;
        }
    }
    public int power;
    public Action<Entity> OnDestroyed;
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
        hp = maxHp;
        SetOriginPower();
        //MoveToTile(field.GetTile(new intVector2(0, 0)));
    }

    private void OnMouseDown()
    {
        InputManager.Instance.EntityInput(this);
    }

    public bool isZero()
    {
        if (hp <= 0) return true;
        else return false;
    }

    public void Dead()
    {
        OnDestroyed?.Invoke(this);
        Destroy(gameObject);
    }
    public virtual void SetAction()
    {
        var tiles = GetEntityArea();
        foreach(var tilePos in tiles)
        {
            Tile tile = field.GetTile(tilePos);
            //빈곳일 때
            if (tile.entityTeam == -1) 
            {
                Debug.Log(tilePos.x + ", " + tilePos.y + ": " + "is Empty");
            }
            //엔티티가 중립 또는 적일 때
            else if (tile.entityTeam == 0 || tile.entityTeam !=teamNum)
            {
                Debug.Log(tilePos.x + ", " + tilePos.y + ": " + "is Enemy :" + teamNum + " TN: " + tile.entityTeam);
                SetEntityAction(tile, actionToEnemy);
            }
            //아군일 때
            else if(tile.entityTeam == teamNum)
            {
                Debug.Log(tilePos.x + ", " + tilePos.y + ": " + "is Ally :" + teamNum + " TN : " + tile.entityTeam);
                SetEntityAction(tile, actionToAlly);
            }
        }
    }
    //셀에 액션 집어넣기
    public void SetEntityAction(Tile tile, ActionType type)
    {
        switch (type)
        {
            case ActionType.Attack:
                tile.cellAttack += Attack;
                break;
            case ActionType.Heal:
                tile.cellHeal += Heal;
                break;
            case ActionType.Enhance:
                tile.cellEnhance += Enhance;
                break;
            default:
                break;
        }
    }
    public void MoveToTile(Tile tile)
    {
        var scale = transform.localScale;
        if (tile.isOccupied)
        {
            return;
        }
        //원래 있던 위치 연결 제거
        if (field.IsValidCellPos(curPos))
        {
            var preCell = field.GetTile(curPos);
            preCell.entityObj = null;
        }
        tile.SetEntity(gameObject);
        curTile = tile;
        Vector3 entityPos = tile.transform.position;
        entityPos.y = 0.25f;
        transform.localScale = scale;
        transform.DOMove(entityPos, 1f);
    }
    //엔티티 공격범위
    public List<intVector2> GetEntityArea()
    {
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
        body.Damaged(power, type);
    }
    public void Damaged(int damage, ColorType type = ColorType.Empty)
    {
        hp -= damage;
        Debug.Log(name + " Damaged ! : " + hp);
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
        var body = target.GetComponent<Entity>();
        if (body == null) return;
        body.Healed(power);
        Debug.Log(target.name + " Healed : " + body.hp);
    }

    public void Healed(int amount)
    {
        hp = hp + amount < maxHp ? hp + amount : maxHp;
    }
}
