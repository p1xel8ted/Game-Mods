namespace GetOuttaMaWay;

[Harmony]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.GlobalEventsCheck))]
    [HarmonyPatch(typeof(WorldMap), nameof(WorldMap.RescanWGOsList))]
    [HarmonyPatch(typeof(WorldZone), nameof(WorldZone.OnPlayerEnter))]
    public static void WorldMap_RescanWGOsList()
    {
        Plugin.GameStartedPlaying();
    }
}
