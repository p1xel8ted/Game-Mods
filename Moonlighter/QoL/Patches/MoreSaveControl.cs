// using System;
// using HarmonyLib;
// using UnityEngine;
//
// namespace QoL.Patches
// {
//     [Harmony]
//     public static class MoreSaveControl
//     {
//         // MonoBehaviour for Unity callbacks
//         public class MoreSaveControlMB : MonoBehaviour
//         {
//             private void Awake()
//             {
//                 Plugin.LOG.LogWarning("MoreSaveControl MonoBehaviour Awake");
//             }
//
//             private void Update()
//             {
//                 // Check the conditions for showing the save confirmation pop-up
//                 if (ShouldShowSavePopUp())
//                 {
//                     GameManager.Instance.CanSave = false;
//                     Override = true;
//                     string message = FormatMessage(DigitalSunGames.Languages.I2.Localization.Get("GUI/CONFIRM_EXIT"));
//                     InfoPopUp.Show(message, result => HandlePopUpResult);
//                 }
//                 try
//                 {
//                     // Placeholder for additional logic
//                 }
//                 catch (Exception ex)
//                 {
//                     Plugin.LOG.LogError(ex);
//                 }
//             }
//
//             private bool ShouldShowSavePopUp()
//             {
//                 return GUIManager.Instance 
//                        && GUIManager.Instance.input.ButtonStart.WasPressed 
//                        && GUIManager.Instance.resultsPanel 
//                        && GUIManager.Instance.resultsPanel._isActive 
//                        && !InfoPopUp._instance._isActive;
//             }
//
//             private string FormatMessage(string text)
//             {
//                 return string.Join("\n", text.Replace(".", ".\n").SplitLines(35));
//             }
//
//             private void HandlePopUpResult(bool confirmed)
//             {
//                 if (confirmed)
//                 {
//                     GameManager.Instance.UnpauseGame();
//                     GameManager.Instance.ReturnToMainMenu();
//                 }
//                 GameManager.Instance.CanSave = true;
//                 Override = false;
//             }
//         }
//
//         // Flag to indicate override behavior
//         public static bool Override { get; set; }
//
//         // Harmony patches to override certain game functions based on the Override flag
//         [HarmonyPrefix]
//         [HarmonyPatch(typeof(DungeonResultsPanel), nameof(DungeonResultsPanel.OnAButtonReleased))]
//         private static bool PrefixOnAButtonReleased(ref bool __result)
//         {
//             __result = false;
//             return !Override;
//         }
//
//         [HarmonyPrefix]
//         [HarmonyPatch(typeof(DungeonResultsPanel), nameof(DungeonResultsPanel.OnBButtonPressed))]
//         private static bool PrefixOnBButtonPressed(ref bool __result)
//         {
//             return PrefixOnAButtonReleased(ref __result);
//         }
//
//         [HarmonyPrefix]
//         [HarmonyPatch(typeof(PausePanel), nameof(PausePanel.ExitGame))]
//         private static bool PrefixExitGame(PausePanel __instance)
//         {
//             return ConfirmSave(GameManager.Instance.ExitGame, __instance.ExitGame);
//         }
//
//         [HarmonyPrefix]
//         [HarmonyPatch(typeof(PausePanel), nameof(PausePanel.GoToMenu))]
//         private static bool PrefixGoToMenu(PausePanel __instance)
//         {
//             return ConfirmSave(() =>
//             {
//                 GameManager.Instance.UnpauseGame();
//                 GameManager.Instance.ReturnToMainMenu();
//             }, __instance.GoToMenu);
//         }
//
//         // Helper to show save confirmation based on game state
//         private static bool ConfirmSave(Action confirmAction, Action cancelAction)
//         {
//             if (ShouldPromptSave())
//             {
//                 string message = FormatMessage("Do you want to save before you exit?");
//                 InfoPopUp.Show(message, result => HandleSavePopUpResult(result, confirmAction, cancelAction));
//                 return false;
//             }
//             return true;
//         }
//
//         private static bool ShouldPromptSave()
//         {
//             return !firstCall && (GameManager.Instance.IsWillInDungeon() || ShopManager.Instance.IsShopOpen());
//         }
//
//         private static string FormatMessage(string text)
//         {
//             return string.Join("\n", text.Replace(".", ".\n").SplitLines(35));
//         }
//
//         private static void HandleSavePopUpResult(bool saveConfirmed, Action confirmAction, Action cancelAction)
//         {
//             if (saveConfirmed)
//             {
//                 GameManager.Instance.SaveSlotOnGoToDungeon(true);
//                 confirmAction();
//             }
//             else
//             {
//                 firstCall = false;
//                 cancelAction();
//             }
//         }
//
//         private static bool firstCall = true;
//     }
// }