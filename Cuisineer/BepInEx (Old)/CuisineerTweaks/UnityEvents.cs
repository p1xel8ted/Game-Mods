// namespace CuisineerTweaks;
//
// public class UnityEvents : MonoBehaviour
// {
//     private void Awake()
//     {
//         Plugin.Logger.LogInfo("UnityEvents Awake");
//     }
//
//     private void Update()
//     {
//         if (Input.GetKeyUp(Plugin.KeybindReload.Value))
//         {
//             Plugin.Instance.Config.Reload();
//         }
//
//         // Handle DebugK key action
//         if (CuisineerSaveManager.m_Instance != null && Input.GetKeyUp(Plugin.KeybindSaveGame.Value))
//         {
//             CuisineerSaveManager.SaveCurrent();
//             Plugin.Logger.LogInfo("Saved current game.");
//         }
//         
//         // Handle zoom level adjustment
//         if (Plugin.AdjustableZoomLevel.Value && GameInstances.PlayerRuntimeDataInstance != null)
//         {
//             var change = 0f;
//             if (Input.GetKeyUp(Plugin.KeybindZoomIn.Value))
//             {
//                 change = -0.1f;
//             }
//             else if (Input.GetKeyUp(Plugin.KeybindZoomOut.Value))
//             {
//                 change = 0.1f;
//             }
//         
//             if (change != 0f)
//             {
//                 var newValue = Mathf.Round((Plugin.UseStaticZoomLevel.Value ? Plugin.StaticZoomAdjustment.Value : Plugin.RelativeZoomAdjustment.Value + change) * 10f) / 10f;
//                 var newZoom = Fixes.GetNewZoomValue(newValue);
//         
//                 if (newZoom > 0.5f)
//                 {
//                     if (Plugin.UseStaticZoomLevel.Value)
//                     {
//                         Plugin.StaticZoomAdjustment.Value = newValue;
//                     }
//                     else
//                     {
//                         Plugin.RelativeZoomAdjustment.Value = newValue;
//                     }
//                     Fixes.UpdateCameraZoom();
//                 }
//             }
//         }
//         
//         // Handle time pause when viewing inventories
//         if (TimeManager.m_Instance == null) return;
//         var shouldPause = Plugin.PauseTimeWhenViewingInventories.Value && UI_InventoryViewBase.AnyInventoryActive;
//         TimeManager.ToggleTimePause(shouldPause);
//     }
//
// }