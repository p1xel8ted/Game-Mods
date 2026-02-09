// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DCameraWindow
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[HelpURL("http://www.procamera2d.com/user-guide/extension-camera-window/")]
public class ProCamera2DCameraWindow : BasePC2D, IPositionDeltaChanger
{
  public static string ExtensionName = "Camera Window";
  public Rect CameraWindowRect = new Rect(0.0f, 0.0f, 0.3f, 0.3f);
  public Rect _cameraWindowRectInWorldCoords;
  public bool IsRelativeSizeAndPosition = true;
  public int _pdcOrder;

  public override void Awake()
  {
    base.Awake();
    this.ProCamera2D.AddPositionDeltaChanger((IPositionDeltaChanger) this);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.ProCamera2D.RemovePositionDeltaChanger((IPositionDeltaChanger) this);
  }

  public Vector3 AdjustDelta(float deltaTime, Vector3 originalDelta)
  {
    if (!this.enabled)
      return originalDelta;
    this._cameraWindowRectInWorldCoords = this.GetRectAroundTransf(this.CameraWindowRect, this.ProCamera2D.ScreenSizeInWorldCoordinates, this._transform, this.IsRelativeSizeAndPosition);
    float num1 = 0.0f;
    if ((double) this.ProCamera2D.CameraTargetPositionSmoothed.x >= (double) this._cameraWindowRectInWorldCoords.x + (double) this._cameraWindowRectInWorldCoords.width)
      num1 = this.ProCamera2D.CameraTargetPositionSmoothed.x - (float) ((double) this.Vector3H(this._transform.localPosition) + (double) this._cameraWindowRectInWorldCoords.width / 2.0 + (double) this.CameraWindowRect.x * (this.IsRelativeSizeAndPosition ? (double) this.ProCamera2D.ScreenSizeInWorldCoordinates.x : 1.0));
    else if ((double) this.ProCamera2D.CameraTargetPositionSmoothed.x <= (double) this._cameraWindowRectInWorldCoords.x)
      num1 = this.ProCamera2D.CameraTargetPositionSmoothed.x - (float) ((double) this.Vector3H(this._transform.localPosition) - (double) this._cameraWindowRectInWorldCoords.width / 2.0 + (double) this.CameraWindowRect.x * (this.IsRelativeSizeAndPosition ? (double) this.ProCamera2D.ScreenSizeInWorldCoordinates.x : 1.0));
    float num2 = 0.0f;
    if ((double) this.ProCamera2D.CameraTargetPositionSmoothed.y >= (double) this._cameraWindowRectInWorldCoords.y + (double) this._cameraWindowRectInWorldCoords.height)
      num2 = this.ProCamera2D.CameraTargetPositionSmoothed.y - (float) ((double) this.Vector3V(this._transform.localPosition) + (double) this._cameraWindowRectInWorldCoords.height / 2.0 + (double) this.CameraWindowRect.y * (this.IsRelativeSizeAndPosition ? (double) this.ProCamera2D.ScreenSizeInWorldCoordinates.y : 1.0));
    else if ((double) this.ProCamera2D.CameraTargetPositionSmoothed.y <= (double) this._cameraWindowRectInWorldCoords.y)
      num2 = this.ProCamera2D.CameraTargetPositionSmoothed.y - (float) ((double) this.Vector3V(this._transform.localPosition) - (double) this._cameraWindowRectInWorldCoords.height / 2.0 + (double) this.CameraWindowRect.y * (this.IsRelativeSizeAndPosition ? (double) this.ProCamera2D.ScreenSizeInWorldCoordinates.y : 1.0));
    return this.VectorHV(num1, num2);
  }

  public int PDCOrder
  {
    get => this._pdcOrder;
    set => this._pdcOrder = value;
  }

  public Rect GetRectAroundTransf(
    Rect rectNormalized,
    Vector2 rectSize,
    Transform transf,
    bool isRelative)
  {
    Vector2 vector2 = Vector2.Scale(new Vector2(rectNormalized.width, rectNormalized.height), isRelative ? rectSize : Vector2.one);
    return new Rect((float) ((double) this.Vector3H(transf.localPosition) - (double) vector2.x / 2.0 + (double) rectNormalized.x * (isRelative ? (double) rectSize.x : 1.0)), (float) ((double) this.Vector3V(transf.localPosition) - (double) vector2.y / 2.0 + (double) rectNormalized.y * (isRelative ? (double) rectSize.y : 1.0)), vector2.x, vector2.y);
  }
}
