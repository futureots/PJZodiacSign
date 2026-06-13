using Steamworks;

public class AchievementManager : Singleton<AchievementManager>
{
    public void Acheieve(string apiName)
    {
        if (SteamManager.Initialized)
        {
            SteamUserStats.GetAchievement(apiName, out bool isAchieved);

            if (!isAchieved)
            {
                SteamUserStats.SetAchievement(apiName);
                SteamUserStats.StoreStats();
            }
        }
    }
}
