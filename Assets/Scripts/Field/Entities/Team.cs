using System;

[Serializable]
public class Team
{
    private PlayerID _teamNumber;
    /// <summary>
    /// 소속한 팀 번호
    /// </summary>
    public PlayerID teamNumber
    {
        get { return _teamNumber; }
        set
        {
            _teamNumber = value;
            OnTeamChanged?.Invoke(_teamNumber);
        }
    }
    public event Action<PlayerID> OnTeamChanged;

    
    /// <summary>
    /// 소속한 팀이 같은지 확인
    /// </summary>
    /// <param name="number">비교하는 팀 번호</param>
    /// <returns></returns>
    public bool IsAlly(PlayerID number)
    {
        return number == teamNumber;
    }
    public bool IsAlly(Team team)
    {
        if (team == null) return false;
         return teamNumber == team.teamNumber;
    }
}
