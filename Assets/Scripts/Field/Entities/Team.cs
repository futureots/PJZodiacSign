using UnityEngine;

public class Team : MonoBehaviour
{
    public int teamNumber;
    public bool isAlly(int number)
    {
        return number == teamNumber;
    }
    public bool isAlly(Team team)
    {
        return teamNumber == team.teamNumber;
    }
}
