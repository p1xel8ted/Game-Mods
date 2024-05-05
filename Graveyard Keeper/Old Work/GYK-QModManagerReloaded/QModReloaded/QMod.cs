using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QModReloaded;

public class QMod
{
    public string AssemblyName { get; set; }
    public string Author { get; set; }
    [JsonIgnore]
    public string Config { get; set; }
    public string Description { get; set; }
    public string DisplayName { get; set; }
    public bool Enable { get; set; }
    public string EntryMethod { get; set; }
    public string Id { get; set; }
    [JsonIgnore]
    public Assembly LoadedAssembly { get; set; }

    public bool UpdateAvailable { get; set; }

    public int LoadOrder { get; set; } = -1;
    [JsonIgnore]
    public string ModAssemblyPath { get; set; }

    public int NexusId { get; set; }
    public string Version { get; set; }
    
    public static QMod FromJsonFile(string file)
    {
        var value = File.ReadAllText(file);
        return JsonSerializer.Deserialize<QMod>(value);
    }

    public void SaveJson()
    {
        var jsonOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
            IncludeFields = true,
            UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement
        };
        File.WriteAllText(Path.Combine(ModAssemblyPath, "mod.json"), JsonSerializer.Serialize(this, jsonOptions));
    }
}