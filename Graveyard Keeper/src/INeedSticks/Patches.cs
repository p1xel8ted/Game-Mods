namespace INeedSticks;

[Harmony]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameBalance), nameof(GameBalance.LoadGameBalance))]
    public static void GameBalance_LoadGameBalance()
    {
        Plugin.OnGameBalanceLoad();
    }
}
