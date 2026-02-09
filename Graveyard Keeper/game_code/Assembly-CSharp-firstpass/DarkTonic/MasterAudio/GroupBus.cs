// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.GroupBus
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine.Audio;

#nullable disable
namespace DarkTonic.MasterAudio;

[Serializable]
public class GroupBus
{
  public string busName;
  public float volume = 1f;
  public bool isSoloed;
  public bool isMuted;
  public int voiceLimit = -1;
  public bool stopOldest;
  public bool isExisting;
  public bool isTemporary;
  public bool isUsingOcclusion;
  public AudioMixerGroup mixerChannel;
  public bool forceTo2D;
  public List<int> _activeAudioSourcesIds = new List<int>(50);
  public float _originalVolume = 1f;

  public void AddActiveAudioSourceId(int id)
  {
    if (this._activeAudioSourcesIds.Contains(id))
      return;
    this._activeAudioSourcesIds.Add(id);
  }

  public void RemoveActiveAudioSourceId(int id) => this._activeAudioSourcesIds.Remove(id);

  public int ActiveVoices => this._activeAudioSourcesIds.Count;

  public bool BusVoiceLimitReached
  {
    get => this.voiceLimit > 0 && this._activeAudioSourcesIds.Count >= this.voiceLimit;
  }

  public float OriginalVolume
  {
    get => this._originalVolume;
    set => this._originalVolume = value;
  }
}
