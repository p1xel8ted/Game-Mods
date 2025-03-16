namespace WorshippersOfCthulhu;

[Harmony]
public static class Patches
{
   
    [HarmonyPrefix]
    [HarmonyPatch(typeof(sacrificial), nameof(CurrencyManager.Spend), typeof(Cost), typeof(int), typeof(int))]
    [HarmonyPatch(typeof(CurrencyManager), nameof(CurrencyManager.Spend), typeof(ShopInventory), typeof(int))]
    [HarmonyPatch(typeof(CurrencyManager), nameof(CurrencyManager.SpendCoins))]
    public static bool CurrencyManager_Spend()
    {
        return false;
    }

}