using HarmonyLib;
using Helper;
using System;
using System.Reflection;

namespace NoTimeForFishing;

[HarmonyPatch]
public static partial class MainPatcher
{
  public static void Patch()
    {
        try
        {
           
            var harmony = new Harmony("p1xel8ted.GraveyardKeeper.NoTimeForFishing");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }
    }
  
  private static void Log(string message, bool error = false)
  {
      if (error)
          Tools.Log("NoTimeForFishing", $"{message}", true);
      else
          Tools.Log("NoTimeForFishing", $"{message}");
  }
}