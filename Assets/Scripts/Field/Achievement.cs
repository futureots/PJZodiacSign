using UnityEngine;

public class Achievement : MonoBehaviour
{
    private void Start()
    {
        StageManager.Instance.OnStageEnded += StageEnd;
    }

    void StageEnd(PlayerID winner)
    {
        if (winner == Agent.LocalPlayer.id)
        {
            string apiName = "Clear_Level_";
            var level = StageManager.Instance.level;
            apiName += level;
            AchievementManager.Instance?.Acheieve(apiName);
        }
        
    }
}
