using UnityEngine;

public class Status
{
    public int baseValue;
    public int modifier;

    public Status(int value)
    {
        baseValue = value;
        modifier = 0;
    }

    public int GetStatus()
    {
        return Mathf.Max(0, baseValue + modifier);
    }

    public void AddStatus(int value)
    {
        modifier += value;
    }
    public void RemoveStatus(int value)
    {
        modifier -= value;
    }

}
