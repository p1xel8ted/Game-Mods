// Decompiled with JetBrains decompiler
// Type: Pathfinding.PathPool
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Pathfinding;

public static class PathPool
{
  public static Dictionary<Type, Stack<Path>> pool = new Dictionary<Type, Stack<Path>>();
  public static Dictionary<Type, int> totalCreated = new Dictionary<Type, int>();

  public static void Pool(Path path)
  {
    lock (PathPool.pool)
    {
      if (path.pooled)
        throw new ArgumentException("The path is already pooled.");
      Stack<Path> pathStack;
      if (!PathPool.pool.TryGetValue(path.GetType(), out pathStack))
      {
        pathStack = new Stack<Path>();
        PathPool.pool[path.GetType()] = pathStack;
      }
      path.pooled = true;
      path.OnEnterPool();
      pathStack.Push(path);
    }
  }

  public static int GetTotalCreated(Type type)
  {
    int num;
    return PathPool.totalCreated.TryGetValue(type, out num) ? num : 0;
  }

  public static int GetSize(Type type)
  {
    Stack<Path> pathStack;
    return PathPool.pool.TryGetValue(type, out pathStack) ? pathStack.Count : 0;
  }

  public static T GetPath<T>() where T : Path, new()
  {
    lock (PathPool.pool)
    {
      Stack<Path> pathStack;
      T path;
      if (PathPool.pool.TryGetValue(typeof (T), out pathStack) && pathStack.Count > 0)
      {
        path = pathStack.Pop() as T;
      }
      else
      {
        path = new T();
        if (!PathPool.totalCreated.ContainsKey(typeof (T)))
          PathPool.totalCreated[typeof (T)] = 0;
        PathPool.totalCreated[typeof (T)]++;
      }
      path.pooled = false;
      path.Reset();
      return path;
    }
  }
}
