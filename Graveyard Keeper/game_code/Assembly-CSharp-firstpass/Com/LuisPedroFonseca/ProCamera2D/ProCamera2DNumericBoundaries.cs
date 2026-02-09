// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DNumericBoundaries
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[HelpURL("http://www.procamera2d.com/user-guide/extension-numeric-boundaries/")]
public class ProCamera2DNumericBoundaries : BasePC2D, IPositionDeltaChanger, ISizeOverrider
{
  public static string ExtensionName = "Numeric Boundaries";
  public Action OnBoundariesTransitionStarted;
  public Action OnBoundariesTransitionFinished;
  public bool UseNumericBoundaries = true;
  public bool UseTopBoundary;
  public float TopBoundary = 10f;
  public float TargetTopBoundary;
  public bool UseBottomBoundary = true;
  public float BottomBoundary = -10f;
  public float TargetBottomBoundary;
  public bool UseLeftBoundary;
  public float LeftBoundary = -10f;
  public float TargetLeftBoundary;
  public bool UseRightBoundary;
  public float RightBoundary = 10f;
  public float TargetRightBoundary;
  public bool IsCameraPositionHorizontallyBounded;
  public bool IsCameraPositionVerticallyBounded;
  public Coroutine TopBoundaryAnimRoutine;
  public Coroutine BottomBoundaryAnimRoutine;
  public Coroutine LeftBoundaryAnimRoutine;
  public Coroutine RightBoundaryAnimRoutine;
  public ProCamera2DTriggerBoundaries CurrentBoundariesTrigger;
  public Coroutine MoveCameraToTargetRoutine;
  public bool HasFiredTransitionStarted;
  public bool HasFiredTransitionFinished;
  public bool UseSoftBoundaries = true;
  [Range(0.0f, 4f)]
  public float Softness = 0.5f;
  [Range(0.0f, 0.5f)]
  public float SoftAreaSize = 0.1f;
  public float _smoothnessVelX;
  public float _smoothnessVelY;
  public int _pdcOrder = 4000;
  public int _soOrder = 2000;

  public NumericBoundariesSettings Settings
  {
    get
    {
      return new NumericBoundariesSettings()
      {
        UseNumericBoundaries = this.UseNumericBoundaries,
        UseTopBoundary = this.UseTopBoundary,
        TopBoundary = this.TopBoundary,
        UseBottomBoundary = this.UseBottomBoundary,
        BottomBoundary = this.BottomBoundary,
        UseLeftBoundary = this.UseLeftBoundary,
        LeftBoundary = this.LeftBoundary,
        UseRightBoundary = this.UseRightBoundary,
        RightBoundary = this.RightBoundary
      };
    }
    set
    {
      this.UseNumericBoundaries = value.UseNumericBoundaries;
      this.UseTopBoundary = value.UseTopBoundary;
      this.TopBoundary = value.TopBoundary;
      this.UseBottomBoundary = value.UseBottomBoundary;
      this.BottomBoundary = value.BottomBoundary;
      this.UseLeftBoundary = value.UseLeftBoundary;
      this.LeftBoundary = value.LeftBoundary;
      this.UseRightBoundary = value.UseRightBoundary;
      this.RightBoundary = value.RightBoundary;
    }
  }

  public override void Awake()
  {
    base.Awake();
    this.ProCamera2D.AddPositionDeltaChanger((IPositionDeltaChanger) this);
    this.ProCamera2D.AddSizeOverrider((ISizeOverrider) this);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.ProCamera2D.RemovePositionDeltaChanger((IPositionDeltaChanger) this);
    this.ProCamera2D.RemoveSizeOverrider((ISizeOverrider) this);
  }

  public Vector3 AdjustDelta(float deltaTime, Vector3 originalDelta)
  {
    if (!this.enabled || !this.UseNumericBoundaries)
      return originalDelta;
    this.IsCameraPositionHorizontallyBounded = false;
    this.ProCamera2D.IsCameraPositionLeftBounded = false;
    this.ProCamera2D.IsCameraPositionRightBounded = false;
    this.IsCameraPositionVerticallyBounded = false;
    this.ProCamera2D.IsCameraPositionTopBounded = false;
    this.ProCamera2D.IsCameraPositionBottomBounded = false;
    float b1 = this.Vector3H(this.ProCamera2D.LocalPosition) + this.Vector3H(originalDelta);
    float b2 = this.Vector3V(this.ProCamera2D.LocalPosition) + this.Vector3V(originalDelta);
    float num1 = this.ProCamera2D.ScreenSizeInWorldCoordinates.x * 0.5f;
    float num2 = this.ProCamera2D.ScreenSizeInWorldCoordinates.y * 0.5f;
    float num3 = this.UseSoftBoundaries ? this.ProCamera2D.ScreenSizeInWorldCoordinates.x * this.SoftAreaSize : 0.0f;
    float num4 = this.UseSoftBoundaries ? this.ProCamera2D.ScreenSizeInWorldCoordinates.y * this.SoftAreaSize : 0.0f;
    if (this.UseLeftBoundary && (double) b1 - (double) num1 < (double) this.LeftBoundary + (double) num3)
    {
      if (this.UseSoftBoundaries)
        b1 = (double) this.Vector3H(originalDelta) > 0.0 ? Mathf.Max(this.LeftBoundary + num1, b1) : Mathf.SmoothDamp(Mathf.Max(this.LeftBoundary + num1, this.Vector3H(this.ProCamera2D.LocalPosition)), Mathf.Max(this.LeftBoundary + num1, b1), ref this._smoothnessVelX, (this.LeftBoundary + num1 - this.Vector3H(this.ProCamera2D.LocalPosition) + num3) / num3 * this.Softness);
      else if (!this.UseSoftBoundaries)
        b1 = this.LeftBoundary + num1;
      this.IsCameraPositionHorizontallyBounded = true;
      this.ProCamera2D.IsCameraPositionLeftBounded = true;
    }
    if (this.UseRightBoundary && (double) b1 + (double) num1 > (double) this.RightBoundary - (double) num3)
    {
      if (this.UseSoftBoundaries)
        b1 = (double) this.Vector3H(originalDelta) < 0.0 ? Mathf.Min(this.RightBoundary - num1, b1) : Mathf.SmoothDamp(Mathf.Min(this.RightBoundary - num1, this.Vector3H(this.ProCamera2D.LocalPosition)), Mathf.Min(this.RightBoundary - num1, b1), ref this._smoothnessVelX, (this.Vector3H(this.ProCamera2D.LocalPosition) - (this.RightBoundary - num1) + num3) / num3 * this.Softness);
      else if (!this.UseSoftBoundaries)
        b1 = this.RightBoundary - num1;
      this.IsCameraPositionHorizontallyBounded = true;
      this.ProCamera2D.IsCameraPositionRightBounded = true;
    }
    if (this.UseBottomBoundary && (double) b2 - (double) num2 < (double) this.BottomBoundary + (double) num4)
    {
      if (this.UseSoftBoundaries)
        b2 = (double) this.Vector3V(originalDelta) > 0.0 ? Mathf.Max(this.BottomBoundary + num2, b2) : Mathf.SmoothDamp(Mathf.Max(this.BottomBoundary + num2, this.Vector3V(this.ProCamera2D.LocalPosition)), Mathf.Max(this.BottomBoundary + num2, b2), ref this._smoothnessVelY, (this.BottomBoundary + num2 + num4 - this.Vector3V(this.ProCamera2D.LocalPosition)) / num3 * this.Softness);
      else if (!this.UseSoftBoundaries)
        b2 = this.BottomBoundary + num2;
      this.IsCameraPositionVerticallyBounded = true;
      this.ProCamera2D.IsCameraPositionBottomBounded = true;
    }
    if (this.UseTopBoundary && (double) b2 + (double) num2 > (double) this.TopBoundary - (double) num4)
    {
      if (this.UseSoftBoundaries)
        b2 = (double) this.Vector3V(originalDelta) < 0.0 ? Mathf.Min(this.TopBoundary - num2, b2) : Mathf.SmoothDamp(Mathf.Min(this.TopBoundary - num2, this.Vector3V(this.ProCamera2D.LocalPosition)), Mathf.Min(this.TopBoundary - num2, b2), ref this._smoothnessVelY, (this.Vector3V(this.ProCamera2D.LocalPosition) - (this.TopBoundary - num2) + num4) / num3 * this.Softness);
      else if (!this.UseSoftBoundaries)
        b2 = this.TopBoundary - num2;
      this.IsCameraPositionVerticallyBounded = true;
      this.ProCamera2D.IsCameraPositionTopBounded = true;
    }
    return this.VectorHV(b1 - this.Vector3H(this.ProCamera2D.LocalPosition), b2 - this.Vector3V(this.ProCamera2D.LocalPosition));
  }

  public int PDCOrder
  {
    get => this._pdcOrder;
    set => this._pdcOrder = value;
  }

  public float OverrideSize(float deltaTime, float originalSize)
  {
    if (!this.enabled || !this.UseNumericBoundaries)
      return originalSize;
    float num = originalSize;
    Vector2 vector2 = new Vector2(this.RightBoundary - this.LeftBoundary, this.TopBoundary - this.BottomBoundary);
    if (this.UseRightBoundary && this.UseLeftBoundary && (double) originalSize * (double) this.ProCamera2D.GameCamera.aspect * 2.0 > (double) vector2.x)
      num = (float) ((double) vector2.x / (double) this.ProCamera2D.GameCamera.aspect * 0.5);
    if (this.UseTopBoundary && this.UseBottomBoundary && (double) num * 2.0 > (double) vector2.y)
      num = vector2.y * 0.5f;
    return num;
  }

  public int SOOrder
  {
    get => this._soOrder;
    set => this._soOrder = value;
  }
}
