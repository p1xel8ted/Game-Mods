// Decompiled with JetBrains decompiler
// Type: TweenFOV
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Camera))]
[AddComponentMenu("NGUI/Tween/Tween Field of View")]
public class TweenFOV : UITweener
{
  public float from = 45f;
  public float to = 45f;
  public Camera mCam;

  public Camera cachedCamera
  {
    get
    {
      if ((UnityEngine.Object) this.mCam == (UnityEngine.Object) null)
        this.mCam = this.GetComponent<Camera>();
      return this.mCam;
    }
  }

  [Obsolete("Use 'value' instead")]
  public float fov
  {
    get => this.value;
    set => this.value = value;
  }

  public float value
  {
    get => this.cachedCamera.fieldOfView;
    set => this.cachedCamera.fieldOfView = value;
  }

  public override void OnUpdate(float factor, bool isFinished)
  {
    this.value = (float) ((double) this.from * (1.0 - (double) factor) + (double) this.to * (double) factor);
  }

  public static TweenFOV Begin(GameObject go, float duration, float to)
  {
    TweenFOV tweenFov = UITweener.Begin<TweenFOV>(go, duration);
    tweenFov.from = tweenFov.value;
    tweenFov.to = to;
    if ((double) duration <= 0.0)
    {
      tweenFov.Sample(1f, true);
      tweenFov.enabled = false;
    }
    return tweenFov;
  }

  [ContextMenu("Set 'From' to current value")]
  public override void SetStartToCurrentValue() => this.from = this.value;

  [ContextMenu("Set 'To' to current value")]
  public override void SetEndToCurrentValue() => this.to = this.value;

  [ContextMenu("Assume value of 'From'")]
  public void SetCurrentValueToStart() => this.value = this.from;

  [ContextMenu("Assume value of 'To'")]
  public void SetCurrentValueToEnd() => this.value = this.to;
}
