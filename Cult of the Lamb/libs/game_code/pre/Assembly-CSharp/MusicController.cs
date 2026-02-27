// Decompiled with JetBrains decompiler
// Type: MusicController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class MusicController : BaseMonoBehaviour
{
  private static MusicController Instance;
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

  public static void Play(AudioClip audioClip)
  {
    if ((Object) MusicController.Instance == (Object) null)
      MusicController.Instance = (Object.Instantiate(Resources.Load("MMAudio/Music Controller")) as GameObject).GetComponent<MusicController>();
    MusicController.Instance.play(audioClip);
  }

  private void play(AudioClip audioClip)
  {
    MusicController.Instance.audioSource.clip = audioClip;
    MusicController.Instance.audioSource.loop = true;
    MusicController.Instance.audioSource.Play();
  }

  public static void FadeOutAndStop() => MusicController.Instance.fadeOutAndStop();

  private void fadeOutAndStop()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DoFadeOutAndStop());
  }

  private IEnumerator DoFadeOutAndStop()
  {
    while ((double) (this.audioSource.volume -= Time.deltaTime) > 0.0)
      yield return (object) null;
    this.audioSource.volume = 0.0f;
    this.audioSource.Stop();
  }
}
