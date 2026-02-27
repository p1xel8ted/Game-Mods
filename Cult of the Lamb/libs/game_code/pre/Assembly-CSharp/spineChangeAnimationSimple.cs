// Decompiled with JetBrains decompiler
// Type: spineChangeAnimationSimple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using WebSocketSharp;

#nullable disable
public class spineChangeAnimationSimple : BaseMonoBehaviour
{
  public SkeletonAnimation SkeletonData;
  [SpineAnimation("", "SkeletonData", true, false)]
  public string skeletonAnimation;
  public bool loop;
  public float waitTime;
  public UnityEvent unityEventOnAnimationEnd;
  public string sfx;
  private bool playedAnimation;
  private TrackEntry Track;
  public bool DestroyAfterAnimation;

  public void Play() => this.changeAnimation();

  public void changeAnimation()
  {
    this.SkeletonData.AnimationState.SetAnimation(0, this.skeletonAnimation, this.loop);
    if (!this.sfx.IsNullOrEmpty())
      AudioManager.Instance.PlayOneShot(this.sfx, this.transform.position);
    this.StartCoroutine((IEnumerator) this.DelayAddEvent());
  }

  private IEnumerator DelayAddEvent()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    spineChangeAnimationSimple changeAnimationSimple = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      changeAnimationSimple.SkeletonData.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(changeAnimationSimple.AnimationEnd);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void OnDestroy()
  {
    if (!((Object) this.SkeletonData != (Object) null) || this.SkeletonData.AnimationState == null)
      return;
    this.SkeletonData.AnimationState.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.AnimationEnd);
  }

  private void OnDisable()
  {
    if (!((Object) this.SkeletonData != (Object) null) || this.SkeletonData.AnimationState == null)
      return;
    this.SkeletonData.AnimationState.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.AnimationEnd);
  }

  private void AnimationEnd(TrackEntry trackEntry)
  {
    if (!(trackEntry.Animation.Name == this.skeletonAnimation))
      return;
    this.unityEventOnAnimationEnd.Invoke();
    if (!this.DestroyAfterAnimation)
      return;
    Object.Destroy((Object) this.gameObject);
  }
}
