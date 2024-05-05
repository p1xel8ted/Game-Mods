using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Serialization;

namespace RegenerationReloaded
{
    public static class Config
    {
        private static Options _options;
        private static ConfigReader _con;

        [Serializable]
        public class Options
        {
            [FormerlySerializedAs("Debug")] public bool debug;

            [FormerlySerializedAs("ShowRegenUpdates")]
            public bool showRegenUpdates = true;

            [FormerlySerializedAs("LifeRegen")] public float lifeRegen = 2f;
            [FormerlySerializedAs("EnergyRegen")] public float energyRegen = 1f;
            [FormerlySerializedAs("RegenDelay")] public float regenDelay = 5f;

            [FormerlySerializedAs("ReloadConfigKeyBind")]
            public KeyCode reloadConfigKeyBind;
        }

        public static Options GetOptions(bool external = false)
        {
            _options = new Options();
            _con = new ConfigReader(external);

            var key = Enum.TryParse<KeyCode>(_con.Value("ReloadConfigKeyBind", "F5"), true, out var b);
            if (key)
            {
                _options.reloadConfigKeyBind = b;
                if (!external)
                {
                    Debug.LogWarning($"[RegenerationReloaded]: Parsed '{b}' for 'ReloadConfigKeyBind'.");
                }
            }
            else
            {
                _options.reloadConfigKeyBind = KeyCode.F5;
                if (!external)
                {
                    Debug.LogWarning($"[RegenerationReloaded]: Failed to parse key for 'ReloadConfigKeyBind'. Setting to default F5.");
                }
            }

            bool.TryParse(_con.Value("Debug", "false"), out var debug);
            _options.debug = debug;

            bool.TryParse(_con.Value("ShowRegenUpdates", "true"), out var showRegenUpdates);
            _options.showRegenUpdates = showRegenUpdates;

            float.TryParse(_con.Value("LifeRegen", "2"), NumberStyles.Number, CultureInfo.InvariantCulture, out var lifeRegen);
            _options.lifeRegen = lifeRegen;

            float.TryParse(_con.Value("EnergyRegen", "1"), NumberStyles.Number, CultureInfo.InvariantCulture, out var energyRegen);
            _options.energyRegen = energyRegen;

            float.TryParse(_con.Value("RegenDelay", "5"), NumberStyles.Number, CultureInfo.InvariantCulture, out var regenDelay);
            _options.regenDelay = regenDelay;

            _con.ConfigWrite();

            return _options;
        }
    }
}