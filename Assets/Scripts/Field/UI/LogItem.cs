using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogItem : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public CanvasGroup canvasGroup;

    private Coroutine disableCoroutine;

    public void Setup(string message, Color color) {
        // 기존에 돌고 있던 코루틴이 있다면 정지 (연타 시 오류 방지)
        if (disableCoroutine != null) StopCoroutine(disableCoroutine);
        
        messageText.text = message;
        messageText.color = color;
        
        canvasGroup.alpha = 1f; // 초기화
        disableCoroutine = StartCoroutine(AutoDisable());
    }

    private IEnumerator AutoDisable() {
        yield return new WaitForSeconds(0.5f); // 잠시 대기
        
        // 페이드 아웃 연출 (Optional)
        while (canvasGroup.alpha > 0) {
            canvasGroup.alpha -= Time.deltaTime * 2f;
            yield return null;
        }

        // 파괴하지 않고 비활성화하여 풀로 돌려보냄
        gameObject.SetActive(false);
    }
}
