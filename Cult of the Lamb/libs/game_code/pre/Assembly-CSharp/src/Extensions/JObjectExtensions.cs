// Decompiled with JetBrains decompiler
// Type: src.Extensions.JObjectExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Newtonsoft.Json.Linq;

#nullable disable
namespace src.Extensions;

public static class JObjectExtensions
{
  public static JObject GetJObject(this JObject jObject, string key)
  {
    return jObject[key] != null ? jObject[key].Value<JObject>() : (JObject) null;
  }

  public static bool TryGetAssignable<T>(this JObject jObject, string key, ref T assignable)
  {
    if (jObject[key] == null)
      return false;
    assignable = jObject[key].Value<T>();
    return true;
  }

  public static void AddAssignable<T>(this JObject jObject, string key, ref T assignable)
  {
    jObject.AddFirst((object) new JProperty(key, (object) assignable));
  }
}
