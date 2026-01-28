// Decompiled with JetBrains decompiler
// Type: Map.ShufflingExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Map;

public static class ShufflingExtension
{
  public static System.Random rng = new System.Random();

  public static void Shuffle<T>(this IList<T> list)
  {
    int count = list.Count;
    while (count > 1)
    {
      --count;
      int index = ShufflingExtension.rng.Next(count + 1);
      T obj = list[index];
      list[index] = list[count];
      list[count] = obj;
    }
  }

  public static T Random<T>(this IList<T> list) => list[ShufflingExtension.rng.Next(list.Count)];

  public static T Last<T>(this IList<T> list) => list[list.Count - 1];

  public static List<T> GetRandomElements<T>(this List<T> list, int elementsCount)
  {
    return list.OrderBy<T, Guid>((Func<T, Guid>) (arg => Guid.NewGuid())).Take<T>(list.Count < elementsCount ? list.Count : elementsCount).ToList<T>();
  }
}
