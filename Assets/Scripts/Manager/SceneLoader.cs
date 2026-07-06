using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GlobalManage
{
    public static class ModelID
    {
        public const string Default = "FieldModel";
        public const string Main = "MainScene";
    }

    public static class ControllerID
    {
        public const string Default = "BaseBattle";
        public const string Tutorial = "TutorialController";
        public const string Augmented = "AugmentDefault";
        public const string AddAugment = "AugmentAdd";
    }
    
    public class SceneLoader : Singleton<SceneLoader>
    {
        private Scene _fieldScene;
        private Scene _controllerScene;

        protected override void Awake()
        {
            base.Awake();
            RefreshCurrentSceneReferences();
        }

        private void Start()
        {
            if (SceneManager.sceneCount != 1) return;
            StartCoroutine(LoadMain());
        }

        public IEnumerator LoadMain()
        {
            if (_controllerScene.isLoaded) yield return SceneManager.UnloadSceneAsync(_controllerScene);
            if (_fieldScene.isLoaded) yield return SceneManager.UnloadSceneAsync(_fieldScene);
            
            yield return SceneManager.LoadSceneAsync(ModelID.Main, LoadSceneMode.Additive);
            
            _fieldScene = SceneManager.GetSceneByName(ModelID.Main);
            SceneManager.SetActiveScene(_fieldScene);
        }

        public IEnumerator LoadBattle(string modelName, string controllerName)
        {
            AsyncOperation modelLoad = null;
            AsyncOperation controllerLoad = null;
            
            //현재 메모리에 'MainScene' 직접 검사
            Scene mainSceneCheck = SceneManager.GetSceneByName(ModelID.Main);
            if (mainSceneCheck.isLoaded)
            {
                EditorLogger.Print($"[SceneLoader] Force Unloading Remaining MainScene.");
                yield return SceneManager.UnloadSceneAsync(mainSceneCheck);
            }

            // 기존 변수를 통한 언로드
            if (_fieldScene.isLoaded && _fieldScene.name != ModelID.Main) 
            {
                yield return SceneManager.UnloadSceneAsync(_fieldScene);
            }

            // 신규 모델 로드
            if (SceneManager.GetSceneByName(modelName).isLoaded == false)
            {
                modelLoad = SceneManager.LoadSceneAsync(modelName, LoadSceneMode.Additive);
                if (modelLoad == null)
                {
                    EditorLogger.PrintError($"Failed to Load Model : {modelName}");
                    yield break;
                }
                modelLoad.allowSceneActivation = false;
            }

            // 컨트롤러 리로드
            if (_controllerScene.isLoaded)
            {
                yield return SceneManager.UnloadSceneAsync(_controllerScene);
            }
            
            controllerLoad = SceneManager.LoadSceneAsync(controllerName, LoadSceneMode.Additive);
            if (controllerLoad == null)
            {
                EditorLogger.PrintError($"Failed to Load Controller : {controllerName}");
                yield break;
            }
            controllerLoad.allowSceneActivation = false;
            
            // '두 씬 중 하나라도 0.9 미만이라면' 계속 대기
            while ((modelLoad is { progress: < 0.9f }) || (controllerLoad is { progress: < 0.9f }))
            {
                yield return null;
                // TODO: Loading UI Refresh
            }
            
            // 씬 로드 완료 후 활성
            if (modelLoad != null) modelLoad.allowSceneActivation = true;
            controllerLoad.allowSceneActivation = true;
            
            yield return new WaitUntil(() => 
                (modelLoad == null || modelLoad.isDone) && 
                (controllerLoad == null || controllerLoad.isDone));

            // 데이터 갱신
            _fieldScene = SceneManager.GetSceneByName(modelName);
            _controllerScene = SceneManager.GetSceneByName(controllerName);
            
            if (_fieldScene.IsValid())
            {
                SceneManager.SetActiveScene(_fieldScene);
            }
        }
        
        public void RefreshCurrentSceneReferences()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene s = SceneManager.GetSceneAt(i);
                if (s.name == "System") continue;

                if (s.name == ModelID.Main)
                {
                    _fieldScene = s;
                }
                else if (s.name.Contains("Controller") || s.name == ControllerID.Default)
                {
                    _controllerScene = s;
                }
                else if (s.name.Contains("Model") || s.name == ModelID.Default)
                {
                    _fieldScene = s;
                }
            }
            
            if (_fieldScene.IsValid())
            {
                SceneManager.SetActiveScene(_fieldScene);
            }
        }
    }
}
