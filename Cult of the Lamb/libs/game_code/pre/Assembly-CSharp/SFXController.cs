// Decompiled with JetBrains decompiler
// Type: SFXController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Audio;

#nullable disable
public class SFXController : BaseMonoBehaviour
{
  private static SFXController Instance;
  public AudioMixerGroup audioMixerGroup;
  private AudioSource _audioSource;

  private AudioSource audioSource
  {
    get
    {
      if ((Object) this._audioSource == (Object) null)
        this._audioSource = this.GetComponent<AudioSource>();
      return this._audioSource;
    }
  }

  private void Start() => Object.DontDestroyOnLoad((Object) this.gameObject);

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
