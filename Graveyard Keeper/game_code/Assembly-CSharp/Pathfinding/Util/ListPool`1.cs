// Decompiled with JetBrains decompiler
// Type: Pathfinding.Util.ListPool`1
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace Pathfinding.Util;

public static class ListPool<T>
{
  public static List<List<T>> pool = new List<List<T>>();
  public static HashSet<List<T>> inPool = new HashSet<List<T>>();
  public const int MaxCapacitySearchLength = 8;

  public static List<T> Claim()
  {
    lock (ListPool<T>.pool)
    {
      if (ListPool<T>.pool.Count <= 0)
        return new List<T>();
      List<T> objList = ListPool<T>.pool[ListPool<T>.pool.Count - 1];
      ListPool<T>.pool.RemoveAt(ListPool<T>.pool.Count - 1);
      ListPool<T>.inPool.Remove(objList);
      return objList;
    }
  }

  public static List<T> Claim(int capacity)
  {
    lock (ListPool<T>.pool)
    {
      List<T> objList1 = (List<T>) null;
      int index1 = -1;
      for (int index2 = 0; index2 < ListPool<T>.pool.Count && index2 < 8; ++index2)
      {
        List<T> objList2 = ListPool<T>.pool[ListPool<T>.pool.Count - 1 - index2];
        if (objList2.Capacity >= capacity)
        {
          ListPool<T>.pool.RemoveAt(ListPool<T>.pool.Count - 1 - index2);
          ListPool<T>.inPool.Remove(objList2);
          return objList2;
        }
        if (objList1 == null || objList2.Capacity > objList1.Capacity)
        {
          objList1 = objList2;
          index1 = ListPool<T>.pool.Count - 1 - index2;
        }
      }
      if (objList1 == null)
      {
        objList1 = new List<T>(capacity);
      }
      else
      {
        objList1.Capacity = capacity;
        ListPool<T>.pool[index1] = ListPool<T>.pool[ListPool<T>.pool.Count - 1];
        ListPool<T>.pool.RemoveAt(ListPool<T>.pool.Count - 1);
        ListPool<T>.inPool.Remove(objList1);
      }
      return objList1;
    }
  }

  public static void Warmup(int count, int size)
  {
    lock (ListPool<T>.pool)
    {
      List<T>[] objListArray = new List<T>[count];
      for (int index = 0; index < count; ++index)
        objListArray[index] = ListPool<T>.Claim(size);
      for (int index = 0; index < count; ++index)
        ListPool<T>.Release(objListArray[index]);
    }
  }

  public static void Release(List<T> list)
  {
    list.Clear();
    lock (ListPool<T>.pool)
      ListPool<T>.pool.Add(list);
  }

  public static void Clear()
  {
    lock (ListPool<T>.pool)
      ListPool<T>.pool.Clear();
  }

  public static int GetSize() => ListPool<T>.pool.Count;
}
