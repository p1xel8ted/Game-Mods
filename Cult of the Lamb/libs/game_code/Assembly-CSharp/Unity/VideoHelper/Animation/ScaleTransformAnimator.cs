// Decompiled with JetBrains decompiler
// Type: Unity.VideoHelper.Animation.ScaleTransformAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Unity.VideoHelper.Animation;

public class ScaleTransformAnimator : AnimationCurveAnimator, IActivate, IDeactivate
{
  public Transform Target;

  public void Activate()
  {
    this.Animate(this.In, this.InDuration, (Action<float>) (x => this.Target.localScale = new Vector3(x, x, x)));
  }

  public void Deactivate()
  {
    this.Animate(this.Out, this.OutDuration, (Action<float>) (x => this.Target.localScale = new Vector3(x, x, x)));
  }

  [CompilerGenerated]
  public void \u003CActivate\u003Eb__1_0(float x) => this.Target.localScale = new Vector3(x, x, x);

  [CompilerGenerated]
  public void \u003CDeactivate\u003Eb__2_0(float x)
  {
    this.Target.localScale = new Vector3(x, x, x);
  }
}
