using UnityEngine;
using UnityEngine.SceneManagement;

namespace Debugging
{
    public class MainManager : MonoBehaviour
    {
        public GameObject startBtn;
        public GameObject giveUpBtn;

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

        public void LoadScene()
        {
            SceneManager.LoadScene(1);
        }

        public void GiveUpGame()
        {
            DataManager.Instance.ResetData("data");
            SceneManager.LoadScene(0);
        }
    }
}