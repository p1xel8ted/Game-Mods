// Decompiled with JetBrains decompiler
// Type: Data.Serialization.MMSerialization
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
    MMSerialization.JsonSerializerSettings = serializerSettings;
    MMSerialization.JsonSerializer = JsonSerializer.Create(MMSerialization.JsonSerializerSettings);
  }
}
