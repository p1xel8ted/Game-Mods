using System;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using GYKHelper;
using HarmonyLib;

namespace GerrysJunkTrunk;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.0.3")]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.gerrysjunktrunk";
    private const string PluginName = "Gerry's Junk Trunk";
    private const string PluginVer = "1.8.8";


    private static ConfigEntry<bool> Debug { get; set; }
    internal static ManualLogSource Log { get; set; }
    internal static Harmony Harmony { get; set; }

    private static ConfigEntry<bool> ModEnabled { get; set; }

    private static ConfigEntry<bool> ShowSoldMessagesOnPlayer { get; set; }
    private static ConfigEntry<bool> ShowSoldMessageWhenNoSale { get; set; }
    private static ConfigEntry<bool> EnableGerry { get; set; }
    // private static ConfigEntry<bool> ShowSummaryMessage { get; set; }

    private static ConfigEntry<bool> ShowItemPriceTooltips { get; set; }
    private static ConfigEntry<bool> InternalShippingBoxBuilt { get; set; }
    private static ConfigEntry<bool> InternalShowIntroMessage { get; set; }


    private void Awake()
    {
        Log = Logger;
        Harmony = new Harmony(PluginGuid);
        InitInternalConfiguration();
        InitConfiguration();
        ApplyPatches(this, null);
    }


    private void InitInternalConfiguration()
    {
        InternalShippingBoxBuilt = Config.Bind("Internal (Dont Touch)", "Shipping Box Built", false,
            new ConfigDescription("Internal use.", null,
                new ConfigurationManagerAttributes
                    {Browsable = false, HideDefaultButton = true, IsAdvanced = true, ReadOnly = true, Order = 497}));
        InternalShowIntroMessage = Config.Bind("Internal (Dont Touch)", "Show Intro Message", false,
            new ConfigDescription("Internal use.", null,
                new ConfigurationManagerAttributes
                    {Browsable = false, HideDefaultButton = true, IsAdvanced = true, ReadOnly = true, Order = 496}));
    }

    private void InitConfiguration()
    {
        ModEnabled = Config.Bind("1. General", "Enabled", true,
            new ConfigDescription($"Toggle {PluginName}", null, new ConfigurationManagerAttributes {Order = 7}));
        ModEnabled.SettingChanged += ApplyPatches;

        EnableGerry = Config.Bind("2. Gerry", "Gerry", true,
            new ConfigDescription("Toggle Gerry", null, new ConfigurationManagerAttributes {Order = 6}));

        ShowSoldMessagesOnPlayer = Config.Bind("3. Messages", "Show Sold Messages On Player", true,
            new ConfigDescription("Display messages on the player when items are sold", null,
                new ConfigurationManagerAttributes {Order = 5}));

        ShowSoldMessageWhenNoSale = Config.Bind("3. Messages", "Show Sold Message When No Sale", false,
            new ConfigDescription("Disable the sold message when there is no sale", null,
                new ConfigurationManagerAttributes {Order = 4}));

        // ShowSummaryMessage = Config.Bind("4. UI", "Show Summary", false,
        //     new ConfigDescription("Display a summary of transactions and other relevant information", null,
        //         new ConfigurationManagerAttributes {Order = 3}));

        ShowItemPriceTooltips = Config.Bind("5. Price Tooltips", "Show Item Price Tooltips", true,
            new ConfigDescription("Display tooltips with item prices in the user interface", null,
                new ConfigurationManagerAttributes {Order = 2}));

        Debug = Config.Bind("6. Advanced", "Debug Logging", false,
            new ConfigDescription("Toggle debug logging on or off", null,
                new ConfigurationManagerAttributes {IsAdvanced = true, Order = 0}));
    }


    private static void ApplyPatches(object sender, EventArgs eventArgs)
    {
        if (ModEnabled.Value)
        {
            Actions.WorldGameObjectInteractPrefix += WorldGameObject_Interact;
            Actions.GameBalanceLoad += GameBalance_LoadGameBalance;
            Actions.EndOfDayPostfix += EnvironmentEngine_OnEndOfDay;
            Log.LogInfo($"Applying patches for {PluginName}");
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        else
        {
            Actions.WorldGameObjectInteractPrefix -= WorldGameObject_Interact;
            Actions.GameBalanceLoad -= GameBalance_LoadGameBalance;
            Actions.EndOfDayPostfix -= EnvironmentEngine_OnEndOfDay;
            Log.LogInfo($"Removing patches for {PluginName}");
            Harmony.UnpatchSelf();
        }
    }
}