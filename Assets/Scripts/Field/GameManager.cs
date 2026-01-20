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
    
    public override void Awake()
    {
        base.Awake();
        // TODO: 데이터 로드 로직 추가
        dataManager.LoadAllData("PlayerData");
        
    }
    
    public void SetModeData(LevelTable levelTable, ShopTable shopTable)
    {
        this.levelTable = levelTable;
        this.shopTable = shopTable;
    }
    public List<AgentData> GetEnemyAgentData(int level)
    {
        List<AgentData> agents = new List<AgentData>();
        
        AgentData enemy = levelTable.GetLevelData(level);
        agents.Add(enemy);
        return agents;
    }
    public StageData CreateStageData(int level,AgentData playerData)
    {
        var data = GetEnemyAgentData(level);
        StageData stageData = new StageData(data, shopTable, level, playerData);
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
        dataManager.SaveAllData("PlayerData");
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

    public event Action<bool> OnGameEnded;
    /// <summary>
    /// Procedure when Battle End
    /// </summary>
    public void ExitBattle (PlayerID winPlayer)
    {
        
        // TODO: 게임 종료 처리 로직 추가
        if (winPlayer == PlayerID.P0)
        {
            //OnGameEnded?.Invoke(true);
            ContinueGame();
        }
        else
        {
            //OnGameEnded?.Invoke(false);
            EndGame(true);
        }
    }

    public void ContinueGame()
    {
        var playerData = Agent.LocalPlayer.getData();
        // 다음 레벨로 넘어가는 코드
        playerData.credit += 100;
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
