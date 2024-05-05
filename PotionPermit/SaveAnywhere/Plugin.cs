using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using NotificationEnum;
using UnityEngine;

namespace SaveAnywhere
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.potionpermit.saveanywhere";
        private const string PluginName = "Potion Permit SaveAnywhere";
        private const string PluginVersion = "0.1.2";

        private static readonly Harmony Harmony = new(PluginGuid);
        private static ManualLogSource _logger;

        private void Awake()
        {
            _logger = Logger;
        }

        private void OnEnable()
        {
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
            L($"Plugin {PluginName} is loaded!");
        }

        private void OnDisable()
        {
            Harmony.UnpatchSelf();
            L($"Plugin {PluginName} is unloaded!");
        }

        private static void L(string message)
        {
            _logger.LogWarning(message);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SaveSystemManager.SAVE();
                var comp = UIManager.NOTIFICATION_UI_MANAGER;
               // var comp = FindObjectOfType<ChemistNotificationUIManager>();

                var getInactiveEtcNotificationUI = comp.GetInactiveEtcNotificationUI;
                getInactiveEtcNotificationUI.Set(NotificationID.RESEARCH, comp.GetNotificationLayer);
                getInactiveEtcNotificationUI.notificationText.text = "Game Saved!";
                getInactiveEtcNotificationUI.newText.text = "Done.";
                getInactiveEtcNotificationUI.Call();
                comp.notificationOnQueueList.Add(getInactiveEtcNotificationUI);
            }
        }
    }
}