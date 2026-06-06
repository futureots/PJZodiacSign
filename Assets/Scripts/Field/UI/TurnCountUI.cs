using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnCountUI : InputManagerUI
{
    InputManager _inputManager;
    // 25턴 시작 시 버튼 활성화
    [SerializeField] private Button drawButton;

    [SerializeField] private CanvasGroup turnPanel;
    [SerializeField] private TextMeshProUGUI turnCountText;
    [SerializeField] private CalculateUI calcPanel;
    [SerializeField] private AudioSource audioSource;

    private int drawCount;
    private uint _prevCount;
    public float fadeDuration = 0.1f;
    public float displayDuration = 0.1f;
    
    // 50턴 시작 시 UI에서 계산 및 종료하기
    public override void Init(InputManager inputManager)
    {
        _inputManager = inputManager;
        var fieldController = _inputManager.agent.fieldController;
        fieldController.OnPhaseStarted += PhaseStart;
        fieldController.OnTurnStarted += TurnStart;
        fieldController.OnDraw += SetDraw;
        drawCount = fieldController.TurnLimit;
        
        drawButton.gameObject.SetActive(false);
        turnPanel.gameObject.SetActive(false);
        calcPanel.gameObject.SetActive(false);
    }

    void PhaseStart(Phase phase)
    {
        if(phase.phaseName == PhaseType.Battle) turnPanel.gameObject.SetActive(true);
        else turnPanel.gameObject.SetActive(false);
    }
    
    void TurnStart(Turn turn, uint count)
    {
        if (turnPanel.isActiveAndEnabled && _prevCount != count)
        {
            audioSource.Play();
            _prevCount = count;
            turnCountText.text = $"{count}/{drawCount}";
            var fade = DOTween.Sequence()
                .Append(turnPanel.DOFade(1f, fadeDuration).SetEase(Ease.OutCubic))
                .AppendInterval(displayDuration)
                .Append(turnPanel.DOFade(0f, fadeDuration).SetEase(Ease.InCubic));
        }

        // 턴 시작 시 활성화
        if (count == drawCount/2)
        {
            drawButton.gameObject.SetActive(true);
        }
        
    }

    // 무승부 선택 시 실행하는 함수
    public void SetDraw()
    {
        calcPanel.gameObject.SetActive(true);
        StartCoroutine(calcPanel.Calculate(_inputManager));
    }
}
