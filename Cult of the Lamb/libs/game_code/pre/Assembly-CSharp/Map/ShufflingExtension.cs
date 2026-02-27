// Decompiled with JetBrains decompiler
// Type: Map.ShufflingExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Map;

public static class ShufflingExtension
{
  private static System.Random rng = new System.Random();

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
