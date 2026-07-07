using UnityEngine;

public static class EditorLogger
{
    public static void Print(object message)
    {
    #if UNITY_EDITOR
        Debug.Log(message);
    #endif
    }
    
    public static void PrintWarning(object message)
    {
    #if UNITY_EDITOR
        Debug.LogWarning(message);
    #endif
    }
    
    public static void PrintError(object message)
    {
    #if UNITY_EDITOR
        Debug.LogError(message);
    #endif
    }
}
