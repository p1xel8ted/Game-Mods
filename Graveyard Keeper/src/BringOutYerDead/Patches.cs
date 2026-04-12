namespace BringOutYerDead;

[HarmonyPatch]
public static class Patches
{
    private static WorldGameObject _donkey;
    private static WorldGameObject _carrotBox;
    private static int _carrotCount;
    private static int _deliveryCount;
    private static bool _strikeDone;
    internal static LogicData Ld;
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.GlobalEventsCheck))]
    public static void GameSave_GlobalEventsCheck_DebugWarning()
    {
        Plugin.ShowDebugWarningOnce();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainGame), nameof(MainGame.Update))]
    public static void MainGame_Update()
    {
        if (!MainGame.game_started) return;
        if (MainGame.paused) return;

        if (!Helpers.TutorialDone() && !Plugin.InternalTutMessageShown.Value)
        {
            Plugin.InternalTutMessageShown.Value = true;
            if (Plugin.DebugEnabled) Helpers.Log("Need to complete all 'tutorial' quests first, up to and including the repair the sword quest.");
            return;
        }

        if (Plugin.Donkey == null)
        {
            Plugin.Donkey = WorldMap.GetWorldGameObjectByCustomTag("donkey", true);
        }

        if (Plugin.Donkey != null)
        {
            var dataGetParam = Plugin.Donkey.data.GetParam("speed");
            var getParam = Plugin.Donkey.GetParam("speed");

            _strikeDone = Plugin.Donkey.GetParam("strike_completed") > 0f;

            if (dataGetParam < Plugin.DonkeySpeed.Value || getParam < Plugin.DonkeySpeed.Value)
            {
                if (Plugin.DebugEnabled) Helpers.Log($"TDU: Donkey old speeds: DataGetParam: {dataGetParam}, GetParam: {getParam}");
                Plugin.Donkey.components.character.SetSpeed(Plugin.DonkeySpeed.Value);
                if (Plugin.DebugEnabled) Helpers.Log($"TDU: Donkey new speeds: DataGetParam: {dataGetParam}, GetParam: {getParam}");
            }

            if (!_strikeDone)
            {
                if (Plugin.DebugEnabled) Helpers.Log($"Must complete the donkey strike first! Pay him 10 carrots, grease his wheels etc.");
                return;
            }
        }
        else
        {
            if (Plugin.DebugEnabled) Helpers.Log($"Donkey is null!?!?!");
            return;
        }

        if (MainGame.me.save.day_of_week == 1)
        {
            if (!Plugin.PrideDayLogged)
            {
                if (Plugin.DebugEnabled) Helpers.Log($"Pride day! Skipping donkey as he doesnt come anyway when asked if its Pride day!");
                Plugin.PrideDayLogged = true;
            }

            return;
        }

        switch (TimeOfDay.me.time_of_day_enum)
        {
            case TimeOfDay.TimeOfDayEnum.Night:
                if (!Plugin.NightDelivery.Value)
                {
                    if (Plugin.DebugEnabled) Helpers.Log("Night delivery is disabled in config!");
                    break;
                }

                if (!Plugin.InternalNightDelivery.Value)
                {
                    if (ForceDonkey(Plugin.Donkey))
                    {
                        if (Plugin.DebugEnabled) Helpers.Log($"It's night! Beginning night time delivery!");
                        Plugin.InternalNightDelivery.Value = true;
                    }
                    else
                    {
                        Plugin.InternalNightDelivery.Value = false;
                        if (Plugin.DebugEnabled) Helpers.Log($"It's night! But we failed to force the donkey to deliver!");
                    }
                }

                break;
            case TimeOfDay.TimeOfDayEnum.Morning:
                if (!Plugin.MorningDelivery.Value)
                {
                    if (Plugin.DebugEnabled) Helpers.Log("Morning delivery is disabled in config!");
                    break;
                }

                if (!Plugin.InternalMorningDelivery.Value)
                {
                    if (Plugin.DebugEnabled) Helpers.Log($"It's morning! Beginning morning delivery!");
                    if (ForceDonkey(Plugin.Donkey))
                    {
                        Plugin.InternalMorningDelivery.Value = true;
                    }
                    else
                    {
                        Plugin.InternalMorningDelivery.Value = false;
                        if (Plugin.DebugEnabled) Helpers.Log($"It's morning! But we failed to force the donkey to deliver!");
                    }
                }

                break;
            case TimeOfDay.TimeOfDayEnum.Day:
                if (!Plugin.DayDelivery.Value)
                {
                    if (Plugin.DebugEnabled) Helpers.Log("Day delivery is disabled in config!");
                    return;
                }

                if (!Plugin.InternalDayDelivery.Value)
                {
                    if (Plugin.DebugEnabled) Helpers.Log($"It's Day! Beginning midday delivery!");
                    if (ForceDonkey(Plugin.Donkey))
                    {
                        Plugin.InternalDayDelivery.Value = true;
                    }
                    else
                    {
                        Plugin.InternalDayDelivery.Value = false;
                        if (Plugin.DebugEnabled) Helpers.Log($"It's midday! But we failed to force the donkey to deliver!");
                    }
                }

                break;
            case TimeOfDay.TimeOfDayEnum.Evening:
                if (!Plugin.EveningDelivery.Value)
                {
                    if (Plugin.DebugEnabled) Helpers.Log("Evening delivery is disabled in config!");
                    return;
                }

                if (!Plugin.InternalEveningDelivery.Value)
                {
                    if (ForceDonkey(Plugin.Donkey))
                    {
                        if (Plugin.DebugEnabled) Helpers.Log($"It's evening! Beginning evening delivery!");
                        Plugin.InternalEveningDelivery.Value = true;
                    }
                    else
                    {
                        Plugin.InternalEveningDelivery.Value = false;
                        if (Plugin.DebugEnabled) Helpers.Log($"It's evening! But we failed to force the donkey to deliver!");
                    }
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(EnvironmentEngine), nameof(EnvironmentEngine.OnEndOfDay))]
    public static void EnvironmentEngine_OnEndOfDay()
    {
        if (EnvironmentEngine.me == null) return;
        Plugin.InternalMorningDelivery.Value = false;
        Plugin.InternalDayDelivery.Value = false;
        Plugin.InternalEveningDelivery.Value = false;
        Plugin.InternalNightDelivery.Value = false;
        Plugin.PrideDayLogged = false;
        if (Plugin.DebugEnabled) Helpers.Log("Resetting donkey day delivery flags!");
    }

    internal static bool ForceDonkey(WorldGameObject donkey)
    {
        if (donkey == null)
        {
            donkey = WorldMap.GetWorldGameObjectByCustomTag("donkey", true);
            if (donkey == null)
            {
                if (Plugin.DebugEnabled) Helpers.Log($"Donkey appears to be on a holiday and cannot be found!");
                return false;
            }
        }

        if (!Helpers.TutorialDone())
        {
            if (Plugin.DebugEnabled) Helpers.Log("Need to complete all 'tutorial' quests first, upto and including the repair the sword quest.");
            return false;
        }

        _strikeDone = donkey.GetParam("strike_completed") > 0f;
        if (!_strikeDone)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"Must complete the donkey strike first! Pay him 10 carrots, grease his wheels etc.");
            return false;
        }

        _carrotBox = WorldMap.GetWorldGameObjectByCustomTag("carrot_box", true);

        if (_carrotBox == null)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"No carrot box! How are you going to pay the donkey?!");
            return false;
        }

        if (_carrotBox.data.inventory.Count > 0)
        {
            _carrotCount = _carrotBox.data.inventory[0].value;
        }


        if (Plugin.DebugEnabled) Helpers.Log($"Current carrots: {_carrotCount}");
        if (_carrotCount <= 0)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"No carrots! How are you going to pay the donkey?!");
            return false;
        }

        if (Plugin.DebugEnabled) Helpers.Log("Forcing donkey to do his thing! Unless it's Pride/Sunday...");

        _donkey = WorldMap.GetWorldGameObjectByCustomTag("donkey", true);

        Ld = new LogicData("donkey");
        Ld.ForceExecute(false);

        if (_donkey != null)
        {
            Plugin.InternalDonkeySpawned.Value = true;
            if (Plugin.DebugEnabled) Helpers.Log($"FD: Found donkey spawn!: Speed: {_donkey.data.GetParam("speed")}");
            _donkey.components.character.SetSpeed(Plugin.DonkeySpeed.Value);
            if (Plugin.DebugEnabled) Helpers.Log($"FD: Found donkey spawn!: New Speed: {_donkey.data.GetParam("speed")}");

            return true;
        }

        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(NewBodyArrivedGUI), nameof(NewBodyArrivedGUI.Display))]
    public static void NewBodyArrivedGUI_Display(NewBodyArrivedGUI __instance)
    {
        if (!MainGame.game_started) return;
        if (!Helpers.TutorialDone()) return;
        if (!_strikeDone) return;
        if (__instance == null) return;
        _deliveryCount++;
        _donkey = WorldMap.GetWorldGameObjectByCustomTag("donkey", true);
        if (_donkey != null)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"Donkey on way home, setting speed! Current speed: {_donkey.data.GetParam("speed")}");
            _donkey.components.character.SetSpeed(Plugin.DonkeySpeed.Value);
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
            Lang.Reload();
            MainGame.me.player.Say(Lang.Get("CarrotMessage"), null, false, SpeechBubbleGUI.SpeechBubbleType.Think, SmartSpeechEngine.VoiceID.None, true);
        }

        if (Plugin.DebugEnabled) Helpers.Log($"Current session delivery count: {_deliveryCount}!");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(LogicData), nameof(LogicData.Execute))]
    public static bool LogicData_Execute(LogicData __instance)
    {
        if (!MainGame.game_started) return true;
        if (!Helpers.TutorialDone()) return true;
        if (!_strikeDone) return true;
        if (__instance == null) return true;
        if (__instance.id == "donkey")
        {
            if (__instance == Ld)
            {
                if (Plugin.DebugEnabled) Helpers.Log($"Helpers.LogicData_Execute: My donkey spawning!");
                return true;
            }

            if (Plugin.DebugEnabled) Helpers.Log($"Helpers.LogicData_Execute: Game trying to spawn regular donkey, skipping!");
            return false;
        }

        return true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.TeleportToGDPoint))]
    public static void WorldGameObject_TeleportToGDPoint(WorldGameObject __instance)
    {
        if (!MainGame.game_started) return;
        if (!Helpers.TutorialDone()) return;
        if (!_strikeDone) return;
        if (__instance == null) return;
        if (string.IsNullOrEmpty(__instance.custom_tag) || string.IsNullOrWhiteSpace(__instance.custom_tag)) return;

        if (__instance.custom_tag.Equals("donkey"))
        {
            if (Plugin.DebugEnabled) Helpers.Log("Donkey is home! Setting DonkeySpawned to false!");
            Plugin.InternalDonkeySpawned.Value = false;
            Ld = null;
        }
    }
}