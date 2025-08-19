using NUnit.Framework;
using System;
using UnityEngine;


public abstract class Agent : MonoBehaviour
{
    public bool isInputStop;
    public Team team { get; protected set; }
    protected void Awake()
    {
        team = GetComponent<Team>();
        controller = GetComponent<EntityController>();
        inventory = GetComponent<Inventory>();
    }
    #region Phase
    /// <summary>
    /// 정비 페이즈 시작
    /// </summary>
    /// <param name="level">현재 레벨</param>
    public abstract void SetRepairPhase(int level, Action call);
    /// <summary>
    /// 정비 페이즈 종료
    /// </summary>
    public virtual void EndRepairPhase()
    {
        controller.DisposeInstantField();
    }

    /// <summary>
    /// 전투 페이즈 시작
    /// </summary>
    public virtual void SetBattlePhase() { }
    /// <summary>
    /// 전투 페이즈 종료
    /// </summary>
    public virtual void EndBattlePhase() { }

    /// <summary>
    /// 행동 턴 시작
    /// </summary>
    /// <param name="call"></param>
    public virtual void SetActionTurn(Action call) { }


    #endregion
    /// <summary>
    /// 현재 controller가 보유중인 커맨드 반환 및 초기화
    /// </summary>
    /// <returns>입력한 커맨드</returns>
    public Command GetCommand()
    {
        var cmd = controller.curCmd;
        return cmd;
    }

    // 컨트롤러
    public EntityController controller { get; protected set; } 
    public Inventory inventory { get; protected set; }

    #region AgentData
    // 데이터 컨테이너(인벤토리는 사용 X)
    protected AgentData data;
    public AgentData GetAgentData()
    {
        // 인벤토리 데이터는 저장 직전 불러오기
        data.items = inventory.GetInventoryData();
        return data;
    }

    /// <summary>
    /// 데이터 기반으로 에이전트 세팅하기(게임 시작, 재개 시 1회만 실행)
    /// </summary>
    /// <param name="agentData"></param>
    public void SetData(AgentData agentData)
    {
        data = agentData;
        inventory.SetItem(agentData.items);
    }
    
    // 데이터 갱신(기물 갱신, 게임 클리어 시 해당 데이터 저장. 아이템은 게임 클리어 시 갱신 및 저장)

    /// <summary>
    /// 아이템을 구매할 수 있으면 구매하고 true반환, 없으면 false 반환
    /// </summary>
    /// <param name="itemData">구매하려는 아이템 데이터</param>
    /// <returns></returns>
    public bool BuyItem(ItemData itemData, int cost)
    {
        if (cost > data.credit) return false;
        data.credit -= cost;
        Debug.Log("Buy Item");
        inventory.AddItem(itemData);
        return true;
    }
    public bool BuyEntity(EntityData entityData, int cost)
    {
        if (data.credit < cost) return false;
        data.credit -= cost;

        var entity = entityData.CreateEntity();

        Debug.Log("Buy Entity");
        controller.PlaceOnInstantField(entity);

        return true;
    }
    #endregion

}
public enum Mode
{
    Repair,//정비 입력(상점창 오픈, 정비용 카메라 무브, 보유 기물 인스턴트 필드)
    Move,//이동 입력(드래그&드롭)
    Skill,//스킬 입력(클릭)
    None // 입력 X, 정보만 표시
}
