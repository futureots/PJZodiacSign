using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TeamColorTable", menuName = "Scriptable Objects/TeamColorTable")]
public class TeamColorTable : ScriptableObject
{
    public List<Color> teamColors;
}
