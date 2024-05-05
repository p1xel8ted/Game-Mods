using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Serialization;

namespace FasterCraftReloaded;

public static class Config
{
    private static Options _options;
    private static ConfigReader _con;

   
    public class Options
    {
        public bool IncreaseBuildAndDestroySpeed;
        public float CraftSpeedMultiplier;

        public bool ModifyPlayerGardenSpeed;

        public float PlayerGardenSpeedMultiplier;

        public bool ModifyZombieGardenSpeed;

        public float ZombieGardenSpeedMultiplier;

        public bool ModifyRefugeeGardenSpeed;

        public float RefugeeGardenSpeedMultiplier;

        public bool ModifyZombieVineyardSpeed;

        public float ZombieVineyardSpeedMultiplier;

        public bool ModifyZombieSawmillSpeed;

        public float ZombieSawmillSpeedMultiplier;

        public bool ModifyZombieMinesSpeed;

        public float ZombieMinesSpeedMultiplier;

        public bool ModifyCompostSpeed;

        public float CompostSpeedMultiplier;

        public KeyCode ReloadConfigKeyBind;

        public bool Debug;
    }

    public static Options GetOptions(bool external = false)
    {
        _options = new Options();
        _con = new ConfigReader(external);

        bool.TryParse(_con.Value("Debug", "false"), out var debug);
        _options.Debug = debug;

        bool.TryParse(_con.Value("IncreaseBuildAndDestroySpeed", "true"), out var increaseBuildAndDestroySpeed);
        _options.IncreaseBuildAndDestroySpeed = increaseBuildAndDestroySpeed;
        
        float.TryParse(_con.Value("CraftSpeedMultiplier", "2"), NumberStyles.Number, CultureInfo.InvariantCulture, out var craftSpeedMultiplier);
        _options.CraftSpeedMultiplier = craftSpeedMultiplier;

        bool.TryParse(_con.Value("ModifyPlayerGardenSpeed", "false"), out var modifyPlayerGardenSpeed);
        _options.ModifyPlayerGardenSpeed = modifyPlayerGardenSpeed;

        float.TryParse(_con.Value("PlayerGardenSpeedMultiplier", "2"), NumberStyles.Number, CultureInfo.InvariantCulture, out var playerGardenSpeedMultiplier);
        _options.PlayerGardenSpeedMultiplier = playerGardenSpeedMultiplier;

        bool.TryParse(_con.Value("ModifyZombieGardenSpeed", "false"), out var modifyZombieGardenSpeed);
        _options.ModifyZombieGardenSpeed = modifyZombieGardenSpeed;

        float.TryParse(_con.Value("ZombieGardenSpeedMultiplier", "2"), NumberStyles.Number, CultureInfo.InvariantCulture, out var zombieGardenSpeedMultiplier);
        _options.ZombieGardenSpeedMultiplier = zombieGardenSpeedMultiplier;

        bool.TryParse(_con.Value("ModifyRefugeeGardenSpeed", "false"), out var modifyRefugeeGardenSpeed);
        _options.ModifyRefugeeGardenSpeed = modifyRefugeeGardenSpeed;

        float.TryParse(_con.Value("RefugeeGardenSpeedMultiplier", "2"), NumberStyles.Number, CultureInfo.InvariantCulture, out var refugeeGardenSpeedMultiplier);
        _options.RefugeeGardenSpeedMultiplier = refugeeGardenSpeedMultiplier;

        bool.TryParse(_con.Value("ModifyZombieVineyardSpeed", "false"), out var modifyZombieVineyardSpeed);
        _options.ModifyZombieVineyardSpeed = modifyZombieVineyardSpeed;

        float.TryParse(_con.Value("ZombieVineyardSpeedMultiplier", "2"), NumberStyles.Number, CultureInfo.InvariantCulture, out var zombieVineyardSpeedMultiplier);
        _options.ZombieVineyardSpeedMultiplier = zombieVineyardSpeedMultiplier;

        bool.TryParse(_con.Value("ModifyZombieSawmillSpeed", "false"), out var modifyZombieSawmillSpeed);
        _options.ModifyZombieSawmillSpeed = modifyZombieSawmillSpeed;

        float.TryParse(_con.Value("ZombieSawmillSpeedMultiplier", "2"), NumberStyles.Number, CultureInfo.InvariantCulture, out var zombieSawmillSpeedMultiplier);
        _options.ZombieSawmillSpeedMultiplier = zombieSawmillSpeedMultiplier;

        bool.TryParse(_con.Value("ModifyZombieMinesSpeed", "false"), out var modifyZombieMinesSpeed);
        _options.ModifyZombieMinesSpeed = modifyZombieMinesSpeed;

        float.TryParse(_con.Value("ZombieMinesSpeedMultiplier", "2"), NumberStyles.Number, CultureInfo.InvariantCulture, out var zombieMinesSpeedMultiplier);
        _options.ZombieMinesSpeedMultiplier = zombieMinesSpeedMultiplier;

        bool.TryParse(_con.Value("ModifyCompostSpeed", "false"), out var modifyCompostSpeed);
        _options.ModifyCompostSpeed = modifyCompostSpeed;

        float.TryParse(_con.Value("CompostSpeedMultiplier", "2"), NumberStyles.Number, CultureInfo.InvariantCulture, out var compostSpeedMultiplier);
        _options.CompostSpeedMultiplier = compostSpeedMultiplier;

        var key = Enum.TryParse<KeyCode>(_con.Value("ReloadConfigKeyBind", "F5"), true, out var b);
        if (key)
        {
            _options.ReloadConfigKeyBind = b;
            if (!external)
            {
                Debug.LogWarning($"[FasterCraftReloaded]: Parsed '{b}' for 'ReloadConfigKeyBind'.");
            }
        }
        else
        {
            _options.ReloadConfigKeyBind = KeyCode.F5;
            if (!external)
            {
                Debug.LogWarning($"[FasterCraftReloaded]: Failed to parse key for 'ReloadConfigKeyBind'. Setting to default F5.");
            }
        }

        _con.ConfigWrite();

        return _options;
    }
}