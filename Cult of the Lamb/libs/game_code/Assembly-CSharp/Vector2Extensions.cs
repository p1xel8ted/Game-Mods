// Decompiled with JetBrains decompiler
// Type: Vector2Extensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class Vector2Extensions
{
  public static string ToResolutionString(this Vector2 vector2) => $"{vector2.x} x {vector2.y}";

  public static Vector2 Abs(this Vector2 vector)
  {
    return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
  }
}
