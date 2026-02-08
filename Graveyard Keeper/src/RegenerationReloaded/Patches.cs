namespace RegenerationReloaded;

[Harmony]
public static class Patches
{
    internal static float EnergyRegen { get; set; }
    internal static float LifeRegen { get; set; }
    internal static bool ShowRegenUpdates { get; set; }
    internal static float RegenDelay { get; set; }
    private static float Time { get; set; }
    private static WorldGameObject Player { get; set; }
    private static GameSave Save { get; set; }

    static Patches()
    {
        EnergyRegen = Plugin.EnergyRegen.Value;
        LifeRegen = Plugin.LifeRegen.Value;
        ShowRegenUpdates = Plugin.ShowRegenUpdates.Value;
        RegenDelay = Plugin.RegenDelay.Value;
        Time = UnityEngine.Time.time;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerComponent), nameof(PlayerComponent.Update))]
    public static void PlayerComponent_Update()
    {
        if (Player == null || Save == null)
        {
            Player = MainGame.me.player;
            Save = MainGame.me.save;
        }

        if (!(UnityEngine.Time.time > Time + RegenDelay)) return;
        if (Player.energy < Save.max_energy)
        {
            Player.energy += EnergyRegen;
            if (ShowRegenUpdates)
            {
                EffectBubblesManager.ShowStackedEnergy(Player, EnergyRegen);
            }
            if (Player.energy > Save.max_energy)
            {
                Player.energy = Save.max_energy;
            }
        }
        else if (Player.hp < Save.max_hp)
        {
            Player.hp += LifeRegen;
            if (ShowRegenUpdates)
            {
                EffectBubblesManager.ShowStackedHP(Player, LifeRegen);
            }
            if (Player.hp > Save.max_hp)
            {
                Player.hp = Save.max_hp;
            }
        }
        Time = UnityEngine.Time.time;
    }
}