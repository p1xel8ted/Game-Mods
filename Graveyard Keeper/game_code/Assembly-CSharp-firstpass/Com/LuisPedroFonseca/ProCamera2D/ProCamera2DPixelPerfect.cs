// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DPixelPerfect
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[HelpURL("http://www.procamera2d.com/user-guide/extension-pixel-perfect/")]
public class ProCamera2DPixelPerfect : BasePC2D, IPositionOverrider
{
  public static string ExtensionName = "Pixel Perfect";
  public float PixelsPerUnit = 32f;
  public AutoScaleMode ViewportAutoScale = AutoScaleMode.Round;
  public Vector2 TargetViewportSizeInPixels = new Vector2(80f, 50f);
  [Range(1f, 32f)]
  public int Zoom = 1;
  public bool SnapMovementToGrid;
  public bool SnapCameraToGrid = true;
  public bool DrawGrid;
  public Color GridColor = new Color(1f, 0.0f, 0.0f, 0.1f);
  public float GridDensity;
  public float _pixelStep = -1f;
  public float _viewportScale;
  public Transform _parent;
  public int _poOrder = 2000;

  public float PixelStep => this._pixelStep;

  public override void Awake()
  {
    base.Awake();
    if (!this.ProCamera2D.GameCamera.orthographic)
    {
      this.enabled = false;
    }
    else
    {
      this.ResizeCameraToPixelPerfect();
      this.ProCamera2D.AddPositionOverrider((IPositionOverrider) this);
    }
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.ProCamera2D.RemovePositionOverrider((IPositionOverrider) this);
  }

  public Vector3 OverridePosition(float deltaTime, Vector3 originalPosition)
  {
    if (!this.enabled)
      return originalPosition;
    float gridSize = this._pixelStep;
    if (this.SnapMovementToGrid && !this.SnapCameraToGrid)
      gridSize = (float) (1.0 / ((double) this.PixelsPerUnit * ((double) this._viewportScale + (double) this.Zoom - 1.0)));
    this._parent = this._transform.parent;
    if ((Object) this._parent != (Object) null && this._parent.position != Vector3.zero)
      this._parent.position = this.VectorHVD(Utils.AlignToGrid(this.Vector3H(this._parent.position), gridSize), Utils.AlignToGrid(this.Vector3V(this._parent.position), gridSize), this.Vector3D(this._parent.position));
    return this.VectorHVD(Utils.AlignToGrid(this.Vector3H(originalPosition), gridSize), Utils.AlignToGrid(this.Vector3V(originalPosition), gridSize), 0.0f);
  }

  public int POOrder
  {
    get => this._poOrder;
    set => this._poOrder = value;
  }

  public void ResizeCameraToPixelPerfect()
  {
    this._viewportScale = this.CalculateViewportScale();
    this._pixelStep = this.CalculatePixelStep(this._viewportScale);
    this.ProCamera2D.UpdateScreenSize((float) ((double) this.ProCamera2D.GameCamera.pixelHeight * 0.5 * (1.0 / (double) this.PixelsPerUnit) / ((double) this._viewportScale + (double) this.Zoom - 1.0)));
  }

  public float CalculateViewportScale()
  {
    if (this.ViewportAutoScale == AutoScaleMode.None)
      return 1f;
    float num1 = (float) this.ProCamera2D.GameCamera.pixelWidth / this.TargetViewportSizeInPixels.x;
    float num2 = (float) this.ProCamera2D.GameCamera.pixelHeight / this.TargetViewportSizeInPixels.y;
    float f = (double) num1 > (double) num2 ? num2 : num1;
    switch (this.ViewportAutoScale)
    {
      case AutoScaleMode.Floor:
        f = Mathf.Floor(f);
        break;
      case AutoScaleMode.Ceil:
        f = Mathf.Ceil(f);
        break;
      case AutoScaleMode.Round:
        f = Mathf.Round(f);
        break;
    }
    if ((double) f < 1.0)
      f = 1f;
    return f;
  }

  public float CalculatePixelStep(float viewportScale)
  {
    return !this.SnapMovementToGrid ? (float) (1.0 / ((double) this.PixelsPerUnit * ((double) viewportScale + (double) this.Zoom - 1.0))) : 1f / this.PixelsPerUnit;
  }
}
