// Decompiled with JetBrains decompiler
// Type: Data.Serialization.Vector3Converter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

#nullable disable
namespace Data.Serialization;

public class Vector3Converter : JsonConverter<Vector3>
{
  private string kXProperty = "x";
  private string kYProperty = "y";
  private string kZProperty = "z";

  public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
  {
    JObject jobject = new JObject();
    jobject.AddFirst((object) new JProperty(this.kXProperty, (object) value.x));
    jobject.AddFirst((object) new JProperty(this.kYProperty, (object) value.y));
    jobject.AddFirst((object) new JProperty(this.kZProperty, (object) value.z));
    jobject.WriteTo(writer);
  }

  public override Vector3 ReadJson(
    JsonReader reader,
    System.Type objectType,
    Vector3 existingValue,
    bool hasExistingValue,
    JsonSerializer serializer)
  {
    JObject jobject = JObject.Load(reader);
    Vector3 zero = Vector3.zero;
    if (jobject[this.kXProperty] != null)
      zero.x = jobject[this.kXProperty].Value<float>();
    if (jobject[this.kYProperty] != null)
      zero.y = jobject[this.kYProperty].Value<float>();
    if (jobject[this.kZProperty] != null)
      zero.z = jobject[this.kZProperty].Value<float>();
    return zero;
  }
}
