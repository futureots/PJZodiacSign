using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndUI : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Button continueBtn;
    [SerializeField] Button endBtn;
    [SerializeField] Button stopBtn;
    [SerializeField] TextMeshProUGUI text;

    private void Awake()
    {
        continueBtn.onClick.AddListener(Continue);
        endBtn.onClick.AddListener(End);
        stopBtn.onClick.AddListener(Stop);
    }
    public void OnGameEnd(bool isWin)
    {
        if (isWin)
        {
            text.text = "승리!";
            continueBtn.gameObject.SetActive(true);
            stopBtn.gameObject.SetActive(true);
            endBtn.gameObject.SetActive(false);
        }
        else
        {
            text.text = "패배...";
            continueBtn.gameObject.SetActive(false);
            stopBtn.gameObject.SetActive(false);
            endBtn.gameObject.SetActive(true);
        }
    }

    void Continue()
    {
        GameManager.Instance.ContinueGame();
    }

    void Stop()
    {
        GameManager.Instance.EndGame();
    }
    void End()
    {
        GameManager.Instance.EndGame(true);
    }


    private void OnEnable()
    {
        GameManager.Instance.OnGameEnded += OnGameEnd;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnGameEnded -= OnGameEnd;
    }
}
