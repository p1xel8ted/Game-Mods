// Decompiled with JetBrains decompiler
// Type: Pathfinding.Util.StackPool`1
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding.Util;

public static class StackPool<T>
{
  public static List<Stack<T>> pool = new List<Stack<T>>();

  public static Stack<T> Claim()
  {
    if (StackPool<T>.pool.Count <= 0)
      return new Stack<T>();
    Stack<T> objStack = StackPool<T>.pool[StackPool<T>.pool.Count - 1];
    StackPool<T>.pool.RemoveAt(StackPool<T>.pool.Count - 1);
    return objStack;
  }

  public static void Warmup(int count)
  {
    Stack<T>[] objStackArray = new Stack<T>[count];
    for (int index = 0; index < count; ++index)
      objStackArray[index] = StackPool<T>.Claim();
    for (int index = 0; index < count; ++index)
      StackPool<T>.Release(objStackArray[index]);
  }

  public static void Release(Stack<T> stack)
  {
    for (int index = 0; index < StackPool<T>.pool.Count; ++index)
    {
      if (StackPool<T>.pool[index] == stack)
        Debug.LogError((object) "The Stack is released even though it is inside the pool");
    }
    stack.Clear();
    StackPool<T>.pool.Add(stack);
  }

  public static void Clear() => StackPool<T>.pool.Clear();

  public static int GetSize() => StackPool<T>.pool.Count;
}
