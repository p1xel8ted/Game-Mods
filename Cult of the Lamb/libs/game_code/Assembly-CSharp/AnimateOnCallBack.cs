// Decompiled with JetBrains decompiler
// Type: AnimateOnCallBack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class AnimateOnCallBack : BaseMonoBehaviour
{
  public string AnimationName;
  public string AddAnimationName;
  public bool Looping;
  public SkeletonAnimation Spine;
  public bool DestroyAfterAnimation;
  public UnityEvent Callback;
  public int Direction;

  public void SetSpineFacing() => this.Spine.skeleton.ScaleX = (float) this.Direction;

  public void Animate() => this.StartCoroutine((IEnumerator) this.DoAnimation());

  public IEnumerator DoAnimation()
  {
    AnimateOnCallBack animateOnCallBack = this;
    float duration = animateOnCallBack.Spine.AnimationState.SetAnimation(0, animateOnCallBack.AnimationName, animateOnCallBack.AddAnimationName != "" && animateOnCallBack.Looping).Animation.Duration;
    if (animateOnCallBack.AddAnimationName != "")
    {
      TrackEntry trackEntry = animateOnCallBack.Spine.AnimationState.AddAnimation(0, animateOnCallBack.AddAnimationName, animateOnCallBack.Looping, 0.0f);
      duration += trackEntry.Animation.Duration;
    }
    yield return (object) new WaitForSeconds(duration);
    if (animateOnCallBack.Callback.GetPersistentEventCount() > 0)
      animateOnCallBack.Callback.Invoke();
    if (animateOnCallBack.DestroyAfterAnimation)
      Object.Destroy((Object) animateOnCallBack.gameObject);
  }
}
