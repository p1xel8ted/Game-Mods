// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.MasterAudio
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

#nullable disable
namespace DarkTonic.MasterAudio;

[AudioScriptOrder(-50)]
public class MasterAudio : MonoBehaviour
{
  public const string MasterAudioDefaultFolder = "Assets/Plugins/DarkTonic/MasterAudio";
  public const string PreviewText = "Random delay, custom fading & start/end position settings are ignored by preview in edit mode.";
  public const string LoopDisabledLoopedChain = "Loop Clip is always OFF for Looped Chain Groups";
  public const string LoopDisabledCustomStartEnd = "Loop Clip is always OFF when using Custom Start/End Position";
  public const string DragAudioTip = "Drag Audio clips or a folder containing some here";
  public const string NoCategory = "[Uncategorized]";
  public const float SemiTonePitchFactor = 1.05946f;
  public const float SpatialBlend_2DValue = 0.0f;
  public const float SpatialBlend_3DValue = 1f;
  public const float MaxCrossFadeTimeSeconds = 120f;
  public const float DefaultDuckVolCut = -6f;
  public const string StoredLanguageNameKey = "~MA_Language_Key~";
  public static YieldInstruction EndOfFrameDelay = (YieldInstruction) new WaitForEndOfFrame();
  public static List<string> ExemptChildNames = new List<string>()
  {
    "_Followers"
  };
  public static System.Action NumberOfAudioSourcesChanged;
  public const string GizmoFileName = "MasterAudio/MasterAudio Icon.png";
  public const int HardCodedBusOptions = 2;
  public const string AllBusesName = "[All]";
  public const string NoGroupName = "[None]";
  public const string DynamicGroupName = "[Type In]";
  public const string NoPlaylistName = "[No Playlist]";
  public const string NoVoiceLimitName = "[NO LMT]";
  public const string OnlyPlaylistControllerName = "~only~";
  public const float InnerLoopCheckInterval = 0.1f;
  public const int MaxComponents = 20;
  public DarkTonic.MasterAudio.MasterAudio.AudioLocation bulkLocationMode;
  public string groupTemplateName = "Default Single";
  public string audioSourceTemplateName = "Max Distance 500";
  public bool showGroupCreation = true;
  public bool useGroupTemplates;
  public DarkTonic.MasterAudio.MasterAudio.DragGroupMode curDragGroupMode;
  public List<GameObject> groupTemplates = new List<GameObject>(10);
  public List<GameObject> audioSourceTemplates = new List<GameObject>(10);
  public bool mixerMuted;
  public bool playlistsMuted;
  public DarkTonic.MasterAudio.MasterAudio.LanguageMode langMode;
  public SystemLanguage testLanguage = SystemLanguage.English;
  public SystemLanguage defaultLanguage = SystemLanguage.English;
  public List<SystemLanguage> supportedLanguages = new List<SystemLanguage>()
  {
    SystemLanguage.English
  };
  public string busFilter = string.Empty;
  public bool useTextGroupFilter;
  public string textGroupFilter = string.Empty;
  public bool resourceClipsPauseDoNotUnload;
  public bool resourceClipsAllLoadAsync = true;
  public Transform playlistControllerPrefab;
  public bool persistBetweenScenes;
  public bool areGroupsExpanded = true;
  public Transform soundGroupTemplate;
  public Transform soundGroupVariationTemplate;
  public List<GroupBus> groupBuses = new List<GroupBus>();
  public bool groupByBus = true;
  public bool showGizmos = true;
  public bool showAdvancedSettings = true;
  public bool showLocalization = true;
  public bool playListExpanded = true;
  public bool playlistsExpanded = true;
  public DarkTonic.MasterAudio.MasterAudio.AllMusicSpatialBlendType musicSpatialBlendType;
  public float musicSpatialBlend;
  public DarkTonic.MasterAudio.MasterAudio.AllMixerSpatialBlendType mixerSpatialBlendType = DarkTonic.MasterAudio.MasterAudio.AllMixerSpatialBlendType.ForceAllTo3D;
  public float mixerSpatialBlend = 1f;
  public DarkTonic.MasterAudio.MasterAudio.ItemSpatialBlendType newGroupSpatialType = DarkTonic.MasterAudio.MasterAudio.ItemSpatialBlendType.ForceTo3D;
  public float newGroupSpatialBlend = 1f;
  public List<DarkTonic.MasterAudio.MasterAudio.Playlist> musicPlaylists = new List<DarkTonic.MasterAudio.MasterAudio.Playlist>()
  {
    new DarkTonic.MasterAudio.MasterAudio.Playlist()
  };
  public float _masterAudioVolume = 1f;
  public bool useSpatializer;
  public bool ignoreTimeScale;
  public bool useGaplessPlaylists;
  public bool saveRuntimeChanges;
  public bool prioritizeOnDistance;
  public int rePrioritizeEverySecIndex = 1;
  public bool useOcclusion;
  public float occlusionMaxCutoffFreq;
  public float occlusionMinCutoffFreq = 22000f;
  public float occlusionFreqChangeSeconds;
  public DarkTonic.MasterAudio.MasterAudio.OcclusionSelectionType occlusionSelectType;
  public int occlusionMaxRayCastsPerFrame = 4;
  public float occlusionRayCastOffset;
  public bool occlusionUseLayerMask;
  public LayerMask occlusionLayerMask;
  public bool occlusionShowRaycasts = true;
  public bool occlusionShowCategories;
  public DarkTonic.MasterAudio.MasterAudio.RaycastMode occlusionRaycastMode;
  public bool occlusionIncludeStartRaycast2DCollider = true;
  public bool occlusionRaycastsHitTriggers = true;
  public bool ambientAdvancedExpanded;
  public int ambientMaxRecalcsPerFrame = 4;
  public bool visualAdvancedExpanded = true;
  public bool logAdvancedExpanded = true;
  public bool listenerAdvancedExpanded;
  public bool listenerFollowerHasRigidBody = true;
  public DarkTonic.MasterAudio.MasterAudio.VariationFollowerType variationFollowerType;
  public bool showFadingSettings;
  public bool stopZeroVolumeGroups;
  public bool stopZeroVolumeBuses;
  public bool stopZeroVolumePlaylists;
  public float stopOldestBusFadeTime = 0.3f;
  public bool resourceAdvancedExpanded = true;
  public bool useClipAgePriority;
  public bool logOutOfVoices = true;
  public bool LogSounds;
  public bool logCustomEvents;
  public bool disableLogging;
  public bool showMusicDucking;
  public bool enableMusicDucking = true;
  public List<DuckGroupInfo> musicDuckingSounds = new List<DuckGroupInfo>();
  public float defaultRiseVolStart = 0.5f;
  public float defaultUnduckTime = 1f;
  public float defaultDuckedVolumeCut = -6f;
  public float crossFadeTime = 1f;
  public float _masterPlaylistVolume = 1f;
  public bool showGroupSelect;
  public bool hideGroupsWithNoActiveVars;
  public string newEventName = "my event";
  public bool showCustomEvents = true;
  public string newCustomEventCategoryName = "New Category";
  public string addToCustomEventCategoryName = "New Category";
  public List<CustomEvent> customEvents = new List<CustomEvent>();
  public List<CustomEventCategory> customEventCategories = new List<CustomEventCategory>()
  {
    new CustomEventCategory()
  };
  public Dictionary<string, DuckGroupInfo> duckingBySoundType = new Dictionary<string, DuckGroupInfo>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  public int frames;
  public bool showUnityMixerGroupAssignment = true;
  public static PlaySoundResult AndForgetSuccessResult = new PlaySoundResult()
  {
    SoundPlayed = true
  };
  public Dictionary<string, DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo> AudioSourcesBySoundType = new Dictionary<string, DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  public Dictionary<string, List<int>> _randomizer = new Dictionary<string, List<int>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  public Dictionary<string, List<int>> _randomizerOrigin = new Dictionary<string, List<int>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  public Dictionary<string, List<int>> _randomizerLeftovers = new Dictionary<string, List<int>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  public Dictionary<string, List<int>> _clipsPlayedBySoundTypeOldestFirst = new Dictionary<string, List<int>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  public List<SoundGroupVariationUpdater> ActiveVariationUpdaters = new List<SoundGroupVariationUpdater>(32 /*0x20*/);
  public List<SoundGroupVariationUpdater> ActiveUpdatersToRemove = new List<SoundGroupVariationUpdater>();
  public List<DarkTonic.MasterAudio.MasterAudio.CustomEventCandidate> ValidReceivers = new List<DarkTonic.MasterAudio.MasterAudio.CustomEventCandidate>(10);
  public List<MasterAudioGroup> SoloedGroups = new List<MasterAudioGroup>();
  public Queue<CustomEventToFireInfo> CustomEventsToFire = new Queue<CustomEventToFireInfo>(32 /*0x20*/);
  public Queue<TransformFollower> TransFollowerColliderPositionRecalcs = new Queue<TransformFollower>(32 /*0x20*/);
  public List<TransformFollower> ProcessedColliderPositionRecalcs = new List<TransformFollower>(32 /*0x20*/);
  public List<BusFadeInfo> BusFades = new List<BusFadeInfo>();
  public List<GroupFadeInfo> GroupFades = new List<GroupFadeInfo>();
  public List<GroupPitchGlideInfo> GroupPitchGlides = new List<GroupPitchGlideInfo>();
  public List<BusPitchGlideInfo> BusPitchGlides = new List<BusPitchGlideInfo>();
  public List<OcclusionFreqChangeInfo> VariationOcclusionFreqChanges = new List<OcclusionFreqChangeInfo>();
  public List<AudioSource> AllAudioSources = new List<AudioSource>(100);
  public Dictionary<string, Dictionary<ICustomEventReceiver, Transform>> ReceiversByEventName = new Dictionary<string, Dictionary<ICustomEventReceiver, Transform>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  public Dictionary<string, PlaylistController> PlaylistControllersByName = new Dictionary<string, PlaylistController>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  public Dictionary<string, DarkTonic.MasterAudio.MasterAudio.SoundGroupRefillInfo> LastTimeSoundGroupPlayed = new Dictionary<string, DarkTonic.MasterAudio.MasterAudio.SoundGroupRefillInfo>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  public List<GameObject> OcclusionSourcesInRange = new List<GameObject>(32 /*0x20*/);
  public List<GameObject> OcclusionSourcesOutOfRange = new List<GameObject>(32 /*0x20*/);
  public List<GameObject> OcclusionSourcesBlocked = new List<GameObject>(32 /*0x20*/);
  public Queue<SoundGroupVariationUpdater> QueuedOcclusionRays = new Queue<SoundGroupVariationUpdater>(32 /*0x20*/);
  public List<SoundGroupVariation> VariationsStartedDuringMultiStop = new List<SoundGroupVariation>(16 /*0x10*/);
  public bool _isStoppingMultiple;
  public float _repriTime = -1f;
  public List<string> _groupsToRemove;
  public bool _mustRescanGroups;
  public Transform _trans;
  public bool _soundsLoaded;
  public bool _warming;
  public static DarkTonic.MasterAudio.MasterAudio _instance;
  public static string _prospectiveMAFolder = string.Empty;
  public static Transform _listenerTrans;
  public static List<DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand> GroupCommandsWithNoGroupSelector = new List<DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand>()
  {
    DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.None,
    DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.PauseAllSoundsOfTransform,
    DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.StopAllSoundsOfTransform,
    DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.UnpauseAllSoundsOfTransform,
    DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.FadeOutAllSoundsOfTransform
  };
  public static List<DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand> GroupCommandsWithNoAllGroupSelector = new List<DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand>()
  {
    DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.None,
    DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.FadeOutSoundGroupOfTransform,
    DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.PauseSoundGroupOfTransform,
    DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.UnpauseSoundGroupOfTransform,
    DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.StopSoundGroupOfTransform,
    DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.ToggleSoundGroupOfTransform,
    DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.ToggleSoundGroup,
    DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.FadeOutAllSoundsOfTransform
  };
  [CompilerGenerated]
  public static bool \u003CAppIsShuttingDown\u003Ek__BackingField;

  public void Awake()
  {
    if (UnityEngine.Object.FindObjectsOfType(typeof (DarkTonic.MasterAudio.MasterAudio)).Length > 1)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      Debug.Log((object) $"More than one Master Audio prefab exists in this Scene. Destroying the newer one called '{this.name}'. You may wish to set up a Bootstrapper Scene so this does not occur.");
    }
    else
    {
      this.useGUILayout = false;
      this._soundsLoaded = false;
      this._mustRescanGroups = false;
      Transform listenerTrans = DarkTonic.MasterAudio.MasterAudio.ListenerTrans;
      if ((UnityEngine.Object) listenerTrans != (UnityEngine.Object) null)
      {
        AudioSource component = listenerTrans.GetComponent<AudioSource>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          UnityEngine.Object.Destroy((UnityEngine.Object) component);
      }
      AmbientUtil.InitFollowerHolder();
      this.AudioSourcesBySoundType.Clear();
      this.PlaylistControllersByName.Clear();
      this.LastTimeSoundGroupPlayed.Clear();
      this.AllAudioSources.Clear();
      this.OcclusionSourcesInRange.Clear();
      this.OcclusionSourcesOutOfRange.Clear();
      this.OcclusionSourcesBlocked.Clear();
      this.QueuedOcclusionRays.Clear();
      this.TransFollowerColliderPositionRecalcs.Clear();
      this.ProcessedColliderPositionRecalcs.Clear();
      this.ActiveVariationUpdaters.Clear();
      this.ActiveUpdatersToRemove.Clear();
      List<string> stringList1 = new List<string>();
      AudioResourceOptimizer.ClearAudioClips();
      PlaylistController.Instances = (List<PlaylistController>) null;
      List<PlaylistController> instances = PlaylistController.Instances;
      for (int index = 0; index < instances.Count; ++index)
      {
        PlaylistController target = instances[index];
        if (stringList1.Contains(target.name))
        {
          Debug.LogError((object) $"You have more than 1 Playlist Controller with the name '{target.name}'. You must name them all uniquely or the same-named ones will be deleted once they awake.");
        }
        else
        {
          stringList1.Add(target.name);
          this.PlaylistControllersByName.Add(target.name, target);
          if (this.persistBetweenScenes)
            UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) target);
        }
      }
      if (this.persistBetweenScenes)
        UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);
      List<int> list = new List<int>();
      this._randomizer = new Dictionary<string, List<int>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this._randomizerOrigin = new Dictionary<string, List<int>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this._randomizerLeftovers = new Dictionary<string, List<int>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this._clipsPlayedBySoundTypeOldestFirst = new Dictionary<string, List<int>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      string sType = string.Empty;
      List<SoundGroupVariation> soundGroupVariationList = new List<SoundGroupVariation>();
      this._groupsToRemove = new List<string>(this.Trans.childCount);
      List<string> stringList2 = new List<string>();
      for (int index1 = 0; index1 < this.Trans.childCount; ++index1)
      {
        Transform child1 = this.Trans.GetChild(index1);
        List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> sources = new List<DarkTonic.MasterAudio.MasterAudio.AudioInfo>();
        MasterAudioGroup component1 = child1.GetComponent<MasterAudioGroup>();
        if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
        {
          if (!ArrayListUtil.IsExcludedChildName(child1.name))
            Debug.LogError((object) $"MasterAudio could not find 'MasterAudioGroup' script for group '{child1.name}'. Skipping this group.");
        }
        else
        {
          string name = child1.name;
          if (string.IsNullOrEmpty(sType))
            sType = name;
          List<Transform> transformList = new List<Transform>();
          List<int> intList = new List<int>();
          for (int index2 = 0; index2 < child1.childCount; ++index2)
          {
            Transform child2 = child1.GetChild(index2);
            SoundGroupVariation component2 = child2.GetComponent<SoundGroupVariation>();
            AudioSource component3 = child2.GetComponent<AudioSource>();
            int weight = component2.weight;
            for (int index3 = 0; index3 < weight; ++index3)
            {
              if (index3 > 0)
              {
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(child2.gameObject, child1.transform.position, Quaternion.identity);
                gameObject.transform.name = child2.gameObject.name;
                SoundGroupVariation component4 = gameObject.GetComponent<SoundGroupVariation>();
                component4.weight = 1;
                transformList.Add(gameObject.transform);
                component3 = gameObject.GetComponent<AudioSource>();
                sources.Add(new DarkTonic.MasterAudio.MasterAudio.AudioInfo(component4, component3, component3.volume));
                soundGroupVariationList.Add(component4);
                switch (component4.audLocation)
                {
                  case DarkTonic.MasterAudio.MasterAudio.AudioLocation.ResourceFile:
                    AudioResourceOptimizer.AddTargetForClip(component4.resourceFileName, component3);
                    continue;
                  case DarkTonic.MasterAudio.MasterAudio.AudioLocation.FileOnInternet:
                    AudioResourceOptimizer.AddTargetForClip(component4.internetFileUrl, component3);
                    continue;
                  default:
                    continue;
                }
              }
              else
              {
                sources.Add(new DarkTonic.MasterAudio.MasterAudio.AudioInfo(component2, component3, component3.volume));
                soundGroupVariationList.Add(component2);
                switch (component2.audLocation)
                {
                  case DarkTonic.MasterAudio.MasterAudio.AudioLocation.ResourceFile:
                    AudioResourceOptimizer.AddTargetForClip(AudioResourceOptimizer.GetLocalizedFileName(component2.useLocalization, component2.resourceFileName), component3);
                    continue;
                  case DarkTonic.MasterAudio.MasterAudio.AudioLocation.FileOnInternet:
                    AudioResourceOptimizer.AddTargetForClip(component2.internetFileUrl, component3);
                    continue;
                  default:
                    continue;
                }
              }
            }
          }
          for (int index4 = 0; index4 < transformList.Count; ++index4)
            transformList[index4].parent = child1;
          DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo groupInfo = new DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo(sources, component1);
          if (component1.isSoloed)
            this.SoloedGroups.Add(component1);
          if (component1.isMuted)
          {
            if (!stringList2.Contains(component1.name))
              stringList2.Add(component1.name);
            else
              continue;
          }
          if (this.AudioSourcesBySoundType.ContainsKey(name))
          {
            Debug.LogError((object) $"You have more than one SoundGroup named '{name}'. Ignoring the 2nd one. Please rename it.");
          }
          else
          {
            groupInfo.Group.OriginalVolume = groupInfo.Group.groupMasterVolume;
            float? groupVolume = PersistentAudioSettings.GetGroupVolume(name);
            if (groupVolume.HasValue)
              groupInfo.Group.groupMasterVolume = groupVolume.Value;
            DarkTonic.MasterAudio.MasterAudio.AddRuntimeGroupInfo(name, groupInfo);
            for (int index5 = 0; index5 < sources.Count; ++index5)
              list.Add(index5);
            if (groupInfo.Group.curVariationSequence == MasterAudioGroup.VariationSequence.Randomized)
              ArrayListUtil.SortIntArray(ref list);
            this._randomizer.Add(name, list);
            intList.Clear();
            intList.AddRange((IEnumerable<int>) list);
            this._randomizerOrigin.Add(name, intList);
            this._randomizerLeftovers.Add(name, new List<int>(list.Count));
            this._randomizerLeftovers[name].AddRange((IEnumerable<int>) list);
            this._clipsPlayedBySoundTypeOldestFirst.Add(name, new List<int>());
            list = new List<int>();
          }
        }
      }
      this.GroupFades.Clear();
      this.BusFades.Clear();
      this.GroupPitchGlides.Clear();
      this.BusPitchGlides.Clear();
      this.VariationOcclusionFreqChanges.Clear();
      for (int index = 0; index < this.groupBuses.Count; ++index)
      {
        GroupBus groupBus = this.groupBuses[index];
        groupBus.OriginalVolume = groupBus.volume;
        string busName = groupBus.busName;
        float? busVolume = PersistentAudioSettings.GetBusVolume(busName);
        if (busVolume.HasValue)
          DarkTonic.MasterAudio.MasterAudio.SetBusVolumeByName(busName, busVolume.Value);
      }
      this.duckingBySoundType.Clear();
      for (int index = 0; index < this.musicDuckingSounds.Count; ++index)
      {
        DuckGroupInfo musicDuckingSound = this.musicDuckingSounds[index];
        if (this.duckingBySoundType.ContainsKey(musicDuckingSound.soundType))
          Debug.LogWarning((object) $"You have more than one Duck Group set up with the Sound Group '{musicDuckingSound.soundType}'. Please delete the duplicates before running again.");
        else
          this.duckingBySoundType.Add(musicDuckingSound.soundType, musicDuckingSound);
      }
      this._soundsLoaded = true;
      this._warming = true;
      if (!string.IsNullOrEmpty(sType))
      {
        PlaySoundResult playSoundResult = DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransform(sType, this.Trans, 0.0f);
        if (playSoundResult != null && playSoundResult.SoundPlayed)
          playSoundResult.ActingVariation.Stop();
      }
      DarkTonic.MasterAudio.MasterAudio.FireCustomEvent("FakeEvent", this._trans);
      for (int index = 0; index < this.customEvents.Count; ++index)
        this.customEvents[index].frameLastFired = -1;
      this.frames = 0;
      UnityEngine.Object[] objectsOfType = UnityEngine.Object.FindObjectsOfType(typeof (EventSounds));
      if (objectsOfType.Length != 0)
      {
        EventSounds eventSounds = objectsOfType[0] as EventSounds;
        eventSounds.PlaySounds(eventSounds.particleCollisionSound, EventSounds.EventType.UserDefinedEvent);
      }
      for (int index = 0; index < stringList2.Count; ++index)
        DarkTonic.MasterAudio.MasterAudio.MuteGroup(stringList2[index], false);
      this._warming = false;
      for (int index = 0; index < soundGroupVariationList.Count; ++index)
        soundGroupVariationList[index].DisableUpdater();
      AmbientUtil.InitListenerFollower();
      PersistentAudioSettings.RestoreMasterSettings();
    }
  }

  public void Start()
  {
    if (this.musicPlaylists.Count <= 0 || this.musicPlaylists[0].MusicSettings == null || this.musicPlaylists[0].MusicSettings.Count <= 0 || !((UnityEngine.Object) this.musicPlaylists[0].MusicSettings[0].clip != (UnityEngine.Object) null) || this.PlaylistControllersByName.Count != 0)
      return;
    Debug.Log((object) "No Playlist Controllers exist in the Scene. Music will not play.");
  }

  public void OnDisable()
  {
    DarkTonic.MasterAudio.MasterAudio.StopTrackingRuntimeAudioSources(((IEnumerable<AudioSource>) this.GetComponentsInChildren<AudioSource>()).ToList<AudioSource>());
  }

  public void Update()
  {
    ++this.frames;
    DarkTonic.MasterAudio.MasterAudio.PerformOcclusionFrequencyChanges();
    this.PerformBusFades();
    this.PerformBusPitchGlides();
    this.PerformGroupFades();
    this.PerformGroupPitchGlides();
    DarkTonic.MasterAudio.MasterAudio.RefillInactiveGroupPools();
    DarkTonic.MasterAudio.MasterAudio.FireCustomEventsWaiting();
    DarkTonic.MasterAudio.MasterAudio.RecalcClosestColliderPositions();
  }

  public void LateUpdate()
  {
    if (this.variationFollowerType != DarkTonic.MasterAudio.MasterAudio.VariationFollowerType.LateUpdate)
      return;
    this.UpdateActiveVariations();
  }

  public void FixedUpdate()
  {
    if (this.variationFollowerType != DarkTonic.MasterAudio.MasterAudio.VariationFollowerType.FixedUpdate)
      return;
    this.UpdateActiveVariations();
  }

  public static void RegisterUpdaterForUpdates(SoundGroupVariationUpdater updater)
  {
    if (DarkTonic.MasterAudio.MasterAudio.Instance.ActiveVariationUpdaters.Contains(updater))
      return;
    DarkTonic.MasterAudio.MasterAudio.Instance.ActiveVariationUpdaters.Add(updater);
  }

  public static void UnregisterUpdaterForUpdates(SoundGroupVariationUpdater updater)
  {
    DarkTonic.MasterAudio.MasterAudio.Instance.ActiveVariationUpdaters.Remove(updater);
  }

  public void UpdateActiveVariations()
  {
    this.ActiveUpdatersToRemove.Clear();
    for (int index = 0; index < this.ActiveVariationUpdaters.Count; ++index)
    {
      SoundGroupVariationUpdater variationUpdater = this.ActiveVariationUpdaters[index];
      if ((UnityEngine.Object) variationUpdater == (UnityEngine.Object) null || !variationUpdater.enabled)
        this.ActiveUpdatersToRemove.Add(variationUpdater);
      else
        variationUpdater.ManualUpdate();
    }
    for (int index = 0; index < this.ActiveUpdatersToRemove.Count; ++index)
      this.ActiveVariationUpdaters.Remove(this.ActiveUpdatersToRemove[index]);
  }

  public static void UpdateRefillTime(string sType, float inactivePeriodSeconds)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.Instance.LastTimeSoundGroupPlayed.ContainsKey(sType))
      DarkTonic.MasterAudio.MasterAudio.Instance.LastTimeSoundGroupPlayed.Add(sType, new DarkTonic.MasterAudio.MasterAudio.SoundGroupRefillInfo(Time.realtimeSinceStartup, inactivePeriodSeconds));
    else
      DarkTonic.MasterAudio.MasterAudio.Instance.LastTimeSoundGroupPlayed[sType].LastTimePlayed = AudioUtil.Time;
  }

  public static void RecalcClosestColliderPositions()
  {
    if (!AmbientUtil.HasListenerFollower)
      AmbientUtil.InitListenerFollower();
    DarkTonic.MasterAudio.MasterAudio.Instance.ProcessedColliderPositionRecalcs.Clear();
    int num1 = 0;
    while (num1 < DarkTonic.MasterAudio.MasterAudio.Instance.TransFollowerColliderPositionRecalcs.Count && DarkTonic.MasterAudio.MasterAudio.Instance.TransFollowerColliderPositionRecalcs.Count != 0)
    {
      TransformFollower transformFollower = DarkTonic.MasterAudio.MasterAudio.Instance.TransFollowerColliderPositionRecalcs.Dequeue();
      if (!((UnityEngine.Object) transformFollower == (UnityEngine.Object) null) && transformFollower.enabled)
      {
        int num2 = transformFollower.RecalcClosestColliderPosition() ? 1 : 0;
        DarkTonic.MasterAudio.MasterAudio.Instance.ProcessedColliderPositionRecalcs.Add(transformFollower);
        if (num2 != 0)
          ++num1;
      }
    }
    for (int index = 0; index < DarkTonic.MasterAudio.MasterAudio.Instance.ProcessedColliderPositionRecalcs.Count; ++index)
      DarkTonic.MasterAudio.MasterAudio.Instance.TransFollowerColliderPositionRecalcs.Enqueue(DarkTonic.MasterAudio.MasterAudio.Instance.ProcessedColliderPositionRecalcs[index]);
  }

  public static void FireCustomEventsWaiting()
  {
    while (DarkTonic.MasterAudio.MasterAudio.Instance.CustomEventsToFire.Count > 0)
    {
      CustomEventToFireInfo customEventToFireInfo = DarkTonic.MasterAudio.MasterAudio.Instance.CustomEventsToFire.Dequeue();
      DarkTonic.MasterAudio.MasterAudio.FireCustomEvent(customEventToFireInfo.eventName, customEventToFireInfo.eventOrigin);
    }
  }

  public static void RefillInactiveGroupPools()
  {
    Dictionary<string, DarkTonic.MasterAudio.MasterAudio.SoundGroupRefillInfo>.Enumerator enumerator = DarkTonic.MasterAudio.MasterAudio.Instance.LastTimeSoundGroupPlayed.GetEnumerator();
    if (DarkTonic.MasterAudio.MasterAudio.Instance._groupsToRemove == null)
      DarkTonic.MasterAudio.MasterAudio.Instance._groupsToRemove = new List<string>();
    DarkTonic.MasterAudio.MasterAudio.Instance._groupsToRemove.Clear();
    while (enumerator.MoveNext())
    {
      KeyValuePair<string, DarkTonic.MasterAudio.MasterAudio.SoundGroupRefillInfo> current = enumerator.Current;
      if ((double) current.Value.LastTimePlayed + (double) current.Value.InactivePeriodSeconds < (double) AudioUtil.Time)
      {
        DarkTonic.MasterAudio.MasterAudio.RefillSoundGroupPool(current.Key);
        DarkTonic.MasterAudio.MasterAudio.Instance._groupsToRemove.Add(current.Key);
      }
    }
    for (int index = 0; index < DarkTonic.MasterAudio.MasterAudio.Instance._groupsToRemove.Count; ++index)
      DarkTonic.MasterAudio.MasterAudio.Instance.LastTimeSoundGroupPlayed.Remove(DarkTonic.MasterAudio.MasterAudio.Instance._groupsToRemove[index]);
  }

  public static void PerformOcclusionFrequencyChanges()
  {
    if (!AmbientUtil.HasListenerFollower)
      AmbientUtil.InitListenerFollower();
    for (int index = 0; index < DarkTonic.MasterAudio.MasterAudio.Instance.VariationOcclusionFreqChanges.Count; ++index)
    {
      OcclusionFreqChangeInfo occlusionFreqChange = DarkTonic.MasterAudio.MasterAudio.Instance.VariationOcclusionFreqChanges[index];
      if (occlusionFreqChange.IsActive)
      {
        float num1 = Math.Max(Math.Min((float) (1.0 - ((double) occlusionFreqChange.CompletionTime - (double) AudioUtil.Time) / ((double) occlusionFreqChange.CompletionTime - (double) occlusionFreqChange.StartTime)), 1f), 0.0f);
        float val1 = occlusionFreqChange.StartFrequency + (occlusionFreqChange.TargetFrequency - occlusionFreqChange.StartFrequency) * num1;
        float num2 = (double) occlusionFreqChange.TargetFrequency <= (double) occlusionFreqChange.StartFrequency ? Math.Max(val1, occlusionFreqChange.TargetFrequency) : Math.Min(val1, occlusionFreqChange.TargetFrequency);
        occlusionFreqChange.ActingVariation.LowPassFilter.cutoffFrequency = num2;
        if ((double) AudioUtil.Time >= (double) occlusionFreqChange.CompletionTime)
          occlusionFreqChange.IsActive = false;
      }
    }
    DarkTonic.MasterAudio.MasterAudio.Instance.VariationOcclusionFreqChanges.RemoveAll((Predicate<OcclusionFreqChangeInfo>) (obj => !obj.IsActive));
  }

  public void PerformBusFades()
  {
    for (int index = 0; index < this.BusFades.Count; ++index)
    {
      BusFadeInfo busFade = this.BusFades[index];
      if (busFade.IsActive)
      {
        GroupBus actingBus = busFade.ActingBus;
        if (actingBus == null)
        {
          busFade.IsActive = false;
        }
        else
        {
          float num = Math.Max(Math.Min((float) (1.0 - ((double) busFade.CompletionTime - (double) AudioUtil.Time) / ((double) busFade.CompletionTime - (double) busFade.StartTime)), 1f), 0.0f);
          float val1 = busFade.StartVolume + (busFade.TargetVolume - busFade.StartVolume) * num;
          float newVolume = (double) busFade.TargetVolume <= (double) busFade.StartVolume ? Math.Max(val1, busFade.TargetVolume) : Math.Min(val1, busFade.TargetVolume);
          DarkTonic.MasterAudio.MasterAudio.SetBusVolumeByName(actingBus.busName, newVolume);
          if ((double) AudioUtil.Time >= (double) busFade.CompletionTime)
          {
            busFade.IsActive = false;
            if (this.stopZeroVolumeBuses && (double) busFade.TargetVolume == 0.0)
              DarkTonic.MasterAudio.MasterAudio.StopBus(busFade.NameOfBus);
            else if (busFade.WillStopGroupAfterFade)
              DarkTonic.MasterAudio.MasterAudio.StopBus(busFade.NameOfBus);
            if (busFade.WillResetVolumeAfterFade)
              DarkTonic.MasterAudio.MasterAudio.SetBusVolumeByName(actingBus.busName, busFade.StartVolume);
            if (busFade.completionAction != null)
              busFade.completionAction();
          }
        }
      }
    }
    this.BusFades.RemoveAll((Predicate<BusFadeInfo>) (obj => !obj.IsActive));
  }

  public void PerformGroupFades()
  {
    for (int index = 0; index < this.GroupFades.Count; ++index)
    {
      GroupFadeInfo groupFade = this.GroupFades[index];
      if (groupFade.IsActive)
      {
        MasterAudioGroup actingGroup = groupFade.ActingGroup;
        if ((UnityEngine.Object) actingGroup == (UnityEngine.Object) null)
        {
          groupFade.IsActive = false;
        }
        else
        {
          float num = Math.Max(Math.Min((float) (1.0 - ((double) groupFade.CompletionTime - (double) AudioUtil.Time) / ((double) groupFade.CompletionTime - (double) groupFade.StartTime)), 1f), 0.0f);
          float val1 = groupFade.StartVolume + (groupFade.TargetVolume - groupFade.StartVolume) * num;
          float volumeLevel = (double) groupFade.TargetVolume <= (double) groupFade.StartVolume ? Math.Max(val1, groupFade.TargetVolume) : Math.Min(val1, groupFade.TargetVolume);
          DarkTonic.MasterAudio.MasterAudio.SetGroupVolume(actingGroup.GameObjectName, volumeLevel);
          if ((double) AudioUtil.Time >= (double) groupFade.CompletionTime)
          {
            groupFade.IsActive = false;
            if (groupFade.completionAction != null)
              groupFade.completionAction();
            if (this.stopZeroVolumeGroups && (double) groupFade.TargetVolume == 0.0)
              DarkTonic.MasterAudio.MasterAudio.StopAllOfSound(groupFade.NameOfGroup);
            else if (groupFade.WillStopGroupAfterFade)
              DarkTonic.MasterAudio.MasterAudio.StopAllOfSound(groupFade.NameOfGroup);
            if (groupFade.WillResetVolumeAfterFade)
              DarkTonic.MasterAudio.MasterAudio.SetGroupVolume(actingGroup.GameObjectName, groupFade.StartVolume);
          }
        }
      }
    }
    this.GroupFades.RemoveAll((Predicate<GroupFadeInfo>) (obj => !obj.IsActive));
  }

  public void PerformGroupPitchGlides()
  {
    for (int index = 0; index < this.GroupPitchGlides.Count; ++index)
    {
      GroupPitchGlideInfo groupPitchGlide = this.GroupPitchGlides[index];
      if (groupPitchGlide.IsActive)
      {
        if ((UnityEngine.Object) groupPitchGlide.ActingGroup == (UnityEngine.Object) null)
          groupPitchGlide.IsActive = false;
        else if ((double) AudioUtil.Time >= (double) groupPitchGlide.CompletionTime)
        {
          groupPitchGlide.IsActive = false;
          if (groupPitchGlide.completionAction != null)
          {
            groupPitchGlide.completionAction();
            groupPitchGlide.completionAction = (System.Action) null;
          }
        }
      }
    }
    this.GroupPitchGlides.RemoveAll((Predicate<GroupPitchGlideInfo>) (obj => !obj.IsActive));
  }

  public void PerformBusPitchGlides()
  {
    for (int index = 0; index < this.BusPitchGlides.Count; ++index)
    {
      BusPitchGlideInfo busPitchGlide = this.BusPitchGlides[index];
      if (busPitchGlide.IsActive)
      {
        if (DarkTonic.MasterAudio.MasterAudio.GetBusIndex(busPitchGlide.NameOfBus, true) < 0)
          busPitchGlide.IsActive = false;
        else if ((double) AudioUtil.Time >= (double) busPitchGlide.CompletionTime)
        {
          busPitchGlide.IsActive = false;
          if (busPitchGlide.completionAction != null)
          {
            busPitchGlide.completionAction();
            busPitchGlide.completionAction = (System.Action) null;
          }
        }
      }
    }
    this.BusPitchGlides.RemoveAll((Predicate<BusPitchGlideInfo>) (obj => !obj.IsActive));
  }

  public void OnApplicationQuit() => DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown = true;

  public static bool PlaySoundAndForget(
    string sType,
    float volumePercentage = 1f,
    float? pitch = null,
    float delaySoundTime = 0.0f,
    string variationName = null,
    double? timeToSchedulePlay = null)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return false;
    if (DarkTonic.MasterAudio.MasterAudio.SoundsReady)
      return DarkTonic.MasterAudio.MasterAudio.PSRAsSuccessBool(DarkTonic.MasterAudio.MasterAudio.PlaySoundAtVolume(sType, volumePercentage, Vector3.zero, timeToSchedulePlay, pitch, variationName: variationName, delaySoundTime: delaySoundTime));
    Debug.LogError((object) ("MasterAudio not finished initializing sounds. Cannot play: " + sType));
    return false;
  }

  public static PlaySoundResult PlaySound(
    string sType,
    float volumePercentage = 1f,
    float? pitch = null,
    float delaySoundTime = 0.0f,
    string variationName = null,
    double? timeToSchedulePlay = null,
    bool isChaining = false,
    bool isSingleSubscribedPlay = false)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return (PlaySoundResult) null;
    if (DarkTonic.MasterAudio.MasterAudio.SoundsReady)
      return DarkTonic.MasterAudio.MasterAudio.PlaySoundAtVolume(sType, volumePercentage, Vector3.zero, timeToSchedulePlay, pitch, variationName: variationName, delaySoundTime: delaySoundTime, makePlaySoundResult: true, isChaining: isChaining, isSingleSubscribedPlay: isSingleSubscribedPlay);
    Debug.LogError((object) ("MasterAudio not finished initializing sounds. Cannot play: " + sType));
    return (PlaySoundResult) null;
  }

  public static bool PlaySound3DAtVector3AndForget(
    string sType,
    Vector3 sourcePosition,
    float volumePercentage = 1f,
    float? pitch = null,
    float delaySoundTime = 0.0f,
    string variationName = null,
    double? timeToSchedulePlay = null)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return false;
    if (DarkTonic.MasterAudio.MasterAudio.SoundsReady)
      return DarkTonic.MasterAudio.MasterAudio.PSRAsSuccessBool(DarkTonic.MasterAudio.MasterAudio.PlaySoundAtVolume(sType, volumePercentage, sourcePosition, timeToSchedulePlay, pitch, variationName: variationName, delaySoundTime: delaySoundTime, useVector3: true));
    Debug.LogError((object) ("MasterAudio not finished initializing sounds. Cannot play: " + sType));
    return false;
  }

  public static PlaySoundResult PlaySound3DAtVector3(
    string sType,
    Vector3 sourcePosition,
    float volumePercentage = 1f,
    float? pitch = null,
    float delaySoundTime = 0.0f,
    string variationName = null,
    double? timeToSchedulePlay = null)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return (PlaySoundResult) null;
    if (DarkTonic.MasterAudio.MasterAudio.SoundsReady)
      return DarkTonic.MasterAudio.MasterAudio.PlaySoundAtVolume(sType, volumePercentage, sourcePosition, timeToSchedulePlay, pitch, variationName: variationName, delaySoundTime: delaySoundTime, useVector3: true, makePlaySoundResult: true);
    Debug.LogError((object) ("MasterAudio not finished initializing sounds. Cannot play: " + sType));
    return (PlaySoundResult) null;
  }

  public static bool PlaySound3DAtTransformAndForget(
    string sType,
    Transform sourceTrans,
    float volumePercentage = 1f,
    float? pitch = null,
    float delaySoundTime = 0.0f,
    string variationName = null,
    double? timeToSchedulePlay = null)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return false;
    if (DarkTonic.MasterAudio.MasterAudio.SoundsReady)
      return DarkTonic.MasterAudio.MasterAudio.PSRAsSuccessBool(DarkTonic.MasterAudio.MasterAudio.PlaySoundAtVolume(sType, volumePercentage, Vector3.zero, timeToSchedulePlay, pitch, sourceTrans, variationName, delaySoundTime: delaySoundTime));
    Debug.LogError((object) ("MasterAudio not finished initializing sounds. Cannot play: " + sType));
    return false;
  }

  public static PlaySoundResult PlaySound3DAtTransform(
    string sType,
    Transform sourceTrans,
    float volumePercentage = 1f,
    float? pitch = null,
    float delaySoundTime = 0.0f,
    string variationName = null,
    double? timeToSchedulePlay = null,
    bool isChaining = false,
    bool isSingleSubscribedPlay = false)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return (PlaySoundResult) null;
    if (DarkTonic.MasterAudio.MasterAudio.SoundsReady)
      return DarkTonic.MasterAudio.MasterAudio.PlaySoundAtVolume(sType, volumePercentage, Vector3.zero, timeToSchedulePlay, pitch, sourceTrans, variationName, delaySoundTime: delaySoundTime, makePlaySoundResult: true, isChaining: isChaining, isSingleSubscribedPlay: isSingleSubscribedPlay);
    Debug.LogError((object) ("MasterAudio not finished initializing sounds. Cannot play: " + sType));
    return (PlaySoundResult) null;
  }

  public static bool PlaySound3DFollowTransformAndForget(
    string sType,
    Transform sourceTrans,
    float volumePercentage = 1f,
    float? pitch = null,
    float delaySoundTime = 0.0f,
    string variationName = null,
    double? timeToSchedulePlay = null)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return false;
    if (DarkTonic.MasterAudio.MasterAudio.SoundsReady)
      return DarkTonic.MasterAudio.MasterAudio.PSRAsSuccessBool(DarkTonic.MasterAudio.MasterAudio.PlaySoundAtVolume(sType, volumePercentage, Vector3.zero, timeToSchedulePlay, pitch, sourceTrans, variationName, true, delaySoundTime));
    Debug.LogError((object) ("MasterAudio not finished initializing sounds. Cannot play: " + sType));
    return false;
  }

  public static PlaySoundResult PlaySound3DFollowTransform(
    string sType,
    Transform sourceTrans,
    float volumePercentage = 1f,
    float? pitch = null,
    float delaySoundTime = 0.0f,
    string variationName = null,
    double? timeToSchedulePlay = null,
    bool isChaining = false,
    bool isSingleSubscribedPlay = false)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return (PlaySoundResult) null;
    if (DarkTonic.MasterAudio.MasterAudio.SoundsReady)
      return DarkTonic.MasterAudio.MasterAudio.PlaySoundAtVolume(sType, volumePercentage, Vector3.zero, timeToSchedulePlay, pitch, sourceTrans, variationName, true, delaySoundTime, makePlaySoundResult: true, isChaining: isChaining, isSingleSubscribedPlay: isSingleSubscribedPlay);
    Debug.LogError((object) ("MasterAudio not finished initializing sounds. Cannot play: " + sType));
    return (PlaySoundResult) null;
  }

  public static IEnumerator PlaySoundAndWaitUntilFinished(
    string sType,
    float volumePercentage = 1f,
    float? pitch = null,
    float delaySoundTime = 0.0f,
    string variationName = null,
    System.Action completedAction = null)
  {
    if (DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
    {
      if (!DarkTonic.MasterAudio.MasterAudio.SoundsReady)
      {
        Debug.LogError((object) ("MasterAudio not finished initializing sounds. Cannot play: " + sType));
      }
      else
      {
        PlaySoundResult playSoundResult = DarkTonic.MasterAudio.MasterAudio.PlaySound(sType, volumePercentage, pitch, delaySoundTime, variationName, isSingleSubscribedPlay: true);
        bool done = false;
        if (playSoundResult != null && !((UnityEngine.Object) playSoundResult.ActingVariation == (UnityEngine.Object) null))
        {
          playSoundResult.ActingVariation.SoundFinished += (SoundGroupVariation.SoundFinishedEventHandler) (() => done = true);
          while (!done)
            yield return (object) DarkTonic.MasterAudio.MasterAudio.EndOfFrameDelay;
          if (completedAction != null)
            completedAction();
        }
      }
    }
  }

  public static IEnumerator PlaySound3DAtTransformAndWaitUntilFinished(
    string sType,
    Transform sourceTrans,
    float volumePercentage = 1f,
    float? pitch = null,
    float delaySoundTime = 0.0f,
    string variationName = null,
    double? timeToSchedulePlay = null,
    System.Action completedAction = null)
  {
    if (DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
    {
      if (!DarkTonic.MasterAudio.MasterAudio.SoundsReady)
      {
        Debug.LogError((object) ("MasterAudio not finished initializing sounds. Cannot play: " + sType));
      }
      else
      {
        PlaySoundResult playSoundResult = DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransform(sType, sourceTrans, volumePercentage, pitch, delaySoundTime, variationName, timeToSchedulePlay, isSingleSubscribedPlay: true);
        bool done = false;
        if (playSoundResult != null && !((UnityEngine.Object) playSoundResult.ActingVariation == (UnityEngine.Object) null))
        {
          playSoundResult.ActingVariation.SoundFinished += (SoundGroupVariation.SoundFinishedEventHandler) (() => done = true);
          while (!done)
            yield return (object) DarkTonic.MasterAudio.MasterAudio.EndOfFrameDelay;
          if (completedAction != null)
            completedAction();
        }
      }
    }
  }

  public static IEnumerator PlaySound3DFollowTransformAndWaitUntilFinished(
    string sType,
    Transform sourceTrans,
    float volumePercentage = 1f,
    float? pitch = null,
    float delaySoundTime = 0.0f,
    string variationName = null,
    double? timeToSchedulePlay = null,
    System.Action completedAction = null)
  {
    if (DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
    {
      if (!DarkTonic.MasterAudio.MasterAudio.SoundsReady)
      {
        Debug.LogError((object) ("MasterAudio not finished initializing sounds. Cannot play: " + sType));
      }
      else
      {
        PlaySoundResult playSoundResult = DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransform(sType, sourceTrans, volumePercentage, pitch, delaySoundTime, variationName, timeToSchedulePlay, isSingleSubscribedPlay: true);
        bool done = false;
        if (playSoundResult != null && !((UnityEngine.Object) playSoundResult.ActingVariation == (UnityEngine.Object) null))
        {
          playSoundResult.ActingVariation.SoundFinished += (SoundGroupVariation.SoundFinishedEventHandler) (() => done = true);
          while (!done)
            yield return (object) DarkTonic.MasterAudio.MasterAudio.EndOfFrameDelay;
          if (completedAction != null)
            completedAction();
        }
      }
    }
  }

  public static bool PSRAsSuccessBool(PlaySoundResult psr)
  {
    if (psr == null)
      return false;
    return psr.SoundPlayed || psr.SoundScheduled;
  }

  public static PlaySoundResult PlaySoundAtVolume(
    string sType,
    float volumePercentage,
    Vector3 sourcePosition,
    double? timeToSchedulePlay,
    float? pitch = null,
    Transform sourceTrans = null,
    string variationName = null,
    bool attachToSource = false,
    float delaySoundTime = 0.0f,
    bool useVector3 = false,
    bool makePlaySoundResult = false,
    bool isChaining = false,
    bool isSingleSubscribedPlay = false,
    bool triggeredAsChildGroup = false)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return (PlaySoundResult) null;
    if (!DarkTonic.MasterAudio.MasterAudio.SoundsReady || sType == string.Empty || sType == "[None]")
      return (PlaySoundResult) null;
    if (!DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.ContainsKey(sType))
    {
      string msg = $"MasterAudio could not find sound: {sType}. If your Scene just changed, this could happen when an OnDisable or OnInvisible event sound happened to a per-scene sound, which is expected.";
      if ((UnityEngine.Object) sourceTrans != (UnityEngine.Object) null)
        msg = $"{msg} Triggered by prefab: {sourceTrans.name}";
      DarkTonic.MasterAudio.MasterAudio.LogWarning(msg);
      return (PlaySoundResult) null;
    }
    DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo audioGroupInfo = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType];
    MasterAudioGroup group = audioGroupInfo.Group;
    bool loggingEnabledForGrp = DarkTonic.MasterAudio.MasterAudio.LoggingEnabledForGroup(group);
    if (DarkTonic.MasterAudio.MasterAudio.Instance.mixerMuted)
    {
      if (loggingEnabledForGrp)
        DarkTonic.MasterAudio.MasterAudio.LogMessage($"MasterAudio playing sound: {sType} silently because the Mixer is muted.");
    }
    else if (group.isMuted && loggingEnabledForGrp)
      DarkTonic.MasterAudio.MasterAudio.LogMessage($"MasterAudio playing sound: {sType} silently because the Group is muted.");
    if (DarkTonic.MasterAudio.MasterAudio.Instance.SoloedGroups.Count > 0 && !DarkTonic.MasterAudio.MasterAudio.Instance.SoloedGroups.Contains(group) && loggingEnabledForGrp)
      DarkTonic.MasterAudio.MasterAudio.LogMessage($"MasterAudio playing sound: {sType} silently because there are one or more Groups soloed. This one is not.");
    audioGroupInfo.PlayedForWarming = DarkTonic.MasterAudio.MasterAudio.IsWarming;
    if (group.curVariationMode == MasterAudioGroup.VariationMode.Normal)
    {
      switch (group.limitMode)
      {
        case MasterAudioGroup.LimitMode.FrameBased:
          if (Time.frameCount - audioGroupInfo.LastFramePlayed < group.limitPerXFrames)
          {
            if (loggingEnabledForGrp)
              DarkTonic.MasterAudio.MasterAudio.LogMessage($"Master Audio skipped playing sound: {sType} due to Group's Per Frame Limit.");
            return (PlaySoundResult) null;
          }
          break;
        case MasterAudioGroup.LimitMode.TimeBased:
          if ((double) group.minimumTimeBetween > 0.0 && (double) Time.realtimeSinceStartup < (double) audioGroupInfo.LastTimePlayed + (double) group.minimumTimeBetween)
          {
            if (loggingEnabledForGrp)
              DarkTonic.MasterAudio.MasterAudio.LogMessage($"MasterAudio skipped playing sound: {sType} due to Group's Min Seconds Between setting.");
            return (PlaySoundResult) null;
          }
          break;
      }
    }
    DarkTonic.MasterAudio.MasterAudio.SetLastPlayed(audioGroupInfo);
    List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> sources = audioGroupInfo.Sources;
    bool isNonSpecific = string.IsNullOrEmpty(variationName);
    if (sources.Count == 0)
    {
      if (loggingEnabledForGrp)
        DarkTonic.MasterAudio.MasterAudio.LogMessage($"Sound Group {{{sType}}} has no active Variations.");
      return (PlaySoundResult) null;
    }
    if (group.curVariationMode == MasterAudioGroup.VariationMode.Normal && audioGroupInfo.Group.limitPolyphony)
    {
      int voiceLimitCount = audioGroupInfo.Group.voiceLimitCount;
      int num = 0;
      for (int index = 0; index < audioGroupInfo.Sources.Count; ++index)
      {
        if (!((UnityEngine.Object) audioGroupInfo.Sources[index].Source == (UnityEngine.Object) null) && audioGroupInfo.Sources[index].Source.isPlaying)
        {
          ++num;
          if (num >= voiceLimitCount)
          {
            if (loggingEnabledForGrp || DarkTonic.MasterAudio.MasterAudio.LogOutOfVoices)
              DarkTonic.MasterAudio.MasterAudio.LogMessage($"Polyphony limit of group: {audioGroupInfo.Group.GameObjectName} exceeded. Will not play this sound for this instance.");
            return (PlaySoundResult) null;
          }
        }
      }
    }
    GroupBus busForGroup = audioGroupInfo.Group.BusForGroup;
    if (busForGroup != null && busForGroup.BusVoiceLimitReached)
    {
      if (!busForGroup.stopOldest)
      {
        if (loggingEnabledForGrp || DarkTonic.MasterAudio.MasterAudio.LogOutOfVoices)
          DarkTonic.MasterAudio.MasterAudio.LogMessage($"Bus voice limit has been reached. Cannot play the sound: {audioGroupInfo.Group.GameObjectName} until one voice has stopped playing. You can turn on the 'Stop Oldest' option for the bus to change ");
        return (PlaySoundResult) null;
      }
      DarkTonic.MasterAudio.MasterAudio.StopOldestSoundOnBus(busForGroup);
    }
    DarkTonic.MasterAudio.MasterAudio.AudioInfo info = (DarkTonic.MasterAudio.MasterAudio.AudioInfo) null;
    bool isSingleVarLoop = false;
    if (sources.Count == 1)
    {
      if (loggingEnabledForGrp)
        DarkTonic.MasterAudio.MasterAudio.LogMessage("Cueing only child of " + sType);
      info = sources[0];
      if (group.curVariationMode == MasterAudioGroup.VariationMode.LoopedChain)
        isSingleVarLoop = true;
    }
    List<int> intList1 = (List<int>) null;
    int? randomIndex = new int?();
    List<int> intList2 = (List<int>) null;
    int num1 = -1;
    if (info == null)
    {
      if (!DarkTonic.MasterAudio.MasterAudio.Instance._randomizer.ContainsKey(sType))
      {
        Debug.Log((object) $"Sound Group {{{sType}}} has no active Variations.");
        return (PlaySoundResult) null;
      }
      if (isNonSpecific)
      {
        intList1 = DarkTonic.MasterAudio.MasterAudio.Instance._randomizer[sType];
        randomIndex = new int?(0);
        num1 = intList1[randomIndex.Value];
        info = sources[num1];
        intList2 = DarkTonic.MasterAudio.MasterAudio.Instance._randomizerLeftovers[sType];
        intList2.Remove(num1);
        if (loggingEnabledForGrp)
          DarkTonic.MasterAudio.MasterAudio.LogMessage($"Cueing child {intList1[randomIndex.Value]} of {sType}");
      }
      else
      {
        bool flag = false;
        int num2 = 0;
        for (int index = 0; index < sources.Count; ++index)
        {
          DarkTonic.MasterAudio.MasterAudio.AudioInfo audioInfo = sources[index];
          if (!(audioInfo.Source.name != variationName))
          {
            ++num2;
            if (audioInfo.Variation.IsAvailableToPlay)
            {
              info = audioInfo;
              flag = true;
              num1 = index;
              break;
            }
          }
        }
        if (!flag)
        {
          if (num2 == 0)
          {
            if (loggingEnabledForGrp)
              DarkTonic.MasterAudio.MasterAudio.LogMessage($"Can't find variation {{{variationName}}} of {sType}");
          }
          else if (loggingEnabledForGrp || DarkTonic.MasterAudio.MasterAudio.LogOutOfVoices)
            DarkTonic.MasterAudio.MasterAudio.LogMessage($"Can't find non-busy variation {{{variationName}}} of {sType}");
          return (PlaySoundResult) null;
        }
        if (loggingEnabledForGrp)
          DarkTonic.MasterAudio.MasterAudio.LogMessage($"Cueing child named '{variationName}' of {sType}");
      }
    }
    if ((UnityEngine.Object) info.Variation == (UnityEngine.Object) null)
    {
      if (DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown || (UnityEngine.Object) info.Source == (UnityEngine.Object) null)
        return (PlaySoundResult) null;
      SoundGroupVariation component = info.Source.GetComponent<SoundGroupVariation>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        return (PlaySoundResult) null;
      info.Variation = component;
    }
    if (info.Variation.audLocation == DarkTonic.MasterAudio.MasterAudio.AudioLocation.Clip && (UnityEngine.Object) info.Variation.VarAudio.clip == (UnityEngine.Object) null)
    {
      if (loggingEnabledForGrp)
        DarkTonic.MasterAudio.MasterAudio.LogMessage($"Child named '{info.Variation.name}' of {sType} has no audio assigned to it so nothing will be played.");
      DarkTonic.MasterAudio.MasterAudio.RemoveClipAndRefillIfEmpty(audioGroupInfo, isNonSpecific, randomIndex, intList1, sType, num1, loggingEnabledForGrp, false);
      DarkTonic.MasterAudio.MasterAudio.MaybeChainNextVar(isChaining, info.Variation, volumePercentage, pitch, sourceTrans, attachToSource);
      return (PlaySoundResult) null;
    }
    if (info.Variation.probabilityToPlay < 100 && UnityEngine.Random.Range(0, 100) >= info.Variation.probabilityToPlay)
    {
      if (loggingEnabledForGrp)
        DarkTonic.MasterAudio.MasterAudio.LogMessage($"Child named '{info.Variation.name}' of {sType} failed its Random number check for 'Probability to Play' to it so nothing will be played this time.");
      DarkTonic.MasterAudio.MasterAudio.RemoveClipAndRefillIfEmpty(audioGroupInfo, isNonSpecific, randomIndex, intList1, sType, num1, loggingEnabledForGrp, false);
      DarkTonic.MasterAudio.MasterAudio.MaybeChainNextVar(isChaining, info.Variation, volumePercentage, pitch, sourceTrans, attachToSource);
      return (PlaySoundResult) null;
    }
    if (audioGroupInfo.Group.curVariationMode == MasterAudioGroup.VariationMode.Dialog)
    {
      if (audioGroupInfo.Group.useDialogFadeOut)
        DarkTonic.MasterAudio.MasterAudio.FadeOutAllOfSound(audioGroupInfo.Group.GameObjectName, audioGroupInfo.Group.dialogFadeOutTime);
      else
        DarkTonic.MasterAudio.MasterAudio.StopAllOfSound(audioGroupInfo.Group.GameObjectName);
    }
    bool flag1 = false;
    bool forgetSoundPlayed = false;
    bool flag2 = false;
    bool flag3;
    PlaySoundResult playSoundResult;
    bool flag4;
    do
    {
      flag3 = false;
      playSoundResult = DarkTonic.MasterAudio.MasterAudio.PlaySoundIfAvailable(info, sourcePosition, volumePercentage, ref forgetSoundPlayed, pitch, audioGroupInfo, sourceTrans, attachToSource, delaySoundTime, useVector3, makePlaySoundResult, timeToSchedulePlay, isChaining, isSingleSubscribedPlay);
      flag4 = ((!makePlaySoundResult ? 0 : (playSoundResult == null ? 0 : (playSoundResult.SoundPlayed ? 1 : (playSoundResult.SoundScheduled ? 1 : 0)))) | (!makePlaySoundResult & forgetSoundPlayed ? 1 : 0)) != 0;
      if (flag4)
      {
        flag1 = true;
        if (!DarkTonic.MasterAudio.MasterAudio.IsWarming)
          DarkTonic.MasterAudio.MasterAudio.RemoveClipAndRefillIfEmpty(audioGroupInfo, isNonSpecific, randomIndex, intList1, sType, num1, loggingEnabledForGrp, isSingleVarLoop);
      }
      else if (isNonSpecific)
      {
        if (intList2 != null)
        {
          if (intList2.Count <= 0)
          {
            if (!flag2)
            {
              DarkTonic.MasterAudio.MasterAudio.RefillSoundGroupPool(sType);
              flag2 = true;
              intList2.Clear();
              intList2.AddRange((IEnumerable<int>) intList1);
            }
            else
              goto label_108;
          }
          info = sources[intList2[0]];
          if ((UnityEngine.Object) info.Variation == (UnityEngine.Object) null)
          {
            SoundGroupVariation component = info.Source.GetComponent<SoundGroupVariation>();
            if (!((UnityEngine.Object) component == (UnityEngine.Object) null))
              info.Variation = component;
            else
              break;
          }
          if (loggingEnabledForGrp)
            DarkTonic.MasterAudio.MasterAudio.LogMessage($"Child was busy. Cueing child {{{info.Source.name}}} of {sType}");
          intList2.RemoveAt(0);
          if (flag2 && intList2.Count == 0)
            flag3 = true;
        }
      }
      else
      {
        if (loggingEnabledForGrp)
          DarkTonic.MasterAudio.MasterAudio.LogMessage("Child was busy. Since you wanted a named Variation, no others to try. Aborting.");
        intList2?.Clear();
      }
label_108:;
    }
    while (!flag1 && intList2 != null && ((intList2.Count > 0 ? 1 : (!flag2 ? 1 : 0)) | (flag3 ? 1 : 0)) != 0);
    if (!flag4)
    {
      if (loggingEnabledForGrp || DarkTonic.MasterAudio.MasterAudio.LogOutOfVoices)
        DarkTonic.MasterAudio.MasterAudio.LogMessage($"All children of {sType} were busy. Will not play this sound for this instance.");
    }
    else
    {
      if (!triggeredAsChildGroup && !DarkTonic.MasterAudio.MasterAudio.IsWarming)
      {
        switch (audioGroupInfo.Group.linkedStartGroupSelectionType)
        {
          case DarkTonic.MasterAudio.MasterAudio.LinkedGroupSelectionType.All:
            for (int index = 0; index < audioGroupInfo.Group.childSoundGroups.Count; ++index)
              DarkTonic.MasterAudio.MasterAudio.PlaySoundAtVolume(audioGroupInfo.Group.childSoundGroups[index], volumePercentage, sourcePosition, timeToSchedulePlay, pitch, sourceTrans, attachToSource: attachToSource, delaySoundTime: delaySoundTime, useVector3: useVector3, triggeredAsChildGroup: true);
            break;
          case DarkTonic.MasterAudio.MasterAudio.LinkedGroupSelectionType.OneAtRandom:
            int index1 = UnityEngine.Random.Range(0, audioGroupInfo.Group.childSoundGroups.Count);
            DarkTonic.MasterAudio.MasterAudio.PlaySoundAtVolume(audioGroupInfo.Group.childSoundGroups[index1], volumePercentage, sourcePosition, timeToSchedulePlay, pitch, sourceTrans, attachToSource: attachToSource, delaySoundTime: delaySoundTime, useVector3: useVector3, triggeredAsChildGroup: true);
            break;
        }
      }
      if (audioGroupInfo.Group.soundPlayedEventActive)
        DarkTonic.MasterAudio.MasterAudio.FireCustomEvent(audioGroupInfo.Group.soundPlayedCustomEvent, DarkTonic.MasterAudio.MasterAudio.Instance._trans);
    }
    return !makePlaySoundResult & flag4 ? DarkTonic.MasterAudio.MasterAudio.AndForgetSuccessResult : playSoundResult;
  }

  public static void MaybeChainNextVar(
    bool isChaining,
    SoundGroupVariation variation,
    float volumePercentage,
    float? pitch,
    Transform sourceTrans,
    bool attachToSource)
  {
    if (!isChaining)
      return;
    variation.DoNextChain(volumePercentage, pitch, sourceTrans, attachToSource);
  }

  public static void SetLastPlayed(DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo grp)
  {
    grp.LastTimePlayed = AudioUtil.Time;
    grp.LastFramePlayed = AudioUtil.FrameCount;
  }

  public static void RemoveClipAndRefillIfEmpty(
    DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo grp,
    bool isNonSpecific,
    int? randomIndex,
    List<int> choices,
    string sType,
    int pickedChoice,
    bool loggingEnabledForGrp,
    bool isSingleVarLoop)
  {
    if (isSingleVarLoop)
    {
      ++grp.Group.ChainLoopCount;
    }
    else
    {
      if (isNonSpecific && randomIndex.HasValue)
      {
        choices.RemoveAt(randomIndex.Value);
        DarkTonic.MasterAudio.MasterAudio.Instance._clipsPlayedBySoundTypeOldestFirst[sType].Add(pickedChoice);
        if (choices.Count == 0)
        {
          if (loggingEnabledForGrp)
            DarkTonic.MasterAudio.MasterAudio.LogMessage("Refilling Variation pool: " + sType);
          DarkTonic.MasterAudio.MasterAudio.RefillSoundGroupPool(sType);
        }
      }
      if (grp.Group.curVariationSequence != MasterAudioGroup.VariationSequence.TopToBottom || !grp.Group.useInactivePeriodPoolRefill)
        return;
      DarkTonic.MasterAudio.MasterAudio.UpdateRefillTime(sType, grp.Group.inactivePeriodSeconds);
    }
  }

  public static PlaySoundResult PlaySoundIfAvailable(
    DarkTonic.MasterAudio.MasterAudio.AudioInfo info,
    Vector3 sourcePosition,
    float volumePercentage,
    ref bool forgetSoundPlayed,
    float? pitch = null,
    DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo audioGroup = null,
    Transform sourceTrans = null,
    bool attachToSource = false,
    float delaySoundTime = 0.0f,
    bool useVector3 = false,
    bool makePlaySoundResult = false,
    double? timeToSchedulePlay = null,
    bool isChaining = false,
    bool isSingleSubscribedPlay = false)
  {
    if ((UnityEngine.Object) info.Source == (UnityEngine.Object) null)
      return (PlaySoundResult) null;
    MasterAudioGroup group = audioGroup.Group;
    if (group.curVariationMode == MasterAudioGroup.VariationMode.Normal && info.Source.isPlaying && (double) AudioUtil.GetAudioPlayedPercentage(info.Source) < (double) group.retriggerPercentage)
      return (PlaySoundResult) null;
    info.Variation.Stop(skipLinked: true);
    info.Variation.ObjectToFollow = (Transform) null;
    bool useClipAgePriority = DarkTonic.MasterAudio.MasterAudio.Instance.prioritizeOnDistance && (DarkTonic.MasterAudio.MasterAudio.Instance.useClipAgePriority || info.Variation.ParentGroup.useClipAgePriority);
    if (useVector3)
    {
      info.Source.transform.position = sourcePosition;
      if (DarkTonic.MasterAudio.MasterAudio.Instance.prioritizeOnDistance)
        AudioPrioritizer.Set3DPriority(info.Variation, useClipAgePriority);
    }
    else if ((UnityEngine.Object) sourceTrans != (UnityEngine.Object) null)
    {
      if (attachToSource)
      {
        info.Variation.ObjectToFollow = sourceTrans;
      }
      else
      {
        info.Source.transform.position = sourceTrans.position;
        info.Variation.ObjectToTriggerFrom = sourceTrans;
      }
      if (DarkTonic.MasterAudio.MasterAudio.Instance.prioritizeOnDistance)
        AudioPrioritizer.Set3DPriority(info.Variation, useClipAgePriority);
    }
    else
    {
      if (DarkTonic.MasterAudio.MasterAudio.Instance.prioritizeOnDistance)
        AudioPrioritizer.Set2DSoundPriority(info.Source);
      info.Source.transform.localPosition = Vector3.zero;
    }
    float groupMasterVolume = group.groupMasterVolume;
    float busVolume = DarkTonic.MasterAudio.MasterAudio.GetBusVolume(group);
    float num1 = info.OriginalVolume;
    float num2 = 0.0f;
    if (info.Variation.useRandomVolume)
    {
      num2 = UnityEngine.Random.Range(info.Variation.randomVolumeMin, info.Variation.randomVolumeMax);
      switch (info.Variation.randomVolumeMode)
      {
        case SoundGroupVariation.RandomVolumeMode.AddToClipVolume:
          num1 += num2;
          break;
        case SoundGroupVariation.RandomVolumeMode.IgnoreClipVolume:
          num1 = num2;
          break;
      }
    }
    float targetVol = num1 * groupMasterVolume * busVolume * DarkTonic.MasterAudio.MasterAudio.Instance._masterAudioVolume;
    float maxVolume = targetVol * volumePercentage;
    info.Source.volume = maxVolume;
    info.LastPercentageVolume = volumePercentage;
    info.LastRandomVolume = num2;
    if (!info.Variation.GameObj.activeInHierarchy)
      return (PlaySoundResult) null;
    PlaySoundResult playSoundResult = (PlaySoundResult) null;
    if (makePlaySoundResult)
    {
      playSoundResult = new PlaySoundResult()
      {
        ActingVariation = info.Variation
      };
      if ((double) delaySoundTime > 0.0)
        playSoundResult.SoundScheduled = true;
      else
        playSoundResult.SoundPlayed = true;
    }
    else
      forgetSoundPlayed = true;
    string gameObjectName = group.GameObjectName;
    if (group.curVariationMode == MasterAudioGroup.VariationMode.LoopedChain)
    {
      if (!isChaining)
        group.ChainLoopCount = 0;
      Transform objectToFollow = info.Variation.ObjectToFollow;
      if (group.ActiveVoices > 0 && !isChaining)
        DarkTonic.MasterAudio.MasterAudio.StopAllOfSound(gameObjectName);
      info.Variation.ObjectToFollow = objectToFollow;
    }
    info.Variation.Play(pitch, maxVolume, gameObjectName, volumePercentage, targetVol, pitch, sourceTrans, attachToSource, delaySoundTime, timeToSchedulePlay, isChaining, isSingleSubscribedPlay);
    if (DarkTonic.MasterAudio.MasterAudio.Instance._isStoppingMultiple)
      DarkTonic.MasterAudio.MasterAudio.Instance.VariationsStartedDuringMultiStop.Add(info.Variation);
    return playSoundResult;
  }

  public static void DuckSoundGroup(string soundGroupName, AudioSource aSource)
  {
    DarkTonic.MasterAudio.MasterAudio instance = DarkTonic.MasterAudio.MasterAudio.Instance;
    if (!instance.EnableMusicDucking || !instance.duckingBySoundType.ContainsKey(soundGroupName) || (UnityEngine.Object) aSource.clip == (UnityEngine.Object) null)
      return;
    DuckGroupInfo duckGroupInfo = instance.duckingBySoundType[soundGroupName];
    float length = aSource.clip.length;
    float pitch = aSource.pitch;
    List<PlaylistController> instances = PlaylistController.Instances;
    for (int index = 0; index < instances.Count; ++index)
      instances[index].DuckMusicForTime(length, duckGroupInfo.unduckTime, pitch, duckGroupInfo.riseVolStart, duckGroupInfo.duckedVolumeCut);
  }

  public static void StopPauseOrUnpauseSoundsOfTransform(
    Transform trans,
    List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> varList,
    DarkTonic.MasterAudio.MasterAudio.VariationCommand varCmd)
  {
    MasterAudioGroup masterAudioGroup = (MasterAudioGroup) null;
    for (int index = 0; index < varList.Count; ++index)
    {
      SoundGroupVariation variation = varList[index].Variation;
      if (variation.WasTriggeredFromTransform(trans))
      {
        if ((UnityEngine.Object) masterAudioGroup == (UnityEngine.Object) null)
          masterAudioGroup = DarkTonic.MasterAudio.MasterAudio.GrabGroup(variation.ParentGroup.GameObjectName);
        bool stopEndDetection = (UnityEngine.Object) masterAudioGroup != (UnityEngine.Object) null && masterAudioGroup.curVariationMode == MasterAudioGroup.VariationMode.LoopedChain;
        switch (varCmd)
        {
          case DarkTonic.MasterAudio.MasterAudio.VariationCommand.Stop:
            variation.Stop(stopEndDetection);
            continue;
          case DarkTonic.MasterAudio.MasterAudio.VariationCommand.Pause:
            variation.Pause();
            continue;
          case DarkTonic.MasterAudio.MasterAudio.VariationCommand.Unpause:
            if (AudioUtil.IsAudioPaused(variation.VarAudio))
            {
              variation.VarAudio.Play();
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }
  }

  public static void StopAllSoundsOfTransform(Transform sourceTrans)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return;
    DarkTonic.MasterAudio.MasterAudio.Instance.VariationsStartedDuringMultiStop.Clear();
    DarkTonic.MasterAudio.MasterAudio.Instance._isStoppingMultiple = true;
    foreach (string key in DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.Keys)
    {
      List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> sources = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[key].Sources;
      DarkTonic.MasterAudio.MasterAudio.StopPauseOrUnpauseSoundsOfTransform(sourceTrans, sources, DarkTonic.MasterAudio.MasterAudio.VariationCommand.Stop);
    }
    DarkTonic.MasterAudio.MasterAudio.Instance._isStoppingMultiple = false;
  }

  public static void StopSoundGroupOfTransform(Transform sourceTrans, string sType)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return;
    if (!DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.ContainsKey(sType))
    {
      Debug.LogWarning((object) $"Could not locate group '{sType}'.");
    }
    else
    {
      List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> sources = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType].Sources;
      DarkTonic.MasterAudio.MasterAudio.StopPauseOrUnpauseSoundsOfTransform(sourceTrans, sources, DarkTonic.MasterAudio.MasterAudio.VariationCommand.Stop);
    }
  }

  public static void PauseAllSoundsOfTransform(Transform sourceTrans)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return;
    foreach (string key in DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.Keys)
    {
      List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> sources = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[key].Sources;
      DarkTonic.MasterAudio.MasterAudio.StopPauseOrUnpauseSoundsOfTransform(sourceTrans, sources, DarkTonic.MasterAudio.MasterAudio.VariationCommand.Pause);
    }
  }

  public static void PauseSoundGroupOfTransform(Transform sourceTrans, string sType)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return;
    if (!DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.ContainsKey(sType))
    {
      Debug.LogWarning((object) $"Could not locate group '{sType}'.");
    }
    else
    {
      List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> sources = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType].Sources;
      DarkTonic.MasterAudio.MasterAudio.StopPauseOrUnpauseSoundsOfTransform(sourceTrans, sources, DarkTonic.MasterAudio.MasterAudio.VariationCommand.Pause);
    }
  }

  public static void UnpauseAllSoundsOfTransform(Transform sourceTrans)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return;
    foreach (string key in DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.Keys)
    {
      List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> sources = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[key].Sources;
      DarkTonic.MasterAudio.MasterAudio.StopPauseOrUnpauseSoundsOfTransform(sourceTrans, sources, DarkTonic.MasterAudio.MasterAudio.VariationCommand.Unpause);
    }
  }

  public static void UnpauseSoundGroupOfTransform(Transform sourceTrans, string sType)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return;
    if (!DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.ContainsKey(sType))
    {
      Debug.LogWarning((object) $"Could not locate group '{sType}'.");
    }
    else
    {
      List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> sources = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType].Sources;
      DarkTonic.MasterAudio.MasterAudio.StopPauseOrUnpauseSoundsOfTransform(sourceTrans, sources, DarkTonic.MasterAudio.MasterAudio.VariationCommand.Unpause);
    }
  }

  public static void FadeOutAllSoundsOfTransform(Transform sourceTrans, float fadeTime)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return;
    List<SoundGroupVariation> variationsOfTransform = DarkTonic.MasterAudio.MasterAudio.GetAllPlayingVariationsOfTransform(sourceTrans);
    HashSet<string> stringSet = new HashSet<string>();
    for (int index = 0; index < variationsOfTransform.Count; ++index)
    {
      string name = variationsOfTransform[index].ParentGroup.name;
      if (!stringSet.Contains(name))
      {
        stringSet.Add(name);
        DarkTonic.MasterAudio.MasterAudio.FadeOutSoundGroupOfTransform(sourceTrans, name, fadeTime);
      }
    }
  }

  public static void FadeOutSoundGroupOfTransform(
    Transform sourceTrans,
    string sType,
    float fadeTime)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return;
    if (!DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.ContainsKey(sType))
    {
      Debug.LogWarning((object) $"Could not locate group '{sType}'.");
    }
    else
    {
      List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> sources = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType].Sources;
      for (int index = 0; index < sources.Count; ++index)
      {
        SoundGroupVariation variation = sources[index].Variation;
        if (variation.WasTriggeredFromTransform(sourceTrans))
          variation.FadeOutNow(fadeTime);
      }
    }
  }

  public static void StopAllOfSound(string sType)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return;
    if (!DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.ContainsKey(sType))
    {
      Debug.LogWarning((object) $"Could not locate group '{sType}'.");
    }
    else
    {
      List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> sources = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType].Sources;
      MasterAudioGroup masterAudioGroup = DarkTonic.MasterAudio.MasterAudio.GrabGroup(sType);
      bool stopEndDetection = (UnityEngine.Object) masterAudioGroup != (UnityEngine.Object) null && masterAudioGroup.curVariationMode == MasterAudioGroup.VariationMode.LoopedChain;
      foreach (DarkTonic.MasterAudio.MasterAudio.AudioInfo audioInfo in sources)
      {
        if (!((UnityEngine.Object) audioInfo.Variation == (UnityEngine.Object) null) && !DarkTonic.MasterAudio.MasterAudio.IsLinkedGroupPlay(audioInfo.Variation))
          audioInfo.Variation.Stop(stopEndDetection);
      }
    }
  }

  public static void FadeOutAllOfSound(string sType, float fadeTime)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return;
    if (!DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.ContainsKey(sType))
    {
      Debug.LogWarning((object) $"Could not locate group '{sType}'.");
    }
    else
    {
      foreach (DarkTonic.MasterAudio.MasterAudio.AudioInfo source in DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType].Sources)
        source.Variation.FadeOutNow(fadeTime);
    }
  }

  public static List<SoundGroupVariation> GetAllPlayingVariations()
  {
    List<SoundGroupVariation> playingVariations = new List<SoundGroupVariation>(32 /*0x20*/);
    foreach (string key in DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.Keys)
    {
      List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> sources = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[key].Sources;
      for (int index = 0; index < sources.Count; ++index)
      {
        SoundGroupVariation variation = sources[index].Variation;
        if (variation.IsPlaying)
          playingVariations.Add(variation);
      }
    }
    return playingVariations;
  }

  public static List<SoundGroupVariation> GetAllPlayingVariationsOfTransform(Transform sourceTrans)
  {
    List<SoundGroupVariation> variationsOfTransform = new List<SoundGroupVariation>(32 /*0x20*/);
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return variationsOfTransform;
    foreach (string key in DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.Keys)
    {
      List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> sources = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[key].Sources;
      for (int index = 0; index < sources.Count; ++index)
      {
        SoundGroupVariation variation = sources[index].Variation;
        if (variation.WasTriggeredFromTransform(sourceTrans))
          variationsOfTransform.Add(variation);
      }
    }
    return variationsOfTransform;
  }

  public static List<SoundGroupVariation> GetAllPlayingVariationsOfTransformList(
    List<Transform> sourceTransList)
  {
    List<SoundGroupVariation> variationsOfTransformList = new List<SoundGroupVariation>(32 /*0x20*/);
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return variationsOfTransformList;
    HashSet<Transform> transMap = new HashSet<Transform>();
    for (int index = 0; index < sourceTransList.Count; ++index)
      transMap.Add(sourceTransList[index]);
    foreach (string key in DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.Keys)
    {
      List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> sources = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[key].Sources;
      for (int index = 0; index < sources.Count; ++index)
      {
        SoundGroupVariation variation = sources[index].Variation;
        if (variation.WasTriggeredFromAnyOfTransformMap(transMap))
          variationsOfTransformList.Add(variation);
      }
    }
    return variationsOfTransformList;
  }

  public static List<SoundGroupVariation> GetAllPlayingVariationsInBus(string busName)
  {
    List<SoundGroupVariation> playingVariationsInBus = new List<SoundGroupVariation>(32 /*0x20*/);
    int busIndex = DarkTonic.MasterAudio.MasterAudio.GetBusIndex(busName, false);
    if (busIndex < 0)
      return playingVariationsInBus;
    Dictionary<string, DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo>.Enumerator enumerator = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.GetEnumerator();
    while (enumerator.MoveNext())
    {
      DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo audioGroupInfo = enumerator.Current.Value;
      if (audioGroupInfo.Group.busIndex == busIndex)
      {
        for (int index = 0; index < audioGroupInfo.Sources.Count; ++index)
        {
          SoundGroupVariation variation = audioGroupInfo.Sources[index].Variation;
          if (variation.IsPlaying)
            playingVariationsInBus.Add(variation);
        }
      }
    }
    return playingVariationsInBus;
  }

  public static void DeleteGroupVariation(string sType, string variationName)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SoundsReady)
      Debug.LogError((object) "MasterAudio not finished initializing sounds. Cannot delete Variation clip yet.");
    else if (!DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.ContainsKey(sType))
    {
      Debug.LogWarning((object) $"Could not locate group '{sType}'.");
    }
    else
    {
      DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo audioGroupInfo = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType];
      List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> audioInfoList = new List<DarkTonic.MasterAudio.MasterAudio.AudioInfo>();
      for (int index = 0; index < audioGroupInfo.Sources.Count; ++index)
      {
        DarkTonic.MasterAudio.MasterAudio.AudioInfo source = audioGroupInfo.Sources[index];
        if (!(source.Variation.name != variationName))
          audioInfoList.Add(source);
      }
      if (audioInfoList.Count == 0)
      {
        DarkTonic.MasterAudio.MasterAudio.LogWarning($"Could not find Variation for '{sType}' Group named '{variationName}'.\nWill not delete any Variations.");
      }
      else
      {
        for (int index1 = 0; index1 < audioInfoList.Count; ++index1)
        {
          DarkTonic.MasterAudio.MasterAudio.AudioInfo audioInfo = audioInfoList[index1];
          SoundGroupVariation variation = audioInfo.Variation;
          variation.Stop();
          variation.DisableUpdater();
          if (variation.audLocation == DarkTonic.MasterAudio.MasterAudio.AudioLocation.ResourceFile)
            AudioResourceOptimizer.DeleteAudioSourceFromList((UnityEngine.Object) variation.VarAudio.clip == (UnityEngine.Object) null ? string.Empty : variation.VarAudio.clip.name, variation.VarAudio);
          int index2 = audioGroupInfo.Sources.IndexOf(audioInfo);
          if (index2 >= 0)
          {
            DarkTonic.MasterAudio.MasterAudio.Instance._randomizer[sType].Remove(index2);
            for (int index3 = 0; index3 < DarkTonic.MasterAudio.MasterAudio.Instance._randomizer[sType].Count; ++index3)
            {
              if (DarkTonic.MasterAudio.MasterAudio.Instance._randomizer[sType][index3] > index2)
                DarkTonic.MasterAudio.MasterAudio.Instance._randomizer[sType][index3]--;
            }
            DarkTonic.MasterAudio.MasterAudio.Instance._randomizerOrigin[sType].Remove(index2);
            for (int index4 = 0; index4 < DarkTonic.MasterAudio.MasterAudio.Instance._randomizerOrigin[sType].Count; ++index4)
            {
              if (DarkTonic.MasterAudio.MasterAudio.Instance._randomizerOrigin[sType][index4] > index2)
                DarkTonic.MasterAudio.MasterAudio.Instance._randomizerOrigin[sType][index4]--;
            }
            DarkTonic.MasterAudio.MasterAudio.Instance._randomizerLeftovers[sType].Remove(index2);
            for (int index5 = 0; index5 < DarkTonic.MasterAudio.MasterAudio.Instance._randomizerLeftovers[sType].Count; ++index5)
            {
              if (DarkTonic.MasterAudio.MasterAudio.Instance._randomizerLeftovers[sType][index5] > index2)
                DarkTonic.MasterAudio.MasterAudio.Instance._randomizerLeftovers[sType][index5]--;
            }
            DarkTonic.MasterAudio.MasterAudio.Instance._clipsPlayedBySoundTypeOldestFirst[sType].Remove(index2);
            audioGroupInfo.Sources.RemoveAt(index2);
          }
          DarkTonic.MasterAudio.MasterAudio.Instance.OcclusionSourcesInRange.Remove(variation.GameObj);
          DarkTonic.MasterAudio.MasterAudio.Instance.OcclusionSourcesOutOfRange.Remove(variation.GameObj);
          DarkTonic.MasterAudio.MasterAudio.Instance.OcclusionSourcesBlocked.Remove(variation.GameObj);
          DarkTonic.MasterAudio.MasterAudio.RemoveFromOcclusionFrequencyTransitioning(variation);
          DarkTonic.MasterAudio.MasterAudio.Instance.AllAudioSources.Remove(variation.VarAudio);
          UnityEngine.Object.Destroy((UnityEngine.Object) variation.GameObj);
        }
      }
    }
  }

  public static void CreateGroupVariationFromClip(
    string sType,
    AudioClip clip,
    string variationName,
    float volume = 1f,
    float pitch = 1f)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SoundsReady)
      Debug.LogError((object) "MasterAudio not finished initializing sounds. Cannot create change variation clip yet.");
    else if (!DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.ContainsKey(sType))
    {
      Debug.LogWarning((object) $"Could not locate group '{sType}'.");
    }
    else
    {
      DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo audioGroupInfo = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType];
      bool flag = false;
      for (int index = 0; index < audioGroupInfo.Sources.Count; ++index)
      {
        if (!(audioGroupInfo.Sources[index].Variation.name != variationName))
        {
          flag = true;
          break;
        }
      }
      if (flag)
        DarkTonic.MasterAudio.MasterAudio.LogWarning($"You already have a Variation for this Group named '{variationName}'. \n\nPlease rename these Variations when finished to be unique, or you may not be able to play them by name if you have a need to.");
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(DarkTonic.MasterAudio.MasterAudio.Instance.soundGroupVariationTemplate.gameObject, audioGroupInfo.Group.transform.position, Quaternion.identity);
      gameObject.transform.name = variationName;
      gameObject.transform.parent = audioGroupInfo.Group.transform;
      AudioSource component1 = gameObject.GetComponent<AudioSource>();
      component1.clip = clip;
      component1.pitch = pitch;
      DarkTonic.MasterAudio.MasterAudio.Instance.AllAudioSources.Add(component1);
      SoundGroupVariation component2 = gameObject.GetComponent<SoundGroupVariation>();
      component2.DisableUpdater();
      DarkTonic.MasterAudio.MasterAudio.AudioInfo audioInfo = new DarkTonic.MasterAudio.MasterAudio.AudioInfo(component2, component2.VarAudio, volume);
      audioGroupInfo.Sources.Add(audioInfo);
      if (!DarkTonic.MasterAudio.MasterAudio.Instance._randomizer.ContainsKey(sType))
        return;
      int num = audioGroupInfo.Sources.Count - 1;
      DarkTonic.MasterAudio.MasterAudio.Instance._randomizer[sType].Add(num);
      DarkTonic.MasterAudio.MasterAudio.Instance._randomizerOrigin[sType].Add(num);
      DarkTonic.MasterAudio.MasterAudio.Instance._randomizerLeftovers[sType].Add(audioGroupInfo.Sources.Count - 1);
    }
  }

  public static void ChangeVariationPitch(
    string sType,
    bool changeAllVariations,
    string variationName,
    float pitch)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SoundsReady)
      Debug.LogError((object) "MasterAudio not finished initializing sounds. Cannot change variation clip yet.");
    else if (!DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.ContainsKey(sType))
    {
      Debug.LogWarning((object) $"Could not locate group '{sType}'.");
    }
    else
    {
      DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo audioGroupInfo = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType];
      int num = 0;
      for (int index = 0; index < audioGroupInfo.Sources.Count; ++index)
      {
        DarkTonic.MasterAudio.MasterAudio.AudioInfo source = audioGroupInfo.Sources[index];
        if (changeAllVariations || !(source.Source.transform.name != variationName))
        {
          source.Variation.original_pitch = pitch;
          AudioSource varAudio = source.Variation.VarAudio;
          if ((UnityEngine.Object) varAudio != (UnityEngine.Object) null)
            varAudio.pitch = pitch;
          ++num;
        }
      }
      if (num != 0 || changeAllVariations)
        return;
      Debug.Log((object) $"Could not find any matching variations of Sound Group '{sType}' to change the pitch of.");
    }
  }

  public static void ChangeVariationVolume(
    string sType,
    bool changeAllVariations,
    string variationName,
    float volume)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SoundsReady)
      Debug.LogError((object) "MasterAudio not finished initializing sounds. Cannot change variation clip yet.");
    else if (!DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.ContainsKey(sType))
    {
      Debug.LogWarning((object) $"Could not locate group '{sType}'.");
    }
    else
    {
      DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo audioGroupInfo = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType];
      int num = 0;
      for (int index = 0; index < audioGroupInfo.Sources.Count; ++index)
      {
        DarkTonic.MasterAudio.MasterAudio.AudioInfo source = audioGroupInfo.Sources[index];
        if (changeAllVariations || !(source.Source.transform.name != variationName))
        {
          source.OriginalVolume = volume;
          ++num;
        }
      }
      if (num != 0 || changeAllVariations)
        return;
      Debug.Log((object) $"Could not find any matching variations of Sound Group '{sType}' to change the volume of.");
    }
  }

  public static void ChangeVariationClipFromResources(
    string sType,
    bool changeAllVariations,
    string variationName,
    string resourceFileName)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SoundsReady)
    {
      Debug.LogError((object) "MasterAudio not finished initializing sounds. Cannot create change variation clip yet.");
    }
    else
    {
      AudioClip clip = Resources.Load(resourceFileName) as AudioClip;
      if ((UnityEngine.Object) clip == (UnityEngine.Object) null)
        DarkTonic.MasterAudio.MasterAudio.LogWarning($"Resource file '{resourceFileName}' could not be located.");
      else
        DarkTonic.MasterAudio.MasterAudio.ChangeVariationClip(sType, changeAllVariations, variationName, clip);
    }
  }

  public static void ChangeVariationClip(
    string sType,
    bool changeAllVariations,
    string variationName,
    AudioClip clip)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SoundsReady)
      Debug.LogError((object) "MasterAudio not finished initializing sounds. Cannot create change variation clip yet.");
    else if (!DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.ContainsKey(sType))
    {
      Debug.LogWarning((object) $"Could not locate group '{sType}'.");
    }
    else
    {
      DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo audioGroupInfo = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType];
      for (int index = 0; index < audioGroupInfo.Sources.Count; ++index)
      {
        DarkTonic.MasterAudio.MasterAudio.AudioInfo source = audioGroupInfo.Sources[index];
        if (changeAllVariations || source.Source.transform.name == variationName)
        {
          if (source.Variation.IsPlaying)
            source.Variation.Stop();
          source.Source.clip = clip;
        }
      }
    }
  }

  public static void GradualOcclusionFreqChange(
    SoundGroupVariation variation,
    float fadeTime,
    float newCutoffFreq)
  {
    if (DarkTonic.MasterAudio.MasterAudio.IsOcclusionFreqencyTransitioning(variation))
      DarkTonic.MasterAudio.MasterAudio.LogWarning($"Occlusion is already fading for: {variation.name}. This is a bug.");
    else
      DarkTonic.MasterAudio.MasterAudio.Instance.VariationOcclusionFreqChanges.Add(new OcclusionFreqChangeInfo()
      {
        ActingVariation = variation,
        CompletionTime = Time.realtimeSinceStartup + fadeTime,
        IsActive = true,
        StartFrequency = variation.LowPassFilter.cutoffFrequency,
        StartTime = Time.realtimeSinceStartup,
        TargetFrequency = newCutoffFreq
      });
  }

  public static bool IsSoundGroupPlaying(string sType)
  {
    MasterAudioGroup masterAudioGroup = DarkTonic.MasterAudio.MasterAudio.GrabGroup(sType, false);
    return !((UnityEngine.Object) masterAudioGroup == (UnityEngine.Object) null) && !DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown && masterAudioGroup.ActiveVoices > 0;
  }

  public static bool IsTransformPlayingSoundGroup(string sType, Transform sourceTrans)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return false;
    if (!DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.ContainsKey(sType))
    {
      Debug.LogWarning((object) $"Could not locate group '{sType}'.");
      return false;
    }
    List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> sources = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType].Sources;
    for (int index = 0; index < sources.Count; ++index)
    {
      if (sources[index].Variation.WasTriggeredFromTransform(sourceTrans))
        return true;
    }
    return false;
  }

  public static void RouteGroupToBus(string sType, string busName)
  {
    MasterAudioGroup masterAudioGroup = DarkTonic.MasterAudio.MasterAudio.GrabGroup(sType);
    if ((UnityEngine.Object) masterAudioGroup == (UnityEngine.Object) null)
    {
      DarkTonic.MasterAudio.MasterAudio.LogError($"Could not find Sound Group '{sType}'");
    }
    else
    {
      int num = 0;
      if (busName != null)
      {
        int index = DarkTonic.MasterAudio.MasterAudio.GroupBuses.FindIndex((Predicate<GroupBus>) (x => x.busName == busName));
        if (index < 0)
        {
          DarkTonic.MasterAudio.MasterAudio.LogError($"Could not find bus '{busName}' to assign to Sound Group '{sType}'");
          return;
        }
        num = 2 + index;
      }
      GroupBus busByIndex = DarkTonic.MasterAudio.MasterAudio.GetBusByIndex(masterAudioGroup.busIndex);
      masterAudioGroup.busIndex = num;
      GroupBus bus = (GroupBus) null;
      bool flag1 = false;
      if (num > 0)
      {
        bus = DarkTonic.MasterAudio.MasterAudio.GroupBuses.Find((Predicate<GroupBus>) (x => x.busName == busName));
        if (bus.isMuted)
        {
          DarkTonic.MasterAudio.MasterAudio.MuteGroup(masterAudioGroup.name, false);
          flag1 = true;
        }
        else if (bus.isSoloed)
        {
          DarkTonic.MasterAudio.MasterAudio.SoloGroup(masterAudioGroup.name, false);
          flag1 = true;
        }
      }
      bool flag2 = false;
      List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> sources = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType].Sources;
      for (int index = 0; index < sources.Count; ++index)
      {
        SoundGroupVariation variation = sources[index].Variation;
        variation.SetMixerGroup();
        variation.SetSpatialBlend();
        if (variation.IsPlaying)
        {
          bus?.AddActiveAudioSourceId(variation.InstanceId);
          busByIndex?.RemoveActiveAudioSourceId(variation.InstanceId);
          flag2 = true;
        }
      }
      if (flag2)
        DarkTonic.MasterAudio.MasterAudio.SetBusVolume(bus, bus != null ? bus.volume : 0.0f);
      if (!(Application.isPlaying & flag1))
        return;
      DarkTonic.MasterAudio.MasterAudio.SilenceOrUnsilenceGroupsFromSoloChange();
    }
  }

  public static float GetVariationLength(string sType, string variationName)
  {
    MasterAudioGroup masterAudioGroup = DarkTonic.MasterAudio.MasterAudio.GrabGroup(sType);
    if ((UnityEngine.Object) masterAudioGroup == (UnityEngine.Object) null)
      return -1f;
    SoundGroupVariation soundGroupVariation = (SoundGroupVariation) null;
    foreach (SoundGroupVariation groupVariation in masterAudioGroup.groupVariations)
    {
      if (!(groupVariation.name != variationName))
      {
        soundGroupVariation = groupVariation;
        break;
      }
    }
    if ((UnityEngine.Object) soundGroupVariation == (UnityEngine.Object) null)
    {
      DarkTonic.MasterAudio.MasterAudio.LogError($"Could not find Variation '{variationName}' in Sound Group '{sType}'.");
      return -1f;
    }
    if (soundGroupVariation.audLocation == DarkTonic.MasterAudio.MasterAudio.AudioLocation.ResourceFile)
    {
      DarkTonic.MasterAudio.MasterAudio.LogError($"Variation '{variationName}' in Sound Group '{sType}' length cannot be determined because it's a Resource Files.");
      return -1f;
    }
    if (soundGroupVariation.audLocation == DarkTonic.MasterAudio.MasterAudio.AudioLocation.FileOnInternet)
    {
      DarkTonic.MasterAudio.MasterAudio.LogError($"Variation '{variationName}' in Sound Group '{sType}' length cannot be determined because it's an Internet File.");
      return -1f;
    }
    AudioClip clip = soundGroupVariation.VarAudio.clip;
    if ((UnityEngine.Object) clip == (UnityEngine.Object) null)
    {
      DarkTonic.MasterAudio.MasterAudio.LogError($"Variation '{variationName}' in Sound Group '{sType}' has no Audio Clip.");
      return -1f;
    }
    if ((double) soundGroupVariation.VarAudio.pitch > 0.0)
      return AudioUtil.AdjustAudioClipDurationForPitch(clip.length, soundGroupVariation.VarAudio);
    DarkTonic.MasterAudio.MasterAudio.LogError($"Variation '{variationName}' in Sound Group '{sType}' has negative or zero pitch. Cannot compute length.");
    return -1f;
  }

  public static void RefillSoundGroupPool(string sType)
  {
    MasterAudioGroup masterAudioGroup = DarkTonic.MasterAudio.MasterAudio.GrabGroup(sType, false);
    if ((UnityEngine.Object) masterAudioGroup == (UnityEngine.Object) null)
      return;
    List<int> intList1 = DarkTonic.MasterAudio.MasterAudio.Instance._randomizer[sType];
    List<int> list = DarkTonic.MasterAudio.MasterAudio.Instance._clipsPlayedBySoundTypeOldestFirst[sType];
    if (intList1.Count > 0)
    {
      for (int index = 0; index < intList1.Count; ++index)
      {
        int num = intList1[index];
        if (!list.Contains(num))
          list.Add(num);
      }
    }
    List<int> intList2 = DarkTonic.MasterAudio.MasterAudio.Instance._randomizerOrigin[sType];
    if (list.Count < intList2.Count)
    {
      for (int index = 0; index < intList2.Count; ++index)
      {
        int num = intList2[index];
        if (!list.Contains(num))
          list.Add(num);
      }
    }
    intList1.Clear();
    if (masterAudioGroup.curVariationSequence == MasterAudioGroup.VariationSequence.Randomized)
    {
      int? nullable = new int?();
      if (masterAudioGroup.UsesNoRepeat && list.Count > 0)
        nullable = new int?(list[list.Count - 1]);
      ArrayListUtil.SortIntArray(ref list);
      if (nullable.HasValue && nullable.Value == list[0])
      {
        int num = list[0];
        list.RemoveAt(0);
        list.Insert(UnityEngine.Random.Range(1, list.Count), num);
      }
    }
    intList1.AddRange((IEnumerable<int>) list);
    DarkTonic.MasterAudio.MasterAudio.Instance._randomizerLeftovers[sType].AddRange((IEnumerable<int>) list);
    list.Clear();
    if (masterAudioGroup.curVariationMode != MasterAudioGroup.VariationMode.LoopedChain)
      return;
    ++masterAudioGroup.ChainLoopCount;
  }

  public static bool SoundGroupExists(string sType)
  {
    return (UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.GrabGroup(sType, false) != (UnityEngine.Object) null;
  }

  public static void PauseSoundGroup(string sType)
  {
    if ((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.GrabGroup(sType) == (UnityEngine.Object) null)
      return;
    List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> sources = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType].Sources;
    for (int index = 0; index < sources.Count; ++index)
      sources[index].Variation.Pause();
  }

  public static void SetGroupSpatialBlend(string sType)
  {
    if ((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.GrabGroup(sType) == (UnityEngine.Object) null)
      return;
    List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> sources = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType].Sources;
    for (int index = 0; index < sources.Count; ++index)
      sources[index].Variation.SetSpatialBlend();
  }

  public static void RouteGroupToUnityMixerGroup(string sType, AudioMixerGroup mixerGroup)
  {
    if (!Application.isPlaying || (UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.GrabGroup(sType, false) == (UnityEngine.Object) null)
      return;
    List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> sources = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType].Sources;
    for (int index = 0; index < sources.Count; ++index)
      sources[index].Variation.VarAudio.outputAudioMixerGroup = mixerGroup;
  }

  public static void UnpauseSoundGroup(string sType)
  {
    if ((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.GrabGroup(sType) == (UnityEngine.Object) null)
      return;
    List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> sources = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType].Sources;
    for (int index = 0; index < sources.Count; ++index)
    {
      SoundGroupVariation variation = sources[index].Variation;
      if (AudioUtil.IsAudioPaused(variation.VarAudio))
      {
        variation.VarAudio.Play();
        if ((UnityEngine.Object) variation.VariationUpdater != (UnityEngine.Object) null)
        {
          variation.VariationUpdater.enabled = true;
          variation.VariationUpdater.Unpause();
        }
      }
    }
  }

  public static void FadeSoundGroupToVolume(
    string sType,
    float newVolume,
    float fadeTime,
    System.Action completionCallback = null,
    bool willStopAfterFade = false,
    bool willResetVolumeAfterFade = false)
  {
    if ((double) newVolume < 0.0 || (double) newVolume > 1.0)
      Debug.LogError((object) $"Illegal volume passed to FadeSoundGroupToVolume: '{newVolume.ToString()}'. Legal volumes are between 0 and 1");
    else if ((double) fadeTime <= 0.10000000149011612)
    {
      DarkTonic.MasterAudio.MasterAudio.SetGroupVolume(sType, newVolume);
      if (completionCallback != null)
        completionCallback();
      if (!willStopAfterFade)
        return;
      DarkTonic.MasterAudio.MasterAudio.StopAllOfSound(sType);
    }
    else
    {
      MasterAudioGroup masterAudioGroup = DarkTonic.MasterAudio.MasterAudio.GrabGroup(sType);
      if ((UnityEngine.Object) masterAudioGroup == (UnityEngine.Object) null)
        return;
      if ((double) newVolume < 0.0 || (double) newVolume > 1.0)
      {
        Debug.Log((object) $"Cannot fade Sound Group '{sType}'. Invalid volume specified. Volume should be between 0 and 1.");
      }
      else
      {
        GroupFadeInfo groupFadeInfo1 = DarkTonic.MasterAudio.MasterAudio.Instance.GroupFades.Find((Predicate<GroupFadeInfo>) (obj => obj.NameOfGroup == sType));
        if (groupFadeInfo1 != null)
          groupFadeInfo1.IsActive = false;
        GroupFadeInfo groupFadeInfo2 = new GroupFadeInfo()
        {
          NameOfGroup = sType,
          ActingGroup = masterAudioGroup,
          StartTime = AudioUtil.Time,
          CompletionTime = AudioUtil.Time + fadeTime,
          StartVolume = masterAudioGroup.groupMasterVolume,
          TargetVolume = newVolume,
          WillStopGroupAfterFade = willStopAfterFade,
          WillResetVolumeAfterFade = willResetVolumeAfterFade
        };
        if (completionCallback != null)
          groupFadeInfo2.completionAction = completionCallback;
        DarkTonic.MasterAudio.MasterAudio.Instance.GroupFades.Add(groupFadeInfo2);
      }
    }
  }

  public static void GlideSoundGroupByPitch(
    string sType,
    float pitchAddition,
    float glideTime,
    System.Action completionCallback = null)
  {
    if ((double) pitchAddition < -3.0 || (double) pitchAddition > 3.0)
      Debug.LogError((object) $"Illegal pitch passed to GlideSoundGroupByPitch: '{pitchAddition.ToString()}'. Legal pitches are between -3 and 3");
    else if ((double) pitchAddition == 0.0)
    {
      if (completionCallback == null)
        return;
      completionCallback();
    }
    else
    {
      MasterAudioGroup masterAudioGroup = DarkTonic.MasterAudio.MasterAudio.GrabGroup(sType);
      if ((UnityEngine.Object) masterAudioGroup == (UnityEngine.Object) null)
        return;
      GroupPitchGlideInfo groupPitchGlideInfo1 = DarkTonic.MasterAudio.MasterAudio.Instance.GroupPitchGlides.Find((Predicate<GroupPitchGlideInfo>) (obj => obj.NameOfGroup == sType));
      if (groupPitchGlideInfo1 != null)
      {
        groupPitchGlideInfo1.IsActive = false;
        if (groupPitchGlideInfo1.completionAction != null)
          groupPitchGlideInfo1.completionAction();
      }
      DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo audioGroupInfo = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType];
      if ((double) glideTime <= 0.10000000149011612)
      {
        for (int index = 0; index < audioGroupInfo.Sources.Count; ++index)
          audioGroupInfo.Sources[index].Variation.GlideByPitch(pitchAddition, 0.0f);
        if (completionCallback == null)
          return;
        completionCallback();
      }
      else
      {
        List<SoundGroupVariation> soundGroupVariationList = new List<SoundGroupVariation>(audioGroupInfo.Sources.Count);
        for (int index = 0; index < audioGroupInfo.Sources.Count; ++index)
        {
          SoundGroupVariation variation = audioGroupInfo.Sources[index].Variation;
          if (variation.IsPlaying)
          {
            if (variation.curPitchMode == SoundGroupVariation.PitchMode.Gliding)
              variation.VariationUpdater.StopPitchGliding();
            variation.GlideByPitch(pitchAddition, glideTime);
            soundGroupVariationList.Add(variation);
          }
        }
        if (soundGroupVariationList.Count == 0 || completionCallback == null)
        {
          if (completionCallback == null)
            return;
          completionCallback();
        }
        else
        {
          GroupPitchGlideInfo groupPitchGlideInfo2 = new GroupPitchGlideInfo()
          {
            NameOfGroup = sType,
            ActingGroup = masterAudioGroup,
            CompletionTime = AudioUtil.Time + glideTime,
            GlidingVariations = soundGroupVariationList
          };
          if (completionCallback != null)
            groupPitchGlideInfo2.completionAction = completionCallback;
          DarkTonic.MasterAudio.MasterAudio.Instance.GroupPitchGlides.Add(groupPitchGlideInfo2);
        }
      }
    }
  }

  public static void DeleteSoundGroup(string sType)
  {
    if ((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (UnityEngine.Object) null)
      return;
    MasterAudioGroup masterAudioGroup = DarkTonic.MasterAudio.MasterAudio.GrabGroup(sType);
    if ((UnityEngine.Object) masterAudioGroup == (UnityEngine.Object) null)
      return;
    DarkTonic.MasterAudio.MasterAudio.StopAllOfSound(sType);
    Transform transform = masterAudioGroup.transform;
    DarkTonic.MasterAudio.MasterAudio instance = DarkTonic.MasterAudio.MasterAudio.Instance;
    if (instance.duckingBySoundType.ContainsKey(sType))
      instance.duckingBySoundType.Remove(sType);
    DarkTonic.MasterAudio.MasterAudio.Instance._randomizer.Remove(sType);
    DarkTonic.MasterAudio.MasterAudio.Instance._randomizerLeftovers.Remove(sType);
    DarkTonic.MasterAudio.MasterAudio.Instance._randomizerOrigin.Remove(sType);
    DarkTonic.MasterAudio.MasterAudio.Instance._clipsPlayedBySoundTypeOldestFirst.Remove(sType);
    DarkTonic.MasterAudio.MasterAudio.RemoveRuntimeGroupInfo(sType);
    DarkTonic.MasterAudio.MasterAudio.Instance.LastTimeSoundGroupPlayed.Remove(sType);
    for (int index = 0; index < transform.childCount; ++index)
    {
      Transform child = transform.GetChild(index);
      AudioSource component1 = child.GetComponent<AudioSource>();
      SoundGroupVariation component2 = child.GetComponent<SoundGroupVariation>();
      switch (component2.audLocation)
      {
        case DarkTonic.MasterAudio.MasterAudio.AudioLocation.ResourceFile:
          AudioResourceOptimizer.DeleteAudioSourceFromList(AudioResourceOptimizer.GetLocalizedFileName(component2.useLocalization, component2.resourceFileName), component1);
          break;
        case DarkTonic.MasterAudio.MasterAudio.AudioLocation.FileOnInternet:
          AudioResourceOptimizer.DeleteAudioSourceFromList(component2.internetFileUrl, component1);
          break;
      }
    }
    transform.parent = (Transform) null;
    UnityEngine.Object.Destroy((UnityEngine.Object) transform.gameObject);
    DarkTonic.MasterAudio.MasterAudio.RescanGroupsNow();
  }

  public static Transform CreateSoundGroup(
    DynamicSoundGroup aGroup,
    string creatorObjectName,
    bool errorOnExisting = true)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return (Transform) null;
    if (!DarkTonic.MasterAudio.MasterAudio.SoundsReady)
    {
      Debug.LogError((object) "MasterAudio not finished initializing sounds. Cannot create new group yet.");
      return (Transform) null;
    }
    string name = aGroup.transform.name;
    DarkTonic.MasterAudio.MasterAudio instance = DarkTonic.MasterAudio.MasterAudio.Instance;
    if ((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.Instance.Trans.GetChildTransform(name) != (UnityEngine.Object) null)
    {
      if (errorOnExisting)
        Debug.LogError((object) $"Cannot add a new Sound Group named '{name}' because there is already a Sound Group of that name.");
      return (Transform) null;
    }
    GameObject gameObject1 = UnityEngine.Object.Instantiate<GameObject>(instance.soundGroupTemplate.gameObject, instance.Trans.position, Quaternion.identity);
    Transform transform = gameObject1.transform;
    transform.name = UtilStrings.TrimSpace(name);
    transform.parent = DarkTonic.MasterAudio.MasterAudio.Instance.Trans;
    transform.gameObject.layer = DarkTonic.MasterAudio.MasterAudio.Instance.gameObject.layer;
    for (int index1 = 0; index1 < aGroup.groupVariations.Count; ++index1)
    {
      DynamicGroupVariation groupVariation = aGroup.groupVariations[index1];
      for (int index2 = 0; index2 < groupVariation.weight; ++index2)
      {
        GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(groupVariation.gameObject, transform.position, Quaternion.identity);
        gameObject2.transform.parent = transform;
        gameObject2.transform.gameObject.layer = transform.gameObject.layer;
        UnityEngine.Object.Destroy((UnityEngine.Object) gameObject2.GetComponent<DynamicGroupVariation>());
        gameObject2.AddComponent<SoundGroupVariation>();
        SoundGroupVariation component1 = gameObject2.GetComponent<SoundGroupVariation>();
        string str = component1.name;
        int length = str.IndexOf("(Clone)");
        if (length >= 0)
          str = str.Substring(0, length);
        AudioSource component2 = groupVariation.GetComponent<AudioSource>();
        switch (groupVariation.audLocation)
        {
          case DarkTonic.MasterAudio.MasterAudio.AudioLocation.Clip:
            AudioClip clip = component2.clip;
            component1.VarAudio.clip = clip;
            break;
          case DarkTonic.MasterAudio.MasterAudio.AudioLocation.ResourceFile:
            AudioResourceOptimizer.AddTargetForClip(AudioResourceOptimizer.GetLocalizedFileName(groupVariation.useLocalization, groupVariation.resourceFileName), component1.VarAudio);
            component1.resourceFileName = groupVariation.resourceFileName;
            component1.useLocalization = groupVariation.useLocalization;
            break;
          case DarkTonic.MasterAudio.MasterAudio.AudioLocation.FileOnInternet:
            AudioResourceOptimizer.AddTargetForClip(groupVariation.internetFileUrl, component1.VarAudio);
            component1.internetFileUrl = groupVariation.internetFileUrl;
            break;
        }
        component1.audLocation = groupVariation.audLocation;
        component1.original_pitch = component2.pitch;
        component1.transform.name = str;
        component1.isExpanded = groupVariation.isExpanded;
        component1.probabilityToPlay = groupVariation.probabilityToPlay;
        component1.useRandomPitch = groupVariation.useRandomPitch;
        component1.randomPitchMode = groupVariation.randomPitchMode;
        component1.randomPitchMin = groupVariation.randomPitchMin;
        component1.randomPitchMax = groupVariation.randomPitchMax;
        component1.useRandomVolume = groupVariation.useRandomVolume;
        component1.randomVolumeMode = groupVariation.randomVolumeMode;
        component1.randomVolumeMin = groupVariation.randomVolumeMin;
        component1.randomVolumeMax = groupVariation.randomVolumeMax;
        component1.useCustomLooping = groupVariation.useCustomLooping;
        component1.minCustomLoops = groupVariation.minCustomLoops;
        component1.maxCustomLoops = groupVariation.maxCustomLoops;
        component1.useFades = groupVariation.useFades;
        component1.fadeInTime = groupVariation.fadeInTime;
        component1.fadeOutTime = groupVariation.fadeOutTime;
        component1.useIntroSilence = groupVariation.useIntroSilence;
        component1.introSilenceMin = groupVariation.introSilenceMin;
        component1.introSilenceMax = groupVariation.introSilenceMax;
        component1.useRandomStartTime = groupVariation.useRandomStartTime;
        component1.randomStartMinPercent = groupVariation.randomStartMinPercent;
        component1.randomStartMaxPercent = groupVariation.randomStartMaxPercent;
        component1.randomEndPercent = groupVariation.randomEndPercent;
        if ((UnityEngine.Object) component1.LowPassFilter != (UnityEngine.Object) null && !component1.LowPassFilter.enabled)
          UnityEngine.Object.Destroy((UnityEngine.Object) component1.LowPassFilter);
        if ((UnityEngine.Object) component1.HighPassFilter != (UnityEngine.Object) null && !component1.HighPassFilter.enabled)
          UnityEngine.Object.Destroy((UnityEngine.Object) component1.HighPassFilter);
        if ((UnityEngine.Object) component1.DistortionFilter != (UnityEngine.Object) null && !component1.DistortionFilter.enabled)
          UnityEngine.Object.Destroy((UnityEngine.Object) component1.DistortionFilter);
        if ((UnityEngine.Object) component1.ChorusFilter != (UnityEngine.Object) null && !component1.ChorusFilter.enabled)
          UnityEngine.Object.Destroy((UnityEngine.Object) component1.ChorusFilter);
        if ((UnityEngine.Object) component1.EchoFilter != (UnityEngine.Object) null && !component1.EchoFilter.enabled)
          UnityEngine.Object.Destroy((UnityEngine.Object) component1.EchoFilter);
        if ((UnityEngine.Object) component1.ReverbFilter != (UnityEngine.Object) null && !component1.ReverbFilter.enabled)
          UnityEngine.Object.Destroy((UnityEngine.Object) component1.ReverbFilter);
      }
    }
    MasterAudioGroup component3 = gameObject1.GetComponent<MasterAudioGroup>();
    component3.retriggerPercentage = aGroup.retriggerPercentage;
    float? groupVolume = PersistentAudioSettings.GetGroupVolume(aGroup.name);
    component3.OriginalVolume = aGroup.groupMasterVolume;
    component3.groupMasterVolume = !groupVolume.HasValue ? aGroup.groupMasterVolume : groupVolume.Value;
    component3.limitMode = aGroup.limitMode;
    component3.limitPerXFrames = aGroup.limitPerXFrames;
    component3.minimumTimeBetween = aGroup.minimumTimeBetween;
    component3.limitPolyphony = aGroup.limitPolyphony;
    component3.voiceLimitCount = aGroup.voiceLimitCount;
    component3.curVariationSequence = aGroup.curVariationSequence;
    component3.useInactivePeriodPoolRefill = aGroup.useInactivePeriodPoolRefill;
    component3.inactivePeriodSeconds = aGroup.inactivePeriodSeconds;
    component3.curVariationMode = aGroup.curVariationMode;
    component3.useNoRepeatRefill = aGroup.useNoRepeatRefill;
    component3.useDialogFadeOut = aGroup.useDialogFadeOut;
    component3.dialogFadeOutTime = aGroup.dialogFadeOutTime;
    component3.isUsingOcclusion = aGroup.isUsingOcclusion;
    component3.willOcclusionOverrideRaycastOffset = aGroup.willOcclusionOverrideRaycastOffset;
    component3.occlusionRayCastOffset = aGroup.occlusionRayCastOffset;
    component3.willOcclusionOverrideFrequencies = aGroup.willOcclusionOverrideFrequencies;
    component3.occlusionMaxCutoffFreq = aGroup.occlusionMaxCutoffFreq;
    component3.occlusionMinCutoffFreq = aGroup.occlusionMinCutoffFreq;
    component3.chainLoopDelayMin = aGroup.chainLoopDelayMin;
    component3.chainLoopDelayMax = aGroup.chainLoopDelayMax;
    component3.chainLoopMode = aGroup.chainLoopMode;
    component3.chainLoopNumLoops = aGroup.chainLoopNumLoops;
    component3.expandLinkedGroups = aGroup.expandLinkedGroups;
    component3.childSoundGroups = aGroup.childSoundGroups;
    component3.endLinkedGroups = aGroup.endLinkedGroups;
    component3.linkedStartGroupSelectionType = aGroup.linkedStartGroupSelectionType;
    component3.linkedStopGroupSelectionType = aGroup.linkedStopGroupSelectionType;
    component3.soundPlayedEventActive = aGroup.soundPlayedEventActive;
    component3.soundPlayedCustomEvent = aGroup.soundPlayedCustomEvent;
    component3.targetDespawnedBehavior = aGroup.targetDespawnedBehavior;
    component3.despawnFadeTime = aGroup.despawnFadeTime;
    component3.resourceClipsAllLoadAsync = aGroup.resourceClipsAllLoadAsync;
    component3.logSound = aGroup.logSound;
    component3.alwaysHighestPriority = aGroup.alwaysHighestPriority;
    component3.spatialBlendType = aGroup.spatialBlendType;
    component3.spatialBlend = aGroup.spatialBlend;
    List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> sources = new List<DarkTonic.MasterAudio.MasterAudio.AudioInfo>();
    List<int> list = new List<int>();
    for (int index = 0; index < gameObject1.transform.childCount; ++index)
    {
      list.Add(index);
      Transform child = gameObject1.transform.GetChild(index);
      AudioSource component4 = child.GetComponent<AudioSource>();
      SoundGroupVariation component5 = child.GetComponent<SoundGroupVariation>();
      sources.Add(new DarkTonic.MasterAudio.MasterAudio.AudioInfo(component5, component4, component4.volume));
      component5.DisableUpdater();
    }
    DarkTonic.MasterAudio.MasterAudio.AddRuntimeGroupInfo(name, new DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo(sources, component3));
    if (component3.curVariationSequence == MasterAudioGroup.VariationSequence.Randomized)
      ArrayListUtil.SortIntArray(ref list);
    DarkTonic.MasterAudio.MasterAudio.Instance._randomizer.Add(name, list);
    List<int> intList = new List<int>(list.Count);
    intList.AddRange((IEnumerable<int>) list);
    DarkTonic.MasterAudio.MasterAudio.Instance._randomizerOrigin.Add(name, intList);
    DarkTonic.MasterAudio.MasterAudio.Instance._randomizerLeftovers.Add(name, new List<int>(list.Count));
    DarkTonic.MasterAudio.MasterAudio.Instance._randomizerLeftovers[name].AddRange((IEnumerable<int>) list);
    DarkTonic.MasterAudio.MasterAudio.Instance._clipsPlayedBySoundTypeOldestFirst.Add(name, new List<int>(list.Count));
    DarkTonic.MasterAudio.MasterAudio.RescanGroupsNow();
    if (string.IsNullOrEmpty(aGroup.busName))
      return transform;
    component3.busIndex = DarkTonic.MasterAudio.MasterAudio.GetBusIndex(aGroup.busName, true);
    if (component3.BusForGroup != null && component3.BusForGroup.isMuted)
      DarkTonic.MasterAudio.MasterAudio.MuteGroup(component3.name, false);
    else if (DarkTonic.MasterAudio.MasterAudio.Instance.mixerMuted)
      DarkTonic.MasterAudio.MasterAudio.MuteGroup(component3.name, false);
    return transform;
  }

  public static float GetGroupVolume(string sType)
  {
    MasterAudioGroup masterAudioGroup = DarkTonic.MasterAudio.MasterAudio.GrabGroup(sType);
    return (UnityEngine.Object) masterAudioGroup == (UnityEngine.Object) null ? 0.0f : masterAudioGroup.groupMasterVolume;
  }

  public static void SetGroupVolume(string sType, float volumeLevel)
  {
    MasterAudioGroup maGroup = DarkTonic.MasterAudio.MasterAudio.GrabGroup(sType, Application.isPlaying);
    if ((UnityEngine.Object) maGroup == (UnityEngine.Object) null || DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown)
      return;
    maGroup.groupMasterVolume = volumeLevel;
    DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo audioGroupInfo = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType];
    float busVolume = DarkTonic.MasterAudio.MasterAudio.GetBusVolume(maGroup);
    for (int index = 0; index < audioGroupInfo.Sources.Count; ++index)
    {
      DarkTonic.MasterAudio.MasterAudio.AudioInfo source1 = audioGroupInfo.Sources[index];
      AudioSource source2 = source1.Source;
      if (!((UnityEngine.Object) source2 == (UnityEngine.Object) null))
      {
        float num = source1.Variation.randomVolumeMode != SoundGroupVariation.RandomVolumeMode.AddToClipVolume ? source1.OriginalVolume * source1.LastPercentageVolume * maGroup.groupMasterVolume * busVolume * DarkTonic.MasterAudio.MasterAudio.Instance._masterAudioVolume : source1.OriginalVolume * source1.LastPercentageVolume * maGroup.groupMasterVolume * busVolume * DarkTonic.MasterAudio.MasterAudio.Instance._masterAudioVolume + source1.LastRandomVolume;
        source2.volume = num;
      }
    }
  }

  public static void MuteGroup(string sType, bool shouldCheckMuteStatus = true)
  {
    MasterAudioGroup aGroup = DarkTonic.MasterAudio.MasterAudio.GrabGroup(sType);
    if ((UnityEngine.Object) aGroup == (UnityEngine.Object) null)
      return;
    DarkTonic.MasterAudio.MasterAudio.Instance.SoloedGroups.Remove(aGroup);
    aGroup.isSoloed = false;
    DarkTonic.MasterAudio.MasterAudio.SetGroupMuteStatus(aGroup, sType, true);
    if (!shouldCheckMuteStatus)
      return;
    DarkTonic.MasterAudio.MasterAudio.SilenceOrUnsilenceGroupsFromSoloChange();
  }

  public static void UnmuteGroup(string sType, bool shouldCheckMuteStatus = true)
  {
    MasterAudioGroup aGroup = DarkTonic.MasterAudio.MasterAudio.GrabGroup(sType);
    if ((UnityEngine.Object) aGroup == (UnityEngine.Object) null)
      return;
    DarkTonic.MasterAudio.MasterAudio.SetGroupMuteStatus(aGroup, sType, false);
    if (!shouldCheckMuteStatus)
      return;
    DarkTonic.MasterAudio.MasterAudio.SilenceOrUnsilenceGroupsFromSoloChange();
  }

  public static void AddRuntimeGroupInfo(string groupName, DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo groupInfo)
  {
    DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.Add(groupName, groupInfo);
    List<AudioSource> sources = new List<AudioSource>(groupInfo.Sources.Count);
    for (int index = 0; index < groupInfo.Sources.Count; ++index)
      sources.Add(groupInfo.Sources[index].Source);
    DarkTonic.MasterAudio.MasterAudio.TrackRuntimeAudioSources(sources);
  }

  public static void FireAudioSourcesNumberChangedEvent()
  {
    if (DarkTonic.MasterAudio.MasterAudio.NumberOfAudioSourcesChanged == null)
      return;
    DarkTonic.MasterAudio.MasterAudio.NumberOfAudioSourcesChanged();
  }

  public static void TrackRuntimeAudioSources(List<AudioSource> sources)
  {
    bool flag = false;
    for (int index = 0; index < sources.Count; ++index)
    {
      AudioSource source = sources[index];
      if (!DarkTonic.MasterAudio.MasterAudio.Instance.AllAudioSources.Contains(source))
      {
        DarkTonic.MasterAudio.MasterAudio.Instance.AllAudioSources.Add(source);
        flag = true;
      }
    }
    if (!flag)
      return;
    DarkTonic.MasterAudio.MasterAudio.FireAudioSourcesNumberChangedEvent();
  }

  public static void StopTrackingRuntimeAudioSources(List<AudioSource> sources)
  {
    if (DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown || (UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (UnityEngine.Object) null)
      return;
    bool flag = false;
    for (int index = 0; index < sources.Count; ++index)
    {
      AudioSource source = sources[index];
      if (DarkTonic.MasterAudio.MasterAudio.Instance.AllAudioSources.Contains(source))
      {
        DarkTonic.MasterAudio.MasterAudio.Instance.AllAudioSources.Remove(source);
        flag = true;
      }
    }
    if (!flag)
      return;
    DarkTonic.MasterAudio.MasterAudio.FireAudioSourcesNumberChangedEvent();
  }

  public static void RemoveRuntimeGroupInfo(string groupName)
  {
    MasterAudioGroup masterAudioGroup = DarkTonic.MasterAudio.MasterAudio.GrabGroup(groupName);
    if ((UnityEngine.Object) masterAudioGroup != (UnityEngine.Object) null)
    {
      List<AudioSource> sources = new List<AudioSource>(masterAudioGroup.groupVariations.Count);
      for (int index = 0; index < masterAudioGroup.groupVariations.Count; ++index)
        sources.Add(masterAudioGroup.groupVariations[index].VarAudio);
      DarkTonic.MasterAudio.MasterAudio.StopTrackingRuntimeAudioSources(sources);
    }
    DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.Remove(groupName);
  }

  public static void RescanChildren(MasterAudioGroup group)
  {
    List<SoundGroupVariation> soundGroupVariationList = new List<SoundGroupVariation>();
    List<string> stringList = new List<string>();
    for (int index = 0; index < group.transform.childCount; ++index)
    {
      Transform child = group.transform.GetChild(index);
      if (!stringList.Contains(child.name))
      {
        stringList.Add(child.name);
        SoundGroupVariation component = child.GetComponent<SoundGroupVariation>();
        soundGroupVariationList.Add(component);
      }
    }
    group.groupVariations = soundGroupVariationList;
  }

  public static void SetGroupMuteStatus(MasterAudioGroup aGroup, string sType, bool isMute)
  {
    aGroup.isMuted = isMute;
    DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo audioGroupInfo = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType];
    for (int index = 0; index < audioGroupInfo.Sources.Count; ++index)
      audioGroupInfo.Sources[index].Source.mute = isMute;
  }

  public static void SoloGroup(string sType, bool shouldCheckMuteStatus = true)
  {
    MasterAudioGroup aGroup = DarkTonic.MasterAudio.MasterAudio.GrabGroup(sType);
    if ((UnityEngine.Object) aGroup == (UnityEngine.Object) null)
      return;
    aGroup.isMuted = false;
    aGroup.isSoloed = true;
    DarkTonic.MasterAudio.MasterAudio.Instance.SoloedGroups.Add(aGroup);
    DarkTonic.MasterAudio.MasterAudio.SetGroupMuteStatus(aGroup, sType, false);
    if (!shouldCheckMuteStatus)
      return;
    DarkTonic.MasterAudio.MasterAudio.SilenceOrUnsilenceGroupsFromSoloChange();
  }

  public static void SilenceOrUnsilenceGroupsFromSoloChange()
  {
    if (DarkTonic.MasterAudio.MasterAudio.Instance.SoloedGroups.Count > 0)
      DarkTonic.MasterAudio.MasterAudio.SilenceNonSoloedGroups();
    else
      DarkTonic.MasterAudio.MasterAudio.UnsilenceNonSoloedGroups();
  }

  public static void UnsilenceNonSoloedGroups()
  {
    foreach (DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo audioGroupInfo in DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.Values)
    {
      if (!audioGroupInfo.Group.isMuted)
        DarkTonic.MasterAudio.MasterAudio.UnsilenceGroup(audioGroupInfo.Group.GameObjectName);
    }
  }

  public static void UnsilenceGroup(string sType)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.ContainsKey(sType))
      return;
    DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo audioGroupInfo = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType];
    for (int index = 0; index < audioGroupInfo.Sources.Count; ++index)
      audioGroupInfo.Sources[index].Source.mute = false;
  }

  public static void SilenceNonSoloedGroups()
  {
    foreach (DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo audioGroupInfo in DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.Values)
    {
      if (!audioGroupInfo.Group.isSoloed && !audioGroupInfo.Group.isMuted)
        DarkTonic.MasterAudio.MasterAudio.SilenceGroup(audioGroupInfo.Group.GameObjectName);
    }
  }

  public static void SilenceGroup(string sType)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.ContainsKey(sType))
      return;
    DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo audioGroupInfo = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType];
    for (int index = 0; index < audioGroupInfo.Sources.Count; ++index)
      audioGroupInfo.Sources[index].Source.mute = true;
  }

  public static void UnsoloGroup(string sType, bool shouldCheckMuteStatus = true)
  {
    MasterAudioGroup masterAudioGroup = DarkTonic.MasterAudio.MasterAudio.GrabGroup(sType);
    if ((UnityEngine.Object) masterAudioGroup == (UnityEngine.Object) null)
      return;
    masterAudioGroup.isSoloed = false;
    DarkTonic.MasterAudio.MasterAudio.Instance.SoloedGroups.Remove(masterAudioGroup);
    if (!shouldCheckMuteStatus)
      return;
    DarkTonic.MasterAudio.MasterAudio.SilenceOrUnsilenceGroupsFromSoloChange();
  }

  public static MasterAudioGroup GrabGroup(string sType, bool logIfMissing = true)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.ContainsKey(sType))
    {
      if (logIfMissing)
        Debug.LogError((object) $"Could not grab Sound Group '{sType}' because it does not exist in this scene.");
      return (MasterAudioGroup) null;
    }
    DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo audioGroupInfo = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType];
    if ((UnityEngine.Object) audioGroupInfo.Group == (UnityEngine.Object) null)
    {
      Transform childTransform = DarkTonic.MasterAudio.MasterAudio.Instance.Trans.GetChildTransform(sType);
      if (!((UnityEngine.Object) childTransform != (UnityEngine.Object) null))
        return (MasterAudioGroup) null;
      MasterAudioGroup component = childTransform.GetComponent<MasterAudioGroup>();
      audioGroupInfo.Group = component;
    }
    MasterAudioGroup group = audioGroupInfo.Group;
    if (group.groupVariations.Count == 0)
      DarkTonic.MasterAudio.MasterAudio.RescanChildren(group);
    return group;
  }

  public static int VoicesForGroup(string sType)
  {
    return !DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.ContainsKey(sType) ? -1 : DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType].Sources.Count;
  }

  public static Transform FindGroupTransform(string sType)
  {
    if ((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance != (UnityEngine.Object) null)
    {
      Transform childTransform = DarkTonic.MasterAudio.MasterAudio.Instance.Trans.GetChildTransform(sType);
      if ((UnityEngine.Object) childTransform != (UnityEngine.Object) null)
        return childTransform;
    }
    DynamicSoundGroupCreator[] objectsOfType = UnityEngine.Object.FindObjectsOfType<DynamicSoundGroupCreator>();
    for (int index = 0; index < ((IEnumerable<DynamicSoundGroupCreator>) objectsOfType).Count<DynamicSoundGroupCreator>(); ++index)
    {
      Transform childTransform = objectsOfType[index].transform.GetChildTransform(sType);
      if ((UnityEngine.Object) childTransform != (UnityEngine.Object) null)
        return childTransform;
    }
    return (Transform) null;
  }

  public static List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> GetAllVariationsOfGroup(
    string sType,
    bool logIfMissing = true)
  {
    if (DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.ContainsKey(sType))
      return DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType].Sources;
    if (logIfMissing)
      Debug.LogError((object) $"Could not grab Sound Group '{sType}' because it does not exist in this scene.");
    return (List<DarkTonic.MasterAudio.MasterAudio.AudioInfo>) null;
  }

  public static DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo GetGroupInfo(string sType)
  {
    return !DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.ContainsKey(sType) ? (DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo) null : DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType];
  }

  public static void SubscribeToLastVariationPlayed(string sType, System.Action finishedCallback)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.ContainsKey(sType))
      Debug.LogError((object) $"Could not grab Sound Group '{sType}' because it does not exist in this scene.");
    else
      DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType].Group.SubscribeToLastVariationFinishedPlay(finishedCallback);
  }

  public static void UnsubscribeFromLastVariationPlayed(string sType)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.ContainsKey(sType))
      return;
    DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[sType].Group.UnsubscribeFromLastVariationFinishedPlay();
  }

  public void SetSpatialBlendForMixer()
  {
    foreach (string key in this.AudioSourcesBySoundType.Keys)
      DarkTonic.MasterAudio.MasterAudio.SetGroupSpatialBlend(key);
  }

  public static void PauseMixer()
  {
    foreach (string key in DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.Keys)
      DarkTonic.MasterAudio.MasterAudio.PauseSoundGroup(DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[key].Group.GameObjectName);
  }

  public static void UnpauseMixer()
  {
    foreach (string key in DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.Keys)
      DarkTonic.MasterAudio.MasterAudio.UnpauseSoundGroup(DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[key].Group.GameObjectName);
  }

  public static void StopMixer()
  {
    DarkTonic.MasterAudio.MasterAudio.Instance.VariationsStartedDuringMultiStop.Clear();
    DarkTonic.MasterAudio.MasterAudio.Instance._isStoppingMultiple = true;
    foreach (string key in DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.Keys)
      DarkTonic.MasterAudio.MasterAudio.StopAllOfSound(DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[key].Group.GameObjectName);
    DarkTonic.MasterAudio.MasterAudio.Instance._isStoppingMultiple = false;
  }

  public static void UnsubscribeFromAllVariations()
  {
    foreach (string key in DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.Keys)
    {
      List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> sources = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[key].Sources;
      for (int index = 0; index < sources.Count; ++index)
        sources[index].Variation.ClearSubscribers();
    }
  }

  public static void StopEverything()
  {
    DarkTonic.MasterAudio.MasterAudio.StopMixer();
    DarkTonic.MasterAudio.MasterAudio.StopAllPlaylists();
  }

  public static void PauseEverything()
  {
    DarkTonic.MasterAudio.MasterAudio.PauseMixer();
    DarkTonic.MasterAudio.MasterAudio.PauseAllPlaylists();
  }

  public static void UnpauseEverything()
  {
    DarkTonic.MasterAudio.MasterAudio.UnpauseMixer();
    DarkTonic.MasterAudio.MasterAudio.UnpauseAllPlaylists();
  }

  public static void MuteEverything()
  {
    DarkTonic.MasterAudio.MasterAudio.MixerMuted = true;
    DarkTonic.MasterAudio.MasterAudio.MuteAllPlaylists();
  }

  public static void UnmuteEverything()
  {
    DarkTonic.MasterAudio.MasterAudio.MixerMuted = false;
    DarkTonic.MasterAudio.MasterAudio.UnmuteAllPlaylists();
  }

  public static List<string> ListOfAudioClipsInGroupsEditTime()
  {
    List<string> stringList = new List<string>();
    for (int index1 = 0; index1 < DarkTonic.MasterAudio.MasterAudio.Instance.transform.childCount; ++index1)
    {
      MasterAudioGroup component1 = DarkTonic.MasterAudio.MasterAudio.Instance.transform.GetChild(index1).GetComponent<MasterAudioGroup>();
      for (int index2 = 0; index2 < component1.transform.childCount; ++index2)
      {
        SoundGroupVariation component2 = component1.transform.GetChild(index2).GetComponent<SoundGroupVariation>();
        string str = string.Empty;
        switch (component2.audLocation)
        {
          case DarkTonic.MasterAudio.MasterAudio.AudioLocation.Clip:
            AudioClip clip = component2.VarAudio.clip;
            if ((UnityEngine.Object) clip != (UnityEngine.Object) null)
            {
              str = clip.name;
              break;
            }
            break;
          case DarkTonic.MasterAudio.MasterAudio.AudioLocation.ResourceFile:
            str = component2.resourceFileName;
            break;
          case DarkTonic.MasterAudio.MasterAudio.AudioLocation.FileOnInternet:
            str = component2.internetFileUrl;
            break;
        }
        if (!string.IsNullOrEmpty(str) && !stringList.Contains(str))
          stringList.Add(str);
      }
    }
    return stringList;
  }

  public static int GetBusIndex(string busName, bool alertMissing)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return -1;
    for (int index = 0; index < DarkTonic.MasterAudio.MasterAudio.GroupBuses.Count; ++index)
    {
      if (DarkTonic.MasterAudio.MasterAudio.GroupBuses[index].busName == busName)
        return index + 2;
    }
    if (alertMissing)
      DarkTonic.MasterAudio.MasterAudio.LogWarning($"Could not find bus '{busName}'.");
    return -1;
  }

  public static GroupBus GetBusByIndex(int busIndex)
  {
    return busIndex < 2 ? (GroupBus) null : DarkTonic.MasterAudio.MasterAudio.GroupBuses[busIndex - 2];
  }

  public static void ChangeBusPitch(string busName, float pitch)
  {
    int busIndex = DarkTonic.MasterAudio.MasterAudio.GetBusIndex(busName, true);
    if (busIndex < 0)
      return;
    Dictionary<string, DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo>.Enumerator enumerator = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.GetEnumerator();
    while (enumerator.MoveNext())
    {
      MasterAudioGroup group = enumerator.Current.Value.Group;
      if (group.busIndex == busIndex)
        DarkTonic.MasterAudio.MasterAudio.ChangeVariationPitch(group.GameObjectName, true, string.Empty, pitch);
    }
  }

  public static void MuteBus(string busName)
  {
    int busIndex = DarkTonic.MasterAudio.MasterAudio.GetBusIndex(busName, true);
    if (busIndex < 0)
      return;
    GroupBus groupBus = DarkTonic.MasterAudio.MasterAudio.GrabBusByName(busName);
    groupBus.isMuted = true;
    if (groupBus.isSoloed)
      DarkTonic.MasterAudio.MasterAudio.UnsoloBus(busName);
    Dictionary<string, DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo>.Enumerator enumerator = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.GetEnumerator();
    while (enumerator.MoveNext())
    {
      MasterAudioGroup group = enumerator.Current.Value.Group;
      if (group.busIndex == busIndex)
        DarkTonic.MasterAudio.MasterAudio.MuteGroup(group.GameObjectName, false);
    }
    if (!Application.isPlaying)
      return;
    DarkTonic.MasterAudio.MasterAudio.SilenceOrUnsilenceGroupsFromSoloChange();
  }

  public static void UnmuteBus(string busName, bool shouldCheckMuteStatus = true)
  {
    int busIndex = DarkTonic.MasterAudio.MasterAudio.GetBusIndex(busName, true);
    if (busIndex < 0)
      return;
    DarkTonic.MasterAudio.MasterAudio.GrabBusByName(busName).isMuted = false;
    Dictionary<string, DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo>.Enumerator enumerator = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.GetEnumerator();
    while (enumerator.MoveNext())
    {
      MasterAudioGroup group = enumerator.Current.Value.Group;
      if (group.busIndex == busIndex)
        DarkTonic.MasterAudio.MasterAudio.UnmuteGroup(group.GameObjectName, false);
    }
    if (!shouldCheckMuteStatus)
      return;
    DarkTonic.MasterAudio.MasterAudio.SilenceOrUnsilenceGroupsFromSoloChange();
  }

  public static void ToggleMuteBus(string busName)
  {
    if (DarkTonic.MasterAudio.MasterAudio.GetBusIndex(busName, true) < 0)
      return;
    if (DarkTonic.MasterAudio.MasterAudio.GrabBusByName(busName).isMuted)
      DarkTonic.MasterAudio.MasterAudio.UnmuteBus(busName);
    else
      DarkTonic.MasterAudio.MasterAudio.MuteBus(busName);
  }

  public static void PauseBus(string busName)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return;
    int busIndex = DarkTonic.MasterAudio.MasterAudio.GetBusIndex(busName, true);
    if (busIndex < 0)
      return;
    Dictionary<string, DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo>.Enumerator enumerator = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.GetEnumerator();
    while (enumerator.MoveNext())
    {
      MasterAudioGroup group = enumerator.Current.Value.Group;
      if (group.busIndex == busIndex)
        DarkTonic.MasterAudio.MasterAudio.PauseSoundGroup(group.GameObjectName);
    }
  }

  public static void SoloBus(string busName)
  {
    int busIndex = DarkTonic.MasterAudio.MasterAudio.GetBusIndex(busName, true);
    if (busIndex < 0)
      return;
    GroupBus groupBus = DarkTonic.MasterAudio.MasterAudio.GrabBusByName(busName);
    groupBus.isSoloed = true;
    if (groupBus.isMuted)
      DarkTonic.MasterAudio.MasterAudio.UnmuteBus(busName);
    Dictionary<string, DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo>.Enumerator enumerator = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.GetEnumerator();
    while (enumerator.MoveNext())
    {
      MasterAudioGroup group = enumerator.Current.Value.Group;
      if (group.busIndex == busIndex)
        DarkTonic.MasterAudio.MasterAudio.SoloGroup(group.GameObjectName, false);
    }
    if (!Application.isPlaying)
      return;
    DarkTonic.MasterAudio.MasterAudio.SilenceOrUnsilenceGroupsFromSoloChange();
  }

  public static void UnsoloBus(string busName, bool shouldCheckMuteStatus = true)
  {
    int busIndex = DarkTonic.MasterAudio.MasterAudio.GetBusIndex(busName, true);
    if (busIndex < 0)
      return;
    DarkTonic.MasterAudio.MasterAudio.GrabBusByName(busName).isSoloed = false;
    Dictionary<string, DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo>.Enumerator enumerator = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.GetEnumerator();
    while (enumerator.MoveNext())
    {
      MasterAudioGroup group = enumerator.Current.Value.Group;
      if (group.busIndex == busIndex)
        DarkTonic.MasterAudio.MasterAudio.UnsoloGroup(group.GameObjectName, false);
    }
    if (!shouldCheckMuteStatus)
      return;
    DarkTonic.MasterAudio.MasterAudio.SilenceOrUnsilenceGroupsFromSoloChange();
  }

  public static void RouteBusToUnityMixerGroup(string busName, AudioMixerGroup mixerGroup)
  {
    if (!Application.isPlaying)
      return;
    int busIndex = DarkTonic.MasterAudio.MasterAudio.GetBusIndex(busName, true);
    if (busIndex < 0)
      return;
    Dictionary<string, DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo>.Enumerator enumerator = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.GetEnumerator();
    while (enumerator.MoveNext())
    {
      MasterAudioGroup group = enumerator.Current.Value.Group;
      if (group.busIndex == busIndex)
        DarkTonic.MasterAudio.MasterAudio.RouteGroupToUnityMixerGroup(group.name, mixerGroup);
    }
  }

  public static void StopOldestSoundOnBus(GroupBus bus)
  {
    int busIndex = DarkTonic.MasterAudio.MasterAudio.GetBusIndex(bus.busName, true);
    if (busIndex < 0)
      return;
    Dictionary<string, DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo>.Enumerator enumerator = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.GetEnumerator();
    SoundGroupVariation soundGroupVariation = (SoundGroupVariation) null;
    float num = -1f;
    while (enumerator.MoveNext())
    {
      DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo audioGroupInfo = enumerator.Current.Value;
      MasterAudioGroup group = audioGroupInfo.Group;
      if (group.busIndex == busIndex && group.ActiveVoices != 0)
      {
        for (int index = 0; index < audioGroupInfo.Sources.Count; ++index)
        {
          SoundGroupVariation variation = audioGroupInfo.Sources[index].Variation;
          if (variation.PlaySoundParm.IsPlaying)
          {
            if (variation.curFadeMode == SoundGroupVariation.FadeMode.FadeOutEarly)
              variation.Stop();
            else if ((UnityEngine.Object) soundGroupVariation == (UnityEngine.Object) null)
            {
              soundGroupVariation = variation;
              num = variation.LastTimePlayed;
            }
            else if ((double) variation.LastTimePlayed < (double) num)
            {
              soundGroupVariation = variation;
              num = variation.LastTimePlayed;
            }
          }
        }
      }
    }
    if (!((UnityEngine.Object) soundGroupVariation != (UnityEngine.Object) null))
      return;
    soundGroupVariation.FadeOutNow(DarkTonic.MasterAudio.MasterAudio.Instance.stopOldestBusFadeTime);
  }

  public static void StopBus(string busName)
  {
    int busIndex = DarkTonic.MasterAudio.MasterAudio.GetBusIndex(busName, true);
    if (busIndex < 0)
      return;
    DarkTonic.MasterAudio.MasterAudio.Instance.VariationsStartedDuringMultiStop.Clear();
    DarkTonic.MasterAudio.MasterAudio.Instance._isStoppingMultiple = true;
    Dictionary<string, DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo>.Enumerator enumerator = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.GetEnumerator();
    while (enumerator.MoveNext())
    {
      MasterAudioGroup group = enumerator.Current.Value.Group;
      if (group.busIndex == busIndex)
        DarkTonic.MasterAudio.MasterAudio.StopAllOfSound(group.GameObjectName);
    }
    DarkTonic.MasterAudio.MasterAudio.Instance._isStoppingMultiple = false;
  }

  public static void UnpauseBus(string busName)
  {
    int busIndex = DarkTonic.MasterAudio.MasterAudio.GetBusIndex(busName, true);
    if (busIndex < 0)
      return;
    Dictionary<string, DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo>.Enumerator enumerator = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.GetEnumerator();
    while (enumerator.MoveNext())
    {
      MasterAudioGroup group = enumerator.Current.Value.Group;
      if (group.busIndex == busIndex)
        DarkTonic.MasterAudio.MasterAudio.UnpauseSoundGroup(group.GameObjectName);
    }
  }

  public static bool CreateBus(string busName, bool errorOnExisting = true, bool isTemporary = false)
  {
    if (DarkTonic.MasterAudio.MasterAudio.GroupBuses.FindAll((Predicate<GroupBus>) (obj => obj.busName == busName)).Count > 0)
    {
      if (errorOnExisting)
        DarkTonic.MasterAudio.MasterAudio.LogError($"You already have a bus named '{busName}'. Not creating a second one.");
      return false;
    }
    GroupBus groupBus = new GroupBus()
    {
      busName = busName,
      isTemporary = isTemporary
    };
    float? busVolume = PersistentAudioSettings.GetBusVolume(busName);
    DarkTonic.MasterAudio.MasterAudio.GroupBuses.Add(groupBus);
    if (busVolume.HasValue)
      DarkTonic.MasterAudio.MasterAudio.SetBusVolumeByName(busName, busVolume.Value);
    return true;
  }

  public static void DeleteBusByName(string busName)
  {
    int busIndex = DarkTonic.MasterAudio.MasterAudio.GetBusIndex(busName, false);
    if (busIndex <= 0)
      return;
    DarkTonic.MasterAudio.MasterAudio.DeleteBusByIndex(busIndex);
  }

  public static void DeleteBusByIndex(int busIndex)
  {
    int index1 = busIndex - 2;
    if (Application.isPlaying)
    {
      GroupBus groupBus = DarkTonic.MasterAudio.MasterAudio.GroupBuses[index1];
      if (groupBus.isSoloed)
        DarkTonic.MasterAudio.MasterAudio.UnsoloBus(groupBus.busName, false);
      else if (groupBus.isMuted)
        DarkTonic.MasterAudio.MasterAudio.UnmuteBus(groupBus.busName, false);
    }
    DarkTonic.MasterAudio.MasterAudio.GroupBuses.RemoveAt(index1);
    Dictionary<string, DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo>.Enumerator enumerator = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.GetEnumerator();
    while (enumerator.MoveNext())
    {
      DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo aGroup = enumerator.Current.Value;
      MasterAudioGroup group = aGroup.Group;
      if (group.busIndex != -1)
      {
        if (group.busIndex == busIndex)
        {
          group.busIndex = -1;
          DarkTonic.MasterAudio.MasterAudio.RouteGroupToUnityMixerGroup(group.name, (AudioMixerGroup) null);
          for (int index2 = 0; index2 < aGroup.Sources.Count; ++index2)
            aGroup.Sources[index2].Variation.SetSpatialBlend();
          DarkTonic.MasterAudio.MasterAudio.RecalculateGroupVolumes(aGroup, (GroupBus) null);
        }
        else if (group.busIndex > busIndex)
          --group.busIndex;
      }
    }
  }

  public static float GetBusVolume(MasterAudioGroup maGroup)
  {
    float busVolume = 1f;
    if (maGroup.busIndex >= 2)
      busVolume = DarkTonic.MasterAudio.MasterAudio.GroupBuses[maGroup.busIndex - 2].volume;
    return busVolume;
  }

  public static void FadeBusToVolume(
    string busName,
    float newVolume,
    float fadeTime,
    System.Action completionCallback = null,
    bool willStopAfterFade = false,
    bool willResetVolumeAfterFade = false)
  {
    if ((double) newVolume < 0.0 || (double) newVolume > 1.0)
      Debug.LogError((object) $"Illegal volume passed to FadeBusToVolume: '{newVolume.ToString()}'. Legal volumes are between 0 and 1");
    else if ((double) fadeTime <= 0.10000000149011612)
    {
      DarkTonic.MasterAudio.MasterAudio.SetBusVolumeByName(busName, newVolume);
      if (completionCallback != null)
        completionCallback();
      if (!willStopAfterFade)
        return;
      DarkTonic.MasterAudio.MasterAudio.StopBus(busName);
    }
    else
    {
      GroupBus groupBus = DarkTonic.MasterAudio.MasterAudio.GrabBusByName(busName);
      if (groupBus == null)
      {
        Debug.Log((object) $"Could not find bus '{busName}' to fade it.");
      }
      else
      {
        BusFadeInfo busFadeInfo1 = DarkTonic.MasterAudio.MasterAudio.Instance.BusFades.Find((Predicate<BusFadeInfo>) (obj => obj.NameOfBus == busName));
        if (busFadeInfo1 != null)
          busFadeInfo1.IsActive = false;
        BusFadeInfo busFadeInfo2 = new BusFadeInfo()
        {
          NameOfBus = busName,
          ActingBus = groupBus,
          StartVolume = groupBus.volume,
          TargetVolume = newVolume,
          StartTime = AudioUtil.Time,
          CompletionTime = AudioUtil.Time + fadeTime,
          WillStopGroupAfterFade = willStopAfterFade,
          WillResetVolumeAfterFade = willResetVolumeAfterFade
        };
        if (completionCallback != null)
          busFadeInfo2.completionAction = completionCallback;
        DarkTonic.MasterAudio.MasterAudio.Instance.BusFades.Add(busFadeInfo2);
      }
    }
  }

  public static void GlideBusByPitch(
    string busName,
    float pitchAddition,
    float glideTime,
    System.Action completionCallback = null)
  {
    if ((double) pitchAddition < -3.0 || (double) pitchAddition > 3.0)
      Debug.LogError((object) $"Illegal pitch passed to GlideBusByPitch: '{pitchAddition.ToString()}'. Legal pitches are between -3 and 3");
    else if ((double) pitchAddition == 0.0)
    {
      if (completionCallback == null)
        return;
      completionCallback();
    }
    else
    {
      int busIndex = DarkTonic.MasterAudio.MasterAudio.GetBusIndex(busName, true);
      if (busIndex < 0)
        return;
      Dictionary<string, DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo>.Enumerator enumerator = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.GetEnumerator();
      if ((double) glideTime <= 0.10000000149011612)
      {
        while (enumerator.MoveNext())
        {
          DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo audioGroupInfo = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[enumerator.Current.Value.Group.name];
          if (audioGroupInfo.Group.busIndex == busIndex)
          {
            for (int index = 0; index < audioGroupInfo.Sources.Count; ++index)
            {
              SoundGroupVariation variation = audioGroupInfo.Sources[index].Variation;
              if (variation.IsPlaying)
              {
                if (variation.curPitchMode == SoundGroupVariation.PitchMode.Gliding)
                  variation.VariationUpdater.StopPitchGliding();
                variation.GlideByPitch(pitchAddition, 0.0f);
              }
            }
          }
        }
        if (completionCallback == null)
          return;
        completionCallback();
      }
      else
      {
        BusPitchGlideInfo busPitchGlideInfo1 = DarkTonic.MasterAudio.MasterAudio.Instance.BusPitchGlides.Find((Predicate<BusPitchGlideInfo>) (obj => obj.NameOfBus == busName));
        if (busPitchGlideInfo1 != null)
        {
          busPitchGlideInfo1.IsActive = false;
          if (busPitchGlideInfo1.completionAction != null)
          {
            busPitchGlideInfo1.completionAction();
            busPitchGlideInfo1.completionAction = (System.Action) null;
          }
        }
        List<SoundGroupVariation> soundGroupVariationList = new List<SoundGroupVariation>(16 /*0x10*/);
        while (enumerator.MoveNext())
        {
          DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo audioGroupInfo = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[enumerator.Current.Value.Group.name];
          if (audioGroupInfo.Group.busIndex == busIndex)
          {
            for (int index = 0; index < audioGroupInfo.Sources.Count; ++index)
            {
              SoundGroupVariation variation = audioGroupInfo.Sources[index].Variation;
              if (variation.IsPlaying)
              {
                if (variation.curPitchMode == SoundGroupVariation.PitchMode.Gliding)
                  variation.VariationUpdater.StopPitchGliding();
                variation.GlideByPitch(pitchAddition, glideTime);
                soundGroupVariationList.Add(variation);
              }
            }
          }
        }
        if (soundGroupVariationList.Count == 0)
        {
          if (completionCallback == null)
            return;
          completionCallback();
        }
        else
        {
          BusPitchGlideInfo busPitchGlideInfo2 = new BusPitchGlideInfo()
          {
            NameOfBus = busName,
            CompletionTime = AudioUtil.Time + glideTime,
            GlidingVariations = soundGroupVariationList
          };
          if (completionCallback != null)
            busPitchGlideInfo2.completionAction = completionCallback;
          DarkTonic.MasterAudio.MasterAudio.Instance.BusPitchGlides.Add(busPitchGlideInfo2);
        }
      }
    }
  }

  public static void SetBusVolumeByName(string busName, float newVolume)
  {
    GroupBus bus = DarkTonic.MasterAudio.MasterAudio.GrabBusByName(busName);
    if (bus == null)
      Debug.LogError((object) $"bus '{busName}' not found!");
    else
      DarkTonic.MasterAudio.MasterAudio.SetBusVolume(bus, newVolume);
  }

  public static void RecalculateGroupVolumes(DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo aGroup, GroupBus bus)
  {
    GroupBus busByIndex = DarkTonic.MasterAudio.MasterAudio.GetBusByIndex(aGroup.Group.busIndex);
    int num1 = busByIndex == null || bus == null ? 0 : (busByIndex.busName == bus.busName ? 1 : 0);
    float num2 = 1f;
    if (num1 != 0)
      num2 = bus.volume;
    else if (busByIndex != null)
      num2 = busByIndex.volume;
    for (int index = 0; index < aGroup.Sources.Count; ++index)
    {
      DarkTonic.MasterAudio.MasterAudio.AudioInfo source1 = aGroup.Sources[index];
      AudioSource source2 = source1.Source;
      if (source1.Variation.IsPlaying)
      {
        float num3 = aGroup.Group.groupMasterVolume * num2 * DarkTonic.MasterAudio.MasterAudio.Instance._masterAudioVolume;
        float num4 = source1.OriginalVolume * source1.LastPercentageVolume * num3 + source1.LastRandomVolume;
        source2.volume = num4;
        source2.GetComponent<SoundGroupVariation>().SetGroupVolume = num3;
      }
    }
  }

  public static void SetBusVolume(GroupBus bus, float newVolume)
  {
    if (bus != null)
      bus.volume = newVolume;
    foreach (string key in DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.Keys)
      DarkTonic.MasterAudio.MasterAudio.RecalculateGroupVolumes(DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[key], bus);
  }

  public static GroupBus GrabBusByName(string busName)
  {
    for (int index = 0; index < DarkTonic.MasterAudio.MasterAudio.GroupBuses.Count; ++index)
    {
      GroupBus groupBus = DarkTonic.MasterAudio.MasterAudio.GroupBuses[index];
      if (groupBus.busName == busName)
        return groupBus;
    }
    return (GroupBus) null;
  }

  public static void PauseBusOfTransform(Transform sourceTrans, string busName)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return;
    int busIndex = DarkTonic.MasterAudio.MasterAudio.GetBusIndex(busName, true);
    if (busIndex < 0)
      return;
    Dictionary<string, DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo>.Enumerator enumerator = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.GetEnumerator();
    while (enumerator.MoveNext())
    {
      MasterAudioGroup group = enumerator.Current.Value.Group;
      if (group.busIndex == busIndex)
        DarkTonic.MasterAudio.MasterAudio.PauseSoundGroupOfTransform(sourceTrans, group.GameObjectName);
    }
  }

  public static void UnpauseBusOfTransform(Transform sourceTrans, string busName)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return;
    int busIndex = DarkTonic.MasterAudio.MasterAudio.GetBusIndex(busName, true);
    if (busIndex < 0)
      return;
    Dictionary<string, DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo>.Enumerator enumerator = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.GetEnumerator();
    while (enumerator.MoveNext())
    {
      MasterAudioGroup group = enumerator.Current.Value.Group;
      if (group.busIndex == busIndex)
        DarkTonic.MasterAudio.MasterAudio.UnpauseSoundGroupOfTransform(sourceTrans, group.GameObjectName);
    }
  }

  public static void StopBusOfTransform(Transform sourceTrans, string busName)
  {
    if (!DarkTonic.MasterAudio.MasterAudio.SceneHasMasterAudio)
      return;
    int busIndex = DarkTonic.MasterAudio.MasterAudio.GetBusIndex(busName, true);
    if (busIndex < 0)
      return;
    Dictionary<string, DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo>.Enumerator enumerator = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.GetEnumerator();
    DarkTonic.MasterAudio.MasterAudio.Instance.VariationsStartedDuringMultiStop.Clear();
    DarkTonic.MasterAudio.MasterAudio.Instance._isStoppingMultiple = true;
    while (enumerator.MoveNext())
    {
      MasterAudioGroup group = enumerator.Current.Value.Group;
      if (group.busIndex == busIndex)
        DarkTonic.MasterAudio.MasterAudio.StopSoundGroupOfTransform(sourceTrans, group.GameObjectName);
    }
    DarkTonic.MasterAudio.MasterAudio.Instance._isStoppingMultiple = false;
  }

  public static void AddSoundGroupToDuckList(
    string sType,
    float riseVolumeStart,
    float duckedVolCut,
    float unduckTime,
    bool isTemporary = false)
  {
    DarkTonic.MasterAudio.MasterAudio instance = DarkTonic.MasterAudio.MasterAudio.Instance;
    if (instance.duckingBySoundType.ContainsKey(sType))
      return;
    DuckGroupInfo duckGroupInfo = new DuckGroupInfo()
    {
      soundType = sType,
      riseVolStart = riseVolumeStart,
      duckedVolumeCut = duckedVolCut,
      unduckTime = unduckTime,
      isTemporary = isTemporary
    };
    instance.duckingBySoundType.Add(sType, duckGroupInfo);
    instance.musicDuckingSounds.Add(duckGroupInfo);
  }

  public static void RemoveSoundGroupFromDuckList(string sType)
  {
    DarkTonic.MasterAudio.MasterAudio instance = DarkTonic.MasterAudio.MasterAudio.Instance;
    if (!instance.duckingBySoundType.ContainsKey(sType))
      return;
    DuckGroupInfo duckGroupInfo = instance.duckingBySoundType[sType];
    instance.musicDuckingSounds.Remove(duckGroupInfo);
    instance.duckingBySoundType.Remove(sType);
  }

  public static DarkTonic.MasterAudio.MasterAudio.Playlist GrabPlaylist(
    string playlistName,
    bool logErrorIfNotFound = true)
  {
    if (playlistName == "[None]")
      return (DarkTonic.MasterAudio.MasterAudio.Playlist) null;
    for (int index = 0; index < DarkTonic.MasterAudio.MasterAudio.MusicPlaylists.Count; ++index)
    {
      DarkTonic.MasterAudio.MasterAudio.Playlist musicPlaylist = DarkTonic.MasterAudio.MasterAudio.MusicPlaylists[index];
      if (musicPlaylist.playlistName == playlistName)
        return musicPlaylist;
    }
    if (logErrorIfNotFound)
      Debug.LogError((object) $"Could not find Playlist '{playlistName}'.");
    return (DarkTonic.MasterAudio.MasterAudio.Playlist) null;
  }

  public static void ChangePlaylistPitch(string playlistName, float pitch, string songName = null)
  {
    DarkTonic.MasterAudio.MasterAudio.Playlist playlist = DarkTonic.MasterAudio.MasterAudio.GrabPlaylist(playlistName);
    if (playlist == null)
      return;
    for (int index = 0; index < playlist.MusicSettings.Count; ++index)
    {
      MusicSetting musicSetting = playlist.MusicSettings[index];
      if (string.IsNullOrEmpty(songName) || !(musicSetting.alias != songName) || !(musicSetting.songName != songName))
        musicSetting.pitch = pitch;
    }
  }

  public static void MutePlaylist() => DarkTonic.MasterAudio.MasterAudio.MutePlaylist("~only~");

  public static void MutePlaylist(string playlistControllerName)
  {
    List<PlaylistController> instances = PlaylistController.Instances;
    List<PlaylistController> playlists = new List<PlaylistController>();
    if (playlistControllerName == "~only~")
    {
      if (!DarkTonic.MasterAudio.MasterAudio.IsOkToCallOnlyPlaylistMethod(instances, "PausePlaylist"))
        return;
      playlists.Add(instances[0]);
    }
    else
    {
      PlaylistController playlistController = PlaylistController.InstanceByName(playlistControllerName);
      if ((UnityEngine.Object) playlistController != (UnityEngine.Object) null)
        playlists.Add(playlistController);
    }
    DarkTonic.MasterAudio.MasterAudio.MutePlaylists(playlists);
  }

  public static void MuteAllPlaylists() => DarkTonic.MasterAudio.MasterAudio.MutePlaylists(PlaylistController.Instances);

  public static void MutePlaylists(List<PlaylistController> playlists)
  {
    if (playlists.Count == PlaylistController.Instances.Count)
      DarkTonic.MasterAudio.MasterAudio.PlaylistsMuted = true;
    for (int index = 0; index < playlists.Count; ++index)
      playlists[index].MutePlaylist();
  }

  public static void UnmutePlaylist() => DarkTonic.MasterAudio.MasterAudio.UnmutePlaylist("~only~");

  public static void UnmutePlaylist(string playlistControllerName)
  {
    List<PlaylistController> instances = PlaylistController.Instances;
    List<PlaylistController> playlists = new List<PlaylistController>();
    if (playlistControllerName == "~only~")
    {
      if (!DarkTonic.MasterAudio.MasterAudio.IsOkToCallOnlyPlaylistMethod(instances, "PausePlaylist"))
        return;
      playlists.Add(instances[0]);
    }
    else
    {
      PlaylistController playlistController = PlaylistController.InstanceByName(playlistControllerName);
      if ((UnityEngine.Object) playlistController != (UnityEngine.Object) null)
        playlists.Add(playlistController);
    }
    DarkTonic.MasterAudio.MasterAudio.UnmutePlaylists(playlists);
  }

  public static void UnmuteAllPlaylists()
  {
    DarkTonic.MasterAudio.MasterAudio.UnmutePlaylists(PlaylistController.Instances);
  }

  public static void UnmutePlaylists(List<PlaylistController> playlists)
  {
    if (playlists.Count == PlaylistController.Instances.Count)
      DarkTonic.MasterAudio.MasterAudio.PlaylistsMuted = false;
    for (int index = 0; index < playlists.Count; ++index)
      playlists[index].UnmutePlaylist();
  }

  public static void ToggleMutePlaylist() => DarkTonic.MasterAudio.MasterAudio.ToggleMutePlaylist("~only~");

  public static void ToggleMutePlaylist(string playlistControllerName)
  {
    List<PlaylistController> instances = PlaylistController.Instances;
    List<PlaylistController> playlists = new List<PlaylistController>();
    if (playlistControllerName == "~only~")
    {
      if (!DarkTonic.MasterAudio.MasterAudio.IsOkToCallOnlyPlaylistMethod(instances, "PausePlaylist"))
        return;
      playlists.Add(instances[0]);
    }
    else
    {
      PlaylistController playlistController = PlaylistController.InstanceByName(playlistControllerName);
      if ((UnityEngine.Object) playlistController != (UnityEngine.Object) null)
        playlists.Add(playlistController);
    }
    DarkTonic.MasterAudio.MasterAudio.ToggleMutePlaylists(playlists);
  }

  public static void ToggleMuteAllPlaylists()
  {
    DarkTonic.MasterAudio.MasterAudio.ToggleMutePlaylists(PlaylistController.Instances);
  }

  public static void ToggleMutePlaylists(List<PlaylistController> playlists)
  {
    for (int index = 0; index < playlists.Count; ++index)
      playlists[index].ToggleMutePlaylist();
  }

  public static void PausePlaylist() => DarkTonic.MasterAudio.MasterAudio.PausePlaylist("~only~");

  public static void PausePlaylist(string playlistControllerName)
  {
    List<PlaylistController> instances = PlaylistController.Instances;
    List<PlaylistController> playlists = new List<PlaylistController>();
    if (playlistControllerName == "~only~")
    {
      if (!DarkTonic.MasterAudio.MasterAudio.IsOkToCallOnlyPlaylistMethod(instances, nameof (PausePlaylist)))
        return;
      playlists.Add(instances[0]);
    }
    else
    {
      PlaylistController playlistController = PlaylistController.InstanceByName(playlistControllerName);
      if ((UnityEngine.Object) playlistController != (UnityEngine.Object) null)
        playlists.Add(playlistController);
    }
    DarkTonic.MasterAudio.MasterAudio.PausePlaylists(playlists);
  }

  public static void PauseAllPlaylists()
  {
    DarkTonic.MasterAudio.MasterAudio.PausePlaylists(PlaylistController.Instances);
  }

  public static void PausePlaylists(List<PlaylistController> playlists)
  {
    for (int index = 0; index < playlists.Count; ++index)
      playlists[index].PausePlaylist();
  }

  public static void UnpausePlaylist() => DarkTonic.MasterAudio.MasterAudio.UnpausePlaylist("~only~");

  public static void UnpausePlaylist(string playlistControllerName)
  {
    List<PlaylistController> instances = PlaylistController.Instances;
    List<PlaylistController> controllers = new List<PlaylistController>();
    if (playlistControllerName == "~only~")
    {
      if (!DarkTonic.MasterAudio.MasterAudio.IsOkToCallOnlyPlaylistMethod(instances, nameof (UnpausePlaylist)))
        return;
      controllers.Add(instances[0]);
    }
    else
    {
      PlaylistController playlistController = PlaylistController.InstanceByName(playlistControllerName);
      if ((UnityEngine.Object) playlistController != (UnityEngine.Object) null)
        controllers.Add(playlistController);
    }
    DarkTonic.MasterAudio.MasterAudio.UnpausePlaylists(controllers);
  }

  public static void UnpauseAllPlaylists()
  {
    DarkTonic.MasterAudio.MasterAudio.UnpausePlaylists(PlaylistController.Instances);
  }

  public static void UnpausePlaylists(List<PlaylistController> controllers)
  {
    for (int index = 0; index < controllers.Count; ++index)
      controllers[index].UnpausePlaylist();
  }

  public static void StopPlaylist() => DarkTonic.MasterAudio.MasterAudio.StopPlaylist("~only~");

  public static void StopPlaylist(string playlistControllerName)
  {
    List<PlaylistController> instances = PlaylistController.Instances;
    List<PlaylistController> playlists = new List<PlaylistController>();
    if (playlistControllerName == "~only~")
    {
      if (!DarkTonic.MasterAudio.MasterAudio.IsOkToCallOnlyPlaylistMethod(instances, nameof (StopPlaylist)))
        return;
      playlists.Add(instances[0]);
    }
    else
    {
      PlaylistController playlistController = PlaylistController.InstanceByName(playlistControllerName);
      if ((UnityEngine.Object) playlistController != (UnityEngine.Object) null)
        playlists.Add(playlistController);
    }
    DarkTonic.MasterAudio.MasterAudio.StopPlaylists(playlists);
  }

  public static void StopAllPlaylists() => DarkTonic.MasterAudio.MasterAudio.StopPlaylists(PlaylistController.Instances);

  public static void StopPlaylists(List<PlaylistController> playlists)
  {
    for (int index = 0; index < playlists.Count; ++index)
      playlists[index].StopPlaylist();
  }

  public static void TriggerNextPlaylistClip() => DarkTonic.MasterAudio.MasterAudio.TriggerNextPlaylistClip("~only~");

  public static void TriggerNextPlaylistClip(string playlistControllerName)
  {
    List<PlaylistController> instances = PlaylistController.Instances;
    List<PlaylistController> playlists = new List<PlaylistController>();
    if (playlistControllerName == "~only~")
    {
      if (!DarkTonic.MasterAudio.MasterAudio.IsOkToCallOnlyPlaylistMethod(instances, nameof (TriggerNextPlaylistClip)))
        return;
      playlists.Add(instances[0]);
    }
    else
    {
      PlaylistController playlistController = PlaylistController.InstanceByName(playlistControllerName);
      if ((UnityEngine.Object) playlistController != (UnityEngine.Object) null)
        playlists.Add(playlistController);
    }
    DarkTonic.MasterAudio.MasterAudio.NextPlaylistClips(playlists);
  }

  public static void TriggerNextClipAllPlaylists()
  {
    DarkTonic.MasterAudio.MasterAudio.NextPlaylistClips(PlaylistController.Instances);
  }

  public static void NextPlaylistClips(List<PlaylistController> playlists)
  {
    for (int index = 0; index < playlists.Count; ++index)
      playlists[index].PlayNextSong();
  }

  public static void TriggerRandomPlaylistClip() => DarkTonic.MasterAudio.MasterAudio.TriggerRandomPlaylistClip("~only~");

  public static void TriggerRandomPlaylistClip(string playlistControllerName)
  {
    List<PlaylistController> instances = PlaylistController.Instances;
    List<PlaylistController> playlists = new List<PlaylistController>();
    if (playlistControllerName == "~only~")
    {
      if (!DarkTonic.MasterAudio.MasterAudio.IsOkToCallOnlyPlaylistMethod(instances, nameof (TriggerRandomPlaylistClip)))
        return;
      playlists.Add(instances[0]);
    }
    else
    {
      PlaylistController playlistController = PlaylistController.InstanceByName(playlistControllerName);
      if ((UnityEngine.Object) playlistController != (UnityEngine.Object) null)
        playlists.Add(playlistController);
    }
    DarkTonic.MasterAudio.MasterAudio.RandomPlaylistClips(playlists);
  }

  public static void TriggerRandomClipAllPlaylists()
  {
    DarkTonic.MasterAudio.MasterAudio.RandomPlaylistClips(PlaylistController.Instances);
  }

  public static void RandomPlaylistClips(List<PlaylistController> playlists)
  {
    for (int index = 0; index < playlists.Count; ++index)
      playlists[index].PlayRandomSong();
  }

  public static void RestartPlaylist() => DarkTonic.MasterAudio.MasterAudio.RestartPlaylist("~only~");

  public static void RestartPlaylist(string playlistControllerName)
  {
    List<PlaylistController> instances = PlaylistController.Instances;
    PlaylistController playlistController1;
    if (playlistControllerName == "~only~")
    {
      if (!DarkTonic.MasterAudio.MasterAudio.IsOkToCallOnlyPlaylistMethod(instances, nameof (RestartPlaylist)))
        return;
      playlistController1 = instances[0];
    }
    else
    {
      PlaylistController playlistController2 = PlaylistController.InstanceByName(playlistControllerName);
      if ((UnityEngine.Object) playlistController2 == (UnityEngine.Object) null)
        return;
      playlistController1 = playlistController2;
    }
    if (!((UnityEngine.Object) playlistController1 != (UnityEngine.Object) null))
      return;
    DarkTonic.MasterAudio.MasterAudio.RestartPlaylists(new List<PlaylistController>()
    {
      playlistController1
    });
  }

  public static void RestartAllPlaylists()
  {
    DarkTonic.MasterAudio.MasterAudio.RestartPlaylists(PlaylistController.Instances);
  }

  public static void RestartPlaylists(List<PlaylistController> playlists)
  {
    for (int index = 0; index < playlists.Count; ++index)
      playlists[index].RestartPlaylist();
  }

  public static void StartPlaylist(string playlistName)
  {
    DarkTonic.MasterAudio.MasterAudio.StartPlaylist("~only~", playlistName);
  }

  public static void StartPlaylist(string playlistControllerName, string playlistName)
  {
    List<PlaylistController> instances = PlaylistController.Instances;
    List<PlaylistController> playlistControllerList = new List<PlaylistController>();
    if (playlistControllerName == "~only~")
    {
      if (!DarkTonic.MasterAudio.MasterAudio.IsOkToCallOnlyPlaylistMethod(instances, "PausePlaylist"))
        return;
      playlistControllerList.Add(instances[0]);
    }
    else
    {
      PlaylistController playlistController = PlaylistController.InstanceByName(playlistControllerName);
      if ((UnityEngine.Object) playlistController != (UnityEngine.Object) null)
        playlistControllerList.Add(playlistController);
    }
    for (int index = 0; index < playlistControllerList.Count; ++index)
      playlistControllerList[index].StartPlaylist(playlistName);
  }

  public static void StopLoopingAllCurrentSongs()
  {
    DarkTonic.MasterAudio.MasterAudio.StopLoopingCurrentSongs(PlaylistController.Instances);
  }

  public static void StopLoopingCurrentSong() => DarkTonic.MasterAudio.MasterAudio.StopLoopingCurrentSong("~only~");

  public static void StopLoopingCurrentSong(string playlistControllerName)
  {
    List<PlaylistController> instances = PlaylistController.Instances;
    PlaylistController playlistController1;
    if (playlistControllerName == "~only~")
    {
      if (!DarkTonic.MasterAudio.MasterAudio.IsOkToCallOnlyPlaylistMethod(instances, nameof (StopLoopingCurrentSong)))
        return;
      playlistController1 = instances[0];
    }
    else
    {
      PlaylistController playlistController2 = PlaylistController.InstanceByName(playlistControllerName);
      if ((UnityEngine.Object) playlistController2 == (UnityEngine.Object) null)
        return;
      playlistController1 = playlistController2;
    }
    if (!((UnityEngine.Object) playlistController1 != (UnityEngine.Object) null))
      return;
    DarkTonic.MasterAudio.MasterAudio.StopLoopingCurrentSongs(new List<PlaylistController>()
    {
      playlistController1
    });
  }

  public static void StopLoopingCurrentSongs(List<PlaylistController> playlistControllers)
  {
    for (int index = 0; index < playlistControllers.Count; ++index)
      playlistControllers[index].StopLoopingCurrentSong();
  }

  public static void StopAllPlaylistsAfterCurrentSongs()
  {
    DarkTonic.MasterAudio.MasterAudio.StopPlaylistAfterCurrentSongs(PlaylistController.Instances);
  }

  public static void StopPlaylistAfterCurrentSong()
  {
    DarkTonic.MasterAudio.MasterAudio.StopPlaylistAfterCurrentSong("~only~");
  }

  public static void StopPlaylistAfterCurrentSong(string playlistControllerName)
  {
    List<PlaylistController> instances = PlaylistController.Instances;
    PlaylistController playlistController1;
    if (playlistControllerName == "~only~")
    {
      if (!DarkTonic.MasterAudio.MasterAudio.IsOkToCallOnlyPlaylistMethod(instances, nameof (StopPlaylistAfterCurrentSong)))
        return;
      playlistController1 = instances[0];
    }
    else
    {
      PlaylistController playlistController2 = PlaylistController.InstanceByName(playlistControllerName);
      if ((UnityEngine.Object) playlistController2 == (UnityEngine.Object) null)
        return;
      playlistController1 = playlistController2;
    }
    if (!((UnityEngine.Object) playlistController1 != (UnityEngine.Object) null))
      return;
    DarkTonic.MasterAudio.MasterAudio.StopPlaylistAfterCurrentSongs(new List<PlaylistController>()
    {
      playlistController1
    });
  }

  public static void StopPlaylistAfterCurrentSongs(List<PlaylistController> playlistControllers)
  {
    for (int index = 0; index < playlistControllers.Count; ++index)
      playlistControllers[index].StopPlaylistAfterCurrentSong();
  }

  public static void QueuePlaylistClip(string clipName)
  {
    DarkTonic.MasterAudio.MasterAudio.QueuePlaylistClip("~only~", clipName);
  }

  public static void QueuePlaylistClip(string playlistControllerName, string clipName)
  {
    List<PlaylistController> instances = PlaylistController.Instances;
    PlaylistController playlistController1;
    if (playlistControllerName == "~only~")
    {
      if (!DarkTonic.MasterAudio.MasterAudio.IsOkToCallOnlyPlaylistMethod(instances, nameof (QueuePlaylistClip)))
        return;
      playlistController1 = instances[0];
    }
    else
    {
      PlaylistController playlistController2 = PlaylistController.InstanceByName(playlistControllerName);
      if ((UnityEngine.Object) playlistController2 == (UnityEngine.Object) null)
        return;
      playlistController1 = playlistController2;
    }
    if (!((UnityEngine.Object) playlistController1 != (UnityEngine.Object) null))
      return;
    playlistController1.QueuePlaylistClip(clipName);
  }

  public static bool TriggerPlaylistClip(string clipName)
  {
    return DarkTonic.MasterAudio.MasterAudio.TriggerPlaylistClip("~only~", clipName);
  }

  public static bool TriggerPlaylistClip(string playlistControllerName, string clipName)
  {
    List<PlaylistController> instances = PlaylistController.Instances;
    PlaylistController playlistController1;
    if (playlistControllerName == "~only~")
    {
      if (!DarkTonic.MasterAudio.MasterAudio.IsOkToCallOnlyPlaylistMethod(instances, nameof (TriggerPlaylistClip)))
        return false;
      playlistController1 = instances[0];
    }
    else
    {
      PlaylistController playlistController2 = PlaylistController.InstanceByName(playlistControllerName);
      if ((UnityEngine.Object) playlistController2 == (UnityEngine.Object) null)
        return false;
      playlistController1 = playlistController2;
    }
    return !((UnityEngine.Object) playlistController1 == (UnityEngine.Object) null) && playlistController1.TriggerPlaylistClip(clipName);
  }

  public static void ChangePlaylistByName(string playlistName, bool playFirstClip = true)
  {
    DarkTonic.MasterAudio.MasterAudio.ChangePlaylistByName("~only~", playlistName, playFirstClip);
  }

  public static void ChangePlaylistByName(
    string playlistControllerName,
    string playlistName,
    bool playFirstClip = true)
  {
    List<PlaylistController> instances = PlaylistController.Instances;
    PlaylistController playlistController1;
    if (playlistControllerName == "~only~")
    {
      if (!DarkTonic.MasterAudio.MasterAudio.IsOkToCallOnlyPlaylistMethod(instances, nameof (ChangePlaylistByName)))
        return;
      playlistController1 = instances[0];
    }
    else
    {
      PlaylistController playlistController2 = PlaylistController.InstanceByName(playlistControllerName);
      if ((UnityEngine.Object) playlistController2 == (UnityEngine.Object) null)
        return;
      playlistController1 = playlistController2;
    }
    if (!((UnityEngine.Object) playlistController1 != (UnityEngine.Object) null))
      return;
    playlistController1.ChangePlaylist(playlistName, playFirstClip);
  }

  public static void FadePlaylistToVolume(float targetVolume, float fadeTime)
  {
    DarkTonic.MasterAudio.MasterAudio.FadePlaylistToVolume("~only~", targetVolume, fadeTime);
  }

  public static void FadePlaylistToVolume(
    string playlistControllerName,
    float targetVolume,
    float fadeTime)
  {
    List<PlaylistController> instances = PlaylistController.Instances;
    List<PlaylistController> playlists = new List<PlaylistController>();
    if (playlistControllerName == "~only~")
    {
      if (!DarkTonic.MasterAudio.MasterAudio.IsOkToCallOnlyPlaylistMethod(instances, nameof (FadePlaylistToVolume)))
        return;
      playlists.Add(instances[0]);
    }
    else
    {
      PlaylistController playlistController = PlaylistController.InstanceByName(playlistControllerName);
      if ((UnityEngine.Object) playlistController != (UnityEngine.Object) null)
        playlists.Add(playlistController);
    }
    DarkTonic.MasterAudio.MasterAudio.FadePlaylists(playlists, targetVolume, fadeTime);
  }

  public static void FadeAllPlaylistsToVolume(float targetVolume, float fadeTime)
  {
    DarkTonic.MasterAudio.MasterAudio.FadePlaylists(PlaylistController.Instances, targetVolume, fadeTime);
  }

  public static void FadePlaylists(
    List<PlaylistController> playlists,
    float targetVolume,
    float fadeTime)
  {
    if ((double) targetVolume < 0.0 || (double) targetVolume > 1.0)
    {
      Debug.LogError((object) $"Illegal volume passed to FadePlaylistToVolume: '{targetVolume.ToString()}'. Legal volumes are between 0 and 1");
    }
    else
    {
      for (int index = 0; index < playlists.Count; ++index)
        playlists[index].FadeToVolume(targetVolume, fadeTime);
    }
  }

  public static void CreatePlaylist(DarkTonic.MasterAudio.MasterAudio.Playlist playlist, bool errorOnDuplicate)
  {
    DarkTonic.MasterAudio.MasterAudio.Playlist playlist1 = DarkTonic.MasterAudio.MasterAudio.GrabPlaylist(playlist.playlistName, false);
    if (playlist1 != null)
    {
      if (!errorOnDuplicate)
        return;
      Debug.LogError((object) $"You already have a Playlist Controller with the name '{playlist1.playlistName}'. You must name them all uniquely. Not adding duplicate named Playlist.");
    }
    else
      DarkTonic.MasterAudio.MasterAudio.MusicPlaylists.Add(playlist);
  }

  public static void DeletePlaylist(string playlistName)
  {
    if ((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (UnityEngine.Object) null)
      return;
    DarkTonic.MasterAudio.MasterAudio.Playlist playlist = DarkTonic.MasterAudio.MasterAudio.GrabPlaylist(playlistName);
    if (playlist == null)
      return;
    for (int index = 0; index < PlaylistController.Instances.Count; ++index)
    {
      PlaylistController instance = PlaylistController.Instances[index];
      if (!(instance.PlaylistName != playlistName))
      {
        instance.StopPlaylist();
        break;
      }
    }
    DarkTonic.MasterAudio.MasterAudio.MusicPlaylists.Remove(playlist);
  }

  public static void AddSongToPlaylist(
    string playlistName,
    AudioClip song,
    bool loopSong = false,
    float songPitch = 1f,
    float songVolume = 1f)
  {
    DarkTonic.MasterAudio.MasterAudio.Playlist playlist = DarkTonic.MasterAudio.MasterAudio.GrabPlaylist(playlistName);
    if (playlist == null)
      return;
    MusicSetting musicSetting = new MusicSetting()
    {
      clip = song,
      isExpanded = true,
      isLoop = loopSong,
      pitch = songPitch,
      volume = songVolume
    };
    playlist.MusicSettings.Add(musicSetting);
  }

  public static float PlaylistMasterVolume
  {
    get => DarkTonic.MasterAudio.MasterAudio.Instance._masterPlaylistVolume;
    set
    {
      DarkTonic.MasterAudio.MasterAudio.Instance._masterPlaylistVolume = value;
      List<PlaylistController> instances = PlaylistController.Instances;
      for (int index = 0; index < instances.Count; ++index)
        instances[index].UpdateMasterVolume();
    }
  }

  public static void ReDownloadAllInternetFiles()
  {
    List<SoundGroupVariation> soundGroupVariationList = new List<SoundGroupVariation>();
    foreach (string key in DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.Keys)
    {
      for (int index = 0; index < DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[key].Sources.Count; ++index)
      {
        SoundGroupVariation component = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[key].Sources[index].Source.GetComponent<SoundGroupVariation>();
        if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && component.audLocation == DarkTonic.MasterAudio.MasterAudio.AudioLocation.FileOnInternet)
        {
          AudioResourceOptimizer.RemoveLoadedInternetClip(component.internetFileUrl);
          component.internetFileLoadStatus = DarkTonic.MasterAudio.MasterAudio.InternetFileLoadStatus.Loading;
          soundGroupVariationList.Add(component);
        }
      }
    }
    for (int index = 0; index < soundGroupVariationList.Count; ++index)
    {
      SoundGroupVariation soundGroupVariation = soundGroupVariationList[index];
      soundGroupVariation.Stop();
      AudioResourceOptimizer.AddTargetForClip(soundGroupVariation.internetFileUrl, soundGroupVariation.VarAudio);
      soundGroupVariation.LoadInternetFile();
    }
  }

  public static void FireCustomEventNextFrame(string customEventName, Transform eventOrigin)
  {
    if (DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown || "[None]" == customEventName || string.IsNullOrEmpty(customEventName))
      return;
    if (!DarkTonic.MasterAudio.MasterAudio.CustomEventExists(customEventName) && !DarkTonic.MasterAudio.MasterAudio.IsWarming)
      Debug.LogError((object) $"Custom Event '{customEventName}' was not found in Master Audio.");
    else
      DarkTonic.MasterAudio.MasterAudio.Instance.CustomEventsToFire.Enqueue(new CustomEventToFireInfo()
      {
        eventName = customEventName,
        eventOrigin = eventOrigin
      });
  }

  public static void AddCustomEventReceiver(ICustomEventReceiver receiver, Transform receiverTrans)
  {
    if (DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown)
      return;
    IList<AudioEventGroup> allEvents = receiver.GetAllEvents();
    for (int index = 0; index < allEvents.Count; ++index)
    {
      AudioEventGroup audioEventGroup = allEvents[index];
      if (receiver.SubscribesToEvent(audioEventGroup.customEventName))
      {
        if (!DarkTonic.MasterAudio.MasterAudio.Instance.ReceiversByEventName.ContainsKey(audioEventGroup.customEventName))
        {
          DarkTonic.MasterAudio.MasterAudio.Instance.ReceiversByEventName.Add(audioEventGroup.customEventName, new Dictionary<ICustomEventReceiver, Transform>()
          {
            {
              receiver,
              receiverTrans
            }
          });
        }
        else
        {
          Dictionary<ICustomEventReceiver, Transform> dictionary = DarkTonic.MasterAudio.MasterAudio.Instance.ReceiversByEventName[audioEventGroup.customEventName];
          if (!dictionary.ContainsKey(receiver))
            dictionary.Add(receiver, receiverTrans);
        }
      }
    }
  }

  public static void RemoveCustomEventReceiver(ICustomEventReceiver receiver)
  {
    if (DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown || (UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (UnityEngine.Object) null)
    {
      if (!((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance != (UnityEngine.Object) null))
        return;
      foreach (string key in DarkTonic.MasterAudio.MasterAudio.Instance.ReceiversByEventName.Keys)
        DarkTonic.MasterAudio.MasterAudio.Instance.ReceiversByEventName[key].Remove(receiver);
    }
    else
    {
      for (int index = 0; index < DarkTonic.MasterAudio.MasterAudio.Instance.customEvents.Count; ++index)
      {
        CustomEvent customEvent = DarkTonic.MasterAudio.MasterAudio.Instance.customEvents[index];
        if (receiver.SubscribesToEvent(customEvent.EventName))
          DarkTonic.MasterAudio.MasterAudio.Instance.ReceiversByEventName[customEvent.EventName].Remove(receiver);
      }
    }
  }

  public static List<Transform> ReceiversForEvent(string customEventName)
  {
    List<Transform> transformList = new List<Transform>();
    if (!DarkTonic.MasterAudio.MasterAudio.Instance.ReceiversByEventName.ContainsKey(customEventName))
      return transformList;
    Dictionary<ICustomEventReceiver, Transform> dictionary = DarkTonic.MasterAudio.MasterAudio.Instance.ReceiversByEventName[customEventName];
    foreach (ICustomEventReceiver key in dictionary.Keys)
    {
      if (key.SubscribesToEvent(customEventName))
        transformList.Add(dictionary[key]);
    }
    return transformList;
  }

  public static CustomEventCategory CreateCustomEventCategoryIfNotThere(
    string categoryName,
    bool isTemporary)
  {
    if (DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown)
      return (CustomEventCategory) null;
    if (DarkTonic.MasterAudio.MasterAudio.Instance.customEventCategories.FindAll((Predicate<CustomEventCategory>) (cat => cat.CatName == categoryName)).Count > 0)
      return (CustomEventCategory) null;
    CustomEventCategory categoryIfNotThere = new CustomEventCategory()
    {
      CatName = categoryName,
      ProspectiveName = categoryName,
      IsTemporary = isTemporary
    };
    DarkTonic.MasterAudio.MasterAudio.Instance.customEventCategories.Add(categoryIfNotThere);
    return categoryIfNotThere;
  }

  public static void CreateCustomEvent(
    string customEventName,
    DarkTonic.MasterAudio.MasterAudio.CustomEventReceiveMode eventReceiveMode,
    float distanceThreshold,
    DarkTonic.MasterAudio.MasterAudio.EventReceiveFilter receiveFilter,
    int filterModeQty,
    string categoryName = "",
    bool isTemporary = false,
    bool errorOnDuplicate = true)
  {
    if (DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown)
      return;
    if (DarkTonic.MasterAudio.MasterAudio.Instance.customEvents.FindAll((Predicate<CustomEvent>) (obj => obj.EventName == customEventName)).Count > 0)
    {
      if (!errorOnDuplicate)
        return;
      Debug.LogError((object) $"You already have a Custom Event named '{customEventName}'. No need to add it again.");
    }
    else
    {
      if (string.IsNullOrEmpty(categoryName))
        categoryName = DarkTonic.MasterAudio.MasterAudio.Instance.customEventCategories[0].CatName;
      DarkTonic.MasterAudio.MasterAudio.Instance.customEvents.Add(new CustomEvent(customEventName)
      {
        eventReceiveMode = eventReceiveMode,
        distanceThreshold = distanceThreshold,
        eventRcvFilterMode = receiveFilter,
        filterModeQty = filterModeQty,
        categoryName = categoryName,
        isTemporary = isTemporary
      });
    }
  }

  public static void DeleteCustomEvent(string customEventName)
  {
    if (DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown || (UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (UnityEngine.Object) null)
      return;
    DarkTonic.MasterAudio.MasterAudio.Instance.customEvents.RemoveAll((Predicate<CustomEvent>) (obj => obj.EventName == customEventName));
  }

  public static CustomEvent GetCustomEventByName(string customEventName)
  {
    List<CustomEvent> all = DarkTonic.MasterAudio.MasterAudio.Instance.customEvents.FindAll((Predicate<CustomEvent>) (obj => obj.EventName == customEventName));
    return all.Count <= 0 ? (CustomEvent) null : all[0];
  }

  public static void FireCustomEvent(string customEventName, Transform originObject, bool logDupe = true)
  {
    if (DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown || "[None]" == customEventName || string.IsNullOrEmpty(customEventName))
      return;
    if ((UnityEngine.Object) originObject == (UnityEngine.Object) null)
      Debug.LogError((object) $"Custom Event '{customEventName}' cannot be fired without an originObject passed in.");
    else if (!DarkTonic.MasterAudio.MasterAudio.CustomEventExists(customEventName) && !DarkTonic.MasterAudio.MasterAudio.IsWarming)
    {
      Debug.LogError((object) $"Custom Event '{customEventName}' was not found in Master Audio.");
    }
    else
    {
      CustomEvent customEventByName = DarkTonic.MasterAudio.MasterAudio.GetCustomEventByName(customEventName);
      if (customEventByName == null)
        return;
      if (customEventByName.frameLastFired >= AudioUtil.FrameCount)
      {
        if (!logDupe)
          return;
        Debug.LogWarning((object) $"Already fired Custom Event '{customEventName}' this frame or later. Cannot be fired twice in the same frame.");
      }
      else
      {
        customEventByName.frameLastFired = AudioUtil.FrameCount;
        if (!DarkTonic.MasterAudio.MasterAudio.Instance.disableLogging && DarkTonic.MasterAudio.MasterAudio.Instance.logCustomEvents)
          Debug.Log((object) ("Firing Custom Event: " + customEventName));
        if (!DarkTonic.MasterAudio.MasterAudio.Instance.ReceiversByEventName.ContainsKey(customEventName))
          return;
        Vector3 position = originObject.position;
        float? nullable1 = new float?();
        Dictionary<ICustomEventReceiver, Transform> dictionary = DarkTonic.MasterAudio.MasterAudio.Instance.ReceiversByEventName[customEventName];
        List<ICustomEventReceiver> customEventReceiverList = (List<ICustomEventReceiver>) null;
        switch (customEventByName.eventReceiveMode)
        {
          case DarkTonic.MasterAudio.MasterAudio.CustomEventReceiveMode.WhenDistanceLessThan:
          case DarkTonic.MasterAudio.MasterAudio.CustomEventReceiveMode.WhenDistanceMoreThan:
            nullable1 = new float?(customEventByName.distanceThreshold * customEventByName.distanceThreshold);
            break;
          case DarkTonic.MasterAudio.MasterAudio.CustomEventReceiveMode.Never:
            if (!DarkTonic.MasterAudio.MasterAudio.Instance.LogSounds)
              return;
            Debug.LogWarning((object) $"Custom Event '{customEventName}' not being transmitted because it is set to 'Never transmit'.");
            return;
          case DarkTonic.MasterAudio.MasterAudio.CustomEventReceiveMode.OnChildGameObject:
            customEventReceiverList = DarkTonic.MasterAudio.MasterAudio.GetChildReceivers(originObject, customEventName, false);
            break;
          case DarkTonic.MasterAudio.MasterAudio.CustomEventReceiveMode.OnParentGameObject:
            customEventReceiverList = DarkTonic.MasterAudio.MasterAudio.GetParentReceivers(originObject, customEventName, false);
            break;
          case DarkTonic.MasterAudio.MasterAudio.CustomEventReceiveMode.OnSameOrChildGameObject:
            customEventReceiverList = DarkTonic.MasterAudio.MasterAudio.GetChildReceivers(originObject, customEventName, true);
            break;
          case DarkTonic.MasterAudio.MasterAudio.CustomEventReceiveMode.OnSameOrParentGameObject:
            customEventReceiverList = DarkTonic.MasterAudio.MasterAudio.GetParentReceivers(originObject, customEventName, true);
            break;
        }
        if (customEventReceiverList == null)
        {
          customEventReceiverList = new List<ICustomEventReceiver>();
          foreach (ICustomEventReceiver key in dictionary.Keys)
          {
            float? nullable2;
            switch (customEventByName.eventReceiveMode)
            {
              case DarkTonic.MasterAudio.MasterAudio.CustomEventReceiveMode.WhenDistanceLessThan:
                double sqrMagnitude1 = (double) (dictionary[key].position - position).sqrMagnitude;
                nullable2 = nullable1;
                double valueOrDefault1 = (double) nullable2.GetValueOrDefault();
                if (!(sqrMagnitude1 > valueOrDefault1 & nullable2.HasValue))
                  break;
                continue;
              case DarkTonic.MasterAudio.MasterAudio.CustomEventReceiveMode.WhenDistanceMoreThan:
                double sqrMagnitude2 = (double) (dictionary[key].position - position).sqrMagnitude;
                nullable2 = nullable1;
                double valueOrDefault2 = (double) nullable2.GetValueOrDefault();
                if (!(sqrMagnitude2 < valueOrDefault2 & nullable2.HasValue))
                  break;
                continue;
              case DarkTonic.MasterAudio.MasterAudio.CustomEventReceiveMode.OnSameGameObject:
                if ((UnityEngine.Object) originObject != (UnityEngine.Object) dictionary[key])
                  continue;
                break;
            }
            customEventReceiverList.Add(key);
          }
        }
        if ((customEventByName.eventRcvFilterMode == DarkTonic.MasterAudio.MasterAudio.EventReceiveFilter.All || customEventByName.filterModeQty >= customEventReceiverList.Count ? 0 : (customEventReceiverList.Count > 1 ? 1 : 0)) == 0)
        {
          for (int index = 0; index < customEventReceiverList.Count; ++index)
            customEventReceiverList[index].ReceiveEvent(customEventName, position);
        }
        else
        {
          DarkTonic.MasterAudio.MasterAudio.Instance.ValidReceivers.Clear();
          for (int index = 0; index < customEventReceiverList.Count; ++index)
          {
            ICustomEventReceiver customEventReceiver = customEventReceiverList[index];
            Transform trans = dictionary[customEventReceiver];
            float distance = 0.0f;
            int randomId = 0;
            switch (customEventByName.eventRcvFilterMode)
            {
              case DarkTonic.MasterAudio.MasterAudio.EventReceiveFilter.Closest:
                distance = (trans.position - position).sqrMagnitude;
                break;
              case DarkTonic.MasterAudio.MasterAudio.EventReceiveFilter.Random:
                randomId = UnityEngine.Random.Range(0, 1000);
                break;
            }
            DarkTonic.MasterAudio.MasterAudio.Instance.ValidReceivers.Add(new DarkTonic.MasterAudio.MasterAudio.CustomEventCandidate(distance, customEventReceiver, trans, randomId));
          }
          switch (customEventByName.eventRcvFilterMode)
          {
            case DarkTonic.MasterAudio.MasterAudio.EventReceiveFilter.Closest:
              DarkTonic.MasterAudio.MasterAudio.Instance.ValidReceivers.Sort((Comparison<DarkTonic.MasterAudio.MasterAudio.CustomEventCandidate>) ((x, y) => x.DistanceAway.CompareTo(y.DistanceAway)));
              int filterModeQty1 = customEventByName.filterModeQty;
              int count1 = DarkTonic.MasterAudio.MasterAudio.Instance.ValidReceivers.Count - filterModeQty1;
              DarkTonic.MasterAudio.MasterAudio.Instance.ValidReceivers.RemoveRange(filterModeQty1, count1);
              break;
            case DarkTonic.MasterAudio.MasterAudio.EventReceiveFilter.Random:
              DarkTonic.MasterAudio.MasterAudio.Instance.ValidReceivers.Sort((Comparison<DarkTonic.MasterAudio.MasterAudio.CustomEventCandidate>) ((x, y) => x.RandomId.CompareTo(y.RandomId)));
              int filterModeQty2 = customEventByName.filterModeQty;
              int count2 = DarkTonic.MasterAudio.MasterAudio.Instance.ValidReceivers.Count - filterModeQty2;
              DarkTonic.MasterAudio.MasterAudio.Instance.ValidReceivers.RemoveRange(filterModeQty2, count2);
              break;
          }
          for (int index = 0; index < DarkTonic.MasterAudio.MasterAudio.Instance.ValidReceivers.Count; ++index)
            DarkTonic.MasterAudio.MasterAudio.Instance.ValidReceivers[index].Receiver.ReceiveEvent(customEventName, position);
        }
      }
    }
  }

  public static bool CustomEventExists(string customEventName)
  {
    return DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown || DarkTonic.MasterAudio.MasterAudio.Instance.customEvents.FindAll((Predicate<CustomEvent>) (obj => obj.EventName == customEventName)).Count > 0;
  }

  public static List<ICustomEventReceiver> GetChildReceivers(
    Transform origin,
    string eventName,
    bool includeSelf)
  {
    List<ICustomEventReceiver> list = ((IEnumerable<ICustomEventReceiver>) origin.GetComponentsInChildren<ICustomEventReceiver>()).ToList<ICustomEventReceiver>();
    list.RemoveAll((Predicate<ICustomEventReceiver>) (rec => !rec.SubscribesToEvent(eventName)));
    return includeSelf ? list : DarkTonic.MasterAudio.MasterAudio.FilterOutSelf(list, origin);
  }

  public static List<ICustomEventReceiver> GetParentReceivers(
    Transform origin,
    string eventName,
    bool includeSelf)
  {
    List<ICustomEventReceiver> list = ((IEnumerable<ICustomEventReceiver>) origin.GetComponentsInParent<ICustomEventReceiver>()).ToList<ICustomEventReceiver>();
    list.RemoveAll((Predicate<ICustomEventReceiver>) (rec => !rec.SubscribesToEvent(eventName)));
    return includeSelf ? list : DarkTonic.MasterAudio.MasterAudio.FilterOutSelf(list, origin);
  }

  public static List<ICustomEventReceiver> FilterOutSelf(
    List<ICustomEventReceiver> sourceList,
    Transform origin)
  {
    List<ICustomEventReceiver> customEventReceiverList = new List<ICustomEventReceiver>();
    foreach (ICustomEventReceiver source in sourceList)
    {
      MonoBehaviour monoBehaviour = source as MonoBehaviour;
      if (!((UnityEngine.Object) monoBehaviour == (UnityEngine.Object) null) && !((UnityEngine.Object) monoBehaviour.transform != (UnityEngine.Object) origin))
        customEventReceiverList.Add(source);
    }
    for (int index = 0; customEventReceiverList.Count > 0 && index < 20; ++index)
    {
      sourceList.Remove(customEventReceiverList[0]);
      customEventReceiverList.RemoveAt(0);
    }
    return sourceList;
  }

  public static bool LoggingEnabledForGroup(MasterAudioGroup grp)
  {
    if (DarkTonic.MasterAudio.MasterAudio.IsWarming || DarkTonic.MasterAudio.MasterAudio.Instance.disableLogging)
      return false;
    return (UnityEngine.Object) grp != (UnityEngine.Object) null && grp.logSound || DarkTonic.MasterAudio.MasterAudio.Instance.LogSounds;
  }

  public static void LogMessage(string message)
  {
    if (DarkTonic.MasterAudio.MasterAudio.Instance.disableLogging)
      return;
    Debug.Log((object) $"T: {Time.time.ToString()} - MasterAudio {message}");
  }

  public static bool LogSoundsEnabled
  {
    get => DarkTonic.MasterAudio.MasterAudio.Instance.LogSounds;
    set => DarkTonic.MasterAudio.MasterAudio.Instance.LogSounds = value;
  }

  public static bool LogOutOfVoices
  {
    get => DarkTonic.MasterAudio.MasterAudio.Instance.logOutOfVoices;
    set => DarkTonic.MasterAudio.MasterAudio.Instance.logOutOfVoices = value;
  }

  public static void LogWarning(string msg)
  {
    if (DarkTonic.MasterAudio.MasterAudio.Instance.disableLogging)
      return;
    Debug.LogWarning((object) msg);
  }

  public static void LogError(string msg)
  {
    if (DarkTonic.MasterAudio.MasterAudio.Instance.disableLogging)
      return;
    Debug.LogError((object) msg);
  }

  public static void LogNoPlaylist(string playlistControllerName, string methodName)
  {
    DarkTonic.MasterAudio.MasterAudio.LogWarning($"There is currently no Playlist assigned to Playlist Controller '{playlistControllerName}'. Cannot call '{methodName}' method.");
  }

  public static bool IsOkToCallOnlyPlaylistMethod(List<PlaylistController> pcs, string methodName)
  {
    if (pcs.Count == 0)
    {
      DarkTonic.MasterAudio.MasterAudio.LogError($"You have no Playlist Controllers in the Scene. You cannot '{methodName}'.");
      return false;
    }
    if (pcs.Count <= 1)
      return true;
    DarkTonic.MasterAudio.MasterAudio.LogError($"You cannot call '{methodName}' without specifying a Playlist Controller name when you have more than one Playlist Controller.");
    return false;
  }

  public static void QueueTransformFollowerForColliderPositionRecalc(TransformFollower follower)
  {
    if ((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (UnityEngine.Object) null)
      return;
    foreach (UnityEngine.Object colliderPositionRecalc in DarkTonic.MasterAudio.MasterAudio.Instance.TransFollowerColliderPositionRecalcs)
    {
      if (colliderPositionRecalc == (UnityEngine.Object) follower)
        return;
    }
    DarkTonic.MasterAudio.MasterAudio.Instance.TransFollowerColliderPositionRecalcs.Enqueue(follower);
  }

  public static void AddToQueuedOcclusionRays(SoundGroupVariationUpdater updater)
  {
    if ((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (UnityEngine.Object) null)
      return;
    foreach (UnityEngine.Object queuedOcclusionRay in DarkTonic.MasterAudio.MasterAudio.Instance.QueuedOcclusionRays)
    {
      if (queuedOcclusionRay == (UnityEngine.Object) updater)
        return;
    }
    DarkTonic.MasterAudio.MasterAudio.Instance.QueuedOcclusionRays.Enqueue(updater);
  }

  public static void AddToOcclusionInRangeSources(GameObject src)
  {
    if (!Application.isEditor || (UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (UnityEngine.Object) null || !DarkTonic.MasterAudio.MasterAudio.Instance.occlusionShowCategories)
      return;
    if (!DarkTonic.MasterAudio.MasterAudio.Instance.OcclusionSourcesInRange.Contains(src))
      DarkTonic.MasterAudio.MasterAudio.Instance.OcclusionSourcesInRange.Add(src);
    if (!DarkTonic.MasterAudio.MasterAudio.Instance.OcclusionSourcesOutOfRange.Contains(src))
      return;
    DarkTonic.MasterAudio.MasterAudio.Instance.OcclusionSourcesOutOfRange.Remove(src);
  }

  public static void AddToOcclusionOutOfRangeSources(GameObject src)
  {
    if (!Application.isEditor || (UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (UnityEngine.Object) null || !DarkTonic.MasterAudio.MasterAudio.Instance.occlusionShowCategories)
      return;
    if (!DarkTonic.MasterAudio.MasterAudio.Instance.OcclusionSourcesOutOfRange.Contains(src))
      DarkTonic.MasterAudio.MasterAudio.Instance.OcclusionSourcesOutOfRange.Add(src);
    if (DarkTonic.MasterAudio.MasterAudio.Instance.OcclusionSourcesInRange.Contains(src))
      DarkTonic.MasterAudio.MasterAudio.Instance.OcclusionSourcesInRange.Remove(src);
    DarkTonic.MasterAudio.MasterAudio.RemoveFromBlockedOcclusionSources(src);
  }

  public static void AddToBlockedOcclusionSources(GameObject src)
  {
    if (!Application.isEditor || (UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (UnityEngine.Object) null || !DarkTonic.MasterAudio.MasterAudio.Instance.occlusionShowCategories || DarkTonic.MasterAudio.MasterAudio.Instance.OcclusionSourcesBlocked.Contains(src))
      return;
    DarkTonic.MasterAudio.MasterAudio.Instance.OcclusionSourcesBlocked.Add(src);
  }

  public static bool HasQueuedOcclusionRays() => DarkTonic.MasterAudio.MasterAudio.Instance.QueuedOcclusionRays.Count > 0;

  public static SoundGroupVariationUpdater OldestQueuedOcclusionRay()
  {
    return (UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (UnityEngine.Object) null ? (SoundGroupVariationUpdater) null : DarkTonic.MasterAudio.MasterAudio.Instance.QueuedOcclusionRays.Dequeue();
  }

  public static bool IsOcclusionFreqencyTransitioning(SoundGroupVariation variation)
  {
    for (int index = 0; index < DarkTonic.MasterAudio.MasterAudio.Instance.VariationOcclusionFreqChanges.Count; ++index)
    {
      if ((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.Instance.VariationOcclusionFreqChanges[index].ActingVariation == (UnityEngine.Object) variation)
        return true;
    }
    return false;
  }

  public static void RemoveFromOcclusionFrequencyTransitioning(SoundGroupVariation variation)
  {
    for (int index = 0; index < DarkTonic.MasterAudio.MasterAudio.Instance.VariationOcclusionFreqChanges.Count; ++index)
    {
      if (!((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.Instance.VariationOcclusionFreqChanges[index].ActingVariation != (UnityEngine.Object) variation))
      {
        DarkTonic.MasterAudio.MasterAudio.Instance.VariationOcclusionFreqChanges.RemoveAt(index);
        break;
      }
    }
  }

  public static void RemoveFromBlockedOcclusionSources(GameObject src)
  {
    if (!Application.isEditor || (UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (UnityEngine.Object) null || !DarkTonic.MasterAudio.MasterAudio.Instance.occlusionShowCategories || !DarkTonic.MasterAudio.MasterAudio.Instance.OcclusionSourcesBlocked.Contains(src))
      return;
    DarkTonic.MasterAudio.MasterAudio.Instance.OcclusionSourcesBlocked.Remove(src);
  }

  public static void StopTrackingOcclusionForSource(GameObject src)
  {
    if (!Application.isEditor || (UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (UnityEngine.Object) null || !DarkTonic.MasterAudio.MasterAudio.Instance.occlusionShowCategories)
      return;
    if (DarkTonic.MasterAudio.MasterAudio.Instance.OcclusionSourcesOutOfRange.Contains(src))
      DarkTonic.MasterAudio.MasterAudio.Instance.OcclusionSourcesOutOfRange.Remove(src);
    if (DarkTonic.MasterAudio.MasterAudio.Instance.OcclusionSourcesInRange.Contains(src))
      DarkTonic.MasterAudio.MasterAudio.Instance.OcclusionSourcesInRange.Remove(src);
    if (!DarkTonic.MasterAudio.MasterAudio.Instance.OcclusionSourcesBlocked.Contains(src))
      return;
    DarkTonic.MasterAudio.MasterAudio.Instance.OcclusionSourcesBlocked.Remove(src);
  }

  public static bool IsLinkedGroupPlay(SoundGroupVariation variation)
  {
    return DarkTonic.MasterAudio.MasterAudio.Instance._isStoppingMultiple && DarkTonic.MasterAudio.MasterAudio.Instance.VariationsStartedDuringMultiStop.Contains(variation);
  }

  public static List<AudioSource> MasterAudioSources => DarkTonic.MasterAudio.MasterAudio.Instance.AllAudioSources;

  public static int RemainingClipsInGroup(string sType)
  {
    return !DarkTonic.MasterAudio.MasterAudio.Instance._randomizer.ContainsKey(sType) ? 0 : DarkTonic.MasterAudio.MasterAudio.Instance._randomizer[sType].Count;
  }

  public static Transform ListenerTrans
  {
    get
    {
      if ((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio._listenerTrans == (UnityEngine.Object) null || !DTMonoHelper.IsActive(DarkTonic.MasterAudio.MasterAudio._listenerTrans.gameObject))
      {
        DarkTonic.MasterAudio.MasterAudio._listenerTrans = (Transform) null;
        foreach (AudioListener audioListener in UnityEngine.Object.FindObjectsOfType<AudioListener>())
        {
          if (DTMonoHelper.IsActive(audioListener.gameObject))
            DarkTonic.MasterAudio.MasterAudio._listenerTrans = audioListener.transform;
        }
      }
      return DarkTonic.MasterAudio.MasterAudio._listenerTrans;
    }
  }

  public static PlaylistController OnlyPlaylistController
  {
    get
    {
      List<PlaylistController> instances = PlaylistController.Instances;
      if (instances.Count != 0)
        return instances[0];
      Debug.LogError((object) "There are no Playlist Controller in this Scene.");
      return (PlaylistController) null;
    }
  }

  public static bool IsWarming
  {
    get => (UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance != (UnityEngine.Object) null && DarkTonic.MasterAudio.MasterAudio.Instance._warming;
  }

  public static bool MixerMuted
  {
    get => DarkTonic.MasterAudio.MasterAudio.Instance.mixerMuted;
    set
    {
      DarkTonic.MasterAudio.MasterAudio.Instance.mixerMuted = value;
      if (value)
      {
        foreach (string key in DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.Keys)
          DarkTonic.MasterAudio.MasterAudio.MuteGroup(DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[key].Group.GameObjectName, false);
      }
      else
      {
        foreach (string key in DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.Keys)
          DarkTonic.MasterAudio.MasterAudio.UnmuteGroup(DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType[key].Group.GameObjectName, false);
      }
      if (!Application.isPlaying)
        return;
      DarkTonic.MasterAudio.MasterAudio.SilenceOrUnsilenceGroupsFromSoloChange();
    }
  }

  public static bool PlaylistsMuted
  {
    get => DarkTonic.MasterAudio.MasterAudio.Instance.playlistsMuted;
    set
    {
      DarkTonic.MasterAudio.MasterAudio.Instance.playlistsMuted = value;
      List<PlaylistController> instances = PlaylistController.Instances;
      for (int index = 0; index < instances.Count; ++index)
      {
        if (value)
          instances[index].MutePlaylist();
        else
          instances[index].UnmutePlaylist();
      }
    }
  }

  public bool EnableMusicDucking
  {
    get => this.enableMusicDucking;
    set => this.enableMusicDucking = value;
  }

  public float MasterCrossFadeTime => this.crossFadeTime;

  public static List<DarkTonic.MasterAudio.MasterAudio.Playlist> MusicPlaylists
  {
    get => DarkTonic.MasterAudio.MasterAudio.Instance.musicPlaylists;
  }

  public static List<GroupBus> GroupBuses => DarkTonic.MasterAudio.MasterAudio.Instance.groupBuses;

  public static List<string> RuntimeSoundGroupNames
  {
    get
    {
      return !Application.isPlaying ? new List<string>() : new List<string>((IEnumerable<string>) DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.Keys);
    }
  }

  public static List<string> RuntimeBusNames
  {
    get
    {
      if (!Application.isPlaying)
        return new List<string>();
      List<string> runtimeBusNames = new List<string>();
      for (int index = 0; index < DarkTonic.MasterAudio.MasterAudio.Instance.groupBuses.Count; ++index)
        runtimeBusNames.Add(DarkTonic.MasterAudio.MasterAudio.Instance.groupBuses[index].busName);
      return runtimeBusNames;
    }
  }

  public static DarkTonic.MasterAudio.MasterAudio SafeInstance
  {
    get
    {
      if ((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio._instance != (UnityEngine.Object) null)
        return DarkTonic.MasterAudio.MasterAudio._instance;
      DarkTonic.MasterAudio.MasterAudio._instance = (DarkTonic.MasterAudio.MasterAudio) UnityEngine.Object.FindObjectOfType(typeof (DarkTonic.MasterAudio.MasterAudio));
      return DarkTonic.MasterAudio.MasterAudio._instance;
    }
  }

  public static DarkTonic.MasterAudio.MasterAudio Instance
  {
    get
    {
      if ((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio._instance != (UnityEngine.Object) null)
        return DarkTonic.MasterAudio.MasterAudio._instance;
      DarkTonic.MasterAudio.MasterAudio._instance = (DarkTonic.MasterAudio.MasterAudio) UnityEngine.Object.FindObjectOfType(typeof (DarkTonic.MasterAudio.MasterAudio));
      if ((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio._instance == (UnityEngine.Object) null && Application.isPlaying)
        Debug.LogError((object) "There is no Master Audio prefab in this Scene. Subsequent method calls will fail.");
      return DarkTonic.MasterAudio.MasterAudio._instance;
    }
    set => DarkTonic.MasterAudio.MasterAudio._instance = (DarkTonic.MasterAudio.MasterAudio) null;
  }

  public static bool SoundsReady
  {
    get => (UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.Instance != (UnityEngine.Object) null && DarkTonic.MasterAudio.MasterAudio.Instance._soundsLoaded;
  }

  public static bool AppIsShuttingDown
  {
    get => DarkTonic.MasterAudio.MasterAudio.\u003CAppIsShuttingDown\u003Ek__BackingField;
    set => DarkTonic.MasterAudio.MasterAudio.\u003CAppIsShuttingDown\u003Ek__BackingField = value;
  }

  public List<string> GroupNames
  {
    get
    {
      List<string> groupHardCodedNames = DarkTonic.MasterAudio.MasterAudio.SoundGroupHardCodedNames;
      List<string> collection = new List<string>(this.Trans.childCount);
      for (int index = 0; index < this.Trans.childCount; ++index)
      {
        string name = this.Trans.GetChild(index).name;
        if (!ArrayListUtil.IsExcludedChildName(name))
          collection.Add(name);
      }
      foreach (Component component1 in UnityEngine.Object.FindObjectsOfType(typeof (DynamicSoundGroupCreator)) as DynamicSoundGroupCreator[])
      {
        Transform transform = component1.transform;
        for (int index = 0; index < transform.childCount; ++index)
        {
          DynamicSoundGroup component2 = transform.GetChild(index).GetComponent<DynamicSoundGroup>();
          if (!((UnityEngine.Object) component2 == (UnityEngine.Object) null) && !collection.Contains(component2.name))
            collection.Add(component2.name);
        }
      }
      collection.Sort();
      groupHardCodedNames.AddRange((IEnumerable<string>) collection);
      return groupHardCodedNames;
    }
  }

  public static List<string> SoundGroupHardCodedNames
  {
    get => new List<string>() { "[Type In]", "[None]" };
  }

  public List<string> BusNames
  {
    get
    {
      List<string> busNames = new List<string>()
      {
        "[Type In]",
        "[None]"
      };
      for (int index = 0; index < this.groupBuses.Count; ++index)
        busNames.Add(this.groupBuses[index].busName);
      return busNames;
    }
  }

  public List<string> PlaylistNames
  {
    get
    {
      List<string> playlistNames = new List<string>()
      {
        "[Type In]",
        "[No Playlist]"
      };
      for (int index = 0; index < this.musicPlaylists.Count; ++index)
        playlistNames.Add(this.musicPlaylists[index].playlistName);
      return playlistNames;
    }
  }

  public List<string> PlaylistNamesOnly
  {
    get
    {
      List<string> playlistNamesOnly = new List<string>(this.musicPlaylists.Count);
      for (int index = 0; index < this.musicPlaylists.Count; ++index)
        playlistNamesOnly.Add(this.musicPlaylists[index].playlistName);
      return playlistNamesOnly;
    }
  }

  public Transform Trans
  {
    get
    {
      if ((UnityEngine.Object) this._trans != (UnityEngine.Object) null)
        return this._trans;
      this._trans = this.GetComponent<Transform>();
      return this._trans;
    }
  }

  public bool ShouldShowUnityAudioMixerGroupAssignments => this.showUnityMixerGroupAssignment;

  public List<string> CustomEventNames
  {
    get
    {
      List<string> eventHardCodedNames = DarkTonic.MasterAudio.MasterAudio.CustomEventHardCodedNames;
      List<CustomEvent> customEvents = DarkTonic.MasterAudio.MasterAudio.Instance.customEvents;
      for (int index = 0; index < customEvents.Count; ++index)
        eventHardCodedNames.Add(customEvents[index].EventName);
      return eventHardCodedNames;
    }
  }

  public List<string> CustomEventNamesOnly
  {
    get
    {
      List<string> customEventNamesOnly = new List<string>(this.customEvents.Count);
      List<CustomEvent> customEvents = DarkTonic.MasterAudio.MasterAudio.Instance.customEvents;
      for (int index = 0; index < customEvents.Count; ++index)
        customEventNamesOnly.Add(customEvents[index].EventName);
      return customEventNamesOnly;
    }
  }

  public static List<string> CustomEventHardCodedNames
  {
    get => new List<string>() { "[Type In]", "[None]" };
  }

  public static float MasterVolumeLevel
  {
    get => DarkTonic.MasterAudio.MasterAudio.Instance._masterAudioVolume;
    set
    {
      DarkTonic.MasterAudio.MasterAudio.Instance._masterAudioVolume = value;
      if (!Application.isPlaying)
        return;
      Dictionary<string, DarkTonic.MasterAudio.MasterAudio.AudioGroupInfo>.Enumerator enumerator = DarkTonic.MasterAudio.MasterAudio.Instance.AudioSourcesBySoundType.GetEnumerator();
      while (enumerator.MoveNext())
      {
        MasterAudioGroup group = enumerator.Current.Value.Group;
        DarkTonic.MasterAudio.MasterAudio.SetGroupVolume(group.GameObjectName, group.groupMasterVolume);
      }
    }
  }

  public static bool SceneHasMasterAudio => (UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.Instance != (UnityEngine.Object) null;

  public static bool IgnoreTimeScale => DarkTonic.MasterAudio.MasterAudio.Instance.ignoreTimeScale;

  public static SystemLanguage DynamicLanguage
  {
    get
    {
      if (!PlayerPrefs.HasKey("~MA_Language_Key~") || string.IsNullOrEmpty(PlayerPrefs.GetString("~MA_Language_Key~")))
        PlayerPrefs.SetString("~MA_Language_Key~", SystemLanguage.Unknown.ToString());
      return (SystemLanguage) Enum.Parse(typeof (SystemLanguage), PlayerPrefs.GetString("~MA_Language_Key~"));
    }
    set
    {
      PlayerPrefs.SetString("~MA_Language_Key~", value.ToString());
      AudioResourceOptimizer.ClearSupportLanguageFolder();
    }
  }

  public static float ReprioritizeTime
  {
    get
    {
      if ((double) DarkTonic.MasterAudio.MasterAudio.Instance._repriTime < 0.0)
        DarkTonic.MasterAudio.MasterAudio.Instance._repriTime = (float) (DarkTonic.MasterAudio.MasterAudio.Instance.rePrioritizeEverySecIndex + 1) * 0.1f;
      return DarkTonic.MasterAudio.MasterAudio.Instance._repriTime;
    }
  }

  public static bool HasAsyncResourceLoaderFeature() => true;

  public static void RescanGroupsNow() => DarkTonic.MasterAudio.MasterAudio.Instance._mustRescanGroups = true;

  public static void DoneRescanningGroups() => DarkTonic.MasterAudio.MasterAudio.Instance._mustRescanGroups = false;

  public static bool ShouldRescanGroups
  {
    get
    {
      return !((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (UnityEngine.Object) null) && DarkTonic.MasterAudio.MasterAudio.Instance._mustRescanGroups;
    }
  }

  public static string ProspectiveMAPath
  {
    get => DarkTonic.MasterAudio.MasterAudio._prospectiveMAFolder;
    set => DarkTonic.MasterAudio.MasterAudio._prospectiveMAFolder = value;
  }

  public static GameObject CreateMasterAudio()
  {
    UnityEngine.Object original = Resources.Load("Assets/Plugins/DarkTonic/MasterAudio/Prefabs/MasterAudio.prefab", typeof (GameObject));
    if (original == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Could not find MasterAudio prefab. Please update the Installation Path in the Master Audio Manager window if you have moved the folder from its default location, then try again.");
      return (GameObject) null;
    }
    GameObject masterAudio = UnityEngine.Object.Instantiate(original) as GameObject;
    masterAudio.name = nameof (MasterAudio);
    return masterAudio;
  }

  public static GameObject CreatePlaylistController()
  {
    UnityEngine.Object original = Resources.Load("Assets/Plugins/DarkTonic/MasterAudio/Prefabs/PlaylistController.prefab", typeof (GameObject));
    if (original == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Could not find PlaylistController prefab. Please update the Installation Path in the Master Audio Manager window if you have moved the folder from its default location, then try again.");
      return (GameObject) null;
    }
    GameObject playlistController = UnityEngine.Object.Instantiate(original) as GameObject;
    playlistController.name = "PlaylistController";
    return playlistController;
  }

  public static GameObject CreateDynamicSoundGroupCreator()
  {
    UnityEngine.Object original = Resources.Load("Assets/Plugins/DarkTonic/MasterAudio/Prefabs/DynamicSoundGroupCreator.prefab", typeof (GameObject));
    if (original == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Could not find DynamicSoundGroupCreator prefab. Please update the Installation Path in the Master Audio Manager window if you have moved the folder from its default location, then try again.");
      return (GameObject) null;
    }
    GameObject soundGroupCreator = UnityEngine.Object.Instantiate(original) as GameObject;
    soundGroupCreator.name = "DynamicSoundGroupCreator";
    return soundGroupCreator;
  }

  public static GameObject CreateSoundGroupOrganizer()
  {
    UnityEngine.Object original = Resources.Load("Assets/Plugins/DarkTonic/MasterAudio/Prefabs/SoundGroupOrganizer.prefab", typeof (GameObject));
    if (original == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Could not find SoundGroupOrganizer prefab. Please update the Installation Path in the Master Audio Manager window if you have moved the folder from its default location, then try again.");
      return (GameObject) null;
    }
    GameObject soundGroupOrganizer = UnityEngine.Object.Instantiate(original) as GameObject;
    soundGroupOrganizer.name = "SoundGroupOrganizer";
    return soundGroupOrganizer;
  }

  public enum VariationFollowerType
  {
    LateUpdate,
    FixedUpdate,
  }

  public enum LinkedGroupSelectionType
  {
    All,
    OneAtRandom,
  }

  public enum OcclusionSelectionType
  {
    AllGroups,
    TurnOnPerBusOrGroup,
  }

  public enum RaycastMode
  {
    Physics3D,
    Physics2D,
  }

  public enum AllMusicSpatialBlendType
  {
    ForceAllTo2D,
    ForceAllTo3D,
    ForceAllToCustom,
    AllowDifferentPerController,
  }

  public enum AllMixerSpatialBlendType
  {
    ForceAllTo2D,
    ForceAllTo3D,
    ForceAllToCustom,
    AllowDifferentPerGroup,
  }

  public enum ItemSpatialBlendType
  {
    ForceTo2D,
    ForceTo3D,
    ForceToCustom,
    UseCurveFromAudioSource,
  }

  public enum InternetFileLoadStatus
  {
    Loading,
    Loaded,
    Failed,
  }

  public enum MixerWidthMode
  {
    Narrow,
    Normal,
    Wide,
  }

  public enum CustomEventReceiveMode
  {
    Always,
    WhenDistanceLessThan,
    WhenDistanceMoreThan,
    Never,
    OnSameGameObject,
    OnChildGameObject,
    OnParentGameObject,
    OnSameOrChildGameObject,
    OnSameOrParentGameObject,
  }

  public enum EventReceiveFilter
  {
    All,
    Closest,
    Random,
  }

  public enum AudioLocation
  {
    Clip,
    ResourceFile,
    FileOnInternet,
  }

  public enum CustomSongStartTimeMode
  {
    Beginning,
    SpecificTime,
    RandomTime,
  }

  public enum BusCommand
  {
    None,
    FadeToVolume,
    Mute,
    Pause,
    Solo,
    Unmute,
    Unpause,
    Unsolo,
    Stop,
    ChangePitch,
    ToggleMute,
    StopBusOfTransform,
    PauseBusOfTransform,
    UnpauseBusOfTransform,
    GlideByPitch,
  }

  public enum DragGroupMode
  {
    OneGroupPerClip,
    OneGroupWithVariations,
  }

  public enum EventSoundFunctionType
  {
    PlaySound,
    GroupControl,
    BusControl,
    PlaylistControl,
    CustomEventControl,
    GlobalControl,
    UnityMixerControl,
    PersistentSettingsControl,
  }

  public enum LanguageMode
  {
    UseDeviceSetting,
    SpecificLanguage,
    DynamicallySet,
  }

  public enum UnityMixerCommand
  {
    None,
    TransitionToSnapshot,
    TransitionToSnapshotBlend,
  }

  public enum PlaylistCommand
  {
    None,
    ChangePlaylist,
    FadeToVolume,
    PlaySong,
    PlayRandomSong,
    PlayNextSong,
    Pause,
    Resume,
    Stop,
    Mute,
    Unmute,
    ToggleMute,
    Restart,
    Start,
    StopLoopingCurrentSong,
    StopPlaylistAfterCurrentSong,
    AddSongToQueue,
  }

  public enum CustomEventCommand
  {
    None,
    FireEvent,
  }

  public enum GlobalCommand
  {
    None,
    PauseMixer,
    UnpauseMixer,
    StopMixer,
    StopEverything,
    PauseEverything,
    UnpauseEverything,
    MuteEverything,
    UnmuteEverything,
    SetMasterMixerVolume,
    SetMasterPlaylistVolume,
  }

  public enum SoundGroupCommand
  {
    None,
    FadeToVolume,
    FadeOutAllOfSound,
    Mute,
    Pause,
    Solo,
    StopAllOfSound,
    Unmute,
    Unpause,
    Unsolo,
    StopAllSoundsOfTransform,
    PauseAllSoundsOfTransform,
    UnpauseAllSoundsOfTransform,
    StopSoundGroupOfTransform,
    PauseSoundGroupOfTransform,
    UnpauseSoundGroupOfTransform,
    FadeOutSoundGroupOfTransform,
    RefillSoundGroupPool,
    RouteToBus,
    GlideByPitch,
    ToggleSoundGroup,
    ToggleSoundGroupOfTransform,
    FadeOutAllSoundsOfTransform,
  }

  public enum PersistentSettingsCommand
  {
    None,
    SetBusVolume,
    SetGroupVolume,
    SetMixerVolume,
    SetMusicVolume,
    MixerMuteToggle,
    MusicMuteToggle,
  }

  public enum SongFadeInPosition
  {
    NewClipFromBeginning = 1,
    NewClipFromLastKnownPosition = 3,
    SynchronizeClips = 5,
  }

  public enum SoundSpawnLocationMode
  {
    MasterAudioLocation,
    CallerLocation,
    AttachToCaller,
  }

  public enum VariationCommand
  {
    None,
    Stop,
    Pause,
    Unpause,
  }

  public struct CustomEventCandidate(
    float distance,
    ICustomEventReceiver rec,
    Transform trans,
    int randomId)
  {
    public float DistanceAway = distance;
    public ICustomEventReceiver Receiver = rec;
    public Transform Trans = trans;
    public int RandomId = randomId;
  }

  [Serializable]
  public class AudioGroupInfo
  {
    public List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> Sources;
    public int LastFramePlayed;
    public float LastTimePlayed;
    public MasterAudioGroup Group;
    public bool PlayedForWarming;

    public AudioGroupInfo(List<DarkTonic.MasterAudio.MasterAudio.AudioInfo> sources, MasterAudioGroup groupScript)
    {
      this.Sources = sources;
      this.LastFramePlayed = -50;
      this.LastTimePlayed = -50f;
      this.Group = groupScript;
      this.PlayedForWarming = false;
    }
  }

  [Serializable]
  public class AudioInfo
  {
    public AudioSource Source;
    public float OriginalVolume;
    public float LastPercentageVolume;
    public float LastRandomVolume;
    public SoundGroupVariation Variation;

    public AudioInfo(SoundGroupVariation variation, AudioSource source, float origVol)
    {
      this.Variation = variation;
      this.Source = source;
      this.OriginalVolume = origVol;
      this.LastPercentageVolume = 1f;
      this.LastRandomVolume = 0.0f;
    }
  }

  [Serializable]
  public class Playlist
  {
    public bool isExpanded = true;
    public string playlistName = "new playlist";
    public DarkTonic.MasterAudio.MasterAudio.SongFadeInPosition songTransitionType = DarkTonic.MasterAudio.MasterAudio.SongFadeInPosition.NewClipFromBeginning;
    public List<MusicSetting> MusicSettings;
    public DarkTonic.MasterAudio.MasterAudio.AudioLocation bulkLocationMode;
    public DarkTonic.MasterAudio.MasterAudio.Playlist.CrossfadeTimeMode crossfadeMode;
    public float crossFadeTime = 1f;
    public bool fadeInFirstSong;
    public bool fadeOutLastSong;
    public bool resourceClipsAllLoadAsync = true;
    public bool isTemporary;

    public Playlist() => this.MusicSettings = new List<MusicSetting>();

    public enum CrossfadeTimeMode
    {
      UseMasterSetting,
      Override,
    }
  }

  [Serializable]
  public class SoundGroupRefillInfo
  {
    public float LastTimePlayed;
    public float InactivePeriodSeconds;

    public SoundGroupRefillInfo(float lastTimePlayed, float inactivePeriodSeconds)
    {
      this.LastTimePlayed = lastTimePlayed;
      this.InactivePeriodSeconds = inactivePeriodSeconds;
    }
  }
}
