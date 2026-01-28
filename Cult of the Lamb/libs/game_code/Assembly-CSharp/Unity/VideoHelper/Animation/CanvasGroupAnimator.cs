// Decompiled with JetBrains decompiler
// Type: Unity.VideoHelper.Animation.CanvasGroupAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
namespace Unity.VideoHelper.Animation;

public class CanvasGroupAnimator : 
  AnimationCurveAnimator,
  IPointerEnterHandler,
  IEventSystemHandler,
  IPointerExitHandler
{
  public CanvasGroup Group;

  public void OnPointerEnter(PointerEventData eventData)
  {
    this.Animate(this.In, this.InDuration, (Action<float>) (x => this.Group.alpha = x));
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    this.Animate(this.Out, this.OutDuration, (Action<float>) (x => this.Group.alpha = x));
  }

  [CompilerGenerated]
  public void \u003COnPointerEnter\u003Eb__1_0(float x) => this.Group.alpha = x;

  [CompilerGenerated]
  public void \u003COnPointerExit\u003Eb__2_0(float x) => this.Group.alpha = x;
}
