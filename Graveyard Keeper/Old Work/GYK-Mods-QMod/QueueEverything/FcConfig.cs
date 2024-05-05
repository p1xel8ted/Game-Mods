using System;
using System.Globalization;
using UnityEngine.Serialization;

namespace QueueEverything
{
    public static class FcConfig
    {
        private static Options _options;
        private static FcConfigReader _con;

        [Serializable]
        public class Options
        {
            [FormerlySerializedAs("CraftSpeedMultiplier")] public float craftSpeedMultiplier;
        }

        public static Options GetOptions(bool external = false)
        {
            _options = new Options();
            _con = new FcConfigReader(external);
            float.TryParse(_con.Value("CraftSpeedMultiplier", "2"), NumberStyles.Float, CultureInfo.InvariantCulture, out var craftSpeedMultiplier);
            _options.craftSpeedMultiplier = craftSpeedMultiplier;

            _con.ConfigWrite();

            return _options;
        }
    }
}