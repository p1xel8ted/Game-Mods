// Decompiled with JetBrains decompiler
// Type: SimpleSpineEventListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using UnityEngine;

#nullable disable
public class SimpleSpineEventListener : BaseMonoBehaviour
{
  public SkeletonAnimation skeletonAnimation;

  public void Start()
  {
    if ((Object) this.skeletonAnimation == (Object) null)
      this.skeletonAnimation = this.GetComponent<SkeletonAnimation>();
    this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  public event SimpleSpineEventListener.SpineEvent OnSpineEvent;

  public void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (this.OnSpineEvent == null)
      return;
    this.OnSpineEvent(e.Data.Name);
  }

  public void OnDestroy()
  {
    this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  public delegate void SpineEvent(string EventName);
}
