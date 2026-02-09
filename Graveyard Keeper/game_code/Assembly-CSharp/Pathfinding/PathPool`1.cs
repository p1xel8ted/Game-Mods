// Decompiled with JetBrains decompiler
// Type: Pathfinding.PathPool`1
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Util;
using System;
using UnityEngine;

#nullable disable
namespace Pathfinding;

[Obsolete("Genric version is now obsolete to trade an extremely tiny performance decrease for a large decrease in boilerplate for Path classes")]
public static class PathPool<T> where T : Path, new()
{
  public static void Recycle(T path) => PathPool.Pool((Path) path);

  public static void Warmup(int count, int length)
  {
    ListPool<GraphNode>.Warmup(count, length);
    ListPool<Vector3>.Warmup(count, length);
    Path[] o = new Path[count];
    for (int index = 0; index < count; ++index)
    {
      o[index] = (Path) PathPool<T>.GetPath();
      o[index].Claim((object) o);
    }
    for (int index = 0; index < count; ++index)
      o[index].Release((object) o);
  }

  public static int GetTotalCreated() => PathPool.GetTotalCreated(typeof (T));

  public static int GetSize() => PathPool.GetSize(typeof (T));

  [Obsolete("Use PathPool.GetPath<T> instead of PathPool<T>.GetPath")]
  public static T GetPath() => PathPool.GetPath<T>();
}
