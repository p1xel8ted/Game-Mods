// Decompiled with JetBrains decompiler
// Type: JsonUtilityHelper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class JsonUtilityHelper
{
  public static string ToJsonList<T>(List<T> list)
  {
    return JsonUtility.ToJson((object) new JsonUtilityHelper.JsonList<T>()
    {
      list = list
    });
  }

  public static List<T> FromJsonList<T>(string json)
  {
    return JsonUtility.FromJson<JsonUtilityHelper.JsonList<T>>(json).list;
  }

  public class JsonList<T>
  {
    public List<T> list;
  }
}
