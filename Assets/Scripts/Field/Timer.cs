using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public void Init(float time=0)
    {
        _startTime = time;
        _elapsedTime = 0;
        _isPaused = false;
    }

    public void Pause()
    {
        _isPaused = true;
    }

    /// <summary>
    /// 시작 시간 + 걸린 시간 합 반환
    /// </summary>
    /// <returns></returns>
    public int GetTime()
    {
        return Mathf.FloorToInt(_lastDisplayedSecond);
    }

    /// <summary>
    /// 현재 스테이지에서만 걸린 시간 반환
    /// </summary>
    /// <returns></returns>
    public int GetElapsedTime()
    {
        return Mathf.FloorToInt(_elapsedTime);
    }

    /// <summary> 시작 시간 </summary>
	private float _startTime;
    /// <summary> 현재 걸린 시간 </summary>
    private float _elapsedTime;
    private bool _isPaused;
    
    private int _lastDisplayedSecond = -1;

    public Action<int> onTimerUpdate;

    void Update()
    {
        if (_isPaused) return;
        
        _elapsedTime += Time.deltaTime;
        
        int currentSecond = Mathf.FloorToInt(_elapsedTime+ _startTime);
        if (currentSecond != _lastDisplayedSecond)
        {
            onTimerUpdate?.Invoke(currentSecond);
            _lastDisplayedSecond = currentSecond;
        }
    }
}
