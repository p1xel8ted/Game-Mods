using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GerrysJunkTrunk;

public static class Config
{
    private static Options _options;
    private static ConfigReader _con;

    public static Options GetOptions(bool external = false)
    {
        _options = new Options();
        _con = new ConfigReader(external);

        bool.TryParse(_con.Value("ShowSoldMessagesOnPlayer", "true"), out var showSoldMessagesOnPlayer);
        _options.showSoldMessagesOnPlayer = showSoldMessagesOnPlayer;

        bool.TryParse(_con.Value("DisableSoldMessageWhenNoSale", "false"), out var disableSoldMessageWhenNoSale);
        _options.disableSoldMessageWhenNoSale = disableSoldMessageWhenNoSale;

        bool.TryParse(_con.Value("EnableGerry", "true"), out var enableGerry);
        _options.enableGerry = enableGerry;

        bool.TryParse(_con.Value("ShowSummary", "true"), out var showSummary);
        _options.showSummary = showSummary;

        bool.TryParse(_con.Value("ShowItemPriceTooltips", "true"), out var showItemPriceTooltips);
        _options.showItemPriceTooltips = showItemPriceTooltips;

        bool.TryParse(_con.Value("ShowKnownVendorCount", "true"), out var showKnownVendorCount);
        
        _options.showKnownVendorCount = showKnownVendorCount;
       
        var key = Enum.TryParse<KeyCode>(_con.Value("ReloadConfigKeyBind", "F5"), true, out var b);
        if (key)
        {
            _options.reloadConfigKeyBind = b;
            if (!external)
            {
                Debug.LogWarning($"[GerrysJunkTrunk]: Parsed '{b}' for 'ReloadConfigKeyBind'.");
            }
        }
        else
        {
            _options.reloadConfigKeyBind = KeyCode.F5;
            if (!external)
            {
                Debug.LogWarning($"[GerrysJunkTrunk]: Failed to parse key for 'ReloadConfigKeyBind'. Setting to default F5.");
            }
        }
        
        bool.TryParse(_con.Value("Debug", "false"), out var debug);
        _options.debug = debug;
        
        _con.ConfigWrite();

        return _options;
    }

    [Serializable]
    public class Options
    {
        [FormerlySerializedAs("ShowSoldMessagesOnPlayer")]
        public bool showSoldMessagesOnPlayer = true;

        [FormerlySerializedAs("DisableSoldMessageWhenNoSale")]
        public bool disableSoldMessageWhenNoSale;

        [FormerlySerializedAs("EnableGerry")] public bool enableGerry;
        [FormerlySerializedAs("ShowSummary")] public bool showSummary;

        [FormerlySerializedAs("ShowItemPriceTooltips")]
        public bool showItemPriceTooltips;

        [FormerlySerializedAs("ShowKnownVendorCount")]
        public bool showKnownVendorCount;

        [FormerlySerializedAs("ReloadConfigKeyBind")]
        public KeyCode reloadConfigKeyBind;

        [FormerlySerializedAs("Debug")] public bool debug;
    }
}