using HarmonyLib;
using System;
using System.Reflection;

namespace Exhaustless;

[HarmonyPatch]
[HarmonyBefore("p1xel8ted.GraveyardKeeper.QueueEverything")]
public static partial class MainPatcher
{
    private static readonly ItemDefinition.ItemType[] ToolItems =
    {
        ItemDefinition.ItemType.Axe, ItemDefinition.ItemType.Shovel, ItemDefinition.ItemType.Hammer,
        ItemDefinition.ItemType.Pickaxe, ItemDefinition.ItemType.FishingRod, ItemDefinition.ItemType.BodyArmor,
        ItemDefinition.ItemType.HeadArmor, ItemDefinition.ItemType.Sword,
    };

    private static Config.Options _cfg;

    public static void Patch()
    {
        try
        {
            var harmony = new Harmony("p1xel8ted.GraveyardKeeper.exhaust-less");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            
            
            _cfg = Config.GetOptions();
     
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }
    }
}