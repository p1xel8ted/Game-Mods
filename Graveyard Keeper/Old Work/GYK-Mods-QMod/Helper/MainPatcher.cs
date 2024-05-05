using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Helper;

[HarmonyPriority(1)]
[HarmonyPatch]
public static class MainPatcher
{
    private const string DisablePath = "./QMods/disable";
    private static bool _disableMods;

    private static void Log(string message, bool error = false)
    {
        Tools.Log("QModHelper", $"{message}", error);
    }

    public static void Patch()
    {
        try
        {
            var harmony = new Harmony("p1xel8ted.GraveyardKeeper.QModHelper");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            _disableMods = File.Exists(DisablePath);
            if (_disableMods)
            {
                File.Delete(DisablePath);
            }
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }
    }


    [HarmonyPriority(1)]
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BaseGUI), nameof(BaseGUI.Hide), typeof(bool))]
    public static void BaseGUI_Hide()
    {
        if (BaseGUI.all_guis_closed)
        {
            Tools.SetAllInteractionsFalse();
        }
    }


    // foreach (WeatherState weatherState in this._states)
    // {
    //     weatherState.StateUpdate(deltaTime);
    //     if (weatherState.type == WeatherState.WeatherType.Rain && (double)weatherState.amount > 0.7)
    //     {
    //         this._is_rainy = true;
    //     }
    // }
    //
    // [HarmonyPriority(1)]
    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(EnvironmentEngine), nameof(EnvironmentEngine.Update))]
    // public static void EnvironmentEngine_Update(List<WeatherState> ____states)
    // {
    //     if (EnvironmentEngine.me == null) return;
    //     foreach (var weatherState in ____states)
    //     {
    //         Log($"Weather State: {weatherState.type}, {weatherState.amount}");
    //    
    //     }
    // }


    [HarmonyPriority(1)]
    [HarmonyPrefix]
    [HarmonyPatch(typeof(InGameMenuGUI), nameof(InGameMenuGUI.OnPressedSaveAndExit))]
    public static void InGameMenuGUI_OnPressedSaveAndExit()
    {
        MainGame.game_started = false;
    }

    [HarmonyPriority(1)]
    [HarmonyPrefix]
    [HarmonyPatch(typeof(SaveSlotsMenuGUI), nameof(SaveSlotsMenuGUI.Open))]
    public static void SaveSlotsMenuGUI_Open()
    {
        MainGame.game_started = false;
    }


    [HarmonyPriority(1)]
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSettings), nameof(GameSettings.ApplyLanguageChange))]
    public static void GameSettingsApplyLanguageChangePostfix()
    {
        CrossModFields.Lang = GameSettings.me.language.Replace('_', '-').ToLower(CultureInfo.InvariantCulture).Trim();
        CrossModFields.Culture = CultureInfo.GetCultureInfo(CrossModFields.Lang);
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
    }


    [HarmonyPriority(1)]
    [HarmonyPostfix]
    [HarmonyPatch(typeof(QuestSystem), nameof(QuestSystem.OnQuestSucceed), typeof(QuestState))]
    public static void QuestSystemOnQuestSucceedPostfix(ref QuestSystem __instance)
    {
        foreach (var q in __instance._succed_quests)
        {
            Log($"[QuestSucceed]: {q}");
        }
    }


    private static bool _cleanGerryRun;
    // private static bool _fixGerryRun;

    private static readonly string[] SafeGerryTags =
    {
        "tavern_skull",
        "talking_skull",
        "crafting_skull",
        "crafting_skull_1",
        "crafting_skull_2",
        "crafting_skull_3",
        "crafting_skull_4",
        "crafting_skull_5",
        "crafting_skull_6",
        "crafting_skull_7",
        "crafting_skull_8",
        "crafting_skull_9",
        "crafting_skull_10",
        "talking_skull_in_tavern"
    };

    private static bool _printQuestsDone;


    [HarmonyPostfix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.Update))]
    public static void TimeOfDayUpdatePostfix(TimeOfDay __instance)
    {
        if (!_cleanGerryRun)
        {
            if (!MainGame.game_started) return;

            if (!_printQuestsDone)
            {
                var quests = MainGame.me.save.quests._succed_quests;
                //var quests = AccessTools.Field(typeof(QuestSystem), "_succed_quests").GetValue(MainGame.me.save.quests) as List<string>;
                _printQuestsDone = true;
                if (quests != null)
                {
                    foreach (var q in quests)
                    {
                        Log($"[QuestDone]: {q}");
                    }
                }
                else
                {
                    Log($"[QuestDone]: null");
                }
            }


            if (!Tools.TutorialDone()) return;
            //get all gerry objects + a few extras
            var otherGerrys = Object.FindObjectsOfType<WorldGameObject>(true).Where(a => a.obj_id.ToLowerInvariant().Contains("skull")).ToList();

            //removes the extras - mainly the skull objects in the dungeon
            otherGerrys.RemoveAll(a => a.obj_id.ToLowerInvariant().StartsWith("skulls"));

            //log each gerry object found
            foreach (var g in otherGerrys.Where(g => g != null))
            {
                Log($"AllGerries: Gerry: {g.obj_id}, CustomTag: {g.custom_tag}, POS: {g.pos3}, Location: {g.GetMyWorldZoneId()}");
            }

            //find gerrys that match any gerrys made by mods
            var gerrys = WorldMap.GetWorldGameObjectsByCustomTag(CrossModFields.ModGerryTag, false);

            //log each mod_gerry object found
            foreach (var g in gerrys.Where(g => g != null))
            {
                Log($"AllModGerries: Gerry: {g.obj_id}, CustomTag: {g.custom_tag}, POS: {g.pos3}, Location: {g.GetMyWorldZoneId()}");
            }

            //remove each gerry, ensuring no gerrys on the safeTag list are removed
            foreach (var gerry in gerrys.Where(gerry => gerry != null))
            {
                if (SafeGerryTags.Contains(gerry.custom_tag)) continue;
                Log($"Destroyed Gerry: {gerry.obj_id}, CustomTag: {gerry.custom_tag}, POS: {gerry.pos3}, Location: {gerry.GetMyWorldZoneId()}");
                gerry.DestroyMe();
            }

            //remove each mod gerry, ensuring no gerrys on the safeTag list are removed
            foreach (var gerry in otherGerrys.Where(gerry => gerry != null))
            {
                if (SafeGerryTags.Contains(gerry.custom_tag)) continue;
                Log($"Destroyed Gerry: {gerry.obj_id}, CustomTag: {gerry.custom_tag}, POS: {gerry.pos3}, Location: {gerry.GetMyWorldZoneId()}");
                gerry.DestroyMe();
            }
        }

        _cleanGerryRun = true;
        CrossModFields.TimeOfDayFloat = __instance.GetTimeK();
        CrossModFields.ConfigReloadShown = false;
    }


    [HarmonyFinalizer]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.Update))]
    public static Exception Finalizer()
    {
        return null;
    }


    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(SmartAudioEngine), nameof(SmartAudioEngine.OnEndNPCInteraction))]
    public static void OnEndNPCInteractionPrefix()
    {
        CrossModFields.TalkingToNpc("QModHelper: SmartAudioEngine.OnEndNPCInteraction", false);
    }

    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(MovementComponent), nameof(MovementComponent.UpdateMovement), typeof(Vector2), typeof(float))]
    public static void Prefix(ref MovementComponent __instance)
    {
        if (__instance.wgo.is_player)
        {
            CrossModFields.PlayerIsDead = __instance.wgo.is_dead;
            CrossModFields.PlayerIsControlled = __instance.player_controlled_by_script;
            CrossModFields.PlayerFollowingTarget = __instance.is_following_target;
        }
    }


    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(MainMenuGUI), nameof(MainMenuGUI.Open))]
    public static void MainMenuGUI_Open_Prefix()
    {
        try
        {
            var mods = AppDomain.CurrentDomain.GetAssemblies().Where(p => !p.IsDynamic)
                .Where(a => a.Location.ToLowerInvariant().Contains("qmods"));
            Tools.LoadedModsById.Clear();
            Tools.LoadedModsByName.Clear();
            Tools.LoadedModsByFileName.Clear();
            foreach (var mod in mods)
            {
                var modInfo = FileVersionInfo.GetVersionInfo(mod.Location);
                var fileInfo = new FileInfo(modInfo.FileName);
                if (!string.IsNullOrEmpty(modInfo.Comments))
                {
                    Tools.LoadedModsById.Add(modInfo.Comments);
                }

                if (!string.IsNullOrEmpty(modInfo.ProductName))
                {
                    Tools.LoadedModsByName.Add(modInfo.ProductName);
                }

                Tools.LoadedModsByFileName.Add(fileInfo.Name);
            }

            foreach (var m in Tools.LoadedModsByFileName)
            {
                Log($"[ModFileName]: {m}");
            }

            var max = Mathf.Max(new int[]
            {
                Tools.LoadedModsById.Count,
                Tools.LoadedModsByName.Count,
                Tools.LoadedModsByFileName.Count
            });

            Log($"Total loaded mod count: {max}");
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}");
        }
    }

    [HarmonyPostfix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(MainMenuGUI), nameof(MainMenuGUI.Open))]
    public static void MainMenuGUI_Open_Postfix(ref MainMenuGUI __instance)
    {
        if (__instance == null) return;
        _cleanGerryRun = false;
        foreach (var comp in __instance.GetComponentsInChildren<UILabel>()
                     .Where(x => x.name.Contains("credits")))
        {
            comp.text =
                "[F7B000]QMod Reloaded[-] by [F7B000]p1xel8ted[-]\r\ngame by: [F7B000]Lazy Bear Games[-]\r\npublished by: [F7B000]tinyBuild[-]";
            comp.overflowMethod = UILabel.Overflow.ResizeFreely;
            comp.multiLine = true;
            comp.MakePixelPerfect();
        }

        foreach (var comp in __instance.GetComponentsInChildren<UILabel>()
                     .Where(x => x.name.Contains("ver txt")))
        {
            comp.text = _disableMods
                ? $"[F7B000] QMod Reloaded[-] [F70000]Disabled[-] [F7B000](Helper v{Assembly.GetExecutingAssembly().GetName().Version.Major}.{Assembly.GetExecutingAssembly().GetName().Version.Minor}.{Assembly.GetExecutingAssembly().GetName().Version.Build})[-]"
                : $"[F7B000] QMod Reloaded[-] [2BFF00]Enabled[-] [F7B000](Helper v{Assembly.GetExecutingAssembly().GetName().Version.Major}.{Assembly.GetExecutingAssembly().GetName().Version.Minor}.{Assembly.GetExecutingAssembly().GetName().Version.Build})[-]";
            comp.overflowMethod = UILabel.Overflow.ResizeFreely;
            comp.multiLine = true;
            comp.MakePixelPerfect();
            //labelToMimic = comp;
        }

        if (LazyConsts.VERSION < 1.405f)
        {
            GUIElements.me.dialog.OpenOK("QMod Manager Reloaded", () =>
            {
                GC.Collect();
                Resources.UnloadUnusedAssets();
                Application.Quit();
            }, "Incompatible game version detected Please update to at least 1.405!", true, "Graveyard Keeper will now exit.");
        }
    }


    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(VendorGUI), nameof(VendorGUI.Open), typeof(WorldGameObject), typeof(GJCommons.VoidDelegate))]
    public static void VendorGUI_Open()
    {
        if (!MainGame.game_started) return;
        CrossModFields.TalkingToNpc("QModHelper: VendorGUI.Open", true);
    }

    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(VendorGUI), nameof(VendorGUI.Hide), typeof(bool))]
    public static void VendorGUI_Hide()
    {
        if (!MainGame.game_started) return;
        CrossModFields.TalkingToNpc("QModHelper: VendorGUI.Hide", false);
    }

    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(VendorGUI), nameof(VendorGUI.OnClosePressed))]
    public static void VendorGUI_OnClosePressed()
    {
        if (!MainGame.game_started) return;
        CrossModFields.TalkingToNpc("QModHelper: VendorGUI.OnClosePressed", false);
    }


    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.Interact))]
    public static void WorldGameObjectInteractPrefix(ref WorldGameObject __instance, ref WorldGameObject other_obj)
    {
        if (!MainGame.game_started || __instance == null) return;

        Log($"Object: {__instance.obj_id}, CustomTag: {__instance.custom_tag}, Location: {__instance.pos3}, Zone:{__instance.GetMyWorldZoneId()}");
        //Where's Ma Storage
        CrossModFields.PreviousWgoInteraction = CrossModFields.CurrentWgoInteraction;
        CrossModFields.CurrentWgoInteraction = __instance;
        CrossModFields.IsVendor = __instance.vendor != null;
        CrossModFields.IsCraft = other_obj.is_player && __instance.obj_def.interaction_type != ObjectDefinition.InteractionType.Chest && __instance.obj_def.has_craft;
        //Log($"IsCraft: {CrossModFields.IsCraft}",true);
        CrossModFields.IsChest = __instance.obj_def.interaction_type == ObjectDefinition.InteractionType.Chest;
        CrossModFields.IsBarman = __instance.obj_id.ToLowerInvariant().Contains("barman");
        CrossModFields.IsTavernCellarRack = __instance.obj_id.ToLowerInvariant().Contains("tavern_cellar_rack");
        CrossModFields.IsRefugee = __instance.obj_id.ToLowerInvariant().Contains("refugee");
        CrossModFields.IsWritersTable = __instance.obj_id.ToLowerInvariant().Contains("writer");
        CrossModFields.IsSoulBox = __instance.obj_id.ToLowerInvariant().Contains("soul_container");
        CrossModFields.IsChurchPulpit = __instance.obj_id.ToLowerInvariant().Contains("pulpit");
        CrossModFields.IsMoneyLender = __instance.obj_id.ToLowerInvariant().Contains("lender");

        if (__instance.obj_def.inventory_size > 0)
        {
            if (__instance.obj_id.Length <= 0)
            {
                __instance.data.sub_name = "Unknown#" + __instance.GetMyWorldZoneId();
            }
            else
            {
                __instance.data.sub_name = __instance.obj_id + "#" + __instance.GetMyWorldZoneId();
            }
        }

        //Beam Me Up & Save Now
        CrossModFields.IsInDungeon = __instance.obj_id.ToLowerInvariant().Contains("dungeon_enter");
        // Log($"[InDungeon]: {CrossModFields.IsInDungeon}");

        //I Build Where I Want
        if (__instance.obj_def.interaction_type is not ObjectDefinition.InteractionType.None)
        {
            CrossModFields.CraftAnywhere = false;
        }

        //Beam Me Up Gerry
        CrossModFields.TalkingToNpc("QModHelper: WorldGameObject.Interact", __instance.obj_def.IsNPC() && !__instance.obj_id.Contains("zombie"));

        //Log($"[WorldGameObject.Interact]: Instance: {__instance.obj_id}, InstanceIsPlayer: {__instance.is_player},  Other: {other_obj.obj_id}, OtherIsPlayer: {other_obj.is_player}");
    }
}