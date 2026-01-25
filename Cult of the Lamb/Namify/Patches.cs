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

        // Clean up existing custom listeners to prevent memory leaks
        // We can't remove the game's listeners, so we track our own
        __instance._acceptButton.onClick.RemoveAllListeners();
        
        // Re-register the game's original onClick behavior by copying it from decompiled code
        // This ensures we have clean state and our listener runs in correct order
        __instance._acceptButton.onClick.AddListener(delegate
        {
            // Remove asterisk from the input field BEFORE game reads it
            var cleanName = instance._nameInputField.text.Replace("*", string.Empty);
            instance._nameInputField.text = cleanName;
            
            // Now let the game's logic run - it will read the cleaned name
            instance._targetFollower.Brain.Info.Name = LocalizeIntegration.Arabic_ReverseNonRTL(instance._nameInputField.text);
            
            if (!string.IsNullOrEmpty(instance.twitchFollowerViewerID))
            {
                DataManager.Instance.TwitchFollowerViewerIDs.Insert(0, instance.twitchFollowerViewerID);
                DataManager.Instance.TwitchFollowerIDs.Insert(0, instance.twitchFollowerID);
                instance.twitchFollowerViewerID = "";
                instance.twitchFollowerID = "";
            }
            
            // Remove name from our lists
            Plugin.Log.LogInfo($"Follower name {cleanName} confirmed! Removing name from saved name list.");
            Data.NamifyNames.Remove(cleanName);
            Data.UserNames.Remove(cleanName);
            
            instance.SwapOutfit();
            instance.Hide();
        });
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIFollowerIndoctrinationMenuController), nameof(UIFollowerIndoctrinationMenuController.RandomiseName))]
    private static void UIFollowerIndoctrinationMenuController_RandomiseName(UIFollowerIndoctrinationMenuController __instance)
    {
        if (!Plugin.AsterixNames.Value) return;

        var name = __instance._nameInputField.text;
        if (Data.NamifyNames.Contains(name) || Data.UserNames.Contains(name))
        {
            __instance._nameInputField.text = $"{name}*";
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerInfo), nameof(FollowerInfo.GenerateName))]
    public static void FollowerInfo_GenerateName(ref string __result)
    {
        if (GameManager.GetInstance() is null) return;
        if (DataManager.Instance is null) return;
        
        // If we have no names at all, try to load from file synchronously
        if (Data.NamifyNames.Count <= 0 && Data.UserNames.Count <= 0)
        {
            try
            {
                Data.LoadData();
            }
            catch (Exception ex)
            {
                Plugin.Log.LogWarning($"Failed to load names during generation: {ex.Message}");
            }
        }
        
        // If still no names after trying to load, trigger async fetch but use game's default name for now
        if (Data.NamifyNames.Count <= 0 && Data.UserNames.Count <= 0)
        {
            Plugin.Log.LogInfo("No names available yet, using game's default. Triggering background fetch...");
            Data.GetNamifyNames();
            return; // Keep the game's generated name
        }
        
        var bothNames = Data.NamifyNames.Concat(Data.UserNames).Distinct().ToList();
        __result = bothNames.RandomElement();
    }
}