using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main
{
    public class MainManager : MonoBehaviour
    {
        public GameObject startBtn;
        public GameObject giveUpBtn;


        public LevelTable levelTable;
        public ShopTable shopTable;

        private void Start()
        {
            Screen.SetResolution(1920, 1080, true);
            if (DataManager.isModified)
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
            if (DataManager.isModified)
            {
                GameManager.Instance.SetModeData(DataManager.levelTable, DataManager.shopTable);
                var data = GameManager.Instance.CreateStageData(DataManager.playData.stageLevel, DataManager.GetPlayerAgentData());
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

        public void GiveUpGame()
        {
            DataManager.ResetData("PlayerData");
            SceneManager.LoadScene(0);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}