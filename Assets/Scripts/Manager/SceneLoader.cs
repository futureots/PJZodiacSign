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
            
            // 메인 씬 로드 확인 및 언로드
            Scene mainSceneCheck = SceneManager.GetSceneByName(ModelID.Main);
            if (mainSceneCheck.isLoaded)
            {
                EditorLogger.Print($"[SceneLoader] Unloading MainScene.");
                yield return SceneManager.UnloadSceneAsync(mainSceneCheck);
            }

            // 기존 필드 모델 씬 언로드
            if (_fieldScene.isLoaded && _fieldScene.name != ModelID.Main) 
            {
                yield return SceneManager.UnloadSceneAsync(_fieldScene);
            }

            // 기존 컨트롤러 씬 언로드
            if (_controllerScene.isLoaded)
            {
                yield return SceneManager.UnloadSceneAsync(_controllerScene);
            }

            // 이전 씬 언로드 대기 (1 프레임)
            yield return null; 

            // 모델 씬 로드
            if (SceneManager.GetSceneByName(modelName).isLoaded == false)
            {
                modelLoad = SceneManager.LoadSceneAsync(modelName, LoadSceneMode.Additive);
                if (modelLoad == null)
                {
                    EditorLogger.PrintError($"Failed to Load Model : {modelName}");
                    yield break;
                }
            }

            // 컨트롤러 씬 로드
            controllerLoad = SceneManager.LoadSceneAsync(controllerName, LoadSceneMode.Additive);
            if (controllerLoad == null)
            {
                EditorLogger.PrintError($"Failed to Load Controller : {controllerName}");
                yield break;
            }
            // 컨트롤러는 진입 타이밍 조율을 위해 0.9에서 대기
            controllerLoad.allowSceneActivation = false;
            
            // 모델 씬 로드, 컨트롤러 씬이 프리로드 대기
            while ((modelLoad is { isDone: false }) || (controllerLoad is { progress: < 0.9f }))
            {
                yield return null;
            }
            
            // 컨트롤러 씬 활성화
            controllerLoad.allowSceneActivation = true;
            
            // 로드 완료 대기
            yield return new WaitUntil(() => 
                (modelLoad == null || modelLoad.isDone) && 
                controllerLoad.isDone);

            // 데이터 참조 갱신
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
