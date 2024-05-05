using System.Diagnostics.CodeAnalysis;
using AudioEnum;
using CharacterChemist.Animation;
using GlobalEnum;
using HarmonyLib;

namespace AnAlchemicalCollection;

[Harmony]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class SoundPatches
{
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(CharacterAnimationDoggy), nameof(CharacterAnimationDoggy.PlayAnimation))]
    public static void CharacterAnimationDoggy_PlayAnimation(ref TypeAnimation selectAnimation)
    {
        if (!Plugin.DogBarkingSound.Value && selectAnimation == TypeAnimation.Dogie_Bark)
        {
            selectAnimation = TypeAnimation.Dogie_SitIdle;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Chemist_SFX), nameof(Chemist_SFX.PLAY_SFX_ONE_SHOT_3D))]
    public static void Chemist_SFX_PLAY_SFX_ONE_SHOT_3D(ref SFXID id)
    {
        if (!Plugin.DogBarkingSound.Value && id == SFXID.SFX_Character_Dog_Bark)
        {
            id = SFXID.NONE;
        }
        
        if (!Plugin.DockStompingSoundWhenFishing.Value && id == SFXID.SFX_Object_Water_SplashHeavy)
        {
            id = SFXID.SFX_Object_Water_SplashLight;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Chemist_SFX), nameof(Chemist_SFX.PLAY_SFX_UI))]
    public static void Chemist_SFX_Set(ref SFXID id)
    {
        if (!Plugin.DialogueTypeWriterSound.Value && id == SFXID.SFX_UI_Dialogue_Medium)
        {
            id = SFXID.NONE;
        }
    }
}