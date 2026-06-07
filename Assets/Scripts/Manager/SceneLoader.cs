using System;
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
        /**
         * Scene Management System
         * - Field + Controller Manage
         * - Scene Load
         */
        
        private Scene _fieldScene;
        private Scene _controllerScene;

        protected override void Awake()
        {
            base.Awake();
            RefreshCurrentSceneReferences();
        }

        private void Start()
        {
            if (SceneManager.sceneCount != 1)
            {
                return;
            }

            // Runtime Entry to Main
            EditorLogger.Print("Loading Main Scene");
            StartCoroutine(LoadMain());
        }

        /// <summary>
        /// Load Main Scene (ModelID.Main)
        /// </summary>
        public IEnumerator LoadMain()
        {
            // Unload Scenes
            if (_controllerScene.isLoaded)
            {
                yield return SceneManager.UnloadSceneAsync(_controllerScene);
            }
            if (_fieldScene.isLoaded)
            {
                yield return SceneManager.UnloadSceneAsync(_fieldScene);
            }
            
            // Load Main Scene
            yield return SceneManager.LoadSceneAsync(ModelID.Main, LoadSceneMode.Additive);
            
            _fieldScene = SceneManager.GetSceneByName(ModelID.Main);
            SceneManager.SetActiveScene(_fieldScene);
        }

        /// <summary>
        /// Load Battle Scene
        /// </summary>
        /// <param name="modelName">ModelID</param>
        /// <param name="controllerName">FieldController</param>
        public IEnumerator LoadBattle(string modelName, string controllerName)
        {
            AsyncOperation modelLoad = null;
            AsyncOperation controllerLoad = null;
            
            // Unload Model
            if (_fieldScene.isLoaded)
            {
                EditorLogger.Print($"Unload Scene : {_fieldScene.name} - {_fieldScene.isLoaded}");
                yield return SceneManager.UnloadSceneAsync(_fieldScene);
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
            // NOTE : 씬은 강제 리로드
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
            _fieldScene = SceneManager.GetSceneByName(modelName);
            _controllerScene = SceneManager.GetSceneByName(controllerName);
            
            // Set Active Scene for Model
            SceneManager.SetActiveScene(_fieldScene);
        }
        
        
        /// <summary>
        /// Get Scene Reference and Refresh
        /// 에디터 부트스트래핑 시 호출됩니다.
        /// </summary>
        public void RefreshCurrentSceneReferences()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene s = SceneManager.GetSceneAt(i);
                
                switch (s.name)
                {
                    // 0. Except Self
                    case "System":
                        continue;
                    // 1. Main
                    case ModelID.Main:
                        _fieldScene = s;
                        EditorLogger.Print($"[SceneLoader] MainScene Detected: {_fieldScene.name}");
                        break;
                    // 2. Battle
                    default:
                    {
                        if (s.name.Contains("Controller") || s.name == ControllerID.Default)
                        {
                            _controllerScene = s;
                            EditorLogger.Print($"[SceneLoader] Controller Detected: {_controllerScene.name}");
                        }
                        // 3. 기타 모델 씬 인식
                        else if (s.name.Contains("Model"))
                        {
                            _fieldScene = s;
                            EditorLogger.Print($"[SceneLoader] Model Detected: {_fieldScene.name}");
                        }

                        break;
                    }
                }
            }
            
            if (_fieldScene.IsValid())
            {
                SceneManager.SetActiveScene(_fieldScene);
            }
        }
    }
}
