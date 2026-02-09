// Decompiled with JetBrains decompiler
// Type: Pathfinding.Util.ArrayPool`1
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Pathfinding.Util;

public static class ArrayPool<T>
{
  public static Stack<T[]>[] pool = new Stack<T[]>[31 /*0x1F*/];
  public static HashSet<T[]> inPool = new HashSet<T[]>();

  public static T[] Claim(int minimumLength)
  {
    int index = 0;
    while (1 << index < minimumLength && index < 30)
      ++index;
    if (index == 30)
      throw new ArgumentException("Too high minimum length");
    lock (ArrayPool<T>.pool)
    {
      if (ArrayPool<T>.pool[index] == null)
        ArrayPool<T>.pool[index] = new Stack<T[]>();
      if (ArrayPool<T>.pool[index].Count > 0)
      {
        T[] objArray = ArrayPool<T>.pool[index].Pop();
        ArrayPool<T>.inPool.Remove(objArray);
        return objArray;
      }
    }
    return new T[1 << index];
  }

  public static void Release(ref T[] array)
  {
    lock (ArrayPool<T>.pool)
    {
      int index = 0;
      while (1 << index < array.Length && index < 30)
        ++index;
      if (array.Length != 1 << index)
        throw new ArgumentException("Array length is not a power of 2");
      if (ArrayPool<T>.pool[index] == null)
        ArrayPool<T>.pool[index] = new Stack<T[]>();
      ArrayPool<T>.pool[index].Push(array);
    }
    array = (T[]) null;
  }
}
