// Decompiled with JetBrains decompiler
// Type: Data.Serialization.MMSerialization
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Newtonsoft.Json;

#nullable disable
namespace Data.Serialization;

public class MMSerialization
{
  public static JsonSerializerSettings JsonSerializerSettings;
  public static JsonSerializer JsonSerializer;

  static MMSerialization()
  {
    JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
    serializerSettings.TypeNameHandling = TypeNameHandling.Auto;
    serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    serializerSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
    serializerSettings.Formatting = Formatting.None;
    serializerSettings.Converters.Add((JsonConverter) new Vector2Converter());
    serializerSettings.Converters.Add((JsonConverter) new Vector3Converter());
    serializerSettings.Converters.Add((JsonConverter) new Vector2IntConverter());
    serializerSettings.Converters.Add((JsonConverter) new Vector3IntConverter());
    serializerSettings.Converters.Add((JsonConverter) new LegacyBooltoFloatConverter());
    serializerSettings.Converters.Add((JsonConverter) new FinalizedNotificationJsonConverter());
    MMSerialization.JsonSerializerSettings = serializerSettings;
    MMSerialization.JsonSerializer = JsonSerializer.Create(MMSerialization.JsonSerializerSettings);
  }
}
