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
