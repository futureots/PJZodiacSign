
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
    public int agentID;
    public string type;

    // NOTE: 
    public List<string> actions; 
}


[CreateAssetMenu(fileName = "Phase", menuName = "Scriptable Objects/PhaseData")]
public class Phase : ScriptableObject
{
    public string name;
    public bool isLoop;
    public List<Turn> turnList = new();
}
