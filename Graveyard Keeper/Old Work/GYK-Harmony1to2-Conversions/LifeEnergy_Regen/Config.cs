using LifeEnergy_Regen.Ini;
using System.IO;

namespace LifeEnergy_Regen
{
    public static class Config
    {
        public static float EnergyRegen;
        public static bool IsStandingStill;
        public static float LifeRegen;
        public static float RegenDelay;
        public static string ReloadKey;

        public static void Load()
        {
            var iniFile = new IniFile(Path.Combine("QMods", "LifeEnergy_Regen", "config.ini"));
            EnergyRegen = (float)iniFile.GetDouble("Main", "EnergyRegenMultiplier", 1.0);
            LifeRegen = (float)iniFile.GetDouble("Main", "LifeRegenMultiplier", 1.0);
            RegenDelay = (float)iniFile.GetDouble("Delays", "RegenDelaySeconds", 5.0);
            ReloadKey = iniFile.GetValue("KeyBinds", "ReloadConfig", "F5");
        }
    }
}