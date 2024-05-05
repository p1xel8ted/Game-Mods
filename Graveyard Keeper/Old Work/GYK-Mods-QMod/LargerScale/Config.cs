using System;
using System.Globalization;
using UnityEngine;

namespace LargerScale;

public static class Config
{
    private static Options _options;
    private static ConfigReader _con;

    public static Options GetOptions(bool external = false)
    {
        _options = new Options();
        _con = new ConfigReader(external);

        float.TryParse(_con.Value("PixelSize", "2"), NumberStyles.Float, CultureInfo.InvariantCulture, out var PixelSize);
        _options.PixelSize = PixelSize;
        
        int.TryParse(_con.Value("PixelZoom", "2"), NumberStyles.Integer, CultureInfo.InvariantCulture, out var PixelZoom);
        _options.PixelZoom = PixelZoom;
        
        int.TryParse(_con.Value("GuiZoom", "1"), NumberStyles.Float, CultureInfo.InvariantCulture, out var GuiZoom);
        _options.GuiZoom = GuiZoom;

        bool.TryParse(_con.Value("Debug", "false"), out var debug);
        _options.Debug = debug;
        
        var key = Enum.TryParse<KeyCode>(_con.Value("ReloadConfigKeyBind", "F5"), true, out var b);
        if (key)
        {
            _options.ReloadConfigKeyBind = b;
            if (!external)
            {
                Debug.LogWarning($"[LargerScale]: Parsed '{b}' for 'ReloadConfigKeyBind'.");
            }
        }
        else
        {
            _options.ReloadConfigKeyBind = KeyCode.F5;
            if (!external)
            {
                Debug.LogWarning($"[LargerScale]: Failed to parse key for 'ReloadConfigKeyBind'. Setting to default F5.");
            }
        }
        
        _con.ConfigWrite();

        return _options;
    }

 
    public class Options
    {
       public KeyCode ReloadConfigKeyBind;
       public float PixelSize;
       public float GuiZoom;
       public int PixelZoom;
        public bool Debug;
    }
}