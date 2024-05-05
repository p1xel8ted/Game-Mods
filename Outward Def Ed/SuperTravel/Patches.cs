using HarmonyLib;

namespace SuperTravel;

[Harmony]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(CharacterStats), nameof(CharacterStats.MovementSpeed), MethodType.Getter)]
    private static void CharacterStats_MovementSpeed(ref CharacterStats __instance, ref float __result)
    {
        var character = __instance.m_character;
        var flag = !character.IsAI && !character.IsEnemyClose(15f);
        if (flag)
        {
            __result += Plugin.SpeedIncrease.Value;
            character.Stats.GetStat(CharacterStats.StatType.StaminaCostReduction).AddMultiplierStack("staminaOutOfCombat", -0.7f);
        }
        else
        {
            character.Stats.GetStat(CharacterStats.StatType.StaminaCostReduction).RemoveMultiplierStack("staminaOutOfCombat");
        }
    }
}