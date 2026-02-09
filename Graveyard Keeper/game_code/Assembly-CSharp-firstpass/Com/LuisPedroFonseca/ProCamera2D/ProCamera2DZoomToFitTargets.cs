// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DZoomToFitTargets
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[HelpURL("http://www.procamera2d.com/user-guide/extension-zoom-to-fit/")]
public class ProCamera2DZoomToFitTargets : BasePC2D, ISizeOverrider
{
  public static string ExtensionName = "Zoom To Fit";
  public float ZoomOutBorder = 0.6f;
  public float ZoomInBorder = 0.4f;
  public float ZoomInSmoothness = 2f;
  public float ZoomOutSmoothness = 1f;
  public float MaxZoomInAmount = 2f;
  public float MaxZoomOutAmount = 4f;
  public bool DisableWhenOneTarget = true;
  public float _zoomVelocity;
  public float _initialCamSize;
  public float _previousCamSize;
  public float _targetCamSize;
  public float _targetCamSizeSmoothed;
  public float _minCameraSize;
  public float _maxCameraSize;
  public int _soOrder;

  public override void Awake()
  {
    base.Awake();
    if ((Object) this.ProCamera2D == (Object) null)
      return;
    this._initialCamSize = this.ProCamera2D.ScreenSizeInWorldCoordinates.y * 0.5f;
    this._targetCamSize = this._initialCamSize;
    this._targetCamSizeSmoothed = this._targetCamSize;
    this.ProCamera2D.AddSizeOverrider((ISizeOverrider) this);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.ProCamera2D.RemoveSizeOverrider((ISizeOverrider) this);
  }

  public float OverrideSize(float deltaTime, float originalSize)
  {
    if (!this.enabled)
      return originalSize;
    this._targetCamSizeSmoothed = this.ProCamera2D.ScreenSizeInWorldCoordinates.y * 0.5f;
    if (this.DisableWhenOneTarget && this.ProCamera2D.CameraTargets.Count <= 1)
    {
      this._targetCamSize = this._initialCamSize;
    }
    else
    {
      if ((double) this._previousCamSize == (double) this.ProCamera2D.ScreenSizeInWorldCoordinates.y)
      {
        this._targetCamSize = this.ProCamera2D.ScreenSizeInWorldCoordinates.y * 0.5f;
        this._targetCamSizeSmoothed = this._targetCamSize;
        this._zoomVelocity = 0.0f;
      }
      this.UpdateTargetCamSize();
    }
    this._previousCamSize = this.ProCamera2D.ScreenSizeInWorldCoordinates.y;
    return this._targetCamSizeSmoothed = Mathf.SmoothDamp(this._targetCamSizeSmoothed, this._targetCamSize, ref this._zoomVelocity, (double) this._targetCamSize < (double) this._targetCamSizeSmoothed ? this.ZoomInSmoothness : this.ZoomOutSmoothness, float.MaxValue, deltaTime);
  }

  public int SOOrder
  {
    get => this._soOrder;
    set => this._soOrder = value;
  }

  public override void OnReset()
  {
    this._zoomVelocity = 0.0f;
    this._previousCamSize = this._initialCamSize;
    this._targetCamSize = this._initialCamSize;
    this._targetCamSizeSmoothed = this._initialCamSize;
  }

  public void UpdateTargetCamSize()
  {
    float num1 = float.NegativeInfinity;
    float num2 = float.PositiveInfinity;
    float num3 = float.NegativeInfinity;
    float num4 = float.PositiveInfinity;
    for (int index = 0; index < this.ProCamera2D.CameraTargets.Count; ++index)
    {
      Vector2 vector2 = new Vector2(this.Vector3H(this.ProCamera2D.CameraTargets[index].TargetPosition) + this.ProCamera2D.CameraTargets[index].TargetOffset.x, this.Vector3V(this.ProCamera2D.CameraTargets[index].TargetPosition) + this.ProCamera2D.CameraTargets[index].TargetOffset.y);
      num1 = (double) vector2.x > (double) num1 ? vector2.x : num1;
      num2 = (double) vector2.x < (double) num2 ? vector2.x : num2;
      num3 = (double) vector2.y > (double) num3 ? vector2.y : num3;
      num4 = (double) vector2.y < (double) num4 ? vector2.y : num4;
    }
    float num5 = Mathf.Abs(num1 - num2) * 0.5f;
    float num6 = Mathf.Abs(num3 - num4) * 0.5f;
    if ((double) num5 > (double) this.ProCamera2D.ScreenSizeInWorldCoordinates.x * (double) this.ZoomOutBorder * 0.5 || (double) num6 > (double) this.ProCamera2D.ScreenSizeInWorldCoordinates.y * (double) this.ZoomOutBorder * 0.5)
      this._targetCamSize = (double) num5 / (double) this.ProCamera2D.ScreenSizeInWorldCoordinates.x < (double) num6 / (double) this.ProCamera2D.ScreenSizeInWorldCoordinates.y ? num6 / this.ZoomOutBorder : num5 / this.ProCamera2D.GameCamera.aspect / this.ZoomOutBorder;
    else if ((double) num5 < (double) this.ProCamera2D.ScreenSizeInWorldCoordinates.x * (double) this.ZoomInBorder * 0.5 && (double) num6 < (double) this.ProCamera2D.ScreenSizeInWorldCoordinates.y * (double) this.ZoomInBorder * 0.5)
      this._targetCamSize = (double) num5 / (double) this.ProCamera2D.ScreenSizeInWorldCoordinates.x < (double) num6 / (double) this.ProCamera2D.ScreenSizeInWorldCoordinates.y ? num6 / this.ZoomInBorder : num5 / this.ProCamera2D.GameCamera.aspect / this.ZoomInBorder;
    this._minCameraSize = this._initialCamSize / this.MaxZoomInAmount;
    this._maxCameraSize = this._initialCamSize * this.MaxZoomOutAmount;
    this._targetCamSize = Mathf.Clamp(this._targetCamSize, this._minCameraSize, this._maxCameraSize);
  }
}
