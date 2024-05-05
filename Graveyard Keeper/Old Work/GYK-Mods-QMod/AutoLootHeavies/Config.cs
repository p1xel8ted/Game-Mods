using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Serialization;

namespace AutoLootHeavies;

public static class Config
{
    private static Options _options;
    private static ConfigReader _con;

    public static void WriteOptions()
    {
        _con.UpdateValue("TeleportToDumpSiteWhenAllStockPilesFull", _options.teleportToDumpSiteWhenAllStockPilesFull.ToString());
        _con.UpdateValue("DesignatedTimberLocation",
            $"{_options.designatedTimberLocation.x},{_options.designatedTimberLocation.y},{_options.designatedTimberLocation.z}".ToString(CultureInfo.InvariantCulture));
        _con.UpdateValue("DesignatedOreLocation",
            $"{_options.designatedOreLocation.x},{_options.designatedOreLocation.y},{_options.designatedOreLocation.z}".ToString(CultureInfo.InvariantCulture));
        _con.UpdateValue("DesignatedStoneLocation",
            $"{_options.designatedStoneLocation.x},{_options.designatedStoneLocation.y},{_options.designatedStoneLocation.z}".ToString(CultureInfo.InvariantCulture));
    }

    public static Options GetOptions(bool external = false)
    {
        _options = new Options();
        _con = new ConfigReader(external);

        bool.TryParse(_con.Value("TeleportToDumpSiteWhenAllStockPilesFull", "true"), out var teleportToDumpSiteWhenAllStockPilesFull);
        _options.teleportToDumpSiteWhenAllStockPilesFull = teleportToDumpSiteWhenAllStockPilesFull;

        bool.TryParse(_con.Value("DisableImmersionMode", "false"), out var disableImmersionMode);
        _options.disableImmersionMode = disableImmersionMode;

        bool.TryParse(_con.Value("Debug", "false"), out var debug);
        _options.debug = debug;

        var tempT = _con.Value("DesignatedTimberLocation", "-3712.003,6144,1294.643".ToString(CultureInfo.InvariantCulture)).Split(',');
        var tempO = _con.Value("DesignatedOreLocation", "-3712.003,6144,1294.643".ToString(CultureInfo.InvariantCulture)).Split(',');
        var tempS = _con.Value("DesignatedStoneLocation", "-3712.003,6144,1294.643".ToString(CultureInfo.InvariantCulture)).Split(',');

        _options.designatedTimberLocation =
            new Vector3(float.Parse(tempT[0], CultureInfo.InvariantCulture), float.Parse(tempT[1], CultureInfo.InvariantCulture), float.Parse(tempT[2], CultureInfo.InvariantCulture));
        _options.designatedOreLocation =
            new Vector3(float.Parse(tempO[0], CultureInfo.InvariantCulture), float.Parse(tempO[1], CultureInfo.InvariantCulture), float.Parse(tempO[2], CultureInfo.InvariantCulture));
        _options.designatedStoneLocation =
            new Vector3(float.Parse(tempS[0], CultureInfo.InvariantCulture), float.Parse(tempS[1], CultureInfo.InvariantCulture), float.Parse(tempS[2], CultureInfo.InvariantCulture));

        var a1 = Enum.TryParse<KeyCode>(_con.Value("ToggleTeleportToDumpSiteKeybind", "Alpha6"), true, out var a2);
        if (a1)
        {
            _options.toggleTeleportToDumpSiteKeybind = a2;
            if (!external)
            {
                Debug.LogWarning($"[AutoLootHeavies]: Parsed '{a2}' for 'ToggleTeleportToDumpSiteKeybind'.");
            }
        }
        else
        {
            _options.toggleTeleportToDumpSiteKeybind = KeyCode.Alpha6;
            if (!external)
            {
                Debug.LogWarning($"[AutoLootHeavies]: Failed to parse key for 'ToggleTeleportToDumpSiteKeybind'. Setting to default Alpha6.");
            }
        }

        var b1 = Enum.TryParse<KeyCode>(_con.Value("SetTimberLocationKeybind", "Alpha7"), true, out var b2);
        if (b1)
        {
            _options.setTimberLocationKeybind = b2;
            if (!external)
            {
                Debug.LogWarning($"[AutoLootHeavies]: Parsed '{b2}' for 'SetTimberLocationKeybind'.");
            }
        }
        else
        {
            _options.setTimberLocationKeybind = KeyCode.Alpha7;
            if (!external)
            {
                Debug.LogWarning($"[AutoLootHeavies]: Failed to parse key for 'SetTimberLocationKeybind'. Setting to default Alpha7.");
            }
        }


        var c1 = Enum.TryParse<KeyCode>(_con.Value("SetOreLocationKeybind", "Alpha8"), true, out var c2);
        if (c1)
        {
            _options.setOreLocationKeybind = c2;
            if (!external)
            {
                Debug.LogWarning($"[AutoLootHeavies]: Parsed '{c2}' for 'SetOreLocationKeybind'.");
            }
        }
        else
        {
            _options.setOreLocationKeybind = KeyCode.Alpha8;
            if (!external)
            {
                Debug.LogWarning($"[AutoLootHeavies]: Failed to parse key for 'SetOreLocationKeybind'. Setting to default Alpha8.");
            }
        }

        var d1 = Enum.TryParse<KeyCode>(_con.Value("SetStoneLocationKeybind", "Alpha9"), true, out var d2);
        if (d1)
        {
            _options.setStoneLocationKeybind = d2;
            if (!external)
            {
                Debug.LogWarning($"[AutoLootHeavies]: Parsed '{d2}' for 'SetStoneLocationKeybind'.");
            }
        }
        else
        {
            _options.setStoneLocationKeybind = KeyCode.Alpha9;
            if (!external)
            {
                Debug.LogWarning($"[AutoLootHeavies]: Failed to parse key for 'SetStoneLocationKeybind'. Setting to default Alpha9.");
            }
        }


        var e1 = Enum.TryParse<KeyCode>(_con.Value("ReloadConfigKeyBind", "F5"), true, out var e2);
        if (e1)
        {
            _options.reloadConfigKeyBind = e2;
            if (!external)
            {
                Debug.LogWarning($"[AutoLootHeavies]: Parsed '{e2}' for 'ReloadConfigKeyBind'.");
            }
        }
        else
        {
            _options.reloadConfigKeyBind = KeyCode.F5;
            if (!external)
            {
                Debug.LogWarning($"[AutoLootHeavies]: Failed to parse key for 'ReloadConfigKeyBind'. Setting to default F5.");
            }
        }

        _con.ConfigWrite();

        return _options;
    }

    [Serializable]
    public class Options
    {
        [FormerlySerializedAs("ReloadConfigKeyBind")]
        public KeyCode reloadConfigKeyBind;

        [FormerlySerializedAs("TeleportToDumpSiteWhenAllStockPilesFull")]
        public bool teleportToDumpSiteWhenAllStockPilesFull;

        [FormerlySerializedAs("DesignatedTimberLocation")]
        public Vector3 designatedTimberLocation;

        [FormerlySerializedAs("DesignatedOreLocation")]
        public Vector3 designatedOreLocation;

        [FormerlySerializedAs("DesignatedStoneLocation")]
        public Vector3 designatedStoneLocation;

        [FormerlySerializedAs("DisableImmersionMode")]
        public bool disableImmersionMode;

        [FormerlySerializedAs("Debug")] public bool debug;

        [FormerlySerializedAs("ToggleTeleportToDumpSiteKeybind")]
        public KeyCode toggleTeleportToDumpSiteKeybind;

        [FormerlySerializedAs("SetTimberLocationKeybind")]
        public KeyCode setTimberLocationKeybind;

        [FormerlySerializedAs("SetOreLocationKeybind")]
        public KeyCode setOreLocationKeybind;

        [FormerlySerializedAs("SetStoneLocationKeybind")]
        public KeyCode setStoneLocationKeybind;
    }
}