namespace CultOfQoL.Patches.Player;

[Harmony]
public static class PlayerPatches
{
    private const string PlayerPrefab = "PlayerPrefab";

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.Start))]
    public static void PlayerController_Start(ref PlayerController __instance)
    {
        Plugin.DodgeSpeed.Value = __instance.DodgeSpeed;
        Plugin.RunSpeed.Value = __instance.RunSpeed;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Health), nameof(Health.DealDamage))]
    public static void Health_DealDamage(ref Health __instance, ref float Damage, ref GameObject Attacker)
    {
        if (__instance is null) return;
        if (__instance.isPlayer) return; // Don't apply to player
        if (!Attacker.name.Contains(PlayerPrefab)) return; // Only apply to player attacks

        if (Helpers.IsMultiplierActive(Plugin.BaseDamageMultiplier.Value))
        {
            Damage *= Mathf.Abs(Plugin.BaseDamageMultiplier.Value);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.Update))]
    public static void PlayerController_Update(ref PlayerController __instance)
    {
        if (!Helpers.IsMultiplierActive(Plugin.RunSpeedMulti.Value)) return;

        var inDungeon = LocationManager.LocationIsDungeon(PlayerFarming.Location);

        // Reset to base speed if conditions require it
        if ((Plugin.DisableRunSpeedInDungeons.Value && inDungeon) ||
            (Plugin.DisableRunSpeedInCombat.Value && inDungeon && IsInCombat()))
        {
            __instance.RunSpeed = Plugin.RunSpeed.Value;
            return;
        }

        __instance.RunSpeed = Plugin.RunSpeed.Value *= Mathf.Abs(Plugin.RunSpeedMulti.Value);
    }

    private static bool IsInCombat()
    {
        var enemyOnboarding = Resources.FindObjectsOfTypeAll<EnemyOnboarding>();

        if (enemyOnboarding.All(eo => eo.enemies == null))
            return false;

        return enemyOnboarding
            .Where(eo => eo.enemies != null)
            .SelectMany(eo => eo.enemies)
            .Any(enemy => enemy && enemy.health && !enemy.health.Unaware);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.Lunge), typeof(float), typeof(float))]
    public static void PlayerController_Lunge(ref float lungeSpeed)
    {
        if (Helpers.IsMultiplierActive(Plugin.LungeSpeedMulti.Value))
        {
            lungeSpeed *= Mathf.Abs(Plugin.LungeSpeedMulti.Value);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.DoIslandDash), typeof(Vector3))]
    public static void PlayerController_DoIslandDash(ref PlayerController __instance)
    {
        if (Helpers.IsMultiplierActive(Plugin.DodgeSpeedMulti.Value))
        {
            __instance.DodgeSpeed = Plugin.DodgeSpeed.Value *= Mathf.Abs(Plugin.DodgeSpeedMulti.Value);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerFarming), nameof(PlayerFarming.DodgeRoll))]
    public static void PlayerFarming_DodgeRoll(ref PlayerFarming __instance)
    {
        if (Helpers.IsMultiplierActive(Plugin.DodgeSpeedMulti.Value))
        {
            __instance.playerController.DodgeSpeed = Plugin.DodgeSpeed.Value *= Mathf.Abs(Plugin.DodgeSpeedMulti.Value);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerWeapon), nameof(PlayerWeapon.DoAttackRoutine))]
    public static void PlayerWeapon_DoAttackRoutine(ref PlayerWeapon __instance)
    {
        if (Helpers.IsMultiplierActive(Plugin.DodgeSpeedMulti.Value))
        {
            __instance.playerController.DodgeSpeed = Plugin.DodgeSpeed.Value *= Mathf.Abs(Plugin.DodgeSpeedMulti.Value);
        }
    }
}