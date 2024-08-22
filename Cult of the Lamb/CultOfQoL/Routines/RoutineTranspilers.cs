namespace CultOfQoL.Routines;

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
    private const string LevelUpRoutine = "LevelUpRoutine";
    private const string RomanceRoutine = "RomanceRoutine";
    private const string ExtortMoneyRoutine = "ExtortMoneyRoutine";
    private readonly static MethodInfo GetInstance = AccessTools.Method(typeof(GameManager), nameof(GameManager.GetInstance));
    private readonly static MethodInfo OnConversationNext = AccessTools.Method(typeof(GameManager), nameof(GameManager.OnConversationNext));
    private readonly static MethodInfo OnConversationNew = AccessTools.Method(typeof(GameManager), nameof(GameManager.OnConversationNew), [typeof(bool), typeof(bool), typeof(PlayerFarming)]);
    private readonly static MethodInfo AddPlayerToCamera = AccessTools.Method(typeof(GameManager), nameof(GameManager.AddPlayerToCamera));
    private readonly static MethodInfo CameraSetOffset = AccessTools.Method(typeof(GameManager), nameof(GameManager.CameraSetOffset));
    private readonly static MethodInfo HudManagerHide = AccessTools.Method(typeof(HUD_Manager), nameof(HUD_Manager.Hide));
    private readonly static FieldInfo HudManagerInstance = AccessTools.Field(typeof(HUD_Manager), nameof(HUD_Manager.Instance));
    private readonly static FieldInfo PlayerInstance = AccessTools.Field(typeof(PlayerFarming), nameof(PlayerFarming.Instance));
    private readonly static MethodInfo setStateMachine = AccessTools.Property(typeof(StateMachine), nameof(StateMachine.CURRENT_STATE)).SetMethod;
    private readonly static MethodInfo simpleAnimatorAnimate = AccessTools.Method(typeof(SimpleSpineAnimator), nameof(SimpleSpineAnimator.Animate), [typeof(string), typeof(int), typeof(bool)]);
    private readonly static FieldInfo BiomeConstantsInstance = AccessTools.Field(typeof(BiomeConstants), nameof(BiomeConstants.Instance));
    private readonly static MethodInfo DepthOfFieldTween = AccessTools.Method(typeof(BiomeConstants), nameof(BiomeConstants.DepthOfFieldTween));
    private readonly static List<string> AlreadyLogged = [];

    private readonly static Dictionary<string, Func<ConfigEntry<bool>>> routineChecks = new()
    {
        [BribeRoutine] = () => Plugin.MassBribe,
        [IntimidateRoutine] = () => Plugin.MassIntimidate,
        [BullyRoutine] = () => Plugin.MassBully,
        [ReassureRoutine] = () => Plugin.MassReassure,
        [ReeducateRoutine] = () => Plugin.MassReeducate,
        [BlessRoutine] = () => Plugin.MassBless,
        [DanceRoutine] = () => Plugin.MassInspire,
        [PetDogRoutine] = () => Plugin.MassPetDog,
        [LevelUpRoutine] = () => Plugin.MassLevelUp,
        [RomanceRoutine] = () => Plugin.MassRomance,
        [ExtortMoneyRoutine] = () => Plugin.MassExtort,
        ["GivePoem"] = () => Plugin.MassBribe
    };

    private static bool RunThisTranspiler => routineChecks.Any(pair => pair.Value.Invoke().Value);

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
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.LevelUpRoutine), MethodType.Enumerator)]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.RomanceRoutine), MethodType.Enumerator)]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.ExtortMoneyRoutine), MethodType.Enumerator)]
    private static IEnumerable<CodeInstruction> interaction_FollowerInteraction_Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase original)
    {
        if (original.DeclaringType != null)
        {
            var declaringType = original.DeclaringType.ToString();

            var routineCheckResults = routineChecks
                .Where(pair => declaringType.Contains(pair.Key, StringComparison.OrdinalIgnoreCase) && !pair.Value.Invoke().Value)
                .ToList();

            foreach (var pair in routineCheckResults)
            {
                Plugin.Log.LogWarning($"Not patching {declaringType}:{original.Name} as {pair.Value.Invoke().Definition.Key} is false!");
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