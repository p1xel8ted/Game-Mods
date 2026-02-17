// Decompiled with JetBrains decompiler
// Type: Data.Serialization.Vector2Converter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

#nullable disable
namespace Data.Serialization;

public class Vector2Converter : JsonConverter<Vector2>
{
  public string kXProperty = "x";
  public string kYProperty = "y";

  public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
  {
    JObject jobject = new JObject();
    jobject.AddFirst((object) new JProperty(this.kXProperty, (object) value.x));
    jobject.AddFirst((object) new JProperty(this.kYProperty, (object) value.y));
    jobject.WriteTo(writer);
  }

  public override Vector2 ReadJson(
    JsonReader reader,
    System.Type objectType,
    Vector2 existingValue,
    bool hasExistingValue,
    JsonSerializer serializer)
  {
    JObject jobject = JObject.Load(reader);
    Vector2 zero = Vector2.zero;
    if (jobject[this.kXProperty] != null)
      zero.x = jobject[this.kXProperty].Value<float>();
    if (jobject[this.kYProperty] != null)
      zero.y = jobject[this.kYProperty].Value<float>();
    return zero;
  }
}
