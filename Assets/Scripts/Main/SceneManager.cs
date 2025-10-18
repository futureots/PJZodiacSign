using UnityEngine;
using UnityEngine.SceneManagement;

namespace Debugging
{
    public class MainManager : MonoBehaviour
    {
        public void LoadScene()
        {
            SceneManager.LoadScene(1);
        }
    }
}