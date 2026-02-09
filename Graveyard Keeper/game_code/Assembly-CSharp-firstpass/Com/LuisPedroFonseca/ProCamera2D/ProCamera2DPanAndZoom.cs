// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DPanAndZoom
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[HelpURL("http://www.procamera2d.com/user-guide/extension-pan-and-zoom/")]
public class ProCamera2DPanAndZoom : BasePC2D, ISizeDeltaChanger, IPreMover
{
  public static string ExtensionName = "Pan And Zoom";
  public bool DisableOverUGUI = true;
  public bool AllowZoom = true;
  public float MouseZoomSpeed = 10f;
  public float PinchZoomSpeed = 50f;
  [Range(0.0f, 2f)]
  public float ZoomSmoothness = 0.2f;
  public float MaxZoomInAmount = 2f;
  public float MaxZoomOutAmount = 2f;
  public bool ZoomToInputCenter = true;
  public float _zoomAmount;
  public float _initialCamSize;
  public bool _zoomStarted;
  public float _origFollowSmoothnessX;
  public float _origFollowSmoothnessY;
  public float _prevZoomAmount;
  public float _zoomVelocity;
  public Vector3 _zoomPoint;
  public float _touchZoomTime;
  public bool AllowPan = true;
  public bool UsePanByDrag = true;
  [Range(0.0f, 1f)]
  public float StopSpeedOnDragStart = 0.95f;
  public Rect DraggableAreaRect = new Rect(0.0f, 0.0f, 1f, 1f);
  public Vector2 DragPanSpeedMultiplier = new Vector2(1f, 1f);
  public bool UsePanByMoveToEdges;
  public Vector2 EdgesPanSpeed = new Vector2(2f, 2f);
  [Range(0.0f, 0.99f)]
  public float TopPanEdge = 0.9f;
  [Range(0.0f, 0.99f)]
  public float BottomPanEdge = 0.9f;
  [Range(0.0f, 0.99f)]
  public float LeftPanEdge = 0.9f;
  [Range(0.0f, 0.99f)]
  public float RightPanEdge = 0.9f;
  [HideInInspector]
  public bool ResetPrevPanPoint;
  public Vector2 _panDelta;
  public Transform _panTarget;
  public Vector3 _prevMousePosition;
  public Vector3 _prevTouchPosition;
  public bool _onMaxZoom;
  public bool _onMinZoom;
  public EventSystem _eventSystem;
  public bool _skip;
  public int _prmOrder;
  public int _sdcOrder;

  public override void Awake()
  {
    base.Awake();
    this.UpdateCurrentFollowSmoothness();
    this._eventSystem = EventSystem.current;
    this._panTarget = new GameObject("PC2DPanTarget").transform;
    this.ProCamera2D.AddPreMover((IPreMover) this);
    this.ProCamera2D.AddSizeDeltaChanger((ISizeDeltaChanger) this);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.ProCamera2D.RemovePreMover((IPreMover) this);
    this.ProCamera2D.RemoveSizeDeltaChanger((ISizeDeltaChanger) this);
  }

  public void Start()
  {
    this._initialCamSize = this.ProCamera2D.ScreenSizeInWorldCoordinates.y * 0.5f;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.CenterPanTargetOnCamera();
    Com.LuisPedroFonseca.ProCamera2D.ProCamera2D.Instance.AddCameraTarget(this._panTarget);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.ResetPrevPanPoint = true;
    this._onMaxZoom = false;
    this._onMinZoom = false;
    this.ProCamera2D.RemoveCameraTarget(this._panTarget);
  }

  public void PreMove(float deltaTime)
  {
    this._skip = this.DisableOverUGUI && (bool) (Object) this._eventSystem && this._eventSystem.IsPointerOverGameObject();
    if (this._skip)
    {
      this._prevMousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(this.Vector3D(this.ProCamera2D.LocalPosition)));
      this.CancelZoom();
    }
    if (!this.enabled || !this.AllowPan || this._skip)
      return;
    this.Pan(deltaTime);
  }

  public int PrMOrder
  {
    get => this._prmOrder;
    set => this._prmOrder = value;
  }

  public float AdjustSize(float deltaTime, float originalDelta)
  {
    return this.enabled && this.AllowZoom && !this._skip ? this.Zoom(deltaTime) + originalDelta : originalDelta;
  }

  public int SDCOrder
  {
    get => this._sdcOrder;
    set => this._sdcOrder = value;
  }

  public void Pan(float deltaTime)
  {
    this._panDelta = Vector2.zero;
    Vector2 vector2 = this.DragPanSpeedMultiplier;
    if (this.UsePanByDrag && Input.GetMouseButtonDown(0))
      this.CenterPanTargetOnCamera(this.StopSpeedOnDragStart);
    Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(this.Vector3D(this.ProCamera2D.LocalPosition)));
    if (this.UsePanByDrag && Input.GetMouseButton(0))
    {
      if (this.InsideDraggableArea(new Vector2(Input.mousePosition.x / (float) Screen.width, Input.mousePosition.y / (float) Screen.height)))
      {
        Vector3 worldPoint1 = this.ProCamera2D.GameCamera.ScreenToWorldPoint(this._prevMousePosition);
        if (this.ResetPrevPanPoint)
        {
          worldPoint1 = this.ProCamera2D.GameCamera.ScreenToWorldPoint(position);
          this.ResetPrevPanPoint = false;
        }
        Vector3 worldPoint2 = this.ProCamera2D.GameCamera.ScreenToWorldPoint(position);
        Vector3 vector3 = worldPoint1 - worldPoint2;
        this._panDelta = new Vector2(this.Vector3H(vector3), this.Vector3V(vector3));
      }
    }
    else if (this.UsePanByMoveToEdges && !Input.GetMouseButton(0))
    {
      float x = ((float) -Screen.width * 0.5f + Input.mousePosition.x) / (float) Screen.width;
      float y = ((float) -Screen.height * 0.5f + Input.mousePosition.y) / (float) Screen.height;
      if ((double) x < 0.0)
        x = x.Remap(-0.5f, (float) (-(double) this.LeftPanEdge * 0.5), -0.5f, 0.0f);
      else if ((double) x > 0.0)
        x = x.Remap(this.RightPanEdge * 0.5f, 0.5f, 0.0f, 0.5f);
      if ((double) y < 0.0)
        y = y.Remap(-0.5f, (float) (-(double) this.BottomPanEdge * 0.5), -0.5f, 0.0f);
      else if ((double) y > 0.0)
        y = y.Remap(this.TopPanEdge * 0.5f, 0.5f, 0.0f, 0.5f);
      this._panDelta = new Vector2(x, y) * deltaTime;
      if (this._panDelta != Vector2.zero)
        vector2 = this.EdgesPanSpeed;
    }
    this._prevMousePosition = position;
    if (this._panDelta != Vector2.zero)
      this._panTarget.Translate(this.VectorHV(this._panDelta.x * vector2.x, this._panDelta.y * vector2.y));
    if (this.ProCamera2D.IsCameraPositionLeftBounded && (double) this.Vector3H(this._panTarget.position) < (double) this.Vector3H(this.ProCamera2D.LocalPosition) || this.ProCamera2D.IsCameraPositionRightBounded && (double) this.Vector3H(this._panTarget.position) > (double) this.Vector3H(this.ProCamera2D.LocalPosition))
      this._panTarget.position = this.VectorHVD(this.Vector3H(this.ProCamera2D.LocalPosition), this.Vector3V(this._panTarget.position), this.Vector3D(this._panTarget.position));
    if ((!this.ProCamera2D.IsCameraPositionBottomBounded || (double) this.Vector3V(this._panTarget.position) >= (double) this.Vector3V(this.ProCamera2D.LocalPosition)) && (!this.ProCamera2D.IsCameraPositionTopBounded || (double) this.Vector3V(this._panTarget.position) <= (double) this.Vector3V(this.ProCamera2D.LocalPosition)))
      return;
    this._panTarget.position = this.VectorHVD(this.Vector3H(this._panTarget.position), this.Vector3V(this.ProCamera2D.LocalPosition), this.Vector3D(this._panTarget.position));
  }

  public float Zoom(float deltaTime)
  {
    if (this._panDelta != Vector2.zero)
    {
      this.CancelZoom();
      this.RestoreFollowSmoothness();
      return 0.0f;
    }
    float axis = Input.GetAxis("Mouse ScrollWheel");
    this._zoomPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(this.Vector3D(this.ProCamera2D.LocalPosition)));
    float mouseZoomSpeed = this.MouseZoomSpeed;
    if (this._onMaxZoom && (double) axis * (double) mouseZoomSpeed < 0.0 || this._onMinZoom && (double) axis * (double) mouseZoomSpeed > 0.0)
    {
      this.CancelZoom();
      return 0.0f;
    }
    this._zoomAmount = Mathf.SmoothDamp(this._prevZoomAmount, axis * mouseZoomSpeed * deltaTime, ref this._zoomVelocity, this.ZoomSmoothness, float.MaxValue, deltaTime);
    if ((double) Mathf.Abs(this._zoomAmount) <= 9.9999997473787516E-05)
    {
      if (this._zoomStarted)
        this.RestoreFollowSmoothness();
      this._zoomStarted = false;
      this._prevZoomAmount = 0.0f;
      return 0.0f;
    }
    if (!this._zoomStarted)
    {
      this._zoomStarted = true;
      this._panTarget.position = this.ProCamera2D.LocalPosition - this.ProCamera2D.InfluencesSum;
      this.UpdateCurrentFollowSmoothness();
      this.RemoveFollowSmoothness();
    }
    float num1 = this.ProCamera2D.ScreenSizeInWorldCoordinates.y / 2f + this._zoomAmount;
    float num2 = this._initialCamSize / this.MaxZoomInAmount;
    float num3 = this.MaxZoomOutAmount * this._initialCamSize;
    this._onMaxZoom = false;
    this._onMinZoom = false;
    if ((double) num1 < (double) num2)
    {
      this._zoomAmount -= num1 - num2;
      this._onMaxZoom = true;
    }
    else if ((double) num1 > (double) num3)
    {
      this._zoomAmount -= num1 - num3;
      this._onMinZoom = true;
    }
    this._prevZoomAmount = this._zoomAmount;
    if (this.ZoomToInputCenter && (double) this._zoomAmount != 0.0)
    {
      float num4 = this._zoomAmount / (this.ProCamera2D.ScreenSizeInWorldCoordinates.y / 2f);
      this._panTarget.position += (this._panTarget.position - this.ProCamera2D.GameCamera.ScreenToWorldPoint(this._zoomPoint)) * num4;
    }
    return this._zoomAmount;
  }

  public void UpdateCurrentFollowSmoothness()
  {
    this._origFollowSmoothnessX = this.ProCamera2D.HorizontalFollowSmoothness;
    this._origFollowSmoothnessY = this.ProCamera2D.VerticalFollowSmoothness;
  }

  public void CenterPanTargetOnCamera(float interpolant = 1f)
  {
    if (!((Object) this._panTarget != (Object) null))
      return;
    this._panTarget.position = Vector3.Lerp(this._panTarget.position, this.VectorHV(this.Vector3H(this.ProCamera2D.LocalPosition), this.Vector3V(this.ProCamera2D.LocalPosition)), interpolant);
  }

  public void CancelZoom()
  {
    this._zoomAmount = 0.0f;
    this._prevZoomAmount = 0.0f;
    this._zoomVelocity = 0.0f;
  }

  public void RestoreFollowSmoothness()
  {
    this.ProCamera2D.HorizontalFollowSmoothness = this._origFollowSmoothnessX;
    this.ProCamera2D.VerticalFollowSmoothness = this._origFollowSmoothnessY;
  }

  public void RemoveFollowSmoothness()
  {
    this.ProCamera2D.HorizontalFollowSmoothness = 0.0f;
    this.ProCamera2D.VerticalFollowSmoothness = 0.0f;
  }

  public bool InsideDraggableArea(Vector2 normalizedInput)
  {
    return (double) this.DraggableAreaRect.x == 0.0 && (double) this.DraggableAreaRect.y == 0.0 && (double) this.DraggableAreaRect.width == 1.0 && (double) this.DraggableAreaRect.height == 1.0 || (double) normalizedInput.x > (double) this.DraggableAreaRect.x + (1.0 - (double) this.DraggableAreaRect.width) / 2.0 && (double) normalizedInput.x < (double) this.DraggableAreaRect.x + (double) this.DraggableAreaRect.width + (1.0 - (double) this.DraggableAreaRect.width) / 2.0 && (double) normalizedInput.y > (double) this.DraggableAreaRect.y + (1.0 - (double) this.DraggableAreaRect.height) / 2.0 && (double) normalizedInput.y < (double) this.DraggableAreaRect.y + (double) this.DraggableAreaRect.height + (1.0 - (double) this.DraggableAreaRect.height) / 2.0;
  }
}
