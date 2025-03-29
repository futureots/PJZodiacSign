using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Jodiac", menuName = "Jodiac")]
public class JodiacSO : ScriptableObject
{
    public List<GameObject> jodiacList;

    public GameObject GetJodiac(Jodiac jodiac)
    {
        var num = (int)jodiac;
        if (jodiacList.Count> num)
        {
            return jodiacList[num];
        }
        else
        {
            return null;
        }
    }
}
public enum Jodiac
{
    None = 0,
    Mouse = 1,
    Cow = 2,
    Tiger = 3,
    Rabbit = 4,
    Dragon = 5,
    Snake = 6,
    Horse = 7,
    Sheep = 8,
    Monkey = 9,
    Chicken = 10,
    Dog = 11,
    Pig = 12
}