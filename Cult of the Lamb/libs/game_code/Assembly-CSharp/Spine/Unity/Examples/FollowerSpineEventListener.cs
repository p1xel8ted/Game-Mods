// Decompiled with JetBrains decompiler
// Type: Spine.Unity.Examples.FollowerSpineEventListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using ProBuilder2.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Spine.Unity.Examples;

public class FollowerSpineEventListener : BaseMonoBehaviour
{
  public List<followerSpineEventListeners> spineEventListeners = new List<followerSpineEventListeners>();
  public List<SoundOnAnimationData> Sounds = new List<SoundOnAnimationData>();
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public interaction_FollowerInteraction follower;
  [SerializeField]
  public bool isRecruit;
  [SerializeField]
  public Interaction_Follower followerRecruit;
  [SerializeField]
  public Interaction_FollowerSpawn followerRecruitSpawn;
  [SerializeField]
  public FollowerRecruit followerRecruitSecond;
  [SerializeField]
  public Interaction_FollowerInSpiderWeb followerRecruitSpider;
  [SerializeField]
  public Interaction_FollowerDessentingChoice followerRecruitDissenter;
  public string _pitchParameter = "follower_pitch";
  public string _vibratoParameter = "follower_vibrato";
  public string _mutatedParameter = "followerIsRotten";
  public string _snowmanParameter = "followerIsSnowlamb";
  public float _pitchValue;
  public float _vibratoValue;
  public float _mutatedValue;
  public float _snowmanValue;
  public int _followerID;
  public int AnimationTrack;
  public string currentAnimationDebug;
  public string cs;
  public EventInstance _loopInstance;
  public bool changed;

  public string CurrentAnimation
  {
    set
    {
      if (this.cs != value)
      {
        foreach (SoundOnAnimationData sound in this.Sounds)
        {
          if (sound == null || (Object) sound.SkeletonData == (Object) null || sound.SkeletonData.state == null || sound.SkeletonData.state.GetCurrent(this.AnimationTrack) == null)
          {
            Debug.Log((object) "Error: Cant get spine animation to set audio source");
          }
          else
          {
            AudioManager.Instance.StopLoop(sound.LoopedSound);
            if (sound.SkeletonData.state.GetCurrent(this.AnimationTrack).ToString() == this.cs && sound.position == SoundOnAnimationData.Position.Beginning || sound.SkeletonData.state.GetCurrent(this.AnimationTrack).ToString() == this.cs && sound.position == SoundOnAnimationData.Position.End)
            {
              string parameterName3 = this._mutatedParameter;
              float num = this._mutatedValue;
              if ((double) this._snowmanValue > 0.0)
              {
                parameterName3 = this._snowmanParameter;
                num = this._snowmanValue;
              }
              AudioManager.Instance.PlayOneShotAndSetParametersValue(sound.AudioSourcePath, this._pitchParameter, this._pitchValue, this._vibratoParameter, this._vibratoValue, parameterName3, num, this.spine.transform);
            }
            else if (sound.SkeletonData.state.GetCurrent(this.AnimationTrack).ToString() == sound.SkeletonsAnimations && sound.position == SoundOnAnimationData.Position.Loop)
            {
              sound.LoopedSound = AudioManager.Instance.CreateLoop(sound.AudioSourcePath, this.spine.gameObject);
              AudioManager.Instance.SetEventInstanceParameter(sound.LoopedSound, this._pitchParameter, this._pitchValue);
              AudioManager.Instance.SetEventInstanceParameter(sound.LoopedSound, this._vibratoParameter, this._vibratoValue);
              AudioManager.Instance.SetEventInstanceParameter(sound.LoopedSound, this._mutatedParameter, this._mutatedValue);
              AudioManager.Instance.PlayLoop(sound.LoopedSound);
            }
          }
        }
      }
      this.cs = value;
    }
  }

  public void OnDestroy() => this.DisableLoops();

  public void OnDisable() => this.DisableLoops();

  public void DisableLoops()
  {
    AudioManager.Instance.StopLoop(this._loopInstance);
    foreach (SoundOnAnimationData sound in this.Sounds)
    {
      if (sound.position == SoundOnAnimationData.Position.Loop)
        AudioManager.Instance.StopLoop(sound.LoopedSound);
    }
  }

  public void Update()
  {
    if (!((Object) this.spine != (Object) null) || this.spine.state == null || this.spine.state.GetCurrent(this.AnimationTrack) == null)
      return;
    this.CurrentAnimation = this.spine.state.GetCurrent(this.AnimationTrack).ToString();
    this.currentAnimationDebug = this.spine.state.GetCurrent(this.AnimationTrack).ToString();
  }

  public void UpdateSpine()
  {
    if (!((Object) this.spine != (Object) null))
      return;
    foreach (followerSpineEventListeners spineEventListener in this.spineEventListeners)
      spineEventListener.skeletonAnimation = this.spine;
    foreach (SoundOnAnimationData sound in this.Sounds)
      sound.SkeletonData = this.spine;
  }

  public void ReplaceEvent(string eventName, string fmodEvent)
  {
    foreach (followerSpineEventListeners spineEventListener in this.spineEventListeners)
    {
      if (spineEventListener.eventName == eventName)
        spineEventListener.soundPath = fmodEvent;
    }
  }

  public void PlayFollowerVO(string soundPath)
  {
    if (this.follower.follower.Brain.Info.ID == 99991)
    {
      AudioManager.Instance.PlayOneShotAndSetParametersValue(soundPath, this._pitchParameter, this._pitchValue, this._vibratoParameter, this._vibratoValue, "fol_heket", 1f, this.spine.transform);
    }
    else
    {
      bool flag = soundPath.Contains("talk");
      this._mutatedValue = this.follower.follower.Brain.HasTrait(FollowerTrait.TraitType.Mutated) ? 1f : 0.0f;
      string parameterName3 = this._mutatedParameter;
      float num = this._mutatedValue;
      if ((double) this._snowmanValue > 0.0)
      {
        parameterName3 = this._snowmanParameter;
        num = this._snowmanValue;
      }
      AudioManager.Instance.PlayOneShotAndSetParametersValue(soundPath, this._pitchParameter, this._pitchValue, this._vibratoParameter, this._vibratoValue, parameterName3, num, this.spine.transform, flag ? this.follower.follower.Brain.Info.ID : -1);
    }
  }

  public void PlayFollowerVOLoop(string soundPath)
  {
    this._mutatedValue = this.follower.follower.Brain.HasTrait(FollowerTrait.TraitType.Mutated) ? 1f : 0.0f;
    this._snowmanValue = this.follower.follower.Brain.Info.IsSnowman ? 1f : 0.0f;
    this._loopInstance = AudioManager.Instance.CreateLoop(soundPath, this.spine.gameObject, true);
    int num1 = (int) this._loopInstance.setParameterByName(this._pitchParameter, this._pitchValue);
    int num2 = (int) this._loopInstance.setParameterByName(this._vibratoParameter, this._vibratoValue);
    int num3 = (int) this._loopInstance.setParameterByName(this._mutatedParameter, this._mutatedValue);
    int num4 = (int) this._loopInstance.setParameterByName(this._snowmanParameter, this._snowmanValue);
  }

  public void StopFollowerVOLoop() => AudioManager.Instance.StopLoop(this._loopInstance);

  public void SetPitchAndVibrator(float pitch, float vibrato, int followerID)
  {
    foreach (followerSpineEventListeners spineEventListener in this.spineEventListeners)
    {
      spineEventListener.Start();
      spineEventListener._pitchParameter = this._pitchParameter;
      spineEventListener._vibratoParameter = this._vibratoParameter;
      spineEventListener._mutatedParameter = this._mutatedParameter;
      spineEventListener._snowmanParameter = this._snowmanParameter;
      spineEventListener._pitchValue = pitch;
      spineEventListener._vibratoValue = vibrato;
      spineEventListener._followerID = (float) followerID;
    }
  }

  public void RefreshRotValue()
  {
    float mutatedValue = this._mutatedValue;
    this._mutatedValue = this.follower.follower.Brain._directInfoAccess.Traits.Contains(FollowerTrait.TraitType.Mutated) ? 1f : 0.0f;
    if (this._mutatedValue.Approx(mutatedValue))
      return;
    foreach (followerSpineEventListeners spineEventListener in this.spineEventListeners)
      spineEventListener._mutatedValue = this._mutatedValue;
  }

  public void Start()
  {
    if (!this.isRecruit)
    {
      if ((Object) this.follower != (Object) null && (Object) this.follower.follower != (Object) null && this.follower.follower.Brain != null)
      {
        this._pitchValue = this.follower.follower.Brain._directInfoAccess.follower_pitch;
        this._vibratoValue = this.follower.follower.Brain._directInfoAccess.follower_vibrato;
        this._mutatedValue = this.follower.follower.Brain._directInfoAccess.Traits.Contains(FollowerTrait.TraitType.Mutated) ? 1f : 0.0f;
        this._snowmanValue = this.follower.follower.Brain._directInfoAccess.IsSnowman ? 1f : 0.0f;
        this._followerID = this.follower.follower.Brain._directInfoAccess.ID;
      }
      foreach (followerSpineEventListeners spineEventListener in this.spineEventListeners)
      {
        spineEventListener.Start();
        spineEventListener._pitchParameter = this._pitchParameter;
        spineEventListener._vibratoParameter = this._vibratoParameter;
        spineEventListener._mutatedParameter = this._mutatedParameter;
        spineEventListener._snowmanParameter = this._snowmanParameter;
        spineEventListener._pitchValue = this._pitchValue;
        spineEventListener._vibratoValue = this._vibratoValue;
        spineEventListener._mutatedValue = this._mutatedValue;
        spineEventListener._snowmanValue = this._snowmanValue;
        spineEventListener._followerID = (float) this._followerID;
      }
    }
    else
      this.followerRecruitAssigned();
  }

  public void OnEnable()
  {
    if (!this.isRecruit)
      return;
    this.followerRecruitAssigned();
  }

  public void followerRecruitAssigned()
  {
    this.StartCoroutine((IEnumerator) this.FollowerRecruitAssigned());
  }

  public IEnumerator FollowerRecruitAssigned()
  {
    yield return (object) new WaitForEndOfFrame();
    this.changed = false;
    if ((Object) this.followerRecruit != (Object) null)
    {
      while (this.followerRecruit.followerInfo == null)
        yield return (object) null;
      this._pitchValue = this.followerRecruit.followerInfo.follower_pitch;
      this._vibratoValue = this.followerRecruit.followerInfo.follower_vibrato;
      this._mutatedValue = this.followerRecruit.followerInfo.Traits.Contains(FollowerTrait.TraitType.Mutated) ? 1f : 0.0f;
      this._snowmanValue = this.followerRecruit.followerInfo.IsSnowman ? 1f : 0.0f;
      this._followerID = this.followerRecruit.followerInfo.ID;
      this.changed = true;
    }
    else if ((Object) this.followerRecruitSpawn != (Object) null && this.followerRecruitSpawn._followerInfo != null)
    {
      this._pitchValue = this.followerRecruitSpawn._followerInfo.follower_pitch;
      this._vibratoValue = this.followerRecruitSpawn._followerInfo.follower_vibrato;
      this._mutatedValue = this.followerRecruitSpawn._followerInfo.Traits.Contains(FollowerTrait.TraitType.Mutated) ? 1f : 0.0f;
      this._snowmanValue = this.followerRecruitSpawn._followerInfo.IsSnowman ? 1f : 0.0f;
      this._followerID = this.followerRecruitSpawn._followerInfo.ID;
      this.changed = true;
    }
    else if ((Object) this.followerRecruitSecond != (Object) null)
    {
      this._pitchValue = this.followerRecruitSecond.FollowerInteraction.follower.Brain._directInfoAccess.follower_pitch;
      this._vibratoValue = this.followerRecruitSecond.FollowerInteraction.follower.Brain._directInfoAccess.follower_vibrato;
      this._mutatedValue = this.followerRecruitSecond.FollowerInteraction.follower.Brain._directInfoAccess.Traits.Contains(FollowerTrait.TraitType.Mutated) ? 1f : 0.0f;
      this._snowmanValue = this.followerRecruitSecond.FollowerInteraction.follower.Brain._directInfoAccess.IsSnowman ? 1f : 0.0f;
      this._followerID = this.followerRecruitSecond.FollowerInteraction.follower.Brain._directInfoAccess.ID;
      this.changed = true;
    }
    else if ((Object) this.followerRecruitSpider != (Object) null)
    {
      this._pitchValue = this.followerRecruitSpider._followerInfo.follower_pitch;
      this._vibratoValue = this.followerRecruitSpider._followerInfo.follower_vibrato;
      this._mutatedValue = this.followerRecruitSpider._followerInfo.Traits.Contains(FollowerTrait.TraitType.Mutated) ? 1f : 0.0f;
      this._snowmanValue = this.followerRecruitSpider._followerInfo.IsSnowman ? 1f : 0.0f;
      this._followerID = this.followerRecruitSpider._followerInfo.ID;
      this.changed = true;
    }
    else if ((Object) this.followerRecruitDissenter != (Object) null)
    {
      this._pitchValue = this.followerRecruitDissenter.followerInfo.follower_pitch;
      this._vibratoValue = this.followerRecruitDissenter.followerInfo.follower_vibrato;
      this._mutatedValue = this.followerRecruitDissenter.followerInfo.Traits.Contains(FollowerTrait.TraitType.Mutated) ? 1f : 0.0f;
      this._snowmanValue = this.followerRecruitDissenter.followerInfo.IsSnowman ? 1f : 0.0f;
      this._followerID = this.followerRecruitDissenter.followerInfo.ID;
      this.changed = true;
    }
    if (this.changed)
    {
      foreach (followerSpineEventListeners spineEventListener in this.spineEventListeners)
      {
        spineEventListener.Start();
        spineEventListener._pitchParameter = this._pitchParameter;
        spineEventListener._vibratoParameter = this._vibratoParameter;
        spineEventListener._mutatedParameter = this._mutatedParameter;
        spineEventListener._snowmanParameter = this._snowmanParameter;
        spineEventListener._pitchValue = this._pitchValue;
        spineEventListener._vibratoValue = this._vibratoValue;
        spineEventListener._mutatedValue = this._mutatedValue;
        spineEventListener._snowmanValue = this._snowmanValue;
        spineEventListener._followerID = (float) this._followerID;
      }
    }
  }
}
