// Decompiled with JetBrains decompiler
// Type: Pathfinding.AstarSplines
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public static class AstarSplines
{
  public static Vector3 CatmullRom(
    Vector3 previous,
    Vector3 start,
    Vector3 end,
    Vector3 next,
    float elapsedTime)
  {
    float num1 = elapsedTime;
    float num2 = num1 * num1;
    float num3 = num2 * num1;
    return previous * (float) (-0.5 * (double) num3 + (double) num2 - 0.5 * (double) num1) + start * (float) (1.5 * (double) num3 + -2.5 * (double) num2 + 1.0) + end * (float) (-1.5 * (double) num3 + 2.0 * (double) num2 + 0.5 * (double) num1) + next * (float) (0.5 * (double) num3 - 0.5 * (double) num2);
  }

  [Obsolete("Use CatmullRom")]
  public static Vector3 CatmullRomOLD(
    Vector3 previous,
    Vector3 start,
    Vector3 end,
    Vector3 next,
    float elapsedTime)
  {
    return AstarSplines.CatmullRom(previous, start, end, next, elapsedTime);
  }

  public static Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
  {
    t = Mathf.Clamp01(t);
    float num = 1f - t;
    return num * num * num * p0 + 3f * num * num * t * p1 + 3f * num * t * t * p2 + t * t * t * p3;
  }
}
