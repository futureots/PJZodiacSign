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


public enum TurnType
{
    REPAIR = 1 << 0,
    ACTION = 1 << 1,
    BOTH = REPAIR | ACTION,
    ATTACK = 1 << 2
}


[Serializable]
public class Turn
{
    public int agentID;
    public TurnType type;

    // NOTE: 
    public List<string> actions; 
}


public enum PhaseType
{
    Battle,
    Repair
}

[CreateAssetMenu(fileName = "Phase", menuName = "Scriptable Objects/PhaseData")]
public class Phase : ScriptableObject
{
    public PhaseType phaseName;
    public bool isLoop;
    public List<Turn> turnList = new();
    
    public UIType useUIType;
}
