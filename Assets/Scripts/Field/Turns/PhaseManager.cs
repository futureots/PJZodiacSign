using System.Collections.Generic;
using UnityEngine;

// PhaseManager 클래스 - RepairPhase와 CombatPhase를 관리
public class PhaseManager : MonoBehaviour
{
    public LinkedList<IPhase> phases;
    
    private void Awake()
    {
        phases = new LinkedList<IPhase>();
        // GameManager의 OnNextLevel 이벤트 구독
        GameManager.OnNextLevel += NextLevel;
    }
    
    // 페이즈 시작 메서드
    public void StartPhase()
    {
        if (phases.Count == 0)
        {
            Debug.Log("모든 페이즈가 완료되었습니다.");
            return;
        }
        
        var currentPhase = phases.First.Value;
        phases.RemoveFirst();
        
        currentPhase.StartPhase(OnPhaseComplete);
    }
    
    // 페이즈 완료 콜백
    private void OnPhaseComplete()
    {
        // 다음 페이즈 시작
        StartPhase();
    }
    
    // 다음 레벨 시작 메서드
    public void NextLevel(int level)
    {
        phases.Clear();
        
        // RepairPhase를 먼저 추가
        phases.AddLast(new RepairPhase(level));
        
        // CombatPhase를 추가
        phases.AddLast(new CombatPhase());
        
        // 첫 번째 페이즈 시작
        StartPhase();
    }
    
    /// <summary>
    /// 현재 레벨의 모든 페이즈가 완료되었을 때 호출
    /// </summary>
    public void OnLevelComplete()
    {
        Debug.Log("현재 레벨의 모든 페이즈가 완료되었습니다.");
        // 레벨 완료 처리 로직 추가
    }
}
