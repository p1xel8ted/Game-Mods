// Decompiled with JetBrains decompiler
// Type: Spine.Unity.Examples.spineEventListeners
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using System;
using UnityEngine.Events;

#nullable disable
namespace Spine.Unity.Examples;

[Serializable]
public class spineEventListeners
{
  public SkeletonAnimation skeletonAnimation;
  [SpineEvent("", "skeletonAnimation", true, true, false)]
  public string eventName;
  public SoundConstants.SoundEventType soundEventType;
  [EventRef]
  public string soundPath = string.Empty;
  public UnityEvent callBack;
  public EventData eventData;

  public void Start()
  {
    if ((UnityEngine.Object) this.skeletonAnimation == (UnityEngine.Object) null)
      return;
    this.skeletonAnimation.Initialize(false);
    if (!this.skeletonAnimation.valid)
      return;
    this.eventData = this.skeletonAnimation.Skeleton.Data.FindEvent(this.eventName);
    this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  public void Play()
  {
    if (this.callBack != null)
      this.callBack.Invoke();
    switch (this.soundEventType)
    {
      case SoundConstants.SoundEventType.OneShot2D:
        AudioManager.Instance.PlayOneShot(this.soundPath);
        break;
      case SoundConstants.SoundEventType.OneShotAtPosition:
        AudioManager.Instance.PlayOneShot(this.soundPath, this.skeletonAnimation.transform.position);
        break;
      case SoundConstants.SoundEventType.OneShotAttached:
        AudioManager.Instance.PlayOneShot(this.soundPath, this.skeletonAnimation.gameObject);
        break;
    }
  }

  public void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (this.eventData != e.Data)
      return;
    this.Play();
  }
}
