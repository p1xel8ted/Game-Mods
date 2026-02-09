// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.DynamicSoundGroupCreator
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

[AudioScriptOrder(-35)]
public class DynamicSoundGroupCreator : MonoBehaviour
{
  public const int ExtraHardCodedBusOptions = 1;
  public SystemLanguage previewLanguage = SystemLanguage.English;
  public DarkTonic.MasterAudio.MasterAudio.DragGroupMode curDragGroupMode;
  public GameObject groupTemplate;
  public GameObject variationTemplate;
  public bool errorOnDuplicates;
  public bool createOnAwake = true;
  public bool soundGroupsAreExpanded = true;
  public bool removeGroupsOnSceneChange = true;
  public DynamicSoundGroupCreator.CreateItemsWhen reUseMode;
  public bool showCustomEvents = true;
  public DarkTonic.MasterAudio.MasterAudio.AudioLocation bulkVariationMode;
  public List<CustomEvent> customEventsToCreate = new List<CustomEvent>();
  public List<CustomEventCategory> customEventCategories = new List<CustomEventCategory>()
  {
    new CustomEventCategory()
  };
  public string newEventName = "my event";
  public string newCustomEventCategoryName = "New Category";
  public string addToCustomEventCategoryName = "New Category";
  public bool showMusicDucking = true;
  public List<DuckGroupInfo> musicDuckingSounds = new List<DuckGroupInfo>();
  public List<GroupBus> groupBuses = new List<GroupBus>();
  public bool playListExpanded;
  public bool playlistEditorExp = true;
  public List<DarkTonic.MasterAudio.MasterAudio.Playlist> musicPlaylists = new List<DarkTonic.MasterAudio.MasterAudio.Playlist>();
  public List<GameObject> audioSourceTemplates = new List<GameObject>(10);
  public string audioSourceTemplateName = "Max Distance 500";
  public bool groupByBus;
  public bool itemsCreatedEventExpanded;
  public string itemsCreatedCustomEvent = string.Empty;
  public bool showUnityMixerGroupAssignment = true;
  public bool _hasCreated;
  public List<Transform> _groupsToRemove = new List<Transform>();
  public Transform _trans;
  public List<DynamicSoundGroup> _groupsToCreate = new List<DynamicSoundGroup>();

  public void Awake()
  {
    this._trans = this.transform;
    this._hasCreated = false;
    AudioSource component = this.GetComponent<AudioSource>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) component);
  }

  public void OnEnable() => this.CreateItemsIfReady();

  public void Start() => this.CreateItemsIfReady();

  public void OnDisable()
  {
    if (DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown || !this.removeGroupsOnSceneChange || !((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance != (UnityEngine.Object) null))
      return;
    this.RemoveItems();
  }

  public void CreateItemsIfReady()
  {
    if ((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (UnityEngine.Object) null || !this.createOnAwake || !DarkTonic.MasterAudio.MasterAudio.SoundsReady || this._hasCreated)
      return;
    this.CreateItems();
  }

  public void RemoveItems()
  {
    for (int index = 0; index < this.groupBuses.Count; ++index)
    {
      GroupBus groupBus = this.groupBuses[index];
      if (!groupBus.isExisting)
        DarkTonic.MasterAudio.MasterAudio.DeleteBusByName(groupBus.busName);
    }
    for (int index = 0; index < this._groupsToRemove.Count; ++index)
    {
      string name = this._groupsToRemove[index].name;
      DarkTonic.MasterAudio.MasterAudio.RemoveSoundGroupFromDuckList(name);
      DarkTonic.MasterAudio.MasterAudio.DeleteSoundGroup(name);
    }
    this._groupsToRemove.Clear();
    for (int index = 0; index < this.customEventsToCreate.Count; ++index)
      DarkTonic.MasterAudio.MasterAudio.DeleteCustomEvent(this.customEventsToCreate[index].EventName);
    for (int index = 0; index < this.customEventCategories.Count; ++index)
    {
      CustomEventCategory aCat = this.customEventCategories[index];
      DarkTonic.MasterAudio.MasterAudio.Instance.customEventCategories.RemoveAll((Predicate<CustomEventCategory>) (cat => cat.CatName == aCat.CatName && cat.IsTemporary));
    }
    for (int index = 0; index < this.musicPlaylists.Count; ++index)
      DarkTonic.MasterAudio.MasterAudio.DeletePlaylist(this.musicPlaylists[index].playlistName);
    if (this.reUseMode == DynamicSoundGroupCreator.CreateItemsWhen.EveryEnable)
      this._hasCreated = false;
    DarkTonic.MasterAudio.MasterAudio.SilenceOrUnsilenceGroupsFromSoloChange();
  }

  public void CreateItems()
  {
    if (this._hasCreated)
    {
      Debug.LogWarning((object) $"DynamicSoundGroupCreator '{this.transform.name}' has already created its items. Cannot create again.");
    }
    else
    {
      if ((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.Instance == (UnityEngine.Object) null)
        return;
      this.PopulateGroupData();
      for (int index = 0; index < this.groupBuses.Count; ++index)
      {
        GroupBus groupBus1 = this.groupBuses[index];
        if (groupBus1.isExisting)
        {
          if (DarkTonic.MasterAudio.MasterAudio.GrabBusByName(groupBus1.busName) == null)
            DarkTonic.MasterAudio.MasterAudio.LogWarning($"Existing bus '{groupBus1.busName}' was not found, specified in prefab '{this.name}'.");
        }
        else if (DarkTonic.MasterAudio.MasterAudio.CreateBus(groupBus1.busName, this.errorOnDuplicates, true))
        {
          GroupBus groupBus2 = DarkTonic.MasterAudio.MasterAudio.GrabBusByName(groupBus1.busName);
          if (groupBus2 != null)
          {
            if (!PersistentAudioSettings.GetBusVolume(groupBus1.busName).HasValue)
            {
              groupBus2.volume = groupBus1.volume;
              groupBus2.OriginalVolume = groupBus2.volume;
            }
            groupBus2.voiceLimit = groupBus1.voiceLimit;
            groupBus2.stopOldest = groupBus1.stopOldest;
            groupBus2.forceTo2D = groupBus1.forceTo2D;
            groupBus2.mixerChannel = groupBus1.mixerChannel;
            groupBus2.isUsingOcclusion = groupBus1.isUsingOcclusion;
          }
        }
      }
      for (int index1 = 0; index1 < this._groupsToCreate.Count; ++index1)
      {
        DynamicSoundGroup aGroup = this._groupsToCreate[index1];
        string str = string.Empty;
        int busIndex = aGroup.busIndex == -1 ? 0 : aGroup.busIndex;
        if (busIndex >= DynamicSoundGroupCreator.HardCodedBusOptions)
          str = this.groupBuses[busIndex - DynamicSoundGroupCreator.HardCodedBusOptions].busName;
        aGroup.busName = str;
        Transform soundGroup = DarkTonic.MasterAudio.MasterAudio.CreateSoundGroup(aGroup, this._trans.name, this.errorOnDuplicates);
        for (int index2 = 0; index2 < aGroup.groupVariations.Count; ++index2)
        {
          DynamicGroupVariation groupVariation = aGroup.groupVariations[index2];
          if ((UnityEngine.Object) groupVariation.LowPassFilter != (UnityEngine.Object) null)
            UnityEngine.Object.Destroy((UnityEngine.Object) groupVariation.LowPassFilter);
          if ((UnityEngine.Object) groupVariation.HighPassFilter != (UnityEngine.Object) null)
            UnityEngine.Object.Destroy((UnityEngine.Object) groupVariation.HighPassFilter);
          if ((UnityEngine.Object) groupVariation.DistortionFilter != (UnityEngine.Object) null)
            UnityEngine.Object.Destroy((UnityEngine.Object) groupVariation.DistortionFilter);
          if ((UnityEngine.Object) groupVariation.ChorusFilter != (UnityEngine.Object) null)
            UnityEngine.Object.Destroy((UnityEngine.Object) groupVariation.ChorusFilter);
          if ((UnityEngine.Object) groupVariation.EchoFilter != (UnityEngine.Object) null)
            UnityEngine.Object.Destroy((UnityEngine.Object) groupVariation.EchoFilter);
          if ((UnityEngine.Object) groupVariation.ReverbFilter != (UnityEngine.Object) null)
            UnityEngine.Object.Destroy((UnityEngine.Object) groupVariation.ReverbFilter);
        }
        if (!((UnityEngine.Object) soundGroup == (UnityEngine.Object) null))
          this._groupsToRemove.Add(soundGroup);
      }
      for (int index = 0; index < this.musicDuckingSounds.Count; ++index)
      {
        DuckGroupInfo musicDuckingSound = this.musicDuckingSounds[index];
        if (!(musicDuckingSound.soundType == "[None]"))
          DarkTonic.MasterAudio.MasterAudio.AddSoundGroupToDuckList(musicDuckingSound.soundType, musicDuckingSound.riseVolStart, musicDuckingSound.duckedVolumeCut, musicDuckingSound.unduckTime, true);
      }
      for (int index = 0; index < this.customEventCategories.Count; ++index)
        DarkTonic.MasterAudio.MasterAudio.CreateCustomEventCategoryIfNotThere(this.customEventCategories[index].CatName, true);
      for (int index = 0; index < this.customEventsToCreate.Count; ++index)
      {
        CustomEvent customEvent = this.customEventsToCreate[index];
        DarkTonic.MasterAudio.MasterAudio.CreateCustomEvent(customEvent.EventName, customEvent.eventReceiveMode, customEvent.distanceThreshold, customEvent.eventRcvFilterMode, customEvent.filterModeQty, customEvent.categoryName, true, this.errorOnDuplicates);
      }
      for (int index = 0; index < this.musicPlaylists.Count; ++index)
      {
        DarkTonic.MasterAudio.MasterAudio.Playlist musicPlaylist = this.musicPlaylists[index];
        musicPlaylist.isTemporary = true;
        DarkTonic.MasterAudio.MasterAudio.CreatePlaylist(musicPlaylist, this.errorOnDuplicates);
      }
      DarkTonic.MasterAudio.MasterAudio.SilenceOrUnsilenceGroupsFromSoloChange();
      this._hasCreated = true;
      if (!this.itemsCreatedEventExpanded)
        return;
      this.FireEvents();
    }
  }

  public void FireEvents()
  {
    DarkTonic.MasterAudio.MasterAudio.FireCustomEventNextFrame(this.itemsCreatedCustomEvent, this._trans);
  }

  public void PopulateGroupData()
  {
    if ((UnityEngine.Object) this._trans == (UnityEngine.Object) null)
      this._trans = this.transform;
    this._groupsToCreate.Clear();
    for (int index1 = 0; index1 < this._trans.childCount; ++index1)
    {
      DynamicSoundGroup component1 = this._trans.GetChild(index1).GetComponent<DynamicSoundGroup>();
      if (!((UnityEngine.Object) component1 == (UnityEngine.Object) null))
      {
        component1.groupVariations.Clear();
        for (int index2 = 0; index2 < component1.transform.childCount; ++index2)
        {
          DynamicGroupVariation component2 = component1.transform.GetChild(index2).GetComponent<DynamicGroupVariation>();
          if (!((UnityEngine.Object) component2 == (UnityEngine.Object) null))
            component1.groupVariations.Add(component2);
        }
        this._groupsToCreate.Add(component1);
      }
    }
  }

  public static int HardCodedBusOptions => 3;

  public List<DynamicSoundGroup> GroupsToCreate => this._groupsToCreate;

  public bool ShouldShowUnityAudioMixerGroupAssignments => this.showUnityMixerGroupAssignment;

  public enum CreateItemsWhen
  {
    FirstEnableOnly,
    EveryEnable,
  }
}
