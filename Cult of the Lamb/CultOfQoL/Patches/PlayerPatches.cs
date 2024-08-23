namespace CultOfQoL.Patches;

[HarmonyPatch]
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
        if (Plugin.EnableBaseDamageMultiplier.Value)
        {
            Damage *= Mathf.Abs(Plugin.BaseDamageMultiplier.Value);
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.Update))]
    public static void PlayerController_Update(ref PlayerController __instance)
    {
        if (!Plugin.EnableRunSpeedMulti.Value) return;
        
        var inDungeon = LocationManager.LocationIsDungeon(PlayerFarming.Location);
        if (Plugin.DisableRunSpeedInDungeons.Value && inDungeon)
        {
            __instance.RunSpeed = Plugin.RunSpeed.Value;
            return;
        }

        if (Plugin.DisableRunSpeedInCombat.Value && inDungeon && IsInCombat())
        {
            __instance.RunSpeed = Plugin.RunSpeed.Value;
            return;
        }

        __instance.RunSpeed = Plugin.RunSpeed.Value *= Mathf.Abs(Plugin.RunSpeedMulti.Value);
    }

    private static bool IsInCombat()
    {
        var inCombat = false;

        var enemyOnboarding = Resources.FindObjectsOfTypeAll<EnemyOnboarding>();
        var allNull = enemyOnboarding.All(eo => eo.enemies == null);
        if (allNull)
        {
            return false;
        }

        foreach (var eo in enemyOnboarding)
        {
            if (eo.enemies == null) continue;

            if (eo.enemies.Any(enemy => enemy && enemy.health && !enemy.health.Unaware))
            {
                inCombat = true;
            }
        }

        return inCombat;
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.Lunge), typeof(float), typeof(float))]
    public static void PlayerController_Lunge(ref float lungeSpeed)
    {
        if (Plugin.EnableLungeSpeedMulti.Value)
        {
            lungeSpeed *= Mathf.Abs(Plugin.LungeSpeedMulti.Value);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.DoIslandDash), typeof(Vector3))]
    public static void PlayerController_DoIslandDash(ref PlayerController __instance)
    {
        if (Plugin.EnableDodgeSpeedMulti.Value)
        {
            __instance.DodgeSpeed = Plugin.DodgeSpeed.Value *= Mathf.Abs(Plugin.DodgeSpeedMulti.Value);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerFarming), nameof(PlayerFarming.DodgeRoll))]
    public static void PlayerFarming_DodgeRoll(ref PlayerFarming __instance)
    {
        if (Plugin.EnableDodgeSpeedMulti.Value)
        {
            __instance.playerController.DodgeSpeed = Plugin.DodgeSpeed.Value *= Mathf.Abs(Plugin.DodgeSpeedMulti.Value);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerWeapon), nameof(PlayerWeapon.DoAttackRoutine))]
    public static void PlayerWeapon_DoAttackRoutine(ref PlayerWeapon __instance)
    {
        if (Plugin.EnableDodgeSpeedMulti.Value)
        {
            __instance.playerController.DodgeSpeed = Plugin.DodgeSpeed.Value *= Mathf.Abs(Plugin.DodgeSpeedMulti.Value);
        }
    }
}