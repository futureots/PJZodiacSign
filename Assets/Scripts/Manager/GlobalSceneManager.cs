
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManage
{
    public static class FieldModel
    {
        public const string Default = "FieldModel";
    }

    public static class FieldController
    {
        public const string Default = "BaseBattle";
    }
    public class GlobalSceneManager : Singleton<GlobalSceneManager>
    {
        [SerializeField] private GameObject LoadingUI;
        private Scene fieldScene;
        private Scene controllerScene;

        public void EnterBattle(StageData stageData)
        {
            string modelName = stageData.modelName;
            string controllerName = stageData.controllerName;
            
            LoadingUI.SetActive(true);
            
            // 0. Unload All 
            
            // 1. Load Model
            AsyncOperation modelLoad = SceneManager.LoadSceneAsync(modelName, LoadSceneMode.Additive);
            
        }
    }
}
