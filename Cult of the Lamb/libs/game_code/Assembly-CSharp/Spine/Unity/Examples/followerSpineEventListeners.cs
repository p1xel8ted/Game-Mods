// Decompiled with JetBrains decompiler
// Type: Spine.Unity.Examples.followerSpineEventListeners
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public string _mutatedParameter;
  [HideInInspector]
  public string _snowmanParameter;
  [HideInInspector]
  public float _pitchValue;
  [HideInInspector]
  public float _vibratoValue;
  [HideInInspector]
  public float _mutatedValue;
  [HideInInspector]
  public float _snowmanValue;
  [HideInInspector]
  public float _followerID;
  public SkeletonAnimation skeletonAnimation;
  [SpineEvent("", "skeletonAnimation", true, true, false)]
  public string eventName;
  public SoundConstants.SoundEventType soundEventType = SoundConstants.SoundEventType.OneShotAtPosition;
  [EventRef]
  public string soundPath = string.Empty;
  public float ChanceToPlay = 1f;
  public bool isVoice = true;
  public bool UseCallBack;
  public UnityEvent callBack;
  public EventData eventData;

  public void Start()
  {
    if ((UnityEngine.Object) this.skeletonAnimation == (UnityEngine.Object) null)
    {
      if (this.eventData == null)
        return;
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
        this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
        this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
      }
    }
  }

  public void Play()
  {
    if (this.callBack != null)
      this.callBack.Invoke();
    if ((double) UnityEngine.Random.value > (double) this.ChanceToPlay || string.IsNullOrEmpty(this.soundPath))
      return;
    string parameterName3 = this._mutatedParameter;
    float num = this._mutatedValue;
    if ((double) this._snowmanValue > 0.0)
    {
      parameterName3 = this._snowmanParameter;
      num = this._snowmanValue;
    }
    switch (this.soundEventType)
    {
      case SoundConstants.SoundEventType.OneShot2D:
        AudioManager.Instance.PlayOneShotAndSetParametersValue(this.soundPath, this._pitchParameter, this._pitchValue, this._vibratoParameter, this._vibratoValue, parameterName3, num, this.skeletonAnimation.transform, this.isVoice ? (int) this._followerID : -1);
        break;
      case SoundConstants.SoundEventType.OneShotAtPosition:
        Transform transform = (UnityEngine.Object) this.skeletonAnimation != (UnityEngine.Object) null ? this.skeletonAnimation.transform : (Transform) null;
        AudioManager.Instance.PlayOneShotAndSetParametersValue(this.soundPath, this._pitchParameter, this._pitchValue, this._vibratoParameter, this._vibratoValue, parameterName3, num, transform, this.isVoice ? (int) this._followerID : -1);
        break;
      case SoundConstants.SoundEventType.OneShotAttached:
        AudioManager.Instance.PlayOneShotAndSetParametersValue(this.soundPath, this._pitchParameter, this._pitchValue, this._vibratoParameter, this._vibratoValue, parameterName3, num, this.skeletonAnimation.transform, this.isVoice ? (int) this._followerID : -1);
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
