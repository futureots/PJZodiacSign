using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelTable", menuName = "Scriptable Objects/LevelTable")]
public class LevelTable : ScriptableObject
{
    public int endLevel;
    public int basicCredit;
    [SerializeField] private List<Phase> basePhases;
    [SerializeField] private EnemyAI baseAI;
    public List<LevelData> data;

    private Dictionary<int, EnemyTable> _levels;


    private Dictionary<int,EnemyTable> Levels
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
    public (EnemyAI,AgentData,List<Phase>, int) GetLevelData(int level)
    {
        if(Levels.TryGetValue(level,out EnemyTable table))
        {
            return (table.enemyAI,table.GetEnemyData(),table.phases, table.turnLimit);
        }
        else
        {
            return (baseAI,new AgentData(level * basicCredit), basePhases, 50);
        }
        
    }

}
[System.Serializable]
public struct LevelData
{
    public int level;
    public EnemyTable enemies;
}
