using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JodiacData",menuName = "JodiacData")]
public class JodiacDataSO : ScriptableObject
{
    public int hp;
    public int hpIncrease;
    public int power;
    public int powerIncrease;
    public Skill skill;
    
}
