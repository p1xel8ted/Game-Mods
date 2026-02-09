// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DRails
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[HelpURL("http://www.procamera2d.com/user-guide/extension-rails/")]
public class ProCamera2DRails : BasePC2D, IPreMover
{
  public static string ExtensionName = "Rails";
  [HideInInspector]
  public List<Vector3> RailNodes = new List<Vector3>();
  public FollowMode FollowMode;
  public List<CameraTarget> CameraTargets = new List<CameraTarget>();
  public Dictionary<CameraTarget, Transform> _cameraTargetsOnRails = new Dictionary<CameraTarget, Transform>();
  public List<CameraTarget> _tempCameraTargets = new List<CameraTarget>();
  public KDTree _kdTree;
  public int _prmOrder = 1000;

  public override void Awake()
  {
    base.Awake();
    this._kdTree = KDTree.MakeFromPoints(this.RailNodes.ToArray());
    for (int index = 0; index < this.CameraTargets.Count; ++index)
    {
      Transform transform = new GameObject(this.CameraTargets[index].TargetTransform.name + "_OnRails").transform;
      this._cameraTargetsOnRails.Add(this.CameraTargets[index], transform);
      this.ProCamera2D.AddCameraTarget(transform).TargetOffset = this.CameraTargets[index].TargetOffset;
    }
    if (this.CameraTargets.Count == 0)
      this.enabled = false;
    Com.LuisPedroFonseca.ProCamera2D.ProCamera2D.Instance.AddPreMover((IPreMover) this);
    this.Step();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.ProCamera2D.RemovePreMover((IPreMover) this);
  }

  public void PreMove(float deltaTime)
  {
    if (!this.enabled)
      return;
    this.Step();
  }

  public int PrMOrder
  {
    get => this._prmOrder;
    set => this._prmOrder = value;
  }

  public void Step()
  {
    Vector3 pos = Vector3.zero;
    for (int index = 0; index < this.CameraTargets.Count; ++index)
    {
      switch (this.FollowMode)
      {
        case FollowMode.BothAxis:
          pos = this.VectorHVD(this.Vector3H(this.CameraTargets[index].TargetPosition) * this.CameraTargets[index].TargetInfluenceH, this.Vector3V(this.CameraTargets[index].TargetPosition) * this.CameraTargets[index].TargetInfluenceV, 0.0f);
          break;
        case FollowMode.HorizontalAxis:
          pos = this.VectorHVD(this.Vector3H(this.CameraTargets[index].TargetPosition) * this.CameraTargets[index].TargetInfluenceH, this.Vector3V(this.ProCamera2D.LocalPosition), 0.0f);
          break;
        case FollowMode.VerticalAxis:
          pos = this.VectorHVD(this.Vector3H(this.ProCamera2D.LocalPosition), this.Vector3V(this.CameraTargets[index].TargetPosition) * this.CameraTargets[index].TargetInfluenceV, 0.0f);
          break;
      }
      this._cameraTargetsOnRails[this.CameraTargets[index]].position = this.GetPositionOnRail(pos);
    }
  }

  public void AddRailsTarget(
    Transform targetTransform,
    float targetInfluenceH = 1f,
    float targetInfluenceV = 1f,
    Vector2 targetOffset = default (Vector2))
  {
    if (this.GetRailsTarget(targetTransform) != null)
      return;
    CameraTarget key = new CameraTarget()
    {
      TargetTransform = targetTransform,
      TargetInfluenceH = targetInfluenceH,
      TargetInfluenceV = targetInfluenceV,
      TargetOffset = targetOffset
    };
    this.CameraTargets.Add(key);
    Transform transform = new GameObject(targetTransform.name + "_OnRails").transform;
    this._cameraTargetsOnRails.Add(key, transform);
    this.ProCamera2D.AddCameraTarget(transform);
    this.enabled = true;
  }

  public void RemoveRailsTarget(Transform targetTransform)
  {
    CameraTarget railsTarget = this.GetRailsTarget(targetTransform);
    if (railsTarget == null)
      return;
    this.CameraTargets.Remove(railsTarget);
    this.ProCamera2D.RemoveCameraTarget(this._cameraTargetsOnRails[railsTarget]);
  }

  public CameraTarget GetRailsTarget(Transform targetTransform)
  {
    for (int index = 0; index < this.CameraTargets.Count; ++index)
    {
      if (this.CameraTargets[index].TargetTransform.GetInstanceID() == targetTransform.GetInstanceID())
        return this.CameraTargets[index];
    }
    return (CameraTarget) null;
  }

  public void DisableTargets(float transitionDuration = 0.0f)
  {
    if (this._tempCameraTargets.Count != 0)
      return;
    for (int index = 0; index < this._cameraTargetsOnRails.Count; ++index)
    {
      this.ProCamera2D.RemoveCameraTarget(this._cameraTargetsOnRails[this.CameraTargets[index]], transitionDuration);
      this._tempCameraTargets.Add(this.ProCamera2D.AddCameraTarget(this.CameraTargets[index].TargetTransform, this.CameraTargets[index].TargetInfluenceH, this.CameraTargets[index].TargetInfluenceV, transitionDuration, this.CameraTargets[index].TargetOffset));
    }
  }

  public void EnableTargets(float transitionDuration = 0.0f)
  {
    for (int index = 0; index < this._tempCameraTargets.Count; ++index)
    {
      this.ProCamera2D.RemoveCameraTarget(this._tempCameraTargets[index].TargetTransform, transitionDuration);
      this.ProCamera2D.AddCameraTarget(this._cameraTargetsOnRails[this.CameraTargets[index]], duration: transitionDuration);
    }
    this._tempCameraTargets.Clear();
  }

  public Vector3 GetPositionOnRail(Vector3 pos)
  {
    int nearest = this._kdTree.FindNearest(pos);
    if (nearest == 0)
      return this.GetPositionOnRailSegment(this.RailNodes[0], this.RailNodes[1], pos);
    if (nearest == this.RailNodes.Count - 1)
      return this.GetPositionOnRailSegment(this.RailNodes[this.RailNodes.Count - 1], this.RailNodes[this.RailNodes.Count - 2], pos);
    Vector3 positionOnRailSegment1 = this.GetPositionOnRailSegment(this.RailNodes[nearest - 1], this.RailNodes[nearest], pos);
    Vector3 positionOnRailSegment2 = this.GetPositionOnRailSegment(this.RailNodes[nearest + 1], this.RailNodes[nearest], pos);
    return (double) (pos - positionOnRailSegment1).sqrMagnitude <= (double) (pos - positionOnRailSegment2).sqrMagnitude ? positionOnRailSegment1 : positionOnRailSegment2;
  }

  public Vector3 GetPositionOnRailSegment(Vector3 node1, Vector3 node2, Vector3 pos)
  {
    Vector3 rhs = pos - node1;
    Vector3 normalized = (node2 - node1).normalized;
    float num = Vector3.Dot(normalized, rhs);
    if ((double) num < 0.0)
      return node1;
    if ((double) num * (double) num > (double) (node2 - node1).sqrMagnitude)
      return node2;
    Vector3 vector3 = normalized * num;
    return node1 + vector3;
  }
}
