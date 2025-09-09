using System.Collections.Generic;
using UnityEngine;

public class TurnLogUI : MonoBehaviour
{
    public int capacity;
    
    public TurnBlock turnBlock;
    public LogBlock logBlock;
    public List<GameObject> blockList;
    public TurnBlock curTurnBlock;

    [Header("UIElement")]
    [SerializeField] GameObject LogToggle;
    [SerializeField] GameObject LogPanel;
    [SerializeField] Transform content;

    private void Awake()
    {
        blockList = new List<GameObject>();
    }
    void Start()
    {
        Entity.onEntityDead += CreateDeadLog;
        BattlePhase.OnTurnStarted += CreateTurnLog;
    }

    public void CreateTurnLog(ITurn turn)
    {
        var block = Instantiate(turnBlock,content);
        blockList.Add(block.gameObject);
        block.Initialize(turn);
        curTurnBlock = block;
        if (blockList.Count > capacity)
        {
            EraseFront();
        }
    }

    public void CreateDeadLog(Entity entity)
    {
        if (curTurnBlock == null) return;
        var block = Instantiate(logBlock,content);
        curTurnBlock.logs.Add(block);
        block.Initialize(entity);
    }

    private void OnDestroy()
    {
        Entity.onEntityDead -= CreateDeadLog;
        BattlePhase.OnTurnStarted -= CreateTurnLog;
    }

    [ContextMenu("Clear")]
    public void EraseFront()
    {
        Destroy(blockList[0]);
        blockList.RemoveAt(0);
    }

    void ClearLog()
    {
        //로그 오브젝트 제거
        
    }

    void PhaseChange(IPhase curPhase, bool start)
    {
        if (curPhase is BattlePhase)
        {
            // 페이즈 시작
            if (start)
            {
                LogToggle.SetActive(true);
                // logpanel 초기화
                ClearLog();
            }
            // 페이즈 종료
            else
            {
                LogToggle.SetActive(false);
                LogPanel.SetActive(false);
            }
        }
    }
    private void OnEnable()
    {
        PhaseManager.onPhaseChanged += PhaseChange;
    }

    private void OnDisable()
    {
        PhaseManager.onPhaseChanged -= PhaseChange;
    }
}
