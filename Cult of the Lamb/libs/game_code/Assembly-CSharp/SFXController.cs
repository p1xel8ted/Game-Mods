// Decompiled with JetBrains decompiler
// Type: SFXController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Audio;

#nullable disable
public class SFXController : BaseMonoBehaviour
{
  public static SFXController Instance;
  public AudioMixerGroup audioMixerGroup;
  public AudioSource _audioSource;

  public AudioSource audioSource
  {
    get
    {
      if ((Object) this._audioSource == (Object) null)
        this._audioSource = this.GetComponent<AudioSource>();
      return this._audioSource;
    }
  }

  public void Start() => Object.DontDestroyOnLoad((Object) this.gameObject);

  public static void Play(AudioClip audioClip)
  {
    if ((Object) SFXController.Instance == (Object) null)
      SFXController.Instance = Object.Instantiate<GameObject>(Resources.Load("MMAudio/SFX Controller") as GameObject).GetComponent<SFXController>();
    SFXController.Instance.audioSource.clip = audioClip;
    SFXController.Instance.audioSource.outputAudioMixerGroup = SFXController.Instance.audioMixerGroup;
    SFXController.Instance.audioSource.PlayOneShot(audioClip);
  }

  public static void Play(AudioClip audioClip, float pitch)
  {
    if ((Object) SFXController.Instance == (Object) null)
      SFXController.Instance = Object.Instantiate<GameObject>(Resources.Load("MMAudio/SFX Controller") as GameObject).GetComponent<SFXController>();
    SFXController.Instance.audioSource.clip = audioClip;
    SFXController.Instance.audioSource.pitch = pitch;
    SFXController.Instance.audioSource.outputAudioMixerGroup = SFXController.Instance.audioMixerGroup;
    SFXController.Instance.audioSource.PlayOneShot(audioClip);
  }

  public static void Play(AudioClip audioClip, float pitch, float volume)
  {
    if ((Object) SFXController.Instance == (Object) null)
      SFXController.Instance = Object.Instantiate<GameObject>(Resources.Load("MMAudio/SFX Controller") as GameObject).GetComponent<SFXController>();
    SFXController.Instance.audioSource.clip = audioClip;
    SFXController.Instance.audioSource.pitch = pitch;
    SFXController.Instance.audioSource.volume = volume;
    SFXController.Instance.audioSource.outputAudioMixerGroup = SFXController.Instance.audioMixerGroup;
    SFXController.Instance.audioSource.PlayOneShot(audioClip);
  }
}
