namespace Rebirth;

public static class SaveData
{
    public static bool FollowerBornAgain(FollowerInfo followerInfo)
    {
        return Plugin.RebirthSaveData.Data != null && Plugin.RebirthSaveData.Data.Exists(a => a == followerInfo.ID);
    }

    public static void AddBornAgainFollower(FollowerInfo followerInfo)
    {
        Plugin.RebirthSaveData.Data?.Add(followerInfo.ID);
        Plugin.RebirthSaveData.Save();
        Plugin.Log.LogInfo($"Saved follower data for {followerInfo.Name}");
    }
}