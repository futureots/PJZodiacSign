
using UnityEngine;
using GlobalManage;

namespace Main
{
    public class MainManager : MonoBehaviour
    {
        public GameObject startBtn;
        public GameObject giveUpBtn;


        public LevelTable levelTable;
        public LevelTable tutorialTable;
        public ShopTable shopTable;

        private void Awake()
        {
            // main씬 로드할 때마다 데이터를 불러와서 사용(매 레벨마다 데이터를 저장하기 떄문에 가장 최신 데이터를 불러올 수 있음)
            DataManager.Instance.LoadAllData("PlayerData");
        }

        private void Start()
        {
            Screen.SetResolution(1920, 1080, true);
            #if UNITY_EDITOR
            giveUpBtn.SetActive(DataManager.Instance && DataManager.Instance.isModified);
            #else
            giveUpBtn.SetActive(DataManager.Instance.isModified);
            #endif
        }

        public void StartGame()
        {
            if (DataManager.Instance.isModified)
            {
                var dataManager = DataManager.Instance;
                GameManager.Instance.SetModeData(DataManager.Instance.levelTable, DataManager.Instance.shopTable);
                var data = GameManager.Instance.CreateStageData(dataManager.playData.stageLevel, dataManager.GetPlayerAgentData(),dataManager.playData.time,dataManager.playData.point);
                
                // TODO: 모드 선택
                data.controllerName = ControllerID.AddAugment;
                
                GameManager.Instance.EnterBattle(data);
            }
            else
            {
                if (!GameManager.Instance)
                {
                    EditorLogger.Print("Missing GameManager");
                    return;
                }
                GameManager.Instance.SetModeData(levelTable, shopTable);
                var data = GameManager.Instance.CreateStageData(1, new AgentData(150),0,0);
                
                GameManager.Instance.EnterBattle(data);
            }
        }

        public void StartTutorial()
        {
            GameManager.Instance.SetModeData(tutorialTable, shopTable);
            var data = GameManager.Instance.CreateStageData(1, new AgentData(9999),0,0);
            data.controllerName = ControllerID.Tutorial;
            GameManager.Instance.EnterBattle(data);
        }

        public void GiveUpGame()
        {
            DataManager.Instance.ResetData("PlayerData");
            giveUpBtn.SetActive(false);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}