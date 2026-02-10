// Decompiled with JetBrains decompiler
// Type: Unity.VideoHelper.Animation.AnimationCurveAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Unity.VideoHelper.Animation;

public abstract class AnimationCurveAnimator : MonoBehaviour
{
  [SerializeField]
  public AnimationCurve inAnimation = AnimationCurve.EaseInOut(0.0f, 0.0f, 1f, 1f);
  [SerializeField]
  public AnimationCurve outAnimation = AnimationCurve.EaseInOut(0.0f, 1f, 1f, 0.0f);
  [SerializeField]
  public bool smooth = true;
  public float time;
  public Coroutine currentCoroutine;
  [CompilerGenerated]
  public float \u003CInDuration\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003COutDuration\u003Ek__BackingField;

  public float InDuration
  {
    get => this.\u003CInDuration\u003Ek__BackingField;
    set => this.\u003CInDuration\u003Ek__BackingField = value;
  }

  public float OutDuration
  {
    get => this.\u003COutDuration\u003Ek__BackingField;
    set => this.\u003COutDuration\u003Ek__BackingField = value;
  }

  public AnimationCurve In
  {
    get => this.inAnimation;
    set
    {
      this.inAnimation = value;
      this.InDuration = this.inAnimation.keys[this.inAnimation.keys.Length - 1].time;
    }
  }

  public AnimationCurve Out
  {
    get => this.outAnimation;
    set
    {
      this.outAnimation = value;
      this.OutDuration = this.outAnimation.keys[this.outAnimation.keys.Length - 1].time;
    }
  }

  public bool Smooth
  {
    get => this.smooth;
    set => this.smooth = value;
  }

  public virtual void OnEnable()
  {
    this.In = this.inAnimation;
    this.Out = this.outAnimation;
  }

  public void Animate(AnimationCurve curve, float duration, Action<float> action)
  {
    if (this.currentCoroutine != null)
      this.StopCoroutine(this.currentCoroutine);
    this.time = !this.smooth ? 0.0f : Mathf.Clamp(duration - this.time, 0.0f, duration);
    this.currentCoroutine = this.StartCoroutine((IEnumerator) this.AnimateInternal(curve, duration, action));
  }

  public virtual IEnumerator AnimateInternal(
    AnimationCurve curve,
    float duration,
    Action<float> action)
  {
    this.CallbackStarting(curve);
    while ((double) this.time < (double) duration)
    {
      action(curve.Evaluate(this.time));
      this.time += Time.deltaTime;
      yield return (object) null;
    }
    this.CallbackFinished(curve);
  }

  public void CallbackFinished(AnimationCurve curve)
  {
    if (curve == this.In)
    {
      this.InFinished();
    }
    else
    {
      if (curve != this.Out)
        return;
      this.OutFinished();
    }
  }

  public void CallbackStarting(AnimationCurve curve)
  {
    if (curve == this.In)
    {
      this.InStarting();
    }
    else
    {
      if (curve != this.Out)
        return;
      this.OutStarting();
    }
  }

  public virtual void InStarting()
  {
  }

  public virtual void OutStarting()
  {
  }

  public virtual void InFinished()
  {
  }

  public virtual void OutFinished()
  {
  }
}
