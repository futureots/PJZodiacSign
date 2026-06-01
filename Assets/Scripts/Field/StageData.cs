using System;
using System.Collections.Generic;

[Serializable]
public class StageData
{
    /**
     * 스테이지 1개 진행에 필요한 정보
     */
    public StageData(EnemyAI aiPrefab,List<AgentData> agents, ShopTable shop,List<Phase> phases, int level , AgentData player, int time, int point, int lastLevel, int  turnLimit)
    {
        this.aiPrefab = aiPrefab;
        this.agents = agents;
        this.shopTable = shop;
        this.level = level;
        this.player = player;
        this.phases = phases;
        
        
        this.time = time;
        this.point = point;
        isLastLevel = level == lastLevel;
        this.turnLimit = turnLimit;
    }

    public int time;
    public int level;
    public int point;
    public bool isLastLevel;
    public int turnLimit;
    
    // Stage Config
    public string modelName = GlobalManage.ModelID.Default;
    public string controllerName = GlobalManage.ControllerID.Default;

    // Player and Agents
    public EnemyAI aiPrefab = null;
    public AgentData player = new AgentData();
    public List<AgentData> agents = new();

    // Field System Data
    public ShopTable shopTable;

    // Special Rules
    public List<string> specialRule = new();
    public List<Phase> phases = new();
}
