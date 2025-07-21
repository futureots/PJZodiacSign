using System;
using UnityEngine;


public abstract class Agent : MonoBehaviour
{
    public bool isInputStop;
    public Team team { get; protected set; }
    public abstract void SetMode(Mode mode, Action call = null);
    protected void Awake()
    {
        team = GetComponent<Team>();
        controller = GetComponent<EntityController>();
        inventory = GetComponent<Inventory>();
    }
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
    AgentData _agentData;
    public AgentData agentData
    {
        get {  return _agentData; }
    }
    public void SetData(AgentData dataSet)
    {
        this._agentData = dataSet;
        Debug.Log(_agentData);
        SetData();
    }
    // 초기 데이터 세팅
    public void SetData()
    {
        controller.SetInstantField(_agentData.handEntities);
        controller.SetMainField(_agentData.fieldEntities);
        inventory.SetItem(_agentData.items);
    }

    // 데이터 갱신(기물 갱신, 게임 클리어 시 해당 데이터 저장. 아이템은 게임 클리어 시 갱신 및 저장)
    public void UpdateEntities()
    {
        controller.DisposeInstantField(ref _agentData.fieldEntities, ref _agentData.handEntities);
    }
    /// <summary>
    /// 아이템을 구매할 수 있으면 구매하고 true반환, 없으면 false 반환
    /// </summary>
    /// <param name="itemData">구매하려는 아이템 데이터</param>
    /// <returns></returns>
    public bool BuyItem(ItemData itemData)
    {
        if (itemData.cost > _agentData.credit) return false;
        _agentData.credit -= itemData.cost;
        _agentData.items.Add(itemData);
        inventory.AddItem(itemData);
        return true;
    }
    public bool BuyEntity(EntityUIData entityData)
    {
        if (_agentData.credit < entityData.normalPrice) return false;
        _agentData.credit -= entityData.normalPrice;
        
        var entity = ResourceManager.CreateEntity(entityData.id);
        
        controller.PlaceEntity(entity, controller.instantField);
        return true;
    }
    #endregion

}
public enum Mode
{
    Repair,//정비 입력(상점창 오픈, 정비용 카메라 무브, 보유 기물 인스턴트 필드)
    Move,//이동 입력(드래그&드롭)
    Active,//스킬 입력(클릭)
    None // 입력 X, 정보만 표시
}
