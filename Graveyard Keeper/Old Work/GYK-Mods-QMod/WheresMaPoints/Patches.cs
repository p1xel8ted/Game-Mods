using System;
using DarkTonic.MasterAudio;
using HarmonyLib;
using Helper;
using UnityEngine;
using WheresMaPoints.lang;

namespace WheresMaPoints;

[HarmonyPatch]
public static partial class MainPatcher
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(TechPointsSpawner), nameof(TechPointsSpawner.SpawnTechPoint))]
    public static bool TechPointsSpawner_SpawnTechPoint(ref TechPointsSpawner __instance, TechPointsSpawner.Type type)
    {
        if (__instance is null) return true;
        GUIElements.me.hud.tech_points_bar.Show();
        switch (type)
        {
            case TechPointsSpawner.Type.R:
                AddPoints("r");
                break;
            case TechPointsSpawner.Type.G:
                AddPoints("g");
                break;
            case TechPointsSpawner.Type.B:
                AddPoints("b");
                break;
        }

        return false;
    }

    private static void AddPoints(string type)
    {
        MainGame.me.player.AddToParams(type, 1);
        MainGame.me.save.achievements.CheckKeyQuests($"tech_collect_{type}");
        
        if (_cfg.showPointGainAboveKeeper && !Tools.PlayerDisabled() && BaseGUI.all_guis_closed)
        {
            EffectBubblesManager.ShowStacked(MainGame.me.player, new GameRes(type, 1));
        }

        if (_cfg.stillPlayCollectAudio && !Tools.PlayerDisabled())
        {
            MasterAudio.PlaySound("pickup", 1f, null, 0f, "pickup1");
        }
    }

    //private static bool _printAchievements;

    //hooks into the time of day update and saves if the K key was pressed
    [HarmonyPrefix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.Update))]
    public static void TimeOfDay_Update()
    {
        if (MainGame.game_started) return;
        // if (!_printAchievements)
        // {
        //     _printAchievements = true;
        //     foreach (var ach in GameBalance.me.achievements_data)
        //     {
        //         foreach (var s in ach.start_key)
        //         {
        //             Log($"Achievement: StartKey: {s}, ID: {ach.id}, Counter: {ach.counter}");
        //         }
        //     }
        // }

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

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AnimatedGUIPanel), nameof(AnimatedGUIPanel.Update))]
    public static bool AnimatedGUIPanel_Update(ref AnimatedGUIPanel __instance)
    {
        if (!_cfg.alwaysShowXpBar) return true;
        if (!MainGame.game_started) return true;
        __instance.SetVisible(true);
        __instance.Redraw();
        // AccessTools.Method(typeof(AnimatedGUIPanel), "SetVisible").Invoke(__instance, new object[] {true});
        // AccessTools.Method(typeof(HUDTechPointsBar), "Redraw").Invoke(__instance, null);
        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AnimatedGUIPanel), nameof(AnimatedGUIPanel.Init))]
    public static void AnimatedGUIPanel_Init(ref AnimatedGUIPanel __instance)
    {
        if (!_cfg.alwaysShowXpBar) return;
        __instance.SetVisible(true);
       // AccessTools.Method(typeof(AnimatedGUIPanel), "SetVisible").Invoke(__instance, new object[] {true});
    }
}