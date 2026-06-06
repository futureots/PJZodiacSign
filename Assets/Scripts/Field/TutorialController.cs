using UnityEngine;

public class TutorialController : FieldController
{
    public override void EndGame()
    {
        // 튜토리얼은 데이터 저장하지 않고 넘어감
        OnBattleEnd?.Invoke("End");
    }

    public override void EndStage(PlayerID winner)
    {
        StageManager.EndStage(winner);
    }
}
