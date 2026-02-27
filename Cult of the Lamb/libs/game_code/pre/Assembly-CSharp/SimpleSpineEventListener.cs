// Decompiled with JetBrains decompiler
// Type: SimpleSpineEventListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using UnityEngine;

#nullable disable
public class SimpleSpineEventListener : BaseMonoBehaviour
{
  public SkeletonAnimation skeletonAnimation;

  private void Start()
  {
    if ((Object) this.skeletonAnimation == (Object) null)
      this.skeletonAnimation = this.GetComponent<SkeletonAnimation>();
    this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  public event SimpleSpineEventListener.SpineEvent OnSpineEvent;

  private void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (this.OnSpineEvent == null)
      return;
    this.OnSpineEvent(e.Data.Name);
  }

  private void OnDestroy()
  {
    this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  public delegate void SpineEvent(string EventName);
}
