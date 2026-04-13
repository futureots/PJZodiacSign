
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalSceneManage;


public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject LoadingUI;      // NOTE: Loading 애니메이션 연결 시 스크립트로 변경
    private StageData currentStage = null;

    public LevelTable levelTable;
    public ShopTable shopTable;
    public int Level { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
        // TODO: 데이터 로드 로직 추가
        DataManager.Instance.LoadAllData("PlayerData");
        
    }
    
    /// <summary>
    /// 해당 모드 세팅(튜토리얼, 일반 모드 등)
    /// </summary>
    /// <param name="levelTable"></param>
    /// <param name="shopTable"></param>
    public void SetModeData(LevelTable levelTable, ShopTable shopTable)
    {
        this.levelTable = levelTable;
        this.shopTable = shopTable;
    }

    /// <summary>
    /// level테이블에서 해당 레벨의 데이터를 생성 후 반환
    /// </summary>
    /// <param name="level"></param>
    /// <param name="playerData"></param>
    /// <returns></returns>
    public StageData CreateStageData(int level,AgentData playerData)
    {
        List<AgentData> agents = new();
        var data = levelTable.GetLevelData(level);
        agents.Add(data.Item2);
        StageData stageData = new(data.Item1,agents, shopTable,data.Item3, level, playerData);
        return stageData;
    }
    #region BattleInit

    /// <summary>
    /// Enter Battle Scene
    /// </summary>
    /// <param name="stageData">Stage data to Load</param>
    public void EnterBattle(StageData stageData)
    {
        Level = stageData.level;
        DataManager.Instance.SetData(stageData.player, Level);
        DataManager.Instance.SaveAllData("PlayerData");
        StartCoroutine(StartBattle(stageData));
    }

    
    /// Model-Controller Scene async Load Routine
    private IEnumerator StartBattle(StageData stageData)
    {
        // Load scenes
        yield return SceneLoader.Instance.LoadBattle(stageData.modelName, stageData.controllerName);              
        
        // Find FieldController in Controller Scene
        FieldController fieldController = FindFirstObjectByType<FieldController>();
        if (!fieldController)
        {
            Debug.LogError($"Failed to Load Controller : {fieldController}");
            yield break;
        }
        
        // Init FieldController
        fieldController.Init(stageData);
        
        // Complete Loading
        currentStage = stageData;
        LoadingUI.SetActive(false);
    }

    #endregion

    #region BattleEnd

    public void ContinueGame()
    {
        var playerData = Agent.LocalPlayer.getData();
        // 다음 레벨로 넘어가는 코드
        EditorLogger.Print($"{playerData.credit} 현재 크레딧");
        EditorLogger.Print($"{Level + 1} 로드 중");
        var stageData = CreateStageData(Level + 1, playerData);
        EnterBattle(stageData);
    }

    public void EndGame(bool isDelete = false)
    {
        if(isDelete) DataManager.Instance.ResetData("PlayerData");
        StartCoroutine(SceneLoader.Instance.LoadMain());
    }
    
    #endregion
}
