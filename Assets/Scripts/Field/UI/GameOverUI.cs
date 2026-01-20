using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public Action onEnd;
    public Action onContinue;
    [SerializeField]
    Button endBtn;
    [SerializeField]
    Button continueBtn;

    [SerializeField]
    TextMeshProUGUI text;

    private void Awake()
    {
        endBtn.onClick.AddListener(()=> onEnd.Invoke());
        continueBtn.onClick.AddListener(()=> onContinue.Invoke());
    }
    // 게임 클리어인지, 패배에 의한 게임 오버인지 확인
    public void Initialize(bool isClear = false)
    {
        if (isClear) {
            text.text = "게임 클리어";
            continueBtn.gameObject.SetActive(true);
        }
        else
        {
            text.text = "게임 오버";
            continueBtn.gameObject.SetActive(false);
        }

        //점수 계산 등등
        
    }
}
