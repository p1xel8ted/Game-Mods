using System;
using HarmonyLib;
using Helper;
using TreesNoMore.lang;
using UnityEngine;

namespace TreesNoMore;

[HarmonyPatch]
public static partial class MainPatcher
{
    //hooks into the time of day update and saves if the K key was pressed
    [HarmonyPrefix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.Update))]
    public static void TimeOfDay_Update()
    {
        if (Input.GetKeyUp(_cfg.reloadConfigKeyBind))
        {
            _cfg = Config.GetOptions();

            if (Time.time - CrossModFields.ConfigReloadTime > CrossModFields.ConfigReloadTimeLength)
            {
                Tools.ShowMessage(GetLocalizedString(strings.ConfigMessage), Vector3.zero);
                CrossModFields.ConfigReloadTime = Time.time;
            }
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.InitNewObject))]
    private static void WorldGameObject_InitNewObject(ref WorldGameObject __instance)
    {
        if (!_cfg.enable) return;
        if (__instance == null) return;
        if (__instance.obj_id.Contains("stump"))
        {
            UnityEngine.Object.Destroy(__instance.gameObject);
        }
    }

    [HarmonyFinalizer]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.InitNewObject))]
    public static Exception WorldGameObject_InitNewObject_Finalizer()
    {
        return null;
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.SmartInstantiate))]
    public static void WorldGameObject_SmartInstantiate(ref WorldObjectPart prefab)
    {
        if (!_cfg.enable) return;
        if (prefab == null) return;
        if ((!MainGame.game_started && !MainGame.game_starting) || !prefab.name.Contains("tree") || prefab.name.Contains("bees")) return;
        if (prefab.name.Contains("apple")) return;
        prefab = null;
    }

    [HarmonyFinalizer]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.SmartInstantiate))]
    public static Exception WorldGameObject_SmartInstantiate_Finalizer()
    {
        
        return null;
    }
}