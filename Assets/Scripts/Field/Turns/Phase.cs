using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum PlayerTurn
{
    P0 = -1,
    P1 = 0,
    P2,
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
    public string phaseName;
    public bool isLoop;
    public List<Turn> turnList = new();
    
    public UIType useUIType;
}
