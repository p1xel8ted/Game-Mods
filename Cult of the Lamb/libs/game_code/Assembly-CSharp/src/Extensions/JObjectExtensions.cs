// Decompiled with JetBrains decompiler
// Type: src.Extensions.JObjectExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
