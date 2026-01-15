using System;
using System.Collections.Generic;

[Serializable]
public class StageData
{
    /**
     * 스테이지 1개 진행에 필요한 정보
     */
    public StageData(List<AgentData> agents, ShopTable shop, int level = 1 , AgentData player = new())
    {
        this.agents = agents;
        this.shopTable = shop;
        this.level = level;
        this.player = player;
    }

    public int level;


    // Stage Config
    public string modelName = SceneName.FieldModel.Default;
    public string controllerName = SceneName.FieldController.Default;

    // Player and Agents
    public AgentData player = new AgentData();
    public List<AgentData> agents = new List<AgentData>();

    // Field System Data
    public ShopTable shopTable;

    // Special Rules
    public List<string> specialRule = new();
}
