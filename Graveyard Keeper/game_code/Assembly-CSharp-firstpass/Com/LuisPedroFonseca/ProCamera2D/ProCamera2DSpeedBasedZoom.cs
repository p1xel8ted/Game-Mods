// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DSpeedBasedZoom
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[HelpURL("http://www.procamera2d.com/user-guide/extension-speed-based-zoom/")]
public class ProCamera2DSpeedBasedZoom : BasePC2D, ISizeDeltaChanger
{
  public static string ExtensionName = "Speed Based Zoom";
  [Tooltip("The speed at which the camera will reach it's max zoom out.")]
  public float CamVelocityForZoomOut = 5f;
  [Tooltip("Below this speed the camera zooms in. Above this speed the camera will start zooming out.")]
  public float CamVelocityForZoomIn = 2f;
  [Tooltip("Represents how smooth the zoom in of the camera should be. The lower the number the quickest the zoom is.")]
  [Range(0.0f, 3f)]
  public float ZoomInSmoothness = 1f;
  [Range(0.0f, 3f)]
  [Tooltip("Represents how smooth the zoom out of the camera should be. The lower the number the quickest the zoom is.")]
  public float ZoomOutSmoothness = 1f;
  [Tooltip("Represents the maximum amount the camera should zoom in when the camera speed is below SpeedForZoomIn")]
  public float MaxZoomInAmount = 2f;
  [Tooltip("Represents the maximum amount the camera should zoom out when the camera speed is equal to SpeedForZoomOut")]
  public float MaxZoomOutAmount = 2f;
  public float _zoomVelocity;
  public float _initialCamSize;
  public float _previousCamSize;
  public Vector3 _previousCameraPosition;
  [HideInInspector]
  public float CurrentVelocity;
  public int _sdcOrder = 1000;

  public override void Awake()
  {
    base.Awake();
    if ((Object) this.ProCamera2D == (Object) null)
      return;
    this._initialCamSize = this.ProCamera2D.ScreenSizeInWorldCoordinates.y * 0.5f;
    this._previousCamSize = this._initialCamSize;
    this._previousCameraPosition = this.VectorHV(this.Vector3H(this.ProCamera2D.LocalPosition), this.Vector3V(this.ProCamera2D.LocalPosition));
    this.ProCamera2D.AddSizeDeltaChanger((ISizeDeltaChanger) this);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.ProCamera2D.RemoveSizeDeltaChanger((ISizeDeltaChanger) this);
  }

  public float AdjustSize(float deltaTime, float originalDelta)
  {
    if (!this.enabled)
      return originalDelta;
    if ((double) this._previousCamSize == (double) this.ProCamera2D.ScreenSizeInWorldCoordinates.y)
      this._zoomVelocity = 0.0f;
    this.CurrentVelocity = (this._previousCameraPosition - this.VectorHV(this.Vector3H(this.ProCamera2D.LocalPosition), this.Vector3V(this.ProCamera2D.LocalPosition))).magnitude / deltaTime;
    this._previousCameraPosition = this.VectorHV(this.Vector3H(this.ProCamera2D.LocalPosition), this.Vector3V(this.ProCamera2D.LocalPosition));
    float current = this.ProCamera2D.ScreenSizeInWorldCoordinates.y * 0.5f;
    float target = current;
    if ((double) this.CurrentVelocity > (double) this.CamVelocityForZoomIn)
    {
      float num = this._initialCamSize * (float) (1.0 + (double) this.MaxZoomOutAmount - 1.0) * Mathf.Clamp01((float) (((double) this.CurrentVelocity - (double) this.CamVelocityForZoomIn) / ((double) this.CamVelocityForZoomOut - (double) this.CamVelocityForZoomIn)));
      if ((double) num > (double) current)
        target = num;
    }
    else
    {
      float num = this._initialCamSize / (this.MaxZoomInAmount * ((float) (1.0 - (double) this.CurrentVelocity / (double) this.CamVelocityForZoomIn)).Remap(0.0f, 1f, 0.5f, 1f));
      if ((double) num < (double) current)
        target = num;
    }
    if ((double) Mathf.Abs(current - target) > 9.9999997473787516E-05)
    {
      float smoothTime = (double) target < (double) current ? this.ZoomInSmoothness : this.ZoomOutSmoothness;
      target = Mathf.SmoothDamp(current, target, ref this._zoomVelocity, smoothTime, float.PositiveInfinity, deltaTime);
    }
    float num1 = target - this.ProCamera2D.ScreenSizeInWorldCoordinates.y / 2f;
    this._previousCamSize = this.ProCamera2D.ScreenSizeInWorldCoordinates.y;
    return originalDelta + num1;
  }

  public int SDCOrder
  {
    get => this._sdcOrder;
    set => this._sdcOrder = value;
  }

  public override void OnReset()
  {
    this._previousCamSize = this._initialCamSize;
    this._previousCameraPosition = this.VectorHV(this.Vector3H(this.ProCamera2D.LocalPosition), this.Vector3V(this.ProCamera2D.LocalPosition));
    this._zoomVelocity = 0.0f;
  }
}
