namespace WheresMaPoints;

[Harmony]
public static class Patches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(TechPointsSpawner), nameof(TechPointsSpawner.SpawnTechPoint))]
    public static bool TechPointsSpawner_SpawnTechPoint(TechPointsSpawner __instance, TechPointsSpawner.Type type)
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

        if (Plugin.ShowPointGainAboveKeeper.Value && !(MainGame.me.player.is_dead || !GS.IsPlayerEnable()) && BaseGUI.all_guis_closed)
        {
            EffectBubblesManager.ShowStacked(MainGame.me.player, new GameRes(type, 1));
        }

        if (Plugin.StillPlayCollectAudio.Value && !(MainGame.me.player.is_dead || !GS.IsPlayerEnable()))
        {
            MasterAudio.PlaySound("pickup", 1f, null, 0f, "pickup1");
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(AnimatedGUIPanel), nameof(AnimatedGUIPanel.Update))]
    public static bool AnimatedGUIPanel_Update(AnimatedGUIPanel __instance)
    {
        if (__instance is not HUDTechPointsBar) return true;
        if (!Plugin.AlwaysShowXpBar.Value) return true;
        if (!MainGame.game_started) return true;
        __instance.SetVisible(true);
        __instance.Redraw();
        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AnimatedGUIPanel), nameof(AnimatedGUIPanel.Init))]
    public static void AnimatedGUIPanel_Init(AnimatedGUIPanel __instance)
    {
        if (__instance is not HUDTechPointsBar) return;
        if (!Plugin.AlwaysShowXpBar.Value) return;
        __instance.SetVisible(true);
    }
}