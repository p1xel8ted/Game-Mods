using HarmonyLib;
using System;
using System.Reflection;

namespace BringOutYerDead;

[HarmonyPatch]
public static partial class MainPatcher
{
    // private const float MadnessSeconds = 1350f; //3 bodies in total
    // private const float EvenLongerSeconds = 1125f; //2 bodies in total
    // private const float DoubleLengthSeconds = 900f; //2 bodies in total
    // private const float DefaultIncreaseSeconds = 675f; //1 body in total
    private static Config.Options _cfg;
    private static SaveData.Data _sd;

    public static void Patch()
    {
        try
        {
            _cfg = Config.GetOptions();
            _sd = SaveData.GetData();

            var harmony = new Harmony("p1xel8ted.GraveyardKeeper.BringOutYerDead");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }
    }
}