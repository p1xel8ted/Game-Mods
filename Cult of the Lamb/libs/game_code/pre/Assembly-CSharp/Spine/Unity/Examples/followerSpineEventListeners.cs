// Decompiled with JetBrains decompiler
// Type: Spine.Unity.Examples.followerSpineEventListeners
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using System;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Spine.Unity.Examples;

[Serializable]
public class followerSpineEventListeners
{
  [HideInInspector]
  public string _pitchParameter;
  [HideInInspector]
  public string _vibratoParameter;
  [HideInInspector]
  public float _pitchValue;
  [HideInInspector]
  public float _vibratoValue;
  public SkeletonAnimation skeletonAnimation;
  [SpineEvent("", "skeletonAnimation", true, true, false)]
  public string eventName;
  public SoundConstants.SoundEventType soundEventType = SoundConstants.SoundEventType.OneShotAtPosition;
  [EventRef]
  public string soundPath = string.Empty;
  public bool isVoice = true;
  public bool UseCallBack;
  public UnityEvent callBack;
  public EventData eventData;

  public void Start()
  {
    if ((UnityEngine.Object) this.skeletonAnimation == (UnityEngine.Object) null)
    {
      Debug.Log((object) ("Skeleton Animation = null, For Event: " + this.eventData.Name));
    }
    else
    {
      this.skeletonAnimation.Initialize(false);
      if (!this.skeletonAnimation.valid)
      {
        Debug.Log((object) ("Skeleton Animation not valid, For Event: " + this.eventData.Name));
      }
      else
      {
        this.eventData = this.skeletonAnimation.Skeleton.Data.FindEvent(this.eventName);
        this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
      }
    }
  }

  private void OnDisable()
  {
    this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  private void OnDestroy()
  {
    this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  public void Play()
  {
    if (this.callBack != null)
      this.callBack.Invoke();
    switch (this.soundEventType)
    {
      case SoundConstants.SoundEventType.OneShot2D:
        AudioManager.Instance.PlayOneShotAndSetParametersValue(this.soundPath, this._pitchParameter, this._pitchValue, this._vibratoParameter, this._vibratoValue, this.skeletonAnimation.transform);
        break;
      case SoundConstants.SoundEventType.OneShotAtPosition:
        AudioManager.Instance.PlayOneShotAndSetParametersValue(this.soundPath, this._pitchParameter, this._pitchValue, this._vibratoParameter, this._vibratoValue, this.skeletonAnimation.transform);
        break;
      case SoundConstants.SoundEventType.OneShotAttached:
        AudioManager.Instance.PlayOneShotAndSetParametersValue(this.soundPath, this._pitchParameter, this._pitchValue, this._vibratoParameter, this._vibratoValue, this.skeletonAnimation.transform);
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
