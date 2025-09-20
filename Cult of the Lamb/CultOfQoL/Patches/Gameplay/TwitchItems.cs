namespace CultOfQoL.Patches.Gameplay;

[HarmonyPatch]
internal static class TwitchItems
{
    // Centralized list of known Twitch skins
    private static readonly List<string> KnownTwitchSkins = ["TwitchCat", "TwitchMouse", "TwitchPoggers", "TwitchDog", "TwitchDogAlt"];
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BuildingShrine), nameof(BuildingShrine.OnEnableInteraction))]
    public static void BuildingShrine_OnEnableInteraction()
    {
        if (!Plugin.UnlockTwitchItems.Value) return;
    
        // Unlock totem decorations
        foreach (var totem in DataManager.GetAvailableTwitchTotemDecorations())
        {
            StructuresData.CompleteResearch(totem);
            StructuresData.SetRevealed(totem);
        }
    
        // Unlock hardcoded known skins
        foreach (var skin in KnownTwitchSkins)
        {
            DataManager.SetFollowerSkinUnlocked(skin);
        }
    
        // Unlock any additional Twitch skins from game data
        foreach (var skin in DataManager.GetAvailableTwitchTotemSkins())
        {
            DataManager.SetFollowerSkinUnlocked(skin);
        }
    }
    
    [Harmony]
    [UsedImplicitly]
    public static class Drops
    {
        private const string AuthenticateMethodPrefix = "Authenticate";
        
        // Paid DLC authentication methods to exclude
        private static readonly HashSet<string> PaidDlcKeywords = new(StringComparer.OrdinalIgnoreCase)
        {
            "Heretic",
            "Cultist", 
            "Sinful",
            "Pilgrim"
        };

        [UsedImplicitly]
        [HarmonyTargetMethods]
        public static IEnumerable<MethodBase> TargetMethods()
        {
            foreach (var method in AccessTools.GetDeclaredMethods(typeof(GameManager)))
            {
                if (!method.Name.StartsWith(AuthenticateMethodPrefix)) continue;
                
                // Skip if it's a paid DLC authentication method
                if (PaidDlcKeywords.Any(dlc => method.Name.Contains(dlc, StringComparison.OrdinalIgnoreCase))) continue;

                Plugin.L($"[Unlock Twitch Items] Patching {method.Name}");
                yield return method;
            }
        }

        [UsedImplicitly]
        [HarmonyPostfix]
        public static void Authenticate(ref bool __result)
        {
            if (Plugin.UnlockTwitchItems.Value)
                __result = true;
        }
    }
}