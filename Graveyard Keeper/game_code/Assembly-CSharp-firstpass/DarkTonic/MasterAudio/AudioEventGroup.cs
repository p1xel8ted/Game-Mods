// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.AudioEventGroup
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace DarkTonic.MasterAudio;

[Serializable]
public class AudioEventGroup
{
  public bool isExpanded = true;
  public bool allPlayersShouldHear;
  public bool useLayerFilter;
  public bool useTagFilter;
  public List<int> matchingLayers = new List<int>() { 0 };
  public List<string> matchingTags = new List<string>()
  {
    "Default"
  };
  public bool customSoundActive;
  public bool isCustomEvent;
  public string customEventName = string.Empty;
  public bool mechanimEventActive;
  public bool isMechanimStateCheckEvent;
  public string mechanimStateName = string.Empty;
  public bool mechEventPlayedForState;
  public List<AudioEvent> SoundEvents = new List<AudioEvent>();
  public EventSounds.PreviousSoundStopMode mouseDragStopMode;
  public float mouseDragFadeOutTime = 1f;
  public EventSounds.RetriggerLimMode retriggerLimitMode;
  public int limitPerXFrm;
  public float limitPerXSec;
  public int triggeredLastFrame = -100;
  public float triggeredLastTime = -100f;
  public float triggerStayForTime;
  public bool doesTriggerStayRepeat = true;
  public float sliderValue;
}
