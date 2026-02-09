// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.DynamicSoundGroup
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

public class DynamicSoundGroup : MonoBehaviour
{
  public GameObject variationTemplate;
  public bool alwaysHighestPriority;
  public float groupMasterVolume = 1f;
  public int retriggerPercentage = 50;
  public MasterAudioGroup.VariationSequence curVariationSequence;
  public bool useNoRepeatRefill = true;
  public bool useInactivePeriodPoolRefill;
  public float inactivePeriodSeconds = 5f;
  public MasterAudioGroup.VariationMode curVariationMode;
  public DarkTonic.MasterAudio.MasterAudio.AudioLocation bulkVariationMode;
  public float chainLoopDelayMin;
  public float chainLoopDelayMax;
  public MasterAudioGroup.ChainedLoopLoopMode chainLoopMode;
  public int chainLoopNumLoops;
  public bool useDialogFadeOut;
  public float dialogFadeOutTime = 0.5f;
  public bool resourceClipsAllLoadAsync = true;
  public bool logSound;
  public bool soundPlayedEventActive;
  public string soundPlayedCustomEvent = string.Empty;
  public int busIndex = -1;
  public DarkTonic.MasterAudio.MasterAudio.ItemSpatialBlendType spatialBlendType = DarkTonic.MasterAudio.MasterAudio.ItemSpatialBlendType.ForceTo3D;
  public float spatialBlend = 1f;
  public string busName = string.Empty;
  public bool isExistingBus;
  public bool isCopiedFromDGSC;
  public MasterAudioGroup.LimitMode limitMode;
  public int limitPerXFrames = 1;
  public float minimumTimeBetween = 0.1f;
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
  public bool copySettingsExpanded;
  public int selectedVariationIndex;
  public bool expandLinkedGroups;
  public List<string> childSoundGroups = new List<string>();
  public List<string> endLinkedGroups = new List<string>();
  public DarkTonic.MasterAudio.MasterAudio.LinkedGroupSelectionType linkedStartGroupSelectionType;
  public DarkTonic.MasterAudio.MasterAudio.LinkedGroupSelectionType linkedStopGroupSelectionType;
  public List<DynamicGroupVariation> groupVariations = new List<DynamicGroupVariation>();
}
