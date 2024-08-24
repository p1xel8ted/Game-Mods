namespace Namify;

[Harmony]
public static class Patches
{
    private static string _pendingName = string.Empty;
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Save))]
    private static void SaveAndLoad_Save()
    {
        if (!DataManager.Instance.AllowSaving || CheatConsole.IN_DEMO) return;
        Data.SaveData();
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Load))]
    private static void SaveAndLoad_Load(int saveSlot)
    {
        if (CheatConsole.IN_DEMO) return;
        Data.LoadData();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIFollowerIndoctrinationMenuController), nameof(UIFollowerIndoctrinationMenuController.Show), typeof(Follower), typeof(OriginalFollowerLookData), typeof(bool))]
    private static void UIFollowerIndoctrinationMenuController_Show(ref UIFollowerIndoctrinationMenuController __instance)
    {
        var instance = __instance;
        __instance._acceptButton.onClick.AddListener(delegate
        {
            var name = instance._targetFollower.Brain.Info.Name;
            if (name != _pendingName) return;
            Plugin.Log.LogInfo($"Follower name {name} confirmed! Removing name from saved name list.");
            Data.NamifyNames.Remove(_pendingName);
            Data.UserNames.Remove(_pendingName);
            
            //remove * from instance._targetFollower.Brain.Info.Name
            instance._targetFollower.Brain.Info.Name = instance._targetFollower.Brain.Info.Name.Replace("*", string.Empty);
            _pendingName = string.Empty;
        });
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerInfo), nameof(FollowerInfo.GenerateName))]
    public static void FollowerInfo_GenerateName(ref string __result)
    {
        if (GameManager.GetInstance() is null) return;
        if (DataManager.Instance is null) return;
        if (Data.NamifyNames.Count <= 0)
        {
            Data.GetNamifyNames();
        }
        if (Data.NamifyNames.Count <= 0 && Data.UserNames.Count <= 0)
        {
            Plugin.Log.LogError("No names found!");
            return;
        }
        if (__result == _pendingName) return;

        var bothNames = Data.NamifyNames.Concat(Data.UserNames).Distinct().ToList();
        _pendingName = bothNames.RandomElement();
        _pendingName = $"{_pendingName}*";

        __result = _pendingName;
    }
}