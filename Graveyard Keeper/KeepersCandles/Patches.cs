namespace KeepersCandles;

[Harmony]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ChunkManager), nameof(ChunkManager.RescanAllObjects))]
    public static void ChunkManager_RescanAllObjects()
    {
        Plugin.OnGameBalanceLoaded();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.LateSaveFixer))]
    public static void GameSave_LateSaveFixer()
    {
        Plugin.OnGameBalanceLoaded();
    }
}