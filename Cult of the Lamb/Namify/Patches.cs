using Shared;

namespace Namify;

[Harmony]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Save), [])]
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

        // Migration: strip leftover asterisks from existing save data
        foreach (var follower in DataManager.Instance.Followers)
        {
            if (follower.Name.Contains("*"))
            {
                follower.Name = follower.Name.Replace("*", string.Empty);
            }
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIFollowerIndoctrinationMenuController), nameof(UIFollowerIndoctrinationMenuController.Show), typeof(Follower), typeof(OriginalFollowerLookData), typeof(bool))]
    private static void UIFollowerIndoctrinationMenuController_Show(UIFollowerIndoctrinationMenuController __instance)
    {
        var instance = __instance;

        if (Plugin.AsterixNames.Value)
        {
            var name = instance._targetFollower.Brain.Info.Name;
            if (Data.NamifyNames.Contains(name) || Data.UserNames.Contains(name))
            {
                instance._nameInputField.text = $"{instance._nameInputField.text}*";
            }
        }

        __instance._acceptButton.onClick.AddListener(delegate
        {
            var name = instance._nameInputField.text.Replace("*", string.Empty);

            Plugin.Log.LogInfo($"Follower name {name} confirmed! Removing name from saved name list.");

            Data.NamifyNames.Remove(name);
            Data.UserNames.Remove(name);

            instance._targetFollower.Brain.Info.Name = name;
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
        __result = bothNames.RandomElement();
    }
}