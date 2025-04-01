using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    public int originHp;
    public int maxHp {  get; private set; }
    public int hp {  get; private set; }
    public Action<int> healthChanged;
    public Action Dead;
    private void Start()
    {
        maxHp = originHp;
        hp = originHp;
        healthChanged?.Invoke(hp);
    }
    public void Damaged(int damage)
    {
        hp -= damage;
        healthChanged?.Invoke(-damage);
    }

    public void Healed(int amount)
    {
        hp += amount;
        healthChanged?.Invoke(amount);
    }

    public bool isZero()
    {
        return hp <= 0;   
    }
    public void Deade()
    {
        Destroy(gameObject);
    }
}
