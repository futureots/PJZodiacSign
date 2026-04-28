using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    public List<GameObject> panelList;
    public Button leftBtn;
    public Button rightBtn;
    private int _currentCount = 0;


    public void Init()
    {
        gameObject.SetActive(true);
        foreach (var panel in panelList)
        {
            panel.SetActive(false);
        }
        _currentCount = 0;
        leftBtn.gameObject.SetActive(false);
        panelList[_currentCount].SetActive(true);
    }
    public void MoveNext()
    {
        // 마지막 페이지에서 다음으로 넘어갈 경우 종료
        if (_currentCount+1 >= panelList.Count)
        {
            gameObject.SetActive(false);
        }
        else
        {
            panelList[_currentCount].SetActive(false);
            _currentCount = Math.Clamp(0, _currentCount + 1, panelList.Count - 1);
            rightBtn.gameObject.SetActive(true);
            leftBtn.gameObject.SetActive(_currentCount > 0);
            panelList[_currentCount].SetActive(true);
        }
    }

    public void MovePrev()
    {
        panelList[_currentCount].SetActive(false);
        _currentCount = Math.Clamp(0, _currentCount - 1, panelList.Count-1);
        rightBtn.gameObject.SetActive(_currentCount < panelList.Count-1);
        leftBtn.gameObject.SetActive(_currentCount > 0);
        panelList[_currentCount].SetActive(true);
    }

    [ContextMenu("SetPanelList")]
    public void SetPanelList()
    {
        panelList.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            panelList.Add(transform.GetChild(i).gameObject);
        }
    }
}
