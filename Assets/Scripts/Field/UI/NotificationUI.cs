using DG.Tweening;
using TMPro;
using UnityEngine;

public class NotificationUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI notificationText;
    public void Show(string text)
    {
        notificationText.text = text;
        var sequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.one*0.2f, 0.3f)).SetEase(Ease.InOutQuad)
            .AppendInterval(0.7f)
            .Append(transform.DOScale(Vector3.zero, 0.3f)).SetEase(Ease.InOutQuad);
    }
    private void Start()
    {
        SetZero();
        BattlePhase.OnTurnStarted += ShowMyTurn;
    }

    private void OnDestroy()
    {
        BattlePhase.OnTurnStarted -= ShowMyTurn;
    }

    void ShowMyTurn(ITurn turn)
    {
        if (turn.TeamNumber == 1 && turn is ActionTurn)
        {
            Show("나의 턴");
        }
    }

    [ContextMenu("SetZero")]
    public void SetZero() => transform.localScale = Vector3.zero;


    [ContextMenu("SetOne")]
    public void SetOne() => transform.localScale = Vector3.one;
}
