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
    AgentData data;
    public void SetData(AgentData dataSet)
    {
        this.data = dataSet;
        Debug.Log(data);
        SetData();
    }
    // 초기 데이터 세팅
    public void SetData()
    {
        controller.SetInstantField(data.handEntities);
        controller.SetMainField(data.fieldEntities);
        inventory.SetItem(data.items);
    }

    // 데이터 갱신(reparturn끝나면 갱신, 게임 클리어 시 해당 데이터 저장)
    public virtual void UpdateData()
    {
        controller.DisposeInstantField(ref data.fieldEntities, ref data.handEntities);
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
