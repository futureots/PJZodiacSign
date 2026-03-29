using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public void Init(float time=0)
    {
        _elapsedTime = time;
        _isPaused = false;
    }

    public void Pause()
    {
        _isPaused = true;
    }

    public int GetTime()
    {
        return Mathf.FloorToInt(_elapsedTime);
    }

    private bool _isPaused;
    private float _elapsedTime;
    private int _lastDisplayedSecond = -1;

    public Action<int> onTimerUpdate;

    void Update()
    {
        if (_isPaused) return;
        
        _elapsedTime += Time.deltaTime;
        
        int currentSecond = Mathf.FloorToInt(_elapsedTime);
        if (currentSecond != _lastDisplayedSecond)
        {
            onTimerUpdate?.Invoke(currentSecond);
            _lastDisplayedSecond = currentSecond;
        }
    }
}
