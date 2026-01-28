// Decompiled with JetBrains decompiler
// Type: ListExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class ListExtensions
{
  public static T LastElement<T>(this List<T> list) where T : class
  {
    return list.Count > 0 ? list[list.Count - 1] : default (T);
  }

  public static T RandomElement<T>(this List<T> list) => list[list.RandomIndex<T>()];

  public static int RandomIndex<T>(this List<T> list) => Random.Range(0, list.Count);

  public static bool Equals<T>(List<T> list1, List<T> list2)
  {
    if (list1.Count != list2.Count)
      return false;
    foreach (T obj1 in list1)
    {
      bool flag = false;
      foreach (T obj2 in list2)
      {
        if (obj1.Equals((object) obj2))
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return false;
    }
    return true;
  }
}
