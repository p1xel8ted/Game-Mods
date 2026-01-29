// Decompiled with JetBrains decompiler
// Type: MusicController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class MusicController : BaseMonoBehaviour
{
  public static MusicController Instance;
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

  public static void Play(AudioClip audioClip)
  {
    if ((Object) MusicController.Instance == (Object) null)
      MusicController.Instance = (Object.Instantiate(Resources.Load("MMAudio/Music Controller")) as GameObject).GetComponent<MusicController>();
    MusicController.Instance.play(audioClip);
  }

  public void play(AudioClip audioClip)
  {
    MusicController.Instance.audioSource.clip = audioClip;
    MusicController.Instance.audioSource.loop = true;
    MusicController.Instance.audioSource.Play();
  }

  public static void FadeOutAndStop() => MusicController.Instance.fadeOutAndStop();

  public void fadeOutAndStop()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DoFadeOutAndStop());
  }

  public IEnumerator DoFadeOutAndStop()
  {
    while ((double) (this.audioSource.volume -= Time.deltaTime) > 0.0)
      yield return (object) null;
    this.audioSource.volume = 0.0f;
    this.audioSource.Stop();
  }
}
