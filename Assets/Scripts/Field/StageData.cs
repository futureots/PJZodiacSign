using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "StageData")]
public class StageData :  ScriptableObject
{
    /**
     * 스테이지 1개 진행에 필요한 정보
     */
    
    // Stage Config
    public string modelName = SceneName.FieldModel.Default;
    public string controllerName = SceneName.FieldController.Default;

    // Player and Agents
    // public AgentData player = new AgentData();
    // public List<AgentData> agents = new List<AgentData>();
    // TODO: AgentData 스크립터블오브젝트 내 참조 오류 해결
    public string player = "DEFAULT";
    public List<string> agents = new();

    // Field System Data
    // public ShopTable shopTable;
    // TODO: 특수 타일, 기믹 오브젝트 등 타일 정보

    // Special Rules
    public List<string> specialRule = new();
}
