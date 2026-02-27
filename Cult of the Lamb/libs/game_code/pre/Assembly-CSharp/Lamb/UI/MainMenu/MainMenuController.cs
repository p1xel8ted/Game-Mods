// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MainMenu.MainMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using MMTools;
using src.UINavigator;
using System.Collections;
using Unify;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.MainMenu;

public class MainMenuController : MonoBehaviour
{
  public Animator StartTextAnimator;
  public Image RedFlash;
  private bool EngagementStarted;
  private const string kIntroStateName = "Intro";
  [SerializeField]
  private Animator _animator;
  [FormerlySerializedAs("_mainMenu")]
  [SerializeField]
  private UIMainMenuController _uiMainMenu;
  private Coroutine cIntroSequence;
  private Coroutine cAttractMode;
  public EventInstance loopedSound;

  private void Start()
  {
    AudioManager.Instance.StopCurrentAtmos();
    AudioManager.Instance.PauseActiveLoops();
    AudioManager.Instance.PlayMusic("event:/music/menu/menu_title");
    if (this.cIntroSequence != null)
      this.StopCoroutine(this.cIntroSequence);
    if (!CheatConsole.ForceAutoAttractMode)
      this.cIntroSequence = this.StartCoroutine((IEnumerator) this.DoIntroSequence());
    if (CheatConsole.IN_DEMO)
    {
      CheatConsole.DemoBeginTime = 0.0f;
      this.AttractMode();
    }
    KeyboardLightingManager.PulseAllKeys(new Color(0.7f, 0.65f, 0.1f, 1f), new Color(1f, 0.7f, 0.4f, 1f), 1f, new KeyCode[0]);
  }

  public void DoIntro() => this.StartCoroutine((IEnumerator) this.DoIntroSequence());

  public void AttractMode()
  {
    if (this.cAttractMode != null)
      this.StopCoroutine(this.cAttractMode);
    this.cAttractMode = this.StartCoroutine((IEnumerator) this.DoAttactMode());
  }

  private IEnumerator DoAttactMode()
  {
    MainMenuController mainMenuController = this;
    CheatConsole.ForceResetTimeSinceLastKeyPress();
    Debug.Log((object) ("CheatConsole.ForceAutoAttractMode: " + CheatConsole.ForceAutoAttractMode.ToString()));
    bool Waiting = !CheatConsole.ForceAutoAttractMode;
    while (Waiting)
    {
      Debug.Log((object) ("CheatConsole.TimeSinceLastKeyPress : " + (object) CheatConsole.TimeSinceLastKeyPress));
      if ((double) CheatConsole.TimeSinceLastKeyPress > 20.0)
        Waiting = false;
      yield return (object) null;
    }
    Debug.Log((object) "SKIP THE WAIT?!");
    if (CheatConsole.ForceAutoAttractMode)
    {
      while (MMTransition.IsPlaying)
        yield return (object) null;
    }
    CheatConsole.ForceAutoAttractMode = false;
    if (mainMenuController.cIntroSequence != null)
      mainMenuController.StopCoroutine(mainMenuController.cIntroSequence);
    MonoSingleton<UINavigatorNew>.Instance.LockInput = MonoSingleton<UINavigatorNew>.Instance.LockNavigation = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 1f, "", new System.Action(mainMenuController.PlayVideo));
  }

  private void PlayVideo()
  {
    AudioManager.Instance.StopCurrentAtmos();
    AudioManager.Instance.StopCurrentMusic();
    AudioManager.Instance.PauseActiveLoops();
    MMVideoPlayer.Play("Trailer", new System.Action(this.VideoComplete), MMVideoPlayer.Options.ENABLE, MMVideoPlayer.Options.DISABLE, false);
    this.loopedSound = AudioManager.Instance.CreateLoop("event:/music/trailer/trailer_video", true);
    MMTransition.ResumePlay();
  }

  private void VideoComplete()
  {
    MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Main Menu", 1f, "", (System.Action) (() =>
    {
      AudioManager.Instance.StopLoop(this.loopedSound);
      MMVideoPlayer.Hide();
      Debug.Log((object) "VIDEO COMPLETE!");
    }));
  }

  private IEnumerator DoIntroSequence()
  {
    MainMenuController coroutineSupport = this;
    while (true)
    {
      if (!coroutineSupport.EngagementStarted && SessionManager.instance.State == SessionManager.SessionState.Loading)
      {
        KeyboardLightingManager.TransitionAllKeys(Color.red, new Color(0.7f, 0.65f, 0.1f, 1f), 2f, Ease.OutBounce);
        coroutineSupport.EngagementStarted = true;
        coroutineSupport.StartTextAnimator.SetTrigger("Active");
        coroutineSupport.RedFlash.color = new Color(coroutineSupport.RedFlash.color.r, coroutineSupport.RedFlash.color.g, coroutineSupport.RedFlash.color.b, 1f);
        DOTweenModuleUI.DOFade(coroutineSupport.RedFlash, 0.0f, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        UIManager.PlayAudio("event:/ui/Sermon Speech Bubble");
        UIManager.PlayAudio("event:/sermon/select_upgrade");
      }
      if (!SessionManager.instance.HasStarted)
        yield return (object) null;
      else
        break;
    }
    if (UnifyManager.platform != UnifyManager.Platform.Standalone)
      Singleton<SettingsManager>.Instance.LoadAndApply();
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact, alsoRumble: true, coroutineSupport: (MonoBehaviour) coroutineSupport);
    UIManager.PlayAudio("event:/ui/start_game");
    coroutineSupport.StartTextAnimator.SetTrigger("End");
    coroutineSupport._uiMainMenu.MainMenu.Show(true);
    coroutineSupport.StartCoroutine((IEnumerator) coroutineSupport._uiMainMenu.PreloadMetadata());
    MonoSingleton<UINavigatorNew>.Instance.LockInput = MonoSingleton<UINavigatorNew>.Instance.LockNavigation = true;
    yield return (object) coroutineSupport._animator.YieldForAnimation("Intro");
    if (CheatConsole.IN_DEMO)
    {
      CheatConsole.DemoBeginTime = 0.0f;
      coroutineSupport.AttractMode();
    }
  }

  public void StartInput()
  {
    MonoSingleton<UINavigatorNew>.Instance.LockInput = MonoSingleton<UINavigatorNew>.Instance.LockNavigation = false;
  }
}
