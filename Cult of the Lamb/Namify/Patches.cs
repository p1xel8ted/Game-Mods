using Shared;

namespace Namify;

[Harmony]
public static class Patches
{
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
    
    internal static void CleanNames(int id = -1)
    {
        var logDict = new Dictionary<int, string>();

        foreach (var follower in FollowerManager.SimFollowers.SelectMany(a => a.Value))
        {
            if (!follower.Brain.Info.Name.TryReplace("*", string.Empty, out var newName)) continue;
            
            follower.Brain.Info.Name = newName;
            logDict.TryAdd(follower.Brain.Info.ID, newName);
        }

        foreach (var follower in FollowerManager.Followers.SelectMany(a => a.Value))
        {
            if (!follower.Brain.Info.Name.TryReplace("*", string.Empty, out var newName)) continue;
            
            follower.Brain.Info.Name = newName;
            logDict.TryAdd(follower.Brain.Info.ID, newName);
        }

        foreach (var follower in DataManager.Instance.Followers)
        {
            if (!follower.Name.TryReplace("*", string.Empty, out var newName)) continue;
            
            follower.Name = newName;
            logDict.TryAdd(follower.ID, newName);
        }

        var f1 = FollowerManager.FindFollowerByID(id);
        if (f1)
        {
            if (f1.Brain.Info.Name.TryReplace("*", string.Empty, out var newName))
            {
                f1.Brain.Info.Name = newName;
                logDict.TryAdd(f1.Brain.Info.ID, newName);
            }
        }

        var f2 = FollowerManager.FindSimFollowerByID(id);
        if (f2 != null)
        {
            if (f2.Brain.Info.Name.TryReplace("*", string.Empty, out var newName))
            {
                f2.Brain.Info.Name = newName;
                logDict.TryAdd(f2.Brain.Info.ID, newName);
            }
        }

        foreach (var kvp in logDict)
        {
            Plugin.Log.LogInfo($"Cleaned ID: {kvp.Key} Name: {kvp.Value}");
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.OnInteract))]
    private static void interaction_FollowerInteraction_OnInteract()
    {
        CleanNames();
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerFarming), nameof(PlayerFarming.OnEnable))]
    private static void PlayerFarming_OnEnable()
    {
        CleanNames();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIFollowerIndoctrinationMenuController), nameof(UIFollowerIndoctrinationMenuController.Show), typeof(Follower), typeof(OriginalFollowerLookData), typeof(bool))]
    private static void UIFollowerIndoctrinationMenuController_Show(ref UIFollowerIndoctrinationMenuController __instance)
    {
        var instance = __instance;
        __instance._acceptButton.onClick.AddListener(delegate
        {
            var name = instance._targetFollower.Brain.Info.Name; //should contain an asterisk at this stage depending on settings
            
            name = name.Replace("*", string.Empty);

            Plugin.Log.LogInfo($"Follower name {name} confirmed! Removing name from saved name list and cleaning up asterisks if applicable.");

            Data.NamifyNames.Remove(name);
            Data.UserNames.Remove(name);
            
            instance._targetFollower.Brain.Info.Name = name;

            CleanNames();
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
        
        var bothNames = Data.NamifyNames.Concat(Data.UserNames).Distinct().ToList();
        var name = bothNames.RandomElement();
        
        if (Plugin.AsterixNames.Value && !name.Contains("*"))
        {
            name = $"{name}*";
        }

        __result = name;
    }
}