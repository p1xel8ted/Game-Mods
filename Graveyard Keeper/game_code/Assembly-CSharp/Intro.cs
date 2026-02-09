// Decompiled with JetBrains decompiler
// Type: Intro
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Intro : MonoBehaviour
{
  public TitleScreenCamera intro_camera;
  public UILabel subtitle_text;
  public Animator animator;
  public static Intro _me;
  public System.Action _on_finished;
  public static bool need_show_first_intro;

  public void ShowSubtitleText(string lng_id)
  {
    GJL.EnsureLabelHasCorrectFont(this.subtitle_text, false);
    this.subtitle_text.text = GJL.L(lng_id);
    this.subtitle_text.gameObject.SetActive(true);
    this.subtitle_text.GetComponent<TypewriterEffect>().enabled = false;
    this.subtitle_text.alpha = 0.0f;
    DOTween.To((DOGetter<float>) (() => this.subtitle_text.alpha), (DOSetter<float>) (v => this.subtitle_text.alpha = v), 1f, 0.3f);
  }

  public void SubtitleTextDisappear()
  {
    DOTween.To((DOGetter<float>) (() => this.subtitle_text.alpha), (DOSetter<float>) (v => this.subtitle_text.alpha = v), 0.0f, 1f);
  }

  public void Awake()
  {
    Debug.Log((object) "Intro.Awake", (UnityEngine.Object) this);
    if ((UnityEngine.Object) MainGame.me != (UnityEngine.Object) null)
    {
      Debug.Log((object) "Loaded from the MainGame");
      this.gameObject.SetActive(false);
      UnityEngine.Object.Destroy((UnityEngine.Object) this.intro_camera.GetComponent<AudioListener>());
    }
    Intro._me = this;
  }

  public static void ShowIntro(System.Action on_finished, bool no_words = false, bool stop_all_playlist = true)
  {
    if (!Intro.need_show_first_intro)
    {
      on_finished.TryInvoke();
    }
    else
    {
      if ((UnityEngine.Object) Intro._me == (UnityEngine.Object) null)
      {
        Intro._me = UnityEngine.Object.FindObjectOfType<Intro>();
        if ((UnityEngine.Object) Intro._me == (UnityEngine.Object) null)
        {
          Debug.Log((object) "ShowIntro, me is null");
          on_finished.TryInvoke();
          return;
        }
      }
      if (stop_all_playlist)
        DarkTonic.MasterAudio.MasterAudio.StopAllPlaylists();
      Intro._me._on_finished = on_finished;
      Intro._me.gameObject.SetActive(true);
      if (no_words)
        Intro._me.animator.SetTrigger("start_no_words");
      else
        Intro._me.animator.SetTrigger("start");
      SmartAudioEngine.me.TransitionToSnapshot("DisabledSoundFXLowPass");
    }
  }

  public static void OnIntroAnimationFinished()
  {
    Debug.Log((object) nameof (OnIntroAnimationFinished));
    if ((UnityEngine.Object) MainGame.me == (UnityEngine.Object) null)
      return;
    Intro._me.gameObject.SetActive(false);
    Intro._me._on_finished.TryInvoke();
  }

  [CompilerGenerated]
  public float \u003CShowSubtitleText\u003Eb__6_0() => this.subtitle_text.alpha;

  [CompilerGenerated]
  public void \u003CShowSubtitleText\u003Eb__6_1(float v) => this.subtitle_text.alpha = v;

  [CompilerGenerated]
  public float \u003CSubtitleTextDisappear\u003Eb__7_0() => this.subtitle_text.alpha;

  [CompilerGenerated]
  public void \u003CSubtitleTextDisappear\u003Eb__7_1(float v) => this.subtitle_text.alpha = v;
}
