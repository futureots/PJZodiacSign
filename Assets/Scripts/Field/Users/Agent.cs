using System;
using UnityEngine;


public abstract class Agent : MonoBehaviour
{
    public Team team { get; protected set; }
    
    public Inventory inventory { get; protected set; }

    int credit;
    public int Credit
    {
        get { return credit; }
        set
        {
            credit = value;
            onCreditChanged?.Invoke(credit);
        }
    }
    public Action<int> onCreditChanged;

    // 행동 포인트
    public int actionCount;
    // 현재 설정 중인 커맨드 인덱스
    int cmdIndex;
    // 설정한 커맨드 리스트
    public Command[] commands;

    protected void Awake()
    {
        team = GetComponent<Team>();
        inventory = GetComponent<Inventory>();

        commands = new Command[actionCount];
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
    /// 현재 controller가 보유중인 커맨드 반환
    /// </summary>
    /// <returns>입력한 커맨드</returns>
    public Command GetCommand()
    {
        throw new NotImplementedException();
    }


    #region AgentData
    // 데이터 컨테이너(인벤토리는 사용 X)
    protected AgentData data;
    public AgentData UpdateAgentData()
    {
        // 인벤토리 데이터는 저장 직전 불러오기
        data.items = inventory.GetInventoryData();
        data.credit = Credit;
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
        Credit = agentData.credit;
    }
    
    
    /// <summary>
    /// 기물 소환
    /// </summary>
    /// <param name="entityData">기물 데이터</param>
    /// <returns></returns>
    public bool SummonEntity(EntityData entityData)
    {
        if (!CanPlaceOnResourceField()) return false;

        var entity = EntityFactory.CreateEntity(entityData);

        return true;
    }
    bool CanPlaceOnResourceField()
    {
        // var list = Field.GetEmptyTiles(controller.resourceField.GetTiles());
        // if (list.Count <= 0) return false;
        // return true;
        throw new NotImplementedException();
    }
    #endregion

}
