using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GlobalManage
{
    public enum GameMode
    {
        Basic,
        Augment,
    }
    
    public class GameManager : Singleton<GameManager>
    {
        [Header("Base Data")]
        [SerializeField] private LoadingUI loadingUI;      // NOTE: Loading 애니메이션 연결 시 스크립트로 변경

        public LevelTable levelTable;
        public ShopTable shopTable;

        [Header("Stage status")]
        [SerializeField] private StageData currentStage = null;

        public int Level { get; private set; }
        
        public GameMode GameMode { get; private set; }

        public List<int> AugmentLevel = new();
        

        #region Initiate

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
    public StageData CreateStageData(int level,AgentData playerData, int time, int point)
    {
        List<AgentData> agents = new();
        var data = levelTable.GetLevelData(level);
        agents.Add(data.Item2);
        StageData stageData = new(data.Item1,agents, shopTable,data.Item3, level, playerData,time,point, levelTable.endLevel,data.Item4);
        return stageData;
    }
    
    #endregion
    
    #region BattleInit

        /// <summary>
        /// Enter Battle Scene
        /// </summary>
        /// <param name="stageData">Stage data to Load</param>
        public void EnterBattle(StageData stageData)
        {
            Level = stageData.level;
            
            // 튜토리얼은 데이터를 저장하지 않음
            if (stageData.controllerName != ControllerID.Tutorial)
            {
                DataManager.Instance.SetData(stageData.player, Level);
                //DataManager.Instance.SaveAllData("PlayerData");
                DataManager.Instance.SaveSteamCloudData("PlayerData");
            }
            
            // 게임모드 설정
            GameMode = stageData.controllerName == ControllerID.Default ? GameMode.Basic : GameMode.Augment;
            
            StartCoroutine(StartBattle(stageData));
        }


        /// <summary>
        /// Load Scene and Init Controller
        /// </summary>
        /// <param name="stageData">Battle Stage Data</param>
        private IEnumerator StartBattle(StageData stageData)
        {
            // Load scenes
            loadingUI.gameObject.SetActive(true);
            yield return loadingUI.FadeIn(0.2f);
            
            yield return SceneLoader.Instance.LoadBattle(stageData.modelName, stageData.controllerName);

            // Find FieldController in Controller Scene
            FieldController controller = FindFirstObjectByType<FieldController>();
            if (!controller)
            {
                EditorLogger.PrintError($"Failed to Load Controller : {controller}");
                yield break;
            }

            // Init FieldController
            try
            {
                controller.Init(stageData);
            }
            catch (Exception e)
            {
                EditorLogger.PrintError(e);
                // Load Failed, Return to Main
                StartCoroutine(SceneLoader.Instance.LoadMain());
            }

            // Connect Battle End Event
            controller.OnBattleEnd += (string s, StageData lastData) => 
            {
                switch (s)
                {
                    case "CLEAR":
                        EditorLogger.Print($"승리 {stageData.level}");
                        ContinueGame(lastData);
                        break;
                    case "FAIL":
                        EditorLogger.Print($"패배 {stageData.level}");
                        EndGame(true);
                        break;
                    case "End":
                        EditorLogger.Print($"종료 {stageData.level}");
                        EndGame();
                        break;
                }
            };

            // Complete Loading
            currentStage = stageData;
            
            yield return loadingUI.FadeOut(1f);
            loadingUI.gameObject.SetActive(false);
        }

        #endregion

        #region BattleEnd

        public void ContinueGame(StageData lastStage)
        {
            var playerData = Agent.LocalPlayer.getData();
            
            // 다음 레벨 스테이지 설계
            var stageData = CreateStageData(Level + 1, playerData,StageManager.Instance.timer.GetTime(), StageManager.Instance.GetTotalPoint());
            
            // 게임모드 확인
            switch (GameMode)
            {
                // 증강모드
                case GameMode.Augment:
                    // 증강 데이터 이전
                    stageData.currentAugment = lastStage.currentAugment;
                    stageData.ignoreAugment = lastStage.ignoreAugment;
                    
                    // 컨트롤러 변경
                    stageData.controllerName = AugmentLevel.Contains(stageData.level) ? ControllerID.AddAugment : ControllerID.Augmented;
                    break;
                
                // 기본모드
                case GameMode.Basic:
                default:
                    break;
            }
            
            // 증강 넘기기, 레벨 확인해서 
            EnterBattle(stageData);
        }

        public void EndGame(bool isDelete = false)
        {
            if (isDelete) DataManager.Instance.ResetData("PlayerData");
            StartCoroutine(SceneLoader.Instance.LoadMain());
        }

        #endregion
    }
}
