using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.PlayerSettings;
using static UnityEngine.EventSystems.EventTrigger;

public class Entity : MonoBehaviour
{
    //공격범위 반전 여부
    public bool isReflect;
    protected int negative => isReflect ? -1 : 1;
    
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
    public JodiacDataSO data;
    public Element elementType;
    [SerializeField] ElementSO material;
    public Jodiac jodiacType;
    public int level;

    public Health health;
    public Action<Entity> OnDestroyed;
    public int power;
    public int teamNum;
    public int TeamNum { get => teamNum; set => teamNum = value; }

    protected bool isAllocated;

    //public float properHeight => transform.lossyScale.y;



    public void SetEntity(int teamNum, int level, bool isReflect, Element type)
    {
        this.TeamNum = teamNum;
        this.isReflect = isReflect;
        this.level = level;
        this.elementType = type;
        GetComponent<Renderer>().SetMaterials(new List<Material>() { material.GetMaterial(type)});
        health.originHp = data.hp + data.hpIncrease * level;
        power = data.power + data.powerIncrease * level;
    }
    protected void Start()
    {
        health.Dead += Dead;
    }

    protected void OnMouseDown()
    {
        InputManager.Instance.OnEntityDown(this);
    }
    protected void OnMouseDrag()
    {
        InputManager.Instance.OnEntityDrag(this);
    }
    protected void OnMouseUp()
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


    public virtual List<Tile> GetAttackArea(intVector2 entityPos)
    {
        return new List<Tile>();
    }

    public List<Tile> GetMoveArea(intVector2 entityPos, bool hasOriginTile = true)
    {
        var area = GetAttackArea(entityPos);

        for (int i = 0; i < area.Count; i++)
        {
            if (area[i] == null) continue;
            if (area[i].isOccupied)
            {
                area.RemoveAt(i);
                i--;
            }
        }
        area.Add(curTile);

        return area;
    }

    public void Attack()
    {
        var targets = new List<IDamageable>();
        if (field == null) return;
        foreach (var tile in GetAttackArea(curPos))
        {
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
