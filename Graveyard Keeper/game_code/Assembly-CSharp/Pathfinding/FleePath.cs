// Decompiled with JetBrains decompiler
// Type: Pathfinding.FleePath
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Pathfinding;

public class FleePath : RandomPath
{
  public static FleePath Construct(
    Vector3 start,
    Vector3 avoid,
    int searchLength,
    OnPathDelegate callback = null)
  {
    FleePath path = PathPool.GetPath<FleePath>();
    path.Setup(start, avoid, searchLength, callback);
    return path;
  }

  public void Setup(Vector3 start, Vector3 avoid, int searchLength, OnPathDelegate callback)
  {
    this.Setup(start, searchLength, callback);
    this.aim = avoid - start;
    this.aim = this.aim * 10f;
    this.aim = start - this.aim;
  }
}
