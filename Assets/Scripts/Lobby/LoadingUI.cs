using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI loadingText;

    private IEnumerator _loading;
    private int _count;
    [SerializeField] private int maxCount=3;
    
    private IEnumerator Fade(float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        

    }

    /// <summary>
    /// 화면을 가리는 함수
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public IEnumerator FadeIn(float duration)
    {
        _loading = Loading();
        StartCoroutine(_loading);
        yield return Fade(1f, duration);
        canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// 화면을 드러내는 함수
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public IEnumerator FadeOut(float duration)
    {
        yield return Fade(0f, duration);
        canvasGroup.blocksRaycasts = false;
        StopCoroutine(_loading);
    }

    private IEnumerator Loading()
    {
        while (true)
        {
            _count += 1;
            if (_count >= maxCount)
            {
                _count %= maxCount;
            }

            string text = "Loading.";
            for (int i = 0; i < _count; i++)
            {
                text += ".";
            }

            loadingText.text = text;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
