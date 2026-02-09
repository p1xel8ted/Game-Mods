// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.SoundGroupVariationUpdater
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

[AudioScriptOrder(-15)]
public class SoundGroupVariationUpdater : MonoBehaviour
{
  public const float TimeEarlyToScheduleNextClip = 0.1f;
  public const float FakeNegativeFloatValue = -10f;
  public Transform _objectToFollow;
  public GameObject _objectToFollowGo;
  public bool _isFollowing;
  public SoundGroupVariation _variation;
  public float _priorityLastUpdated = -10f;
  public bool _useClipAgePriority;
  public SoundGroupVariationUpdater.WaitForSoundFinishMode _waitMode;
  public AudioSource _varAudio;
  public MasterAudioGroup _parentGrp;
  public Transform _trans;
  public int _frameNum = -1;
  public bool _inited;
  public float _fadeOutStartTime = -5f;
  public bool _fadeInOutWillFadeOut;
  public bool _hasFadeInOutSetMaxVolume;
  public float _fadeInOutInFactor;
  public float _fadeInOutOutFactor;
  public int _fadeOutEarlyTotalFrames;
  public float _fadeOutEarlyFrameVolChange;
  public int _fadeOutEarlyFrameNumber;
  public float _fadeOutEarlyOrigVol;
  public float _fadeToTargetFrameVolChange;
  public int _fadeToTargetFrameNumber;
  public float _fadeToTargetOrigVol;
  public int _fadeToTargetTotalFrames;
  public float _fadeToTargetVolume;
  public bool _fadeOutStarted;
  public float _lastFrameClipTime = -1f;
  public bool _isPlayingBackward;
  public int _pitchGlideToTargetTotalFrames;
  public float _pitchGlideToTargetFramePitchChange;
  public int _pitchGlideToTargetFrameNumber;
  public float _glideToTargetPitch;
  public float _glideToTargetOrigPitch;
  public Action _glideToPitchCompletionCallback;
  public bool _hasStartedNextInChain;
  public bool _isWaitingForQueuedOcclusionRay;
  public int _framesPlayed;
  public float? _clipStartPosition;
  public float? _clipEndPosition;
  public double? _clipSchedEndTime;
  public bool _hasScheduledNextClip;
  public bool _hasScheduledEndLinkedGroups;
  public int _lastFrameClipPosition = -1;
  public int _timesLooped;
  public bool _isPaused;
  public double _pauseTime;
  public static int _maCachedFromFrame = -1;
  public static DarkTonic.MasterAudio.MasterAudio _maThisFrame;
  public static Transform _listenerThisFrame;

  public void GlidePitch(float targetPitch, float glideTime, Action completionCallback = null)
  {
    this.GrpVariation.curPitchMode = SoundGroupVariation.PitchMode.Gliding;
    float num = targetPitch - this.VarAudio.pitch;
    this._pitchGlideToTargetTotalFrames = (int) ((double) glideTime / (double) AudioUtil.FrameTime);
    this._pitchGlideToTargetFramePitchChange = num / (float) this._pitchGlideToTargetTotalFrames;
    this._pitchGlideToTargetFrameNumber = 0;
    this._glideToTargetPitch = targetPitch;
    this._glideToTargetOrigPitch = this.VarAudio.pitch;
    this._glideToPitchCompletionCallback = completionCallback;
  }

  public void FadeOverTimeToVolume(float targetVolume, float fadeTime)
  {
    this.GrpVariation.curFadeMode = SoundGroupVariation.FadeMode.GradualFade;
    float num = targetVolume - this.VarAudio.volume;
    float time = this.VarAudio.time;
    float clipEndPosition = this.ClipEndPosition;
    if (!this.VarAudio.loop && (UnityEngine.Object) this.VarAudio.clip != (UnityEngine.Object) null && (double) fadeTime + (double) time > (double) clipEndPosition)
      fadeTime = clipEndPosition - time;
    this._fadeToTargetTotalFrames = (int) ((double) fadeTime / (double) AudioUtil.FrameTime);
    this._fadeToTargetFrameVolChange = num / (float) this._fadeToTargetTotalFrames;
    this._fadeToTargetFrameNumber = 0;
    this._fadeToTargetOrigVol = this.VarAudio.volume;
    this._fadeToTargetVolume = targetVolume;
  }

  public void FadeOutEarly(float fadeTime)
  {
    this.GrpVariation.curFadeMode = SoundGroupVariation.FadeMode.FadeOutEarly;
    if (!this.VarAudio.loop && (UnityEngine.Object) this.VarAudio.clip != (UnityEngine.Object) null && (double) this.VarAudio.time + (double) fadeTime > (double) this.ClipEndPosition)
      fadeTime = this.ClipEndPosition - this.VarAudio.time;
    float num = AudioUtil.FrameTime;
    if ((double) num == 0.0)
      num = AudioUtil.FixedDeltaTime;
    this._fadeOutEarlyTotalFrames = (int) ((double) fadeTime / (double) num);
    this._fadeOutEarlyFrameVolChange = -this.VarAudio.volume / (float) this._fadeOutEarlyTotalFrames;
    this._fadeOutEarlyFrameNumber = 0;
    this._fadeOutEarlyOrigVol = this.VarAudio.volume;
  }

  public void Initialize()
  {
    if (this._inited)
      return;
    this._lastFrameClipPosition = -1;
    this._timesLooped = 0;
    this._isPaused = false;
    this._pauseTime = -1.0;
    this._clipStartPosition = new float?();
    this._clipEndPosition = new float?();
    this._clipSchedEndTime = new double?();
    this._hasScheduledNextClip = false;
    this._hasScheduledEndLinkedGroups = false;
    this._inited = true;
  }

  public void FadeInOut()
  {
    this.GrpVariation.curFadeMode = SoundGroupVariation.FadeMode.FadeInOut;
    this._fadeOutStartTime = this.ClipEndPosition - this.GrpVariation.fadeOutTime;
    if ((double) this.GrpVariation.fadeInTime > 0.0)
    {
      this.VarAudio.volume = 0.0f;
      this._fadeInOutInFactor = this.GrpVariation.fadeMaxVolume / this.GrpVariation.fadeInTime;
    }
    else
      this._fadeInOutInFactor = 0.0f;
    this._fadeInOutWillFadeOut = (double) this.GrpVariation.fadeOutTime > 0.0 && !this.VarAudio.loop;
    if (this._fadeInOutWillFadeOut)
      this._fadeInOutOutFactor = this.GrpVariation.fadeMaxVolume / (this.ClipEndPosition - this._fadeOutStartTime);
    else
      this._fadeInOutOutFactor = 0.0f;
  }

  public void FollowObject(bool follow, Transform objToFollow, bool clipAgePriority)
  {
    this._isFollowing = follow;
    if ((UnityEngine.Object) objToFollow != (UnityEngine.Object) null)
    {
      this._objectToFollow = objToFollow;
      this._objectToFollowGo = objToFollow.gameObject;
    }
    this._useClipAgePriority = clipAgePriority;
    this.UpdateCachedObjects();
    this.UpdateAudioLocationAndPriority(false);
  }

  public void WaitForSoundFinish()
  {
    if (DarkTonic.MasterAudio.MasterAudio.IsWarming)
      this.PlaySoundAndWait();
    else
      this._waitMode = SoundGroupVariationUpdater.WaitForSoundFinishMode.Play;
  }

  public void StopPitchGliding()
  {
    this.GrpVariation.curPitchMode = SoundGroupVariation.PitchMode.None;
    if (this._glideToPitchCompletionCallback != null)
    {
      this._glideToPitchCompletionCallback();
      this._glideToPitchCompletionCallback = (Action) null;
    }
    this.DisableIfFinished();
  }

  public void StopFading()
  {
    this.GrpVariation.curFadeMode = SoundGroupVariation.FadeMode.None;
    this.DisableIfFinished();
  }

  public void StopWaitingForFinish()
  {
    this._waitMode = SoundGroupVariationUpdater.WaitForSoundFinishMode.None;
    this.GrpVariation.curDetectEndMode = SoundGroupVariation.DetectEndMode.None;
    this.DisableIfFinished();
  }

  public void StopFollowing()
  {
    this._isFollowing = false;
    this._useClipAgePriority = false;
    this._objectToFollow = (Transform) null;
    this._objectToFollowGo = (GameObject) null;
    this.DisableIfFinished();
  }

  public void DisableIfFinished()
  {
    if (this._isFollowing || this.GrpVariation.curDetectEndMode == SoundGroupVariation.DetectEndMode.DetectEnd || this.GrpVariation.curFadeMode != SoundGroupVariation.FadeMode.None || this.GrpVariation.curPitchMode != SoundGroupVariation.PitchMode.None)
      return;
    this.enabled = false;
  }

  public void UpdateAudioLocationAndPriority(bool rePrioritize)
  {
    if (this._isFollowing && (UnityEngine.Object) this._objectToFollow != (UnityEngine.Object) null)
      this.Trans.position = this._objectToFollow.position;
    if (!SoundGroupVariationUpdater._maThisFrame.prioritizeOnDistance || !rePrioritize || this.ParentGroup.alwaysHighestPriority || (double) Time.realtimeSinceStartup - (double) this._priorityLastUpdated <= (double) DarkTonic.MasterAudio.MasterAudio.ReprioritizeTime)
      return;
    AudioPrioritizer.Set3DPriority(this.GrpVariation, this._useClipAgePriority);
    this._priorityLastUpdated = AudioUtil.Time;
  }

  public void ResetToNonOcclusionSetting()
  {
    AudioLowPassFilter lowPassFilter = this.GrpVariation.LowPassFilter;
    if (!((UnityEngine.Object) lowPassFilter != (UnityEngine.Object) null))
      return;
    lowPassFilter.cutoffFrequency = 22000f;
  }

  public void UpdateOcclusion()
  {
    if (!this.GrpVariation.UsesOcclusion)
    {
      DarkTonic.MasterAudio.MasterAudio.StopTrackingOcclusionForSource(this.GrpVariation.GameObj);
      this.ResetToNonOcclusionSetting();
    }
    else
    {
      if ((UnityEngine.Object) SoundGroupVariationUpdater._listenerThisFrame == (UnityEngine.Object) null || this.IsOcclusionMeasuringPaused)
        return;
      DarkTonic.MasterAudio.MasterAudio.AddToQueuedOcclusionRays(this);
      this._isWaitingForQueuedOcclusionRay = true;
    }
  }

  public void DoneWithOcclusion()
  {
    this._isWaitingForQueuedOcclusionRay = false;
    DarkTonic.MasterAudio.MasterAudio.RemoveFromOcclusionFrequencyTransitioning(this.GrpVariation);
  }

  public bool RayCastForOcclusion()
  {
    this.DoneWithOcclusion();
    Vector3 vector3_1 = this.Trans.position;
    float castOriginOffset = this.RayCastOriginOffset;
    if ((double) castOriginOffset > 0.0)
      vector3_1 = Vector3.MoveTowards(vector3_1, SoundGroupVariationUpdater._listenerThisFrame.position, castOriginOffset);
    Vector3 direction = SoundGroupVariationUpdater._listenerThisFrame.position - vector3_1;
    float magnitude = direction.magnitude;
    if ((double) magnitude > (double) this.VarAudio.maxDistance)
    {
      DarkTonic.MasterAudio.MasterAudio.AddToOcclusionOutOfRangeSources(this.GrpVariation.GameObj);
      this.ResetToNonOcclusionSetting();
      return false;
    }
    DarkTonic.MasterAudio.MasterAudio.AddToOcclusionInRangeSources(this.GrpVariation.GameObj);
    bool flag1 = SoundGroupVariationUpdater._maThisFrame.occlusionRaycastMode == DarkTonic.MasterAudio.MasterAudio.RaycastMode.Physics2D;
    if ((UnityEngine.Object) this.GrpVariation.LowPassFilter == (UnityEngine.Object) null)
      this.GrpVariation.LowPassFilter = this.GrpVariation.gameObject.AddComponent<AudioLowPassFilter>();
    bool startInColliders = Physics2D.queriesStartInColliders;
    if (flag1)
      Physics2D.queriesStartInColliders = SoundGroupVariationUpdater._maThisFrame.occlusionIncludeStartRaycast2DCollider;
    bool queriesHitTriggers;
    if (flag1)
    {
      queriesHitTriggers = Physics2D.queriesHitTriggers;
      Physics2D.queriesHitTriggers = SoundGroupVariationUpdater._maThisFrame.occlusionRaycastsHitTriggers;
    }
    else
    {
      queriesHitTriggers = Physics.queriesHitTriggers;
      Physics.queriesHitTriggers = SoundGroupVariationUpdater._maThisFrame.occlusionRaycastsHitTriggers;
    }
    Vector3 vector3_2 = Vector3.zero;
    float? nullable = new float?();
    bool flag2 = false;
    if (SoundGroupVariationUpdater._maThisFrame.occlusionUseLayerMask)
    {
      switch (SoundGroupVariationUpdater._maThisFrame.occlusionRaycastMode)
      {
        case DarkTonic.MasterAudio.MasterAudio.RaycastMode.Physics3D:
          RaycastHit hitInfo1;
          if (Physics.Raycast(vector3_1, direction, out hitInfo1, magnitude, SoundGroupVariationUpdater._maThisFrame.occlusionLayerMask.value))
          {
            flag2 = true;
            vector3_2 = hitInfo1.point;
            nullable = new float?(hitInfo1.distance);
            break;
          }
          break;
        case DarkTonic.MasterAudio.MasterAudio.RaycastMode.Physics2D:
          RaycastHit2D raycastHit2D1 = Physics2D.Raycast((Vector2) vector3_1, (Vector2) direction, magnitude, SoundGroupVariationUpdater._maThisFrame.occlusionLayerMask.value);
          if ((UnityEngine.Object) raycastHit2D1.transform != (UnityEngine.Object) null)
          {
            flag2 = true;
            vector3_2 = (Vector3) raycastHit2D1.point;
            nullable = new float?(raycastHit2D1.distance);
            break;
          }
          break;
      }
    }
    else
    {
      switch (SoundGroupVariationUpdater._maThisFrame.occlusionRaycastMode)
      {
        case DarkTonic.MasterAudio.MasterAudio.RaycastMode.Physics3D:
          RaycastHit hitInfo2;
          if (Physics.Raycast(vector3_1, direction, out hitInfo2, magnitude))
          {
            flag2 = true;
            vector3_2 = hitInfo2.point;
            nullable = new float?(hitInfo2.distance);
            break;
          }
          break;
        case DarkTonic.MasterAudio.MasterAudio.RaycastMode.Physics2D:
          RaycastHit2D raycastHit2D2 = Physics2D.Raycast((Vector2) vector3_1, (Vector2) direction, magnitude);
          if ((UnityEngine.Object) raycastHit2D2.transform != (UnityEngine.Object) null)
          {
            flag2 = true;
            vector3_2 = (Vector3) raycastHit2D2.point;
            nullable = new float?(raycastHit2D2.distance);
            break;
          }
          break;
      }
    }
    if (flag1)
    {
      Physics2D.queriesStartInColliders = startInColliders;
      Physics2D.queriesHitTriggers = queriesHitTriggers;
    }
    else
      Physics.queriesHitTriggers = queriesHitTriggers;
    if (SoundGroupVariationUpdater._maThisFrame.occlusionShowRaycasts)
    {
      Vector3 end = flag2 ? vector3_2 : SoundGroupVariationUpdater._listenerThisFrame.position;
      Color color = flag2 ? Color.red : Color.green;
      Debug.DrawLine(vector3_1, end, color, 0.1f);
    }
    if (!flag2)
    {
      DarkTonic.MasterAudio.MasterAudio.RemoveFromBlockedOcclusionSources(this.GrpVariation.GameObj);
      this.ResetToNonOcclusionSetting();
      return true;
    }
    DarkTonic.MasterAudio.MasterAudio.AddToBlockedOcclusionSources(this.GrpVariation.GameObj);
    float frequencyByDistanceRatio = AudioUtil.GetOcclusionCutoffFrequencyByDistanceRatio(nullable.Value / this.VarAudio.maxDistance, this);
    float freqChangeSeconds = SoundGroupVariationUpdater._maThisFrame.occlusionFreqChangeSeconds;
    if ((double) freqChangeSeconds <= 0.10000000149011612)
    {
      this.GrpVariation.LowPassFilter.cutoffFrequency = frequencyByDistanceRatio;
      return true;
    }
    DarkTonic.MasterAudio.MasterAudio.GradualOcclusionFreqChange(this.GrpVariation, freqChangeSeconds, frequencyByDistanceRatio);
    return true;
  }

  public void PlaySoundAndWait()
  {
    if ((UnityEngine.Object) this.VarAudio.clip == (UnityEngine.Object) null)
      return;
    double dspTime = AudioSettings.dspTime;
    if (this.GrpVariation.PlaySoundParm.TimeToSchedulePlay.HasValue)
      dspTime = this.GrpVariation.PlaySoundParm.TimeToSchedulePlay.Value;
    float num1 = 0.0f;
    if (this.GrpVariation.useIntroSilence && (double) this.GrpVariation.introSilenceMax > 0.0)
    {
      float num2 = UnityEngine.Random.Range(this.GrpVariation.introSilenceMin, this.GrpVariation.introSilenceMax);
      num1 += num2;
    }
    float num3 = num1 + this.GrpVariation.PlaySoundParm.DelaySoundTime;
    if ((double) num3 > 0.0)
      dspTime += (double) num3;
    this.VarAudio.PlayScheduled(dspTime);
    AudioUtil.ClipPlayed(this.VarAudio.clip, this.GrpVariation.GameObj);
    if (this.GrpVariation.useRandomStartTime)
    {
      this.VarAudio.time = this.ClipStartPosition;
      float num4 = AudioUtil.AdjustAudioClipDurationForPitch(this.ClipEndPosition - this.ClipStartPosition, this.VarAudio);
      this._clipSchedEndTime = new double?(dspTime + (double) num4);
      this.VarAudio.SetScheduledEndTime(this._clipSchedEndTime.Value);
    }
    this.GrpVariation.LastTimePlayed = AudioUtil.Time;
    DarkTonic.MasterAudio.MasterAudio.DuckSoundGroup(this.ParentGroup.GameObjectName, this.VarAudio);
    this._isPlayingBackward = (double) this.GrpVariation.OriginalPitch < 0.0;
    this._lastFrameClipTime = this._isPlayingBackward ? this.ClipEndPosition + 1f : -1f;
    this._waitMode = SoundGroupVariationUpdater.WaitForSoundFinishMode.WaitForEnd;
  }

  public void StopOrChain()
  {
    SoundGroupVariation.PlaySoundParams playSoundParm = this.GrpVariation.PlaySoundParm;
    bool flag = playSoundParm.IsPlaying && playSoundParm.IsChainLoop;
    if (!this.VarAudio.loop | flag)
      this.GrpVariation.Stop();
    if (!flag)
      return;
    this.StopWaitingForFinish();
    this.MaybeChain();
  }

  public void Pause()
  {
    this._isPaused = true;
    this._pauseTime = AudioSettings.dspTime;
  }

  public void Unpause()
  {
    this._isPaused = false;
    if (!this._clipSchedEndTime.HasValue)
      return;
    double num1 = AudioSettings.dspTime - this._pauseTime;
    double? clipSchedEndTime = this._clipSchedEndTime;
    double num2 = num1;
    this._clipSchedEndTime = clipSchedEndTime.HasValue ? new double?(clipSchedEndTime.GetValueOrDefault() + num2) : new double?();
    this.VarAudio.SetScheduledEndTime(this._clipSchedEndTime.Value);
  }

  public void MaybeChain()
  {
    if (this._hasStartedNextInChain)
      return;
    this._hasStartedNextInChain = true;
    SoundGroupVariation.PlaySoundParams playSoundParm = this.GrpVariation.PlaySoundParm;
    if (DarkTonic.MasterAudio.MasterAudio.RemainingClipsInGroup(this.ParentGroup.GameObjectName) == DarkTonic.MasterAudio.MasterAudio.VoicesForGroup(this.ParentGroup.GameObjectName))
      this.ParentGroup.FireLastVariationFinishedPlay();
    if (this.ParentGroup.chainLoopMode == MasterAudioGroup.ChainedLoopLoopMode.NumberOfLoops && this.ParentGroup.ChainLoopCount >= this.ParentGroup.chainLoopNumLoops)
      return;
    float delaySoundTime = playSoundParm.DelaySoundTime;
    if ((double) this.ParentGroup.chainLoopDelayMin > 0.0 || (double) this.ParentGroup.chainLoopDelayMax > 0.0)
      delaySoundTime = UnityEngine.Random.Range(this.ParentGroup.chainLoopDelayMin, this.ParentGroup.chainLoopDelayMax);
    if (playSoundParm.AttachToSource || (UnityEngine.Object) playSoundParm.SourceTrans != (UnityEngine.Object) null)
    {
      if (playSoundParm.AttachToSource)
        DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransform(playSoundParm.SoundType, playSoundParm.SourceTrans, playSoundParm.VolumePercentage, playSoundParm.Pitch, delaySoundTime, timeToSchedulePlay: this._clipSchedEndTime, isChaining: true);
      else
        DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransform(playSoundParm.SoundType, playSoundParm.SourceTrans, playSoundParm.VolumePercentage, playSoundParm.Pitch, delaySoundTime, timeToSchedulePlay: this._clipSchedEndTime, isChaining: true);
    }
    else
      DarkTonic.MasterAudio.MasterAudio.PlaySound(playSoundParm.SoundType, playSoundParm.VolumePercentage, playSoundParm.Pitch, delaySoundTime, timeToSchedulePlay: this._clipSchedEndTime, isChaining: true);
  }

  public void UpdatePitch()
  {
    switch (this.GrpVariation.curPitchMode)
    {
      case SoundGroupVariation.PitchMode.Gliding:
        if (!this.VarAudio.isPlaying)
          break;
        ++this._pitchGlideToTargetFrameNumber;
        if (this._pitchGlideToTargetFrameNumber >= this._pitchGlideToTargetTotalFrames)
        {
          this.VarAudio.pitch = this._glideToTargetPitch;
          this.StopPitchGliding();
          break;
        }
        this.VarAudio.pitch = (float) this._pitchGlideToTargetFrameNumber * this._pitchGlideToTargetFramePitchChange + this._glideToTargetOrigPitch;
        break;
    }
  }

  public void PerformFading()
  {
    switch (this.GrpVariation.curFadeMode)
    {
      case SoundGroupVariation.FadeMode.FadeInOut:
        if (!this.VarAudio.isPlaying)
          break;
        float time = this.VarAudio.time;
        if ((double) this.GrpVariation.fadeInTime > 0.0 && (double) time < (double) this.GrpVariation.fadeInTime)
        {
          this.VarAudio.volume = time * this._fadeInOutInFactor;
          break;
        }
        if ((double) time >= (double) this.GrpVariation.fadeInTime && !this._hasFadeInOutSetMaxVolume)
        {
          this.VarAudio.volume = this.GrpVariation.fadeMaxVolume;
          this._hasFadeInOutSetMaxVolume = true;
          if (this._fadeInOutWillFadeOut)
            break;
          this.StopFading();
          break;
        }
        if (!this._fadeInOutWillFadeOut || (double) time < (double) this._fadeOutStartTime)
          break;
        if (this.GrpVariation.PlaySoundParm.IsChainLoop && !this._fadeOutStarted)
        {
          this.MaybeChain();
          this._fadeOutStarted = true;
        }
        this.VarAudio.volume = (this.ClipEndPosition - time) * this._fadeInOutOutFactor;
        break;
      case SoundGroupVariation.FadeMode.FadeOutEarly:
        if (!this.VarAudio.isPlaying)
          break;
        ++this._fadeOutEarlyFrameNumber;
        this.VarAudio.volume = (float) this._fadeOutEarlyFrameNumber * this._fadeOutEarlyFrameVolChange + this._fadeOutEarlyOrigVol;
        if (this._fadeOutEarlyFrameNumber < this._fadeOutEarlyTotalFrames)
          break;
        this.GrpVariation.curFadeMode = SoundGroupVariation.FadeMode.None;
        this.GrpVariation.Stop();
        break;
      case SoundGroupVariation.FadeMode.GradualFade:
        if (!this.VarAudio.isPlaying)
          break;
        ++this._fadeToTargetFrameNumber;
        if (this._fadeToTargetFrameNumber >= this._fadeToTargetTotalFrames)
        {
          this.VarAudio.volume = this._fadeToTargetVolume;
          this.StopFading();
          break;
        }
        this.VarAudio.volume = (float) this._fadeToTargetFrameNumber * this._fadeToTargetFrameVolChange + this._fadeToTargetOrigVol;
        break;
    }
  }

  public void OnEnable()
  {
    this._inited = false;
    this._fadeInOutWillFadeOut = false;
    this._hasFadeInOutSetMaxVolume = false;
    this._fadeOutStarted = false;
    this._hasStartedNextInChain = false;
    this._framesPlayed = 0;
    this._clipStartPosition = new float?();
    this._clipEndPosition = new float?();
    this.DoneWithOcclusion();
    DarkTonic.MasterAudio.MasterAudio.RegisterUpdaterForUpdates(this);
  }

  public void OnDisable()
  {
    if (DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown)
      return;
    this._framesPlayed = 0;
    this.DoneWithOcclusion();
    DarkTonic.MasterAudio.MasterAudio.UnregisterUpdaterForUpdates(this);
  }

  public void UpdateCachedObjects()
  {
    this._frameNum = AudioUtil.FrameCount;
    if (SoundGroupVariationUpdater._maCachedFromFrame >= this._frameNum)
      return;
    SoundGroupVariationUpdater._maCachedFromFrame = this._frameNum;
    SoundGroupVariationUpdater._maThisFrame = DarkTonic.MasterAudio.MasterAudio.Instance;
    SoundGroupVariationUpdater._listenerThisFrame = DarkTonic.MasterAudio.MasterAudio.ListenerTrans;
  }

  public void ManualUpdate()
  {
    this.UpdateCachedObjects();
    ++this._framesPlayed;
    if (this.VarAudio.loop)
    {
      if (this.VarAudio.timeSamples < this._lastFrameClipPosition)
      {
        ++this._timesLooped;
        if (this.VarAudio.loop && this.GrpVariation.useCustomLooping && this._timesLooped >= this.GrpVariation.MaxLoops)
          this.GrpVariation.Stop();
        else
          this.GrpVariation.SoundLoopStarted(this._timesLooped);
      }
      this._lastFrameClipPosition = this.VarAudio.timeSamples;
    }
    if (this._isFollowing && this.ParentGroup.targetDespawnedBehavior != MasterAudioGroup.TargetDespawnedBehavior.None && ((UnityEngine.Object) this._objectToFollowGo == (UnityEngine.Object) null || !DTMonoHelper.IsActive(this._objectToFollowGo)))
    {
      switch (this.ParentGroup.targetDespawnedBehavior)
      {
        case MasterAudioGroup.TargetDespawnedBehavior.Stop:
          this.GrpVariation.Stop();
          break;
        case MasterAudioGroup.TargetDespawnedBehavior.FadeOut:
          this.GrpVariation.FadeOutNow(this.ParentGroup.despawnFadeTime);
          break;
      }
      this.StopFollowing();
    }
    this.PerformFading();
    this.UpdateAudioLocationAndPriority(true);
    this.UpdateOcclusion();
    this.UpdatePitch();
    switch (this._waitMode)
    {
      case SoundGroupVariationUpdater.WaitForSoundFinishMode.Play:
        this.PlaySoundAndWait();
        break;
      case SoundGroupVariationUpdater.WaitForSoundFinishMode.WaitForEnd:
        if (this._isPaused)
          break;
        if (this._clipSchedEndTime.HasValue && AudioSettings.dspTime + 0.10000000149011612 >= this._clipSchedEndTime.Value)
        {
          if (this.GrpVariation.PlaySoundParm.IsChainLoop && !this._hasScheduledNextClip)
          {
            this.MaybeChain();
            this._hasScheduledNextClip = true;
          }
          if (this.HasEndLinkedGroups && !this._hasScheduledEndLinkedGroups)
          {
            this.GrpVariation.PlayEndLinkedGroups(new double?(this._clipSchedEndTime.Value));
            this._hasScheduledEndLinkedGroups = true;
          }
        }
        bool flag = false;
        if (this._isPlayingBackward)
        {
          if ((double) this.VarAudio.time > (double) this._lastFrameClipTime)
            flag = true;
        }
        else if ((double) this.VarAudio.time < (double) this._lastFrameClipTime)
          flag = true;
        this._lastFrameClipTime = this.VarAudio.time;
        if (!flag)
          break;
        this._waitMode = SoundGroupVariationUpdater.WaitForSoundFinishMode.StopOrRepeat;
        break;
      case SoundGroupVariationUpdater.WaitForSoundFinishMode.StopOrRepeat:
        this.StopOrChain();
        break;
    }
  }

  public float ClipStartPosition
  {
    get
    {
      if (this._clipStartPosition.HasValue)
        return this._clipStartPosition.Value;
      this._clipStartPosition = !this.GrpVariation.useRandomStartTime ? new float?(0.0f) : new float?(UnityEngine.Random.Range(this.GrpVariation.randomStartMinPercent, this.GrpVariation.randomStartMaxPercent) * 0.01f * this.VarAudio.clip.length);
      return this._clipStartPosition.Value;
    }
  }

  public float ClipEndPosition
  {
    get
    {
      if (this._clipEndPosition.HasValue)
        return this._clipEndPosition.Value;
      this._clipEndPosition = !this.GrpVariation.useRandomStartTime ? new float?(this.VarAudio.clip.length) : new float?(this.GrpVariation.randomEndPercent * 0.01f * this.VarAudio.clip.length);
      return this._clipEndPosition.Value;
    }
  }

  public int FramesPlayed => this._framesPlayed;

  public DarkTonic.MasterAudio.MasterAudio MAThisFrame => SoundGroupVariationUpdater._maThisFrame;

  public float MaxOcclusionFreq
  {
    get
    {
      return this.GrpVariation.UsesOcclusion && this.ParentGroup.willOcclusionOverrideFrequencies ? this.ParentGroup.occlusionMaxCutoffFreq : SoundGroupVariationUpdater._maThisFrame.occlusionMaxCutoffFreq;
    }
  }

  public float MinOcclusionFreq
  {
    get
    {
      return this.GrpVariation.UsesOcclusion && this.ParentGroup.willOcclusionOverrideFrequencies ? this.ParentGroup.occlusionMinCutoffFreq : SoundGroupVariationUpdater._maThisFrame.occlusionMinCutoffFreq;
    }
  }

  public Transform Trans
  {
    get
    {
      if ((UnityEngine.Object) this._trans != (UnityEngine.Object) null)
        return this._trans;
      this._trans = this.GrpVariation.Trans;
      return this._trans;
    }
  }

  public AudioSource VarAudio
  {
    get
    {
      if ((UnityEngine.Object) this._varAudio != (UnityEngine.Object) null)
        return this._varAudio;
      this._varAudio = this.GrpVariation.VarAudio;
      return this._varAudio;
    }
  }

  public MasterAudioGroup ParentGroup
  {
    get
    {
      if ((UnityEngine.Object) this._parentGrp != (UnityEngine.Object) null)
        return this._parentGrp;
      this._parentGrp = this.GrpVariation.ParentGroup;
      return this._parentGrp;
    }
  }

  public SoundGroupVariation GrpVariation
  {
    get
    {
      if ((UnityEngine.Object) this._variation != (UnityEngine.Object) null)
        return this._variation;
      this._variation = this.GetComponent<SoundGroupVariation>();
      return this._variation;
    }
  }

  public float RayCastOriginOffset
  {
    get
    {
      return this.GrpVariation.UsesOcclusion && this.ParentGroup.willOcclusionOverrideRaycastOffset ? this.ParentGroup.occlusionRayCastOffset : SoundGroupVariationUpdater._maThisFrame.occlusionRayCastOffset;
    }
  }

  public bool IsOcclusionMeasuringPaused
  {
    get
    {
      return this._isWaitingForQueuedOcclusionRay || DarkTonic.MasterAudio.MasterAudio.IsOcclusionFreqencyTransitioning(this.GrpVariation);
    }
  }

  public bool HasEndLinkedGroups => this.GrpVariation.ParentGroup.endLinkedGroups.Count > 0;

  public enum WaitForSoundFinishMode
  {
    None,
    Play,
    WaitForEnd,
    StopOrRepeat,
  }
}
