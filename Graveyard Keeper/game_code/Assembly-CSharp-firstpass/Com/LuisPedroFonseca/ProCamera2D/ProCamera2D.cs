// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2D
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[HelpURL("http://www.procamera2d.com/user-guide/core/")]
[RequireComponent(typeof (Camera))]
public class ProCamera2D : MonoBehaviour, ISerializationCallbackReceiver
{
  public const string Title = "Pro Camera 2D";
  public static Version Version = new Version("2.4.4");
  public List<CameraTarget> CameraTargets = new List<CameraTarget>();
  public bool CenterTargetOnStart;
  public MovementAxis Axis;
  public UpdateType UpdateType;
  public bool FollowHorizontal = true;
  public float HorizontalFollowSmoothness = 0.15f;
  public bool FollowVertical = true;
  public float VerticalFollowSmoothness = 0.15f;
  [Range(-1f, 1f)]
  public float OffsetX;
  [Range(-1f, 1f)]
  public float OffsetY;
  public bool IsRelativeOffset = true;
  public bool ZoomWithFOV;
  public static Com.LuisPedroFonseca.ProCamera2D.ProCamera2D _instance;
  public float _cameraTargetHorizontalPositionSmoothed;
  public float _cameraTargetVerticalPositionSmoothed;
  public Vector2 _screenSizeInWorldCoordinates;
  public Vector3 _previousTargetsMidPoint;
  public Vector3 _targetsMidPoint;
  public Vector3 _cameraTargetPosition;
  public float _deltaTime;
  public Vector3 _parentPosition;
  public Vector3 _influencesSum = Vector3.zero;
  public Action<float> PreMoveUpdate;
  public Action<float> PostMoveUpdate;
  public Action<Vector2> OnCameraResize;
  public Action OnReset;
  public Vector3? ExclusiveTargetPosition;
  public int CurrentZoomTriggerID;
  public bool IsCameraPositionLeftBounded;
  public bool IsCameraPositionRightBounded;
  public bool IsCameraPositionTopBounded;
  public bool IsCameraPositionBottomBounded;
  public Camera GameCamera;
  public Func<Vector3, float> Vector3H;
  public Func<Vector3, float> Vector3V;
  public Func<Vector3, float> Vector3D;
  public Func<float, float, Vector3> VectorHV;
  public Func<float, float, float, Vector3> VectorHVD;
  public Vector2 _startScreenSizeInWorldCoordinates;
  public Coroutine _updateScreenSizeCoroutine;
  public Coroutine _dollyZoomRoutine;
  public List<Vector3> _influences = new List<Vector3>();
  public float _originalCameraDepthSign;
  public float _previousCameraTargetHorizontalPositionSmoothed;
  public float _previousCameraTargetVerticalPositionSmoothed;
  public int _previousScreenWidth;
  public int _previousScreenHeight;
  public Vector3 _previousCameraPosition;
  public WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
  public Transform _transform;
  public List<IPreMover> _preMovers = new List<IPreMover>();
  public List<IPositionDeltaChanger> _positionDeltaChangers = new List<IPositionDeltaChanger>();
  public List<IPositionOverrider> _positionOverriders = new List<IPositionOverrider>();
  public List<ISizeDeltaChanger> _sizeDeltaChangers = new List<ISizeDeltaChanger>();
  public List<ISizeOverrider> _sizeOverriders = new List<ISizeOverrider>();
  public List<IPostMover> _postMovers = new List<IPostMover>();

  public static Com.LuisPedroFonseca.ProCamera2D.ProCamera2D Instance
  {
    get
    {
      if (object.Equals((object) Com.LuisPedroFonseca.ProCamera2D.ProCamera2D._instance, (object) null))
      {
        Com.LuisPedroFonseca.ProCamera2D.ProCamera2D._instance = UnityEngine.Object.FindObjectOfType(typeof (Com.LuisPedroFonseca.ProCamera2D.ProCamera2D)) as Com.LuisPedroFonseca.ProCamera2D.ProCamera2D;
        if (object.Equals((object) Com.LuisPedroFonseca.ProCamera2D.ProCamera2D._instance, (object) null))
          throw new UnityException("ProCamera2D does not exist.");
      }
      return Com.LuisPedroFonseca.ProCamera2D.ProCamera2D._instance;
    }
  }

  public static bool Exists => (UnityEngine.Object) Com.LuisPedroFonseca.ProCamera2D.ProCamera2D._instance != (UnityEngine.Object) null;

  public bool IsMoving
  {
    get
    {
      return (double) this.Vector3H(this._transform.localPosition) != (double) this.Vector3H(this._previousCameraPosition) || (double) this.Vector3V(this._transform.localPosition) != (double) this.Vector3V(this._previousCameraPosition);
    }
  }

  public Rect Rect
  {
    get => this.GameCamera.rect;
    set
    {
      this.GameCamera.rect = value;
      ProCamera2DParallax componentInChildren = this.GetComponentInChildren<ProCamera2DParallax>();
      if (!((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null))
        return;
      for (int index = 0; index < componentInChildren.ParallaxLayers.Count; ++index)
        componentInChildren.ParallaxLayers[index].ParallaxCamera.rect = value;
    }
  }

  public Vector2 CameraTargetPositionSmoothed
  {
    get
    {
      return new Vector2(this._cameraTargetHorizontalPositionSmoothed, this._cameraTargetVerticalPositionSmoothed);
    }
    set
    {
      this._cameraTargetHorizontalPositionSmoothed = value.x;
      this._cameraTargetVerticalPositionSmoothed = value.y;
    }
  }

  public Vector3 LocalPosition
  {
    get => this._transform.localPosition;
    set => this._transform.localPosition = value;
  }

  public Vector2 ScreenSizeInWorldCoordinates => this._screenSizeInWorldCoordinates;

  public Vector3 PreviousTargetsMidPoint => this._previousTargetsMidPoint;

  public Vector3 TargetsMidPoint => this._targetsMidPoint;

  public Vector3 CameraTargetPosition => this._cameraTargetPosition;

  public float DeltaTime => this._deltaTime;

  public Vector3 ParentPosition => this._parentPosition;

  public Vector3 InfluencesSum => this._influencesSum;

  public void Awake()
  {
    Debug.Log((object) "ProCamera2D.Awake()", (UnityEngine.Object) this);
    Com.LuisPedroFonseca.ProCamera2D.ProCamera2D._instance = this;
    this._transform = this.transform;
    if ((UnityEngine.Object) this._transform.parent != (UnityEngine.Object) null)
      this._parentPosition = this._transform.parent.position;
    if ((UnityEngine.Object) this.GameCamera == (UnityEngine.Object) null)
      this.GameCamera = this.GetComponent<Camera>();
    if ((UnityEngine.Object) this.GameCamera == (UnityEngine.Object) null)
      Debug.LogError((object) ("Unity Camera not set and not found on the GameObject: " + this.gameObject.name));
    this.ResetAxisFunctions();
    for (int index = 0; index < this.CameraTargets.Count; ++index)
    {
      if ((UnityEngine.Object) this.CameraTargets[index].TargetTransform == (UnityEngine.Object) null)
        this.CameraTargets.RemoveAt(index);
    }
    this.CalculateScreenSize();
    this._startScreenSizeInWorldCoordinates = this._screenSizeInWorldCoordinates;
    this._originalCameraDepthSign = Mathf.Sign(this.Vector3D(this._transform.localPosition));
  }

  public void Start()
  {
    this.SortPreMovers();
    this.SortPositionDeltaChangers();
    this.SortPositionOverriders();
    this.SortSizeDeltaChangers();
    this.SortSizeOverriders();
    this.SortPostMovers();
    this._targetsMidPoint = this.GetTargetsWeightedMidPoint(ref this.CameraTargets);
    this._cameraTargetHorizontalPositionSmoothed = this.Vector3H(this._targetsMidPoint);
    this._cameraTargetVerticalPositionSmoothed = this.Vector3V(this._targetsMidPoint);
    this._deltaTime = Time.deltaTime;
    if (this.CenterTargetOnStart && this.CameraTargets.Count > 0)
    {
      Vector3 weightedMidPoint = this.GetTargetsWeightedMidPoint(ref this.CameraTargets);
      Vector2 cameraPos = new Vector2(this.FollowHorizontal ? this.Vector3H(weightedMidPoint) : this.Vector3H(this._transform.localPosition), this.FollowVertical ? this.Vector3V(weightedMidPoint) : this.Vector3V(this._transform.localPosition));
      cameraPos += new Vector2(this.GetOffsetX() - this.Vector3H(this._parentPosition), this.GetOffsetY() - this.Vector3V(this._parentPosition));
      this.MoveCameraInstantlyToPosition(cameraPos);
    }
    else
    {
      this._cameraTargetPosition = this._transform.position - this._parentPosition;
      this._cameraTargetHorizontalPositionSmoothed = this.Vector3H(this._cameraTargetPosition);
      this._previousCameraTargetHorizontalPositionSmoothed = this._cameraTargetHorizontalPositionSmoothed;
      this._cameraTargetVerticalPositionSmoothed = this.Vector3V(this._cameraTargetPosition);
      this._previousCameraTargetVerticalPositionSmoothed = this._cameraTargetVerticalPositionSmoothed;
    }
  }

  public void LateUpdate()
  {
    if (this.UpdateType != UpdateType.LateUpdate)
      return;
    this.Move(Time.deltaTime);
  }

  public void FixedUpdate()
  {
    if (this.UpdateType != UpdateType.FixedUpdate)
      return;
    this.Move(Time.fixedDeltaTime);
  }

  public void OnApplicationQuit() => Com.LuisPedroFonseca.ProCamera2D.ProCamera2D._instance = (Com.LuisPedroFonseca.ProCamera2D.ProCamera2D) null;

  public void ApplyInfluence(Vector2 influence)
  {
    if ((double) Time.deltaTime < 9.9999997473787516E-05 || float.IsNaN(influence.x) || float.IsNaN(influence.y))
      return;
    this._influences.Add(this.VectorHV(influence.x, influence.y));
  }

  public Coroutine ApplyInfluencesTimed(Vector2[] influences, float[] durations)
  {
    return this.StartCoroutine(this.ApplyInfluencesTimedRoutine((IList<Vector2>) influences, durations));
  }

  public CameraTarget AddCameraTarget(
    Transform targetTransform,
    float targetInfluenceH = 1f,
    float targetInfluenceV = 1f,
    float duration = 0.0f,
    Vector2 targetOffset = default (Vector2))
  {
    CameraTarget cameraTarget = new CameraTarget()
    {
      TargetTransform = targetTransform,
      TargetInfluenceH = targetInfluenceH,
      TargetInfluenceV = targetInfluenceV,
      TargetOffset = targetOffset
    };
    this.CameraTargets.Add(cameraTarget);
    if ((double) duration > 0.0)
    {
      cameraTarget.TargetInfluence = 0.0f;
      this.StartCoroutine(this.AdjustTargetInfluenceRoutine(cameraTarget, targetInfluenceH, targetInfluenceV, duration));
    }
    return cameraTarget;
  }

  public void AddCameraTargets(
    IList<Transform> targetsTransforms,
    float targetsInfluenceH = 1f,
    float targetsInfluenceV = 1f,
    float duration = 0.0f,
    Vector2 targetOffset = default (Vector2))
  {
    for (int index = 0; index < targetsTransforms.Count; ++index)
      this.AddCameraTarget(targetsTransforms[index], targetsInfluenceH, targetsInfluenceV, duration, targetOffset);
  }

  public void AddCameraTargets(IList<CameraTarget> cameraTargets)
  {
    this.CameraTargets.AddRange((IEnumerable<CameraTarget>) cameraTargets);
  }

  public CameraTarget GetCameraTarget(Transform targetTransform)
  {
    for (int index = 0; index < this.CameraTargets.Count; ++index)
    {
      if (this.CameraTargets[index].TargetTransform.GetInstanceID() == targetTransform.GetInstanceID())
        return this.CameraTargets[index];
    }
    return (CameraTarget) null;
  }

  public void RemoveCameraTarget(Transform targetTransform, float duration = 0.0f)
  {
    for (int index = 0; index < this.CameraTargets.Count; ++index)
    {
      if (this.CameraTargets[index].TargetTransform.GetInstanceID() == targetTransform.GetInstanceID())
      {
        if ((double) duration > 0.0)
          this.StartCoroutine(this.AdjustTargetInfluenceRoutine(this.CameraTargets[index], 0.0f, 0.0f, duration, true));
        else
          this.CameraTargets.Remove(this.CameraTargets[index]);
      }
    }
  }

  public void RemoveAllCameraTargets(float duration = 0.0f)
  {
    if ((double) duration == 0.0)
    {
      this.CameraTargets.Clear();
    }
    else
    {
      for (int index = 0; index < this.CameraTargets.Count; ++index)
        this.StartCoroutine(this.AdjustTargetInfluenceRoutine(this.CameraTargets[index], 0.0f, 0.0f, duration, true));
    }
  }

  public Coroutine AdjustCameraTargetInfluence(
    CameraTarget cameraTarget,
    float targetInfluenceH,
    float targetInfluenceV,
    float duration = 0.0f)
  {
    if ((double) duration > 0.0)
      return this.StartCoroutine(this.AdjustTargetInfluenceRoutine(cameraTarget, targetInfluenceH, targetInfluenceV, duration));
    cameraTarget.TargetInfluenceH = targetInfluenceH;
    cameraTarget.TargetInfluenceV = targetInfluenceV;
    return (Coroutine) null;
  }

  public Coroutine AdjustCameraTargetInfluence(
    Transform cameraTargetTransf,
    float targetInfluenceH,
    float targetInfluenceV,
    float duration = 0.0f)
  {
    CameraTarget cameraTarget = this.GetCameraTarget(cameraTargetTransf);
    return cameraTarget == null ? (Coroutine) null : this.AdjustCameraTargetInfluence(cameraTarget, targetInfluenceH, targetInfluenceV, duration);
  }

  public void MoveCameraInstantlyToPosition(Vector2 cameraPos)
  {
    if ((UnityEngine.Object) this._transform == (UnityEngine.Object) null)
      this._transform = this.transform;
    this._transform.localPosition = this.VectorHVD(cameraPos.x, cameraPos.y, this.Vector3D(this._transform.localPosition));
    this.ResetMovement();
  }

  public void Reset(bool centerOnTargets = true, bool resetSize = true, bool resetExtensions = true)
  {
    if (centerOnTargets)
      this.CenterOnTargets();
    else
      this.ResetMovement();
    if (resetSize)
      this.ResetSize();
    if (!resetExtensions)
      return;
    this.ResetExtensions();
  }

  public void ResetMovement()
  {
    this._cameraTargetPosition = this._transform.localPosition;
    this._cameraTargetHorizontalPositionSmoothed = this.Vector3H(this._cameraTargetPosition);
    this._cameraTargetVerticalPositionSmoothed = this.Vector3V(this._cameraTargetPosition);
    this._previousCameraTargetHorizontalPositionSmoothed = this._cameraTargetHorizontalPositionSmoothed;
    this._previousCameraTargetVerticalPositionSmoothed = this._cameraTargetVerticalPositionSmoothed;
  }

  public void ResetSize() => this.SetScreenSize(this._startScreenSizeInWorldCoordinates.y / 2f);

  public void ResetExtensions()
  {
    if (this.OnReset == null)
      return;
    this.OnReset();
  }

  public void CenterOnTargets()
  {
    Vector3 weightedMidPoint = this.GetTargetsWeightedMidPoint(ref this.CameraTargets);
    this.MoveCameraInstantlyToPosition(new Vector2(this.Vector3H(weightedMidPoint), this.Vector3V(weightedMidPoint)) + new Vector2(this.GetOffsetX(), this.GetOffsetY()));
  }

  public void UpdateScreenSize(float newSize, float duration = 0.0f, EaseType easeType = EaseType.EaseInOut)
  {
    if (!this.enabled)
      return;
    if (this._updateScreenSizeCoroutine != null)
      this.StopCoroutine(this._updateScreenSizeCoroutine);
    if ((double) duration > 0.0)
      this._updateScreenSizeCoroutine = this.StartCoroutine(this.UpdateScreenSizeRoutine(newSize, duration, easeType));
    else
      this.SetScreenSize(newSize);
  }

  public void CalculateScreenSize()
  {
    this.GameCamera.ResetAspect();
    this._screenSizeInWorldCoordinates = Utils.GetScreenSizeInWorldCoords(this.GameCamera, Mathf.Abs(this.Vector3D(this._transform.localPosition)));
    this._previousScreenWidth = Screen.width;
    this._previousScreenHeight = Screen.height;
  }

  public void Zoom(float zoomAmount, float duration = 0.0f, EaseType easeType = EaseType.EaseInOut)
  {
    this.UpdateScreenSize(this._screenSizeInWorldCoordinates.y * 0.5f + zoomAmount, duration, easeType);
  }

  public void DollyZoom(float targetFOV, float duration = 1f, EaseType easeType = EaseType.EaseInOut)
  {
    if (!this.enabled)
      return;
    if (this.GameCamera.orthographic)
    {
      Debug.LogWarning((object) "Dolly zooming is only supported on perspective cameras");
    }
    else
    {
      if (this._dollyZoomRoutine != null)
        this.StopCoroutine(this._dollyZoomRoutine);
      targetFOV = Mathf.Clamp(targetFOV, 0.1f, 179.9f);
      if ((double) duration <= 0.0)
      {
        this.GameCamera.fieldOfView = targetFOV;
        this._transform.localPosition = this.VectorHVD(this.Vector3H(this._transform.localPosition), this.Vector3V(this._transform.localPosition), this.GetCameraDistanceForFOV(this.GameCamera.fieldOfView, this._screenSizeInWorldCoordinates.y) * this._originalCameraDepthSign);
      }
      else
        this.StartCoroutine(this.DollyZoomRoutine(targetFOV, duration, easeType));
    }
  }

  public void Move(float deltaTime)
  {
    this._previousCameraPosition = this._transform.localPosition;
    if (Screen.width != this._previousScreenWidth || Screen.height != this._previousScreenHeight)
      this.CalculateScreenSize();
    this._deltaTime = deltaTime;
    if ((double) this._deltaTime < 9.9999997473787516E-05)
      return;
    if (this.PreMoveUpdate != null)
      this.PreMoveUpdate(this._deltaTime);
    for (int index = 0; index < this._preMovers.Count; ++index)
      this._preMovers[index].PreMove(deltaTime);
    this._previousTargetsMidPoint = this._targetsMidPoint;
    this._targetsMidPoint = this.GetTargetsWeightedMidPoint(ref this.CameraTargets);
    this._cameraTargetPosition = this._targetsMidPoint;
    this._influencesSum = Utils.GetVectorsSum((IList<Vector3>) this._influences);
    this._cameraTargetPosition += this._influencesSum;
    this._influences.Clear();
    this._cameraTargetPosition = this.VectorHV((this.FollowHorizontal ? this.Vector3H(this._cameraTargetPosition) : this.Vector3H(this._transform.localPosition)) - this.Vector3H(this._parentPosition), (this.FollowVertical ? this.Vector3V(this._cameraTargetPosition) : this.Vector3V(this._transform.localPosition)) - this.Vector3V(this._parentPosition));
    if (this.ExclusiveTargetPosition.HasValue)
    {
      this._cameraTargetPosition = this.VectorHV(this.Vector3H(this.ExclusiveTargetPosition.Value) - this.Vector3H(this._parentPosition), this.Vector3V(this.ExclusiveTargetPosition.Value) - this.Vector3V(this._parentPosition));
      this.ExclusiveTargetPosition = new Vector3?();
    }
    this._cameraTargetPosition += this.VectorHV(this.FollowHorizontal ? this.GetOffsetX() : 0.0f, this.FollowVertical ? this.GetOffsetY() : 0.0f);
    this._cameraTargetHorizontalPositionSmoothed = Utils.SmoothApproach(this._cameraTargetHorizontalPositionSmoothed, this._previousCameraTargetHorizontalPositionSmoothed, this.Vector3H(this._cameraTargetPosition), 1f / this.HorizontalFollowSmoothness, this._deltaTime);
    this._previousCameraTargetHorizontalPositionSmoothed = this._cameraTargetHorizontalPositionSmoothed;
    this._cameraTargetVerticalPositionSmoothed = Utils.SmoothApproach(this._cameraTargetVerticalPositionSmoothed, this._previousCameraTargetVerticalPositionSmoothed, this.Vector3V(this._cameraTargetPosition), 1f / this.VerticalFollowSmoothness, this._deltaTime);
    this._previousCameraTargetVerticalPositionSmoothed = this._cameraTargetVerticalPositionSmoothed;
    Vector3 originalDelta1 = this.VectorHV(this._cameraTargetHorizontalPositionSmoothed - this.Vector3H(this._transform.localPosition), this._cameraTargetVerticalPositionSmoothed - this.Vector3V(this._transform.localPosition));
    float originalDelta2 = 0.0f;
    for (int index = 0; index < this._sizeDeltaChangers.Count; ++index)
      originalDelta2 = this._sizeDeltaChangers[index].AdjustSize(deltaTime, originalDelta2);
    float num = this._screenSizeInWorldCoordinates.y * 0.5f + originalDelta2;
    for (int index = 0; index < this._sizeOverriders.Count; ++index)
      num = this._sizeOverriders[index].OverrideSize(deltaTime, num);
    if ((double) num != (double) this._screenSizeInWorldCoordinates.y * 0.5)
      this.SetScreenSize(num);
    for (int index = 0; index < this._positionDeltaChangers.Count; ++index)
      originalDelta1 = this._positionDeltaChangers[index].AdjustDelta(deltaTime, originalDelta1);
    Vector3 originalPosition = this.LocalPosition + originalDelta1;
    for (int index = 0; index < this._positionOverriders.Count; ++index)
      originalPosition = this._positionOverriders[index].OverridePosition(deltaTime, originalPosition);
    this._transform.localPosition = this.VectorHVD(this.Vector3H(originalPosition), this.Vector3V(originalPosition), this.Vector3D(this._transform.localPosition));
    for (int index = 0; index < this._postMovers.Count; ++index)
      this._postMovers[index].PostMove(deltaTime);
    if (this.PostMoveUpdate == null)
      return;
    this.PostMoveUpdate(this._deltaTime);
  }

  public YieldInstruction GetYield()
  {
    return this.UpdateType == UpdateType.FixedUpdate ? (YieldInstruction) this._waitForFixedUpdate : (YieldInstruction) null;
  }

  public void ResetAxisFunctions()
  {
    switch (this.Axis)
    {
      case MovementAxis.XY:
        this.Vector3H = (Func<Vector3, float>) (vector => vector.x);
        this.Vector3V = (Func<Vector3, float>) (vector => vector.y);
        this.Vector3D = (Func<Vector3, float>) (vector => vector.z);
        this.VectorHV = (Func<float, float, Vector3>) ((h, v) => new Vector3(h, v, 0.0f));
        this.VectorHVD = (Func<float, float, float, Vector3>) ((h, v, d) => new Vector3(h, v, d));
        break;
      case MovementAxis.XZ:
        this.Vector3H = (Func<Vector3, float>) (vector => vector.x);
        this.Vector3V = (Func<Vector3, float>) (vector => vector.z);
        this.Vector3D = (Func<Vector3, float>) (vector => vector.y);
        this.VectorHV = (Func<float, float, Vector3>) ((h, v) => new Vector3(h, 0.0f, v));
        this.VectorHVD = (Func<float, float, float, Vector3>) ((h, v, d) => new Vector3(h, d, v));
        break;
      case MovementAxis.YZ:
        this.Vector3H = (Func<Vector3, float>) (vector => vector.z);
        this.Vector3V = (Func<Vector3, float>) (vector => vector.y);
        this.Vector3D = (Func<Vector3, float>) (vector => vector.x);
        this.VectorHV = (Func<float, float, Vector3>) ((h, v) => new Vector3(0.0f, v, h));
        this.VectorHVD = (Func<float, float, float, Vector3>) ((h, v, d) => new Vector3(d, v, h));
        break;
    }
  }

  public Vector3 GetTargetsWeightedMidPoint(ref List<CameraTarget> targets)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    if (targets.Count == 0)
      return this.transform.localPosition;
    float num3 = 0.0f;
    float num4 = 0.0f;
    int num5 = 0;
    int num6 = 0;
    for (int index = 0; index < targets.Count; ++index)
    {
      if (targets[index] == null || (UnityEngine.Object) targets[index].TargetTransform == (UnityEngine.Object) null)
      {
        targets.RemoveAt(index);
      }
      else
      {
        num1 += (this.Vector3H(targets[index].TargetPosition) + targets[index].TargetOffset.x) * targets[index].TargetInfluenceH;
        num2 += (this.Vector3V(targets[index].TargetPosition) + targets[index].TargetOffset.y) * targets[index].TargetInfluenceV;
        num3 += targets[index].TargetInfluenceH;
        num4 += targets[index].TargetInfluenceV;
        if ((double) targets[index].TargetInfluenceH > 0.0)
          ++num5;
        if ((double) targets[index].TargetInfluenceV > 0.0)
          ++num6;
      }
    }
    if ((double) num3 < 1.0 && num5 == 1)
      num3 += 1f - num3;
    if ((double) num4 < 1.0 && num6 == 1)
      num4 += 1f - num4;
    if ((double) num3 > 9.9999997473787516E-05)
      num1 /= num3;
    if ((double) num4 > 9.9999997473787516E-05)
      num2 /= num4;
    return this.VectorHV(num1, num2);
  }

  public IEnumerator ApplyInfluencesTimedRoutine(IList<Vector2> influences, float[] durations)
  {
    Com.LuisPedroFonseca.ProCamera2D.ProCamera2D proCamera2D = this;
    int count = -1;
    while (count < durations.Length - 1)
    {
      ++count;
      float duration = durations[count];
      yield return (object) proCamera2D.StartCoroutine(proCamera2D.ApplyInfluenceTimedRoutine(influences[count], duration));
    }
  }

  public IEnumerator ApplyInfluenceTimedRoutine(Vector2 influence, float duration)
  {
    while ((double) duration > 0.0)
    {
      duration -= this._deltaTime;
      this.ApplyInfluence(influence);
      yield return (object) this.GetYield();
    }
  }

  public IEnumerator AdjustTargetInfluenceRoutine(
    CameraTarget cameraTarget,
    float influenceH,
    float influenceV,
    float duration,
    bool removeIfZeroInfluence = false)
  {
    float startInfluenceH = cameraTarget.TargetInfluenceH;
    float startInfluenceV = cameraTarget.TargetInfluenceV;
    float t = 0.0f;
    while ((double) t <= 1.0)
    {
      t += this._deltaTime / duration;
      cameraTarget.TargetInfluenceH = Utils.EaseFromTo(startInfluenceH, influenceH, t, EaseType.Linear);
      cameraTarget.TargetInfluenceV = Utils.EaseFromTo(startInfluenceV, influenceV, t, EaseType.Linear);
      yield return (object) this.GetYield();
    }
    if (removeIfZeroInfluence && (double) cameraTarget.TargetInfluenceH <= 0.0 && (double) cameraTarget.TargetInfluenceV <= 0.0)
      this.CameraTargets.Remove(cameraTarget);
  }

  public IEnumerator UpdateScreenSizeRoutine(float finalSize, float duration, EaseType easeType)
  {
    float startSize = this._screenSizeInWorldCoordinates.y * 0.5f;
    float t = 0.0f;
    while ((double) t <= 1.0)
    {
      t += this._deltaTime / duration;
      this.SetScreenSize(Utils.EaseFromTo(startSize, finalSize, t, easeType));
      yield return (object) this.GetYield();
    }
    this._updateScreenSizeCoroutine = (Coroutine) null;
  }

  public IEnumerator DollyZoomRoutine(float finalFOV, float duration, EaseType easeType)
  {
    float startFOV = this.GameCamera.fieldOfView;
    float t = 0.0f;
    while ((double) t <= 1.0)
    {
      t += this._deltaTime / duration;
      float fov = Utils.EaseFromTo(startFOV, finalFOV, t, easeType);
      this.GameCamera.fieldOfView = fov;
      this._transform.localPosition = this.VectorHVD(this.Vector3H(this._transform.localPosition), this.Vector3V(this._transform.localPosition), this.GetCameraDistanceForFOV(fov, this._screenSizeInWorldCoordinates.y) * this._originalCameraDepthSign);
      yield return (object) this.GetYield();
    }
    this._dollyZoomRoutine = (Coroutine) null;
  }

  public void SetScreenSize(float newSize)
  {
    if (this.GameCamera.orthographic)
    {
      newSize = Mathf.Max(newSize, 0.1f);
      this.GameCamera.orthographicSize = newSize;
    }
    else if (this.ZoomWithFOV)
      this.GameCamera.fieldOfView = Mathf.Clamp((float) (2.0 * (double) Mathf.Atan(newSize / Mathf.Abs(this.Vector3D(this._transform.localPosition))) * 57.295780181884766), 0.1f, 179.9f);
    else
      this._transform.localPosition = this.VectorHVD(this.Vector3H(this._transform.localPosition), this.Vector3V(this._transform.localPosition), newSize / Mathf.Tan((float) ((double) this.GameCamera.fieldOfView * 0.5 * (Math.PI / 180.0))) * this._originalCameraDepthSign);
    this._screenSizeInWorldCoordinates = new Vector2(newSize * 2f * this.GameCamera.aspect, newSize * 2f);
    if (this.OnCameraResize == null)
      return;
    this.OnCameraResize(this._screenSizeInWorldCoordinates);
  }

  public float GetCameraDistanceForFOV(float fov, float cameraHeight)
  {
    return cameraHeight / (2f * Mathf.Tan((float) (0.5 * (double) fov * (Math.PI / 180.0))));
  }

  public float GetOffsetX()
  {
    return !this.IsRelativeOffset ? this.OffsetX : (float) ((double) this.OffsetX * (double) this._screenSizeInWorldCoordinates.x * 0.5);
  }

  public float GetOffsetY()
  {
    return !this.IsRelativeOffset ? this.OffsetY : (float) ((double) this.OffsetY * (double) this._screenSizeInWorldCoordinates.y * 0.5);
  }

  public void AddPreMover(IPreMover mover) => this._preMovers.Add(mover);

  public void RemovePreMover(IPreMover mover) => this._preMovers.Remove(mover);

  public void SortPreMovers()
  {
    this._preMovers = this._preMovers.OrderBy<IPreMover, int>((Func<IPreMover, int>) (a => a.PrMOrder)).ToList<IPreMover>();
  }

  public void AddPositionDeltaChanger(IPositionDeltaChanger changer)
  {
    this._positionDeltaChangers.Add(changer);
  }

  public void RemovePositionDeltaChanger(IPositionDeltaChanger changer)
  {
    this._positionDeltaChangers.Remove(changer);
  }

  public void SortPositionDeltaChangers()
  {
    this._positionDeltaChangers = this._positionDeltaChangers.OrderBy<IPositionDeltaChanger, int>((Func<IPositionDeltaChanger, int>) (a => a.PDCOrder)).ToList<IPositionDeltaChanger>();
  }

  public void AddPositionOverrider(IPositionOverrider overrider)
  {
    this._positionOverriders.Add(overrider);
  }

  public void RemovePositionOverrider(IPositionOverrider overrider)
  {
    this._positionOverriders.Remove(overrider);
  }

  public void SortPositionOverriders()
  {
    this._positionOverriders = this._positionOverriders.OrderBy<IPositionOverrider, int>((Func<IPositionOverrider, int>) (a => a.POOrder)).ToList<IPositionOverrider>();
  }

  public void AddSizeDeltaChanger(ISizeDeltaChanger changer)
  {
    this._sizeDeltaChangers.Add(changer);
  }

  public void RemoveSizeDeltaChanger(ISizeDeltaChanger changer)
  {
    this._sizeDeltaChangers.Remove(changer);
  }

  public void SortSizeDeltaChangers()
  {
    this._sizeDeltaChangers = this._sizeDeltaChangers.OrderBy<ISizeDeltaChanger, int>((Func<ISizeDeltaChanger, int>) (a => a.SDCOrder)).ToList<ISizeDeltaChanger>();
  }

  public void AddSizeOverrider(ISizeOverrider overrider) => this._sizeOverriders.Add(overrider);

  public void RemoveSizeOverrider(ISizeOverrider overrider)
  {
    this._sizeOverriders.Remove(overrider);
  }

  public void SortSizeOverriders()
  {
    this._sizeOverriders = this._sizeOverriders.OrderBy<ISizeOverrider, int>((Func<ISizeOverrider, int>) (a => a.SOOrder)).ToList<ISizeOverrider>();
  }

  public void AddPostMover(IPostMover mover) => this._postMovers.Add(mover);

  public void RemovePostMover(IPostMover mover) => this._postMovers.Remove(mover);

  public void SortPostMovers()
  {
    this._postMovers = this._postMovers.OrderBy<IPostMover, int>((Func<IPostMover, int>) (a => a.PMOrder)).ToList<IPostMover>();
  }

  public void OnBeforeSerialize()
  {
  }

  public void OnAfterDeserialize() => this.ResetAxisFunctions();

  public static void SetInstance(Com.LuisPedroFonseca.ProCamera2D.ProCamera2D ins) => ins.Awake();
}
