// Decompiled with JetBrains decompiler
// Type: GameOverScreenController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using UnityEngine;

#nullable disable
public class GameOverScreenController : BaseMonoBehaviour
{
  public AudioClip Music;
  public AudioClip Quote;
  public float fadeIn = 1f;
  public AudioSource audioSource;
  public float _UnscaledTime;
  public string QuitToScene = "Main Menu";
  public Animator animator;

  public void Start()
  {
    this.animator.Play("EndOfGame");
    AmbientMusicController.PlayTrack(this.Music, this.fadeIn);
    Debug.Log((object) LocalizationManager.CurrentLanguage);
    if (LocalizationManager.CurrentLanguage == "English")
    {
      this.audioSource.clip = this.Quote;
      this.audioSource.Play();
    }
    MMTransition.ResumePlay();
  }

  public void Quit()
  {
    Time.timeScale = 1f;
    MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, this.QuitToScene, 2f, "", (System.Action) null);
  }

  public void Retry()
  {
    Time.timeScale = 1f;
    MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Base Biome 1", 2f, "", (System.Action) null);
  }

  public void Clear()
  {
    Time.timeScale = 1f;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
