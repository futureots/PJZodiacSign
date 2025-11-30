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

    public void SetLoadingUI(bool set)
    {
        // NOTE: 로딩씬 Script 추가 시 해당 로직 추가로 변경
        GameObject LoadingUI = transform.GetChild(0).gameObject;
        
        LoadingUI.SetActive(set);
    }
    
    public void LoadField()
    {
        // TODO: 필드에 필요한 정보 전달
        StageManager.Instance.Load();
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
    public void Init(StageData data)
    {
        /* 레벨 데이터로 씬 로드 준비
         * - 에이전트 목록 확인 및 생성
         * - 전투 정보 정리 (상점아이템, 특수조건, 기믹 등)
         * - 필드 전달용 정보 정리
         *  - 에이전트별 기물 정보
         *  - 상점아이템 목록
         *  - 특수타일 정보
         * - 필드 로드
         */
        commandSystem = new();
        
        
        // TODO: 필드 주입받기
    }
}
