namespace Rebirth;

[HarmonyPatch]
[HarmonyWrapSafe]
public static class Patches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(DropLootOnDeath), nameof(DropLootOnDeath.OnDie))]
    public static void DropLootOnDeath_OnDie(DropLootOnDeath __instance, Health Victim)
    {
        if (Victim.team == Health.Team.Team2)
        {
            if (Random.Range(0, 101) <= Plugin.EnemyDropRate.Value)
            {
                Plugin.Log.LogInfo($"Got a Rebirth token from {__instance.name}!");
                InventoryItem.Spawn(Plugin.RebirthItem, Random.Range(Plugin.DropMinQuantity.Value, Plugin.DropMaxQuantity.Value + 1), __instance.transform.position);
            }
        }
        else if (Victim.name.ToLower(CultureInfo.InvariantCulture).Contains("breakable body pile"))
        {
            if (Random.Range(0, 101) <= Plugin.EnemyDropRate.Value)
            {
                Plugin.Log.LogInfo($"Got a Rebirth token from {__instance.name}!");
                InventoryItem.Spawn(Plugin.RebirthItem, Random.Range(Plugin.DropMinQuantity.Value, Plugin.DropMaxQuantity.Value + 1), __instance.transform.position);
            }
        }
    }
}