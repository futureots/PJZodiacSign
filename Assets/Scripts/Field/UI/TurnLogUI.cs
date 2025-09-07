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
        Entity.onDead += CreateDeadLog;
        BattlePhase.OnTurnStarted += CreateLog;
    }

    public void CreateLog(ITurn turn)
    {
        var block = Instantiate(turnBlock,content);
        blockList.Add(block.gameObject);
        block.Initialize(turn);
        curTurnBlock = block;
    }

    public void CreateDeadLog(Entity entity)
    {
        if (curTurnBlock == null) return;
        var block = Instantiate(logBlock, content);
        curTurnBlock.logs.Add(block);
        block.Initialize(entity);
    }

    private void OnEnable()
    {
        
        
    }
    private void OnDisable()
    {
        
    }

    private void OnDestroy()
    {
        Entity.onDead -= CreateDeadLog;
        BattlePhase.OnTurnStarted -= CreateLog;
    }
}
