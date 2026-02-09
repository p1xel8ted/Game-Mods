// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.MusicSetting
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

[Serializable]
public class MusicSetting
{
  public string alias = string.Empty;
  public DarkTonic.MasterAudio.MasterAudio.AudioLocation audLocation;
  public AudioClip clip;
  public string songName = string.Empty;
  public string resourceFileName = string.Empty;
  public float volume = 1f;
  public float pitch = 1f;
  public bool isExpanded = true;
  public bool isLoop;
  public DarkTonic.MasterAudio.MasterAudio.CustomSongStartTimeMode songStartTimeMode;
  public float customStartTime;
  public float customStartTimeMax;
  public int lastKnownTimePoint;
  public bool wasLastKnownTimePointSet;
  public int songIndex;
  public bool songStartedEventExpanded;
  public string songStartedCustomEvent = string.Empty;
  public bool songChangedEventExpanded;
  public string songChangedCustomEvent = string.Empty;

  public MusicSetting() => this.songChangedEventExpanded = false;

  public float SongStartTime
  {
    get
    {
      switch (this.songStartTimeMode)
      {
        case DarkTonic.MasterAudio.MasterAudio.CustomSongStartTimeMode.SpecificTime:
          return this.customStartTime;
        case DarkTonic.MasterAudio.MasterAudio.CustomSongStartTimeMode.RandomTime:
          return UnityEngine.Random.Range(this.customStartTime, this.customStartTimeMax);
        default:
          return 0.0f;
      }
    }
  }

  public static MusicSetting Clone(MusicSetting mus)
  {
    return new MusicSetting()
    {
      alias = mus.alias,
      audLocation = mus.audLocation,
      clip = mus.clip,
      songName = mus.songName,
      resourceFileName = mus.resourceFileName,
      volume = mus.volume,
      pitch = mus.pitch,
      isExpanded = mus.isExpanded,
      isLoop = mus.isLoop,
      customStartTime = mus.customStartTime,
      songStartedEventExpanded = mus.songStartedEventExpanded,
      songStartedCustomEvent = mus.songStartedCustomEvent,
      songChangedEventExpanded = mus.songChangedEventExpanded,
      songChangedCustomEvent = mus.songChangedCustomEvent
    };
  }
}
