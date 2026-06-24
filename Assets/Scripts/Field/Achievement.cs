using UnityEngine;

public class Achievement : InputManagerUI
{
    InputManager _inputManager;
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
            var result = AchievementManager.Instance?.Achieve(apiName);
            //_inputManager.onMessageActivated?.Invoke($"achieve : {apiName} {result}",LogType.Log);
        }
        
    }

    public override void Init(InputManager inputManager)
    {
        _inputManager = inputManager;
    }
}
