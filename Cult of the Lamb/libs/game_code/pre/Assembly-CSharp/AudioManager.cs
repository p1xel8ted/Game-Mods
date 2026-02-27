// Decompiled with JetBrains decompiler
// Type: AudioManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using MMRoomGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

#nullable disable
public class AudioManager : BaseMonoBehaviour
{
  private static AudioManager _instance;
  [SerializeField]
  private Transform listener;
  [SerializeField]
  [Range(0.0f, 1f)]
  private float listenerPosBetweenCameraAndTarget;
  private EventInstance invalidEventInstance;
  private bool fmodBanksLoaded;
  private EventInstance currentMusicInstance;
  private string queuedMusicPath = string.Empty;
  private EventInstance AtmosInstance;
  private EventInstance masterBusVolumeSnapshot;
  private EventInstance sfxBusVolumeSnapshot;
  private EventInstance musicBusVolumeSnapshot;
  private EventInstance voBusVolumeSnapshot;
  [SerializeField]
  private GameManager gameManager;
  private bool SetFilter;
  private List<EventInstance> InstanceList = new List<EventInstance>();
  private List<EventInstance> activeLoops = new List<EventInstance>();
  public string playerFootstepOverride = string.Empty;
  public string footstepOverride = string.Empty;

  public static AudioManager Instance
  {
    get
    {
      if ((Object) AudioManager._instance == (Object) null)
        AudioManager._instance = (Object.Instantiate(Resources.Load("MMAudio/AudioManager")) as GameObject).GetComponent<AudioManager>();
      return AudioManager._instance;
    }
  }

  public GameObject Listener => this.listener.gameObject;

  private void Awake()
  {
    if ((Object) AudioManager._instance != (Object) null && (Object) AudioManager._instance != (Object) this)
    {
      Object.Destroy((Object) this.gameObject);
    }
    else
    {
      AudioManager._instance = this;
      if ((Object) this.transform.parent != (Object) null)
        this.transform.SetParent((Transform) null);
      Object.DontDestroyOnLoad((Object) this.gameObject);
    }
  }

  private IEnumerator Start()
  {
    while (!RuntimeManager.HaveAllBanksLoaded)
    {
      yield return (object) 0;
      Debug.Log((object) "FMOD All Banks not loaded");
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
    if (SettingsManager.Settings != null)
    {
      this.SetMasterBusVolume(SettingsManager.Settings.Audio.MasterVolume);
      this.SetMusicBusVolume(SettingsManager.Settings.Audio.MusicVolume);
      this.SetSFXBusVolume(SettingsManager.Settings.Audio.SFXVolume);
      this.SetVOBusVolume(SettingsManager.Settings.Audio.VOVolume);
    }
    else
      Debug.LogError((object) "Settings were read before being loaded.");
  }

  private void Update()
  {
    if ((Object) GameManager.GetInstance() == (Object) null || !((Object) GameManager.GetInstance().CamFollowTarget != (Object) null))
      return;
    this.listener.position = Vector3.Lerp(GameManager.GetInstance().CamFollowTarget.transform.position, GameManager.GetInstance().CamFollowTarget.targetPosition, this.listenerPosBetweenCameraAndTarget);
  }

  public void SetMusicFilter(string SoundParam, float value)
  {
    int num = (int) RuntimeManager.StudioSystem.setParameterByName(SoundParam, value);
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

  private IEnumerator ToggleFilterDelay(string SoundParam, bool toggle, float delay)
  {
    yield return (object) new WaitForSecondsRealtime(delay);
    this.ToggleFilter(SoundParam, toggle);
  }

  public void SetGameManager(GameManager gm) => this.gameManager = gm;

  private bool IsSoundPathValid(string soundPath)
  {
    if (string.IsNullOrEmpty(soundPath))
      return false;
    if (!this.fmodBanksLoaded)
    {
      Debug.LogWarning((object) $"FMOD cannot play event '{soundPath}' because banks have not loaded yet");
      return false;
    }
    EventDescription _event;
    int num = (int) RuntimeManager.StudioSystem.getEvent(soundPath, out _event);
    if (_event.isValid())
      return true;
    Debug.LogWarning((object) $"FMOD cannot find event '{soundPath}'");
    return false;
  }

  public bool CurrentEventIsPlayingPath(EventInstance currentInstance, string soundPath)
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
        if (this.CurrentEventIsPlayingPath(this.currentMusicInstance, soundPath))
        {
          Debug.Log((object) $"PK: {soundPath} is already playing");
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
      Debug.Log((object) "Couldn't start instance");
  }

  public void StartMusic()
  {
    if (this.currentMusicInstance.isValid())
    {
      int num = (int) this.currentMusicInstance.start();
    }
    else
      Debug.Log((object) "Couldn't start music");
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
        if (this.CurrentEventIsPlayingPath(this.AtmosInstance, soundPath))
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
      Debug.Log((object) "AtmosInstance not valid");
  }

  public void StopCurrentAtmos(bool AllowFadeOut = true)
  {
    if (!this.AtmosInstance.isValid())
      return;
    if (AllowFadeOut)
    {
      int num1 = (int) this.AtmosInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    else
    {
      int num2 = (int) this.AtmosInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
  }

  public void SetMusicRoomID(SoundConstants.RoomID roomID)
  {
    if (!this.currentMusicInstance.isValid())
      return;
    if (roomID == SoundConstants.RoomID.NoMusic)
      this.StopCurrentMusic();
    else
      this.SetEventInstanceParameter(this.currentMusicInstance, SoundParams.RoomID, (float) roomID);
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
      Debug.Log((object) "currentMusicInstance is not valid");
  }

  public void SetMusicBaseID(SoundConstants.BaseID baseID)
  {
    if (!this.currentMusicInstance.isValid())
      return;
    if (baseID == SoundConstants.BaseID.NoMusic)
      this.StopLoop(this.currentMusicInstance);
    else
      this.SetEventInstanceParameter(this.currentMusicInstance, SoundParams.BaseID, (float) baseID);
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
    RuntimeManager.PlayOneShot(soundPath);
  }

  public void PlayOneShotDelayed(string soundPath, float delay)
  {
    if (!this.IsSoundPathValid(soundPath))
      return;
    this.StartCoroutine((IEnumerator) this.OneShotDelayed(soundPath, delay));
  }

  private IEnumerator OneShotDelayed(string soundPath, float delay)
  {
    yield return (object) new WaitForSecondsRealtime(delay);
    RuntimeManager.PlayOneShot(soundPath);
  }

  public void PlayOneShot(string soundPath, Vector3 pos)
  {
    if (!this.IsSoundPathValid(soundPath))
      return;
    RuntimeManager.PlayOneShot(soundPath, pos);
  }

  public void PlayOneShot(string soundPath, GameObject go)
  {
    if (!this.IsSoundPathValid(soundPath))
      return;
    RuntimeManager.PlayOneShotAttached(soundPath, go);
  }

  public EventInstance CreateLoop(string soundPath, bool playLoop = false, bool addToActiveLoops = true)
  {
    if (!this.IsSoundPathValid(soundPath))
      return this.invalidEventInstance;
    EventInstance instance = RuntimeManager.CreateInstance(soundPath);
    if (addToActiveLoops)
      this.activeLoops.Add(instance);
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

  public void PauseActiveLoops()
  {
    foreach (EventInstance activeLoop in this.activeLoops)
    {
      int num = (int) activeLoop.setPaused(true);
    }
  }

  public void StopActiveLoops()
  {
    foreach (EventInstance activeLoop in this.activeLoops)
    {
      int num1 = (int) activeLoop.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
      int num2 = (int) activeLoop.release();
    }
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
    EventInstance instance = RuntimeManager.CreateInstance(soundPath);
    if (addToActiveLoops)
      this.activeLoops.Add(instance);
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

  public void StopLoop(EventInstance instance)
  {
    if (!instance.isValid())
      return;
    if (this.activeLoops.Contains(instance))
      this.activeLoops.Remove(instance);
    int num1 = (int) instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    int num2 = (int) instance.release();
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
    if ((Object) tf != (Object) null)
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
    if ((Object) tf != (Object) null)
    {
      int num4 = (int) instance.set3DAttributes(tf.To3DAttributes());
    }
    int num5 = (int) instance.start();
    int num6 = (int) instance.release();
  }

  public void SetEventInstanceParameter(EventInstance eventInstance, string name, float value)
  {
    if (!eventInstance.isValid())
    {
      Debug.Log((object) "Event Instance not valid");
    }
    else
    {
      int num = (int) eventInstance.setParameterByName(name, value);
    }
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

  private void OnDisable()
  {
    if (!((Object) AudioManager._instance == (Object) this))
      return;
    this.StopCurrentMusic();
  }
}
