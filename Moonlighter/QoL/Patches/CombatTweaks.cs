using System;
using HarmonyLib;

namespace QoL.Patches;

[Harmony]
public static class CombatTweaks
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(HeroMerchantController), nameof(HeroMerchantController.SecondaryAttackUseStart))]
    private static void HeroMerchantController_SecondaryAttackUseStart(ref HeroMerchantController __instance)
    {
        var run = __instance.currentEquippedWeapon.weaponMaster.weaponType == WeaponEquipmentMaster.WeaponType.ShortSword;
        if (run)
        {
            __instance.currentEquippedWeapon.GetComponent<ShortSword>().ResetNumberOfShieldHits();
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(HeroMerchantController), nameof(HeroMerchantController.AddSpeedEffectModifier), typeof(float), typeof(float))]
    private static void HeroMerchantController_AddSpeedEffectModifier(ref float multiplier)
    {
        var run = Environment.StackTrace.Contains("at Dash");
        if (run)
        {
            Plugin.LOG.LogWarning("Dash detected, increasing speed effect modifier");
            multiplier *= 1.2f;
        }
        else
        {
            Plugin.LOG.LogWarning("No dash detected, not increasing speed effect modifier"); 
        }
    }
    
}