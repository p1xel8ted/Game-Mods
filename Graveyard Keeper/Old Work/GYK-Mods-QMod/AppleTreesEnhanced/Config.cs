using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace AppleTreesEnhanced;

public static class Config
{
    private static Options _options;
    private static ConfigReader _con;

    public static Options GetOptions(bool external = false)
    {
        _options = new Options();
        _con = new ConfigReader(external);
        
        bool.TryParse(_con.Value("ShowHarvestReadyMessages", "true"), out var showHarvestReadyMessages);
        _options.ShowHarvestReadyMessages = showHarvestReadyMessages;

        bool.TryParse(_con.Value("IncludeGardenBerryBushes", "true"), out var includeGardenBerryBushes);
        _options.IncludeGardenBerryBushes = includeGardenBerryBushes;

        bool.TryParse(_con.Value("IncludeWorldBerryBushes", "false"), out var includeWorldBerryBushes);
        _options.IncludeWorldBerryBushes = includeWorldBerryBushes;

        bool.TryParse(_con.Value("IncludeGardenTrees", "true"), out var includeGardenTrees);
        _options.IncludeGardenTrees = includeGardenTrees;

        bool.TryParse(_con.Value("RealisticHarvest", "true"), out var realisticHarvest);
        _options.RealisticHarvest = realisticHarvest;

        bool.TryParse(_con.Value("IncludeGardenBeeHives", "true"), out var includeGardenBeeHives);
        _options.IncludeGardenBeeHives = includeGardenBeeHives;

        bool.TryParse(_con.Value("BeeKeeperBuyback", "true"), out var beeKeeperBuyback);
        _options.BeeKeeperBuyback = beeKeeperBuyback;
        
        bool.TryParse(_con.Value("BoostGrowSpeedWhenRaining", "true"), out var boostGrowSpeedWhenRaining);
        _options.BoostGrowSpeedWhenRaining = boostGrowSpeedWhenRaining;
        
        bool.TryParse(_con.Value("Debug", "true"), out var debug);
        _options.Debug = debug;

        var key = Enum.TryParse<KeyCode>(_con.Value("ReloadConfigKeyBind", "F5"), true, out var b);
        if (key)
        {
            _options.ReloadConfigKeyBind = b;
            if (!external)
            {
                Debug.LogWarning($"[AppleTreesEnhanced]: Parsed '{b}' for 'ReloadConfigKeyBind'.");
            }
        }
        else
        {
            _options.ReloadConfigKeyBind = KeyCode.F5;
            if (!external)
            {
                Debug.LogWarning($"[AppleTreesEnhanced]: Failed to parse key for 'ReloadConfigKeyBind'. Setting to default F5.");
            }
        }

        _con.ConfigWrite();

        return _options;
    }

 
    public class Options
    {
        public bool IncludeGardenBerryBushes;

        public bool IncludeGardenTrees;

        public bool IncludeWorldBerryBushes;

        public bool ShowHarvestReadyMessages;

        public bool RealisticHarvest;

        public bool IncludeGardenBeeHives;

        public bool BeeKeeperBuyback;
        public bool BoostGrowSpeedWhenRaining;
        public KeyCode ReloadConfigKeyBind;
        public bool Debug;
    }
}