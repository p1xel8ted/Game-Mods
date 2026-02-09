// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DForwardFocus
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[HelpURL("http://www.procamera2d.com/user-guide/extension-forward-focus/")]
public class ProCamera2DForwardFocus : BasePC2D, IPreMover
{
  public static string ExtensionName = "Forward Focus";
  public const float EPSILON = 0.001f;
  public bool Progressive = true;
  public float SpeedMultiplier = 1f;
  public float TransitionSmoothness = 0.5f;
  public bool MaintainInfluenceOnStop = true;
  public Vector2 MovementThreshold = Vector2.zero;
  [Range(0.001f, 0.5f)]
  public float LeftFocus = 0.25f;
  [Range(0.001f, 0.5f)]
  public float RightFocus = 0.25f;
  [Range(0.001f, 0.5f)]
  public float TopFocus = 0.25f;
  [Range(0.001f, 0.5f)]
  public float BottomFocus = 0.25f;
  public float _hVel;
  public float _hVelSmooth;
  public float _vVel;
  public float _vVelSmooth;
  public float _targetHVel;
  public float _targetVVel;
  public bool _isFirstHorizontalCameraMovement;
  public bool _isFirstVerticalCameraMovement;
  public bool __enabled;
  public int _prmOrder = 2000;

  public override void Awake()
  {
    base.Awake();
    this.StartCoroutine(this.Enable());
    Com.LuisPedroFonseca.ProCamera2D.ProCamera2D.Instance.AddPreMover((IPreMover) this);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.ProCamera2D.RemovePreMover((IPreMover) this);
  }

  public void PreMove(float deltaTime)
  {
    if (!this.__enabled || !this.enabled)
      return;
    this.ApplyInfluence(deltaTime);
  }

  public int PrMOrder
  {
    get => this._prmOrder;
    set => this._prmOrder = value;
  }

  public override void OnReset()
  {
    this._hVel = 0.0f;
    this._hVelSmooth = 0.0f;
    this._vVel = 0.0f;
    this._vVelSmooth = 0.0f;
    this._targetHVel = 0.0f;
    this._targetVVel = 0.0f;
    this._isFirstHorizontalCameraMovement = false;
    this._isFirstVerticalCameraMovement = false;
    this.__enabled = false;
    this.StartCoroutine(this.Enable());
  }

  public IEnumerator Enable()
  {
    yield return (object) new WaitForEndOfFrame();
    this.__enabled = true;
  }

  public void ApplyInfluence(float deltaTime)
  {
    float f1 = this.Vector3H(this.ProCamera2D.TargetsMidPoint) - this.Vector3H(this.ProCamera2D.PreviousTargetsMidPoint);
    float f2 = (double) Mathf.Abs(f1) >= (double) this.MovementThreshold.x ? f1 / deltaTime : 0.0f;
    float f3 = this.Vector3V(this.ProCamera2D.TargetsMidPoint) - this.Vector3V(this.ProCamera2D.PreviousTargetsMidPoint);
    float f4 = (double) Mathf.Abs(f3) >= (double) this.MovementThreshold.y ? f3 / deltaTime : 0.0f;
    float f5;
    float f6;
    if (this.Progressive)
    {
      f5 = Mathf.Clamp(f2 * this.SpeedMultiplier, -this.LeftFocus * this.ProCamera2D.ScreenSizeInWorldCoordinates.x, this.RightFocus * this.ProCamera2D.ScreenSizeInWorldCoordinates.x);
      f6 = Mathf.Clamp(f4 * this.SpeedMultiplier, -this.BottomFocus * this.ProCamera2D.ScreenSizeInWorldCoordinates.y, this.TopFocus * this.ProCamera2D.ScreenSizeInWorldCoordinates.y);
      if (this.MaintainInfluenceOnStop)
      {
        if ((double) Mathf.Sign(f5) == 1.0 && (double) f5 < (double) this._hVel || (double) Mathf.Sign(f5) == -1.0 && (double) f5 > (double) this._hVel || (double) Mathf.Abs(f5) < 1.0 / 1000.0)
          f5 = this._hVel;
        if ((double) Mathf.Sign(f6) == 1.0 && (double) f6 < (double) this._vVel || (double) Mathf.Sign(f6) == -1.0 && (double) f6 > (double) this._vVel || (double) Mathf.Abs(f6) < 1.0 / 1000.0)
          f6 = this._vVel;
      }
    }
    else if (this.MaintainInfluenceOnStop)
    {
      bool flag1;
      if (!this._isFirstHorizontalCameraMovement && (double) Mathf.Abs(f2) >= 1.0 / 1000.0)
      {
        this._isFirstHorizontalCameraMovement = true;
        flag1 = true;
      }
      else
        flag1 = (double) Mathf.Sign(f2) != (double) Mathf.Sign(this._targetHVel);
      if ((double) Mathf.Abs(f2) >= 1.0 / 1000.0 & flag1)
        this._targetHVel = ((double) f2 < 0.0 ? -this.LeftFocus : this.RightFocus) * this.ProCamera2D.ScreenSizeInWorldCoordinates.x;
      f5 = this._targetHVel;
      bool flag2;
      if (!this._isFirstVerticalCameraMovement && (double) Mathf.Abs(f4) >= 1.0 / 1000.0)
      {
        this._isFirstVerticalCameraMovement = true;
        flag2 = true;
      }
      else
        flag2 = (double) Mathf.Sign(f4) != (double) Mathf.Sign(this._targetVVel);
      if ((double) Mathf.Abs(f4) >= 1.0 / 1000.0 & flag2)
        this._targetVVel = ((double) f4 < 0.0 ? -this.BottomFocus : this.TopFocus) * this.ProCamera2D.ScreenSizeInWorldCoordinates.y;
      f6 = this._targetVVel;
    }
    else
    {
      f5 = (double) Mathf.Abs(f2) < 1.0 / 1000.0 ? 0.0f : ((double) f2 < 0.0 ? -this.LeftFocus : this.RightFocus) * this.ProCamera2D.ScreenSizeInWorldCoordinates.x;
      f6 = (double) Mathf.Abs(f4) < 1.0 / 1000.0 ? 0.0f : ((double) f4 < 0.0 ? -this.BottomFocus : this.TopFocus) * this.ProCamera2D.ScreenSizeInWorldCoordinates.y;
    }
    float target1 = Mathf.Clamp(f5, -this.LeftFocus * this.ProCamera2D.ScreenSizeInWorldCoordinates.x, this.RightFocus * this.ProCamera2D.ScreenSizeInWorldCoordinates.x);
    float target2 = Mathf.Clamp(f6, -this.BottomFocus * this.ProCamera2D.ScreenSizeInWorldCoordinates.y, this.TopFocus * this.ProCamera2D.ScreenSizeInWorldCoordinates.y);
    this._hVel = Mathf.SmoothDamp(this._hVel, target1, ref this._hVelSmooth, this.TransitionSmoothness, float.MaxValue, deltaTime);
    this._vVel = Mathf.SmoothDamp(this._vVel, target2, ref this._vVelSmooth, this.TransitionSmoothness, float.MaxValue, deltaTime);
    this.ProCamera2D.ApplyInfluence(new Vector2(this._hVel, this._vVel));
  }
}
