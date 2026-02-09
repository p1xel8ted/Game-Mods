// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DTriggerZoom
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[HelpURL("http://www.procamera2d.com/user-guide/trigger-zoom/")]
public class ProCamera2DTriggerZoom : BaseTrigger
{
  public static string TriggerName = "Zoom Trigger";
  public bool SetSizeAsMultiplier = true;
  public float TargetZoom = 1.5f;
  public float ZoomSmoothness = 1f;
  [Range(0.0f, 1f)]
  public float ExclusiveInfluencePercentage = 0.25f;
  public bool ResetSizeOnExit;
  public float ResetSizeSmoothness = 1f;
  public float _startCamSize;
  public float _initialCamSize;
  public float _targetCamSize;
  public float _targetCamSizeSmoothed;
  public float _previousCamSize;
  public float _zoomVelocity;
  public float _initialCamDepth;

  public void Start()
  {
    if ((UnityEngine.Object) this.ProCamera2D == (UnityEngine.Object) null)
      return;
    this._startCamSize = this.ProCamera2D.ScreenSizeInWorldCoordinates.y * 0.5f;
    this._initialCamSize = this._startCamSize;
    this._targetCamSize = this._startCamSize;
    this._targetCamSizeSmoothed = this._startCamSize;
    this._initialCamDepth = this.Vector3D(this.ProCamera2D.LocalPosition);
  }

  public override void EnteredTrigger()
  {
    base.EnteredTrigger();
    this.ProCamera2D.CurrentZoomTriggerID = this._instanceID;
    if (this.ResetSizeOnExit)
    {
      this._initialCamSize = this._startCamSize;
      this._targetCamSize = this.ProCamera2D.ScreenSizeInWorldCoordinates.y * 0.5f;
      this._targetCamSizeSmoothed = this.ProCamera2D.ScreenSizeInWorldCoordinates.y * 0.5f;
    }
    else
    {
      this._initialCamSize = this.ProCamera2D.ScreenSizeInWorldCoordinates.y * 0.5f;
      this._targetCamSize = this._initialCamSize;
      this._targetCamSizeSmoothed = this._initialCamSize;
    }
    this.StartCoroutine(this.InsideTriggerRoutine());
  }

  public override void ExitedTrigger()
  {
    base.ExitedTrigger();
    if (!this.ResetSizeOnExit)
      return;
    this._targetCamSize = this._startCamSize;
    this.StartCoroutine(this.OutsideTriggerRoutine());
  }

  public IEnumerator InsideTriggerRoutine()
  {
    ProCamera2DTriggerZoom camera2DtriggerZoom = this;
    while (camera2DtriggerZoom._insideTrigger && camera2DtriggerZoom._instanceID == camera2DtriggerZoom.ProCamera2D.CurrentZoomTriggerID)
    {
      camera2DtriggerZoom._exclusiveInfluencePercentage = camera2DtriggerZoom.ExclusiveInfluencePercentage;
      Vector2 point = new Vector2(camera2DtriggerZoom.Vector3H(camera2DtriggerZoom.UseTargetsMidPoint ? camera2DtriggerZoom.ProCamera2D.TargetsMidPoint : camera2DtriggerZoom.TriggerTarget.position), camera2DtriggerZoom.Vector3V(camera2DtriggerZoom.UseTargetsMidPoint ? camera2DtriggerZoom.ProCamera2D.TargetsMidPoint : camera2DtriggerZoom.TriggerTarget.position));
      float centerPercentage = camera2DtriggerZoom.GetDistanceToCenterPercentage(point);
      float num1 = !camera2DtriggerZoom.SetSizeAsMultiplier ? (!camera2DtriggerZoom.ProCamera2D.GameCamera.orthographic ? Mathf.Abs(camera2DtriggerZoom._initialCamDepth) * Mathf.Tan((float) ((double) camera2DtriggerZoom.TargetZoom * 0.5 * (Math.PI / 180.0))) : camera2DtriggerZoom.TargetZoom) : camera2DtriggerZoom._startCamSize / camera2DtriggerZoom.TargetZoom;
      float num2 = (float) ((double) camera2DtriggerZoom._initialCamSize * (double) centerPercentage + (double) num1 * (1.0 - (double) centerPercentage));
      if ((double) num1 > (double) camera2DtriggerZoom.ProCamera2D.ScreenSizeInWorldCoordinates.y * 0.5 && (double) num2 > (double) camera2DtriggerZoom._targetCamSize || (double) num1 < (double) camera2DtriggerZoom.ProCamera2D.ScreenSizeInWorldCoordinates.y * 0.5 && (double) num2 < (double) camera2DtriggerZoom._targetCamSize || camera2DtriggerZoom.ResetSizeOnExit)
        camera2DtriggerZoom._targetCamSize = num2;
      camera2DtriggerZoom._previousCamSize = camera2DtriggerZoom.ProCamera2D.ScreenSizeInWorldCoordinates.y;
      yield return (object) camera2DtriggerZoom.ProCamera2D.GetYield();
      if ((double) Mathf.Abs(camera2DtriggerZoom.ProCamera2D.ScreenSizeInWorldCoordinates.y * 0.5f - camera2DtriggerZoom._targetCamSize) > 9.9999997473787516E-05)
        camera2DtriggerZoom.UpdateScreenSize(camera2DtriggerZoom.ResetSizeOnExit ? camera2DtriggerZoom.ResetSizeSmoothness : camera2DtriggerZoom.ZoomSmoothness);
      if ((double) camera2DtriggerZoom._previousCamSize == (double) camera2DtriggerZoom.ProCamera2D.ScreenSizeInWorldCoordinates.y)
      {
        camera2DtriggerZoom._targetCamSize = camera2DtriggerZoom.ProCamera2D.ScreenSizeInWorldCoordinates.y * 0.5f;
        camera2DtriggerZoom._targetCamSizeSmoothed = camera2DtriggerZoom._targetCamSize;
        camera2DtriggerZoom._zoomVelocity = 0.0f;
      }
    }
  }

  public IEnumerator OutsideTriggerRoutine()
  {
    ProCamera2DTriggerZoom camera2DtriggerZoom = this;
    while (!camera2DtriggerZoom._insideTrigger && camera2DtriggerZoom._instanceID == camera2DtriggerZoom.ProCamera2D.CurrentZoomTriggerID && (double) Mathf.Abs(camera2DtriggerZoom.ProCamera2D.ScreenSizeInWorldCoordinates.y * 0.5f - camera2DtriggerZoom._targetCamSize) > 9.9999997473787516E-05)
    {
      camera2DtriggerZoom.UpdateScreenSize(camera2DtriggerZoom.ResetSizeOnExit ? camera2DtriggerZoom.ResetSizeSmoothness : camera2DtriggerZoom.ZoomSmoothness);
      yield return (object) camera2DtriggerZoom.ProCamera2D.GetYield();
    }
    camera2DtriggerZoom._zoomVelocity = 0.0f;
  }

  public void UpdateScreenSize(float smoothness)
  {
    this._targetCamSizeSmoothed = Mathf.SmoothDamp(this._targetCamSizeSmoothed, this._targetCamSize, ref this._zoomVelocity, smoothness, float.MaxValue, this.ProCamera2D.DeltaTime);
    this.ProCamera2D.UpdateScreenSize(this._targetCamSizeSmoothed);
  }
}
