namespace CultOfQoL.Core.Routines;

// NOTE: This entire file is commented out - the transpilers are no longer needed.
//
// With the direct effects approach (MassActionEffects), only the original follower runs
// the vanilla routine. Other followers get instant effects without invoking routines.
// This eliminates the need to NOP out HUD/camera/conversation calls that would otherwise
// conflict when multiple routines ran simultaneously.
//
// Kept for reference in case the routine-based approach is ever needed again.

// [Harmony]
// public static class RoutinesTranspilers
// {
//     private const string IntimidateRoutine = "IntimidateRoutine";
//     private const string BribeRoutine = "BribeRoutine";
//     private const string BullyRoutine = "BullyRoutine";
//     private const string ReassureRoutine = "ReassureRoutine";
//     private const string ReeducateRoutine = "ReeducateRoutine";
//     private const string BlessRoutine = "BlessRoutine";
//     private const string DanceRoutine = "DanceRoutine";
//     private const string PetDogRoutine = "PetDogRoutine";
//     private const string RomanceRoutine = "RomanceRoutine";
//     private const string ExtortMoneyRoutine = "ExtortMoneyRoutine";
//     private const string LevelUpRoutine = "LevelUpRoutine";
//
//     private static readonly MethodInfo GetInstance = AccessTools.Method(typeof(GameManager), nameof(GameManager.GetInstance));
//     private static readonly MethodInfo OnConversationNext = AccessTools.Method(typeof(GameManager), nameof(GameManager.OnConversationNext));
//     private static readonly MethodInfo OnConversationNew = AccessTools.Method(typeof(GameManager), nameof(GameManager.OnConversationNew), [typeof(bool), typeof(bool), typeof(PlayerFarming)]);
//     private static readonly MethodInfo AddPlayerToCamera = AccessTools.Method(typeof(GameManager), nameof(GameManager.AddPlayerToCamera));
//     private static readonly MethodInfo CameraSetOffset = AccessTools.Method(typeof(GameManager), nameof(GameManager.CameraSetOffset));
//     private static readonly MethodInfo HudManagerHide = AccessTools.Method(typeof(HUD_Manager), nameof(HUD_Manager.Hide));
//     private static readonly FieldInfo HudManagerInstance = AccessTools.Field(typeof(HUD_Manager), nameof(HUD_Manager.Instance));
//     private static readonly FieldInfo BiomeConstantsInstance = AccessTools.Field(typeof(BiomeConstants), nameof(BiomeConstants.Instance));
//     private static readonly MethodInfo DepthOfFieldTween = AccessTools.Method(typeof(BiomeConstants), nameof(BiomeConstants.DepthOfFieldTween));
//     private static readonly HashSet<string> AlreadyLogged = [];
//
//     internal static bool AnyMassActionsEnabled => RoutineChecks.Any(pair => pair.Value.Invoke().Value);
//
//     private static readonly Dictionary<string, Func<ConfigEntry<bool>>> RoutineChecks = new()
//     {
//         [BribeRoutine] = () => Plugin.MassBribe,
//         [IntimidateRoutine] = () => Plugin.MassIntimidate,
//         [BullyRoutine] = () => Plugin.MassBully,
//         [ReassureRoutine] = () => Plugin.MassReassure,
//         [ReeducateRoutine] = () => Plugin.MassReeducate,
//         [BlessRoutine] = () => Plugin.MassBless,
//         [DanceRoutine] = () => Plugin.MassInspire,
//         [PetDogRoutine] = () => Plugin.MassPetFollower,
//         [RomanceRoutine] = () => Plugin.MassRomance,
//         [ExtortMoneyRoutine] = () => Plugin.MassExtort,
//         [LevelUpRoutine] = () => Plugin.MassLevelUp
//     };
//
//     [HarmonyTranspiler]
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.OnInteract))]
//     private static IEnumerable<CodeInstruction> interaction_FollowerInteraction_OnInteract(IEnumerable<CodeInstruction> instructions, MethodBase original)
//     {
//         var originalCodes = instructions.ToList();
//         if (!AnyMassActionsEnabled) return originalCodes;
//
//         try
//         {
//             var codes = new List<CodeInstruction>(originalCodes);
//             LogOnce(original, "Removing DepthOfFieldTween.");
//             NopCallSequence(codes, c => c.LoadsField(BiomeConstantsInstance), DepthOfFieldTween);
//             return codes;
//         }
//         catch (Exception ex)
//         {
//             Plugin.Log.LogWarning($"[Transpiler] {GetRoutineName(original)}: {ex.Message}");
//             return originalCodes;
//         }
//     }
//
//     [HarmonyTranspiler]
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.BribeRoutine), MethodType.Enumerator)]
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.IntimidateRoutine), MethodType.Enumerator)]
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.BullyRoutine), MethodType.Enumerator)]
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.ReassureRoutine), MethodType.Enumerator)]
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.ReeducateRoutine), MethodType.Enumerator)]
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.BlessRoutine), MethodType.Enumerator)]
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.DanceRoutine), MethodType.Enumerator)]
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.PetDogRoutine), MethodType.Enumerator)]
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.RomanceRoutine), MethodType.Enumerator)]
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.ExtortMoneyRoutine), MethodType.Enumerator)]
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.LevelUpRoutine), MethodType.Enumerator)]
//     private static IEnumerable<CodeInstruction> interaction_FollowerInteraction_Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase original)
//     {
//         var originalCodes = instructions.ToList();
//
//         if (original.DeclaringType != null)
//         {
//             var declaringType = original.DeclaringType.ToString();
//             if (RoutineChecks.Any(pair => declaringType.Contains(pair.Key, StringComparison.OrdinalIgnoreCase) && !pair.Value.Invoke().Value))
//             {
//                 return originalCodes;
//             }
//         }
//
//         try
//         {
//             var codes = new List<CodeInstruction>(originalCodes);
//             LogOnce(original, "Removing HUD/conversation/camera calls.");
//
//             NopCallSequence(codes, c => c.LoadsField(HudManagerInstance), HudManagerHide);
//             NopCallSequence(codes, c => c.Calls(GetInstance), OnConversationNew);
//             NopCallSequence(codes, c => c.Calls(GetInstance), OnConversationNext);
//             NopCallSequence(codes, c => c.Calls(GetInstance), AddPlayerToCamera);
//             NopCallSequence(codes, c => c.Calls(GetInstance), CameraSetOffset);
//
//             return codes;
//         }
//         catch (Exception ex)
//         {
//             Plugin.Log.LogWarning($"[Transpiler] {GetRoutineName(original)}: {ex.Message}");
//             return originalCodes;
//         }
//     }
//
//     [HarmonyPrefix]
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.LevelUpRoutine))]
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.ExtortMoneyRoutine))]
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.IntimidateRoutine))]
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.BlessRoutine))]
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.ReeducateRoutine))]
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.RomanceRoutine))]
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.PetDogRoutine))]
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.ReassureRoutine))]
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.BullyRoutine))]
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.DanceRoutine))]
//     [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.BribeRoutine))]
//     private static void EnsurePlayerFarming(interaction_FollowerInteraction __instance, MethodBase __originalMethod)
//     {
//         if (!RoutineChecks.TryGetValue(__originalMethod.Name, out var configGetter) || !configGetter().Value) return;
//
//         if (!__instance.playerFarming)
//         {
//             __instance.playerFarming = PlayerFarming.Instance ??= Object.FindObjectOfType<PlayerFarming>();
//         }
//     }
//
//     private static string GetRoutineName(MethodBase original)
//     {
//         var declaringType = original.DeclaringType?.ToString() ?? "Unknown";
//         var startIndex = declaringType.IndexOf('<');
//         var endIndex = declaringType.IndexOf('>');
//         if (startIndex >= 0 && endIndex > startIndex)
//         {
//             return declaringType.Substring(startIndex + 1, endIndex - startIndex - 1);
//         }
//
//         return declaringType;
//     }
//
//     private static void LogOnce(MethodBase original, string description)
//     {
//         var key = $"{original.DeclaringType}.{original.Name}";
//         if (!AlreadyLogged.Add(key)) return;
//
//         var routineName = GetRoutineName(original);
//         var featureName = RoutineChecks.TryGetValue(routineName, out var configGetter)
//             ? configGetter().Definition.Key
//             : "Unknown";
//
//         Plugin.Log.LogInfo($"[Transpiler] {routineName} ({featureName}): {description}");
//     }
//
//     private static void NopCallSequence(List<CodeInstruction> codes, Func<CodeInstruction, bool> anchorTest, MethodInfo targetCall, int maxWindow = 12)
//     {
//         for (var i = 0; i < codes.Count; i++)
//         {
//             if (!codes[i].Calls(targetCall)) continue;
//
//             var anchorFound = false;
//             for (var j = i - 1; j >= Math.Max(0, i - maxWindow); j--)
//             {
//                 if (!anchorTest(codes[j])) continue;
//
//                 for (var k = j; k <= i; k++)
//                 {
//                     codes[k].opcode = OpCodes.Nop;
//                     codes[k].operand = null;
//                 }
//
//                 anchorFound = true;
//                 break;
//             }
//
//             if (!anchorFound)
//             {
//                 Plugin.Log.LogWarning($"[Transpiler] NopCallSequence: Failed to find anchor for {targetCall.Name} at instruction {i}.");
//             }
//         }
//     }
// }
