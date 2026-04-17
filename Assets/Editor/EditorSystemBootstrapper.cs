#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using GlobalManage;

/// <summary>
/// 에디터에서 Play 모드 진입 시 시스템 환경을 자동으로 구축합니다.
/// </summary>
[InitializeOnLoad]
public static class EditorSceneBootstrapper
{
    private const string SYSTEM_SCENE_NAME = "System";

    static EditorSceneBootstrapper()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            // 1. 이미 로드되어 있는지 확인
            if (IsSceneLoaded(SYSTEM_SCENE_NAME))
            {
                // 이미 로드되어 있다면 즉시 실행 시도
                TryRefresh();
            }
            else
            {
                // 2. 로드되어 있지 않다면 로드 이벤트를 구독하고 로드 시작
                SceneManager.sceneLoaded += OnSystemSceneLoaded;
                Debug.Log($"<color=cyan>[Bootstrapper]</color> '{SYSTEM_SCENE_NAME}' 씬을 자동 로드합니다.");
                SceneManager.LoadScene(SYSTEM_SCENE_NAME, LoadSceneMode.Additive);
            }
        }
    }

    private static void OnSystemSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 우리가 기다리던 System 씬인지 확인
        if (scene.name == SYSTEM_SCENE_NAME)
        {
            // 이벤트 중복 호출 방지를 위해 구독 해제
            SceneManager.sceneLoaded -= OnSystemSceneLoaded;

            // 씬 로드 직후에는 Awake가 아직 안 돌았을 수 있으므로 
            // 한 프레임 뒤에(다음 루프에) 실행하도록 delayCall 사용
            EditorApplication.delayCall += TryRefresh;
        }
    }

    private static void TryRefresh()
    {
        // 싱글톤 인스턴스가 있는지 확인
        if (SceneLoader.Instance != null)
        {
            EditorLogger.Print("<color=green>[Bootstrapper]</color> RefreshCurrentSceneReferences executed.");
            SceneLoader.Instance.RefreshCurrentSceneReferences();
        }
        else
        {
            // 여전히 Null이라면 씬 안의 오브젝트를 직접 찾아서 시도 (최후의 수단)
            var loader = Object.FindFirstObjectByType<SceneLoader>();
            if (loader != null)
            {
                loader.RefreshCurrentSceneReferences();
            }
            else
            {
                EditorLogger.PrintError("[Bootstrapper] SceneLoader instance could not be found even after load.");
            }
        }
    }

    private static bool IsSceneLoaded(string name)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == name) return true;
        }
        return false;
    }
}
#endif
