using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace QueueEverything;

public static class Config
{
    private static ConfigReader _con;
    private static Options _options;

    public static Options GetOptions(bool external = false)
    {
        _options = new Options();
        _con = new ConfigReader(external);

        bool.TryParse(_con.Value("HalfFireRequirements", "true"), out var halfFireRequirements);
        _options.halfFireRequirements = halfFireRequirements;

        bool.TryParse(_con.Value("AutoMaxMultiQualCrafts", "true"), out var autoMaxMultiQualCrafts);
        _options.autoMaxMultiQualCrafts = autoMaxMultiQualCrafts;

        bool.TryParse(_con.Value("AutoMaxNormalCrafts", "false"), out var autoMaxNormalCrafts);
        _options.autoMaxNormalCrafts = autoMaxNormalCrafts;

        bool.TryParse(_con.Value("AutoSelectHighestQualRecipe", "true"), out var autoSelectHighestQualRecipe);
        _options.autoSelectHighestQualRecipe = autoSelectHighestQualRecipe;

        bool.TryParse(_con.Value("AutoSelectCraftButtonWithController", "true"),
            out var autoSelectCraftButtonWithController);
        _options.autoSelectCraftButtonWithController = autoSelectCraftButtonWithController;

        bool.TryParse(_con.Value("MakeEverythingAuto", "true"),
            out var makeEverythingAuto);
        _options.makeEverythingAuto = makeEverythingAuto;

        bool.TryParse(_con.Value("MakeHandTasksAuto", "false"),
            out var makeHandTasksAuto);
        _options.makeHandTasksAuto = makeHandTasksAuto;

        bool.TryParse(_con.Value("DisableComeBackLaterThoughts", "false"),
            out var disableComeBackLaterThoughts);
        _options.disableComeBackLaterThoughts = disableComeBackLaterThoughts;

        bool.TryParse(_con.Value("ForceMultiCraft", "true"),
            out var forceMultiCraft);
        _options.forceMultiCraft = forceMultiCraft;

        bool.TryParse(_con.Value("Debug", "false"), out var debug);
        _options.debug = debug;

        var key = Enum.TryParse<KeyCode>(_con.Value("ReloadConfigKeyBind", "F5"), true, out var b);
        if (key)
        {
            _options.reloadConfigKeyBind = b;
            if (!external)
            {
                Debug.LogWarning($"[QueueEverything]: Parsed '{b}' for 'ReloadConfigKeyBind'.");
            }
        }
        else
        {
            _options.reloadConfigKeyBind = KeyCode.F5;
            if (!external)
            {
                Debug.LogWarning($"[QueueEverything]: Failed to parse key for 'ReloadConfigKeyBind'. Setting to default F5.");
            }
        }

        _con.ConfigWrite();

        return _options;
    }

    [Serializable]
    public class Options
    {
        [FormerlySerializedAs("ReloadConfigKeyBind")]
        public KeyCode reloadConfigKeyBind;

        [FormerlySerializedAs("AutoMaxMultiQualCrafts")]
        public bool autoMaxMultiQualCrafts;

        [FormerlySerializedAs("AutoMaxNormalCrafts")]
        public bool autoMaxNormalCrafts;

        [FormerlySerializedAs("AutoSelectCraftButtonWithController")]
        public bool autoSelectCraftButtonWithController;

        [FormerlySerializedAs("AutoSelectHighestQualRecipe")]
        public bool autoSelectHighestQualRecipe;

        [FormerlySerializedAs("HalfFireRequirements")]
        public bool halfFireRequirements;

        [FormerlySerializedAs("MakeEverythingAuto")]
        public bool makeEverythingAuto;

        [FormerlySerializedAs("MakeHandTasksAuto")]
        public bool makeHandTasksAuto;

        [FormerlySerializedAs("DisableComeBackLaterThoughts")]
        public bool disableComeBackLaterThoughts;

        [FormerlySerializedAs("ForceMultiCraft")]
        public bool forceMultiCraft;

        [FormerlySerializedAs("Debug")] public bool debug;
    }
}