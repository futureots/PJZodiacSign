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
    ACTION,
    ATTACK,
    REPAIR
}

[Serializable]
public class Turn
{
    public int agentID;
    public TurnType type;

    // NOTE: 
    public List<string> actions; 
}

[Flags]
public enum PhaseType
{
    None = 0,
    Battle = 1 << 0,
    Repair = 1 << 1
}

[CreateAssetMenu(fileName = "Phase", menuName = "Scriptable Objects/PhaseData")]
public class Phase : ScriptableObject
{
    public PhaseType phaseName;
    public bool isLoop;
    public List<Turn> turnList = new();
    
    public UIType useUIType;
}
