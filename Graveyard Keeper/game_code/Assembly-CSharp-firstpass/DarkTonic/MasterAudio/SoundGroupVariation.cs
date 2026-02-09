// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.SoundGroupVariation
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

#nullable disable
namespace DarkTonic.MasterAudio;

[RequireComponent(typeof (SoundGroupVariationUpdater))]
[AudioScriptOrder(-40)]
public class SoundGroupVariation : MonoBehaviour
{
  public int weight = 1;
  [Range(0.0f, 1f)]
  public int probabilityToPlay = 100;
  public bool useLocalization;
  public bool useRandomPitch;
  public SoundGroupVariation.RandomPitchMode randomPitchMode;
  public float randomPitchMin;
  public float randomPitchMax;
  public bool useRandomVolume;
  public SoundGroupVariation.RandomVolumeMode randomVolumeMode;
  public float randomVolumeMin;
  public float randomVolumeMax;
  public DarkTonic.MasterAudio.MasterAudio.AudioLocation audLocation;
  public string resourceFileName;
  public string internetFileUrl;
  public DarkTonic.MasterAudio.MasterAudio.InternetFileLoadStatus internetFileLoadStatus;
  public float original_pitch;
  public float original_volume;
  public bool isExpanded = true;
  public bool isChecked = true;
  public bool useFades;
  public float fadeInTime;
  public float fadeOutTime;
  public bool useCustomLooping;
  public int minCustomLoops = 1;
  public int maxCustomLoops = 5;
  public bool useRandomStartTime;
  public float randomStartMinPercent;
  public float randomStartMaxPercent = 100f;
  public float randomEndPercent = 100f;
  public bool useIntroSilence;
  public float introSilenceMin;
  public float introSilenceMax;
  public float fadeMaxVolume;
  public SoundGroupVariation.FadeMode curFadeMode;
  public SoundGroupVariation.PitchMode curPitchMode;
  public SoundGroupVariation.DetectEndMode curDetectEndMode;
  public int frames;
  public AudioSource _audioSource;
  public SoundGroupVariation.PlaySoundParams _playSndParam = new SoundGroupVariation.PlaySoundParams(string.Empty, 1f, 1f, new float?(1f), (Transform) null, false, 0.0f, new double?(), false, false);
  public AudioDistortionFilter _distFilter;
  public AudioEchoFilter _echoFilter;
  public AudioHighPassFilter _hpFilter;
  public AudioLowPassFilter _lpFilter;
  public AudioReverbFilter _reverbFilter;
  public AudioChorusFilter _chorusFilter;
  public float _maxVol = 1f;
  public int _instanceId = -1;
  public bool? _audioLoops;
  public int _maxLoops;
  public SoundGroupVariationUpdater _varUpdater;
  public int _previousSoundFinishedFrame = -1;
  public string _soundGroupName;
  public Transform _trans;
  public GameObject _go;
  public Transform _objectToFollow;
  public Transform _objectToTriggerFrom;
  public MasterAudioGroup _parentGroupScript;
  public bool _attachToSource;
  public string _resFileName = string.Empty;
  public bool _hasStartedEndLinkedGroups;
  [CompilerGenerated]
  public float \u003CLastTimePlayed\u003Ek__BackingField;

  public event SoundGroupVariation.SoundFinishedEventHandler SoundFinished;

  public event SoundGroupVariation.SoundLoopedEventHandler SoundLooped;

  public void Awake()
  {
    this.original_pitch = this.VarAudio.pitch;
    this.original_volume = this.VarAudio.volume;
    this._audioLoops = new bool?(this.VarAudio.loop);
    AudioClip clip = this.VarAudio.clip;
    GameObject gameObj = this.GameObj;
    if (!((UnityEngine.Object) clip != (UnityEngine.Object) null))
    {
      int num = (UnityEngine.Object) gameObj != (UnityEngine.Object) null ? 1 : 0;
    }
    if (!this.VarAudio.playOnAwake)
      return;
    Debug.LogWarning((object) $"The 'Play on Awake' checkbox in the Audio Source for Sound Group '{this.ParentGroup.name}', Variation '{this.name}' is checked. This is not used in Master Audio and can lead to buggy behavior. Make sure to uncheck it before hitting Play next time. To play ambient sounds, use an EventSounds component and activate the Start event to play a Sound Group of your choice.");
  }

  public void Start()
  {
    if ((UnityEngine.Object) this.ParentGroup == (UnityEngine.Object) null)
    {
      Debug.LogError((object) $"Sound Variation '{this.name}' has no parent!");
    }
    else
    {
      this.GameObj.layer = DarkTonic.MasterAudio.MasterAudio.Instance.gameObject.layer;
      if (this.audLocation == DarkTonic.MasterAudio.MasterAudio.AudioLocation.FileOnInternet && this.internetFileLoadStatus == DarkTonic.MasterAudio.MasterAudio.InternetFileLoadStatus.Loading)
        this.LoadInternetFile();
      this.SetMixerGroup();
      this.SetSpatialBlend();
      this.SetPriority();
      this.SetOcclusion();
      SpatializerHelper.TurnOnSpatializerIfEnabled(this.VarAudio);
    }
  }

  public void SetMixerGroup()
  {
    GroupBus busForGroup = this.ParentGroup.BusForGroup;
    if (busForGroup != null)
      this.VarAudio.outputAudioMixerGroup = busForGroup.mixerChannel;
    else
      this.VarAudio.outputAudioMixerGroup = (AudioMixerGroup) null;
  }

  public void SetSpatialBlend()
  {
    float spatialBlendForGroup = this.ParentGroup.SpatialBlendForGroup;
    if ((double) spatialBlendForGroup != -99.0)
      this.VarAudio.spatialBlend = spatialBlendForGroup;
    GroupBus busForGroup = this.ParentGroup.BusForGroup;
    if (busForGroup == null || DarkTonic.MasterAudio.MasterAudio.Instance.mixerSpatialBlendType == DarkTonic.MasterAudio.MasterAudio.AllMixerSpatialBlendType.ForceAllTo2D || !busForGroup.forceTo2D)
      return;
    this.VarAudio.spatialBlend = 0.0f;
  }

  public void LoadInternetFile()
  {
    this.StartCoroutine(AudioResourceOptimizer.PopulateSourceWithInternetFile(this.internetFileUrl, this, new Action(this.InternetFileLoaded), new Action(this.InternetFileFailedToLoad)));
  }

  public void SetOcclusion()
  {
    this.VariationUpdater.UpdateCachedObjects();
    if (!this.UsesOcclusion)
      return;
    if ((UnityEngine.Object) this.LowPassFilter == (UnityEngine.Object) null)
    {
      this._lpFilter = this.GetComponent<AudioLowPassFilter>();
      if ((UnityEngine.Object) this._lpFilter == (UnityEngine.Object) null)
        this._lpFilter = this.gameObject.AddComponent<AudioLowPassFilter>();
    }
    else
      this._lpFilter = this.GetComponent<AudioLowPassFilter>();
    this.LowPassFilter.cutoffFrequency = AudioUtil.MinCutoffFreq(this.VariationUpdater);
  }

  public void SetPriority()
  {
    if (!DarkTonic.MasterAudio.MasterAudio.Instance.prioritizeOnDistance)
      return;
    if (this.ParentGroup.alwaysHighestPriority)
      AudioPrioritizer.Set2DSoundPriority(this.VarAudio);
    else
      AudioPrioritizer.SetSoundGroupInitialPriority(this.VarAudio);
  }

  public void DisableUpdater()
  {
    if ((UnityEngine.Object) this.VariationUpdater == (UnityEngine.Object) null)
      return;
    this.VariationUpdater.enabled = false;
  }

  public void OnDestroy() => this.StopSoundEarly();

  public void OnDisable() => this.StopSoundEarly();

  public void StopSoundEarly()
  {
    if (DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown)
      return;
    this.Stop();
  }

  public void OnDrawGizmos()
  {
    if (!DarkTonic.MasterAudio.MasterAudio.Instance.showGizmos || !this.IsPlaying)
      return;
    Gizmos.DrawIcon(this.transform.position, "MasterAudio/MasterAudio Icon.png", true);
  }

  public void Play(
    float? pitch,
    float maxVolume,
    string gameObjectName,
    float volPercent,
    float targetVol,
    float? targetPitch,
    Transform sourceTrans,
    bool attach,
    float delayTime,
    double? timeToSchedulePlay,
    bool isChaining,
    bool isSingleSubscribedPlay)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.IsWarming && this.audLocation == DarkTonic.MasterAudio.MasterAudio.AudioLocation.FileOnInternet)
    {
      switch (this.internetFileLoadStatus)
      {
        case DarkTonic.MasterAudio.MasterAudio.InternetFileLoadStatus.Loading:
          if (!this.ParentGroup.LoggingEnabledForGroup)
            return;
          DarkTonic.MasterAudio.MasterAudio.LogWarning($"Cannot play Variation '{this.name}' because its Internet file has not been downloaded yet.");
          return;
        case DarkTonic.MasterAudio.MasterAudio.InternetFileLoadStatus.Failed:
          if (!this.ParentGroup.LoggingEnabledForGroup)
            return;
          DarkTonic.MasterAudio.MasterAudio.LogWarning($"Cannot play Variation '{this.name}' because its Internet file failed downloading.");
          return;
      }
    }
    this.MaybeCleanupFinishedDelegate();
    this._hasStartedEndLinkedGroups = false;
    this.SetPlaySoundParams(gameObjectName, volPercent, targetVol, targetPitch, sourceTrans, attach, delayTime, timeToSchedulePlay, isChaining, isSingleSubscribedPlay);
    this.SetPriority();
    if (DarkTonic.MasterAudio.MasterAudio.HasAsyncResourceLoaderFeature() && this.ShouldLoadAsync)
      this.StopAllCoroutines();
    if (pitch.HasValue)
      this.VarAudio.pitch = pitch.Value;
    else if (this.useRandomPitch)
    {
      float num = UnityEngine.Random.Range(this.randomPitchMin, this.randomPitchMax);
      if (this.randomPitchMode == SoundGroupVariation.RandomPitchMode.AddToClipPitch)
        num += this.OriginalPitch;
      this.VarAudio.pitch = num;
    }
    else
      this.VarAudio.pitch = this.OriginalPitch;
    this.SetSpatialBlend();
    this.curFadeMode = SoundGroupVariation.FadeMode.None;
    this.curPitchMode = SoundGroupVariation.PitchMode.None;
    this.curDetectEndMode = SoundGroupVariation.DetectEndMode.DetectEnd;
    this._maxVol = maxVolume;
    this._maxLoops = this.maxCustomLoops != this.minCustomLoops ? UnityEngine.Random.Range(this.minCustomLoops, this.maxCustomLoops + 1) : this.minCustomLoops;
    switch (this.audLocation)
    {
      case DarkTonic.MasterAudio.MasterAudio.AudioLocation.Clip:
        this.FinishSetupToPlay();
        break;
      case DarkTonic.MasterAudio.MasterAudio.AudioLocation.ResourceFile:
        if (DarkTonic.MasterAudio.MasterAudio.HasAsyncResourceLoaderFeature() && this.ShouldLoadAsync)
        {
          this.StartCoroutine(AudioResourceOptimizer.PopulateSourcesWithResourceClipAsync(this.ResFileName, this, new Action(this.FinishSetupToPlay), new Action(this.ResourceFailedToLoad)));
          break;
        }
        if (!AudioResourceOptimizer.PopulateSourcesWithResourceClip(this.ResFileName, this))
          break;
        this.FinishSetupToPlay();
        break;
      case DarkTonic.MasterAudio.MasterAudio.AudioLocation.FileOnInternet:
        this.FinishSetupToPlay();
        break;
    }
  }

  public void SetPlaySoundParams(
    string gameObjectName,
    float volPercent,
    float targetVol,
    float? targetPitch,
    Transform sourceTrans,
    bool attach,
    float delayTime,
    double? timeToSchedulePlay,
    bool isChaining,
    bool isSingleSubscribedPlay)
  {
    this._playSndParam.SoundType = gameObjectName;
    this._playSndParam.VolumePercentage = volPercent;
    this._playSndParam.GroupCalcVolume = targetVol;
    this._playSndParam.Pitch = targetPitch;
    this._playSndParam.SourceTrans = sourceTrans;
    this._playSndParam.AttachToSource = attach;
    this._playSndParam.DelaySoundTime = delayTime;
    this._playSndParam.TimeToSchedulePlay = timeToSchedulePlay;
    this._playSndParam.IsChainLoop = isChaining || this.ParentGroup.curVariationMode == MasterAudioGroup.VariationMode.LoopedChain;
    this._playSndParam.IsSingleSubscribedPlay = isSingleSubscribedPlay;
    this._playSndParam.IsPlaying = true;
  }

  public void InternetFileFailedToLoad()
  {
    this.internetFileLoadStatus = DarkTonic.MasterAudio.MasterAudio.InternetFileLoadStatus.Failed;
  }

  public void MaybeCleanupFinishedDelegate()
  {
    if (!this.ParentGroup.willCleanUpDelegatesAfterStop)
      return;
    this.ClearSubscribers();
  }

  public void InternetFileLoaded()
  {
    if (this.ParentGroup.LoggingEnabledForGroup)
      DarkTonic.MasterAudio.MasterAudio.LogWarning($"Internet file: '{this.internetFileUrl}' loaded successfully.");
    this.internetFileLoadStatus = DarkTonic.MasterAudio.MasterAudio.InternetFileLoadStatus.Loaded;
  }

  public void ResourceFailedToLoad() => this.Stop();

  public void FinishSetupToPlay()
  {
    if ((this.VarAudio.isPlaying || (double) this.VarAudio.time <= 0.0) && this.useFades && ((double) this.fadeInTime > 0.0 || (double) this.fadeOutTime > 0.0))
    {
      this.fadeMaxVolume = this._maxVol;
      if ((double) this.fadeInTime > 0.0)
        this.VarAudio.volume = 0.0f;
      if ((UnityEngine.Object) this.VariationUpdater != (UnityEngine.Object) null)
      {
        this.EnableUpdater(false);
        this.VariationUpdater.FadeInOut();
      }
    }
    this.VarAudio.loop = this.AudioLoops;
    if (this._playSndParam.IsPlaying && (this._playSndParam.IsChainLoop || this._playSndParam.IsSingleSubscribedPlay || this.useRandomStartTime))
      this.VarAudio.loop = false;
    if (!this._playSndParam.IsPlaying)
      return;
    this.ParentGroup.AddActiveAudioSourceId(this.InstanceId);
    this.EnableUpdater();
    this._attachToSource = false;
    bool clipAgePriority = DarkTonic.MasterAudio.MasterAudio.Instance.prioritizeOnDistance && (DarkTonic.MasterAudio.MasterAudio.Instance.useClipAgePriority || this.ParentGroup.useClipAgePriority);
    if (!this._playSndParam.AttachToSource && !clipAgePriority)
      return;
    this._attachToSource = this._playSndParam.AttachToSource;
    if (!((UnityEngine.Object) this.VariationUpdater != (UnityEngine.Object) null))
      return;
    this.VariationUpdater.FollowObject(this._attachToSource, this.ObjectToFollow, clipAgePriority);
  }

  public void JumpToTime(float timeToJumpTo)
  {
    if (!this._playSndParam.IsPlaying)
      return;
    this.VarAudio.time = timeToJumpTo;
  }

  public void GlideByPitch(float pitchAddition, float glideTime, Action completionCallback = null)
  {
    if ((double) pitchAddition == 0.0)
    {
      if (completionCallback == null)
        return;
      completionCallback();
    }
    else
    {
      float targetPitch = this.VarAudio.pitch + pitchAddition;
      if ((double) targetPitch < -3.0)
        targetPitch = -3f;
      if ((double) targetPitch > 3.0)
        targetPitch = 3f;
      if (!this.VarAudio.clip.IsClipReadyToPlay())
      {
        if (!this.ParentGroup.LoggingEnabledForGroup)
          return;
        DarkTonic.MasterAudio.MasterAudio.LogWarning($"Cannot GlideToPitch Variation '{this.name}' because it is still loading.");
      }
      else if ((double) glideTime <= 0.10000000149011612)
      {
        if ((double) this.VarAudio.pitch != (double) targetPitch)
          this.VarAudio.pitch = targetPitch;
        if (completionCallback == null)
          return;
        completionCallback();
      }
      else
      {
        if (!((UnityEngine.Object) this.VariationUpdater != (UnityEngine.Object) null))
          return;
        this.VariationUpdater.GlidePitch(targetPitch, glideTime, completionCallback);
      }
    }
  }

  public void AdjustVolume(float volumePercentage)
  {
    if (!this._playSndParam.IsPlaying)
      return;
    this.VarAudio.volume = this._playSndParam.GroupCalcVolume * volumePercentage;
    this._playSndParam.VolumePercentage = volumePercentage;
    List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> variationsOfGroup = DarkTonic.MasterAudio.MasterAudio.GetAllVariationsOfGroup(this.ParentGroup.name);
    for (int index = 0; index < variationsOfGroup.Count; ++index)
    {
      DarkTonic.MasterAudio.MasterAudio.AudioInfo audioInfo = variationsOfGroup[index];
      if (!((UnityEngine.Object) audioInfo.Variation != (UnityEngine.Object) this))
      {
        audioInfo.LastPercentageVolume = volumePercentage;
        break;
      }
    }
  }

  public void Pause()
  {
    if (!DarkTonic.MasterAudio.MasterAudio.Instance.resourceClipsPauseDoNotUnload)
    {
      switch (this.audLocation)
      {
        case DarkTonic.MasterAudio.MasterAudio.AudioLocation.Clip:
          if (!AudioUtil.AudioClipWillPreload(this.VarAudio.clip))
          {
            this.Stop();
            return;
          }
          break;
        case DarkTonic.MasterAudio.MasterAudio.AudioLocation.ResourceFile:
          this.Stop();
          return;
      }
    }
    this.VarAudio.Pause();
    if (this.VariationUpdater.enabled)
      this.VariationUpdater.Pause();
    this.curFadeMode = SoundGroupVariation.FadeMode.None;
    this.curPitchMode = SoundGroupVariation.PitchMode.None;
  }

  public void DoNextChain(float volumePercentage, float? pitch, Transform transActor, bool attach)
  {
    this.EnableUpdater(false);
    this.SetPlaySoundParams(this.ParentGroup.GameObjectName, volumePercentage, volumePercentage, pitch, transActor, attach, 0.0f, new double?(), true, false);
    this.VariationUpdater.MaybeChain();
    this.VariationUpdater.StopWaitingForFinish();
  }

  public void PlayEndLinkedGroups(double? timeToPlayClip = null)
  {
    if (DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown || DarkTonic.MasterAudio.MasterAudio.IsWarming || this.ParentGroup.endLinkedGroups.Count == 0 || this._hasStartedEndLinkedGroups)
      return;
    this._hasStartedEndLinkedGroups = true;
    if ((UnityEngine.Object) this.VariationUpdater == (UnityEngine.Object) null || this.VariationUpdater.FramesPlayed == 0)
      return;
    switch (this.ParentGroup.linkedStopGroupSelectionType)
    {
      case DarkTonic.MasterAudio.MasterAudio.LinkedGroupSelectionType.All:
        for (int index = 0; index < this.ParentGroup.endLinkedGroups.Count; ++index)
          this.PlayEndLinkedGroup(this.ParentGroup.endLinkedGroups[index], timeToPlayClip);
        break;
      case DarkTonic.MasterAudio.MasterAudio.LinkedGroupSelectionType.OneAtRandom:
        this.PlayEndLinkedGroup(this.ParentGroup.endLinkedGroups[UnityEngine.Random.Range(0, this.ParentGroup.endLinkedGroups.Count)], timeToPlayClip);
        break;
    }
  }

  public void EnableUpdater(bool waitForSoundFinish = true)
  {
    if (!((UnityEngine.Object) this.VariationUpdater != (UnityEngine.Object) null))
      return;
    this.VariationUpdater.enabled = true;
    this.VariationUpdater.Initialize();
    if (!waitForSoundFinish)
      return;
    this.VariationUpdater.WaitForSoundFinish();
  }

  public void MaybeUnloadClip()
  {
    if (this.audLocation == DarkTonic.MasterAudio.MasterAudio.AudioLocation.ResourceFile)
      AudioResourceOptimizer.UnloadClipIfUnused(this._resFileName);
    AudioUtil.UnloadNonPreloadedAudioData(this.VarAudio.clip, this.GameObj);
  }

  public void PlayEndLinkedGroup(string sType, double? timeToPlayClip = null)
  {
    if (this._playSndParam.AttachToSource && (UnityEngine.Object) this._playSndParam.SourceTrans != (UnityEngine.Object) null)
      DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransformAndForget(sType, this._playSndParam.SourceTrans, this._playSndParam.VolumePercentage, this._playSndParam.Pitch, timeToSchedulePlay: timeToPlayClip);
    else if ((UnityEngine.Object) this._playSndParam.SourceTrans != (UnityEngine.Object) null)
      DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransformAndForget(sType, this._playSndParam.SourceTrans, this._playSndParam.VolumePercentage, this._playSndParam.Pitch, timeToSchedulePlay: timeToPlayClip);
    else
      DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtVector3AndForget(sType, this.Trans.position, this._playSndParam.VolumePercentage, this._playSndParam.Pitch, timeToSchedulePlay: timeToPlayClip);
  }

  public void Stop(bool stopEndDetection = false, bool skipLinked = false)
  {
    bool flag = false;
    if (stopEndDetection && (UnityEngine.Object) this.VariationUpdater != (UnityEngine.Object) null)
    {
      this.VariationUpdater.StopWaitingForFinish();
      flag = true;
    }
    if (!skipLinked)
      this.PlayEndLinkedGroups();
    this._objectToFollow = (Transform) null;
    this._objectToTriggerFrom = (Transform) null;
    this.VarAudio.pitch = this.OriginalPitch;
    this.ParentGroup.RemoveActiveAudioSourceId(this.InstanceId);
    DarkTonic.MasterAudio.MasterAudio.StopTrackingOcclusionForSource(this.GameObj);
    this.VarAudio.Stop();
    this.VarAudio.time = 0.0f;
    if ((UnityEngine.Object) this.VariationUpdater != (UnityEngine.Object) null)
    {
      this.VariationUpdater.StopFollowing();
      this.VariationUpdater.StopFading();
      this.VariationUpdater.StopPitchGliding();
    }
    if (!flag && (UnityEngine.Object) this.VariationUpdater != (UnityEngine.Object) null)
      this.VariationUpdater.StopWaitingForFinish();
    this._playSndParam.IsPlaying = false;
    if (this.SoundFinished != null)
    {
      int num = this._previousSoundFinishedFrame == AudioUtil.FrameCount ? 1 : 0;
      this._previousSoundFinishedFrame = AudioUtil.FrameCount;
      if (num == 0)
        this.SoundFinished();
      this.MaybeCleanupFinishedDelegate();
    }
    this.Trans.localPosition = Vector3.zero;
    this.MaybeUnloadClip();
  }

  public void FadeToVolume(float newVolume, float fadeTime)
  {
    if ((double) newVolume < 0.0 || (double) newVolume > 1.0)
      Debug.LogError((object) $"Illegal volume passed to FadeToVolume: '{newVolume.ToString()}'. Legal volumes are between 0 and 1.");
    else if (!this.VarAudio.clip.IsClipReadyToPlay())
    {
      if (!this.ParentGroup.LoggingEnabledForGroup)
        return;
      DarkTonic.MasterAudio.MasterAudio.LogWarning($"Cannot Fade Variation '{this.name}' because it is still loading.");
    }
    else if ((double) fadeTime <= 0.10000000149011612)
    {
      this.VarAudio.volume = newVolume;
      if ((double) this.VarAudio.volume > 0.0)
        return;
      this.Stop();
    }
    else
    {
      if (!((UnityEngine.Object) this.VariationUpdater != (UnityEngine.Object) null))
        return;
      this.VariationUpdater.FadeOverTimeToVolume(newVolume, fadeTime);
    }
  }

  public void FadeOutNow()
  {
    if (DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown || !this.IsPlaying || !this.useFades || !((UnityEngine.Object) this.VariationUpdater != (UnityEngine.Object) null))
      return;
    this.VariationUpdater.FadeOutEarly(this.fadeOutTime);
  }

  public void FadeOutNow(float fadeTime)
  {
    if (DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown || !this.IsPlaying || !((UnityEngine.Object) this.VariationUpdater != (UnityEngine.Object) null))
      return;
    this.VariationUpdater.FadeOutEarly(fadeTime);
  }

  public bool WasTriggeredFromTransform(Transform trans)
  {
    return (UnityEngine.Object) this.ObjectToFollow == (UnityEngine.Object) trans || (UnityEngine.Object) this.ObjectToTriggerFrom == (UnityEngine.Object) trans;
  }

  public bool WasTriggeredFromAnyOfTransformMap(HashSet<Transform> transMap)
  {
    return (UnityEngine.Object) this.ObjectToFollow != (UnityEngine.Object) null && transMap.Contains(this.ObjectToFollow) || (UnityEngine.Object) this.ObjectToTriggerFrom != (UnityEngine.Object) null && transMap.Contains(this.ObjectToTriggerFrom);
  }

  public AudioDistortionFilter DistortionFilter
  {
    get
    {
      if ((UnityEngine.Object) this._distFilter != (UnityEngine.Object) null)
        return this._distFilter;
      this._distFilter = this.GetComponent<AudioDistortionFilter>();
      return this._distFilter;
    }
  }

  public AudioReverbFilter ReverbFilter
  {
    get
    {
      if ((UnityEngine.Object) this._reverbFilter != (UnityEngine.Object) null)
        return this._reverbFilter;
      this._reverbFilter = this.GetComponent<AudioReverbFilter>();
      return this._reverbFilter;
    }
  }

  public AudioChorusFilter ChorusFilter
  {
    get
    {
      if ((UnityEngine.Object) this._chorusFilter != (UnityEngine.Object) null)
        return this._chorusFilter;
      this._chorusFilter = this.GetComponent<AudioChorusFilter>();
      return this._chorusFilter;
    }
  }

  public AudioEchoFilter EchoFilter
  {
    get
    {
      if ((UnityEngine.Object) this._echoFilter != (UnityEngine.Object) null)
        return this._echoFilter;
      this._echoFilter = this.GetComponent<AudioEchoFilter>();
      return this._echoFilter;
    }
  }

  public AudioLowPassFilter LowPassFilter
  {
    get => this._lpFilter;
    set => this._lpFilter = value;
  }

  public AudioHighPassFilter HighPassFilter
  {
    get
    {
      if ((UnityEngine.Object) this._hpFilter != (UnityEngine.Object) null)
        return this._hpFilter;
      this._hpFilter = this.GetComponent<AudioHighPassFilter>();
      return this._hpFilter;
    }
  }

  public Transform ObjectToFollow
  {
    get => this._objectToFollow;
    set
    {
      this._objectToFollow = value;
      this.UpdateTransformTracker(value);
    }
  }

  public Transform ObjectToTriggerFrom
  {
    get => this._objectToTriggerFrom;
    set
    {
      this._objectToTriggerFrom = value;
      this.UpdateTransformTracker(value);
    }
  }

  public void UpdateTransformTracker(Transform sourceTrans)
  {
    if ((UnityEngine.Object) sourceTrans == (UnityEngine.Object) null || !Application.isEditor || DarkTonic.MasterAudio.MasterAudio.IsWarming || !((UnityEngine.Object) sourceTrans.GetComponent<AudioTransformTracker>() == (UnityEngine.Object) null))
      return;
    sourceTrans.gameObject.AddComponent<AudioTransformTracker>();
  }

  public bool HasActiveFXFilter
  {
    get
    {
      return (UnityEngine.Object) this.HighPassFilter != (UnityEngine.Object) null && this.HighPassFilter.enabled || (UnityEngine.Object) this.LowPassFilter != (UnityEngine.Object) null && this.LowPassFilter.enabled || (UnityEngine.Object) this.ReverbFilter != (UnityEngine.Object) null && this.ReverbFilter.enabled || (UnityEngine.Object) this.DistortionFilter != (UnityEngine.Object) null && this.DistortionFilter.enabled || (UnityEngine.Object) this.EchoFilter != (UnityEngine.Object) null && this.EchoFilter.enabled || (UnityEngine.Object) this.ChorusFilter != (UnityEngine.Object) null && this.ChorusFilter.enabled;
    }
  }

  public MasterAudioGroup ParentGroup
  {
    get
    {
      if ((UnityEngine.Object) this.Trans.parent == (UnityEngine.Object) null)
        return (MasterAudioGroup) null;
      if ((UnityEngine.Object) this._parentGroupScript == (UnityEngine.Object) null)
        this._parentGroupScript = this.Trans.parent.GetComponent<MasterAudioGroup>();
      if ((UnityEngine.Object) this._parentGroupScript == (UnityEngine.Object) null)
        Debug.LogError((object) $"The Group that Sound Variation '{this.name}' is in does not have a MasterAudioGroup script in it!");
      return this._parentGroupScript;
    }
  }

  public float OriginalPitch
  {
    get
    {
      if ((double) this.original_pitch == 0.0)
        this.original_pitch = this.VarAudio.pitch;
      return this.original_pitch;
    }
  }

  public float OriginalVolume
  {
    get
    {
      if ((double) this.original_volume == 0.0)
        this.original_volume = this.VarAudio.volume;
      return this.original_volume;
    }
  }

  public string SoundGroupName
  {
    get
    {
      if (this._soundGroupName != null)
        return this._soundGroupName;
      this._soundGroupName = this.ParentGroup.GameObjectName;
      return this._soundGroupName;
    }
  }

  public bool IsAvailableToPlay
  {
    get
    {
      if (this.weight == 0)
        return false;
      return !this._playSndParam.IsPlaying && (double) this.VarAudio.time == 0.0 || (double) AudioUtil.GetAudioPlayedPercentage(this.VarAudio) >= (double) this.ParentGroup.retriggerPercentage;
    }
  }

  public float LastTimePlayed
  {
    get => this.\u003CLastTimePlayed\u003Ek__BackingField;
    set => this.\u003CLastTimePlayed\u003Ek__BackingField = value;
  }

  public bool IsPlaying => this._playSndParam.IsPlaying;

  public int InstanceId
  {
    get
    {
      if (this._instanceId < 0)
        this._instanceId = this.GetInstanceID();
      return this._instanceId;
    }
  }

  public Transform Trans
  {
    get
    {
      if ((UnityEngine.Object) this._trans != (UnityEngine.Object) null)
        return this._trans;
      this._trans = this.transform;
      return this._trans;
    }
  }

  public GameObject GameObj
  {
    get
    {
      if ((UnityEngine.Object) this._go != (UnityEngine.Object) null)
        return this._go;
      this._go = this.gameObject;
      return this._go;
    }
  }

  public AudioSource VarAudio
  {
    get
    {
      if ((UnityEngine.Object) this._audioSource != (UnityEngine.Object) null)
        return this._audioSource;
      this._audioSource = this.GetComponent<AudioSource>();
      return this._audioSource;
    }
  }

  public bool AudioLoops
  {
    get
    {
      if (!this._audioLoops.HasValue)
        this._audioLoops = new bool?(this.VarAudio.loop);
      return this._audioLoops.Value;
    }
  }

  public string ResFileName
  {
    get
    {
      if (string.IsNullOrEmpty(this._resFileName))
        this._resFileName = AudioResourceOptimizer.GetLocalizedFileName(this.useLocalization, this.resourceFileName);
      return this._resFileName;
    }
  }

  public SoundGroupVariationUpdater VariationUpdater
  {
    get
    {
      if ((UnityEngine.Object) this._varUpdater != (UnityEngine.Object) null)
        return this._varUpdater;
      this._varUpdater = this.GetComponent<SoundGroupVariationUpdater>();
      return this._varUpdater;
    }
  }

  public SoundGroupVariation.PlaySoundParams PlaySoundParm => this._playSndParam;

  public float SetGroupVolume
  {
    get => this._playSndParam.GroupCalcVolume;
    set => this._playSndParam.GroupCalcVolume = value;
  }

  public int MaxLoops => this._maxLoops;

  public bool Is2D => (double) this.VarAudio.spatialBlend <= 0.0;

  public bool ShouldLoadAsync
  {
    get
    {
      return DarkTonic.MasterAudio.MasterAudio.Instance.resourceClipsAllLoadAsync || this.ParentGroup.resourceClipsAllLoadAsync;
    }
  }

  public bool UsesOcclusion
  {
    get
    {
      if (!this.VariationUpdater.MAThisFrame.useOcclusion || this.Is2D)
        return false;
      switch (this.VariationUpdater.MAThisFrame.occlusionSelectType)
      {
        case DarkTonic.MasterAudio.MasterAudio.OcclusionSelectionType.TurnOnPerBusOrGroup:
          if (this.ParentGroup.isUsingOcclusion)
            return true;
          GroupBus busForGroup = this.ParentGroup.BusForGroup;
          return busForGroup != null && busForGroup.isUsingOcclusion;
        default:
          return true;
      }
    }
  }

  public void SoundLoopStarted(int numberOfLoops)
  {
    if (this.SoundLooped == null)
      return;
    this.SoundLooped(numberOfLoops);
  }

  public void ClearSubscribers()
  {
    this.SoundFinished = (SoundGroupVariation.SoundFinishedEventHandler) null;
    this.SoundLooped = (SoundGroupVariation.SoundLoopedEventHandler) null;
  }

  public delegate void SoundFinishedEventHandler();

  public delegate void SoundLoopedEventHandler(int loopNumberStarted);

  public class PlaySoundParams
  {
    public string SoundType;
    public float VolumePercentage;
    public float? Pitch;
    public double? TimeToSchedulePlay;
    public Transform SourceTrans;
    public bool AttachToSource;
    public float DelaySoundTime;
    public bool IsChainLoop;
    public bool IsSingleSubscribedPlay;
    public float GroupCalcVolume;
    public bool IsPlaying;

    public PlaySoundParams(
      string soundType,
      float volPercent,
      float groupCalcVolume,
      float? pitch,
      Transform sourceTrans,
      bool attach,
      float delaySoundTime,
      double? timeToSchedulePlay,
      bool isChainLoop,
      bool isSingleSubscribedPlay)
    {
      this.SoundType = soundType;
      this.VolumePercentage = volPercent;
      this.GroupCalcVolume = groupCalcVolume;
      this.Pitch = pitch;
      this.SourceTrans = sourceTrans;
      this.AttachToSource = attach;
      this.DelaySoundTime = delaySoundTime;
      this.TimeToSchedulePlay = timeToSchedulePlay;
      this.IsChainLoop = isChainLoop;
      this.IsSingleSubscribedPlay = isSingleSubscribedPlay;
      this.IsPlaying = false;
    }
  }

  public enum PitchMode
  {
    None,
    Gliding,
  }

  public enum FadeMode
  {
    None,
    FadeInOut,
    FadeOutEarly,
    GradualFade,
  }

  public enum RandomPitchMode
  {
    AddToClipPitch,
    IgnoreClipPitch,
  }

  public enum RandomVolumeMode
  {
    AddToClipVolume,
    IgnoreClipVolume,
  }

  public enum DetectEndMode
  {
    None,
    DetectEnd,
  }
}
