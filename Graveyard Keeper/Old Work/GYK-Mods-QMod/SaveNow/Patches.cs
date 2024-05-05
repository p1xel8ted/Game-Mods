using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using HarmonyLib;
using Helper;
using Rewired;
using SaveNow.lang;
using UnityEngine;

namespace SaveNow;

[HarmonyPatch]
public static partial class MainPatcher
{
    //sorts the save list gui via newest to oldest (newest at top and highlights it). Also trims the list to the
    //amount specified in the config
    [HarmonyPrefix]
    [HarmonyPatch(typeof(SaveSlotsMenuGUI), nameof(SaveSlotsMenuGUI.RedrawSlots))]
    public static void SaveSlotsMenuGUI_RedrawSlots(ref List<SaveSlotData> slot_datas, ref bool focus_on_first)
    {
        slot_datas.Clear();
        AllSaveGames.Clear();
        _sortedTrimmedSaveGames.Clear();

        //load each save game in the directory
        foreach (var text in Directory.GetFiles(PlatformSpecific.GetSaveFolder(), "*.info",
                     SearchOption.TopDirectoryOnly))
        {
            var data = SaveSlotData.FromJSON(File.ReadAllText(text));
            if (data == null) continue;
            data.filename_no_extension = Path.GetFileNameWithoutExtension(text);
            AllSaveGames.Add(data);
        }


        if (_cfg.sortByRealTime)
        {
            _sortedTrimmedSaveGames = _cfg.ascendingSort ? AllSaveGames.OrderBy(o => o.real_time).ToList() : AllSaveGames.OrderByDescending(o => o.real_time).ToList();
        }
        else
        {
            _sortedTrimmedSaveGames = _cfg.ascendingSort ? AllSaveGames.OrderBy(o => o.game_time).ToList() : AllSaveGames.OrderByDescending(o => o.game_time).ToList();
        }

        if (_sortedTrimmedSaveGames.Count > _cfg.maximumSavesVisible)
        {
            Resize(_sortedTrimmedSaveGames, _cfg.maximumSavesVisible);
        }

        slot_datas = _sortedTrimmedSaveGames;
        focus_on_first = true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InGameMenuGUI), nameof(InGameMenuGUI.OnPressedSaveAndExit))]
    public static bool InGameMenuGUI_OnPressedSaveAndExit(InGameMenuGUI __instance)
    {
        if (!Tools.TutorialDone()) return true;
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        
        __instance.SetControllsActive(false);
        __instance.OnClosePressed();

        var messageText = strings.SaveAreYouSureMenu + "?\n\n" +
                          (_cfg.disableSaveOnExit ? strings.SaveProgressNotSaved : CrossModFields.IsInDungeon ? strings.SaveProgressNotSaved : strings.SaveProgressSaved) + ".";

        if (_cfg.exitToDesktop)
        {
            messageText = strings.SaveAreYouSureDesktop + "?\n\n" +
                          (_cfg.disableSaveOnExit ? strings.SaveProgressNotSaved : CrossModFields.IsInDungeon ? strings.SaveProgressNotSaved : strings.SaveProgressSaved) + ".";
        }

        var test = GUIElements.me.dialog;
        test.OpenYesNo(messageText
            ,
            delegate
            {
                if (_cfg.disableSaveOnExit || CrossModFields.IsInDungeon)
                {
                    SaveExit();
                }
                else
                {
                    if (SaveLocation(true, string.Empty))
                    {
                        PlatformSpecific.SaveGame(MainGame.me.save_slot, MainGame.me.save,
                            delegate { SaveExit(); });
                    }
                }
            }, () =>
            {
                Tools.ResumeControl();
                test.OnClosePressed();
            }
            , () =>
            {
                Tools.ResumeControl();
                test.OnClosePressed();
            });

        void SaveExit()
        {
            if (_cfg.exitToDesktop)
            {
                GC.Collect();
                Resources.UnloadUnusedAssets();
                Application.Quit();
            }
            else
            {
                LoadingGUI.Show(__instance.ReturnToMainMenu);
            }
        }

        return false;
    }

    // if this isn't here, when you sleep, it teleport you back to where the mod saved you last
    [HarmonyPrefix]
    [HarmonyPatch(typeof(SleepGUI), nameof(SleepGUI.WakeUp))]
    public static void SleepGUI_WakeUp()
    {
        SaveLocation(false, string.Empty);
    }


    //change exit menu based on config
    [HarmonyPostfix]
    [HarmonyPatch(typeof(InGameMenuGUI), nameof(InGameMenuGUI.Open))]
    public static void InGameMenuGUI_Open(ref InGameMenuGUI __instance)
    {
        if (__instance == null) return;
        foreach (var comp in __instance.GetComponentsInChildren<UIButton>().Where(x => x.name.Contains("exit")))
        foreach (var label in comp.GetComponentsInChildren<UILabel>())
            if (_cfg.exitToDesktop)
                label.text = strings.ExitButtonText;
    }


    //this is called last when loading a save game without patch
    [HarmonyPrefix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.GlobalEventsCheck))]
    public static void GameSave_GlobalEventsCheck()
    {
        RestoreLocation();
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(MovementComponent), nameof(MovementComponent.UpdateMovement), null)]
    public static void MovementComponent_UpdateMovement(MovementComponent __instance)
    {
        _canSave = !__instance.player_controlled_by_script;
    }


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
        if (!MainGame.game_started) return;

        if (!Tools.TutorialDone()) return;

        if (Input.GetKeyUp(_cfg.manualSaveKeyBind) || (_cfg.enableManualSaveControllerButton && LazyInput.gamepad_active && ReInput.players.GetPlayer(0).GetButtonDown(_cfg.manualSaveControllerButton)))
        {
            if (CrossModFields.IsInDungeon)
            {
                Tools.SpawnGerry(GetLocalizedString(strings.CantSaveHere), Vector3.zero, CrossModFields.ModGerryTag);
            }
            else
            {
                GUIElements.me.ShowSavingStatus(true);
                if (_cfg.newFileOnManualSave)
                {
                    var date = DateTime.Now.ToString("ddmmyyhhmmss");
                    var newSlot = $"manualsave.{date}".Trim();

                    MainGame.me.save_slot.filename_no_extension = newSlot;
                    PlatformSpecific.SaveGame(MainGame.me.save_slot, MainGame.me.save,
                        delegate
                        {
                            SaveLocation(false, newSlot);
                            GUIElements.me.ShowSavingStatus(false);
                        });
                }
                else
                {
                    PlatformSpecific.SaveGame(MainGame.me.save_slot, MainGame.me.save,
                        delegate
                        {
                            SaveLocation(false, string.Empty);
                            GUIElements.me.ShowSavingStatus(false);
                        });
                }
            }
        }
    }
}