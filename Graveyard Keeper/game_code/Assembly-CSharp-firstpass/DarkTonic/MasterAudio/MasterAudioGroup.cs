// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.MasterAudioGroup
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

public class MasterAudioGroup : MonoBehaviour
{
  public const float UseCurveSpatialBlend = -99f;
  public const string NoBus = "[NO BUS]";
  public const int MinNoRepeatVariations = 3;
  public int busIndex = -1;
  public DarkTonic.MasterAudio.MasterAudio.ItemSpatialBlendType spatialBlendType = DarkTonic.MasterAudio.MasterAudio.ItemSpatialBlendType.ForceTo3D;
  public float spatialBlend = 1f;
  public bool isSelected;
  public bool isExpanded = true;
  public float groupMasterVolume = 1f;
  public int retriggerPercentage = 100;
  public MasterAudioGroup.VariationMode curVariationMode;
  public bool alwaysHighestPriority;
  public float chainLoopDelayMin;
  public float chainLoopDelayMax;
  public MasterAudioGroup.ChainedLoopLoopMode chainLoopMode;
  public int chainLoopNumLoops;
  public bool useDialogFadeOut;
  public float dialogFadeOutTime = 0.5f;
  public MasterAudioGroup.VariationSequence curVariationSequence;
  public bool useNoRepeatRefill = true;
  public bool useInactivePeriodPoolRefill;
  public float inactivePeriodSeconds = 5f;
  public List<SoundGroupVariation> groupVariations = new List<SoundGroupVariation>();
  public DarkTonic.MasterAudio.MasterAudio.AudioLocation bulkVariationMode;
  public bool resourceClipsAllLoadAsync = true;
  public bool logSound;
  public bool copySettingsExpanded;
  public int selectedVariationIndex;
  public bool expandLinkedGroups;
  public List<string> childSoundGroups = new List<string>();
  public List<string> endLinkedGroups = new List<string>();
  public DarkTonic.MasterAudio.MasterAudio.LinkedGroupSelectionType linkedStartGroupSelectionType;
  public DarkTonic.MasterAudio.MasterAudio.LinkedGroupSelectionType linkedStopGroupSelectionType;
  public MasterAudioGroup.LimitMode limitMode;
  public int limitPerXFrames = 1;
  public float minimumTimeBetween = 0.1f;
  public bool useClipAgePriority;
  public bool limitPolyphony;
  public int voiceLimitCount = 1;
  public MasterAudioGroup.TargetDespawnedBehavior targetDespawnedBehavior = MasterAudioGroup.TargetDespawnedBehavior.FadeOut;
  public float despawnFadeTime = 0.3f;
  public bool isUsingOcclusion;
  public bool willOcclusionOverrideRaycastOffset;
  public float occlusionRayCastOffset;
  public bool willOcclusionOverrideFrequencies;
  public float occlusionMaxCutoffFreq;
  public float occlusionMinCutoffFreq = 22000f;
  public bool isSoloed;
  public bool isMuted;
  public bool soundPlayedEventActive;
  public string soundPlayedCustomEvent = string.Empty;
  public bool willCleanUpDelegatesAfterStop = true;
  public int frames;
  public List<int> _activeAudioSourcesIds;
  public string _objectName = string.Empty;
  public Transform _trans;
  public float _originalVolume = 1f;
  [CompilerGenerated]
  public int \u003CChainLoopCount\u003Ek__BackingField;

  public event Action LastVariationFinishedPlay;

  public DarkTonic.MasterAudio.MasterAudio.InternetFileLoadStatus GroupLoadStatus
  {
    get
    {
      DarkTonic.MasterAudio.MasterAudio.InternetFileLoadStatus groupLoadStatus = DarkTonic.MasterAudio.MasterAudio.InternetFileLoadStatus.Loaded;
      for (int index = 0; index < this.Trans.childCount; ++index)
      {
        SoundGroupVariation component = this.Trans.GetChild(index).GetComponent<SoundGroupVariation>();
        if (component.audLocation == DarkTonic.MasterAudio.MasterAudio.AudioLocation.FileOnInternet)
        {
          if (component.internetFileLoadStatus == DarkTonic.MasterAudio.MasterAudio.InternetFileLoadStatus.Failed)
          {
            groupLoadStatus = DarkTonic.MasterAudio.MasterAudio.InternetFileLoadStatus.Failed;
            break;
          }
          if (component.internetFileLoadStatus == DarkTonic.MasterAudio.MasterAudio.InternetFileLoadStatus.Loading)
            groupLoadStatus = DarkTonic.MasterAudio.MasterAudio.InternetFileLoadStatus.Loading;
        }
      }
      return groupLoadStatus;
    }
  }

  public void Start()
  {
    this._objectName = this.name;
    int count = this.ActiveAudioSourceIds.Count;
    bool flag = false;
    if ((UnityEngine.Object) this.Trans.parent != (UnityEngine.Object) null)
      this.gameObject.layer = this.Trans.parent.gameObject.layer;
    for (int index = 0; index < this.Trans.childCount; ++index)
    {
      SoundGroupVariation component = this.Trans.GetChild(index).GetComponent<SoundGroupVariation>();
      if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && !((UnityEngine.Object) component.GetComponent<SoundGroupVariationUpdater>() != (UnityEngine.Object) null))
      {
        flag = true;
        break;
      }
    }
    if (!flag)
      return;
    Debug.LogError((object) $"One or more Variations of Sound Group '{this.GameObjectName}' do not have the SoundGroupVariationUpdater component and will not function properly. Please stop and fix this by opening the Master Audio Manager window and clicking the Upgrade MA Prefab button before continuing.");
  }

  public void OnDisable()
  {
    for (int index = 0; index < this.Trans.childCount; ++index)
    {
      SoundGroupVariation component = this.Trans.GetChild(index).GetComponent<SoundGroupVariation>();
      if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && component.audLocation == DarkTonic.MasterAudio.MasterAudio.AudioLocation.FileOnInternet)
        AudioResourceOptimizer.RemoveLoadedInternetClip(component.internetFileUrl);
    }
  }

  public void AddActiveAudioSourceId(int varInstanceId)
  {
    if (this.ActiveAudioSourceIds.Contains(varInstanceId))
      return;
    this.ActiveAudioSourceIds.Add(varInstanceId);
    this.BusForGroup?.AddActiveAudioSourceId(varInstanceId);
  }

  public void RemoveActiveAudioSourceId(int varInstanceId)
  {
    this.ActiveAudioSourceIds.Remove(varInstanceId);
    this.BusForGroup?.RemoveActiveAudioSourceId(varInstanceId);
  }

  public float SpatialBlendForGroup
  {
    get
    {
      switch (DarkTonic.MasterAudio.MasterAudio.Instance.mixerSpatialBlendType)
      {
        case DarkTonic.MasterAudio.MasterAudio.AllMixerSpatialBlendType.ForceAllTo2D:
          return 0.0f;
        case DarkTonic.MasterAudio.MasterAudio.AllMixerSpatialBlendType.ForceAllTo3D:
          return 1f;
        case DarkTonic.MasterAudio.MasterAudio.AllMixerSpatialBlendType.ForceAllToCustom:
          return DarkTonic.MasterAudio.MasterAudio.Instance.mixerSpatialBlend;
        default:
          switch (this.spatialBlendType)
          {
            case DarkTonic.MasterAudio.MasterAudio.ItemSpatialBlendType.ForceTo2D:
              return 0.0f;
            case DarkTonic.MasterAudio.MasterAudio.ItemSpatialBlendType.ForceTo3D:
              return 1f;
            case DarkTonic.MasterAudio.MasterAudio.ItemSpatialBlendType.ForceToCustom:
              return this.spatialBlend;
            default:
              return -99f;
          }
      }
    }
  }

  public int ActiveVoices => this.ActiveAudioSourceIds.Count;

  public int TotalVoices => this.transform.childCount;

  public bool WillCleanUpDelegatesAfterStop
  {
    set => this.willCleanUpDelegatesAfterStop = value;
  }

  public GroupBus BusForGroup
  {
    get
    {
      if (this.busIndex < 2)
        return (GroupBus) null;
      int index = this.busIndex - 2;
      return index >= DarkTonic.MasterAudio.MasterAudio.GroupBuses.Count ? (GroupBus) null : DarkTonic.MasterAudio.MasterAudio.GroupBuses[index];
    }
  }

  public float OriginalVolume
  {
    get => this._originalVolume;
    set => this._originalVolume = value;
  }

  public bool LoggingEnabledForGroup => this.logSound || DarkTonic.MasterAudio.MasterAudio.LogSoundsEnabled;

  public void FireLastVariationFinishedPlay()
  {
    if (this.LastVariationFinishedPlay == null)
      return;
    this.LastVariationFinishedPlay();
  }

  public void SubscribeToLastVariationFinishedPlay(Action finishedCallback)
  {
    this.LastVariationFinishedPlay = (Action) null;
    this.LastVariationFinishedPlay += finishedCallback;
  }

  public void UnsubscribeFromLastVariationFinishedPlay()
  {
    this.LastVariationFinishedPlay = (Action) null;
  }

  public int ChainLoopCount
  {
    get => this.\u003CChainLoopCount\u003Ek__BackingField;
    set => this.\u003CChainLoopCount\u003Ek__BackingField = value;
  }

  public string GameObjectName
  {
    get
    {
      if (string.IsNullOrEmpty(this._objectName))
        this._objectName = this.name;
      return this._objectName;
    }
  }

  public bool UsesNoRepeat
  {
    get
    {
      return this.curVariationSequence == MasterAudioGroup.VariationSequence.Randomized && this.groupVariations.Count >= 3 && this.useNoRepeatRefill;
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

  public List<int> ActiveAudioSourceIds
  {
    get
    {
      if (this._activeAudioSourcesIds != null)
        return this._activeAudioSourcesIds;
      this._activeAudioSourcesIds = new List<int>(this.Trans.childCount);
      return this._activeAudioSourcesIds;
    }
  }

  public enum TargetDespawnedBehavior
  {
    None,
    Stop,
    FadeOut,
  }

  public enum VariationSequence
  {
    Randomized,
    TopToBottom,
  }

  public enum VariationMode
  {
    Normal,
    LoopedChain,
    Dialog,
  }

  public enum ChainedLoopLoopMode
  {
    Endless,
    NumberOfLoops,
  }

  public enum LimitMode
  {
    None,
    FrameBased,
    TimeBased,
  }
}
