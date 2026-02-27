// Decompiled with JetBrains decompiler
// Type: GameOverScreenController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private float _UnscaledTime;
  public string QuitToScene = "Main Menu";
  public Animator animator;

  private void Start()
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

  private void Clear()
  {
    Time.timeScale = 1f;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
