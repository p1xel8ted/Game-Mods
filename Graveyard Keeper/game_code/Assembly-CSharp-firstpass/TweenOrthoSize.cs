// Decompiled with JetBrains decompiler
// Type: TweenOrthoSize
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Tween/Tween Orthographic Size")]
[RequireComponent(typeof (Camera))]
public class TweenOrthoSize : UITweener
{
  public float from = 1f;
  public float to = 1f;
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
  public float orthoSize
  {
    get => this.value;
    set => this.value = value;
  }

  public float value
  {
    get => this.cachedCamera.orthographicSize;
    set => this.cachedCamera.orthographicSize = value;
  }

  public override void OnUpdate(float factor, bool isFinished)
  {
    this.value = (float) ((double) this.from * (1.0 - (double) factor) + (double) this.to * (double) factor);
  }

  public static TweenOrthoSize Begin(GameObject go, float duration, float to)
  {
    TweenOrthoSize tweenOrthoSize = UITweener.Begin<TweenOrthoSize>(go, duration);
    tweenOrthoSize.from = tweenOrthoSize.value;
    tweenOrthoSize.to = to;
    if ((double) duration <= 0.0)
    {
      tweenOrthoSize.Sample(1f, true);
      tweenOrthoSize.enabled = false;
    }
    return tweenOrthoSize;
  }

  public override void SetStartToCurrentValue() => this.from = this.value;

  public override void SetEndToCurrentValue() => this.to = this.value;
}
