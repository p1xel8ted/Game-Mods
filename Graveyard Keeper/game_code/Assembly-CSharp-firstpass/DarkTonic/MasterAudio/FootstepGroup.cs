// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.FootstepGroup
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace DarkTonic.MasterAudio;

[Serializable]
public class FootstepGroup
{
  public bool isExpanded = true;
  public bool useLayerFilter;
  public bool useTagFilter;
  public List<int> matchingLayers = new List<int>() { 0 };
  public List<string> matchingTags = new List<string>()
  {
    "Default"
  };
  public string soundType = "[None]";
  public EventSounds.VariationType variationType = EventSounds.VariationType.PlayRandom;
  public string variationName = string.Empty;
  public float volume = 1f;
  public bool useFixedPitch;
  public float pitch = 1f;
  public float delaySound;
}
