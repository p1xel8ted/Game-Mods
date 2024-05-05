using HarmonyLib;
using System;
using System.Reflection;

namespace AddStraightToTable;

public static partial class MainPatcher
{
    private const string WheresMaStorageId = "WheresMaStorage";
    private const string WheresMaStorageFileName = "WheresMaStorage.dll";
    private const string WheresMaStorageName = "Where's Ma' Storage!";
    private static Config.Options _cfg;
    private static bool _wms;

    public static void Patch()
    {
        try
        {
            var harmony = new Harmony("p1xel8ted.GraveyardKeeper.AddStraightToTable");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }
    }
}