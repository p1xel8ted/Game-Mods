// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.DynamicGroupVariation
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

public class DynamicGroupVariation : MonoBehaviour
{
  [Range(0.0f, 1f)]
  public int probabilityToPlay = 100;
  public bool useLocalization;
  public bool useRandomPitch;
  public SoundGroupVariation.RandomPitchMode randomPitchMode;
  public float randomPitchMin;
  public float randomPitchMax;
  public bool useRandomVolume;
  public SoundGroupVariation.RandomVolumeMode randomVolumeMode;
  public float randomVolumeMin;
  public float randomVolumeMax;
  public int weight = 1;
  public DarkTonic.MasterAudio.MasterAudio.AudioLocation audLocation;
  public string resourceFileName;
  public string internetFileUrl;
  public bool isExpanded = true;
  public bool isChecked = true;
  public bool useFades;
  public float fadeInTime;
  public float fadeOutTime;
  public bool useCustomLooping;
  public int minCustomLoops = 1;
  public int maxCustomLoops = 5;
  public bool useIntroSilence;
  public float introSilenceMin;
  public float introSilenceMax;
  public bool useRandomStartTime;
  public float randomStartMinPercent;
  public float randomStartMaxPercent = 100f;
  public float randomEndPercent = 100f;
  public AudioDistortionFilter _distFilter;
  public AudioEchoFilter _echoFilter;
  public AudioHighPassFilter _hpFilter;
  public AudioLowPassFilter _lpFilter;
  public AudioReverbFilter _reverbFilter;
  public AudioChorusFilter _chorusFilter;
  public DynamicSoundGroup _parentGroupScript;
  public Transform _trans;
  public AudioSource _aud;

  public AudioDistortionFilter DistortionFilter
  {
    get
    {
      if ((Object) this._distFilter != (Object) null)
        return this._distFilter;
      this._distFilter = this.GetComponent<AudioDistortionFilter>();
      return this._distFilter;
    }
  }

  public AudioReverbFilter ReverbFilter
  {
    get
    {
      if ((Object) this._reverbFilter != (Object) null)
        return this._reverbFilter;
      this._reverbFilter = this.GetComponent<AudioReverbFilter>();
      return this._reverbFilter;
    }
  }

  public AudioChorusFilter ChorusFilter
  {
    get
    {
      if ((Object) this._chorusFilter != (Object) null)
        return this._chorusFilter;
      this._chorusFilter = this.GetComponent<AudioChorusFilter>();
      return this._chorusFilter;
    }
  }

  public AudioEchoFilter EchoFilter
  {
    get
    {
      if ((Object) this._echoFilter != (Object) null)
        return this._echoFilter;
      this._echoFilter = this.GetComponent<AudioEchoFilter>();
      return this._echoFilter;
    }
  }

  public AudioLowPassFilter LowPassFilter
  {
    get
    {
      if ((Object) this._lpFilter != (Object) null)
        return this._lpFilter;
      this._lpFilter = this.GetComponent<AudioLowPassFilter>();
      return this._lpFilter;
    }
  }

  public AudioHighPassFilter HighPassFilter
  {
    get
    {
      if ((Object) this._hpFilter != (Object) null)
        return this._hpFilter;
      this._hpFilter = this.GetComponent<AudioHighPassFilter>();
      return this._hpFilter;
    }
  }

  public DynamicSoundGroup ParentGroup
  {
    get
    {
      if ((Object) this._parentGroupScript == (Object) null)
        this._parentGroupScript = this.Trans.parent.GetComponent<DynamicSoundGroup>();
      if ((Object) this._parentGroupScript == (Object) null)
        Debug.LogError((object) $"The Group that Dynamic Sound Variation '{this.name}' is in does not have a DynamicSoundGroup script in it!");
      return this._parentGroupScript;
    }
  }

  public Transform Trans
  {
    get
    {
      if ((Object) this._trans != (Object) null)
        return this._trans;
      this._trans = this.transform;
      return this._trans;
    }
  }

  public bool HasActiveFXFilter
  {
    get
    {
      return (Object) this.HighPassFilter != (Object) null && this.HighPassFilter.enabled || (Object) this.LowPassFilter != (Object) null && this.LowPassFilter.enabled || (Object) this.ReverbFilter != (Object) null && this.ReverbFilter.enabled || (Object) this.DistortionFilter != (Object) null && this.DistortionFilter.enabled || (Object) this.EchoFilter != (Object) null && this.EchoFilter.enabled || (Object) this.ChorusFilter != (Object) null && this.ChorusFilter.enabled;
    }
  }

  public AudioSource VarAudio
  {
    get
    {
      if ((Object) this._aud != (Object) null)
        return this._aud;
      this._aud = this.GetComponent<AudioSource>();
      return this._aud;
    }
  }
}
