using System.Collections.Generic;
using UnityEngine;

public class TurnLogUI : MonoBehaviour
{
    public int capacity;
    public Transform content;
    public TurnBlock turnBlock;
    public LogBlock logBlock;
    public List<GameObject> blockList;
    public TurnBlock curTurnBlock;
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
        var block = Instantiate(logBlock, content);
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
}
