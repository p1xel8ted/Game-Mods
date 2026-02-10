// Decompiled with JetBrains decompiler
// Type: AudioManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using AOT;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using MMRoomGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WebSocketSharp;

#nullable disable
public class AudioManager : BaseMonoBehaviour
{
  public static AudioManager _instance;
  [SerializeField]
  public Transform listener;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float listenerPosBetweenCameraAndTarget;
  public EventInstance invalidEventInstance;
  public bool fmodBanksLoaded;
  public EventInstance currentMusicInstance;
  public string queuedMusicPath = string.Empty;
  public EventInstance AtmosInstance;
  public EventInstance masterBusVolumeSnapshot;
  public EventInstance sfxBusVolumeSnapshot;
  public EventInstance musicBusVolumeSnapshot;
  public EventInstance voBusVolumeSnapshot;
  public EventInstance buildMenuSnapshot;
  public Bus SFXPauseBus;
  public Bus VOPauseBus;
  public Bus StingersPauseBus;
  public Bus FreezeTimeSFXBus;
  public EVENT_CALLBACK oneShotCleanupCallback;
  [SerializeField]
  public GameManager gameManager;
  public bool SetFilter;
  public List<EventInstance> InstanceList = new List<EventInstance>();
  public AudioManager.AudioAction OnCreateLoop;
  public AudioManager.AudioAction OnReleaseLoop;
  public List<EventInstance> activeLoops = new List<EventInstance>();
  public Dictionary<string, EventInstance> cachedLoops = new Dictionary<string, EventInstance>();
  public Dictionary<EventInstance, int> cachedLoopsCounter = new Dictionary<EventInstance, int>();
  public Dictionary<string, EventInstance> oneShotEventInstances = new Dictionary<string, EventInstance>();
  public string playerFootstepOverride = string.Empty;
  public string footstepOverride = string.Empty;

  public static AudioManager Instance
  {
    get
    {
      if ((UnityEngine.Object) AudioManager._instance == (UnityEngine.Object) null)
        AudioManager._instance = (UnityEngine.Object.Instantiate(Resources.Load("MMAudio/AudioManager")) as GameObject).GetComponent<AudioManager>();
      return AudioManager._instance;
    }
  }

  public GameObject Listener => this.listener.gameObject;

  public EventInstance CurrentMusicInstance => this.currentMusicInstance;

  public void Awake()
  {
    if ((UnityEngine.Object) AudioManager._instance != (UnityEngine.Object) null && (UnityEngine.Object) AudioManager._instance != (UnityEngine.Object) this)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
    {
      AudioManager._instance = this;
      if ((UnityEngine.Object) this.transform.parent != (UnityEngine.Object) null)
        this.transform.SetParent((Transform) null);
      UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);
    }
  }

  public IEnumerator Start()
  {
    while (!RuntimeManager.HaveAllBanksLoaded)
    {
      yield return (object) 0;
      UnityEngine.Debug.Log((object) "FMOD All Banks not loaded");
    }
    this.gameManager = GameManager.GetInstance();
    this.fmodBanksLoaded = true;
    if (this.queuedMusicPath != string.Empty)
    {
      this.PlayMusic(this.queuedMusicPath);
      this.queuedMusicPath = string.Empty;
    }
    this.masterBusVolumeSnapshot = RuntimeManager.CreateInstance("snapshot:/master_bus");
    int num1 = (int) this.masterBusVolumeSnapshot.start();
    this.sfxBusVolumeSnapshot = RuntimeManager.CreateInstance("snapshot:/sfx_bus");
    int num2 = (int) this.sfxBusVolumeSnapshot.start();
    this.musicBusVolumeSnapshot = RuntimeManager.CreateInstance("snapshot:/music_bus");
    int num3 = (int) this.musicBusVolumeSnapshot.start();
    this.voBusVolumeSnapshot = RuntimeManager.CreateInstance("snapshot:/vo_bus");
    int num4 = (int) this.voBusVolumeSnapshot.start();
    this.buildMenuSnapshot = RuntimeManager.CreateInstance("snapshot:/build_mode");
    if (SettingsManager.Settings != null)
    {
      this.SetMasterBusVolume(SettingsManager.Settings.Audio.MasterVolume);
      this.SetMusicBusVolume(SettingsManager.Settings.Audio.MusicVolume);
      this.SetSFXBusVolume(SettingsManager.Settings.Audio.SFXVolume);
      this.SetVOBusVolume(SettingsManager.Settings.Audio.VOVolume);
    }
    this.SFXPauseBus = RuntimeManager.GetBus("bus:/SFXBus/paused_by_pause_menu");
    this.VOPauseBus = RuntimeManager.GetBus("bus:/VOBus/paused_by_pause_menu");
    this.StingersPauseBus = RuntimeManager.GetBus("bus:/MusicBus/Stings/paused_by_pause_menu");
    this.FreezeTimeSFXBus = RuntimeManager.GetBus("bus:/SFXBus/paused_by_pause_menu/affected_by_loading_screen/affected_by_freeze");
    this.oneShotCleanupCallback = new EVENT_CALLBACK(AudioManager.OneShotInstanceCleanup);
  }

  public void Update()
  {
    if ((UnityEngine.Object) GameManager.GetInstance() == (UnityEngine.Object) null || !((UnityEngine.Object) GameManager.GetInstance().CamFollowTarget != (UnityEngine.Object) null))
      return;
    if (GameManager.GetInstance().CamFollowTarget.enabled)
    {
      this.listener.position = Vector3.Lerp(GameManager.GetInstance().CamFollowTarget.transform.position, GameManager.GetInstance().CamFollowTarget.targetPosition, this.listenerPosBetweenCameraAndTarget);
    }
    else
    {
      Transform transform = GameManager.GetInstance().CamFollowTarget.transform;
      this.listener.position = Vector3.Lerp(transform.position, transform.position + transform.forward, this.listenerPosBetweenCameraAndTarget);
    }
  }

  public void SetMusicFilter(string SoundParam, float value)
  {
    int num = (int) RuntimeManager.StudioSystem.setParameterByName(SoundParam, value);
  }

  public void SetMusicPitch(float value)
  {
    int num = (int) this.currentMusicInstance.setPitch(value);
  }

  public void ToggleFilter(string SoundParam, bool toggle)
  {
    if (toggle)
    {
      int num1 = (int) RuntimeManager.StudioSystem.setParameterByName(SoundParam, 1f);
    }
    else
    {
      int num2 = (int) RuntimeManager.StudioSystem.setParameterByName(SoundParam, 0.0f);
    }
  }

  public void ToggleFilter(string SoundParam, bool toggle, float delay)
  {
    this.StartCoroutine((IEnumerator) this.ToggleFilterDelay(SoundParam, toggle, delay));
  }

  public IEnumerator ToggleFilterDelay(string SoundParam, bool toggle, float delay)
  {
    yield return (object) new WaitForSecondsRealtime(delay);
    this.ToggleFilter(SoundParam, toggle);
  }

  public void SetGameManager(GameManager gm) => this.gameManager = gm;

  public void SetBuildSnapshotEnabled(bool state)
  {
    if (this.buildMenuSnapshot.isValid())
    {
      if (state)
      {
        int num1 = (int) this.buildMenuSnapshot.start();
      }
      else
      {
        int num2 = (int) this.buildMenuSnapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
      }
    }
    else
      UnityEngine.Debug.Log((object) "FMOD error: EventInstance reference to 'snapshot:/build_mode' is not valid");
  }

  public bool IsSoundPathValid(string soundPath)
  {
    try
    {
      if (string.IsNullOrEmpty(soundPath))
        return false;
      if (!this.fmodBanksLoaded)
      {
        UnityEngine.Debug.LogWarning((object) $"FMOD cannot play event '{soundPath}' because banks have not loaded yet");
        return false;
      }
      EventDescription _event;
      int num = (int) RuntimeManager.StudioSystem.getEvent(soundPath, out _event);
      if (_event.isValid())
        return true;
      UnityEngine.Debug.LogWarning((object) $"FMOD cannot find event '{soundPath}'");
      return false;
    }
    catch (Exception ex)
    {
      return false;
    }
  }

  public bool CurrentEventInstanceIsPlayingPath(EventInstance currentInstance, string soundPath)
  {
    if (currentInstance.isValid())
    {
      PLAYBACK_STATE state;
      int playbackState = (int) currentInstance.getPlaybackState(out state);
      if (state == PLAYBACK_STATE.STOPPED || state == PLAYBACK_STATE.STOPPING)
        return false;
      EventDescription description1;
      int description2 = (int) currentInstance.getDescription(out description1);
      if (description1.isValid())
      {
        string path1;
        int path2 = (int) description1.getPath(out path1);
        if (path1 == soundPath)
          return true;
      }
    }
    return false;
  }

  public bool CurrentEventInstanceIsPlaying(EventInstance currentInstance)
  {
    if (!currentInstance.isValid())
      return false;
    PLAYBACK_STATE state;
    int playbackState = (int) currentInstance.getPlaybackState(out state);
    return state == PLAYBACK_STATE.PLAYING;
  }

  public bool CurrentEventIsPlaying(string eventPath)
  {
    EventDescription _event;
    int num = (int) RuntimeManager.StudioSystem.getEvent(eventPath, out _event);
    if (!_event.isValid())
      return false;
    int count = 0;
    int instanceCount = (int) _event.getInstanceCount(out count);
    return count > 0;
  }

  public void PlayMusic(string soundPath, bool StartMusic = true)
  {
    if (!this.fmodBanksLoaded)
    {
      this.queuedMusicPath = soundPath;
    }
    else
    {
      if (!this.IsSoundPathValid(soundPath))
        return;
      if (this.currentMusicInstance.isValid())
      {
        if (this.CurrentEventInstanceIsPlayingPath(this.currentMusicInstance, soundPath))
        {
          UnityEngine.Debug.Log((object) $"PK: {soundPath} is already playing");
          return;
        }
        this.StopCurrentMusic();
      }
      this.currentMusicInstance = this.CreateLoop(soundPath, StartMusic, false);
    }
  }

  public void PlayLoop(EventInstance instance)
  {
    if (instance.isValid())
    {
      int num = (int) instance.start();
    }
    else
      UnityEngine.Debug.Log((object) "Couldn't start instance");
  }

  public bool IsEventInstancePlaying(EventInstance loop)
  {
    if (!loop.isValid())
      return false;
    PLAYBACK_STATE state;
    int playbackState = (int) loop.getPlaybackState(out state);
    return state == PLAYBACK_STATE.PLAYING;
  }

  public void StartMusic()
  {
    if (this.currentMusicInstance.isValid())
    {
      int num = (int) this.currentMusicInstance.start();
    }
    else
      UnityEngine.Debug.Log((object) "Couldn't start music");
  }

  public void PlayCurrentAtmos()
  {
    if (!this.AtmosInstance.isValid())
      return;
    int num = (int) this.AtmosInstance.start();
  }

  public void PlayAtmos(string soundPath)
  {
    if (!this.fmodBanksLoaded)
    {
      this.queuedMusicPath = soundPath;
    }
    else
    {
      if (!this.IsSoundPathValid(soundPath))
        return;
      if (this.AtmosInstance.isValid())
      {
        if (this.CurrentEventInstanceIsPlayingPath(this.AtmosInstance, soundPath))
          return;
        this.StopCurrentAtmos();
      }
      this.AtmosInstance = this.CreateLoop(soundPath, true, false);
    }
  }

  public void AdjustAtmosParameter(string parameter, float value)
  {
    if (this.AtmosInstance.isValid())
      this.SetEventInstanceParameter(this.AtmosInstance, parameter, value);
    else
      UnityEngine.Debug.Log((object) "AtmosInstance not valid");
  }

  public void StopCurrentAtmos(bool AllowFadeOut = true)
  {
    if (!this.AtmosInstance.isValid())
      return;
    if (AllowFadeOut)
      this.StopLoop(this.AtmosInstance);
    else
      this.StopLoop(this.AtmosInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
  }

  public void SetMusicRoomID(SoundConstants.RoomID roomID, bool awaitPlaybackState = false)
  {
    if (!this.currentMusicInstance.isValid())
      return;
    if (roomID == SoundConstants.RoomID.NoMusic)
      this.StopCurrentMusic();
    else
      this.SetEventInstanceParameter(this.currentMusicInstance, SoundParams.RoomID, (float) roomID, awaitPlaybackState);
  }

  public void SetMusicRoomID(int roomID, string Parameter)
  {
    if (this.currentMusicInstance.isValid())
    {
      if (roomID == 9999)
        this.StopCurrentMusic();
      else
        this.SetEventInstanceParameter(this.currentMusicInstance, Parameter, (float) roomID);
    }
    else
      UnityEngine.Debug.Log((object) "currentMusicInstance is not valid");
  }

  public void SetMusicBaseID(SoundConstants.BaseID baseID)
  {
    if (!this.currentMusicInstance.isValid())
      return;
    UnityEngine.Debug.Log((object) ("PK: Set base ID to " + baseID.ToString()));
    if (baseID == SoundConstants.BaseID.NoMusic)
      this.StopLoop(this.currentMusicInstance);
    else
      this.SetEventInstanceParameter(this.currentMusicInstance, SoundParams.BaseID, (float) baseID);
  }

  public void SetFlockadeGameState(SoundConstants.FlockadeGameState state)
  {
    if (!this.currentMusicInstance.isValid())
      return;
    this.SetEventInstanceParameter(this.currentMusicInstance, SoundParams.FlockadeGameState, (float) state);
  }

  public void SetFollowersDance(float value)
  {
    this.SetEventInstanceParameter(this.currentMusicInstance, SoundParams.FollowersDance, value);
  }

  public void SetFollowersSing(float value)
  {
    this.SetEventInstanceParameter(this.currentMusicInstance, SoundParams.FollowersSing, value);
  }

  public void SetMusicPsychedelic(float value)
  {
    this.SetEventInstanceParameter(this.currentMusicInstance, SoundParams.Psychedelic, value);
  }

  public void SetMusicParam(string param, float value)
  {
    this.SetEventInstanceParameter(this.currentMusicInstance, param, value);
  }

  public void StopCurrentMusic()
  {
    if (!this.currentMusicInstance.isValid())
      return;
    this.StopLoop(this.currentMusicInstance);
  }

  public void SetMusicCombatState(bool active = true)
  {
    int num = (int) RuntimeManager.StudioSystem.setParameterByName(SoundParams.Combat, active ? 1f : 0.0f);
  }

  public void PlayOneShot(string soundPath)
  {
    if (!this.IsSoundPathValid(soundPath))
      return;
    try
    {
      RuntimeManager.PlayOneShot(soundPath);
    }
    catch (EventNotFoundException ex)
    {
      RuntimeUtils.DebugLogWarning("[FMOD] Event not found: " + soundPath);
    }
  }

  public void PlayOneShot(string soundPath, int startTime)
  {
    if (!this.IsSoundPathValid(soundPath))
      return;
    try
    {
      string path = soundPath;
      int num = startTime;
      Vector3 position = new Vector3();
      int startTime1 = num;
      RuntimeManager.PlayOneShot(path, position, startTime1);
    }
    catch (EventNotFoundException ex)
    {
      RuntimeUtils.DebugLogWarning("[FMOD] Event not found: " + soundPath);
    }
  }

  public void PlayOneShotDelayed(string soundPath, float delay, Transform t)
  {
    if (!this.IsSoundPathValid(soundPath))
      return;
    this.StartCoroutine((IEnumerator) this.OneShotDelayedTransform(soundPath, delay, t));
  }

  public void PlayOneShotDelayed(string soundPath, float delay)
  {
    if (!this.IsSoundPathValid(soundPath))
      return;
    this.StartCoroutine((IEnumerator) this.OneShotDelayed(soundPath, delay));
  }

  public IEnumerator OneShotDelayedTransform(string soundPath, float delay, Transform t = null)
  {
    yield return (object) new WaitForSecondsRealtime(delay);
    try
    {
      RuntimeManager.PlayOneShot(soundPath, t.position);
    }
    catch (EventNotFoundException ex)
    {
      RuntimeUtils.DebugLogWarning("[FMOD] Event not found: " + soundPath);
    }
  }

  public IEnumerator OneShotDelayed(string soundPath, float delay)
  {
    yield return (object) new WaitForSecondsRealtime(delay);
    try
    {
      RuntimeManager.PlayOneShot(soundPath);
    }
    catch (EventNotFoundException ex)
    {
      RuntimeUtils.DebugLogWarning("[FMOD] Event not found: " + soundPath);
    }
  }

  public void PlayOneShot(string soundPath, Vector3 pos)
  {
    if (!this.IsSoundPathValid(soundPath))
      return;
    try
    {
      RuntimeManager.PlayOneShot(soundPath, pos);
    }
    catch (EventNotFoundException ex)
    {
      RuntimeUtils.DebugLogWarning("[FMOD] Event not found: " + soundPath);
    }
  }

  public EventInstance PlayOneShotWithInstance(string soundPath)
  {
    if (!this.IsSoundPathValid(soundPath))
      return this.invalidEventInstance;
    try
    {
      EventInstance instance = RuntimeManager.CreateInstance(soundPath);
      if (!instance.isValid())
        return this.invalidEventInstance;
      GCHandle gcHandle = GCHandle.Alloc((object) new FMODOneShotWrapper(instance));
      int num1 = (int) instance.setUserData(GCHandle.ToIntPtr(gcHandle));
      int num2 = (int) instance.start();
      return instance;
    }
    catch (EventNotFoundException ex)
    {
      RuntimeUtils.DebugLogWarning("[FMOD] Event not found: " + soundPath);
      return new EventInstance();
    }
  }

  public EventInstance PlayOneShotWithInstanceCleanup(
    string soundPath,
    Transform transform,
    bool attachToObject = true)
  {
    if (!this.IsSoundPathValid(soundPath))
      return this.invalidEventInstance;
    try
    {
      EventInstance instance = RuntimeManager.CreateInstance(soundPath);
      if (!instance.isValid())
        return this.invalidEventInstance;
      if ((UnityEngine.Object) transform != (UnityEngine.Object) null)
      {
        if (attachToObject)
        {
          RuntimeManager.AttachInstanceToGameObject(instance, transform);
        }
        else
        {
          int num1 = (int) instance.set3DAttributes(transform.To3DAttributes());
        }
      }
      GCHandle gcHandle = GCHandle.Alloc((object) new FMODOneShotWrapper(instance));
      int num2 = (int) instance.setUserData(GCHandle.ToIntPtr(gcHandle));
      int num3 = (int) instance.setCallback(this.oneShotCleanupCallback, EVENT_CALLBACK_TYPE.STOPPED);
      int num4 = (int) instance.start();
      return instance;
    }
    catch (EventNotFoundException ex)
    {
      RuntimeUtils.DebugLogWarning("[FMOD] Event not found: " + soundPath);
      return this.invalidEventInstance;
    }
  }

  public EventInstance PlayOneShotWithInstance(string soundPath, int startTime)
  {
    if (!this.IsSoundPathValid(soundPath))
      return this.invalidEventInstance;
    try
    {
      EventInstance instance = RuntimeManager.CreateInstance(soundPath);
      if (!instance.isValid())
        return this.invalidEventInstance;
      GCHandle gcHandle = GCHandle.Alloc((object) new FMODOneShotWrapper(instance));
      int num1 = (int) instance.setUserData(GCHandle.ToIntPtr(gcHandle));
      int num2 = (int) instance.start();
      int num3 = (int) instance.setTimelinePosition(startTime);
      return instance;
    }
    catch (EventNotFoundException ex)
    {
      RuntimeUtils.DebugLogWarning("[FMOD] Event not found: " + soundPath);
      return new EventInstance();
    }
  }

  public void StopOneShotInstanceEarly(EventInstance instance, FMOD.Studio.STOP_MODE stopMode)
  {
    try
    {
      if (!instance.isValid())
        return;
      IntPtr userdata;
      if (instance.getUserData(out userdata) == RESULT.OK && userdata != IntPtr.Zero)
      {
        GCHandle gcHandle = GCHandle.FromIntPtr(userdata);
        if (!(gcHandle.Target is FMODOneShotWrapper target))
          return;
        target.Stop(stopMode);
        gcHandle.Free();
        int num = (int) instance.setUserData(IntPtr.Zero);
      }
      else
      {
        int num1 = (int) instance.stop(stopMode);
        int num2 = (int) instance.release();
      }
    }
    catch (Exception ex)
    {
      UnityEngine.Debug.LogError((object) $"[FMOD] Failed to stop one-shot: {ex}");
    }
  }

  [MonoPInvokeCallback(typeof (EVENT_CALLBACK))]
  public static RESULT OneShotInstanceCleanup(
    EVENT_CALLBACK_TYPE type,
    IntPtr eventPtr,
    IntPtr parameterPtr)
  {
    if (type == EVENT_CALLBACK_TYPE.STOPPED)
    {
      EventInstance eventInstance = new EventInstance(eventPtr);
      IntPtr userdata;
      if (!eventInstance.isValid() || eventInstance.getUserData(out userdata) != RESULT.OK || !(userdata != IntPtr.Zero))
        return RESULT.OK;
      GCHandle gcHandle = GCHandle.FromIntPtr(userdata);
      if (gcHandle.Target is FMODOneShotWrapper target)
      {
        target.Cleanup();
        gcHandle.Free();
        int num = (int) eventInstance.setUserData(IntPtr.Zero);
      }
    }
    return RESULT.OK;
  }

  public EventInstance PlayOneShotWithInstanceDontStart(string soundPath, bool addToActiveLoops = true)
  {
    if (!this.IsSoundPathValid(soundPath))
      return this.invalidEventInstance;
    try
    {
      EventInstance instance;
      this.CreateLoopInternal(soundPath, out instance);
      if (!instance.isValid())
        return this.invalidEventInstance;
      if (addToActiveLoops && !this.activeLoops.Contains(instance))
        this.activeLoops.Add(instance);
      int num1 = (int) instance.set3DAttributes(this.gameObject.transform.To3DAttributes());
      GCHandle gcHandle = GCHandle.Alloc((object) new FMODOneShotWrapper(instance));
      int num2 = (int) instance.setUserData(GCHandle.ToIntPtr(gcHandle));
      return instance;
    }
    catch (EventNotFoundException ex)
    {
      RuntimeUtils.DebugLogWarning("[FMOD] Event not found: " + soundPath);
      return new EventInstance();
    }
  }

  public bool IsInstanceInActiveLoops(EventInstance instance)
  {
    return this.activeLoops.Contains(instance);
  }

  public void StopOneShotDelay(EventInstance _event, float _delay)
  {
    this.StartCoroutine((IEnumerator) this.StopOneShotDelayRoutine(_event, _delay));
  }

  public IEnumerator StopOneShotDelayRoutine(EventInstance _event, float _delay)
  {
    yield return (object) new WaitForSeconds(_delay);
    int num = (int) _event.release();
  }

  public void PlayOneShot(string soundPath, GameObject go)
  {
    if (!this.IsSoundPathValid(soundPath))
      return;
    try
    {
      RuntimeManager.PlayOneShotAttached(soundPath, go);
    }
    catch (Exception ex)
    {
      UnityEngine.Debug.Log((object) ex);
    }
  }

  public EventInstance CreateLoop(string soundPath, bool playLoop = false, bool addToActiveLoops = true)
  {
    if (!this.IsSoundPathValid(soundPath))
      return this.invalidEventInstance;
    EventInstance instance;
    this.CreateLoopInternal(soundPath, out instance);
    if (addToActiveLoops && !this.activeLoops.Contains(instance))
      this.activeLoops.Add(instance);
    AudioManager.AudioAction onCreateLoop = this.OnCreateLoop;
    if (onCreateLoop != null)
      onCreateLoop(instance);
    if (instance.isValid())
    {
      int num1 = (int) instance.set3DAttributes(this.gameObject.transform.To3DAttributes());
    }
    if (playLoop)
    {
      int num2 = (int) instance.start();
    }
    return instance;
  }

  public void PauseActiveLoopsAndSFX()
  {
    this.PauseActiveLoops();
    this.PauseSFXVOBusses();
  }

  public void ResumePausedLoopsAndSFX()
  {
    this.ResumeActiveLoops();
    this.ResumeSFXVOBusses();
  }

  public void StopActiveLoopsAndSFX()
  {
    this.StopActiveLoops();
    this.StopPausedSFXVOBusses();
  }

  public void PauseActiveLoops()
  {
    foreach (EventInstance activeLoop in this.activeLoops)
    {
      int num = (int) activeLoop.setPaused(true);
    }
  }

  public void PauseSFXVOBusses()
  {
    if (this.SFXPauseBus.isValid())
    {
      int num1 = (int) this.SFXPauseBus.setPaused(true);
    }
    if (this.VOPauseBus.isValid())
    {
      int num2 = (int) this.VOPauseBus.setPaused(true);
    }
    if (!this.StingersPauseBus.isValid())
      return;
    int num3 = (int) this.StingersPauseBus.setPaused(true);
  }

  public void ResumeSFXVOBusses()
  {
    if (this.SFXPauseBus.isValid())
    {
      int num1 = (int) this.SFXPauseBus.setPaused(false);
    }
    if (this.VOPauseBus.isValid())
    {
      int num2 = (int) this.VOPauseBus.setPaused(false);
    }
    if (!this.StingersPauseBus.isValid())
      return;
    int num3 = (int) this.StingersPauseBus.setPaused(false);
  }

  public void PauseForFreezeTime()
  {
    if (!this.FreezeTimeSFXBus.isValid())
      return;
    int num = (int) this.FreezeTimeSFXBus.setPaused(true);
  }

  public void ResumeForFreezeTime()
  {
    if (!this.FreezeTimeSFXBus.isValid())
      return;
    int num = (int) this.FreezeTimeSFXBus.setPaused(false);
  }

  public void StopPausedSFXVOBusses()
  {
    if (this.SFXPauseBus.isValid())
    {
      int num1 = (int) this.SFXPauseBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
    if (this.VOPauseBus.isValid())
    {
      int num2 = (int) this.VOPauseBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
    if (!this.StingersPauseBus.isValid())
      return;
    int num3 = (int) this.StingersPauseBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
  }

  public void StopActiveLoops()
  {
    foreach (EventInstance activeLoop in this.activeLoops)
      this.StopLoopInternal(activeLoop, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    this.activeLoops.Clear();
  }

  public bool StopLoopInternal(EventInstance e, FMOD.Studio.STOP_MODE stopMode)
  {
    int num1 = (int) e.stop(stopMode);
    int num2 = (int) e.release();
    return true;
  }

  public void ResumeActiveLoops()
  {
    foreach (EventInstance activeLoop in this.activeLoops)
    {
      int num = (int) activeLoop.setPaused(false);
    }
  }

  public EventInstance CreateLoop(
    string soundPath,
    GameObject go,
    bool playLoop = false,
    bool addToActiveLoops = true)
  {
    if (!this.IsSoundPathValid(soundPath))
      return this.invalidEventInstance;
    EventInstance instance;
    this.CreateLoopInternal(soundPath, out instance);
    if (addToActiveLoops && !this.activeLoops.Contains(instance))
      this.activeLoops.Add(instance);
    AudioManager.AudioAction onCreateLoop = this.OnCreateLoop;
    if (onCreateLoop != null)
      onCreateLoop(instance);
    if (instance.isValid())
    {
      int num1 = (int) instance.set3DAttributes(go.transform.To3DAttributes());
    }
    RuntimeManager.AttachInstanceToGameObject(instance, go.transform, (Rigidbody) null);
    if (playLoop)
    {
      int num2 = (int) instance.start();
    }
    return instance;
  }

  public void CreateLoopInternal(string soundPath, out EventInstance instance)
  {
    instance = RuntimeManager.CreateInstance(soundPath);
  }

  public void StopLoop(EventInstance instance, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
  {
    if (!instance.isValid() || !this.StopLoopInternal(instance, stopMode))
      return;
    if (this.activeLoops.Contains(instance))
      this.activeLoops.Remove(instance);
    AudioManager.AudioAction onReleaseLoop = this.OnReleaseLoop;
    if (onReleaseLoop == null)
      return;
    onReleaseLoop(instance);
  }

  public void PlayOneShotAndSetParameterValue(
    string soundPath,
    string parameterName,
    float value,
    Transform tf = null)
  {
    if (!this.IsSoundPathValid(soundPath))
      return;
    EventInstance instance = RuntimeManager.CreateInstance(soundPath);
    if (!instance.isValid())
      return;
    int num1 = (int) instance.setParameterByName(parameterName, value);
    if ((UnityEngine.Object) tf != (UnityEngine.Object) null)
    {
      int num2 = (int) instance.set3DAttributes(tf.To3DAttributes());
    }
    int num3 = (int) instance.start();
    int num4 = (int) instance.release();
  }

  public void PlayOneShotAndSetParametersValue(
    string soundPath,
    string parameterName,
    float value,
    string parameterName2,
    float value2,
    Transform tf = null,
    int followerID = -1)
  {
    if (!this.IsSoundPathValid(soundPath))
      return;
    if (followerID != -1 && FollowerManager.GetSpecialFollowerFallback(followerID) != null)
      soundPath = FollowerManager.GetSpecialFollowerFallback(followerID);
    EventInstance instance = RuntimeManager.CreateInstance(soundPath);
    if (!instance.isValid())
      return;
    int num1 = (int) instance.setParameterByName(parameterName, value);
    int num2 = (int) instance.setParameterByName(parameterName2, value2);
    int num3 = (int) instance.setVolume(1f);
    if ((UnityEngine.Object) tf != (UnityEngine.Object) null)
    {
      int num4 = (int) instance.set3DAttributes(tf.To3DAttributes());
    }
    int num5 = (int) instance.start();
    int num6 = (int) instance.release();
  }

  public void PlayOneShotAndSetParametersValue(
    string soundPath,
    string parameterName,
    float value,
    string parameterName2,
    float value2,
    Transform tf = null)
  {
    if (!this.IsSoundPathValid(soundPath))
      return;
    EventInstance instance = RuntimeManager.CreateInstance(soundPath);
    if (!instance.isValid())
      return;
    int num1 = (int) instance.setParameterByName(parameterName, value);
    int num2 = (int) instance.setParameterByName(parameterName2, value2);
    int num3 = (int) instance.setVolume(1f);
    if ((UnityEngine.Object) tf != (UnityEngine.Object) null)
    {
      int num4 = (int) instance.set3DAttributes(tf.To3DAttributes());
    }
    int num5 = (int) instance.start();
    int num6 = (int) instance.release();
  }

  public void PlayOneShotAndSetParametersValue(
    string soundPath,
    string parameterName,
    float value,
    string parameterName2,
    float value2,
    string parameterName3,
    float value3,
    Transform tf = null,
    int followerID = -1)
  {
    if (!this.IsSoundPathValid(soundPath))
      return;
    if (followerID != -1 && FollowerManager.GetSpecialFollowerFallback(followerID) != null)
      soundPath = FollowerManager.GetSpecialFollowerFallback(followerID);
    EventInstance instance = RuntimeManager.CreateInstance(soundPath);
    if (!instance.isValid())
      return;
    int num1 = (int) instance.setParameterByName(parameterName, value);
    int num2 = (int) instance.setParameterByName(parameterName2, value2);
    int num3 = (int) instance.setParameterByName(parameterName3, value3);
    int num4 = (int) instance.setVolume(1f);
    if ((UnityEngine.Object) tf != (UnityEngine.Object) null)
    {
      int num5 = (int) instance.set3DAttributes(tf.To3DAttributes());
    }
    int num6 = (int) instance.start();
    int num7 = (int) instance.release();
  }

  public void ReleaseInstances()
  {
    foreach (EventInstance eventInstance in this.oneShotEventInstances.Values)
    {
      int num1 = (int) eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
      int num2 = (int) eventInstance.release();
    }
    this.oneShotEventInstances.Clear();
    foreach (EventInstance eventInstance in this.cachedLoops.Values)
    {
      int num3 = (int) eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
      int num4 = (int) eventInstance.release();
    }
    this.cachedLoops.Clear();
    this.cachedLoopsCounter.Clear();
  }

  public float GetEventInstanceParameter(EventInstance eventInstance, string name)
  {
    if (!eventInstance.isValid())
      return 0.0f;
    float instanceParameter;
    int parameterByName = (int) eventInstance.getParameterByName(name, out instanceParameter);
    return instanceParameter;
  }

  public void SetEventInstanceParameter(
    EventInstance eventInstance,
    string name,
    float value,
    bool awaitPlaybackState = true)
  {
    if (!eventInstance.isValid())
      UnityEngine.Debug.Log((object) "Event Instance not valid");
    else
      this.StartCoroutine((IEnumerator) this.SetEventInstanceParameterDelay(eventInstance, name, value, awaitPlaybackState));
  }

  public IEnumerator SetEventInstanceParameterDelay(
    EventInstance eventInstance,
    string name,
    float value,
    bool awaitPlaybackState = true)
  {
    if (awaitPlaybackState)
    {
      PLAYBACK_STATE playback = PLAYBACK_STATE.STARTING;
      while (playback == PLAYBACK_STATE.STARTING)
      {
        int playbackState = (int) eventInstance.getPlaybackState(out playback);
        yield return (object) null;
      }
    }
    int num = (int) eventInstance.setParameterByName(name, value);
  }

  public void PlayFootstep(Vector3 pos)
  {
    if (!this.footstepOverride.IsNullOrEmpty())
    {
      this.PlayOneShot(this.footstepOverride, pos);
    }
    else
    {
      string soundPath = "event:/material/footstep_grass";
      switch (GenerateRoom.GetGroundTypeFromPosition(pos))
      {
        case GroundType.Grass:
          soundPath = "event:/material/footstep_grass";
          break;
        case GroundType.Hard:
          soundPath = "event:/material/footstep_hard";
          break;
        case GroundType.Sand:
          soundPath = "event:/material/footstep_sand";
          break;
        case GroundType.Snow:
          soundPath = "event:/material/footstep_snow";
          break;
        case GroundType.Water:
          soundPath = "event:/material/footstep_water";
          break;
        case GroundType.Woodland:
          soundPath = "event:/material/footstep_woodland";
          break;
        case GroundType.Bush:
          soundPath = "event:/material/footstep_bush";
          break;
        case GroundType.Ice:
          soundPath = "event:/player/footstep_ice";
          break;
      }
      this.PlayOneShot(soundPath, pos);
    }
  }

  public string GetGroundType(GroundType groundType)
  {
    switch (groundType)
    {
      case GroundType.Grass:
        return "event:/player/footstep_grass";
      case GroundType.Hard:
        return "event:/player/footstep_hard";
      case GroundType.Sand:
        return "event:/player/footstep_sand";
      case GroundType.Snow:
        return "event:/player/footstep_snow";
      case GroundType.Water:
        return "event:/player/footstep_water";
      case GroundType.Woodland:
        return "event:/player/footstep_woodland";
      case GroundType.Bush:
        return "event:/player/footstep_bush";
      default:
        return "";
    }
  }

  public void PlayFootstepPlayer(Vector3 pos)
  {
    if (PlayerFarming.Location == FollowerLocation.Base)
      this.playerFootstepOverride = this.GetGroundType(PathTileManager.Instance.GetTileSoundAtPosition(pos));
    if (!this.playerFootstepOverride.IsNullOrEmpty())
    {
      this.PlayOneShot(this.playerFootstepOverride, pos);
    }
    else
    {
      string soundPath = "event:/player/footstep_grass";
      switch (GenerateRoom.GetGroundTypeFromPosition(pos))
      {
        case GroundType.Grass:
          soundPath = "event:/player/footstep_grass";
          break;
        case GroundType.Hard:
          soundPath = "event:/player/footstep_hard";
          break;
        case GroundType.Sand:
          soundPath = "event:/player/footstep_sand";
          break;
        case GroundType.Snow:
          soundPath = "event:/player/footstep_snow";
          break;
        case GroundType.Water:
          soundPath = "event:/player/footstep_water";
          break;
        case GroundType.Woodland:
          soundPath = "event:/player/footstep_woodland";
          break;
        case GroundType.Bush:
          soundPath = "event:/player/footstep_bush";
          break;
        case GroundType.Ice:
          soundPath = "event:/player/footstep_ice";
          break;
      }
      this.PlayOneShot(soundPath, pos);
    }
  }

  public void SetMasterBusVolume(float volume)
  {
    int num = (int) this.masterBusVolumeSnapshot.setParameterByName(SoundParams.MasterBusVolume, volume);
  }

  public void SetMusicBusVolume(float volume)
  {
    int num = (int) this.musicBusVolumeSnapshot.setParameterByName(SoundParams.MusicBusVolume, volume);
  }

  public void SetSFXBusVolume(float volume)
  {
    int num = (int) this.sfxBusVolumeSnapshot.setParameterByName(SoundParams.SFXBusVolume, volume);
  }

  public void SetVOBusVolume(float volume)
  {
    int num = (int) this.voBusVolumeSnapshot.setParameterByName(SoundParams.VOBusVolume, volume);
  }

  public float GetVOBusVolume()
  {
    float voBusVolume;
    int parameterByName = (int) this.voBusVolumeSnapshot.getParameterByName(SoundParams.VOBusVolume, out voBusVolume);
    return voBusVolume;
  }

  public void OnDisable()
  {
    if (!((UnityEngine.Object) AudioManager._instance == (UnityEngine.Object) this))
      return;
    this.StopCurrentMusic();
  }

  public delegate void AudioAction(EventInstance eventInstance);
}
