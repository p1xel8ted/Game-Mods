// Decompiled with JetBrains decompiler
// Type: IListExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
