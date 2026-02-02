// Decompiled with JetBrains decompiler
// Type: Data.Serialization.LegacyBooltoFloatConverter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Globalization;

#nullable disable
namespace Data.Serialization;

public class LegacyBooltoFloatConverter : JsonConverter<float>
{
  public override float ReadJson(
    JsonReader reader,
    Type objectType,
    float existingValue,
    bool hasExistingValue,
    JsonSerializer serializer)
  {
    switch (reader.TokenType)
    {
      case JsonToken.Integer:
      case JsonToken.Float:
        return Convert.ToSingle(reader.Value);
      case JsonToken.String:
        string s = (string) reader.Value;
        bool result1;
        float result2;
        return bool.TryParse(s, out result1) ? (!result1 ? 0.0f : 1f) : (float.TryParse(s, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result2) ? result2 : 1f);
      case JsonToken.Boolean:
        return !(bool) reader.Value ? 0.0f : 1f;
      case JsonToken.Null:
        return 1f;
      default:
        return 1f;
    }
  }

  public override void WriteJson(JsonWriter writer, float value, JsonSerializer serializer)
  {
    writer.WriteValue(value);
  }
}
