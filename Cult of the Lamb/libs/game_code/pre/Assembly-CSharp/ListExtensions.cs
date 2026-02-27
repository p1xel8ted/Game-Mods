// Decompiled with JetBrains decompiler
// Type: ListExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
