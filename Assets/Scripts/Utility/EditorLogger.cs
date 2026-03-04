using UnityEngine;

public static class EditorLogger
{
    public static void Print(object message)
    {
    #if UNITY_EDITOR
            Debug.Log(message);
    #endif
    }
}
