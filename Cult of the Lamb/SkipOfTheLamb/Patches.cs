namespace SkipOfTheLamb;

[Harmony]
public static class Patches
{
    private static bool _hasSkippedDevIntros;
    
    //SkipCrownVideo = the video that plays when the player first dies and is crowned by the death cat
    //SkipDevIntros = the developer intros that play on game start before the main menu
    //SkipMiniBossIntros = the intros that play when encountering mini-bosses for the first time
    //SkipBossIntros = the intros that play when encountering main bosses for the first time
    //IgnoreFirstEncounter = when skipping boss intros, ignore the first encounter and only skip subsequent ones
    //SkipWoolhavenInGameFirstArrivalScene = the in-game cutscene that plays when the player first arrives in woolhaven
    //SkipWoolhavenPrerenderedCinematic = the prerended video after the user completes the woolhaven intro dungeon

    #region Existing Patches

    [HarmonyPrefix]
    [HarmonyPatch(typeof(IntroDeathSceneManager), nameof(IntroDeathSceneManager.GiveCrown))]
    public static bool IntroDeathSceneManager_GiveCrown(ref IntroDeathSceneManager __instance)
    {
        if (!Plugin.SkipCrownVideo.Value) return true;

        Plugin.Log.LogInfo("[IntroDeathSceneManager.GiveCrown]: Skipping crown video");
        __instance.VideoComplete();
        DataManager.Instance.HadInitialDeathCatConversation = true;
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(LoadMainMenu), nameof(LoadMainMenu.Start))]
    public static bool LoadMainMenu_Start()
    {
        if (_hasSkippedDevIntros || !Plugin.SkipDevIntros.Value) return true;

        Plugin.Log.LogInfo("[LoadMainMenu.Start]: Skipping dev intros");
        _hasSkippedDevIntros = true;

        AudioManager.Instance.enabled = true;
        MMTransition.Play(
            MMTransition.TransitionType.ChangeSceneAutoResume,
            MMTransition.Effect.BlackFade,
            "Main Menu",
            0f,
            "",
            null
        );

        return false;
    }

    #endregion

    #region DLC Intro Skips

    [HarmonyPrefix]
    [HarmonyPatch(typeof(WoolhavenYngyaStatue), nameof(WoolhavenYngyaStatue.PlayWoolhavenIntro))]
    public static bool WoolhavenYngyaStatue_PlayWoolhavenIntro()
    {
        if (!Plugin.SkipWoolhavenInGameFirstArrivalScene.Value) return true;

        Plugin.Log.LogInfo("[WoolhavenYngyaStatue.PlayWoolhavenIntro]: Skipping first-arrival cutscene");

        DataManager.Instance.DiscoverLocation(FollowerLocation.LambTown);
        DataManager.Instance.VisitedLocations.Add(FollowerLocation.LambTown);
        DataManager.Instance.OnboardedLambTown = true;

        ObjectiveManager.Add(
            new Objectives_Custom("Objectives/GroupTitles/GoToDLCDungeonFirstTime", Objectives.CustomQuestTypes.GoToDLCDungeonFirstTime),
            true, true);

        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MMTransition), nameof(MMTransition.Play), typeof(MMTransition.TransitionType), typeof(MMTransition.Effect), typeof(string), typeof(float), typeof(string), typeof(System.Action), typeof(System.Action))]
    public static void MMTransition_Play(ref string SceneToLoad)
    {
        var stackTrace = new StackTrace();
        var callers = string.Join(" <- ", stackTrace.GetFrames()
            .Skip(1)
            .Take(3)
            .Select(f => $"{f.GetMethod().DeclaringType?.Name}.{f.GetMethod().Name}"));

        Plugin.Log.LogInfo($"[MMTransition.Play]: '{SceneToLoad}' | Callers: {callers}");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MMVideoPlayer), nameof(MMVideoPlayer.Play), typeof(string), typeof(System.Action), typeof(MMVideoPlayer.Options), typeof(MMVideoPlayer.Options), typeof(bool), typeof(bool))]
    public static void MMVideoPlayer_Play(string _FileName)
    {
        var stackTrace = new StackTrace();
        var callers = string.Join(" <- ", stackTrace.GetFrames()
            .Skip(1)
            .Take(3)
            .Select(f => $"{f.GetMethod().DeclaringType?.Name}.{f.GetMethod().Name}"));

        Plugin.Log.LogInfo($"[MMVideoPlayer.Play]: '{_FileName}' | Callers: {callers}");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BiomeGenerator), nameof(BiomeGenerator.PlayDLCVideoIE))]
    public static bool BiomeGenerator_PlayDLCVideoIE(ref IEnumerator __result)
    {
        if (!Plugin.SkipWoolhavenPrerenderedCinematic.Value) return true;

        Plugin.Log.LogInfo("[BiomeGenerator.PlayDLCVideoIE]: Skipping coroutine");
        __result = SkipWoolhavenPrerenderedCinematic();
        return false;
    }

    private static IEnumerator SkipWoolhavenPrerenderedCinematic()
    {
        SeasonsManager.ActivateSeasons();
        DataManager.Instance.OnboardedWolf = true;
        DataManager.Instance.ForeshadowedWolf = true;
        DataManager.Instance.RevealDLCDungeonNode = true;
        DataManager.Instance.SpokeToYngyaOnMountainTop = true;
        AudioManager.Instance.StartMusic();
        PlayerFarming.PositionAllPlayers(new Vector3(-0.325f, -4f, 0.0f));
        GameManager.GetInstance().OnConversationEnd();

        var white = Color.white;
        white.a = 0.65f;
        WeatherSystemController.Instance.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Extreme, 0.0f);
        WeatherSystemController.Instance.ShowBlizzardOverlay(white, 0.0f);

        yield break;
    }

    #endregion

    #region Mini-Boss Intro Skip

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MiniBossController), nameof(MiniBossController.Play))]
    public static void MiniBossController_Play(ref bool skipped)
    {
        if (!Plugin.SkipMiniBossIntros.Value) return;
        Plugin.Log.LogWarning("[MiniBossController.Play]: Skipping intro");
        skipped = true;
    }

    #endregion

    #region Yngya Boss Intro Skip

    [HarmonyPostfix]
    [HarmonyPatch(typeof(EnemyYngyaIntro), nameof(EnemyYngyaIntro.Start))]
    public static void EnemyYngyaIntro_Start(EnemyYngyaIntro __instance)
    {
        if (!Plugin.SkipBossIntros.Value) return;
        if (!Plugin.IgnoreFirstEncounter.Value && !DataManager.Instance.DiedToYngyaBoss) return;

        Plugin.Log.LogInfo("[EnemyYngyaIntro.Start]: Skipping Yngya boss intro");
        __instance.skippable = true;
        __instance.skipped = true;
        __instance.yngyaBoss.SkipIntro();
    }

    #endregion

    #region Boss Ritual Intro Skips

    // Helper that replaces GetAttackButtonDown() in Update() - returns true to trigger game's built-in skip
    public static bool ShouldSkipBossIntro(RewiredGameplayInputSource gameplay, PlayerFarming player)
    {
        return Plugin.SkipBossIntros.Value || gameplay.GetAttackButtonDown(player);
    }

    private static readonly MethodInfo GetAttackButtonDownMethod = typeof(RewiredGameplayInputSource).GetMethod(nameof(RewiredGameplayInputSource.GetAttackButtonDown));
    private static readonly MethodInfo ShouldSkipBossIntroMethod = typeof(Patches).GetMethod(nameof(ShouldSkipBossIntro), BindingFlags.Public | BindingFlags.Static);

    // Worm (Leshy) boss ritual
    [HarmonyPostfix]
    [HarmonyPatch(typeof(WormBossIntroRitual), nameof(WormBossIntroRitual.OnTriggerEnter2D))]
    public static void WormBossIntroRitual_OnTriggerEnter2D_Postfix(WormBossIntroRitual __instance)
    {
        if (!Plugin.SkipBossIntros.Value) return;
        if (__instance.healingLeshy) return;
        if (!Plugin.IgnoreFirstEncounter.Value && !DataManager.Instance.BossesEncountered.Contains(PlayerFarming.Location)) return;
        __instance.skippable = true;
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(WormBossIntroRitual), nameof(WormBossIntroRitual.Update))]
    public static IEnumerable<CodeInstruction> WormBossIntroRitual_Update_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (var instruction in instructions)
        {
            if (instruction.Calls(GetAttackButtonDownMethod))
            {
                yield return new CodeInstruction(OpCodes.Call, ShouldSkipBossIntroMethod);
            }
            else
            {
                yield return instruction;
            }
        }
    }

    // Frog (Heket) boss ritual
    [HarmonyPostfix]
    [HarmonyPatch(typeof(FrogBossIntroRitual), nameof(FrogBossIntroRitual.OnTriggerEnter2D))]
    public static void FrogBossIntroRitual_OnTriggerEnter2D_Postfix(FrogBossIntroRitual __instance)
    {
        if (!Plugin.SkipBossIntros.Value) return;
        if (__instance.healingHeket) return;
        if (!Plugin.IgnoreFirstEncounter.Value && !DataManager.Instance.BossesEncountered.Contains(PlayerFarming.Location)) return;
        __instance.skippable = true;
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(FrogBossIntroRitual), nameof(FrogBossIntroRitual.Update))]
    public static IEnumerable<CodeInstruction> FrogBossIntroRitual_Update_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (var instruction in instructions)
        {
            if (instruction.Calls(GetAttackButtonDownMethod))
            {
                yield return new CodeInstruction(OpCodes.Call, ShouldSkipBossIntroMethod);
            }
            else
            {
                yield return instruction;
            }
        }
    }

    // Jelly (Kallamar) boss ritual
    [HarmonyPostfix]
    [HarmonyPatch(typeof(JellyBossIntroRitual), nameof(JellyBossIntroRitual.OnTriggerEnter2D))]
    public static void JellyBossIntroRitual_OnTriggerEnter2D_Postfix(JellyBossIntroRitual __instance)
    {
        if (!Plugin.SkipBossIntros.Value) return;
        if (__instance.healingKallamar) return;
        if (!Plugin.IgnoreFirstEncounter.Value && !DataManager.Instance.BossesEncountered.Contains(PlayerFarming.Location)) return;
        __instance.skippable = true;
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(JellyBossIntroRitual), nameof(JellyBossIntroRitual.Update))]
    public static IEnumerable<CodeInstruction> JellyBossIntroRitual_Update_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (var instruction in instructions)
        {
            if (instruction.Calls(GetAttackButtonDownMethod))
            {
                yield return new CodeInstruction(OpCodes.Call, ShouldSkipBossIntroMethod);
            }
            else
            {
                yield return instruction;
            }
        }
    }

    // Spider (Shamura) boss ritual
    [HarmonyPostfix]
    [HarmonyPatch(typeof(SpiderBossIntroRitual), nameof(SpiderBossIntroRitual.OnTriggerEnter2D))]
    public static void SpiderBossIntroRitual_OnTriggerEnter2D_Postfix(SpiderBossIntroRitual __instance)
    {
        if (!Plugin.SkipBossIntros.Value) return;
        if (__instance.healingShamura) return;
        if (!Plugin.IgnoreFirstEncounter.Value && !DataManager.Instance.BossesEncountered.Contains(PlayerFarming.Location)) return;
        __instance.skippable = true;
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(SpiderBossIntroRitual), nameof(SpiderBossIntroRitual.Update))]
    public static IEnumerable<CodeInstruction> SpiderBossIntroRitual_Update_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (var instruction in instructions)
        {
            if (instruction.Calls(GetAttackButtonDownMethod))
            {
                yield return new CodeInstruction(OpCodes.Call, ShouldSkipBossIntroMethod);
            }
            else
            {
                yield return instruction;
            }
        }
    }

    #endregion
}
