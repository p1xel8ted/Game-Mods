using System;
using UnityEngine;

namespace TheSeedEqualizer;

public static class Config
{
    private static Options _options;
    private static ConfigReader _con;

    public static Options GetOptions(bool external = false)
    {
        _options = new Options();
        _con = new ConfigReader(external);

        bool.TryParse(_con.Value("ModifyZombieGardens", "true"), out var modifyZombieGardens);
        _options.ModifyZombieGardens = modifyZombieGardens;

        bool.TryParse(_con.Value("ModifyZombieVineyards", "true"), out var modifyZombieVineyards);
        _options.ModifyZombieVineyards = modifyZombieVineyards;

        bool.TryParse(_con.Value("ModifyPlayerGardens", "false"), out var modifyPlayerGardens);
        _options.ModifyPlayerGardens = modifyPlayerGardens;

        bool.TryParse(_con.Value("ModifyRefugeeGardens", "true"), out var modifyRefugeeGardens);
        _options.ModifyRefugeeGardens = modifyRefugeeGardens;

        bool.TryParse(_con.Value("AddWasteToZombieGardens", "true"), out var addWasteToZombieGardens);
        _options.AddWasteToZombieGardens = addWasteToZombieGardens;

        bool.TryParse(_con.Value("AddWasteToZombieVineyards", "true"), out var addWasteToZombieVineyards);
        _options.AddWasteToZombieVineyards = addWasteToZombieVineyards;

        bool.TryParse(_con.Value("BoostPotentialSeedOutput", "true"), out var boostPotentialSeedOutput);
        _options.BoostPotentialSeedOutput = boostPotentialSeedOutput;
        
        bool.TryParse(_con.Value("BoostGrowSpeedWhenRaining", "true"), out var boostGrowSpeedWhenRaining);
        _options.BoostGrowSpeedWhenRaining = boostGrowSpeedWhenRaining;

        bool.TryParse(_con.Value("Debug", "false"), out var debug);
        _options.Debug = debug;
        
        var key = Enum.TryParse<KeyCode>(_con.Value("ReloadConfigKeyBind", "F5"), true, out var b);
        if (key)
        {
            _options.ReloadConfigKeyBind = b;
            if (!external)
            {
                Debug.LogWarning($"[TheSeedEqualizer]: Parsed '{b}' for 'ReloadConfigKeyBind'.");
            }
        }
        else
        {
            _options.ReloadConfigKeyBind = KeyCode.F5;
            if (!external)
            {
                Debug.LogWarning($"[TheSeedEqualizer]: Failed to parse key for 'ReloadConfigKeyBind'. Setting to default F5.");
            }
        }

        _con.ConfigWrite();

        return _options;
    }


    public class Options
    {
        public KeyCode ReloadConfigKeyBind;
        public bool ModifyZombieGardens;
        public bool ModifyZombieVineyards;
        public bool ModifyPlayerGardens;
        public bool ModifyRefugeeGardens;
        public bool AddWasteToZombieGardens;
        public bool AddWasteToZombieVineyards;
        public bool BoostGrowSpeedWhenRaining;
        public bool BoostPotentialSeedOutput;
        public bool Debug;
    }
}