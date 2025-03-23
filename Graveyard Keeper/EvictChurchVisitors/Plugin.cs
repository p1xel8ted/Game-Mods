using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using GYKHelper;
using UnityEngine;

namespace EvictChurchVisitors;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.1.0")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.evictchurchvisitors";
    private const string PluginName = "Evict Church Visitors!";
    private const string PluginVer = "0.1.0";
    
    private static ManualLogSource Log { get; set; }

    private void Awake()
    {
        Log = Logger;
        Config.Bind("01. Fixes", "Evict All Church Visitors", true,
            new ConfigDescription("Force any stuck church visitors to vacate the premise.", null,
                new ConfigurationManagerAttributes
                    { Order = 100, HideDefaultButton = true, CustomDrawer = EvictVisitorsButton }));
        Log.LogInfo($"Plugin {PluginName} is loaded!");
    }

    private static void DisplayConfirmationDialog()
    {
        GUILayout.Label("The game may appear to hang while all the visitors are evicted. It has not crashed. Continue?");

        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Yes", GUILayout.ExpandWidth(true)))
            {
                EvictVisitors();
                _showConfirmationDialog = false;
            }

            if (GUILayout.Button("No", GUILayout.ExpandWidth(true)))
            {
                _showConfirmationDialog = false;
            }
        }
        GUILayout.EndHorizontal();
    }

    private static void EvictVisitors()
    {
        var churchVisitors = WorldMap._objs.FindAll(a => a.obj_id.Contains("npc_church_visitor"));
        if (churchVisitors.Count == 0)
        {
            Log.LogWarning("No church visitors to evict.");
            return;
        }

        Log.LogWarning($"Starting to evict {churchVisitors.Count} visitors. Hang tight wile I work. Game may appear to hang depending on quantity of visitors.");

        var zones = new List<WorldZone>();

        foreach (var visitor in churchVisitors.Where(visitor => visitor.obj_def.IsNPC()))
        {
            zones.Add(visitor._zone);

            if (visitor.is_removed)
            {
                return;
            }

            visitor.components.craft.enabled = false;
            visitor.components.timer.enabled = false;
            visitor.components.hp.enabled = false;
            ChunkManager.OnDestroyObject(visitor);
            if (visitor._bubble != null)
            {
                InteractionBubbleGUI.RemoveBubble(visitor.unique_id, true);
                visitor._bubble = null;
            }

            visitor.UnlinkWithSpawnerIfExists();
            visitor.is_removed = true;
            Destroy(visitor.gameObject);
            if (!visitor._was_ever_active)
            {
                visitor.OnDestroy();
            }
        }

        zones = zones.Distinct().ToList();

        foreach (var zone in zones)
        {
            zone.Recalculate();
        }
    }

    private static bool _showConfirmationDialog;

    private static void EvictVisitorsButton(ConfigEntryBase entry)
    {
        if (_showConfirmationDialog)
        {
            DisplayConfirmationDialog();
        }
        else
        {
            var button = GUILayout.Button("Evict Church Visitors", GUILayout.ExpandWidth(true));
            if (button)
            {
                _showConfirmationDialog = true;
            }
        }
    }
}