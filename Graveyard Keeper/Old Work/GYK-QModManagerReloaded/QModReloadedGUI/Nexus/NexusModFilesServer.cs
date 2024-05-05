using System.Text.Json.Serialization;

namespace QModReloadedGUI;

public class NexusModFilesServer

{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("short_name")]
    public string ShortName { get; set; }
    [JsonPropertyName("URI")]
    public string Uri { get; set; }
}