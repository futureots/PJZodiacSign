using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    /** FieldController
     * 필드 로드
     * 레벨 기믹 수행
     * Agent 생성 및 필드 시스템과 Command 통신
     */
    
    #region FieldLoad
    private Field field;   // Inject
    private Shop shop;  // Inject
    
    public void LoadField()
    {
        
    }
    
    #endregion
    
    #region TurnManage
    public uint turnCount;
    public ITurn curState;
    
    
    public void SetState(ITurn state)
    {

        curState = state;
    }
    
    #endregion
    
    #region Commands
    public List<Command> commandList;
    private CommandSystem commandSystem;    // Attach
    
    public void SendCommands()
    {
        //field에 커맨드 전송 및 콜백 함수 설정
    }
    void OnCommandExecuted()
    {

    }
    
    #endregion
    
    #region Agents
    public List<Agent> agents;
    #endregion

    /// <summary>
    /// Initiate Controller
    /// </summary>
    /// <remarks>필드, 상점 주입받고 전투 Model을 초기화</remarks>
    public void Init(Field field, Shop shop = null)
    {
        this.field = field;
        this.shop = shop;

        commandSystem = new();
    }
}
