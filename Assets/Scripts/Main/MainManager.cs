using UnityEngine;
using UnityEngine.SceneManagement;

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
            if (DataManager.Instance.isModified)
            {
                giveUpBtn.SetActive(true);
            }
            else
            {
                giveUpBtn.SetActive(false);
            }
        }

        public void StartGame()
        {
            if (DataManager.Instance.isModified)
            {
                GameManager.Instance.SetModeData(DataManager.Instance.levelTable, DataManager.Instance.shopTable);
                var data = GameManager.Instance.CreateStageData(DataManager.Instance.playData.stageLevel, DataManager.Instance.GetPlayerAgentData());
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
                var data = GameManager.Instance.CreateStageData(1, new AgentData(150));
                GameManager.Instance.EnterBattle(data);
            }
        }

        public void StartTutorial()
        {
            GameManager.Instance.SetModeData(tutorialTable, shopTable);
            var data = GameManager.Instance.CreateStageData(1, new AgentData(50));
            // 데이터 세팅 및 상점 세팅(기본 기물 1개?, 아이템 1개 제공)
            // 튜토리얼 : 기물 구매, 아이템 사용, 기물 배치, 기물 이동, 기물 스킬 사용, 전투 클리어
            data.modelName = SceneName.FieldModel.Tutorial;
            GameManager.Instance.EnterBattle(data);
        }

        public void GiveUpGame()
        {
            DataManager.Instance.ResetData("PlayerData");
            SceneManager.LoadScene(0);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}