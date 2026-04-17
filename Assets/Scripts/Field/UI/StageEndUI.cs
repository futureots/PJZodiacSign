using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageEndUI : MonoBehaviour
{
    InputManager _inputManager;
    [SerializeField] private GameObject blindPanel;
    [SerializeField] private GameObject panel;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button defeatButton;
    [SerializeField] private TextMeshProUGUI title;
    public void Init(InputManager inputManager)
    {
        _inputManager = inputManager;
        StageManager.Instance.OnStageEnded += id =>
        {
            blindPanel.SetActive(true);
            panel.SetActive(true);
            if (id == Agent.LocalPlayer.id)
            {
                title.text = "클리어!";
                // 승리 시에만 다음 버튼 활성화
                continueButton.gameObject.SetActive(true);
            }
        };
    }

    public void ContinueGame()
    {
        var credit = _inputManager.agent.Credit;
        _inputManager.agent.Credit += 100 + Mathf.RoundToInt(credit*0.2f);
        
        // TODO : Continue Game 작업?
    }

    public void DefeatGame()
    {
        // TODO : Defeat Game 작업?
    }
}
