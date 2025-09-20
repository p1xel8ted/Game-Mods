namespace CultOfQoL.Core.Routines;

[Harmony]
public static class RoutinesTranspilers
{
    private const string IntimidateRoutine = "IntimidateRoutine";
    private const string BribeRoutine = "BribeRoutine";
    private const string BullyRoutine = "BullyRoutine";
    private const string ReassureRoutine = "ReassureRoutine";
    private const string ReeducateRoutine = "ReeducateRoutine";
    private const string BlessRoutine = "BlessRoutine";
    private const string DanceRoutine = "DanceRoutine";
    private const string PetDogRoutine = "PetDogRoutine";
    // private const string LevelUpRoutine = "LevelUpRoutine";
    private const string RomanceRoutine = "RomanceRoutine";
    private const string ExtortMoneyRoutine = "ExtortMoneyRoutine";
    private static readonly MethodInfo GetInstance = AccessTools.Method(typeof(GameManager), nameof(GameManager.GetInstance));
    private static readonly MethodInfo OnConversationNext = AccessTools.Method(typeof(GameManager), nameof(GameManager.OnConversationNext));
    private static readonly MethodInfo OnConversationNew = AccessTools.Method(typeof(GameManager), nameof(GameManager.OnConversationNew), [typeof(bool), typeof(bool), typeof(PlayerFarming)]);
    private static readonly MethodInfo AddPlayerToCamera = AccessTools.Method(typeof(GameManager), nameof(GameManager.AddPlayerToCamera));
    private static readonly MethodInfo CameraSetOffset = AccessTools.Method(typeof(GameManager), nameof(GameManager.CameraSetOffset));
    private static readonly MethodInfo HudManagerHide = AccessTools.Method(typeof(HUD_Manager), nameof(HUD_Manager.Hide));
    private static readonly FieldInfo HudManagerInstance = AccessTools.Field(typeof(HUD_Manager), nameof(HUD_Manager.Instance));
    // private readonly static FieldInfo PlayerInstance = AccessTools.Field(typeof(PlayerFarming), nameof(PlayerFarming.Instance));
    // private readonly static MethodInfo setStateMachine = AccessTools.Property(typeof(StateMachine), nameof(StateMachine.CURRENT_STATE)).SetMethod;
    // private readonly static MethodInfo simpleAnimatorAnimate = AccessTools.Method(typeof(SimpleSpineAnimator), nameof(SimpleSpineAnimator.Animate), [typeof(string), typeof(int), typeof(bool)]);
    private static readonly FieldInfo BiomeConstantsInstance = AccessTools.Field(typeof(BiomeConstants), nameof(BiomeConstants.Instance));
    private static readonly MethodInfo DepthOfFieldTween = AccessTools.Method(typeof(BiomeConstants), nameof(BiomeConstants.DepthOfFieldTween));
    private static readonly List<string> AlreadyLogged = [];

    
    internal static bool AnyMassActionsEnabled => RoutineChecks.Any(pair => pair.Value.Invoke().Value);
    
    private static readonly Dictionary<string, Func<ConfigEntry<bool>>> RoutineChecks = new()
    {
        [BribeRoutine] = () => Plugin.MassBribe,
        [IntimidateRoutine] = () => Plugin.MassIntimidate,
        [BullyRoutine] = () => Plugin.MassBully,
        [ReassureRoutine] = () => Plugin.MassReassure,
        [ReeducateRoutine] = () => Plugin.MassReeducate,
        [BlessRoutine] = () => Plugin.MassBless,
        [DanceRoutine] = () => Plugin.MassInspire,
        [PetDogRoutine] = () => Plugin.MassPetDog,
        // [LevelUpRoutine] = () => Plugin.MassLevelUp,
        [RomanceRoutine] = () => Plugin.MassRomance,
        [ExtortMoneyRoutine] = () => Plugin.MassExtort
    };

    private static bool RunThisTranspiler => RoutineChecks.Any(pair => pair.Value.Invoke().Value);

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.OnInteract))]
    private static IEnumerable<CodeInstruction> interaction_FollowerInteraction_OnInteract(IEnumerable<CodeInstruction> instructions, MethodBase original)
    {
        if (!RunThisTranspiler) return instructions;

        if (original.DeclaringType != null)
        {
            var declaringType = original.DeclaringType.ToString();
            LogOnce(declaringType, $"Patching {declaringType}:{original.Name}");
        }


        var codes = new List<CodeInstruction>(instructions);
        for (var index = 0; index < codes.Count; index++)
        {
            TryReplaceWithNop(codes, index, codes[index].LoadsField(BiomeConstantsInstance) && codes[index + 6].Calls(DepthOfFieldTween), 7);
        }
        return codes.AsEnumerable();
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.BribeRoutine), MethodType.Enumerator)]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.IntimidateRoutine), MethodType.Enumerator)]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.BullyRoutine), MethodType.Enumerator)]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.ReassureRoutine), MethodType.Enumerator)]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.ReeducateRoutine), MethodType.Enumerator)]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.BlessRoutine), MethodType.Enumerator)]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.DanceRoutine), MethodType.Enumerator)]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.PetDogRoutine), MethodType.Enumerator)]
    // [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.LevelUpRoutine), MethodType.Enumerator)]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.RomanceRoutine), MethodType.Enumerator)]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.ExtortMoneyRoutine), MethodType.Enumerator)]
    private static IEnumerable<CodeInstruction> interaction_FollowerInteraction_Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase original)
    {
        if (original.DeclaringType != null)
        {
            var declaringType = original.DeclaringType.ToString();

            var routineCheckResults = RoutineChecks
                .Where(pair => declaringType.Contains(pair.Key, StringComparison.OrdinalIgnoreCase) && !pair.Value.Invoke().Value)
                .ToList();

            if (routineCheckResults.Any())
            {
                return instructions;
            }

            LogOnce(declaringType, $"Patching {declaringType}:{original.Name}");
        }


        var codes = new List<CodeInstruction>(instructions);

        for (var index = 0; index < codes.Count; index++)
        {
            // if (!declaringType.Contains(IntimidateRoutine, StringComparison.OrdinalIgnoreCase))
            // {
            //     TryReplaceWithNop(codes, index, codes[index].LoadsField(PlayerInstance) && codes[index + 3].Calls(setStateMachine), 4);
            //     TryReplaceWithNop(codes, index, codes[index].LoadsField(PlayerInstance) && codes[index + 5].Calls(simpleAnimatorAnimate), 14);
            // }
            TryReplaceWithNop(codes, index, codes[index].LoadsField(HudManagerInstance) && codes[index + 4].Calls(HudManagerHide), 5);
            TryReplaceWithNop(codes, index, codes[index].Calls(GetInstance) && codes[index + 3].Calls(OnConversationNew) && codes[index + 4].Calls(GetInstance) && codes[index + 8].Calls(OnConversationNext), 9);
            TryReplaceWithNop(codes, index, codes[index].Calls(GetInstance) && codes[index + 4].Calls(OnConversationNext) && codes[index + 6].Calls(AddPlayerToCamera) && codes[index + 9].Calls(CameraSetOffset), 10);
        }
        return codes.AsEnumerable();
    }

    private static void LogOnce(string declaringType, string message)
    {
        if (AlreadyLogged.Contains(declaringType)) return;
        AlreadyLogged.Add(declaringType);
        Plugin.L($"[Mass Actions] -> {message}");
    }

    private static void TryReplaceWithNop(IReadOnlyList<CodeInstruction> codes, int index, bool condition, int count)
    {
        if (!condition) return;

        for (var j = 0; j < count; j++)
        {
            codes[index + j].opcode = OpCodes.Nop;
        }
    }

}