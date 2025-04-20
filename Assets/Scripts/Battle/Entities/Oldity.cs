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

public class Oldity : MonoBehaviour
{
    
    //공격범위 반전 여부
    bool isReflect;
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
    Element element;
    public Element elementType
    {
        get
        {
            return element;
        }
        set
        {
            element = value;
            GetComponent<Renderer>().SetMaterials(new List<Material>() { material.GetMaterial(value) });
        }
    }
    // 속성별 메테리얼
    [SerializeField] ElementSO material;

    public int level;

    public Health health;
    
    public Action<Oldity> OnDestroyed;
    
    public int power;
    int teamNum;
    public int TeamNum { get => teamNum; set => teamNum = value; }


    public void SetEntityData(int teamNum, int level, bool isReflect)
    {
        this.TeamNum = teamNum;
        this.isReflect = isReflect;
        this.level = level;
        health.originHp = data.hp + data.hpIncrease * level;
        power = data.power + data.powerIncrease * level;
    }
    void Start()
    {
        var temp = GetComponent<IDamageable>();
        health.OnDead += Dead;
    }

    void OnMouseDown()
    {
        //InputManager.Instance.OnGameObjectDown(this);
    }
    void OnMouseDrag()
    {
        //InputManager.Instance.OnEntityDrag(this);
    }
    void OnMouseUp()
    {
        //InputManager.Instance.OnGameObjectUp(this);
    }


    public void Dead()
    {
        OnDestroyed?.Invoke(this);
        
        Destroy(gameObject);
    }

    public void MoveToTile(Tile tile, bool isInArea = true)
    {
        //이동 가능여부 확인
        if (tile == null) return;
        if (isInArea)
        {
            var list = GetMoveArea();
            if (!list.Contains(tile)) return;
        }
        //이동 세팅
        StartCoroutine(MoveEntityCoroutine(tile));
    }
    IEnumerator MoveEntityCoroutine(Tile tile)
    {
        SetClickable(false);
        tile.OccupyObject(gameObject);
        curTile.OccupyObject();
        Vector3 entityPos = tile.transform.position;
        transform.DOMove(entityPos, 1f);
        field = tile.field;
        yield return new WaitForSeconds(1f);
        SetClickable(true);
    }

    public virtual List<Tile> GetAttackArea(intVector2 entityPos)
    {
        return new List<Tile>();
    }
    public virtual List<Tile> GetMoveArea(intVector2 entityPos, bool hasOriginTile = true)
    {
        var area = GetAttackArea(entityPos);

        for (int i = 0; i < area.Count; i++)
        {
            if (area[i] == null) continue;
            if (area[i].isEmpty)
            {
                area.RemoveAt(i);
                i--;
            }
        }
        if(hasOriginTile)area.Add(curTile);

        return area;
    }
    public List<Tile> GetMoveArea()
    {
        if(field == null) return new List<Tile>();
        return GetMoveArea(curPos);
    }

    public void Activate()
    {
        var targets = new List<IDamageable>();
        if (field == null) return;
        foreach (var tile in GetAttackArea(curPos))
        {
            if (tile == null) continue;
            if (tile.isEmpty)
            {
                if (tile.occupiedObject.tag == tag) continue; 
                var damageable = tile.occupiedObject.GetComponent<IDamageable>();
                if (damageable == null) continue;
                targets.Add(damageable);
            }
        }
        foreach (var target in targets)
        {
            target.Damaged(power);
        }
    }
    public void Attack(IDamageable enemyHp)
    {
        enemyHp.Damaged(power);
    }
    public void SetClickable(bool enable = true)
    {
        var collider = GetComponent<Collider>();
        collider.enabled = enable;
    }
}
