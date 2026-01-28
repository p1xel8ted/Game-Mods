// Decompiled with JetBrains decompiler
// Type: EllipseMovement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public struct EllipseMovement
{
  public Vector3 Up;
  public Vector3 Right;
  public Vector3 CenterOffset;
  public float Radius1;
  public float Radius2;
  public float StartAngle;
  public float AngleToMove;
  public float Duration;
  public AnimationCurve RadiusMultiplierOverTime;

  public EllipseMovement(
    Vector3 up,
    Vector3 right,
    float radius1,
    float radius2,
    float startAngle,
    float angleToMove,
    float duration,
    AnimationCurve radiusMultiplierOverTime)
  {
    this.Up = up;
    this.Right = right;
    this.Radius1 = radius1;
    this.Radius2 = radius2;
    this.StartAngle = startAngle;
    this.AngleToMove = angleToMove;
    this.Duration = duration;
    this.RadiusMultiplierOverTime = radiusMultiplierOverTime;
    this.CenterOffset = Vector3.zero;
  }

  public EllipseMovement(
    Vector3 up,
    Vector3 right,
    Vector3 centerOffset,
    float radius1,
    float radius2,
    float startAngle,
    float angleToMove,
    float duration,
    AnimationCurve radiusMultiplierOverTime)
  {
    this.Up = up;
    this.Right = right;
    this.Radius1 = radius1;
    this.Radius2 = radius2;
    this.StartAngle = startAngle;
    this.AngleToMove = angleToMove;
    this.Duration = duration;
    this.RadiusMultiplierOverTime = radiusMultiplierOverTime;
    this.CenterOffset = centerOffset;
  }
}
