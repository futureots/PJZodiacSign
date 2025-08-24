using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelTable", menuName = "Scriptable Objects/LevelTable")]
public class LevelTable : ScriptableObject
{
    public int basicCredit;
    public List<LevelData> data;

    Dictionary<int, EnemyTable> levels;

    public Dictionary<int,EnemyTable> Levels
    {
        get
        {
            if(levels == null)
            {
                levels = new Dictionary<int, EnemyTable>();
                foreach(var level in data)
                {
                    levels.Add(level.level, level.enemies);
                }
            }
            return levels;
        }
    }

    public AgentData GetLevelData(int level)
    {
        if(Levels.TryGetValue(level,out var table))
        {
            return table.GetAgentData();
        }
        else
        {
            for(int i=level; i > 0; i--)
            {
                if (Levels.TryGetValue(i, out var t))
                {
                    var agentData = t.GetAgentData();
                    agentData.credit += basicCredit * (level - i);
                    return agentData;
                }
            }
        }
        return GetBasicData(level);
    }
    public AgentData GetBasicData(int level)
    {
        return new AgentData(level * basicCredit);
    }

}
[System.Serializable]
public struct LevelData
{
    public int level;
    public EnemyTable enemies;
}
