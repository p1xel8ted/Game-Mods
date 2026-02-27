// Decompiled with JetBrains decompiler
// Type: Data.Serialization.ColorConverter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

#nullable disable
namespace Data.Serialization;

public class ColorConverter : JsonConverter<Color>
{
  public string kRProperty = "r";
  public string kGProperty = "g";
  public string kBProperty = "b";
  public string kAProperty = "a";

  public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
  {
    JObject jobject = new JObject();
    jobject.AddFirst((object) new JProperty(this.kRProperty, (object) value.r));
    jobject.AddFirst((object) new JProperty(this.kGProperty, (object) value.g));
    jobject.AddFirst((object) new JProperty(this.kBProperty, (object) value.b));
    jobject.AddFirst((object) new JProperty(this.kAProperty, (object) value.a));
    jobject.WriteTo(writer);
  }

  public override Color ReadJson(
    JsonReader reader,
    System.Type objectType,
    Color existingValue,
    bool hasExistingValue,
    JsonSerializer serializer)
  {
    JObject jobject = JObject.Load(reader);
    Color white = Color.white;
    if (jobject[this.kRProperty] != null)
      white.r = jobject[this.kRProperty].Value<float>();
    if (jobject[this.kGProperty] != null)
      white.g = jobject[this.kGProperty].Value<float>();
    if (jobject[this.kBProperty] != null)
      white.b = jobject[this.kBProperty].Value<float>();
    if (jobject[this.kAProperty] != null)
      white.a = jobject[this.kAProperty].Value<float>();
    return white;
  }
}
