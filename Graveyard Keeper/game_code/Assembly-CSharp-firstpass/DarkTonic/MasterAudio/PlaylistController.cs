// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.PlaylistController
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

[AudioScriptOrder(-80)]
[RequireComponent(typeof (AudioSource))]
public class PlaylistController : MonoBehaviour
{
  public const float ScheduledSongMinBadOffset = 0.5f;
  public const int FramesEarlyToTrigger = 2;
  public const int FramesEarlyToBeSyncable = 10;
  public const string NotReadyMessage = "Playlist Controller is not initialized yet. It must call its own Awake & Start method before any other methods are called. If you have a script with an Awake or Start event that needs to call it, make sure PlaylistController.cs is set to execute first (Script Execution Order window in Unity). Awake event is still not guaranteed to work, so use Start where possible.";
  public const float MinSongLength = 0.5f;
  public const float SlowestFrameTimeForCalc = 0.3f;
  public bool startPlaylistOnAwake = true;
  public bool isShuffle;
  public bool isAutoAdvance = true;
  public bool loopPlaylist = true;
  public float _playlistVolume = 1f;
  public bool isMuted;
  public string startPlaylistName = string.Empty;
  public int syncGroupNum = -1;
  public AudioMixerGroup mixerChannel;
  public DarkTonic.MasterAudio.MasterAudio.ItemSpatialBlendType spatialBlendType;
  public float spatialBlend;
  public bool initializedEventExpanded;
  public string initializedCustomEvent = string.Empty;
  public bool crossfadeStartedExpanded;
  public string crossfadeStartedCustomEvent = string.Empty;
  public bool songChangedEventExpanded;
  public string songChangedCustomEvent = string.Empty;
  public bool songEndedEventExpanded;
  public string songEndedCustomEvent = string.Empty;
  public bool songLoopedEventExpanded;
  public string songLoopedCustomEvent = string.Empty;
  public bool playlistStartedEventExpanded;
  public string playlistStartedCustomEvent = string.Empty;
  public bool playlistEndedEventExpanded;
  public string playlistEndedCustomEvent = string.Empty;
  public AudioSource _activeAudio;
  public AudioSource _transitioningAudio;
  public float _activeAudioEndVolume;
  public float _transitioningAudioStartVolume;
  public float _crossFadeStartTime;
  public List<int> _clipsRemaining = new List<int>(10);
  public int _currentSequentialClipIndex;
  public PlaylistController.AudioDuckingMode _duckingMode;
  public float _timeToStartUnducking;
  public float _timeToFinishUnducking;
  public float _originalMusicVolume;
  public float _initialDuckVolume;
  public float _duckRange;
  public MusicSetting _currentSong;
  public GameObject _go;
  public string _name;
  public PlaylistController.FadeMode _curFadeMode;
  public float _slowFadeStartTime;
  public float _slowFadeCompletionTime;
  public float _slowFadeStartVolume;
  public float _slowFadeTargetVolume;
  public DarkTonic.MasterAudio.MasterAudio.Playlist _currentPlaylist;
  public float _lastTimeMissingPlaylistLogged = -5f;
  public Action _fadeCompleteCallback;
  public List<MusicSetting> _queuedSongs = new List<MusicSetting>(5);
  public bool _lostFocus;
  public bool _autoStartedPlaylist;
  public AudioSource _audioClip;
  public AudioSource _transClip;
  public MusicSetting _newSongSetting;
  public bool _nextSongRequested;
  public bool _nextSongScheduled;
  public int _lastRandomClipIndex = -1;
  public float _lastTimeSongRequested = -1f;
  public float _currentDuckVolCut;
  public int? _lastSongPosition;
  public double? _currentSchedSongDspStartTime;
  public double? _currentSchedSongDspEndTime;
  public int _lastFrameSongPosition = -1;
  public Dictionary<AudioSource, double> _scheduledSongOffsetByAudioSource = new Dictionary<AudioSource, double>(2);
  public int _frames;
  public static List<PlaylistController> _instances;
  public int _songsPlayedFromPlaylist;
  public AudioSource _audio1;
  public AudioSource _audio2;
  public Transform _trans;
  public bool _willPersist;
  public double? _songPauseTime;
  public int framesOfSongPlayed;
  [CompilerGenerated]
  public bool \u003CControllerIsReady\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CIsCrossFading\u003Ek__BackingField;

  public event PlaylistController.SongChangedEventHandler SongChanged;

  public event PlaylistController.SongEndedEventHandler SongEnded;

  public event PlaylistController.SongLoopedEventHandler SongLooped;

  public void Awake()
  {
    this.useGUILayout = false;
    if (this.ControllerIsReady)
      return;
    this.ControllerIsReady = false;
    PlaylistController[] objectsOfType = (PlaylistController[]) UnityEngine.Object.FindObjectsOfType(typeof (PlaylistController));
    int num = 0;
    for (int index = 0; index < objectsOfType.Length; ++index)
    {
      if (objectsOfType[index].ControllerName == this.ControllerName)
        ++num;
    }
    if (num > 1)
    {
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.gameObject);
      Debug.Log((object) $"More than one Playlist Controller prefab exists in this Scene with the same name. Destroying the one called '{this.ControllerName}'. You may wish to set up a Bootstrapper Scene so this does not occur.");
    }
    else
    {
      this._autoStartedPlaylist = false;
      this._duckingMode = PlaylistController.AudioDuckingMode.NotDucking;
      this._currentSong = (MusicSetting) null;
      this._songsPlayedFromPlaylist = 0;
      AudioSource[] components = this.GetComponents<AudioSource>();
      if (components.Length < 2)
      {
        Debug.LogError((object) "This prefab should have exactly two Audio Source components. Please revert it.");
      }
      else
      {
        DarkTonic.MasterAudio.MasterAudio safeInstance = DarkTonic.MasterAudio.MasterAudio.SafeInstance;
        this._willPersist = (UnityEngine.Object) safeInstance != (UnityEngine.Object) null && safeInstance.persistBetweenScenes;
        this._audio1 = components[0];
        this._audio2 = components[1];
        this._audio1.clip = (AudioClip) null;
        this._audio2.clip = (AudioClip) null;
        if (this._audio1.playOnAwake || this._audio2.playOnAwake)
          Debug.LogWarning((object) $"One or more 'Play on Awake' checkboxes in the Audio Sources on Playlist Controller '{this.name}' are checked. These are not used in Master Audio. Make sure to uncheck them before hitting Play next time. For Playlist Controllers, use the similarly named checkbox 'Start Playlist on Awake' in the Playlist Controller's Inspector.");
        this._activeAudio = this._audio1;
        this._transitioningAudio = this._audio2;
        this._audio1.outputAudioMixerGroup = this.mixerChannel;
        this._audio2.outputAudioMixerGroup = this.mixerChannel;
        this.SetSpatialBlend();
        this._curFadeMode = PlaylistController.FadeMode.None;
        this._fadeCompleteCallback = (Action) null;
        this._lostFocus = false;
      }
    }
  }

  public void SetSpatialBlend()
  {
    if ((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (UnityEngine.Object) null)
      return;
    switch (DarkTonic.MasterAudio.MasterAudio.Instance.musicSpatialBlendType)
    {
      case DarkTonic.MasterAudio.MasterAudio.AllMusicSpatialBlendType.ForceAllTo2D:
        this.SetAudioSpatialBlend(0.0f);
        break;
      case DarkTonic.MasterAudio.MasterAudio.AllMusicSpatialBlendType.ForceAllTo3D:
        this.SetAudioSpatialBlend(1f);
        break;
      case DarkTonic.MasterAudio.MasterAudio.AllMusicSpatialBlendType.ForceAllToCustom:
        this.SetAudioSpatialBlend(DarkTonic.MasterAudio.MasterAudio.Instance.musicSpatialBlend);
        break;
      case DarkTonic.MasterAudio.MasterAudio.AllMusicSpatialBlendType.AllowDifferentPerController:
        switch (this.spatialBlendType)
        {
          case DarkTonic.MasterAudio.MasterAudio.ItemSpatialBlendType.ForceTo2D:
            this.SetAudioSpatialBlend(0.0f);
            return;
          case DarkTonic.MasterAudio.MasterAudio.ItemSpatialBlendType.ForceTo3D:
            this.SetAudioSpatialBlend(1f);
            return;
          case DarkTonic.MasterAudio.MasterAudio.ItemSpatialBlendType.ForceToCustom:
            this.SetAudioSpatialBlend(this.spatialBlend);
            return;
          case DarkTonic.MasterAudio.MasterAudio.ItemSpatialBlendType.UseCurveFromAudioSource:
            return;
          default:
            return;
        }
    }
  }

  public void SetAudiosIfEmpty()
  {
    AudioSource[] components = this.GetComponents<AudioSource>();
    this._audio1 = components[0];
    this._audio2 = components[1];
  }

  public void SetAudioSpatialBlend(float blend)
  {
    if ((UnityEngine.Object) this._audio1 == (UnityEngine.Object) null)
      this.SetAudiosIfEmpty();
    this._audio1.spatialBlend = blend;
    this._audio2.spatialBlend = blend;
  }

  public void Start()
  {
    if (this.ControllerIsReady)
      return;
    if ((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "No Master Audio game object exists in the Hierarchy. Aborting Playlist Controller setup code.");
    }
    else
    {
      if (!string.IsNullOrEmpty(this.startPlaylistName) && this._currentPlaylist == null)
        this.InitializePlaylist();
      this.ControllerIsReady = true;
      if (this.initializedEventExpanded && this.initializedCustomEvent != string.Empty && this.initializedCustomEvent != "[None]")
        DarkTonic.MasterAudio.MasterAudio.FireCustomEvent(this.initializedCustomEvent, this.Trans, false);
      this.AutoStartPlaylist();
      if (!this.IsMuted)
        return;
      this.MutePlaylist();
    }
  }

  public void AutoStartPlaylist()
  {
    if (this._currentPlaylist == null || !this.startPlaylistOnAwake || !this.IsFrameFastEnough || this._autoStartedPlaylist)
      return;
    this.PlayNextOrRandom(PlaylistController.AudioPlayType.PlayNow);
    this._autoStartedPlaylist = true;
  }

  public void CoUpdate()
  {
    if ((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (UnityEngine.Object) null || this._curFadeMode != PlaylistController.FadeMode.GradualFade || (UnityEngine.Object) this._activeAudio == (UnityEngine.Object) null)
      return;
    float val1 = this._slowFadeStartVolume + (this._slowFadeTargetVolume - this._slowFadeStartVolume) * Math.Max(Math.Min((float) (1.0 - ((double) this._slowFadeCompletionTime - (double) AudioUtil.Time) / ((double) this._slowFadeCompletionTime - (double) this._slowFadeStartTime)), 1f), 0.0f);
    this._playlistVolume = (double) this._slowFadeTargetVolume <= (double) this._slowFadeStartVolume ? Math.Max(val1, this._slowFadeTargetVolume) : Math.Min(val1, this._slowFadeTargetVolume);
    this.UpdateMasterVolume();
    if ((double) AudioUtil.Time < (double) this._slowFadeCompletionTime)
      return;
    if (DarkTonic.MasterAudio.MasterAudio.Instance.stopZeroVolumePlaylists && (double) this._slowFadeTargetVolume == 0.0)
      this.StopPlaylist();
    if (this._fadeCompleteCallback != null)
    {
      this._fadeCompleteCallback();
      this._fadeCompleteCallback = (Action) null;
    }
    this._curFadeMode = PlaylistController.FadeMode.None;
  }

  public void OnEnable()
  {
    PlaylistController._instances = (List<PlaylistController>) null;
    DarkTonic.MasterAudio.MasterAudio.TrackRuntimeAudioSources(new List<AudioSource>()
    {
      this._audio1,
      this._audio2
    });
  }

  public void OnDisable()
  {
    PlaylistController._instances = (List<PlaylistController>) null;
    if ((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (UnityEngine.Object) null || DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown)
      return;
    if ((UnityEngine.Object) this.ActiveAudioSource != (UnityEngine.Object) null && (UnityEngine.Object) this.ActiveAudioSource.clip != (UnityEngine.Object) null && !this._willPersist)
      this.StopPlaylist();
    DarkTonic.MasterAudio.MasterAudio.StopTrackingRuntimeAudioSources(new List<AudioSource>()
    {
      this._audio1,
      this._audio2
    });
  }

  public void OnApplicationPause(bool pauseStatus) => this._lostFocus = pauseStatus;

  public void Update()
  {
    ++this._frames;
    this.CoUpdate();
    if (this._lostFocus || !this.ControllerIsReady)
      return;
    this.AutoStartPlaylist();
    if (this._activeAudio.isPlaying)
      ++this.framesOfSongPlayed;
    if (this.IsCrossFading)
    {
      if ((double) this._activeAudio.volume >= (double) this._activeAudioEndVolume)
      {
        this.CeaseAudioSource(this._transitioningAudio);
        this.IsCrossFading = false;
        if (this.CanSchedule && !this._nextSongScheduled)
          this.PlayNextOrRandom(PlaylistController.AudioPlayType.Schedule);
        this.SetDuckProperties();
      }
      float num = (Time.realtimeSinceStartup - this._crossFadeStartTime) / Math.Max(this.CrossFadeTime, 1f / 1000f);
      this._activeAudio.volume = num * this._activeAudioEndVolume;
      this._transitioningAudio.volume = this._transitioningAudioStartVolume * (1f - num);
    }
    if (!this._activeAudio.loop && (UnityEngine.Object) this._activeAudio.clip != (UnityEngine.Object) null)
    {
      if (!AudioUtil.IsAudioPaused(this._activeAudio))
      {
        if (!this._activeAudio.isPlaying)
        {
          if (!this.IsAutoAdvance)
          {
            this.FirePlaylistEndedEventIfAny();
            this.CeaseAudioSource(this._activeAudio);
            return;
          }
          if ((!this.isShuffle ? this._currentSequentialClipIndex >= this._currentPlaylist.MusicSettings.Count : this._clipsRemaining.Count == 0) && !this._activeAudio.isPlaying)
          {
            this.FirePlaylistEndedEventIfAny();
            this.CeaseAudioSource(this._activeAudio);
            return;
          }
        }
        bool flag = false;
        if (this.ShouldNotSwitchEarly)
        {
          if (this._currentSchedSongDspStartTime.HasValue && AudioSettings.dspTime > this._currentSchedSongDspStartTime.Value)
            flag = true;
        }
        else if (this.PlaylistState == PlaylistController.PlaylistStates.Stopped)
          flag = true;
        else if (this.IsFrameFastEnough)
          flag = (double) this._activeAudio.clip.length - (double) this._activeAudio.time - (double) AudioUtil.AdjustEndLeadTimeForPitch(this.CrossFadeTime, this._activeAudio) <= (double) AudioUtil.AdjustEndLeadTimeForPitch(AudioUtil.FrameTime * 2f, this._activeAudio);
        if (flag)
        {
          if (this._currentPlaylist.fadeOutLastSong)
          {
            if (this.isShuffle)
            {
              if (this._clipsRemaining.Count == 0 || !this.IsAutoAdvance)
              {
                this.FadeOutPlaylist();
                return;
              }
            }
            else if (this._currentSequentialClipIndex >= this._currentPlaylist.MusicSettings.Count || this._currentPlaylist.MusicSettings.Count == 1 || !this.IsAutoAdvance)
            {
              this.FadeOutPlaylist();
              return;
            }
          }
          if (this.IsAutoAdvance && !this._nextSongRequested && (double) this._lastTimeSongRequested + 0.5 <= (double) AudioUtil.Time)
          {
            this._lastTimeSongRequested = AudioUtil.Time;
            if (this.CanSchedule)
            {
              this._lastSongPosition = new int?();
              this.FadeInScheduledSong();
            }
            else
            {
              this._lastSongPosition = new int?(0);
              this.PlayNextOrRandom(PlaylistController.AudioPlayType.PlayNow);
            }
          }
        }
      }
      else
        goto label_42;
    }
    if (this._activeAudio.loop && (UnityEngine.Object) this._activeAudio.clip != (UnityEngine.Object) null)
    {
      if (this._activeAudio.timeSamples < this._lastFrameSongPosition)
      {
        string name = this._activeAudio.clip.name;
        if (this.SongLooped != null && !string.IsNullOrEmpty(name))
          this.SongLooped(name);
        if (this.songLoopedEventExpanded && this.songLoopedCustomEvent != string.Empty && this.songLoopedCustomEvent != "[None]")
          DarkTonic.MasterAudio.MasterAudio.FireCustomEvent(this.songLoopedCustomEvent, this.Trans, false);
      }
      this._lastFrameSongPosition = this._activeAudio.timeSamples;
    }
label_42:
    if (this.IsCrossFading)
      return;
    this.AudioDucking();
  }

  public static PlaylistController InstanceByName(
    string playlistControllerName,
    bool errorIfNotFound = true)
  {
    PlaylistController playlistController = PlaylistController.Instances.Find((Predicate<PlaylistController>) (obj => (UnityEngine.Object) obj != (UnityEngine.Object) null && obj.ControllerName == playlistControllerName));
    if ((UnityEngine.Object) playlistController != (UnityEngine.Object) null)
      return playlistController;
    if (errorIfNotFound)
      Debug.LogError((object) $"Could not find Playlist Controller '{playlistControllerName}'.");
    return (PlaylistController) null;
  }

  public bool IsSongPlaying(string songName)
  {
    return this.HasPlaylist && !((UnityEngine.Object) this.ActiveAudioSource == (UnityEngine.Object) null) && !((UnityEngine.Object) this.ActiveAudioSource.clip == (UnityEngine.Object) null) && this.ActiveAudioSource.clip.name == songName;
  }

  public void ClearQueue()
  {
    if (!this.ControllerIsReady)
      Debug.LogError((object) "Playlist Controller is not initialized yet. It must call its own Awake & Start method before any other methods are called. If you have a script with an Awake or Start event that needs to call it, make sure PlaylistController.cs is set to execute first (Script Execution Order window in Unity). Awake event is still not guaranteed to work, so use Start where possible.");
    else
      this._queuedSongs.Clear();
  }

  public void ToggleMutePlaylist()
  {
    if (Application.isPlaying && !this.ControllerIsReady)
      Debug.LogError((object) "Playlist Controller is not initialized yet. It must call its own Awake & Start method before any other methods are called. If you have a script with an Awake or Start event that needs to call it, make sure PlaylistController.cs is set to execute first (Script Execution Order window in Unity). Awake event is still not guaranteed to work, so use Start where possible.");
    else if (this.IsMuted)
      this.UnmutePlaylist();
    else
      this.MutePlaylist();
  }

  public void MutePlaylist() => this.PlaylistIsMuted = true;

  public void UnmutePlaylist() => this.PlaylistIsMuted = false;

  public void PausePlaylist()
  {
    if (!this.ControllerIsReady)
    {
      Debug.LogError((object) "Playlist Controller is not initialized yet. It must call its own Awake & Start method before any other methods are called. If you have a script with an Awake or Start event that needs to call it, make sure PlaylistController.cs is set to execute first (Script Execution Order window in Unity). Awake event is still not guaranteed to work, so use Start where possible.");
    }
    else
    {
      if ((UnityEngine.Object) this._activeAudio == (UnityEngine.Object) null || (UnityEngine.Object) this._transitioningAudio == (UnityEngine.Object) null)
        return;
      if ((UnityEngine.Object) this._activeAudio.clip != (UnityEngine.Object) null)
        this._activeAudio.Pause();
      if (!this._songPauseTime.HasValue)
        this._songPauseTime = new double?(AudioSettings.dspTime);
      if (!((UnityEngine.Object) this._transitioningAudio.clip != (UnityEngine.Object) null))
        return;
      this._transitioningAudio.Pause();
    }
  }

  public bool UnpausePlaylist()
  {
    if (!this.ControllerIsReady)
    {
      Debug.LogError((object) "Playlist Controller is not initialized yet. It must call its own Awake & Start method before any other methods are called. If you have a script with an Awake or Start event that needs to call it, make sure PlaylistController.cs is set to execute first (Script Execution Order window in Unity). Awake event is still not guaranteed to work, so use Start where possible.");
      this._songPauseTime = new double?();
      return false;
    }
    if (this.PlaylistState == PlaylistController.PlaylistStates.Playing || this.PlaylistState == PlaylistController.PlaylistStates.Crossfading || this.PlaylistState == PlaylistController.PlaylistStates.Stopped)
    {
      this._songPauseTime = new double?();
      return false;
    }
    if ((UnityEngine.Object) this._activeAudio == (UnityEngine.Object) null || (UnityEngine.Object) this._transitioningAudio == (UnityEngine.Object) null)
    {
      this._songPauseTime = new double?();
      return false;
    }
    if ((UnityEngine.Object) this._activeAudio.clip == (UnityEngine.Object) null && this._currentPlaylist != null)
    {
      this.FinishPlaylistInit();
      this._songPauseTime = new double?();
      return true;
    }
    if ((UnityEngine.Object) this._activeAudio.clip == (UnityEngine.Object) null)
    {
      this._songPauseTime = new double?();
      return false;
    }
    if (!this._scheduledSongOffsetByAudioSource.ContainsKey(this._activeAudio))
    {
      this._activeAudio.Play();
      this.framesOfSongPlayed = 0;
      AudioUtil.ClipPlayed(this._activeAudio.clip, this.GameObj);
    }
    else if (this._songPauseTime.HasValue && this._currentSchedSongDspStartTime.HasValue)
    {
      double scheduledPlayTimeOffset = this._currentSchedSongDspStartTime.Value - AudioSettings.dspTime + (AudioSettings.dspTime - this._songPauseTime.Value);
      this._songPauseTime = new double?();
      this._activeAudio.Stop();
      this.ScheduleClipPlay(scheduledPlayTimeOffset, this._activeAudio, true);
    }
    if (!this._scheduledSongOffsetByAudioSource.ContainsKey(this._transitioningAudio))
    {
      this._transitioningAudio.Play();
      AudioUtil.ClipPlayed(this._transitioningAudio.clip, this.GameObj);
    }
    else if (this._songPauseTime.HasValue && this._currentSchedSongDspStartTime.HasValue)
    {
      double scheduledPlayTimeOffset = this._currentSchedSongDspStartTime.Value - AudioSettings.dspTime + (AudioSettings.dspTime - this._songPauseTime.Value);
      this._songPauseTime = new double?();
      this._transitioningAudio.Stop();
      this.ScheduleClipPlay(scheduledPlayTimeOffset, this._transitioningAudio, true);
    }
    return true;
  }

  public void StopPlaylist(bool onlyFadingClip = false)
  {
    if (!this.ControllerIsReady)
    {
      Debug.LogError((object) "Playlist Controller is not initialized yet. It must call its own Awake & Start method before any other methods are called. If you have a script with an Awake or Start event that needs to call it, make sure PlaylistController.cs is set to execute first (Script Execution Order window in Unity). Awake event is still not guaranteed to work, so use Start where possible.");
    }
    else
    {
      if (!Application.isPlaying)
        return;
      this._currentSchedSongDspStartTime = new double?();
      this._currentSchedSongDspEndTime = new double?();
      this._currentSong = (MusicSetting) null;
      switch (this.PlaylistState)
      {
        case PlaylistController.PlaylistStates.NotInScene:
        case PlaylistController.PlaylistStates.Stopped:
          break;
        default:
          if (!onlyFadingClip)
            this.CeaseAudioSource(this._activeAudio);
          this.CeaseAudioSource(this._transitioningAudio);
          break;
      }
    }
  }

  public void FadeToVolume(float targetVolume, float fadeTime, Action callback = null)
  {
    if (!this.ControllerIsReady)
      Debug.LogError((object) "Playlist Controller is not initialized yet. It must call its own Awake & Start method before any other methods are called. If you have a script with an Awake or Start event that needs to call it, make sure PlaylistController.cs is set to execute first (Script Execution Order window in Unity). Awake event is still not guaranteed to work, so use Start where possible.");
    else if ((double) fadeTime <= 0.10000000149011612)
    {
      this._playlistVolume = targetVolume;
      this.UpdateMasterVolume();
      this._curFadeMode = PlaylistController.FadeMode.None;
    }
    else
    {
      this._curFadeMode = PlaylistController.FadeMode.GradualFade;
      this._slowFadeStartVolume = this._duckingMode != PlaylistController.AudioDuckingMode.NotDucking ? this._activeAudio.volume : this._playlistVolume;
      this._slowFadeTargetVolume = targetVolume;
      this._slowFadeStartTime = AudioUtil.Time;
      this._slowFadeCompletionTime = AudioUtil.Time + fadeTime;
      this._fadeCompleteCallback = callback;
    }
  }

  public void PlayRandomSong()
  {
    if (!this.ControllerIsReady)
      Debug.LogError((object) "Playlist Controller is not initialized yet. It must call its own Awake & Start method before any other methods are called. If you have a script with an Awake or Start event that needs to call it, make sure PlaylistController.cs is set to execute first (Script Execution Order window in Unity). Awake event is still not guaranteed to work, so use Start where possible.");
    else
      this.PlayARandomSong(PlaylistController.AudioPlayType.PlayNow);
  }

  public void PlayARandomSong(PlaylistController.AudioPlayType playType)
  {
    if (!this.ControllerIsReady)
      Debug.LogError((object) "Playlist Controller is not initialized yet. It must call its own Awake & Start method before any other methods are called. If you have a script with an Awake or Start event that needs to call it, make sure PlaylistController.cs is set to execute first (Script Execution Order window in Unity). Awake event is still not guaranteed to work, so use Start where possible.");
    else if (this._clipsRemaining.Count == 0)
    {
      Debug.LogWarning((object) "There are no clips left in this Playlist. Turn on Loop Playlist if you want to loop the entire song selection.");
    }
    else
    {
      if (this.IsCrossFading && playType == PlaylistController.AudioPlayType.Schedule)
        return;
      if (this.framesOfSongPlayed > 0)
        this._nextSongScheduled = false;
      int num = UnityEngine.Random.Range(0, this._clipsRemaining.Count);
      int index = this._clipsRemaining[num];
      switch (playType)
      {
        case PlaylistController.AudioPlayType.PlayNow:
          this.RemoveRandomClip(num);
          break;
        case PlaylistController.AudioPlayType.Schedule:
          this._lastRandomClipIndex = num;
          break;
        case PlaylistController.AudioPlayType.AlreadyScheduled:
          if (this._lastRandomClipIndex >= 0)
          {
            this.RemoveRandomClip(this._lastRandomClipIndex);
            break;
          }
          break;
      }
      this.PlaySong(this._currentPlaylist.MusicSettings[index], playType);
    }
  }

  public void RemoveRandomClip(int randIndex)
  {
    this._clipsRemaining.RemoveAt(randIndex);
    if (!this.loopPlaylist || this._clipsRemaining.Count != 0)
      return;
    this.FillClips();
  }

  public void PlayFirstQueuedSong(PlaylistController.AudioPlayType playType)
  {
    if (this._queuedSongs.Count == 0)
    {
      Debug.LogWarning((object) $"There are zero queued songs in PlaylistController '{this.ControllerName}'. Cannot play first queued song.");
    }
    else
    {
      MusicSetting queuedSong = this._queuedSongs[0];
      this._queuedSongs.RemoveAt(0);
      this._currentSequentialClipIndex = queuedSong.songIndex;
      this.PlaySong(queuedSong, playType);
    }
  }

  public void PlayNextSong()
  {
    if (!this.ControllerIsReady)
      Debug.LogError((object) "Playlist Controller is not initialized yet. It must call its own Awake & Start method before any other methods are called. If you have a script with an Awake or Start event that needs to call it, make sure PlaylistController.cs is set to execute first (Script Execution Order window in Unity). Awake event is still not guaranteed to work, so use Start where possible.");
    else
      this.PlayTheNextSong(PlaylistController.AudioPlayType.PlayNow);
  }

  public void PlayTheNextSong(PlaylistController.AudioPlayType playType)
  {
    if (this._currentPlaylist == null || this.IsCrossFading && playType == PlaylistController.AudioPlayType.Schedule)
      return;
    if (playType != PlaylistController.AudioPlayType.AlreadyScheduled && this._songsPlayedFromPlaylist > 0 && !this._nextSongScheduled)
      this.AdvanceSongCounter();
    if (this._currentSequentialClipIndex >= this._currentPlaylist.MusicSettings.Count)
    {
      Debug.LogWarning((object) "There are no clips left in this Playlist. Turn on Loop Playlist if you want to loop the entire song selection.");
    }
    else
    {
      if (this.framesOfSongPlayed > 0)
      {
        this._nextSongScheduled = false;
        this._lastSongPosition = new int?(this.ActiveAudioSource.timeSamples);
      }
      this.PlaySong(this._currentPlaylist.MusicSettings[this._currentSequentialClipIndex], playType);
    }
  }

  public void AdvanceSongCounter()
  {
    ++this._currentSequentialClipIndex;
    if (this._currentSequentialClipIndex < this._currentPlaylist.MusicSettings.Count || !this.loopPlaylist)
      return;
    this._currentSequentialClipIndex = 0;
  }

  public void StopPlaylistAfterCurrentSong()
  {
    if (!this.ControllerIsReady)
      Debug.LogError((object) "Playlist Controller is not initialized yet. It must call its own Awake & Start method before any other methods are called. If you have a script with an Awake or Start event that needs to call it, make sure PlaylistController.cs is set to execute first (Script Execution Order window in Unity). Awake event is still not guaranteed to work, so use Start where possible.");
    else if (this._currentPlaylist == null)
      DarkTonic.MasterAudio.MasterAudio.LogNoPlaylist(this.ControllerName, nameof (StopPlaylistAfterCurrentSong));
    else if (!this._activeAudio.isPlaying)
    {
      Debug.Log((object) "No song is currently playing.");
    }
    else
    {
      this._activeAudio.loop = false;
      this._queuedSongs.Clear();
      this.isAutoAdvance = false;
      if (this._scheduledSongOffsetByAudioSource.ContainsKey(this._activeAudio))
        this.CeaseAudioSource(this._activeAudio);
      if (!this._scheduledSongOffsetByAudioSource.ContainsKey(this._transitioningAudio))
        return;
      this.CeaseAudioSource(this._transitioningAudio);
    }
  }

  public void StopLoopingCurrentSong()
  {
    if (!this.ControllerIsReady)
      Debug.LogError((object) "Playlist Controller is not initialized yet. It must call its own Awake & Start method before any other methods are called. If you have a script with an Awake or Start event that needs to call it, make sure PlaylistController.cs is set to execute first (Script Execution Order window in Unity). Awake event is still not guaranteed to work, so use Start where possible.");
    else if (this._currentPlaylist == null)
      DarkTonic.MasterAudio.MasterAudio.LogNoPlaylist(this.ControllerName, nameof (StopLoopingCurrentSong));
    else if (!this._activeAudio.isPlaying)
    {
      Debug.Log((object) "No song is currently playing.");
    }
    else
    {
      this._activeAudio.loop = false;
      if (!this.CanSchedule || this._queuedSongs.Count != 0)
        return;
      this.ScheduleNextSong();
    }
  }

  public void QueuePlaylistClip(string clipName, bool scheduleNow = true)
  {
    if (!this.ControllerIsReady)
      Debug.LogError((object) "Playlist Controller is not initialized yet. It must call its own Awake & Start method before any other methods are called. If you have a script with an Awake or Start event that needs to call it, make sure PlaylistController.cs is set to execute first (Script Execution Order window in Unity). Awake event is still not guaranteed to work, so use Start where possible.");
    else if (this._currentPlaylist == null)
      DarkTonic.MasterAudio.MasterAudio.LogNoPlaylist(this.ControllerName, nameof (QueuePlaylistClip));
    else if (!this._activeAudio.isPlaying)
    {
      this.TriggerPlaylistClip(clipName);
    }
    else
    {
      MusicSetting musicSetting = this._currentPlaylist.MusicSettings.Find((Predicate<MusicSetting>) (obj =>
      {
        if (obj.audLocation != DarkTonic.MasterAudio.MasterAudio.AudioLocation.Clip)
          return obj.resourceFileName == clipName;
        return (UnityEngine.Object) obj.clip != (UnityEngine.Object) null && obj.clip.name == clipName;
      }));
      if (musicSetting == null)
      {
        Debug.LogWarning((object) $"Could not find clip '{clipName}' in current Playlist in '{this.ControllerName}'.");
      }
      else
      {
        this._activeAudio.loop = false;
        this._queuedSongs.Add(musicSetting);
        if (!(this.CanSchedule & scheduleNow))
          return;
        this.PlayNextOrRandom(PlaylistController.AudioPlayType.Schedule);
      }
    }
  }

  public bool TriggerPlaylistClip(string clipName)
  {
    if (!this.ControllerIsReady)
    {
      Debug.LogError((object) "Playlist Controller is not initialized yet. It must call its own Awake & Start method before any other methods are called. If you have a script with an Awake or Start event that needs to call it, make sure PlaylistController.cs is set to execute first (Script Execution Order window in Unity). Awake event is still not guaranteed to work, so use Start where possible.");
      return false;
    }
    if (this._currentPlaylist == null)
    {
      DarkTonic.MasterAudio.MasterAudio.LogNoPlaylist(this.ControllerName, nameof (TriggerPlaylistClip));
      return false;
    }
    MusicSetting setting = this._currentPlaylist.MusicSettings.Find((Predicate<MusicSetting>) (obj => obj.alias == clipName)) ?? this._currentPlaylist.MusicSettings.Find((Predicate<MusicSetting>) (obj => obj.audLocation == DarkTonic.MasterAudio.MasterAudio.AudioLocation.Clip ? (UnityEngine.Object) obj.clip != (UnityEngine.Object) null && obj.clip.name == clipName : obj.resourceFileName == clipName || obj.songName == clipName));
    if (setting == null)
    {
      Debug.LogWarning((object) $"Could not find clip '{clipName}' in current Playlist in '{this.ControllerName}'.");
      return false;
    }
    this._currentSequentialClipIndex = setting.songIndex;
    this.PlaySong(setting, PlaylistController.AudioPlayType.PlayNow);
    return true;
  }

  public void DuckMusicForTime(
    float duckLength,
    float unduckTime,
    float pitch,
    float duckedTimePercentage,
    float duckedVolCut)
  {
    if (DarkTonic.MasterAudio.MasterAudio.IsWarming)
      return;
    if (!this.ControllerIsReady)
    {
      Debug.LogError((object) "Playlist Controller is not initialized yet. It must call its own Awake & Start method before any other methods are called. If you have a script with an Awake or Start event that needs to call it, make sure PlaylistController.cs is set to execute first (Script Execution Order window in Unity). Awake event is still not guaranteed to work, so use Start where possible.");
    }
    else
    {
      if (this.IsCrossFading)
        return;
      float num1 = AudioUtil.AdjustAudioClipDurationForPitch(duckLength, pitch);
      this._currentDuckVolCut = duckedVolCut;
      float floatVolumeFromDb = AudioUtil.GetFloatVolumeFromDb(AudioUtil.GetDbFromFloatVolume(this._originalMusicVolume) + duckedVolCut);
      this._initialDuckVolume = floatVolumeFromDb;
      this._duckRange = this._originalMusicVolume - floatVolumeFromDb;
      this._duckingMode = PlaylistController.AudioDuckingMode.SetToDuck;
      this._timeToStartUnducking = AudioUtil.Time + num1 * duckedTimePercentage;
      float num2 = this._timeToStartUnducking + unduckTime;
      if ((double) num2 > (double) AudioUtil.Time + (double) num1)
        num2 = AudioUtil.Time + num1;
      this._timeToFinishUnducking = num2;
    }
  }

  public void InitControllerIfNot()
  {
    if (this.ControllerIsReady)
      return;
    this.Awake();
    this.Start();
  }

  public void UpdateMasterVolume()
  {
    if (!Application.isPlaying)
      return;
    this.InitControllerIfNot();
    if (this._currentSong != null)
    {
      float num = this._currentSong.volume * this.PlaylistVolume;
      if (!this.IsCrossFading)
      {
        if ((UnityEngine.Object) this._activeAudio != (UnityEngine.Object) null)
          this._activeAudio.volume = num;
        if ((UnityEngine.Object) this._transitioningAudio != (UnityEngine.Object) null)
          this._transitioningAudio.volume = num;
      }
      this._activeAudioEndVolume = num;
    }
    this.SetDuckProperties();
  }

  public void StartPlaylist(string playlistName)
  {
    if (!this.ControllerIsReady)
      Debug.LogError((object) "Playlist Controller is not initialized yet. It must call its own Awake & Start method before any other methods are called. If you have a script with an Awake or Start event that needs to call it, make sure PlaylistController.cs is set to execute first (Script Execution Order window in Unity). Awake event is still not guaranteed to work, so use Start where possible.");
    else if (this._currentPlaylist != null && this._currentPlaylist.playlistName == playlistName)
      this.RestartPlaylist();
    else
      this.ChangePlaylist(playlistName);
  }

  public void ChangePlaylist(string playlistName, bool playFirstClip = true)
  {
    this.InitControllerIfNot();
    if (!this.ControllerIsReady)
      Debug.LogError((object) "Playlist Controller is not initialized yet. It must call its own Awake & Start method before any other methods are called. If you have a script with an Awake or Start event that needs to call it, make sure PlaylistController.cs is set to execute first (Script Execution Order window in Unity). Awake event is still not guaranteed to work, so use Start where possible.");
    else if (this._currentPlaylist != null && this._currentPlaylist.playlistName == playlistName)
    {
      Debug.LogWarning((object) $"The Playlist '{playlistName}' is already loaded. Ignoring Change Playlist request.");
    }
    else
    {
      this.startPlaylistName = playlistName;
      this.FinishPlaylistInit(playFirstClip);
    }
  }

  public void FinishPlaylistInit(bool playFirstClip = true)
  {
    if (this.IsCrossFading)
      this.StopPlaylist(true);
    this.InitializePlaylist();
    if (!Application.isPlaying)
      return;
    this._queuedSongs.Clear();
    if (!playFirstClip)
      return;
    this.PlayNextOrRandom(PlaylistController.AudioPlayType.PlayNow);
  }

  public void RestartPlaylist()
  {
    if (!this.ControllerIsReady)
      Debug.LogError((object) "Playlist Controller is not initialized yet. It must call its own Awake & Start method before any other methods are called. If you have a script with an Awake or Start event that needs to call it, make sure PlaylistController.cs is set to execute first (Script Execution Order window in Unity). Awake event is still not guaranteed to work, so use Start where possible.");
    else
      this.FinishPlaylistInit();
  }

  public void CheckIfPlaylistStarted()
  {
    if (this._songsPlayedFromPlaylist > 0 || !this.playlistStartedEventExpanded || !(this.playlistStartedCustomEvent != string.Empty) || !(this.playlistStartedCustomEvent != "[None]"))
      return;
    DarkTonic.MasterAudio.MasterAudio.FireCustomEvent(this.playlistStartedCustomEvent, this.Trans);
  }

  public PlaylistController FindOtherControllerInSameSyncGroup()
  {
    return this.syncGroupNum <= 0 || this._currentPlaylist.songTransitionType != DarkTonic.MasterAudio.MasterAudio.SongFadeInPosition.SynchronizeClips ? (PlaylistController) null : PlaylistController.Instances.Find((Predicate<PlaylistController>) (obj => (UnityEngine.Object) obj != (UnityEngine.Object) this && obj.syncGroupNum == this.syncGroupNum && (UnityEngine.Object) obj.ActiveAudioSource != (UnityEngine.Object) null && obj.ActiveAudioSource.isPlaying));
  }

  public void FadeOutPlaylist()
  {
    if (this._curFadeMode == PlaylistController.FadeMode.GradualFade)
      return;
    float volumeBeforeFade = this._playlistVolume;
    this.FadeToVolume(0.0f, this.CrossFadeTime, (Action) (() =>
    {
      this.StopPlaylist();
      this._playlistVolume = volumeBeforeFade;
    }));
  }

  public void InitializePlaylist()
  {
    this.FillClips();
    this._songsPlayedFromPlaylist = 0;
    this._currentSequentialClipIndex = 0;
    this._nextSongScheduled = false;
    this._lastRandomClipIndex = -1;
  }

  public void PlayNextOrRandom(PlaylistController.AudioPlayType playType)
  {
    this._nextSongRequested = true;
    if (this._queuedSongs.Count > 0)
      this.PlayFirstQueuedSong(playType);
    else if (!this.isShuffle)
      this.PlayTheNextSong(playType);
    else
      this.PlayARandomSong(playType);
  }

  public void FirePlaylistEndedEventIfAny()
  {
    if (!this.playlistEndedEventExpanded || !(this.playlistEndedCustomEvent != string.Empty) || !(this.playlistStartedCustomEvent != "[None]"))
      return;
    DarkTonic.MasterAudio.MasterAudio.FireCustomEvent(this.playlistEndedCustomEvent, this.Trans);
  }

  public void FillClips()
  {
    this._clipsRemaining.Clear();
    if (this.startPlaylistName == "[No Playlist]")
      return;
    this._currentPlaylist = DarkTonic.MasterAudio.MasterAudio.GrabPlaylist(this.startPlaylistName);
    if (this._currentPlaylist == null)
      return;
    for (int index = 0; index < this._currentPlaylist.MusicSettings.Count; ++index)
    {
      MusicSetting musicSetting = this._currentPlaylist.MusicSettings[index];
      musicSetting.songIndex = index;
      if (musicSetting.audLocation != DarkTonic.MasterAudio.MasterAudio.AudioLocation.ResourceFile)
      {
        if ((UnityEngine.Object) musicSetting.clip == (UnityEngine.Object) null)
          continue;
      }
      else if (string.IsNullOrEmpty(musicSetting.resourceFileName))
        continue;
      this._clipsRemaining.Add(index);
    }
  }

  public void PlaySong(MusicSetting setting, PlaylistController.AudioPlayType playType)
  {
    this._newSongSetting = setting;
    if ((UnityEngine.Object) this._activeAudio == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "PlaylistController prefab is not in your scene. Cannot play a song.");
    }
    else
    {
      AudioClip clipToPlay = (AudioClip) null;
      int num = playType == PlaylistController.AudioPlayType.PlayNow ? 1 : (playType == PlaylistController.AudioPlayType.AlreadyScheduled ? 1 : 0);
      if (num != 0)
        this._lastFrameSongPosition = -1;
      if (num != 0 && this._currentSong != null && !this.CanSchedule && this._currentSong.songChangedEventExpanded && this._currentSong.songChangedCustomEvent != string.Empty && this._currentSong.songChangedCustomEvent != "[None]")
        DarkTonic.MasterAudio.MasterAudio.FireCustomEvent(this._currentSong.songChangedCustomEvent, this.Trans);
      if (playType != PlaylistController.AudioPlayType.AlreadyScheduled)
      {
        if ((UnityEngine.Object) this._activeAudio.clip != (UnityEngine.Object) null)
        {
          string str = string.Empty;
          switch (setting.audLocation)
          {
            case DarkTonic.MasterAudio.MasterAudio.AudioLocation.Clip:
              if ((UnityEngine.Object) setting.clip != (UnityEngine.Object) null)
              {
                str = setting.clip.name;
                break;
              }
              break;
            case DarkTonic.MasterAudio.MasterAudio.AudioLocation.ResourceFile:
              str = setting.resourceFileName;
              break;
          }
          if (string.IsNullOrEmpty(str))
          {
            Debug.LogWarning((object) "The next song has no clip or Resource file assigned. Please fix. Ignoring song change request.");
            return;
          }
        }
        if ((UnityEngine.Object) this._activeAudio.clip == (UnityEngine.Object) null)
        {
          this._audioClip = this._activeAudio;
          this._transClip = this._transitioningAudio;
        }
        else if ((UnityEngine.Object) this._transitioningAudio.clip == (UnityEngine.Object) null)
        {
          this._audioClip = this._transitioningAudio;
          this._transClip = this._activeAudio;
        }
        else
        {
          this._audioClip = this._transitioningAudio;
          this._transClip = this._activeAudio;
        }
        this._audioClip.loop = this.SongShouldLoop(setting);
        switch (setting.audLocation)
        {
          case DarkTonic.MasterAudio.MasterAudio.AudioLocation.Clip:
            if ((UnityEngine.Object) setting.clip == (UnityEngine.Object) null)
            {
              DarkTonic.MasterAudio.MasterAudio.LogWarning($"MasterAudio will not play empty Playlist clip for PlaylistController '{this.ControllerName}'.");
              return;
            }
            clipToPlay = setting.clip;
            break;
          case DarkTonic.MasterAudio.MasterAudio.AudioLocation.ResourceFile:
            if (DarkTonic.MasterAudio.MasterAudio.HasAsyncResourceLoaderFeature() && this.ShouldLoadAsync)
            {
              this.StartCoroutine(AudioResourceOptimizer.PopulateResourceSongToPlaylistControllerAsync(setting.resourceFileName, this.CurrentPlaylist.playlistName, this, playType));
              break;
            }
            clipToPlay = AudioResourceOptimizer.PopulateResourceSongToPlaylistController(this.ControllerName, setting.resourceFileName, this.CurrentPlaylist.playlistName);
            if ((UnityEngine.Object) clipToPlay == (UnityEngine.Object) null)
              return;
            break;
        }
      }
      else
        this.FinishLoadingNewSong((AudioClip) null, PlaylistController.AudioPlayType.AlreadyScheduled);
      if (!((UnityEngine.Object) clipToPlay != (UnityEngine.Object) null))
        return;
      this.FinishLoadingNewSong(clipToPlay, playType);
    }
  }

  public double? ScheduledGaplessNextSongStartTime()
  {
    return !this._scheduledSongOffsetByAudioSource.ContainsKey(this._audioClip) ? new double?() : new double?(this._scheduledSongOffsetByAudioSource[this._audioClip]);
  }

  public void FinishLoadingNewSong(AudioClip clipToPlay, PlaylistController.AudioPlayType playType)
  {
    this._nextSongRequested = false;
    bool flag1 = playType == PlaylistController.AudioPlayType.Schedule;
    int num1 = playType == PlaylistController.AudioPlayType.PlayNow | flag1 ? 1 : 0;
    bool flag2 = playType == PlaylistController.AudioPlayType.PlayNow || playType == PlaylistController.AudioPlayType.AlreadyScheduled;
    if (num1 != 0)
    {
      this._audioClip.clip = clipToPlay;
      this._audioClip.pitch = this._newSongSetting.pitch;
    }
    if (this._currentSong != null)
    {
      this._currentSong.lastKnownTimePoint = this._activeAudio.timeSamples;
      this._currentSong.wasLastKnownTimePointSet = true;
    }
    if (flag2)
    {
      if ((double) this.CrossFadeTime == 0.0 || (UnityEngine.Object) this._transClip.clip == (UnityEngine.Object) null)
      {
        this.CeaseAudioSource(this._transClip);
        this._audioClip.volume = this._newSongSetting.volume * this.PlaylistVolume;
        if (!this.ActiveAudioSource.isPlaying && this._currentPlaylist != null && this._currentPlaylist.fadeInFirstSong && (double) this.CrossFadeTime > 0.0)
          this.CrossFadeNow(this._audioClip);
      }
      else
        this.CrossFadeNow(this._audioClip);
      this.SetDuckProperties();
    }
    switch (playType)
    {
      case PlaylistController.AudioPlayType.PlayNow:
        if ((UnityEngine.Object) this._audioClip.clip != (UnityEngine.Object) null && this._audioClip.timeSamples >= this._audioClip.clip.samples - 1)
          this._audioClip.timeSamples = 0;
        this._audioClip.Play();
        this.framesOfSongPlayed = 0;
        AudioUtil.ClipPlayed(this._activeAudio.clip, this.GameObj);
        this.CheckIfPlaylistStarted();
        ++this._songsPlayedFromPlaylist;
        break;
      case PlaylistController.AudioPlayType.Schedule:
        this.ScheduleClipPlay(this.CalculateNextTrackStartTimeOffset(), this._audioClip, false);
        this._nextSongScheduled = true;
        this.CheckIfPlaylistStarted();
        ++this._songsPlayedFromPlaylist;
        break;
      case PlaylistController.AudioPlayType.AlreadyScheduled:
        this._nextSongScheduled = false;
        this.RemoveScheduledClip();
        break;
    }
    bool flag3 = false;
    if (playType == PlaylistController.AudioPlayType.PlayNow)
    {
      PlaylistController controllerInSameSyncGroup = this.FindOtherControllerInSameSyncGroup();
      if ((UnityEngine.Object) controllerInSameSyncGroup != (UnityEngine.Object) null)
      {
        int timeSamples = controllerInSameSyncGroup._activeAudio.timeSamples;
        bool flag4 = (double) Math.Abs(this._audioClip.clip.length - controllerInSameSyncGroup._audioClip.time) >= (double) AudioUtil.FrameTime * 10.0;
        if (((!((UnityEngine.Object) this._audioClip.clip != (UnityEngine.Object) null) ? 0 : (timeSamples < this._audioClip.clip.samples ? 1 : 0)) & (flag4 ? 1 : 0)) != 0)
        {
          this._audioClip.timeSamples = timeSamples;
          flag3 = true;
        }
      }
    }
    if (this._currentPlaylist != null)
    {
      if (this._songsPlayedFromPlaylist <= 1 && !flag3)
      {
        this._audioClip.timeSamples = 0;
      }
      else
      {
        switch (this._currentPlaylist.songTransitionType)
        {
          case DarkTonic.MasterAudio.MasterAudio.SongFadeInPosition.NewClipFromBeginning:
            if (!this.ShouldNotSwitchEarly)
            {
              this._audioClip.timeSamples = 0;
              break;
            }
            break;
          case DarkTonic.MasterAudio.MasterAudio.SongFadeInPosition.NewClipFromLastKnownPosition:
            MusicSetting musicSetting = this._currentPlaylist.MusicSettings.Find((Predicate<MusicSetting>) (obj => obj == this._newSongSetting));
            if (musicSetting != null)
            {
              int num2 = musicSetting.lastKnownTimePoint;
              if ((UnityEngine.Object) this._transitioningAudio.clip != (UnityEngine.Object) null && num2 >= this._transitioningAudio.clip.samples - 1)
                num2 = 0;
              this._transitioningAudio.timeSamples = num2;
              break;
            }
            break;
          case DarkTonic.MasterAudio.MasterAudio.SongFadeInPosition.SynchronizeClips:
            if (!flag3)
            {
              if (playType == PlaylistController.AudioPlayType.PlayNow)
              {
                int num3 = this._lastSongPosition.HasValue ? this._lastSongPosition.Value : this._activeAudio.timeSamples;
                if ((UnityEngine.Object) this._transitioningAudio.clip != (UnityEngine.Object) null && num3 >= this._transitioningAudio.clip.samples - 1)
                  num3 = 0;
                this._lastSongPosition = new int?();
                this._transitioningAudio.timeSamples = num3;
                break;
              }
              if (!this.ShouldNotSwitchEarly)
              {
                this._transitioningAudio.timeSamples = 0;
                break;
              }
              break;
            }
            break;
        }
      }
      if (this._currentPlaylist.songTransitionType == DarkTonic.MasterAudio.MasterAudio.SongFadeInPosition.NewClipFromBeginning | (this._currentPlaylist.songTransitionType == DarkTonic.MasterAudio.MasterAudio.SongFadeInPosition.NewClipFromLastKnownPosition && !this._newSongSetting.wasLastKnownTimePointSet))
      {
        float songStartTime = this._newSongSetting.SongStartTime;
        if ((double) songStartTime > 0.0)
          this._audioClip.timeSamples = (int) ((double) songStartTime * (double) this._audioClip.clip.frequency);
      }
    }
    if (flag1)
      this.UpdateMasterVolume();
    if (flag2)
    {
      this._activeAudio = this._audioClip;
      this._transitioningAudio = this._transClip;
      if (this.songChangedCustomEvent != string.Empty && this.songChangedEventExpanded && this.songChangedCustomEvent != "[None]")
        DarkTonic.MasterAudio.MasterAudio.FireCustomEvent(this.songChangedCustomEvent, this.Trans);
      if (this.SongChanged != null)
      {
        string newSongName = string.Empty;
        if ((UnityEngine.Object) this._audioClip != (UnityEngine.Object) null)
          newSongName = this._audioClip.clip.name;
        this.SongChanged(newSongName);
      }
    }
    this._activeAudioEndVolume = this._newSongSetting.volume * this.PlaylistVolume;
    float volume = this._transitioningAudio.volume;
    if (this._currentSong != null)
      volume = this._currentSong.volume;
    this._transitioningAudioStartVolume = volume * this.PlaylistVolume;
    this._currentSong = this._newSongSetting;
    if (flag2 && this._currentSong.songStartedEventExpanded && this._currentSong.songStartedCustomEvent != string.Empty && this._currentSong.songStartedCustomEvent != "[None]")
      DarkTonic.MasterAudio.MasterAudio.FireCustomEvent(this._currentSong.songStartedCustomEvent, this.Trans);
    if (!this.CanSchedule || playType == PlaylistController.AudioPlayType.Schedule || this._currentSong.isLoop)
      return;
    this.ScheduleNextSong();
  }

  public void RemoveScheduledClip()
  {
    if (!((UnityEngine.Object) this._audioClip != (UnityEngine.Object) null))
      return;
    this._scheduledSongOffsetByAudioSource.Remove(this._audioClip);
  }

  public void ScheduleNextSong()
  {
    this.PlayNextOrRandom(PlaylistController.AudioPlayType.Schedule);
  }

  public void FadeInScheduledSong()
  {
    this.PlayNextOrRandom(PlaylistController.AudioPlayType.AlreadyScheduled);
  }

  public double CalculateNextTrackStartTimeOffset()
  {
    PlaylistController controllerInSameSyncGroup = this.FindOtherControllerInSameSyncGroup();
    if ((UnityEngine.Object) controllerInSameSyncGroup != (UnityEngine.Object) null)
    {
      double? nullable = controllerInSameSyncGroup.ScheduledGaplessNextSongStartTime();
      if (nullable.HasValue)
        return nullable.Value;
    }
    return this.GetClipDuration(this._activeAudio);
  }

  public double GetClipDuration(AudioSource src)
  {
    return (double) AudioUtil.AdjustAudioClipDurationForPitch(src.clip.length - src.time, src) - (double) this.CrossFadeTime;
  }

  public void ScheduleClipPlay(
    double scheduledPlayTimeOffset,
    AudioSource source,
    bool calledAfterPause)
  {
    double time = AudioSettings.dspTime + scheduledPlayTimeOffset;
    if (this.ShouldNotSwitchEarly && this._currentSchedSongDspEndTime.HasValue && !calledAfterPause)
    {
      time = this._currentSchedSongDspEndTime.Value;
      scheduledPlayTimeOffset = time - AudioSettings.dspTime;
    }
    source.PlayScheduled(time);
    this._currentSchedSongDspStartTime = new double?(time);
    this._currentSchedSongDspEndTime = new double?(time + this.GetClipDuration(source));
    this.RemoveScheduledClip();
    this._scheduledSongOffsetByAudioSource.Add(source, scheduledPlayTimeOffset);
  }

  public void CrossFadeNow(AudioSource audioClip)
  {
    audioClip.volume = 0.0f;
    this.IsCrossFading = true;
    this._duckingMode = PlaylistController.AudioDuckingMode.NotDucking;
    this._crossFadeStartTime = AudioUtil.Time;
    if (!this.crossfadeStartedExpanded || !(this.crossfadeStartedCustomEvent != string.Empty) || !(this.crossfadeStartedCustomEvent != "[None]"))
      return;
    DarkTonic.MasterAudio.MasterAudio.FireCustomEvent(this.crossfadeStartedCustomEvent, this.Trans, false);
  }

  public void CeaseAudioSource(AudioSource source)
  {
    if ((UnityEngine.Object) source == (UnityEngine.Object) null)
      return;
    if ((UnityEngine.Object) source == (UnityEngine.Object) this._activeAudio)
      this.framesOfSongPlayed = 0;
    int num = (UnityEngine.Object) source.clip != (UnityEngine.Object) null ? 1 : 0;
    string songName = (UnityEngine.Object) source.clip == (UnityEngine.Object) null ? string.Empty : source.clip.name;
    source.Stop();
    AudioUtil.UnloadNonPreloadedAudioData(source.clip, this.GameObj);
    AudioResourceOptimizer.UnloadPlaylistSongIfUnused(this.ControllerName, source.clip);
    source.clip = (AudioClip) null;
    this.RemoveScheduledClip();
    if (num == 0)
      return;
    if (!string.IsNullOrEmpty(songName) && this.songEndedEventExpanded && this.songEndedCustomEvent != string.Empty && this.songEndedCustomEvent != "[None]")
      DarkTonic.MasterAudio.MasterAudio.FireCustomEvent(this.songEndedCustomEvent, this.Trans, false);
    if (this.SongEnded == null || string.IsNullOrEmpty(songName))
      return;
    this.SongEnded(songName);
  }

  public void SetDuckProperties()
  {
    this._originalMusicVolume = (UnityEngine.Object) this._activeAudio == (UnityEngine.Object) null ? 1f : this._activeAudio.volume;
    if (this._currentSong != null)
      this._originalMusicVolume = this._currentSong.volume * this.PlaylistVolume;
    float floatVolumeFromDb = AudioUtil.GetFloatVolumeFromDb(AudioUtil.GetDbFromFloatVolume(this._originalMusicVolume) - this._currentDuckVolCut);
    this._duckRange = this._originalMusicVolume - floatVolumeFromDb;
    this._initialDuckVolume = floatVolumeFromDb;
    this._duckingMode = PlaylistController.AudioDuckingMode.NotDucking;
  }

  public void AudioDucking()
  {
    switch (this._duckingMode)
    {
      case PlaylistController.AudioDuckingMode.SetToDuck:
        this._activeAudio.volume = this._initialDuckVolume;
        this._duckingMode = PlaylistController.AudioDuckingMode.Ducked;
        break;
      case PlaylistController.AudioDuckingMode.Ducked:
        if ((double) Time.realtimeSinceStartup >= (double) this._timeToFinishUnducking)
        {
          this._activeAudio.volume = this._originalMusicVolume;
          this._duckingMode = PlaylistController.AudioDuckingMode.NotDucking;
          break;
        }
        if ((double) Time.realtimeSinceStartup < (double) this._timeToStartUnducking)
          break;
        this._activeAudio.volume = this._initialDuckVolume + (float) (((double) Time.realtimeSinceStartup - (double) this._timeToStartUnducking) / ((double) this._timeToFinishUnducking - (double) this._timeToStartUnducking)) * this._duckRange;
        break;
    }
  }

  public bool SongIsNonAdvancible
  {
    get
    {
      return this.CurrentPlaylist != null && this.CurrentPlaylist.songTransitionType == DarkTonic.MasterAudio.MasterAudio.SongFadeInPosition.SynchronizeClips && (double) this.CrossFadeTime > 0.0;
    }
  }

  public bool SongShouldLoop(MusicSetting setting)
  {
    if (this._queuedSongs.Count > 0)
      return false;
    return this.SongIsNonAdvancible || setting.isLoop;
  }

  public bool ShouldLoadAsync
  {
    get
    {
      return DarkTonic.MasterAudio.MasterAudio.Instance.resourceClipsAllLoadAsync || this.CurrentPlaylist.resourceClipsAllLoadAsync;
    }
  }

  public bool ControllerIsReady
  {
    get => this.\u003CControllerIsReady\u003Ek__BackingField;
    set => this.\u003CControllerIsReady\u003Ek__BackingField = value;
  }

  public PlaylistController.PlaylistStates PlaylistState
  {
    get
    {
      if ((UnityEngine.Object) this._activeAudio == (UnityEngine.Object) null || (UnityEngine.Object) this._transitioningAudio == (UnityEngine.Object) null)
        return PlaylistController.PlaylistStates.NotInScene;
      return !this.ActiveAudioSource.isPlaying ? ((double) this.ActiveAudioSource.time != 0.0 ? PlaylistController.PlaylistStates.Paused : PlaylistController.PlaylistStates.Stopped) : (this.IsCrossFading ? PlaylistController.PlaylistStates.Crossfading : PlaylistController.PlaylistStates.Playing);
    }
  }

  public AudioSource ActiveAudioSource
  {
    get
    {
      return (UnityEngine.Object) this._activeAudio != (UnityEngine.Object) null && (UnityEngine.Object) this._activeAudio.clip == (UnityEngine.Object) null ? this._transitioningAudio : this._activeAudio;
    }
  }

  public static List<PlaylistController> Instances
  {
    get
    {
      if (PlaylistController._instances != null)
        return PlaylistController._instances;
      PlaylistController._instances = new List<PlaylistController>();
      foreach (UnityEngine.Object @object in UnityEngine.Object.FindObjectsOfType(typeof (PlaylistController)))
        PlaylistController._instances.Add(@object as PlaylistController);
      return PlaylistController._instances;
    }
    set => PlaylistController._instances = value;
  }

  public GameObject PlaylistControllerGameObject => this._go;

  public AudioSource CurrentPlaylistSource => this._activeAudio;

  public AudioClip CurrentPlaylistClip
  {
    get => (UnityEngine.Object) this._activeAudio == (UnityEngine.Object) null ? (AudioClip) null : this._activeAudio.clip;
  }

  public AudioClip FadingPlaylistClip
  {
    get
    {
      if (!this.IsCrossFading)
        return (AudioClip) null;
      return (UnityEngine.Object) this._transitioningAudio == (UnityEngine.Object) null ? (AudioClip) null : this._transitioningAudio.clip;
    }
  }

  public AudioSource FadingSource
  {
    get => !this.IsCrossFading ? (AudioSource) null : this._transitioningAudio;
  }

  public bool IsCrossFading
  {
    get => this.\u003CIsCrossFading\u003Ek__BackingField;
    set => this.\u003CIsCrossFading\u003Ek__BackingField = value;
  }

  public bool IsFading => this.IsCrossFading || this._curFadeMode != 0;

  public float PlaylistVolume
  {
    get => DarkTonic.MasterAudio.MasterAudio.PlaylistMasterVolume * this._playlistVolume;
    set
    {
      this._playlistVolume = value;
      this.UpdateMasterVolume();
    }
  }

  public void RouteToMixerChannel(AudioMixerGroup group)
  {
    this._activeAudio.outputAudioMixerGroup = group;
    this._transitioningAudio.outputAudioMixerGroup = group;
  }

  public DarkTonic.MasterAudio.MasterAudio.Playlist CurrentPlaylist
  {
    get
    {
      if (this._currentPlaylist != null || (double) Time.realtimeSinceStartup - (double) this._lastTimeMissingPlaylistLogged <= 2.0)
        return this._currentPlaylist;
      Debug.LogWarning((object) "Current Playlist is NULL. Subsequent calls will fail.");
      this._lastTimeMissingPlaylistLogged = AudioUtil.Time;
      return this._currentPlaylist;
    }
  }

  public bool HasPlaylist => this._currentPlaylist != null;

  public string PlaylistName
  {
    get => this.CurrentPlaylist == null ? string.Empty : this.CurrentPlaylist.playlistName;
  }

  public bool IsMuted => this.isMuted;

  public bool PlaylistIsMuted
  {
    set
    {
      this.isMuted = value;
      if (Application.isPlaying)
      {
        if ((UnityEngine.Object) this._activeAudio != (UnityEngine.Object) null)
          this._activeAudio.mute = value;
        if (!((UnityEngine.Object) this._transitioningAudio != (UnityEngine.Object) null))
          return;
        this._transitioningAudio.mute = value;
      }
      else
      {
        foreach (AudioSource component in this.GetComponents<AudioSource>())
          component.mute = value;
      }
    }
  }

  public float CrossFadeTime
  {
    get
    {
      return this._currentPlaylist != null && this._currentPlaylist.crossfadeMode != DarkTonic.MasterAudio.MasterAudio.Playlist.CrossfadeTimeMode.UseMasterSetting ? this._currentPlaylist.crossFadeTime : DarkTonic.MasterAudio.MasterAudio.Instance.MasterCrossFadeTime;
    }
  }

  public bool IsAutoAdvance => !this.SongIsNonAdvancible && this.isAutoAdvance;

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

  public string ControllerName
  {
    get
    {
      if (this._name != null)
        return this._name;
      this._name = this.GameObj.name;
      return this._name;
    }
  }

  public bool CanSchedule => DarkTonic.MasterAudio.MasterAudio.Instance.useGaplessPlaylists && this.IsAutoAdvance;

  public bool IsFrameFastEnough => (double) AudioUtil.FrameTime < 0.30000001192092896;

  public bool ShouldNotSwitchEarly => (double) this.CrossFadeTime <= 0.0 && this.CanSchedule;

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

  public int ClipsRemainingInCurrentPlaylist => this._clipsRemaining.Count;

  [CompilerGenerated]
  public bool \u003CFindOtherControllerInSameSyncGroup\u003Eb__136_0(PlaylistController obj)
  {
    return (UnityEngine.Object) obj != (UnityEngine.Object) this && obj.syncGroupNum == this.syncGroupNum && (UnityEngine.Object) obj.ActiveAudioSource != (UnityEngine.Object) null && obj.ActiveAudioSource.isPlaying;
  }

  [CompilerGenerated]
  public bool \u003CFinishLoadingNewSong\u003Eb__144_0(MusicSetting obj)
  {
    return obj == this._newSongSetting;
  }

  public enum AudioPlayType
  {
    PlayNow,
    Schedule,
    AlreadyScheduled,
  }

  public enum PlaylistStates
  {
    NotInScene,
    Stopped,
    Playing,
    Paused,
    Crossfading,
  }

  public enum FadeMode
  {
    None,
    GradualFade,
  }

  public enum AudioDuckingMode
  {
    NotDucking,
    SetToDuck,
    Ducked,
  }

  public delegate void SongChangedEventHandler(string newSongName);

  public delegate void SongEndedEventHandler(string songName);

  public delegate void SongLoopedEventHandler(string songName);
}
