using UnityEngine;

public class StageEndUI : MonoBehaviour
{
    [SerializeField] private GameObject blindPanel;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject defeatUI;
    public void Init()
    {
        StageManager.Instance.OnStageEnded += id =>
        {
            blindPanel.SetActive(true);
            if (id == Agent.LocalPlayer.id)
            {
                // 승리 UI 표시
                winUI.SetActive(true);
            }
            else
            {
                // 패배 UI 표시
                defeatUI.SetActive(true);
            }

        };
    }

    public void ContinueGame()
    {
        GameManager.Instance.ContinueGame();
    }

    public void DefeatGame()
    {
        GameManager.Instance.EndGame(true);
    }
}
