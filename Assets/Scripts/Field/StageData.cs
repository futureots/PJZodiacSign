using System;
using System.Collections.Generic;

[Serializable]
public class StageData
{
    /**
     * 스테이지 1개 진행에 필요한 정보
     */
    
    // Stage Config
    public string modelName = SceneName.FieldModel.Default;
    public string controllerName = SceneName.FieldController.Default;

    // Player and Agents
    public AgentData player = new AgentData();
    public List<AgentData> agents = new List<AgentData>();

    // Field System Data
    public ShopTable shopTable;
    // TODO: 특수 타일, 기믹 오브젝트 등 타일 정보

    // Special Rules
    public List<string> specialRule = new();
}
