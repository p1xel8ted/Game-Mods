// Decompiled with JetBrains decompiler
// Type: spineChangeAnimationSimple_UI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class spineChangeAnimationSimple_UI : BaseMonoBehaviour
{
  public SkeletonGraphic SkeletonData;
  [SpineAnimation("", "SkeletonData", true, false)]
  public string skeletonAnimation;
  public bool loop;
  public float waitTime;
  public UnityEvent unityEventOnAnimationEnd;
  public AudioClip sfx;
  public bool playedAnimation;
  public TrackEntry Track;

  public void Start()
  {
  }

  public void Update()
  {
  }

  public void changeAnimation()
  {
    this.Track = this.SkeletonData.AnimationState.SetAnimation(0, this.skeletonAnimation, this.loop);
    this.SkeletonData.AnimationState.End += (Spine.AnimationState.TrackEntryDelegate) (_param1 => this.unityEventOnAnimationEnd.Invoke());
  }

  [CompilerGenerated]
  public void \u003CchangeAnimation\u003Eb__10_0(TrackEntry _param1)
  {
    this.unityEventOnAnimationEnd.Invoke();
  }
}
