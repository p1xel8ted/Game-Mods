// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DTriggerBoundaries
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[HelpURL("http://www.procamera2d.com/user-guide/trigger-boundaries/")]
public class ProCamera2DTriggerBoundaries : BaseTrigger, IPositionOverrider
{
  public static string TriggerName = "Boundaries Trigger";
  public ProCamera2DNumericBoundaries NumericBoundaries;
  public bool AreBoundariesRelative = true;
  public bool UseTopBoundary = true;
  public float TopBoundary = 10f;
  public bool UseBottomBoundary = true;
  public float BottomBoundary = -10f;
  public bool UseLeftBoundary = true;
  public float LeftBoundary = -10f;
  public bool UseRightBoundary = true;
  public float RightBoundary = 10f;
  public float TransitionDuration = 1f;
  public EaseType TransitionEaseType;
  public bool ChangeZoom;
  public float TargetZoom = 1.5f;
  public float ZoomSmoothness = 1f;
  public bool _setAsStartingBoundaries;
  public float _initialCamSize;
  public BoundariesAnimator _boundsAnim;
  public float _targetTopBoundary;
  public float _targetBottomBoundary;
  public float _targetLeftBoundary;
  public float _targetRightBoundary;
  public bool _transitioning;
  public Vector3 _newPos;
  public int _poOrder = 1000;

  public bool IsCurrentTrigger
  {
    get => this.NumericBoundaries.CurrentBoundariesTrigger._instanceID == this._instanceID;
  }

  public bool SetAsStartingBoundaries
  {
    set
    {
      if (value && !this._setAsStartingBoundaries)
      {
        foreach (ProCamera2DTriggerBoundaries dtriggerBoundaries in UnityEngine.Object.FindObjectsOfType(typeof (ProCamera2DTriggerBoundaries)))
          dtriggerBoundaries.SetAsStartingBoundaries = false;
      }
      this._setAsStartingBoundaries = value;
    }
    get => this._setAsStartingBoundaries;
  }

  public override void Awake()
  {
    base.Awake();
    Com.LuisPedroFonseca.ProCamera2D.ProCamera2D.Instance.AddPositionOverrider((IPositionOverrider) this);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.ProCamera2D.RemovePositionOverrider((IPositionOverrider) this);
  }

  public void Start()
  {
    if ((UnityEngine.Object) this.ProCamera2D == (UnityEngine.Object) null)
      return;
    if ((UnityEngine.Object) this.NumericBoundaries == (UnityEngine.Object) null)
    {
      ProCamera2DNumericBoundaries objectOfType = UnityEngine.Object.FindObjectOfType<ProCamera2DNumericBoundaries>();
      this.NumericBoundaries = (UnityEngine.Object) objectOfType == (UnityEngine.Object) null ? this.ProCamera2D.gameObject.AddComponent<ProCamera2DNumericBoundaries>() : objectOfType;
    }
    this._boundsAnim = new BoundariesAnimator(this.ProCamera2D, this.NumericBoundaries);
    this._boundsAnim.OnTransitionStarted += (Action) (() =>
    {
      if (this.NumericBoundaries.OnBoundariesTransitionStarted == null)
        return;
      this.NumericBoundaries.OnBoundariesTransitionStarted();
    });
    this._boundsAnim.OnTransitionFinished += (Action) (() =>
    {
      if (this.NumericBoundaries.OnBoundariesTransitionFinished == null)
        return;
      this.NumericBoundaries.OnBoundariesTransitionFinished();
    });
    this.GetTargetBoundaries();
    if (this.SetAsStartingBoundaries)
      this.SetBoundaries();
    this._initialCamSize = this.ProCamera2D.ScreenSizeInWorldCoordinates.y * 0.5f;
  }

  public Vector3 OverridePosition(float deltaTime, Vector3 originalPosition)
  {
    return !this.enabled || !this._transitioning ? originalPosition : this._newPos;
  }

  public int POOrder
  {
    get => this._poOrder;
    set => this._poOrder = value;
  }

  public override void EnteredTrigger()
  {
    base.EnteredTrigger();
    if ((UnityEngine.Object) this.NumericBoundaries.CurrentBoundariesTrigger != (UnityEngine.Object) null)
      this.StartCoroutine(this.TurnOffPreviousTrigger(this.NumericBoundaries.CurrentBoundariesTrigger));
    if ((!((UnityEngine.Object) this.NumericBoundaries.CurrentBoundariesTrigger != (UnityEngine.Object) null) || this.NumericBoundaries.CurrentBoundariesTrigger._instanceID == this._instanceID) && !((UnityEngine.Object) this.NumericBoundaries.CurrentBoundariesTrigger == (UnityEngine.Object) null))
      return;
    this.NumericBoundaries.CurrentBoundariesTrigger = this;
    this.StartCoroutine(this.Transition());
  }

  public override void ExitedTrigger()
  {
    base.ExitedTrigger();
    if (!((UnityEngine.Object) this.NumericBoundaries.CurrentBoundariesTrigger == (UnityEngine.Object) this))
      return;
    this.StartCoroutine(this.ExitTriggerRoutine());
  }

  public IEnumerator TurnOffPreviousTrigger(ProCamera2DTriggerBoundaries trigger)
  {
    yield return (object) new WaitForEndOfFrame();
    trigger._transitioning = false;
  }

  public IEnumerator ExitTriggerRoutine()
  {
    ProCamera2DTriggerBoundaries dtriggerBoundaries = this;
    yield return (object) new WaitForEndOfFrame();
    if (!((UnityEngine.Object) dtriggerBoundaries.NumericBoundaries.CurrentBoundariesTrigger != (UnityEngine.Object) dtriggerBoundaries))
    {
      dtriggerBoundaries._transitioning = false;
      BoundariesAnimator boundsAnim = dtriggerBoundaries._boundsAnim;
      int num1;
      bool flag1 = (num1 = 0) != 0;
      boundsAnim.UseRightBoundary = num1 != 0;
      int num2;
      bool flag2 = (num2 = flag1 ? 1 : 0) != 0;
      boundsAnim.UseLeftBoundary = num2 != 0;
      int num3;
      bool flag3 = (num3 = flag2 ? 1 : 0) != 0;
      boundsAnim.UseBottomBoundary = num3 != 0;
      boundsAnim.UseTopBoundary = flag3;
      boundsAnim.TransitionDuration = 1f;
      boundsAnim.TransitionEaseType = EaseType.EaseInOut;
      boundsAnim.Transition();
      dtriggerBoundaries.NumericBoundaries.CurrentBoundariesTrigger = (ProCamera2DTriggerBoundaries) null;
    }
  }

  public void SetBoundaries()
  {
    if (!((UnityEngine.Object) this.NumericBoundaries != (UnityEngine.Object) null))
      return;
    this.NumericBoundaries.CurrentBoundariesTrigger = this;
    this.NumericBoundaries.UseLeftBoundary = this.UseLeftBoundary;
    if (this.UseLeftBoundary)
      this.NumericBoundaries.LeftBoundary = this.NumericBoundaries.TargetLeftBoundary = this._targetLeftBoundary;
    this.NumericBoundaries.UseRightBoundary = this.UseRightBoundary;
    if (this.UseRightBoundary)
      this.NumericBoundaries.RightBoundary = this.NumericBoundaries.TargetRightBoundary = this._targetRightBoundary;
    this.NumericBoundaries.UseTopBoundary = this.UseTopBoundary;
    if (this.UseTopBoundary)
      this.NumericBoundaries.TopBoundary = this.NumericBoundaries.TargetTopBoundary = this._targetTopBoundary;
    this.NumericBoundaries.UseBottomBoundary = this.UseBottomBoundary;
    if (this.UseBottomBoundary)
      this.NumericBoundaries.BottomBoundary = this.NumericBoundaries.TargetBottomBoundary = this._targetBottomBoundary;
    if (!this.UseTopBoundary && !this.UseBottomBoundary && !this.UseLeftBoundary && !this.UseRightBoundary)
      this.NumericBoundaries.UseNumericBoundaries = false;
    else
      this.NumericBoundaries.UseNumericBoundaries = true;
  }

  public void GetTargetBoundaries()
  {
    if (this.AreBoundariesRelative)
    {
      this._targetTopBoundary = this.Vector3V(this.transform.position) + this.TopBoundary;
      this._targetBottomBoundary = this.Vector3V(this.transform.position) + this.BottomBoundary;
      this._targetLeftBoundary = this.Vector3H(this.transform.position) + this.LeftBoundary;
      this._targetRightBoundary = this.Vector3H(this.transform.position) + this.RightBoundary;
    }
    else
    {
      this._targetTopBoundary = this.TopBoundary;
      this._targetBottomBoundary = this.BottomBoundary;
      this._targetLeftBoundary = this.LeftBoundary;
      this._targetRightBoundary = this.RightBoundary;
    }
  }

  public IEnumerator Transition()
  {
    ProCamera2DTriggerBoundaries dtriggerBoundaries = this;
    if (!dtriggerBoundaries.UseTopBoundary && !dtriggerBoundaries.UseBottomBoundary && !dtriggerBoundaries.UseLeftBoundary && !dtriggerBoundaries.UseRightBoundary)
    {
      dtriggerBoundaries.NumericBoundaries.UseNumericBoundaries = false;
    }
    else
    {
      bool flag = true;
      if (dtriggerBoundaries.UseTopBoundary && ((double) dtriggerBoundaries.NumericBoundaries.TopBoundary != (double) dtriggerBoundaries.TopBoundary || !dtriggerBoundaries.NumericBoundaries.UseTopBoundary))
        flag = false;
      if (dtriggerBoundaries.UseBottomBoundary && ((double) dtriggerBoundaries.NumericBoundaries.BottomBoundary != (double) dtriggerBoundaries.BottomBoundary || !dtriggerBoundaries.NumericBoundaries.UseBottomBoundary))
        flag = false;
      if (dtriggerBoundaries.UseLeftBoundary && ((double) dtriggerBoundaries.NumericBoundaries.LeftBoundary != (double) dtriggerBoundaries.LeftBoundary || !dtriggerBoundaries.NumericBoundaries.UseLeftBoundary))
        flag = false;
      if (dtriggerBoundaries.UseRightBoundary && ((double) dtriggerBoundaries.NumericBoundaries.RightBoundary != (double) dtriggerBoundaries.RightBoundary || !dtriggerBoundaries.NumericBoundaries.UseRightBoundary))
        flag = false;
      if (!flag)
      {
        dtriggerBoundaries.NumericBoundaries.UseNumericBoundaries = true;
        dtriggerBoundaries.GetTargetBoundaries();
        dtriggerBoundaries._boundsAnim.UseTopBoundary = dtriggerBoundaries.UseTopBoundary;
        dtriggerBoundaries._boundsAnim.TopBoundary = dtriggerBoundaries._targetTopBoundary;
        dtriggerBoundaries._boundsAnim.UseBottomBoundary = dtriggerBoundaries.UseBottomBoundary;
        dtriggerBoundaries._boundsAnim.BottomBoundary = dtriggerBoundaries._targetBottomBoundary;
        dtriggerBoundaries._boundsAnim.UseLeftBoundary = dtriggerBoundaries.UseLeftBoundary;
        dtriggerBoundaries._boundsAnim.LeftBoundary = dtriggerBoundaries._targetLeftBoundary;
        dtriggerBoundaries._boundsAnim.UseRightBoundary = dtriggerBoundaries.UseRightBoundary;
        dtriggerBoundaries._boundsAnim.RightBoundary = dtriggerBoundaries._targetRightBoundary;
        dtriggerBoundaries._boundsAnim.TransitionDuration = dtriggerBoundaries.TransitionDuration;
        dtriggerBoundaries._boundsAnim.TransitionEaseType = dtriggerBoundaries.TransitionEaseType;
        if (dtriggerBoundaries.ChangeZoom && (double) dtriggerBoundaries._initialCamSize / (double) dtriggerBoundaries.TargetZoom != (double) dtriggerBoundaries.ProCamera2D.ScreenSizeInWorldCoordinates.y * 0.5)
          dtriggerBoundaries.ProCamera2D.UpdateScreenSize(dtriggerBoundaries._initialCamSize / dtriggerBoundaries.TargetZoom, dtriggerBoundaries.ZoomSmoothness, dtriggerBoundaries.TransitionEaseType);
        if (dtriggerBoundaries._boundsAnim.GetAnimsCount() > 1)
        {
          if (dtriggerBoundaries.NumericBoundaries.MoveCameraToTargetRoutine != null)
            dtriggerBoundaries.NumericBoundaries.StopCoroutine(dtriggerBoundaries.NumericBoundaries.MoveCameraToTargetRoutine);
          dtriggerBoundaries.NumericBoundaries.MoveCameraToTargetRoutine = dtriggerBoundaries.NumericBoundaries.StartCoroutine(dtriggerBoundaries.MoveCameraToTarget());
        }
        yield return (object) new WaitForEndOfFrame();
        dtriggerBoundaries._boundsAnim.Transition();
      }
    }
  }

  public IEnumerator MoveCameraToTarget()
  {
    ProCamera2DTriggerBoundaries dtriggerBoundaries = this;
    float initialCamPosH = dtriggerBoundaries.Vector3H(dtriggerBoundaries.ProCamera2D.LocalPosition);
    float initialCamPosV = dtriggerBoundaries.Vector3V(dtriggerBoundaries.ProCamera2D.LocalPosition);
    dtriggerBoundaries._newPos = dtriggerBoundaries.VectorHVD(initialCamPosH, initialCamPosV, 0.0f);
    dtriggerBoundaries._transitioning = true;
    float t = 0.0f;
    while ((double) t <= 1.0)
    {
      t += dtriggerBoundaries.ProCamera2D.DeltaTime / dtriggerBoundaries.TransitionDuration;
      float horizontalPos = Utils.EaseFromTo(initialCamPosH, dtriggerBoundaries.ProCamera2D.CameraTargetPositionSmoothed.x, t, dtriggerBoundaries.TransitionEaseType);
      float verticalPos = Utils.EaseFromTo(initialCamPosV, dtriggerBoundaries.ProCamera2D.CameraTargetPositionSmoothed.y, t, dtriggerBoundaries.TransitionEaseType);
      dtriggerBoundaries.LimitToNumericBoundaries(ref horizontalPos, ref verticalPos);
      dtriggerBoundaries._newPos = dtriggerBoundaries.VectorHVD(horizontalPos, verticalPos, 0.0f);
      yield return (object) dtriggerBoundaries.ProCamera2D.GetYield();
    }
    dtriggerBoundaries.NumericBoundaries.MoveCameraToTargetRoutine = (Coroutine) null;
    dtriggerBoundaries._transitioning = false;
  }

  public void LimitToNumericBoundaries(ref float horizontalPos, ref float verticalPos)
  {
    if (this.NumericBoundaries.UseLeftBoundary && (double) horizontalPos - (double) this.ProCamera2D.ScreenSizeInWorldCoordinates.x / 2.0 < (double) this.NumericBoundaries.LeftBoundary)
      horizontalPos = this.NumericBoundaries.LeftBoundary + this.ProCamera2D.ScreenSizeInWorldCoordinates.x / 2f;
    else if (this.NumericBoundaries.UseRightBoundary && (double) horizontalPos + (double) this.ProCamera2D.ScreenSizeInWorldCoordinates.x / 2.0 > (double) this.NumericBoundaries.RightBoundary)
      horizontalPos = this.NumericBoundaries.RightBoundary - this.ProCamera2D.ScreenSizeInWorldCoordinates.x / 2f;
    if (this.NumericBoundaries.UseBottomBoundary && (double) verticalPos - (double) this.ProCamera2D.ScreenSizeInWorldCoordinates.y / 2.0 < (double) this.NumericBoundaries.BottomBoundary)
    {
      verticalPos = this.NumericBoundaries.BottomBoundary + this.ProCamera2D.ScreenSizeInWorldCoordinates.y / 2f;
    }
    else
    {
      if (!this.NumericBoundaries.UseTopBoundary || (double) verticalPos + (double) this.ProCamera2D.ScreenSizeInWorldCoordinates.y / 2.0 <= (double) this.NumericBoundaries.TopBoundary)
        return;
      verticalPos = this.NumericBoundaries.TopBoundary - this.ProCamera2D.ScreenSizeInWorldCoordinates.y / 2f;
    }
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__32_0()
  {
    if (this.NumericBoundaries.OnBoundariesTransitionStarted == null)
      return;
    this.NumericBoundaries.OnBoundariesTransitionStarted();
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__32_1()
  {
    if (this.NumericBoundaries.OnBoundariesTransitionFinished == null)
      return;
    this.NumericBoundaries.OnBoundariesTransitionFinished();
  }
}
