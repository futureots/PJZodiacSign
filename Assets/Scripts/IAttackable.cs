using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    public void Attack(GameObject target = null);
    public void Enhance(GameObject target = null);
    public void Heal(GameObject target = null);

}
