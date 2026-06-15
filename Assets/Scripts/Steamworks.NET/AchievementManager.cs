using Steamworks;

public class AchievementManager : Singleton<AchievementManager>
{
    public bool Achieve(string apiName)
    {
        if (SteamManager.Initialized)
        {
            SteamUserStats.GetAchievement(apiName, out bool isAchieved);

            if (!isAchieved)
            {
                var value = SteamUserStats.SetAchievement(apiName);
                SteamUserStats.StoreStats();
                return value;
            }
        }

        return false;
    }
}
