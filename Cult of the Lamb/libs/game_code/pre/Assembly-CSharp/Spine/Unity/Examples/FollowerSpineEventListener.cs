// Decompiled with JetBrains decompiler
// Type: Spine.Unity.Examples.FollowerSpineEventListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
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
  private SkeletonAnimation spine;
  [SerializeField]
  private interaction_FollowerInteraction follower;
  [SerializeField]
  private bool isRecruit;
  [SerializeField]
  private Interaction_Follower followerRecruit;
  [SerializeField]
  private Interaction_FollowerSpawn followerRecruitSpawn;
  [SerializeField]
  private FollowerRecruit followerRecruitSecond;
  [SerializeField]
  private Interaction_FollowerInSpiderWeb followerRecruitSpider;
  [SerializeField]
  private Interaction_FollowerDessentingChoice followerRecruitDissenter;
  private string _pitchParameter = "follower_pitch";
  private string _vibratoParameter = "follower_vibrato";
  private float _pitchValue;
  private float _vibratoValue;
  public int AnimationTrack;
  public string currentAnimationDebug;
  private string cs;
  public EventInstance _loopInstance;
  private bool changed;

  private string CurrentAnimation
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
              AudioManager.Instance.PlayOneShotAndSetParametersValue(sound.AudioSourcePath, this._pitchParameter, this._pitchValue, this._vibratoParameter, this._vibratoValue, this.spine.transform);
            else if (sound.SkeletonData.state.GetCurrent(this.AnimationTrack).ToString() == sound.SkeletonsAnimations && sound.position == SoundOnAnimationData.Position.Loop)
            {
              sound.LoopedSound = AudioManager.Instance.CreateLoop(sound.AudioSourcePath, this.spine.gameObject);
              AudioManager.Instance.SetEventInstanceParameter(sound.LoopedSound, this._pitchParameter, this._pitchValue);
              AudioManager.Instance.SetEventInstanceParameter(sound.LoopedSound, this._vibratoParameter, this._vibratoValue);
              AudioManager.Instance.PlayLoop(sound.LoopedSound);
            }
          }
        }
      }
      this.cs = value;
    }
  }

  private void OnDestroy() => this.DisableLoops();

  private void OnDisable() => this.DisableLoops();

  private void DisableLoops()
  {
    AudioManager.Instance.StopLoop(this._loopInstance);
    foreach (SoundOnAnimationData sound in this.Sounds)
    {
      if (sound.position == SoundOnAnimationData.Position.Loop)
        AudioManager.Instance.StopLoop(sound.LoopedSound);
    }
  }

  private void Update()
  {
    if (!((Object) this.spine != (Object) null) || this.spine.state.GetCurrent(this.AnimationTrack) == null)
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

  public void PlayFollowerVO(string soundPath)
  {
    Debug.Log((object) $"Play one shot + {soundPath}pitch: {(object) this._pitchValue}vibrato: {(object) this._vibratoValue}");
    AudioManager.Instance.PlayOneShotAndSetParametersValue(soundPath, this._pitchParameter, this._pitchValue, this._vibratoParameter, this._vibratoValue, this.spine.transform);
  }

  public void PlayFollowerVOLoop(string soundPath)
  {
    Debug.Log((object) $"Play loop+ {soundPath}pitch: {(object) this._pitchValue} vibrato: {(object) this._vibratoValue}");
    this._loopInstance = AudioManager.Instance.CreateLoop(soundPath, this.spine.gameObject, true);
    int num1 = (int) this._loopInstance.setParameterByName(this._pitchParameter, this._pitchValue);
    int num2 = (int) this._loopInstance.setParameterByName(this._vibratoParameter, this._vibratoValue);
  }

  public void StopFollowerVOLoop() => AudioManager.Instance.StopLoop(this._loopInstance);

  public void SetPitchAndVibrator(float pitch, float vibrato)
  {
    foreach (followerSpineEventListeners spineEventListener in this.spineEventListeners)
    {
      spineEventListener.Start();
      spineEventListener._pitchParameter = this._pitchParameter;
      spineEventListener._vibratoParameter = this._vibratoParameter;
      spineEventListener._pitchValue = pitch;
      spineEventListener._vibratoValue = vibrato;
    }
  }

  private void Start()
  {
    if (!this.isRecruit)
    {
      if ((Object) this.follower != (Object) null && (Object) this.follower.follower != (Object) null && this.follower.follower.Brain != null)
        this._pitchValue = this.follower.follower.Brain._directInfoAccess.follower_pitch;
      if ((Object) this.follower != (Object) null && (Object) this.follower.follower != (Object) null && this.follower.follower.Brain != null)
        this._vibratoValue = this.follower.follower.Brain._directInfoAccess.follower_vibrato;
      foreach (followerSpineEventListeners spineEventListener in this.spineEventListeners)
      {
        spineEventListener.Start();
        spineEventListener._pitchParameter = this._pitchParameter;
        spineEventListener._vibratoParameter = this._vibratoParameter;
        spineEventListener._pitchValue = this._pitchValue;
        spineEventListener._vibratoValue = this._vibratoValue;
      }
    }
    else
      this.followerRecruitAssigned();
  }

  private void followerRecruitAssigned()
  {
    this.StartCoroutine((IEnumerator) this.FollowerRecruitAssigned());
  }

  private IEnumerator FollowerRecruitAssigned()
  {
    yield return (object) new WaitForEndOfFrame();
    this.changed = false;
    if ((Object) this.followerRecruit != (Object) null)
    {
      while (this.followerRecruit.followerInfo == null)
        yield return (object) null;
      this._pitchValue = this.followerRecruit.followerInfo.follower_pitch;
      this._vibratoValue = this.followerRecruit.followerInfo.follower_vibrato;
      this.changed = true;
    }
    else if ((Object) this.followerRecruitSpawn != (Object) null && this.followerRecruitSpawn._followerInfo != null)
    {
      this._pitchValue = this.followerRecruitSpawn._followerInfo.follower_pitch;
      this._vibratoValue = this.followerRecruitSpawn._followerInfo.follower_vibrato;
      this.changed = true;
    }
    else if ((Object) this.followerRecruitSecond != (Object) null)
    {
      this._pitchValue = this.followerRecruitSecond.FollowerInteraction.follower.Brain._directInfoAccess.follower_pitch;
      this._vibratoValue = this.followerRecruitSecond.FollowerInteraction.follower.Brain._directInfoAccess.follower_vibrato;
      this.changed = true;
    }
    else if ((Object) this.followerRecruitSpider != (Object) null)
    {
      this._pitchValue = this.followerRecruitSpider._followerInfo.follower_pitch;
      this._vibratoValue = this.followerRecruitSpider._followerInfo.follower_vibrato;
      this.changed = true;
    }
    else if ((Object) this.followerRecruitDissenter != (Object) null)
    {
      this._pitchValue = this.followerRecruitDissenter.followerInfo.follower_pitch;
      this._vibratoValue = this.followerRecruitDissenter.followerInfo.follower_vibrato;
      this.changed = true;
    }
    if (this.changed)
    {
      foreach (followerSpineEventListeners spineEventListener in this.spineEventListeners)
      {
        spineEventListener.Start();
        spineEventListener._pitchParameter = this._pitchParameter;
        spineEventListener._vibratoParameter = this._vibratoParameter;
        spineEventListener._pitchValue = this._pitchValue;
        spineEventListener._vibratoValue = this._vibratoValue;
      }
    }
  }
}
