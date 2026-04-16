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

    // Transition-tracking so per-frame logs don't spam the BepInEx console. Each holds
    // the last-logged value for its topic; we log only when the value changes.
    private static TimeOfDay.TimeOfDayEnum? _lastPhaseLogged;
    private static bool? _lastTutLogged;
    private static bool? _lastStrikeLogged;
    private static float _lastDeliveryGameTime = -1f;

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

        var tutorialDone = Helpers.TutorialDone();
        if (_lastTutLogged != tutorialDone)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[Tut] TutorialDone transitioned {_lastTutLogged?.ToString() ?? "null"} -> {tutorialDone}. Reason: {Helpers.TutorialDoneReason()}");
            _lastTutLogged = tutorialDone;
        }

        if (!tutorialDone && !Plugin.InternalTutMessageShown.Value)
        {
            Plugin.InternalTutMessageShown.Value = true;
            if (Plugin.DebugEnabled) Helpers.Log("[Tut] Need to complete all 'tutorial' quests first, up to and including the repair the sword quest.");
            return;
        }

        if (Plugin.Donkey == null)
        {
            Plugin.Donkey = WorldMap.GetWorldGameObjectByCustomTag("donkey", true);
            if (Plugin.DebugEnabled && Plugin.Donkey != null) Helpers.Log($"[Tick] Cached donkey WGO: {Plugin.Donkey.obj_id} at {Plugin.Donkey.pos3}");
        }

        if (Plugin.Donkey != null)
        {
            var dataGetParam = Plugin.Donkey.data.GetParam("speed");
            var getParam = Plugin.Donkey.GetParam("speed");

            var strikeNow = Plugin.Donkey.GetParam("strike_completed") > 0f;
            if (_lastStrikeLogged != strikeNow)
            {
                if (Plugin.DebugEnabled) Helpers.Log($"[Strike] strike_completed transitioned {_lastStrikeLogged?.ToString() ?? "null"} -> {strikeNow} (raw param: {Plugin.Donkey.GetParam("strike_completed")})");
                _lastStrikeLogged = strikeNow;
            }
            _strikeDone = strikeNow;

            if (dataGetParam < Plugin.DonkeySpeed.Value || getParam < Plugin.DonkeySpeed.Value)
            {
                if (Plugin.DebugEnabled) Helpers.Log($"[Speed] Donkey old speeds — dataGetParam: {dataGetParam}, getParam: {getParam}, target: {Plugin.DonkeySpeed.Value}");
                Plugin.Donkey.components.character.SetSpeed(Plugin.DonkeySpeed.Value);
                if (Plugin.DebugEnabled) Helpers.Log($"[Speed] Donkey new speeds — dataGetParam: {Plugin.Donkey.data.GetParam("speed")}, getParam: {Plugin.Donkey.GetParam("speed")}");
            }

            if (!_strikeDone)
            {
                return;
            }
        }
        else
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[Tick] Donkey WGO not found in world (WorldMap.GetWorldGameObjectByCustomTag('donkey') returned null)");
            return;
        }

        if (MainGame.me.save.day_of_week == 1)
        {
            if (!Plugin.PrideDayLogged)
            {
                if (Plugin.DebugEnabled) Helpers.Log($"[PrideDay] day_of_week==1 (Pride/Sunday); skipping donkey — vanilla donkey doesn't deliver this day anyway.");
                Plugin.PrideDayLogged = true;
            }

            return;
        }

        var phase = TimeOfDay.me.time_of_day_enum;
        if (_lastPhaseLogged != phase)
        {
            if (Plugin.DebugEnabled)
            {
                var timeK = TimeOfDay.me.GetTimeK();
                var rawTime = TimeOfDay.me.time_of_day;
                var gameTime = MainGame.game_time;
                Helpers.Log($"[Phase] {_lastPhaseLogged?.ToString() ?? "null"} -> {phase} | timeK={timeK:F4} time_of_day={rawTime:F4} game_time={gameTime:F4} day={MainGame.me.save.day} dow={MainGame.me.save.day_of_week}");
                Helpers.Log($"[Phase] Flags: Night={Plugin.InternalNightDelivery.Value} Morning={Plugin.InternalMorningDelivery.Value} Day={Plugin.InternalDayDelivery.Value} Evening={Plugin.InternalEveningDelivery.Value} | Enabled: Night={Plugin.NightDelivery.Value} Morning={Plugin.MorningDelivery.Value} Day={Plugin.DayDelivery.Value} Evening={Plugin.EveningDelivery.Value}");
                Helpers.Log($"[Phase] DonkeySpawned={Plugin.InternalDonkeySpawned.Value} Ld={(Ld == null ? "null" : "active")} donkey_in_world={(WorldMap.GetWorldGameObjectByCustomTag("donkey", true) != null)}");
            }
            _lastPhaseLogged = phase;
        }

        switch (phase)
        {
            case TimeOfDay.TimeOfDayEnum.Night:
                if (!Plugin.NightDelivery.Value)
                {
                    break;
                }

                if (!Plugin.InternalNightDelivery.Value)
                {
                    if (ForceDonkey(Plugin.Donkey))
                    {
                        if (Plugin.DebugEnabled) Helpers.Log($"[Phase] Night delivery triggered successfully.");
                        Plugin.InternalNightDelivery.Value = true;
                    }
                    else
                    {
                        Plugin.InternalNightDelivery.Value = false;
                        if (Plugin.DebugEnabled) Helpers.Log($"[Phase] Night delivery ForceDonkey returned false; will retry next frame.");
                    }
                }

                break;
            case TimeOfDay.TimeOfDayEnum.Morning:
                if (!Plugin.MorningDelivery.Value)
                {
                    break;
                }

                if (!Plugin.InternalMorningDelivery.Value)
                {
                    if (ForceDonkey(Plugin.Donkey))
                    {
                        if (Plugin.DebugEnabled) Helpers.Log($"[Phase] Morning delivery triggered successfully.");
                        Plugin.InternalMorningDelivery.Value = true;
                    }
                    else
                    {
                        Plugin.InternalMorningDelivery.Value = false;
                        if (Plugin.DebugEnabled) Helpers.Log($"[Phase] Morning delivery ForceDonkey returned false; will retry next frame.");
                    }
                }

                break;
            case TimeOfDay.TimeOfDayEnum.Day:
                if (!Plugin.DayDelivery.Value)
                {
                    return;
                }

                if (!Plugin.InternalDayDelivery.Value)
                {
                    if (ForceDonkey(Plugin.Donkey))
                    {
                        if (Plugin.DebugEnabled) Helpers.Log($"[Phase] Day delivery triggered successfully.");
                        Plugin.InternalDayDelivery.Value = true;
                    }
                    else
                    {
                        Plugin.InternalDayDelivery.Value = false;
                        if (Plugin.DebugEnabled) Helpers.Log($"[Phase] Day delivery ForceDonkey returned false; will retry next frame.");
                    }
                }

                break;
            case TimeOfDay.TimeOfDayEnum.Evening:
                if (!Plugin.EveningDelivery.Value)
                {
                    return;
                }

                if (!Plugin.InternalEveningDelivery.Value)
                {
                    if (ForceDonkey(Plugin.Donkey))
                    {
                        if (Plugin.DebugEnabled) Helpers.Log($"[Phase] Evening delivery triggered successfully.");
                        Plugin.InternalEveningDelivery.Value = true;
                    }
                    else
                    {
                        Plugin.InternalEveningDelivery.Value = false;
                        if (Plugin.DebugEnabled) Helpers.Log($"[Phase] Evening delivery ForceDonkey returned false; will retry next frame.");
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
        if (Plugin.DebugEnabled)
        {
            Helpers.Log($"[EoD] Day ending; pre-reset flags — Night={Plugin.InternalNightDelivery.Value} Morning={Plugin.InternalMorningDelivery.Value} Day={Plugin.InternalDayDelivery.Value} Evening={Plugin.InternalEveningDelivery.Value} | DonkeySpawned={Plugin.InternalDonkeySpawned.Value} Ld={(Ld == null ? "null" : "active")}");
        }
        Plugin.InternalMorningDelivery.Value = false;
        Plugin.InternalDayDelivery.Value = false;
        Plugin.InternalEveningDelivery.Value = false;
        Plugin.InternalNightDelivery.Value = false;
        Plugin.PrideDayLogged = false;
        _lastPhaseLogged = null;
        if (Plugin.DebugEnabled) Helpers.Log($"[EoD] All 4 delivery flags reset. New day begins at day={MainGame.me.save.day + 1} dow={(MainGame.me.save.day_of_week + 1) % 7}");
    }

    internal static bool ForceDonkey(WorldGameObject donkey)
    {
        if (donkey == null)
        {
            donkey = WorldMap.GetWorldGameObjectByCustomTag("donkey", true);
            if (donkey == null)
            {
                if (Plugin.DebugEnabled) Helpers.Log($"[Force] Fail — donkey WGO not found by custom_tag 'donkey'.");
                return false;
            }
        }

        if (!Helpers.TutorialDone())
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[Force] Fail — TutorialDone()==false. Reason: {Helpers.TutorialDoneReason()}");
            return false;
        }

        _strikeDone = donkey.GetParam("strike_completed") > 0f;
        if (!_strikeDone)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[Force] Fail — strike_completed==0 (pay donkey 10 carrots first).");
            return false;
        }

        _carrotBox = WorldMap.GetWorldGameObjectByCustomTag("carrot_box", true);

        if (_carrotBox == null)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[Force] Fail — no carrot_box WGO in world.");
            return false;
        }

        if (_carrotBox.data.inventory.Count > 0)
        {
            _carrotCount = _carrotBox.data.inventory[0].value;
        }
        else
        {
            _carrotCount = 0;
        }

        if (Plugin.DebugEnabled) Helpers.Log($"[Force] carrot_box inventory[0]={_carrotCount}");
        if (_carrotCount <= 0)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[Force] Fail — carrot count <=0; player needs to restock the carrot box.");
            return false;
        }

        var gameTimeNow = MainGame.game_time;
        var sinceLast = _lastDeliveryGameTime >= 0f ? gameTimeNow - _lastDeliveryGameTime : -1f;
        if (Plugin.DebugEnabled) Helpers.Log($"[Force] Triggering delivery — game_time={gameTimeNow:F4} sinceLast={(sinceLast < 0 ? "n/a" : sinceLast.ToString("F4"))} previousLd={(Ld == null ? "null" : $"active(executing={Ld._executing},started={Ld._started})")}");
        _lastDeliveryGameTime = gameTimeNow;

        _donkey = WorldMap.GetWorldGameObjectByCustomTag("donkey", true);

        Ld = new LogicData("donkey");
        var forceResult = Ld.ForceExecute(false);
        if (Plugin.DebugEnabled) Helpers.Log($"[Force] Ld.ForceExecute(false) -> {forceResult} (running_scripts={Ld._running_scripts?.Count ?? -1}, waiting_finish={Ld._waiting_finish_scripts?.Count ?? -1})");

        if (_donkey != null)
        {
            Plugin.InternalDonkeySpawned.Value = true;
            if (Plugin.DebugEnabled) Helpers.Log($"[Force] Donkey found post-force — pos={_donkey.pos3}, speed(data)={_donkey.data.GetParam("speed")}, custom_interaction_events={_donkey.custom_interaction_events?.Count ?? -1}");
            _donkey.components.character.SetSpeed(Plugin.DonkeySpeed.Value);
            if (Plugin.DebugEnabled) Helpers.Log($"[Force] Set donkey speed to target {Plugin.DonkeySpeed.Value} (data.speed now {_donkey.data.GetParam("speed")})");

            return true;
        }

        if (Plugin.DebugEnabled) Helpers.Log($"[Force] Fail — donkey disappeared after ForceExecute; returning false.");
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
            if (Plugin.DebugEnabled) Helpers.Log($"[Delivery] Body arrived — donkey heading home. pos={_donkey.pos3}, speed(data)={_donkey.data.GetParam("speed")}, target={Plugin.DonkeySpeed.Value}");
            _donkey.components.character.SetSpeed(Plugin.DonkeySpeed.Value);
        }

        _carrotBox = WorldMap.GetWorldGameObjectByCustomTag("carrot_box", true);
        if (_carrotBox != null)
        {
            if (_carrotBox.data.inventory.Count > 0)
            {
                _carrotCount = _carrotBox.data.inventory[0].value;
            }
            else
            {
                _carrotCount = 0;
            }
        }

        if (_carrotCount <= 0)
        {
            Lang.Reload();
            MainGame.me.player.Say(Lang.Get("CarrotMessage"), null, false, SpeechBubbleGUI.SpeechBubbleType.Think, SmartSpeechEngine.VoiceID.None, true);
        }

        if (Plugin.DebugEnabled) Helpers.Log($"[Delivery] session count={_deliveryCount}, carrots left={_carrotCount}, phase={TimeOfDay.me.time_of_day_enum} game_time={MainGame.game_time:F4}");
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
                if (Plugin.DebugEnabled) Helpers.Log($"[Logic] Allowing our own Ld to Execute (id=donkey, started={__instance._started}, executing={__instance._executing})");
                return true;
            }

            if (Plugin.DebugEnabled) Helpers.Log($"[Logic] Blocking vanilla donkey LogicData.Execute (started={__instance._started}, executing={__instance._executing}, next_exec={__instance._next_execution_time:F4}, game_time={MainGame.game_time:F4})");
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
            if (Plugin.DebugEnabled) Helpers.Log($"[Home] Donkey teleported to GD point — trip complete. phase={TimeOfDay.me.time_of_day_enum} game_time={MainGame.game_time:F4}");
            Plugin.InternalDonkeySpawned.Value = false;
            Ld = null;
        }
    }
}
