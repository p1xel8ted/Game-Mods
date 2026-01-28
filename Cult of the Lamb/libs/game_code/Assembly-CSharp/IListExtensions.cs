// Decompiled with JetBrains decompiler
// Type: IListExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class IListExtensions
{
  public static void Shuffle<T>(this IList<T> ts)
  {
    int count = ts.Count;
    int num = count - 1;
    for (int index1 = 0; index1 < num; ++index1)
    {
      int index2 = Random.Range(index1, count);
      T t = ts[index1];
      ts[index1] = ts[index2];
      ts[index2] = t;
    }
  }
}
