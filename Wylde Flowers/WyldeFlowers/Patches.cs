using HarmonyLib;

namespace WyldeFlowers;

[Harmony]
public static class Patches
{
    private const float RunSpeed = 7f;
    private const string Player = "Player";
    private const string FrontendFarm = "frontend_farm";
    private static SaveManager SaveManager { get; set; }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Actor), nameof(Actor.DropItemToPlayer), typeof(ItemType), typeof(int), typeof(bool))]
    private static bool Actor_DropItemToPlayer(ItemType itemType, int count, ref bool allowDropOnSelf)
    {
        if (!Plugin.DropStraightToInventory.Value) return true;
        if (Plugin.PlayerStateInstance == null) return true;
        Plugin.PlayerStateInstance.GiveItem(itemType, count);
        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SplashVideo), nameof(SplashVideo.Start))]
    private static void SplashVideo_Start()
    {
        if (Plugin.SkipLogos.Value)
        {
            SplashVideo.Skip();
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.Awake))]
    private static void SaveManager_Awake(ref SaveManager __instance)
    {
        SaveManager = __instance;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CutscenePlayer), nameof(CutscenePlayer.Begin))]
    private static void CutscenePlayer_Begin(ref CutscenePlayer __instance)
    {
        if (!__instance.cutscene.name.Equals(FrontendFarm)) return;
        var saveGameSlot = SaveManager._continueSlot;
        var saveGame = saveGameSlot.localFile.saveGame;
        Shell.instance.Load(saveGameSlot, saveGame, GameContext.InGame);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerState), nameof(PlayerState.CreateSnapshot))]
    [HarmonyPatch(typeof(PlayerState), nameof(PlayerState.Restore))]
    [HarmonyPatch(typeof(PlayerState), nameof(PlayerState.OnPlayerStateChanged))]
    private static void PlayerState_Start(ref PlayerState __instance)
    {
        Plugin.PlayerStateInstance = __instance;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CharacterLocomotion), nameof(CharacterLocomotion.OnUpdate))]
    private static void CharacterLocomotion_OnUpdate(CharacterLocomotion __instance)
    {
        if (!__instance.actor.name.Equals(Player)) return;
        var speedMultiplier = 1 + Plugin.RunSpeedPercentIncrease.Value / 100f;
        var newSpeed = RunSpeed * speedMultiplier;
        __instance.actor.movement.runSpeed = newSpeed;
        __instance.animations.runSpeed = Plugin.AlsoAdjustRunAnimationSpeed.Value ? newSpeed : RunSpeed;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(InteractionTrigger), nameof(InteractionTrigger.OnTriggerExit))]
    [HarmonyPatch(typeof(InteractionTrigger), nameof(InteractionTrigger.OnTriggerEnter))]
    [HarmonyPatch(typeof(InteractionTrigger), nameof(InteractionTrigger.Refresh))]
    private static void InteractionTrigger_OnTriggerExit(ref InteractionTrigger __instance)
    {
        Plugin.PlayerStateInstance = __instance.actor.player;
    }
}