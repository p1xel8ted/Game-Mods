using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Tweaks;

[HarmonyPatch] // This attribute effectively marks this class for Harmony to consider for patching
[HarmonyAfter("com.sinai.SideLoader")]
public static class TargetMethodsPatch
{
    [HarmonyTargetMethods]
    internal static IEnumerable<MethodBase> TargetMethods()
    {
        // Directly use the fully qualified class name and assembly name
        const string typeName = "SideLoader.SL, SideLoader";
        var type = Type.GetType(typeName);

        // Check if the type was successfully retrieved
        if (type == null)
        {
            Plugin.Log.LogWarning($"Could not find type: {typeName}. Make sure the assembly is loaded and the type name is correct.");
            yield break; // Exit if the type is not found
        }

        // Retrieve all methods declared in the specified type that do not start with "Log"
        var methods = type.GetMethods(AccessTools.all)
            .Where(method => method.Name.StartsWith("Log"));

        foreach (var method in methods)
        {
            yield return method;
        }
    }

    [HarmonyPrefix]
    public static bool SL_Log()
    {
        // Assuming this is a method intended to intercept calls to logging methods and suppress them
        // If you need to do something more specific, adjust the logic here
        return false; // Returning false should prevent the original method from executing
    }
}