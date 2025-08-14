using DG.Tweening.Core.Easing;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    public LinkedList<IPhase> phases;
    
    private void Awake()
    {
        phases = new LinkedList<IPhase>();
        GameManager.OnNextLevel += NextLevel;
    }
    
    public void StartPhase()
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
        StartPhase();
    }
    
    public void NextLevel(int level)
    {
        phases.Clear();
        
        // 정비 페이즈를 먼저 추가
        phases.AddLast(new RepairPhase(level));
        
        // 전투 페이즈를 추가
        phases.AddLast(new BattlePhase());

        // 첫 번째 페이즈 시작
        StartPhase();
    }
    
}
