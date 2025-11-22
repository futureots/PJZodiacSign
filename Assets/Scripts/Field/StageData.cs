using System.Collections.Generic;

[System.Serializable]
public class StageData
{
    // 스테이지 1개 진행에 필요한 정보

    public AgentData[] agents = new AgentData[2];

    public ShopTable shopTable;

    public List<string> SpecialRule = new();
}
