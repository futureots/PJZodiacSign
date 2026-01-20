using System.Collections.Generic;
using UnityEngine;

public class TurnLogUI : MonoBehaviour
{
    
    public int capacity;
    
    public CommandBlock turnBlock;
    public LogBlock logBlock;
    public List<GameObject> blockList;
    public CommandBlock curTurnBlock;

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
        //BattlePhase.OnTurnStarted += CreateTurnLog;
    }

    public void Init()
    {
        
        //phaseManageService.onPhaseChanged += PhaseChange;
    }

    public void CreateTurnLog()
    {
        var block = Instantiate(turnBlock,content);
        blockList.Add(block.gameObject);
        //block.Initialize(turn);
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
        //BattlePhase.OnTurnStarted -= CreateTurnLog;
    }

    [ContextMenu("Clear")]
    public void EraseFront()
    {
        if (blockList.Count <= 0) return;
        Destroy(blockList[0]);
        blockList.RemoveAt(0);
    }




}
