using System.Diagnostics.CodeAnalysis;
using GlobalEnum;
using HarmonyLib;

namespace AnAlchemicalCollection;

[HarmonyPatch]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class PlayerSpeedPatches
{
    private static float OriginalPlayerSpeed = 150f;
    private static float OriginalDogSpeed = 300f;

    // Store original player speed on Start
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerCharacter), nameof(PlayerCharacter.Start))]
    public static void StorePlayerOriginalSpeed() 
    {
        OriginalPlayerSpeed = PlayerCharacter.Instance.MoveSpeed;
        Plugin.L($"OriginalPlayerSpeed: {OriginalPlayerSpeed}",true);
    }

    // Update player run speed on Move
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerCharacter), nameof(PlayerCharacter.Move))]
    public static void PlayerCharacter_Move() 
    {
        if (Plugin.EnableRunSpeedMultiplier.Value)
        {
            UpdatePlayerSpeed();
        }
    }

    // Method to update player speed based on direction
    private static void UpdatePlayerSpeed()
    {
        if (PlayerCharacter.Instance.CurrentDirection is Direction.None) return;

        var speedMultiplier = 1.0f;

        switch (PlayerCharacter.Instance.CurrentDirection)
        {
            case Direction.Top:
            case Direction.Bottom:
                speedMultiplier = Plugin.RunSpeedMultiplier.Value;
                break;

            case Direction.Left:
            case Direction.Right:
                speedMultiplier = Plugin.RunSpeedMultiplier.Value * Plugin.LeftRightRunSpeedMultiplier.Value;
                break;
        }

        PlayerCharacter.Instance.MoveSpeed = OriginalPlayerSpeed * speedMultiplier;
    }

    // Store original dog speed on Start
    [HarmonyPostfix]
    [HarmonyPatch(typeof(DogieAI), nameof(DogieAI.Start))]
    public static void StoreDogOriginalSpeed(ref DogieAI __instance) 
    {
        OriginalDogSpeed= __instance.speed;
        Plugin.L($"OriginalDogSpeed: {OriginalDogSpeed}",true);
    }

    // Update dog speed on FIXED_UPDATE
    [HarmonyPostfix]
    [HarmonyPatch(typeof(DogieAI), nameof(DogieAI.FIXED_UPDATE))]
    public static void DogieAI_Patches(ref DogieAI __instance)
    {
        if (Plugin.EnableRunSpeedMultiplier.Value)
        {
            __instance.speed = PlayerCharacter.Instance.MoveSpeed * 0.75f;
        }
    }
}