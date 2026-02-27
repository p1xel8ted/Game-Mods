// Decompiled with JetBrains decompiler
// Type: Data.Serialization.Vector2IntConverter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

#nullable disable
namespace Data.Serialization;

public class Vector2IntConverter : JsonConverter<Vector2Int>
{
  public string kXProperty = "x";
  public string kYProperty = "y";

  public override void WriteJson(JsonWriter writer, Vector2Int value, JsonSerializer serializer)
  {
    JObject jobject = new JObject();
    jobject.AddFirst((object) new JProperty(this.kXProperty, (object) value.x));
    jobject.AddFirst((object) new JProperty(this.kYProperty, (object) value.y));
    jobject.WriteTo(writer);
  }

  public override Vector2Int ReadJson(
    JsonReader reader,
    System.Type objectType,
    Vector2Int existingValue,
    bool hasExistingValue,
    JsonSerializer serializer)
  {
    JObject jobject = JObject.Load(reader);
    Vector2Int zero = Vector2Int.zero;
    if (jobject[this.kXProperty] != null)
      zero.x = jobject[this.kXProperty].Value<int>();
    if (jobject[this.kYProperty] != null)
      zero.y = jobject[this.kYProperty].Value<int>();
    return zero;
  }
}
