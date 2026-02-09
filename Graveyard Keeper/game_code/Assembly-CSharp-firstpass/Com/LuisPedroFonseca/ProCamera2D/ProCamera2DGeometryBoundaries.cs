// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DGeometryBoundaries
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[HelpURL("http://www.procamera2d.com/user-guide/extension-geometry-boundaries/")]
public class ProCamera2DGeometryBoundaries : BasePC2D, IPositionDeltaChanger
{
  public static string ExtensionName = "Geometry Boundaries";
  [Tooltip("The layer that contains the (3d) colliders that define the boundaries for the camera")]
  public LayerMask BoundariesLayerMask;
  public MoveInColliderBoundaries _cameraMoveInColliderBoundaries;
  public int _pdcOrder = 3000;

  public override void Awake()
  {
    base.Awake();
    this._cameraMoveInColliderBoundaries = new MoveInColliderBoundaries(this.ProCamera2D);
    this._cameraMoveInColliderBoundaries.CameraTransform = this.ProCamera2D.transform;
    this._cameraMoveInColliderBoundaries.CameraCollisionMask = this.BoundariesLayerMask;
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
    this._cameraMoveInColliderBoundaries.CameraSize = this.ProCamera2D.ScreenSizeInWorldCoordinates;
    return this._cameraMoveInColliderBoundaries.Move(originalDelta);
  }

  public int PDCOrder
  {
    get => this._pdcOrder;
    set => this._pdcOrder = value;
  }
}
