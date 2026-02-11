namespace CultOfQoL.Patches.Gameplay;

[Harmony]
[SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
public static class InteractionPatches
{
    /// <summary>
    /// Interactions that game code triggers programmatically during menus/rewards.
    /// These call OnInteract directly while menus are open (e.g., job rewards, rituals, doctrines).
    /// </summary>
    private static readonly HashSet<Type> ProgrammaticInteractionTypes =
    [
        typeof(LoreStone),
        typeof(Interaction_TempleAltar),
        typeof(Interaction_DoctrineStone),
        typeof(Interaction_SimpleConversation)
    ];

    /// <summary>
    /// Prevents ALL interactions when menus are open.
    /// Fixes bug where pressing A on a menu also triggers world interactions with nearby objects.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Interaction), nameof(Interaction.OnInteract), typeof(StateMachine))]
    public static bool Interaction_OnInteract(ref Interaction __instance)
    {
        if (ProgrammaticInteractionTypes.Contains(__instance.GetType()))
        {
            return true;
        }

        if (UIMenuBase.ActiveMenus.Count == 0 && !GameManager.InMenu)
        {
            return true;
        }

        Plugin.WriteLog($"[Interaction] Blocking interaction with {__instance.GetType().Name} - {UIMenuBase.ActiveMenus.Count} menu(s) open or InMenu={GameManager.InMenu}");
        return false;
    }
}
