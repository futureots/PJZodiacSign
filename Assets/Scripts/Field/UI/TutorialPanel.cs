using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    public List<RectTransform> panelList;
    public Button leftBtn;
    public Button rightBtn;
    private int _currentCount = 0;

    public void MoveNext()
    {
        panelList[_currentCount].DOAnchorPos(new Vector2(-Screen.width, 0), 1f).SetEase(Ease.Linear);
        _currentCount = Math.Clamp(0, _currentCount + 1, panelList.Count-1);
        rightBtn.gameObject.SetActive(_currentCount < panelList.Count-1);
        leftBtn.gameObject.SetActive(_currentCount > 0);
        panelList[_currentCount].DOAnchorPos(new Vector2(0, 0), 1f).SetEase(Ease.Linear);
    }

    public void MovePrev()
    {
        panelList[_currentCount].DOAnchorPos(new Vector2(Screen.width, 0), 1f).SetEase(Ease.Linear);
        _currentCount = Math.Clamp(0, _currentCount - 1, panelList.Count-1);
        rightBtn.gameObject.SetActive(_currentCount < panelList.Count-1);
        leftBtn.gameObject.SetActive(_currentCount > 0);
        panelList[_currentCount].DOAnchorPos(new Vector2(0, 0), 1f).SetEase(Ease.Linear);
    }

    [ContextMenu("SetPanelList")]
    public void SetPanelList()
    {
        panelList.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            panelList.Add(transform.GetChild(i).GetComponent<RectTransform>());
        }
    }
}
