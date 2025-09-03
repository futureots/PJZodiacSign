using System.Collections.Generic;
using UnityEngine;

public class TurnLogUI : MonoBehaviour
{
    public int capacity;
    public Transform content;
    public CommandBlock commandBlock;
    public List<GameObject> blockList;
    private void Awake()
    {
        blockList = new List<GameObject>();
    }
    void Start()
    {
        
    }

    public void CreateLog(Agent agent, Command command)
    {
        var block = Instantiate(commandBlock,content);
        blockList.Add(block.gameObject);
        block.Init(command, agent);

    }

    private void OnEnable()
    {
        
        
    }
    private void OnDisable()
    {
        
    }
}
