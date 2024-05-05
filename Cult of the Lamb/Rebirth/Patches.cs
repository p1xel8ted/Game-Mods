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
            if (CustomItemManager.DropLoot(Plugin.RebirthItemInstance,0.05f))
            {
                Plugin.Log.LogInfo($"Got a Rebirth token from {__instance.name}!");
                InventoryItem.Spawn(Plugin.RebirthItem, Random.Range(1, 3), __instance.transform.position);
            }
        }

        if (Victim.name.ToLower(CultureInfo.InvariantCulture).Contains("breakable body pile"))
        {
            if (CustomItemManager.DropLoot(Plugin.RebirthItemInstance,0.05f))
            {
                Plugin.Log.LogInfo($"Got a Rebirth token from {__instance.name}!");
                InventoryItem.Spawn(Plugin.RebirthItem, Random.Range(1, 3), __instance.transform.position);
            }
        }
    }
}