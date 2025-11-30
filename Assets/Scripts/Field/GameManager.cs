using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : SingletonObject<GameManager>
{
    private DataManager dataManager;
    [SerializeField] private GameObject LoadingUI;      // NOTE: Loading 애니메이션 연결 시 스크립트로 변경
    
    public override void Awake()
    {
        base.Awake();
        // dataManager = this.GetOrAddComponent<DataManager>();     // TODO: 데이터 로드 로직 추가
        // dataManager.LoadAllData();                   
    }
    
    #region BattleInit

    /// <summary>
    /// 전투 스테이지 진입
    /// </summary>
    /// <param name="stageData">Stage data to Load</param>
    public void EnterBattle(StageData stageData)
    {
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
        LoadingUI.SetActive(false);
    }

    #endregion

    #region BattleEnd
    
    /// <summary>
    /// 게임 종료 시 처리
    /// </summary>
    void ExitBattle ()
    {
        Debug.Log("게임 종료");
        // TODO: 게임 종료 처리 로직 추가
        EnterBattle(ScriptableObject.CreateInstance<StageData>());       // 기본 씬 재로드
    }
    
    #endregion
}
