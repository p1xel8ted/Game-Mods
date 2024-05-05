using System.Diagnostics.CodeAnalysis;
using CharacterChemist.Animation;
using GlobalEnum;
using HarmonyLib;

namespace AnAlchemicalCollection;

[Harmony]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class CharacterBobPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(CharacterAnimationBase), nameof(CharacterAnimationBase.Play))]
    public static void CharacterAnimationBase_Play(ref CharacterAnimationBase __instance, ref float durationAnimation,
        ref int totalFrame)
    {
        if (Plugin.CharacterBob.Value) return;
        if (__instance.selectedAnimation != TypeAnimation.Idle) return;
        durationAnimation = 0;
        totalFrame = 0;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CharacterAnimationNPC), nameof(CharacterAnimationNPC.Play))]
    public static void CharacterAnimationNPC_Play(ref CharacterAnimationNPC __instance, ref float durationAnimation,
        ref int totalFrame)
    {
        if (Plugin.CharacterBob.Value) return;
        if (__instance.selectedAnimation != TypeAnimation.Idle) return;
        durationAnimation = 0;
        totalFrame = 0;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CharacterAnimationPlayer), nameof(CharacterAnimationPlayer.Play))]
    public static void CharacterAnimationPlayer_Play(ref CharacterAnimationPlayer __instance,
        ref float durationAnimation, ref int totalFrame)
    {
        if (Plugin.CharacterBob.Value) return;
        if (__instance.selectedAnimation != TypeAnimation.Idle) return;
        durationAnimation = 0;
        totalFrame = 0;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CharacterAnimationDoggy), nameof(CharacterAnimationDoggy.Play))]
    public static void CharacterAnimationDoggy_Play(ref CharacterAnimationDoggy __instance)
    {
        if (Plugin.CharacterBob.Value) return;
        if (__instance.currentAnimation == TypeAnimation.Dogie_Idle)
        {
            __instance.currentAnimation = TypeAnimation.Dogie_SitIdle;
        }
    }
}