// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.AudioEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine.Audio;

#nullable disable
namespace DarkTonic.MasterAudio;

[Serializable]
public class AudioEvent
{
  public string actionName = "Your action name";
  public bool isExpanded = true;
  public string soundType = "[None]";
  public bool allPlaylistControllersForGroupCmd;
  public bool allSoundTypesForGroupCmd;
  public bool allSoundTypesForBusCmd;
  public float volume = 1f;
  public bool useFixedPitch;
  public float pitch = 1f;
  public EventSounds.GlidePitchType glidePitchType;
  public float targetGlidePitch = 1f;
  public float pitchGlideTime = 1f;
  public float delaySound;
  public DarkTonic.MasterAudio.MasterAudio.EventSoundFunctionType currentSoundFunctionType;
  public DarkTonic.MasterAudio.MasterAudio.PlaylistCommand currentPlaylistCommand;
  public DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand currentSoundGroupCommand;
  public DarkTonic.MasterAudio.MasterAudio.BusCommand currentBusCommand;
  public DarkTonic.MasterAudio.MasterAudio.CustomEventCommand currentCustomEventCommand;
  public DarkTonic.MasterAudio.MasterAudio.GlobalCommand currentGlobalCommand;
  public DarkTonic.MasterAudio.MasterAudio.UnityMixerCommand currentMixerCommand;
  public AudioMixerSnapshot snapshotToTransitionTo;
  public float snapshotTransitionTime = 1f;
  public List<AudioEvent.MA_SnapshotInfo> snapshotsToBlend = new List<AudioEvent.MA_SnapshotInfo>()
  {
    new AudioEvent.MA_SnapshotInfo((AudioMixerSnapshot) null, 1f)
  };
  public DarkTonic.MasterAudio.MasterAudio.PersistentSettingsCommand currentPersistentSettingsCommand;
  public string busName = string.Empty;
  public string playlistName = string.Empty;
  public string playlistControllerName = string.Empty;
  public bool startPlaylist = true;
  public float fadeVolume;
  public float fadeTime = 1f;
  public bool stopAfterFade;
  public bool restoreVolumeAfterFade;
  public AudioEvent.TargetVolumeMode targetVolMode;
  public string clipName = "[None]";
  public EventSounds.VariationType variationType = EventSounds.VariationType.PlayRandom;
  public string variationName = string.Empty;
  public string theCustomEventName = string.Empty;

  public bool IsFadeCommand
  {
    get
    {
      return this.currentSoundFunctionType == DarkTonic.MasterAudio.MasterAudio.EventSoundFunctionType.PlaylistControl && this.currentPlaylistCommand == DarkTonic.MasterAudio.MasterAudio.PlaylistCommand.FadeToVolume || this.currentSoundFunctionType == DarkTonic.MasterAudio.MasterAudio.EventSoundFunctionType.BusControl && this.currentBusCommand == DarkTonic.MasterAudio.MasterAudio.BusCommand.FadeToVolume || this.currentSoundFunctionType == DarkTonic.MasterAudio.MasterAudio.EventSoundFunctionType.GroupControl && (this.currentSoundGroupCommand == DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.FadeToVolume || this.currentSoundGroupCommand == DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.FadeOutAllOfSound || this.currentSoundGroupCommand == DarkTonic.MasterAudio.MasterAudio.SoundGroupCommand.FadeOutSoundGroupOfTransform);
    }
  }

  public enum TargetVolumeMode
  {
    UseSliderValue,
    UseSpecificValue,
  }

  [Serializable]
  public class MA_SnapshotInfo
  {
    public AudioMixerSnapshot snapshot;
    public float weight;

    public MA_SnapshotInfo(AudioMixerSnapshot snap, float wt)
    {
      this.snapshot = snap;
      this.weight = wt;
    }
  }
}
