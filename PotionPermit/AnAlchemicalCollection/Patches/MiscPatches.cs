using System.Diagnostics.CodeAnalysis;
using HarmonyLib;

namespace AnAlchemicalCollection;

[HarmonyPatch]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class MiscPatches
{
    //stops ridiculous log spam
    [HarmonyPrefix]
    [HarmonyPatch(typeof(tk2dBaseSprite), nameof(tk2dBaseSprite.SetSprite), typeof(string))]
    public static bool tk2dBaseSprite_SetSprite(string spriteName, ref tk2dBaseSprite __instance, 
        // ReSharper disable once RedundantAssignment
        ref bool __result)
    {
        var spriteIdByName = __instance.collection.GetSpriteIdByName(spriteName, -1);
        if (spriteIdByName != -1)
        {
            __instance.SetSprite(spriteIdByName);
        }

        __result = spriteIdByName != -1;
        return false;
    }

    //stops ridiculous log spam
    [HarmonyPrefix]
    [HarmonyPatch(typeof(tk2dBaseSprite), nameof(tk2dBaseSprite.SetSprite), typeof(tk2dSpriteCollectionData),
        typeof(string))]
    public static bool tk2dBaseSprite_SetSprite(tk2dSpriteCollectionData newCollection, string spriteName,
        // ReSharper disable once RedundantAssignment
        ref tk2dBaseSprite __instance, ref bool __result)
    {
        var spriteIdByName = newCollection.GetSpriteIdByName(spriteName, -1);
        if (spriteIdByName != -1)
        {
            __instance.SetSprite(newCollection, spriteIdByName);
        }

        __result = spriteIdByName != -1;
        return false;
    }
}