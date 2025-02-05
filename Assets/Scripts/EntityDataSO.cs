using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityData",menuName = "EntityData")]
public class EntityDataSO : ScriptableObject
{
    public int hp;
    public int power;
    [SerializeField] public List<intVector2> area;
    
}
