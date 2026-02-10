// Decompiled with JetBrains decompiler
// Type: VolumeAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using Unity.VideoHelper.Animation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class VolumeAnimator : AnimationCurveAnimator, IPointerExitHandler, IEventSystemHandler
{
  public LayoutElement Target;
  public RectTransform Transform;
  public Selectable Selectable;
  public bool isEntered;

  public void Start()
  {
    this.Transform.gameObject.AddComponent<PointerRouter>().OnEnter += (System.Action) (() =>
    {
      if (this.isEntered)
        return;
      this.Animate(this.In, this.InDuration, (Action<float>) (x => this.Target.preferredWidth = x));
    });
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    this.Animate(this.Out, this.OutDuration, (Action<float>) (x => this.Target.preferredWidth = x));
    this.isEntered = false;
  }

  public override void InStarting() => this.Selectable.interactable = true;

  public override void InFinished() => this.isEntered = true;

  public override void OutFinished() => this.Selectable.interactable = false;

  [CompilerGenerated]
  public void \u003CStart\u003Eb__4_0()
  {
    if (this.isEntered)
      return;
    this.Animate(this.In, this.InDuration, (Action<float>) (x => this.Target.preferredWidth = x));
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__4_1(float x) => this.Target.preferredWidth = x;

  [CompilerGenerated]
  public void \u003COnPointerExit\u003Eb__5_0(float x) => this.Target.preferredWidth = x;
}
