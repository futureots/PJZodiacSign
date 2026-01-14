using UnityEngine;

public class StageEntry : MonoBehaviour
{
    [SerializeField]
    public StageData stageData = new StageData();

    public void EnterBattle()
    {
        if (!GameManager.Instance)
        {
            EditorLogger.Print("Missing GameManager");
            return;
        }
        
        GameManager.Instance.EnterBattle(stageData);
    }
}
