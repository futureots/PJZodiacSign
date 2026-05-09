#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class EditorSceneBootstrapper
{
    private const string SYSTEM_SCENE_NAME = "System";

    static EditorSceneBootstrapper()
    {
        EditorApplication.playModeStateChanged += (state) =>
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                // System 씬이 로드되지 않았다면 로드만 해줌
                if (!IsSceneLoaded(SYSTEM_SCENE_NAME))
                {
                    Debug.Log($"<color=cyan>[Bootstrapper]</color> {SYSTEM_SCENE_NAME} 씬을 로드합니다.");
                    SceneManager.LoadScene(SYSTEM_SCENE_NAME, LoadSceneMode.Additive);
                }
            }
        };
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