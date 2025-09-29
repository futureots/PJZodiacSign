using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhaseManager : MonoBehaviour, IPhaseManageService
{
    Field mainField;

    public LinkedList<IPhase> phases;
    IPhase currentPhase;
    public event Action<IPhase,bool> onPhaseChanged;

    public PhaseType CurPhase => currentPhase.PhaseType;

    private void Awake()
    {
        phases = new LinkedList<IPhase>();
    }

    public void Init(Field mainField)
    {
        this.mainField = mainField;
    }
    
    public void BeginPhase()
    {
        if (currentPhase != null)
        {
            // 현재 페이즈 종료
            onPhaseChanged?.Invoke(currentPhase, false);
        }

        if (phases.Count == 0)
        {
            Debug.Log("AllPhaseEnd");
            return;
        }
        
        currentPhase = phases.First.Value;
        phases.RemoveFirst();

        // 페이즈 시작
        onPhaseChanged?.Invoke(currentPhase, true);

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
        phases.AddLast(new RepairPhase(level, mainField));
        
        // 전투 페이즈를 추가
        var battlePhase = new BattlePhase(level,mainField);
        phases.AddLast(battlePhase);

        // 첫 번째 페이즈 시작
        BeginPhase();
    }
    private void OnEnable()
    {
        GameManager.onNextLevel += NextLevel;
    }
    private void OnDisable()
    {
        GameManager.onNextLevel -= NextLevel;
    }
}
[Flags]
public enum PhaseType
{
    None = 0,
    Battle = 1 << 0,
    Repair = 1 << 1
}

public interface IPhaseManageService
{
    public PhaseType CurPhase { get;}
    public event Action<IPhase, bool> onPhaseChanged;
}