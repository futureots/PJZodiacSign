using UnityEngine;

public class EndCommand : Command
{
    public EndCommand()
    {
    }

    public override void Execute()
    {
        Debug.Log("Turn End");
    }
}
