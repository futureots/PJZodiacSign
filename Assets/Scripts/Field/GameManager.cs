using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : SingletonObject<GameManager>
{
    [SerializeField] private DataManager dataManager;
    [SerializeField] private GameObject LoadingUI;      // NOTE: Loading 애니메이션 연결 시 스크립트로 변경
    private StageData currentStage = null;

    public LevelTable levelTable;
    public ShopTable shopTable;
    public int Level { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
        // TODO: 데이터 로드 로직 추가
        dataManager.LoadAllData("PlayerData");
        
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
        dataManager.SetData(stageData.player, Level);
        // 튜토리얼은 데이터를 저장하지 않음
        if (stageData.modelName == SceneName.FieldModel.Default)
        {
            dataManager.SaveAllData("PlayerData");
        }
        StartCoroutine(LoadBattleScene(stageData));
    }

    
    /// Model-Controller Scene async Load Routine
    private IEnumerator LoadBattleScene(StageData stageData)
    {
        string modelName = stageData.modelName;
        string controllerName = stageData.controllerName;
        
        // Set Loading UI
        LoadingUI.SetActive(true);
        
        // Load Scenes
        AsyncOperation modelOp = SceneManager.LoadSceneAsync(modelName, LoadSceneMode.Single);
        if (modelOp == null) { 
            Debug.LogError($"Failed to Load Model : {modelName}");
            yield break;
        }
        
        AsyncOperation controllerOp = SceneManager.LoadSceneAsync(controllerName, LoadSceneMode.Additive);
        if (controllerOp == null) { 
            Debug.LogError($"Failed to Load Controller : {controllerName}");
            yield break;
        }
        controllerOp.allowSceneActivation = false;
        
        // Wait for Scene Load
        yield return new WaitUntil(() => modelOp.progress >= 0.9f && controllerOp.progress >= 0.9f);
        
        // Start Loaded Scene
        controllerOp.allowSceneActivation = true;
        yield return new WaitUntil(() => modelOp.isDone && controllerOp.isDone);
        yield return null;                 
        
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
        SceneManager.LoadScene(0);
    }
    
    #endregion
}
