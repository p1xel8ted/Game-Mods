// Decompiled with JetBrains decompiler
// Type: spineChangeAnimationSimple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.Events;
using WebSocketSharp;

#nullable disable
public class spineChangeAnimationSimple : BaseMonoBehaviour
{
  public SkeletonAnimation SkeletonData;
  [SpineAnimation("", "SkeletonData", true, false)]
  public string skeletonAnimation;
  public bool loop;
  public bool add;
  public float waitTime;
  public UnityEvent unityEventOnAnimationEnd;
  public string sfx;
  public bool finished;
  public bool useGameManager;
  public TrackEntry Track;
  public bool DestroyAfterAnimation;

  public void Play() => this.changeAnimation();

  public void changeAnimation()
  {
    this.finished = false;
    if (this.useGameManager)
    {
      GameManager.GetInstance().WaitForSeconds(this.waitTime, (System.Action) (() =>
      {
        if (this.add)
          this.SkeletonData.AnimationState.AddAnimation(0, this.skeletonAnimation, this.loop, 0.0f);
        else
          this.SkeletonData.AnimationState.SetAnimation(0, this.skeletonAnimation, this.loop);
        if (!this.sfx.IsNullOrEmpty())
          AudioManager.Instance.PlayOneShot(this.sfx, this.transform.position);
        this.StartCoroutine((IEnumerator) this.DelayAddEvent());
      }));
    }
    else
    {
      if ((UnityEngine.Object) this.SkeletonData != (UnityEngine.Object) null || this.SkeletonData.AnimationState != null)
      {
        if (this.add)
          this.SkeletonData.AnimationState.AddAnimation(0, this.skeletonAnimation, this.loop, 0.0f);
        else
          this.SkeletonData.AnimationState.SetAnimation(0, this.skeletonAnimation, this.loop);
      }
      if (!this.sfx.IsNullOrEmpty())
        AudioManager.Instance.PlayOneShot(this.sfx, this.transform.position);
      if (!this.gameObject.activeSelf)
        return;
      this.StartCoroutine((IEnumerator) this.DelayAddEvent());
    }
  }

  public void Finish()
  {
    if (this.finished)
      return;
    this.finished = true;
    this.unityEventOnAnimationEnd.Invoke();
    if (!this.DestroyAfterAnimation)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public IEnumerator DelayAddEvent()
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

  public void OnDestroy()
  {
    if (!((UnityEngine.Object) this.SkeletonData != (UnityEngine.Object) null) || this.SkeletonData.AnimationState == null)
      return;
    this.SkeletonData.AnimationState.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.AnimationEnd);
  }

  public void OnDisable()
  {
    if (!((UnityEngine.Object) this.SkeletonData != (UnityEngine.Object) null) || this.SkeletonData.AnimationState == null)
      return;
    this.SkeletonData.AnimationState.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.AnimationEnd);
  }

  public void AnimationEnd(TrackEntry trackEntry)
  {
    if (!(trackEntry.Animation.Name == this.skeletonAnimation))
      return;
    this.Finish();
  }

  [CompilerGenerated]
  public void \u003CchangeAnimation\u003Eb__12_0()
  {
    if (this.add)
      this.SkeletonData.AnimationState.AddAnimation(0, this.skeletonAnimation, this.loop, 0.0f);
    else
      this.SkeletonData.AnimationState.SetAnimation(0, this.skeletonAnimation, this.loop);
    if (!this.sfx.IsNullOrEmpty())
      AudioManager.Instance.PlayOneShot(this.sfx, this.transform.position);
    this.StartCoroutine((IEnumerator) this.DelayAddEvent());
  }
}
