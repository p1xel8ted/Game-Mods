namespace CultOfQoL.Patches.Gameplay;

[Harmony]
internal static class TwitchItems
{
    // All free Twitch drop skins (cross-referenced from DataManager.ActivateTwitchDrop1-20 + ActivateSupportStreamer)
    private static readonly List<string> KnownTwitchSkins =
    [
        "TwitchCat", "TwitchMouse", "TwitchPoggers", "TwitchDog", "TwitchDogAlt",
        "Lion", "Penguin", "Kiwi", "Pelican",
        "Anglerfish", "SeaButterfly", "Jellyfish", "Leech", "LizardTongue",
        "DogTeddy"
    ];
    
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
    public static class Drops
    {
        private const string AuthenticateMethodPrefix = "Authenticate";
        
        // Paid DLC authentication methods to exclude
        private static readonly HashSet<string> PaidDlcKeywords = new(StringComparer.OrdinalIgnoreCase)
        {
            "Heretic",
            "Cultist",
            "Sinful",
            "Pilgrim",
            "Major"
        };

        [HarmonyTargetMethods]
        public static IEnumerable<MethodBase> TargetMethods()
        {
            foreach (var method in AccessTools.GetDeclaredMethods(typeof(GameManager)))
            {
                if (!method.Name.StartsWith(AuthenticateMethodPrefix)) continue;
                
                // Skip if it's a paid DLC authentication method
                if (PaidDlcKeywords.Any(dlc => method.Name.Contains(dlc, StringComparison.OrdinalIgnoreCase))) continue;

                Plugin.WriteLog($"[Unlock Twitch Items] Patching {method.Name}");
                yield return method;
            }
        }

        [HarmonyPostfix]
        public static void Authenticate(ref bool __result)
        {
            if (Plugin.UnlockTwitchItems.Value)
            {
                __result = true;
            }
        }
    }
}