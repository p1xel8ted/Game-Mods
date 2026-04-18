using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace Shared;

// Holds cross-mod debug-warning registrations on a single named GameObject so every mod's
// linked copy of this file sees the same registry. Without this the statics would be
// per-assembly and the "one dialog for all mods" behaviour wouldn't work.
internal class DebugWarningRegistry : MonoBehaviour
{
    private const string HostObjectName = "~DebugWarningRegistry";
    private static DebugWarningRegistry _instance;

    internal readonly Dictionary<string, Func<bool>> Registrations = new();
    internal bool DialogShown;

    internal static DebugWarningRegistry Instance
    {
        get
        {
            if (_instance != null) return _instance;
            var go = GameObject.Find(HostObjectName);
            if (go == null)
            {
                go = new GameObject(HostObjectName);
                DontDestroyOnLoad(go);
            }
            _instance = go.GetComponent<DebugWarningRegistry>() ?? go.AddComponent<DebugWarningRegistry>();
            return _instance;
        }
    }
}

// Call DebugWarningDialog.Register(MyPluginInfo.PLUGIN_NAME, () => DebugEnabled) in Awake.
// The first mod's copy of this file to be loaded creates the shared registry GameObject;
// every mod shares it at runtime via GameObject.Find.
public static class DebugWarningDialog
{
    public static void Register(string pluginName, Func<bool> isDebugEnabled)
    {
        if (string.IsNullOrEmpty(pluginName) || isDebugEnabled == null) return;
        DebugWarningRegistry.Instance.Registrations[pluginName] = isDebugEnabled;
    }
}

[Harmony]
internal static class DebugWarningDialogPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenuGUI), nameof(MainMenuGUI.Open), typeof(bool))]
    public static void MainMenuGUI_Open_ShowDebugWarning()
    {
        var registry = DebugWarningRegistry.Instance;
        if (registry.DialogShown) return;
        if (GUIElements.me == null || GUIElements.me.dialog == null) return;

        var enabled = registry.Registrations
            .Where(kv =>
            {
                try { return kv.Value(); }
                catch { return false; }
            })
            .Select(kv => kv.Key)
            .OrderBy(name => name)
            .ToList();

        if (enabled.Count == 0) return;

        registry.DialogShown = true;

        var list = string.Join("\n", enabled.Select(n => $"\u2022 {n}").ToArray());
        var message = enabled.Count == 1
            ? $"Debug Logging is currently ON for:\n\n{list}\n\nVerbose diagnostics will be written to the BepInEx console. Turn it off in the mod's settings once you're done troubleshooting."
            : $"Debug Logging is currently ON for:\n\n{list}\n\nVerbose diagnostics will be written to the BepInEx console. Turn each off in the corresponding mod's settings once you're done troubleshooting.";

        GUIElements.me.dialog.OpenOK("Debug Logging", null, message, true, string.Empty);
    }
}
