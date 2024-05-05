using System.Collections.Generic;
using System.IO;

namespace GerryFixer;

public class ConfigReader
{
    private static string _configPath = "./QMods/GerryFixer/config.ini";

    private readonly Dictionary<string, string> _values = new();

    public ConfigReader(bool external = false)
    {
        if (external)
        {
            _configPath = Path.Combine(@"../", @"../", _configPath);
        }
        if (!File.Exists(_configPath)) File.WriteAllText(_configPath, "");

        foreach (var line in File.ReadAllLines(_configPath))
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) return;
            var splitString = line.Split('=');
            _values.Add(splitString[0].Trim(), splitString[1].Trim());
        }
    }

    public string Value(string name, string value = null)
    {
        if (_values != null && _values.ContainsKey(name)) return _values[name];

        _values?.Add(name.Trim(), value?.Trim());
        return value;
    }

    public void ConfigWrite()
    {
        using var file = new StreamWriter(_configPath, false);
        foreach (var entry in _values)
            file.WriteLine("{0}={1}", entry.Key.Trim(), entry.Value.Trim());
    }
}