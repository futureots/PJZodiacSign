using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GlobalManage
{
    public static class FieldModel
    {
        public const string Default = "FieldModel";
        public const string Main = "MainScene";
    }

    public static class FieldController
    {
        public const string Default = "BaseBattle";
    }
    
    public class SceneLoader : Singleton<SceneLoader>
    {
        [SerializeField] private GameObject LoadingUI;
        private Scene fieldScene;
        private Scene controllerScene;
        
        #if UNITY_EDITOR
        public void Start()
        {
            // Scene Entry Error
            if (SceneManager.sceneCount > 2)
            {
                EditorLogger.PrintError($"Scene Load Exception: Odd Scene is Loaded");
                return;
            }

            // System Scene Entry
            if (SceneManager.sceneCount < 2)
            {
                StartCoroutine(LoadMain());
                return;
            }

            // Main Scene Loader
            if (SceneManager.GetSceneByName(FieldModel.Main).isLoaded)
            {
                fieldScene = SceneManager.GetSceneByName(FieldModel.Main);
            }
            else
            {
                EditorLogger.PrintError($"Scene Load Exception: Odd Scene is Loaded");
            }
            
            EditorLogger.Print($"Current Field : {fieldScene.name}");
        }
        #endif

        public IEnumerator LoadMain()
        {
            // Set Load UI
            LoadingUI.SetActive(true);
            
            // Unload Scenes
            if (controllerScene.isLoaded)
            {
                yield return SceneManager.UnloadSceneAsync(controllerScene);
            }
            if (fieldScene.isLoaded)
            {
                yield return SceneManager.UnloadSceneAsync(fieldScene);
            }
            
            // Load Main Scene
            yield return SceneManager.LoadSceneAsync(FieldModel.Main, LoadSceneMode.Additive);
            
            fieldScene = SceneManager.GetSceneByName(FieldModel.Main);
            SceneManager.SetActiveScene(fieldScene);
            
            // Finish Load
            LoadingUI.SetActive(false);
        }

        public IEnumerator LoadBattle(string modelName, string controllerName)
        {
            // Set Load UI
            LoadingUI.SetActive(true);

            AsyncOperation modelLoad = null;
            AsyncOperation controllerLoad = null;
            
            // Unload Model
            if (fieldScene.isLoaded && modelName != fieldScene.name)
            {
                EditorLogger.Print($"Unload Scene : {fieldScene.name} - {fieldScene.isLoaded}");
                yield return SceneManager.UnloadSceneAsync(fieldScene);
            }
            // Load New Model
            if (SceneManager.GetSceneByName(modelName).isLoaded == false)
            {
                modelLoad = SceneManager.LoadSceneAsync(modelName, LoadSceneMode.Additive);
                if (modelLoad == null)
                {
                    EditorLogger.PrintError($"Failed to Load Model : {modelName}");
                    yield break;
                }
            }

            // Reload Controller
            // NOTE : Controller씬은 강제 리로드
            if (controllerScene.isLoaded)
            {
                yield return SceneManager.UnloadSceneAsync(controllerScene);
            }
            controllerLoad = SceneManager.LoadSceneAsync(controllerName, LoadSceneMode.Additive);
            if (controllerLoad == null)
            {
                EditorLogger.PrintError($"Failed to Load Controller : {controllerName}");
                yield break;
            }
            controllerLoad.allowSceneActivation = false;
            
            // 3. Wait for Load
            while (modelLoad is { progress: < 0.9f } && controllerLoad is { progress: < 0.9f })
            {
                yield return null;
                // TODO: Loading UI Refresh
            }
            
            // Start Scene Activate
            controllerLoad.allowSceneActivation = true;
            if (modelLoad != null) modelLoad.allowSceneActivation = true;
            
            yield return new WaitUntil(() => 
                (modelLoad == null || modelLoad.isDone) && 
                (controllerLoad == null || controllerLoad.isDone));

            // refresh Scene
            fieldScene = SceneManager.GetSceneByName(modelName);
            controllerScene = SceneManager.GetSceneByName(controllerName);
            
            // Set Active Scene for Model
            SceneManager.SetActiveScene(fieldScene);
        }
    }
}
