namespace WheresMaPoints;

[Harmony]
public static class Patches
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

        if (Plugin.ShowPointGainAboveKeeper.Value && !Tools.PlayerDisabled() && BaseGUI.all_guis_closed)
        {
            EffectBubblesManager.ShowStacked(MainGame.me.player, new GameRes(type, 1));
        }

        if (Plugin.StillPlayCollectAudio.Value && !Tools.PlayerDisabled())
        {
            MasterAudio.PlaySound("pickup", 1f, null, 0f, "pickup1");
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(AnimatedGUIPanel), nameof(AnimatedGUIPanel.Update))]
    public static bool AnimatedGUIPanel_Update(ref AnimatedGUIPanel __instance)
    {
        if (!Plugin.AlwaysShowXpBar.Value) return true;
        if (!MainGame.game_started) return true;
        __instance.SetVisible(true);
        __instance.Redraw();
        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AnimatedGUIPanel), nameof(AnimatedGUIPanel.Init))]
    public static void AnimatedGUIPanel_Init(ref AnimatedGUIPanel __instance)
    {
        if (!Plugin.AlwaysShowXpBar.Value) return;
        __instance.SetVisible(true);
    }
}