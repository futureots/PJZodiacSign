using UnityEngine;

public class Team : MonoBehaviour
{
    /// <summary>
    /// 소속한 팀 번호
    /// </summary>
    public int teamNumber;
    /// <summary>
    /// 소속한 팀이 같은지 확인
    /// </summary>
    /// <param name="number">비교하는 팀 번호</param>
    /// <returns></returns>
    public bool IsAlly(int number)
    {
        return number == teamNumber;
    }
    public bool IsAlly(Team team)
    {
        if (team == null) return false;
         return teamNumber == team.teamNumber;
    }
}
