using System.Reflection;
using BepInEx;
using HarmonyLib;

namespace LagLess;

[BepInPlugin("acr.20mintilldawn.lagless", "Lag Minus Minus", "0.1.3")]
public class LagLessPlugin : BaseUnityPlugin
{
    private void Awake()
    {
        LLConstants.Logger = Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "acr.20mintilldawn.lagless");
    }
}