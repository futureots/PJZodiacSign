
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

        private void Start()
        {
            Screen.SetResolution(1920, 1080, true);
            giveUpBtn.SetActive(/*DataManager.Instance.isModified*/ false);     // TODO: DataManager 로드 확인 필요
        }

        public void StartGame()
        {
            if (DataManager.Instance.isModified)
            {
                GameManager.Instance.SetModeData(DataManager.Instance.levelTable, DataManager.Instance.shopTable);
                var data = GameManager.Instance.CreateStageData(DataManager.Instance.playData.stageLevel, DataManager.Instance.GetPlayerAgentData(),DataManager.Instance.playData.time);
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
                var data = GameManager.Instance.CreateStageData(1, new AgentData(150),0);
                GameManager.Instance.EnterBattle(data);
            }
        }

        public void StartTutorial()
        {
            GameManager.Instance.SetModeData(tutorialTable, shopTable);
            var data = GameManager.Instance.CreateStageData(1, new AgentData(9999),0);
            data.controllerName = ControllerID.Tutorial;
            GameManager.Instance.EnterBattle(data);
        }

        public void GiveUpGame()
        {
            DataManager.Instance.ResetData("PlayerData");
            StartCoroutine(SceneLoader.Instance.LoadMain());
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}