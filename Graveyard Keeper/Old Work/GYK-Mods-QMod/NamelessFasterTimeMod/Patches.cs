using HarmonyLib;
using Helper;
using UnityEngine;
using NamelessFasterTimeMod.lang;

namespace NamelessFasterTimeMod;

[HarmonyPatch]
public static partial class MainPatcher
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(WaitingGUI), nameof(WaitingGUI.Update))]
    public static void WaitingGUI_Update(ref WaitingGUI __instance)
    {
        if (__instance._state == 0)
        {
            Time.timeScale = 10f;
            Time.fixedDeltaTime = 0.0833333358f;
        }
    }


    //hooks into the time of day update and saves if the K key was pressed
    [HarmonyPrefix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.Update))]
    public static void TimeOfDay_Update()
    {
        if (Input.GetKeyUp(_cfg.ReloadConfigKeyBind))
        {
            _cfg = Config.GetOptions();

            if (Time.time - CrossModFields.ConfigReloadTime > CrossModFields.ConfigReloadTimeLength)
            {
                Tools.ShowMessage(GetLocalizedString(strings.ConfigMessage), Vector3.zero);
                CrossModFields.ConfigReloadTime = Time.time;
            }
        }
    }
}