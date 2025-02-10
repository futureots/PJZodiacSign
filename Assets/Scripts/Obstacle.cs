using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Rendering;
using UnityEngine;

public class Obstacle : MonoBehaviour,IDamageable
{
    //디버그용 시작 위치
    public Field field;
    public intVector2 startPos;
    public Tile curTile;
    public intVector2 curPos
    {
        get
        {
            if (curTile == null) return new intVector2(-1, -1);
            return curTile.fieldPos;
        }
    }
    
    public int hp;
    public void Damaged(int damage, Element type = Element.Empty)
    {
        hp -= 1;
        Debug.Log(name + " Damaged ! : " + hp);
    }

    public void Dead()
    {
        Debug.Log(name + " Dead!");
        Destroy(gameObject);
    }

    public bool isZero()
    {
        return hp <= 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 originScale = transform.localScale;
        var cell = field.GetTile(startPos);
        cell.SetEntity(gameObject);
        transform.position = cell.transform.position;
        transform.localScale = originScale;
    }

    public void Healed(int amount)
    {
        hp += 1;
    }
}
