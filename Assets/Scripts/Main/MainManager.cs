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
                var data = GameManager.Instance.CreateStageData(1, new AgentData(100));
                GameManager.Instance.EnterBattle(data);
            }
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