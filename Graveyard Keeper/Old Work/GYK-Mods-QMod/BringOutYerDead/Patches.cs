using System;
using BringOutYerDead.lang;
using HarmonyLib;
using Helper;
using UnityEngine;

namespace BringOutYerDead;

[HarmonyPatch]
public static partial class MainPatcher
{
    private static bool _prideDayLogged;
    private static WorldGameObject _donkey;
    private static WorldGameObject _carrotBox;

    private static int _carrotCount;
    private static int _deliveryCount;
    private static bool _tutorialDoneShown;

    private static LogicData _ld;
    private static bool _strikeDone;

    //hooks into the time of day update and saves if the K key was pressed
    [HarmonyPrefix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.Update))]
    public static void TimeOfDay_Update(ref TimeOfDay __instance)
    {
        if (__instance == null) return;
        if (Input.GetKeyUp(_cfg.ReloadConfigKeyBind))
        {
            _cfg = Config.GetOptions();

            if (Time.time - CrossModFields.ConfigReloadTime > CrossModFields.ConfigReloadTimeLength)
            {
                Tools.ShowMessage(GetLocalizedString(strings.ConfigMessage), Vector3.zero);
                CrossModFields.ConfigReloadTime = Time.time;
            }
        }

        if (!MainGame.game_started) return;
        if (MainGame.paused) return;

        if (!Tools.TutorialDone() && !_tutorialDoneShown)
        {
            _tutorialDoneShown = true;
            Log("Need to complete all 'tutorial' quests first, upto and including the repair the sword quest.");
            return;
        }

        if (!Tools.TutorialDone())
        {
            if (!_tutorialDoneShown)
            {
                _tutorialDoneShown = true;
                Log("Need to complete all 'tutorial' quests first, upto and including the repair the sword quest.");
            }

            return;
        }

        _ld = new LogicData("donkey")
        {
            _started = false
        };
        // AccessTools.Field(typeof(LogicData), "_started").SetValue(_ld, false);

        if (_donkey == null)
        {
            _donkey = WorldMap.GetWorldGameObjectByCustomTag("donkey", true);
        }


        if (_donkey != null)
        {
            var dataGetParam = _donkey.data.GetParam("speed");
            var getParam = _donkey.GetParam("speed");

            _strikeDone = _donkey.GetParam("strike_completed") > 0f;

            if (dataGetParam < _cfg.DonkeySpeed || getParam < _cfg.DonkeySpeed)
            {
                Log($"TDU: Donkey old speeds: DataGetParam: {dataGetParam}, GetParam: {getParam}");
                _donkey.components.character.SetSpeed(_cfg.DonkeySpeed);
                Log($"TDU: Donkey new speeds: DataGetParam: {dataGetParam}, GetParam: {getParam}");
            }

            if (!_strikeDone)
            {
                Log($"Must complete the donkey strike first! Pay him 10 carrots, grease his wheels etc.");
                return;
            }
        }
        else
        {
            Log($"Donkey is null!?!?!");
            return;
        }


        if (MainGame.me.save.day_of_week == 1)
        {
            if (!_prideDayLogged)
            {
                Log($"Pride day! Skipping donkey as he doesnt come anyway when asked if its Pride day!");
                _prideDayLogged = true;
            }

            return;
        }


        switch (TimeOfDay.me.time_of_day_enum)
        {
            case TimeOfDay.TimeOfDayEnum.Night:
                if (!_cfg.NightDelivery)
                {
                    Log("Night delivery is disabled in config!");
                    break;
                }

                if (!_sd.NightDelivery)
                {
                    //Tools.ShowMessage("Night Delivery!", MainGame.me.player_pos);

                    if (ForceDonkey(_donkey))
                    {
                        Log($"It's night! Beginning night time delivery!");
                        _sd.NightDelivery = true;
                    }
                    else
                    {
                        _sd.NightDelivery = false;
                        Log($"It's night! But we failed to force the donkey to deliver!");
                    }
                }

                break;
            case TimeOfDay.TimeOfDayEnum.Morning:
                if (!_cfg.MorningDelivery)
                {
                    Log("Morning delivery is disabled in config!");
                    break;
                }

                if (!_sd.MorningDelivery)
                {
                    //Tools.ShowMessage("Morning Delivery!", MainGame.me.player_pos);
                    Log($"It's morning! Beginning morning delivery!");
                    if (ForceDonkey(_donkey))
                    {
                        _sd.MorningDelivery = true;
                    }
                    else
                    {
                        _sd.MorningDelivery = false;
                        Log($"It's morning! But we failed to force the donkey to deliver!");
                    }
                }

                break;
            case TimeOfDay.TimeOfDayEnum.Day:
                if (!_cfg.DayDelivery)
                {
                    Log("Day delivery is disabled in config!");
                    return;
                }

                if (!_sd.DayDelivery)
                {
                    // Tools.ShowMessage("Day Delivery!", MainGame.me.player_pos);
                    Log($"It's Day! Beginning midday delivery!");
                    if (ForceDonkey(_donkey))
                    {
                        _sd.DayDelivery = true;
                    }
                    else
                    {
                        _sd.DayDelivery = false;
                        Log($"It's midday! But we failed to force the donkey to deliver!");
                    }
                }

                break;
            case TimeOfDay.TimeOfDayEnum.Evening:
                if (!_cfg.EveningDelivery)
                {
                    Log("Evening delivery is disabled in config!");
                    return;
                }

                if (!_sd.EveningDelivery)
                {
                    if (ForceDonkey(_donkey))
                    {
                        Log($"It's evening! Beginning evening delivery!");
                        _sd.EveningDelivery = true;
                    }
                    else
                    {
                        _sd.EveningDelivery = false;
                        Log($"It's evening! But we failed to force the donkey to deliver!");
                    }
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(EnvironmentEngine), nameof(EnvironmentEngine.OnEndOfDay))]
    public static void EnvironmentEngine_OnEndOfDay(ref EnvironmentEngine __instance)
    {
        if (__instance == null) return;
        _sd.MorningDelivery = false;
        _sd.DayDelivery = false;
        _sd.EveningDelivery = false;
        _sd.NightDelivery = false;
        _prideDayLogged = false;
        SaveData.WriteOptions();
        Log("Resetting donkey day delivery flags!");
    }

    private static bool ForceDonkey(WorldGameObject donkey)
    {
        if (donkey == null)
        {
            donkey = WorldMap.GetWorldGameObjectByCustomTag("donkey", true);
            if (donkey == null)
            {
                Log($"Donkey appears to be on a holiday and cannot be found!");
                return false;
            }
        }

        if (!Tools.TutorialDone())
        {
            Log("Need to complete all 'tutorial' quests first, upto and including the repair the sword quest.");
            return false;
        }

        _strikeDone = donkey.GetParam("strike_completed") > 0f;
        if (!_strikeDone)
        {
            Log($"Must complete the donkey strike first! Pay him 10 carrots, grease his wheels etc.");
            return false;
        }

        _carrotBox = WorldMap.GetWorldGameObjectByCustomTag("carrot_box", true);

        if (_carrotBox == null)
        {
            Log($"No carrot box! How are you going to pay the donkey?!");
            return false;
        }

        if (_carrotBox.data.inventory.Count > 0)
        {
            _carrotCount = _carrotBox.data.inventory[0].value;
        }


        Log($"Current carrots: {_carrotCount}");
        if (_carrotCount <= 0)
        {
            Log($"No carrots! How are you going to pay the donkey?!");
            return false;
        }

        Log("Forcing donkey to do his thing! Unless it's Pride/Sunday...");
        SaveData.WriteOptions();
        _donkey = WorldMap.GetWorldGameObjectByCustomTag("donkey", true);
        // _donkey.SetParam("send_forced_donkey",1);
        // _donkey.SetParam("donkey_no_body_say",0);
        // _donkey.SetParam("donkey_forced_came",1);
        //

        _ld = new LogicData("donkey");
        _ld.ForceExecute(false);
     
        if (_donkey != null)
        {
            _sd.DonkeySpawned = true;
            SaveData.WriteOptions();
            Log($"FD: Found donkey spawn!: Speed: {_donkey.data.GetParam("speed")}");
            _donkey.components.character.SetSpeed(_cfg.DonkeySpeed);
            Log($"FD: Found donkey spawn!: New Speed: {_donkey.data.GetParam("speed")}");

            return true;
        }

        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(NewBodyArrivedGUI), nameof(NewBodyArrivedGUI.Display))]
    public static void NewBodyArrivedGUI_Display(ref NewBodyArrivedGUI __instance)
    {
        if (!MainGame.game_started) return;
        if (!Tools.TutorialDone()) return;
        if (!_strikeDone) return;
        if (__instance == null) return;
        _deliveryCount++;
        _donkey = WorldMap.GetWorldGameObjectByCustomTag("donkey", true);
        if (_donkey != null)
        {
            Log($"Donkey on way home, setting speed! Current speed: {_donkey.data.GetParam("speed")}");
            _donkey.components.character.SetSpeed(_cfg.DonkeySpeed);
        }

        _carrotBox = WorldMap.GetWorldGameObjectByCustomTag("carrot_box", true);
        if (_carrotBox != null)
        {
            if (_carrotBox.data.inventory.Count > 0)
            {
                _carrotCount = _carrotBox.data.inventory[0].value;
            }
        }

        if (_carrotCount <= 0)
        {
            Tools.ShowMessage(GetLocalizedString(strings.CarrotMessage), MainGame.me.player_pos, sayAsPlayer: true);
        }

        Log($"Current session delivery count: {_deliveryCount}!");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(LogicData), nameof(LogicData.Execute))]
    public static bool LogicData_Execute(ref LogicData __instance)
    {
        if (!MainGame.game_started) return true;
        if (!Tools.TutorialDone()) return true;
        if (!_strikeDone) return true;
        if (__instance == null) return true;
        if (__instance.id == "donkey")
        {
            if (__instance == _ld)
            {
                Log($"LogicData_Execute: My donkey spawning!");
                return true;
            }

            Log($"LogicData_Execute: Game trying to spawn regular donkey, skipping!");
            return false;
        }

        return true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.TeleportToGDPoint))]
    public static void WorldGameObject_TeleportToGDPoint(ref WorldGameObject __instance)
    {
        if (!MainGame.game_started) return;
        if (!Tools.TutorialDone()) return;
        if (!_strikeDone) return;
        if (__instance == null) return;
        if (string.IsNullOrEmpty(__instance.custom_tag) || string.IsNullOrWhiteSpace(__instance.custom_tag)) return;

        if (__instance.custom_tag.Equals("donkey"))
        {
            Log("Donkey is home! Setting DonkeySpawned to false!");
            _sd.DonkeySpawned = false;
            SaveData.WriteOptions();
            // _donkey = null;
        }
    }
}