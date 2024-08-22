// namespace CultOfQoL.Patches;
//
// [HarmonyPatch]
// public static class Scales
// {
//
//     private readonly static List<CanvasScaler> Scalers = [];
//     
//
//     [HarmonyPostfix]
//     [HarmonyPatch(typeof(CanvasScaler), nameof(CanvasScaler.OnEnable))]
//     public static void CanvasScaler_OnEnable(ref CanvasScaler __instance)
//     {
//         Scalers.Add(__instance);
//         Plugin.Log.LogWarning($"Found CanvasScaler: {__instance.name} - {__instance.gameObject.GetPath()}");
//         ChangeScale(__instance);
//     }
//
//     private static void ChangeScale(CanvasScaler scaler)
//     {
//         if (Plugin.EnableCustomUiScale.Value)
//         {
//             scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
//             scaler.scaleFactor = Plugin.CustomUiScale.Value;
//         }
//         else
//         {
//             scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
//         } 
//     }
//     
//     internal static void ChangeAllScalers()
//     {
//         foreach (var scaler in Scalers)
//         {
//             ChangeScale(scaler);
//         }
//     }
// }