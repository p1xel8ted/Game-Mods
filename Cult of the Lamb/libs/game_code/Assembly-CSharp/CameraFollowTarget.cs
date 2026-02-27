// Decompiled with JetBrains decompiler
// Type: CameraFollowTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class CameraFollowTarget : BaseMonoBehaviour
{
  public const float kIsMovingThreshold = 0.05f;
  public float LookDistance = 0.5f;
  public static CameraFollowTarget Instance;
  public float CamWobbleSettings = 1f;
  public Transform target;
  public List<CameraFollowTarget.Target> targets = new List<CameraFollowTarget.Target>();
  [HideInInspector]
  public bool ContainsMainPlayer;
  public Vector3 targetPosition = Vector3.zero;
  public float targetDistance = 10f;
  public float distance = 10f;
  public float MaxZoom = 13f;
  public float MinZoom = 11f;
  public float ZoomLimiter = 5f;
  public float ZoomSpeed = 2f;
  public float ZoomSpeedConversation = 0.0833f;
  public float MoveSpeed = 5f;
  public bool init;
  [HideInInspector]
  public float angle = -45f;
  public Vector3 TargetOffset = Vector3.zero;
  public Vector3 CurrentOffset = Vector3.zero;
  public bool SnappyMovement;
  public bool IN_CONVERSATION;
  public Vector3 Look;
  public Vector3 CamWobble;
  public float Wobble;
  public StateMachine PlayerState;
  public float LookSpeed = 5f;
  public bool DisablePlayerLook;
  [CompilerGenerated]
  public Vector3 \u003CCurrentPosition\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CIsMoving\u003Ek__BackingField;
  public float MaxZoomInConversation = 4f;
  public Vector3 CurrentPositionVelocity;
  public float CurrentPositionMaxSpeed = 14f;
  public float SmoothZoom;
  public Bounds bounds;
  public bool UseCameraLimit;
  public Bounds CameraLimitBounds;
  [HideInInspector]
  public Camera TargetCamera;
  [CompilerGenerated]
  public Vector3 \u003CCurrentTargetCameraPosition\u003Ek__BackingField;
  [CompilerGenerated]
  public AnimationCurve \u003CTargetCameraAnimationCurve\u003Ek__BackingField;
  public float TargetCameraTransitionDuration;
  public Coroutine cSetTargetCamera;
  public Coroutine cResetTargetCamera;
  public float CamWobbleIntensity;

  public void AddTarget(GameObject g, float Weight)
  {
    bool flag = false;
    for (int index = 0; index < this.targets.Count; ++index)
    {
      if ((UnityEngine.Object) this.targets[index].gameObject == (UnityEngine.Object) g)
        flag = true;
    }
    if (flag)
      return;
    this.targets.Add(new CameraFollowTarget.Target(g, Weight));
  }

  public bool ContainsAnyPlayer()
  {
    for (int index = 0; index < this.targets.Count; ++index)
    {
      if (this.targets[index] != null && (UnityEngine.Object) this.targets[index].gameObject != (UnityEngine.Object) null && this.targets[index].gameObject.CompareTag("Player Camera Bone"))
      {
        PlayerFarming componentInParent = this.targets[index].gameObject.GetComponentInParent<PlayerFarming>(true);
        if ((UnityEngine.Object) componentInParent != (UnityEngine.Object) null && componentInParent.EnableCoopFeatures)
          return true;
      }
    }
    return false;
  }

  public bool Contains(GameObject gameObject)
  {
    if (this.targets != null)
    {
      foreach (CameraFollowTarget.Target target in this.targets)
      {
        if (target != null && (UnityEngine.Object) target.gameObject == (UnityEngine.Object) gameObject)
          return true;
      }
    }
    return false;
  }

  public void RemoveTarget(GameObject gameObject)
  {
    if (this.targets == null)
      return;
    foreach (CameraFollowTarget.Target target in this.targets)
    {
      if ((UnityEngine.Object) target.gameObject == (UnityEngine.Object) gameObject)
      {
        this.targets.Remove(target);
        break;
      }
    }
  }

  public void CleanTargets()
  {
    this.targets.RemoveAll((Predicate<CameraFollowTarget.Target>) (t => t == null || (UnityEngine.Object) t.gameObject == (UnityEngine.Object) null));
  }

  public void ClearAllTargets()
  {
    this.targets.Clear();
    this.ContainsMainPlayer = false;
  }

  public void Awake() => CameraFollowTarget.Instance = this;

  public void Start()
  {
    this.IN_CONVERSATION = false;
    if (SettingsManager.Settings == null)
      return;
    this.CamWobbleSettings = (float) (1 - SettingsManager.Settings.Accessibility.ReduceCameraMotion.ToInt());
  }

  public Vector3 CurrentPosition
  {
    get => this.\u003CCurrentPosition\u003Ek__BackingField;
    set => this.\u003CCurrentPosition\u003Ek__BackingField = value;
  }

  public bool IsMoving
  {
    get => this.\u003CIsMoving\u003Ek__BackingField;
    set => this.\u003CIsMoving\u003Ek__BackingField = value;
  }

  public Vector3 GetCameraTargetWorldPosition()
  {
    return this.CurrentPosition - new Vector3(0.0f, this.distance * Mathf.Sin((float) Math.PI / 180f * this.angle), -this.distance * Mathf.Cos((float) Math.PI / 180f * this.angle));
  }

  public void SnapTo(Vector3 position)
  {
    this.transform.position = this.CurrentPosition = position + new Vector3(0.0f, this.distance * Mathf.Sin((float) Math.PI / 180f * this.angle), -this.distance * Mathf.Cos((float) Math.PI / 180f * this.angle));
  }

  public void ForceSnapTo(Vector3 position)
  {
    this.transform.position = this.CurrentPosition = position;
  }

  public void SetOffset(Vector3 Offset) => this.TargetOffset = Offset;

  public void LateUpdate()
  {
    if (!this.init)
    {
      if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && this.targets.Count <= 0)
      {
        GameObject cameraBone = PlayerFarming.Instance.CameraBone;
        if ((UnityEngine.Object) cameraBone != (UnityEngine.Object) null)
          this.AddTarget(cameraBone, 1f);
        if (this.targets.Count <= 0)
          return;
      }
      this.init = true;
    }
    else if (this.targets != null && this.targets.Count > 0)
    {
      this.CleanTargets();
      if (!SettingsManager.Settings.Accessibility.ReduceCameraMotion && this.targets.Count == 1 && !LetterBox.IsPlaying && this.targets[0] != null && (UnityEngine.Object) this.targets[0].gameObject != (UnityEngine.Object) null && this.targets[0].gameObject.CompareTag("Player Camera Bone"))
      {
        if ((UnityEngine.Object) this.PlayerState != (UnityEngine.Object) null && !this.DisablePlayerLook && (double) Time.timeScale > 0.0)
          this.Look = Vector3.Lerp(this.Look, new Vector3(this.LookDistance * Mathf.Cos(this.PlayerState.facingAngle * ((float) Math.PI / 180f)), this.LookDistance * Mathf.Sin(this.PlayerState.facingAngle * ((float) Math.PI / 180f))), Time.unscaledDeltaTime * this.LookSpeed);
        else if (this.DisablePlayerLook)
          this.Look = Vector3.Lerp(this.Look, Vector3.zero, Time.unscaledDeltaTime * this.LookSpeed);
        else
          this.PlayerState = this.targets[0].gameObject.GetComponentInParent<StateMachine>();
      }
      else
        this.Look = Vector3.zero;
      this.targetPosition = this.GetCentrePoint();
      if ((double) this.MaxZoom <= 0.0)
        this.MaxZoom = 13f;
      if ((double) this.MinZoom <= 0.0)
        this.MinZoom = 11f;
      if ((double) this.ZoomLimiter <= 0.0)
        this.ZoomLimiter = 5f;
      int num = 1;
      if (CoopManager.CoopActive && PlayerFarming.AllCoopPlayersInCamera && LocationManager.LocationIsDungeon(PlayerFarming.Location))
        num = 2;
      if (!MMConversation.isPlaying || this.SnappyMovement)
      {
        this.distance = this.targets.Count <= num || this.SnappyMovement ? Mathf.SmoothDamp(this.distance, this.targetDistance, ref this.SmoothZoom, 0.0166666675f * this.ZoomSpeed, float.PositiveInfinity, this.SnappyMovement ? Time.unscaledDeltaTime : Time.deltaTime) : Mathf.SmoothDamp(this.distance, Mathf.Lerp(this.MinZoom, this.MaxZoom, Mathf.Max(this.bounds.size.x, this.bounds.size.y) / this.ZoomLimiter), ref this.SmoothZoom, 0.0166666675f * this.ZoomSpeed, float.PositiveInfinity, this.SnappyMovement ? Time.unscaledDeltaTime : Time.deltaTime);
        if ((UnityEngine.Object) this.TargetCamera == (UnityEngine.Object) null)
          this.CamWobble = Vector3.zero;
        else
          this.CamWobble += Vector3.forward * 0.0005f * Mathf.Cos(this.Wobble += Time.unscaledDeltaTime * 2f);
      }
      else
      {
        this.distance = Mathf.SmoothDamp(this.distance, this.targetDistance, ref this.SmoothZoom, 0.0166666675f * this.ZoomSpeedConversation, this.MaxZoomInConversation, Time.unscaledDeltaTime);
        this.CamWobble += Vector3.forward * 0.0005f * Mathf.Cos(this.Wobble += Time.unscaledDeltaTime * 2f);
      }
      if (MMConversation.isPlaying)
        this.targetPosition += Vector3.up * 0.5f;
      if (float.IsNaN(this.distance))
        this.distance = 10f;
      if (float.IsNaN(this.SmoothZoom))
        this.SmoothZoom = 1f;
      Vector3 vector3 = this.targetPosition + new Vector3(0.0f, this.distance * Mathf.Sin((float) Math.PI / 180f * this.angle), -this.distance * Mathf.Cos((float) Math.PI / 180f * this.angle)) + this.CamWobble * this.CamWobbleSettings + this.Look + (this.CurrentOffset = Vector3.Lerp(this.CurrentOffset, this.TargetOffset, this.MoveSpeed * Time.unscaledDeltaTime));
      if (SettingsManager.Settings.Accessibility.ReduceCameraMotion && (double) Vector3.Distance(vector3, this.CurrentPosition) < 0.05000000074505806)
        vector3 = this.CurrentPosition;
      this.CurrentPosition = !MMConversation.isPlaying || this.SnappyMovement ? Vector3.Lerp(this.CurrentPosition, vector3, this.MoveSpeed * Time.unscaledDeltaTime) : Vector3.SmoothDamp(this.CurrentPosition, vector3, ref this.CurrentPositionVelocity, 0.2f, this.CurrentPositionMaxSpeed, Time.unscaledDeltaTime);
      this.IsMoving = (double) Vector3.Distance(this.CurrentPosition, vector3) > 0.05000000074505806;
    }
    else
      this.IsMoving = false;
    this.transform.position = !((UnityEngine.Object) this.TargetCamera == (UnityEngine.Object) null) || float.IsNaN(this.CurrentPosition.x) || float.IsNaN(this.CurrentPosition.y) || float.IsNaN(this.CurrentPosition.z) ? this.CurrentTargetCameraPosition + this.CamWobble * 0.1f * this.CamWobbleSettings : this.CurrentPosition;
  }

  public void SetCameraLimits(Bounds Limits)
  {
    this.UseCameraLimit = true;
    this.CameraLimitBounds = Limits;
  }

  public void DisableCameraLimits() => this.UseCameraLimit = false;

  public Vector3 GetCentrePoint()
  {
    switch (this.targets.Count)
    {
      case 0:
        return this.transform.position;
      case 1:
        if (this.targets[0] == null || !((UnityEngine.Object) this.targets[0].gameObject != (UnityEngine.Object) null))
          return Vector3.zero;
        Vector3 centrePoint = this.targets[0].gameObject.transform.position + Vector3.back * 0.5f;
        if (this.UseCameraLimit)
        {
          if ((double) centrePoint.x >= (double) this.CameraLimitBounds.center.x + (double) this.CameraLimitBounds.extents.x)
            this.Look = Vector3.zero;
          centrePoint.x = Mathf.Min(this.CameraLimitBounds.center.x + this.CameraLimitBounds.extents.x, centrePoint.x);
          if ((double) centrePoint.x <= (double) this.CameraLimitBounds.center.x - (double) this.CameraLimitBounds.extents.x)
            this.Look = Vector3.zero;
          centrePoint.x = Mathf.Max(this.CameraLimitBounds.center.x - this.CameraLimitBounds.extents.x, centrePoint.x);
          if ((double) centrePoint.y >= (double) this.CameraLimitBounds.center.y + (double) this.CameraLimitBounds.extents.y)
            this.Look = Vector3.zero;
          centrePoint.y = Mathf.Min(this.CameraLimitBounds.center.y + this.CameraLimitBounds.extents.y, centrePoint.y);
          if ((double) centrePoint.y <= (double) this.CameraLimitBounds.center.y - (double) this.CameraLimitBounds.extents.y)
            this.Look = Vector3.zero;
          centrePoint.y = Mathf.Max(this.CameraLimitBounds.center.y - this.CameraLimitBounds.extents.y, centrePoint.y);
        }
        return centrePoint;
      default:
        if (this.targets.Count > 0 && this.targets[0] != null && (UnityEngine.Object) this.targets[0].gameObject != (UnityEngine.Object) null)
        {
          this.bounds = new Bounds(this.targets[0].gameObject.transform.position, Vector3.zero);
          int index = 0;
          float num = 0.0f;
          while (++index < this.targets.Count)
          {
            if (this.targets[index] != null)
            {
              this.bounds.Encapsulate(this.targets[index].gameObject.transform.position);
              num += this.targets[index].Weight;
            }
          }
        }
        return this.bounds.center + Vector3.back * 0.5f;
    }
  }

  public void OnDrawGizmos()
  {
    Bounds bounds = this.bounds;
    Gizmos.DrawCube(this.bounds.center, this.bounds.size);
  }

  public Vector3 CurrentTargetCameraPosition
  {
    get => this.\u003CCurrentTargetCameraPosition\u003Ek__BackingField;
    set => this.\u003CCurrentTargetCameraPosition\u003Ek__BackingField = value;
  }

  public AnimationCurve TargetCameraAnimationCurve
  {
    get => this.\u003CTargetCameraAnimationCurve\u003Ek__BackingField;
    set => this.\u003CTargetCameraAnimationCurve\u003Ek__BackingField = value;
  }

  public void SetTargetCamera(Camera camera, float Duration, AnimationCurve animationCurve)
  {
    this.TargetCamera = camera;
    this.TargetCameraTransitionDuration = Duration;
    this.TargetCameraAnimationCurve = animationCurve;
    this.CamWobbleIntensity = 0.0f;
    if (this.cSetTargetCamera != null)
      this.StopCoroutine(this.cSetTargetCamera);
    if (this.cResetTargetCamera != null)
      this.StopCoroutine(this.cResetTargetCamera);
    this.cSetTargetCamera = this.StartCoroutine(this.SetTargetCameraRoutine());
  }

  public IEnumerator SetTargetCameraRoutine()
  {
    CameraFollowTarget cameraFollowTarget = this;
    float Progress = 0.0f;
    Vector3 StartingPosition = cameraFollowTarget.transform.position;
    Quaternion StartingAngle = cameraFollowTarget.transform.rotation;
    while ((double) (Progress += Time.deltaTime) < (double) cameraFollowTarget.TargetCameraTransitionDuration)
    {
      while (PhotoModeManager.PhotoModeActive)
        yield return (object) null;
      cameraFollowTarget.CurrentTargetCameraPosition = Vector3.Lerp(StartingPosition, cameraFollowTarget.TargetCamera.transform.position, cameraFollowTarget.TargetCameraAnimationCurve.Evaluate(Progress / cameraFollowTarget.TargetCameraTransitionDuration));
      cameraFollowTarget.transform.rotation = Quaternion.Slerp(StartingAngle, cameraFollowTarget.TargetCamera.transform.rotation, cameraFollowTarget.TargetCameraAnimationCurve.Evaluate(Progress / cameraFollowTarget.TargetCameraTransitionDuration));
      yield return (object) null;
    }
    Progress = 0.0f;
    float Duration = 0.5f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      while (PhotoModeManager.PhotoModeActive)
        yield return (object) null;
      cameraFollowTarget.CamWobbleIntensity = Mathf.Lerp(0.0f, 1f, Progress / Duration);
      yield return (object) null;
    }
    cameraFollowTarget.CamWobbleIntensity = 1f;
  }

  public void ResetTargetCamera(float Duration)
  {
    this.TargetCameraTransitionDuration = Duration;
    if (this.cSetTargetCamera != null)
      this.StopCoroutine(this.cSetTargetCamera);
    if (this.cResetTargetCamera != null)
      this.StopCoroutine(this.cResetTargetCamera);
    this.cResetTargetCamera = this.StartCoroutine(this.ResetTargetCameraRoutine());
  }

  public IEnumerator ResetTargetCameraRoutine()
  {
    CameraFollowTarget cameraFollowTarget = this;
    float Progress = 0.0f;
    Vector3 StartingPosition = cameraFollowTarget.transform.position;
    Quaternion StartingAngle = cameraFollowTarget.transform.rotation;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) cameraFollowTarget.TargetCameraTransitionDuration)
    {
      while (PhotoModeManager.PhotoModeActive)
        yield return (object) null;
      cameraFollowTarget.CamWobbleIntensity = Mathf.Lerp(1f, 0.0f, Progress / 0.1f);
      cameraFollowTarget.CurrentTargetCameraPosition = Vector3.Lerp(StartingPosition, cameraFollowTarget.CurrentPosition, cameraFollowTarget.TargetCameraAnimationCurve != null ? cameraFollowTarget.TargetCameraAnimationCurve.Evaluate(Progress / cameraFollowTarget.TargetCameraTransitionDuration) : 1f);
      cameraFollowTarget.transform.rotation = Quaternion.Slerp(StartingAngle, Quaternion.Euler(new Vector3(-45f, 0.0f, 0.0f)), cameraFollowTarget.TargetCameraAnimationCurve != null ? cameraFollowTarget.TargetCameraAnimationCurve.Evaluate(Progress / cameraFollowTarget.TargetCameraTransitionDuration) : 1f);
      yield return (object) null;
    }
    cameraFollowTarget.TargetCamera = (Camera) null;
  }

  public void OnPreRender() => GL.wireframe = CheatConsole.WIREFRAME_ENABLED;

  public void OnPostRender() => GL.wireframe = false;

  [Serializable]
  public class Target
  {
    public GameObject gameObject;
    public float Weight = 1f;

    public Target(GameObject gameObject, float Weight)
    {
      this.gameObject = gameObject;
      this.Weight = Weight;
    }
  }
}
