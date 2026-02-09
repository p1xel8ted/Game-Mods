// Decompiled with JetBrains decompiler
// Type: DungeonGenerator.IntVector2
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace DungeonGenerator;

[Serializable]
public class IntVector2
{
  [SerializeField]
  public int x;
  [SerializeField]
  public int y;

  public IntVector2() => this.x = this.y = 0;

  public IntVector2(int t_x = 0, int t_y = 0)
  {
    this.x = t_x;
    this.y = t_y;
  }

  public IntVector2(Vector2 v)
  {
    this.x = Mathf.RoundToInt(v.x);
    this.y = Mathf.RoundToInt(v.y);
  }

  public static IntVector2 operator +(IntVector2 left, IntVector2 right)
  {
    return new IntVector2(left.x + right.x, left.y + right.y);
  }

  public IntVector2 Copy() => new IntVector2(this.x, this.y);

  public override string ToString() => $"[{this.x.ToString()}, {this.y.ToString()}]";

  public Vector2 ToVector2() => new Vector2((float) this.x, (float) this.y);

  public bool EqualsTo(IntVector2 v) => v.x == this.x && v.y == this.y;
}
