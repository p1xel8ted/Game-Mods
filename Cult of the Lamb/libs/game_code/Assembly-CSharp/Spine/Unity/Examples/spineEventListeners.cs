// Decompiled with JetBrains decompiler
// Type: Spine.Unity.Examples.spineEventListeners
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public bool hasSpecialBool;
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
    if (!this.hasSpecialBool)
      return;
    this.CheckSpecialBool();
  }

  public void CheckSpecialBool()
  {
    if (this.soundPath == "event:/dialogue/dun1_cult_leader_leshy/battle_cry_leshy" && GameManager.Layer2)
      this.soundPath = "event:/dialogue/dun1_cult_leader_leshy/undead_battle_cry_leshy";
    if (this.soundPath == "event:/dialogue/dun2_cult_leader_heket/battle_cry_heket" && GameManager.Layer2)
      this.soundPath = "event:/dialogue/dun2_cult_leader_heket/undead_battle_cry_heket";
    if (this.soundPath == "event:/dialogue/dun3_cult_leader_kallamar/battle_cry_kallamar" && GameManager.Layer2)
      this.soundPath = "event:/dialogue/dun3_cult_leader_kallamar/undead_battle_cry_kallamar";
    if (!(this.soundPath == "event:/dialogue/dun4_cult_leader_shamura/battle_cry_shamura") || !GameManager.Layer2)
      return;
    this.soundPath = "event:/dialogue/dun4_cult_leader_shamura/undead_battle_cry_shamura";
  }

  public void Play()
  {
    if (this.callBack != null)
      this.callBack.Invoke();
    switch (this.soundEventType)
    {
      case SoundConstants.SoundEventType.OneShot2D:
        AudioManager.Instance.PlayOneShot(this.soundPath, this.skeletonAnimation.transform.position);
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
