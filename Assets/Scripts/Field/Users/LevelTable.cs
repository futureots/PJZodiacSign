using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelTable", menuName = "Scriptable Objects/LevelTable")]
public class LevelTable : ScriptableObject
{
    public int basicCredit;
    [SerializeField] private List<Phase> basePhases;
    public List<LevelData> data;

    private Dictionary<int, EnemyTable> _levels;

    
    public Dictionary<int,EnemyTable> Levels
    {
        get
        {
            if(_levels == null)
            {
                _levels = new Dictionary<int, EnemyTable>();
                foreach(var level in data)
                {
                    _levels.Add(level.level, level.enemies);
                }
            }
            return _levels;
        }
    }
    
    /// <summary>
    /// 해당 레벨의 레벨 테이블 반환(적 종류, 턴 구조 보유) 
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public (AgentData,List<Phase>) GetLevelData(int level)
    {
        if(Levels.TryGetValue(level,out var table))
        {
            return (table.GetAgentData(),table.phases);
        }
        else
        {
            return (new AgentData(level * basicCredit), basePhases);
        }
        
    }

}
[System.Serializable]
public struct LevelData
{
    public int level;
    public EnemyTable enemies;
}
