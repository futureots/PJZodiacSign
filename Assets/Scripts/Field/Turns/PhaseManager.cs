using DG.Tweening.Core.Easing;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    public LinkedList<IPhase> phases;
    public static PhaseType curPhase;

    private void Awake()
    {
        phases = new LinkedList<IPhase>();
        GameManager.OnNextLevel += NextLevel;
        curPhase = PhaseType.None;
    }
    
    public void BeginPhase()
    {
        // 엔딩 제외 실행되면 안되는 부분
        if (phases.Count == 0)
        {
            Debug.Log("AllPhaseEnd");
            return;
        }
        
        var currentPhase = phases.First.Value;
        phases.RemoveFirst();
        
        currentPhase.StartPhase(OnPhaseComplete);
    }
    
    private void OnPhaseComplete()
    {
        // 다음 페이즈 시작
        BeginPhase();
    }
    
    public void NextLevel(int level)
    {
        phases.Clear();
        
        // 정비 페이즈를 먼저 추가
        phases.AddLast(new RepairPhase(level));
        
        // 전투 페이즈를 추가
        phases.AddLast(new BattlePhase());

        // 첫 번째 페이즈 시작
        BeginPhase();
    }
    
}
[Flags]
public enum PhaseType
{
    None = 0,
    Battle = 1 << 0,
    Repair = 1 << 1
}