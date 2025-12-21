
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum PlayerTurn
{
    P1 = -1,
    P2 = 0,
    P3,
    P4,
    P5,
    P6,
    P7,
    P8
}

[Serializable]
public class Turn
{
    public PlayerTurn player;

    public string type;
}


[CreateAssetMenu(fileName = "Phase", menuName = "Scriptable Objects/PhaseData")]
public class Phase : ScriptableObject
{
    public bool loop;

    public List<Turn> turnList = new();
}
