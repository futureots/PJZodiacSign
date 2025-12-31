using UnityEditor;

// Editor Fix : Using Default Inspector GUI
[CustomEditor(typeof(UnityEngine.Object), true)]
[CanEditMultipleObjects]
public class GlobalInspectorFix : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        // 기본 인스펙터를 IMGUI 방식으로 출력
        DrawDefaultInspector();
        
        serializedObject.ApplyModifiedProperties();
    }
}