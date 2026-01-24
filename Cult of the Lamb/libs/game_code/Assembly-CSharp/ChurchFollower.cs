// Decompiled with JetBrains decompiler
// Type: ChurchFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class ChurchFollower : BaseMonoBehaviour
{
  public SkeletonAnimation Spine;
  public WorshipperInfoManager wim;
  public WorshipperBubble worshipperBubble;
  [SpineAnimation("", "SkeletonData", true, false)]
  public string AnimIdle;
  [SpineAnimation("", "SkeletonData", true, false)]
  public string AnimHoodUp;
  [SpineAnimation("", "SkeletonData", true, false)]
  public string AnimHoodDown;
  [SpineAnimation("", "SkeletonData", true, false)]
  public string AnimWalking;
  [SpineAnimation("", "SkeletonData", true, false)]
  public string AnimPray;
  [SpineAnimation("", "SkeletonData", true, false)]
  public string AnimWorship;
  public Gradient ColorGradient;
  public System.Action Callback;

  public void HoodOn(string Animation, bool Snap)
  {
    if (Snap)
    {
      this.wim.SetOutfit(WorshipperInfoManager.Outfit.Follower, true);
      this.Spine.AnimationState.SetAnimation(0, Animation, true);
    }
    else
      this.StartCoroutine((IEnumerator) this.PutHoodOn(Animation));
  }

  public IEnumerator PutHoodOn(string Animation)
  {
    if (!this.wim.IsHooded)
    {
      this.Spine.AnimationState.SetAnimation(0, this.AnimHoodUp, false);
      this.Spine.AnimationState.AddAnimation(0, Animation, true, 0.0f);
      yield return (object) new WaitForSeconds(0.6333333f);
      this.wim.SetOutfit(WorshipperInfoManager.Outfit.Follower, true);
    }
    else
      this.Spine.AnimationState.SetAnimation(0, Animation, true);
    yield return (object) new WaitForSeconds(0.333333343f);
  }

  public void Pray() => this.Spine.AnimationState.SetAnimation(0, this.AnimPray, true);

  public void Worship() => this.Spine.AnimationState.SetAnimation(0, this.AnimWorship, true);

  public void WorshipUp() => this.Spine.AnimationState.SetAnimation(0, "idle-ritual-up", true);

  public void Dance() => this.Spine.AnimationState.SetAnimation(0, "dance-hooded", true);

  public void DevotionLoop()
  {
    this.Spine.AnimationState.SetAnimation(0, "devotion/devotion-waiting", true);
  }

  public void DevotionRefuseSoul()
  {
    this.Spine.AnimationState.SetAnimation(0, "devotion/refuse", false);
    this.Spine.AnimationState.AddAnimation(0, "devotion/refused", true, 0.0f);
  }

  public void AnimateWhenPlayerNear(string Animation, string AnimationNear)
  {
    this.StartCoroutine((IEnumerator) this.AnimateWhenPlayerNearRouitine(Animation, AnimationNear));
  }

  public void ArriveAtDevotionCircle()
  {
    this.StartCoroutine((IEnumerator) this.ArriveAtDevotionCircleRoutine());
  }

  public IEnumerator ArriveAtDevotionCircleRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    ChurchFollower churchFollower = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      churchFollower.AnimateWhenPlayerNear("worship", "devotion/devotion-waiting");
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    churchFollower.FacePosition(ChurchFollowerManager.Instance.RitualCenterPosition.position);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) churchFollower.StartCoroutine((IEnumerator) churchFollower.PutHoodOn("devotion/devotion-waiting"));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void ArriveAtRitualCircle()
  {
    this.StartCoroutine((IEnumerator) this.ArriveAtRitualCircleRoutine());
  }

  public IEnumerator ArriveAtRitualCircleRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    ChurchFollower churchFollower = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    churchFollower.FacePosition(ChurchFollowerManager.Instance.RitualCenterPosition.position);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) churchFollower.StartCoroutine((IEnumerator) churchFollower.PutHoodOn("ritual1"));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void ArriveAtSermonAudience()
  {
    this.StartCoroutine((IEnumerator) this.ArriveAtSermonAudienceRoutine());
  }

  public IEnumerator ArriveAtSermonAudienceRoutine()
  {
    this.FacePosition(ChurchFollowerManager.Instance.AltarPosition.position);
    yield return (object) this.PutHoodOn("devotion/devotion-waiting");
  }

  public IEnumerator AnimateWhenPlayerNearRouitine(string Animation, string AnimationNear)
  {
    ChurchFollower churchFollower = this;
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      yield return (object) null;
    while (true)
    {
      if ((double) Vector3.Distance(churchFollower.transform.position, PlayerFarming.Instance.transform.position) > 1.5)
      {
        if ((double) PlayerFarming.Instance.transform.position.y > (double) churchFollower.transform.position.y)
        {
          if (churchFollower.Spine.AnimationName != "idle-ritual-up")
            churchFollower.Spine.AnimationState.SetAnimation(0, "idle-ritual-up", true);
        }
        else if (churchFollower.Spine.AnimationName != Animation)
          churchFollower.Spine.AnimationState.SetAnimation(0, Animation, true);
      }
      else if (churchFollower.Spine.AnimationName != AnimationNear)
        churchFollower.Spine.AnimationState.SetAnimation(0, AnimationNear, true);
      yield return (object) null;
    }
  }

  public void HoodOff(string Animation = "idle")
  {
    if (!this.wim.IsHooded)
      return;
    this.StartCoroutine((IEnumerator) this.TakeHoodOff(Animation));
  }

  public IEnumerator TakeHoodOff(string Animation = "idle")
  {
    this.Spine.AnimationState.SetAnimation(0, this.AnimHoodDown, false);
    this.Spine.AnimationState.AddAnimation(0, Animation, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    this.Spine.Skeleton.SetSkin(this.wim.v_i.SkinName);
    yield return (object) new WaitForSeconds(0.5f);
  }

  public void TakeHoodOffAndGo(Vector3 Destination, System.Action Callback)
  {
    this.StartCoroutine((IEnumerator) this.TakeHoodOffAndGoRoutine(Destination, Callback));
  }

  public IEnumerator TakeHoodOffAndGoRoutine(Vector3 Destination, System.Action Callback)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    ChurchFollower churchFollower = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      churchFollower.GoTo(Destination, Callback);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) churchFollower.StartCoroutine((IEnumerator) churchFollower.TakeHoodOff());
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void ApplySermonEffect(string Anim, Action<ChurchFollower> EffectCallback)
  {
    this.StartCoroutine((IEnumerator) this.ApplySermonEffectRoutine(Anim, EffectCallback));
  }

  public IEnumerator ApplySermonEffectRoutine(string Anim, Action<ChurchFollower> EffectCallback)
  {
    ChurchFollower churchFollower = this;
    bool hoodAnimfinished = false;
    churchFollower.Spine.AnimationState.SetAnimation(0, churchFollower.AnimHoodDown, false);
    churchFollower.Spine.AnimationState.Complete += (Spine.AnimationState.TrackEntryDelegate) (trackEntry => hoodAnimfinished = true);
    while (!hoodAnimfinished)
      yield return (object) null;
    churchFollower.Spine.Skeleton.SetSkin(churchFollower.wim.v_i.SkinName);
    bool reactAnimFinished = false;
    churchFollower.Spine.AnimationState.SetAnimation(0, Anim, false);
    churchFollower.Spine.AnimationState.Complete += (Spine.AnimationState.TrackEntryDelegate) (trackEntry => reactAnimFinished = true);
    while (!reactAnimFinished)
      yield return (object) null;
    Action<ChurchFollower> action = EffectCallback;
    if (action != null)
      action(churchFollower);
    yield return (object) new WaitForSeconds(0.5f);
  }

  public void GoToDoor()
  {
    this.GoTo(ChurchFollowerManager.Instance.DoorPosition.position + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), 0.0f), new System.Action(this.DestroySelf));
  }

  public void GoTo(Vector3 Destination, System.Action Callback)
  {
    this.Callback = Callback;
    this.StartCoroutine((IEnumerator) this.GoToRoutine(Destination));
  }

  public IEnumerator GoToRoutine(Vector3 Destination)
  {
    ChurchFollower churchFollower = this;
    churchFollower.Spine.AnimationState.SetAnimation(0, churchFollower.AnimWalking, true);
    Vector3 StartPosition = churchFollower.transform.position;
    float Progress = 0.0f;
    float num = 2f + UnityEngine.Random.Range(-0.2f, 0.2f);
    float Duration = Vector3.Distance(StartPosition, Destination) / num;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      churchFollower.FacePosition(Destination);
      churchFollower.transform.position = Vector3.Lerp(StartPosition, Destination, Progress / Duration);
      yield return (object) null;
    }
    churchFollower.Spine.AnimationState.SetAnimation(0, churchFollower.AnimIdle, true);
    System.Action callback = churchFollower.Callback;
    if (callback != null)
      callback();
  }

  public void FacePosition(Vector3 PositionToFace)
  {
    this.Spine.Skeleton.ScaleX = (double) this.transform.position.x < (double) PositionToFace.x ? -1f : 1f;
  }

  public void DestroySelf() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  public void OnDisable()
  {
  }
}
