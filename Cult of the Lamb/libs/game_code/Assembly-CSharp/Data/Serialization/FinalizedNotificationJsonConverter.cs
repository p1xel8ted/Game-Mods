// Decompiled with JetBrains decompiler
// Type: Data.Serialization.FinalizedNotificationJsonConverter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

#nullable disable
namespace Data.Serialization;

public sealed class FinalizedNotificationJsonConverter : JsonConverter
{
  public override bool CanConvert(Type objectType) => objectType == typeof (FinalizedNotification);

  public override object ReadJson(
    JsonReader reader,
    Type objectType,
    object existingValue,
    JsonSerializer serializer)
  {
    if (reader.TokenType == JsonToken.Null)
      return (object) null;
    JObject jobject = JObject.Load(reader);
    FinalizedNotification target = jobject["FaithDelta"] == null ? (jobject["ItemType"] != null || jobject["ItemDelta"] != null ? (FinalizedNotification) new FinalizedItemNotification() : (jobject["FollowerAnimationA"] != null || jobject["FollowerAnimationB"] != null ? (FinalizedNotification) new FinalizedRelationshipNotification() : (jobject["Animation"] == null ? (FinalizedNotification) new FinalizedNotificationSimple() : (FinalizedNotification) new FinalizedFollowerNotification()))) : (FinalizedNotification) new FinalizedFaithNotification();
    using (JsonReader reader1 = jobject.CreateReader())
      JsonSerializer.CreateDefault().Populate(reader1, (object) target);
    return (object) target;
  }

  public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
  {
    serializer.Serialize(writer, value);
  }
}
