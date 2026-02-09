// Decompiled with JetBrains decompiler
// Type: DockPointMarker
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class DockPointMarker : MonoBehaviour
{
  public GameObject right;
  public GameObject left;
  public GameObject down;
  public GameObject up;

  public GameObject GetMarker(Direction dir)
  {
    switch (dir)
    {
      case Direction.Right:
        return this.left;
      case Direction.Up:
        return this.down;
      case Direction.Left:
        return this.right;
      case Direction.Down:
        return this.up;
      default:
        throw new ArgumentOutOfRangeException(nameof (dir), (object) dir, (string) null);
    }
  }
}
