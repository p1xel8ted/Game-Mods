// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MainMenu.MainMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Beffio.Dithering;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using MMTools;
using Spine.Unity;
using src.Managers;
using src.UINavigator;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
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
  public float glitchVolumeMultiplier = 1f;
  [CompilerGenerated]
  public static bool \u003CEngagementStarted\u003Ek__BackingField;
  public const string kIntroStateName = "Intro";
  [SerializeField]
  public Animator _animator;
  [FormerlySerializedAs("_mainMenu")]
  [SerializeField]
  public UIMainMenuController _uiMainMenu;
  [SerializeField]
  public CameraSubtleMovementOnInput _cameraSubtle;
  public Coroutine cIntroSequence;
  [SerializeField]
  public Stylizer _stylizer;
  [SerializeField]
  public Palette _defaultPalette;
  [SerializeField]
  public Palette _goatPalette;
  [SerializeField]
  public Palette _DLCPalette;
  [SerializeField]
  public GameObject _glitchEffect;
  [SerializeField]
  public GameObject _snowEffect;
  [SerializeField]
  public CanvasGroup _canvasGroupPRC;
  [SerializeField]
  public Image _darkModeInvertImage;
  [SerializeField]
  public Text _versionText;
  [SerializeField]
  public TextMeshProUGUI _editionText;
  [SerializeField]
  public SkeletonAnimation lambSpine;
  public bool isMajorDLC;
  public bool doIntroGlitch;
  public Coroutine cAttractMode;
  public EventInstance loopedSound;
  public TweenerCore<float, float, FloatOptions> tween;

  public static bool EngagementStarted
  {
    get => MainMenuController.\u003CEngagementStarted\u003Ek__BackingField;
    set => MainMenuController.\u003CEngagementStarted\u003Ek__BackingField = value;
  }

  public void Start()
  {
    this._cameraSubtle.enabled = false;
    this._glitchEffect.gameObject.SetActive(false);
    AudioManager.Instance.StopCurrentAtmos();
    AudioManager.Instance.StopActiveLoopsAndSFX();
    AudioManager.Instance.PlayMusic("event:/music/menu/menu_title");
    this._stylizer.LerpPalette = 0.0f;
    if (this.cIntroSequence != null)
      this.StopCoroutine(this.cIntroSequence);
    if (!CheatConsole.ForceAutoAttractMode)
      this.cIntroSequence = this.StartCoroutine(this.DoIntroSequence());
    if (CheatConsole.IN_DEMO)
    {
      CheatConsole.DemoBeginTime = 0.0f;
      this.AttractMode();
    }
    this._canvasGroupPRC.alpha = 0.0f;
    this._canvasGroupPRC.DOFade(1f, 1.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    DeviceLightingManager.PulseAllLighting(new Color(0.7f, 0.65f, 0.1f, 1f), new Color(1f, 0.7f, 0.4f, 1f), 1f, new KeyCode[0]);
    this._stylizer.EnableSecondPalette();
  }

  public IEnumerator CoOpUpdateRoutine()
  {
    while (true)
    {
      do
      {
        yield return (object) new WaitForSeconds((float) UnityEngine.Random.Range(3, 10));
      }
      while (!this.doIntroGlitch || UIComicMenuController.ComicActive || !((UnityEngine.Object) MMVideoPlayer.mmVideoPlayer == (UnityEngine.Object) null) && MMVideoPlayer.mmVideoPlayer.IsPlaying);
      this._stylizer.gameObject.transform.parent.DOKill();
      this._stylizer.gameObject.transform.parent.DOShakePosition(1f, new Vector3(0.0f, 0.15f));
      this._glitchEffect.gameObject.SetActive(true);
      EventInstance eventInstance = AudioManager.Instance.PlayOneShotWithInstanceCleanup("event:/relics/glitch", (Transform) null);
      float volume1 = 0.0f;
      int volume2 = (int) eventInstance.getVolume(out volume1);
      int num = (int) eventInstance.setVolume(volume1 * this.glitchVolumeMultiplier);
      this.lambSpine.skeleton.SetSkin("goat");
      this._stylizer.Palette = this._goatPalette;
      MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact);
      DeviceLightingManager.FlashColor(new Color(107f, 73f, 159f));
      this.lambSpine.transform.DOKill();
      this.lambSpine.transform.DOPunchPosition(new Vector3(0.0f, -0.25f, 0.0f), 0.25f);
      yield return (object) new WaitForSeconds(0.33f);
      this._glitchEffect.gameObject.SetActive(false);
      this._stylizer.Palette = this._defaultPalette;
      this.lambSpine.skeleton.SetSkin("lamb");
    }
  }

  public void DoIntro() => this.StartCoroutine(this.DoIntroSequence());

  public void AttractMode()
  {
    if (this.cAttractMode != null)
      this.StopCoroutine(this.cAttractMode);
    this.cAttractMode = this.StartCoroutine(this.DoAttactMode());
  }

  public IEnumerator DoAttactMode()
  {
    MainMenuController mainMenuController = this;
    CheatConsole.ForceResetTimeSinceLastKeyPress();
    Debug.Log((object) ("CheatConsole.ForceAutoAttractMode: " + CheatConsole.ForceAutoAttractMode.ToString()));
    bool Waiting = !CheatConsole.ForceAutoAttractMode;
    while (Waiting)
    {
      Debug.Log((object) ("CheatConsole.TimeSinceLastKeyPress : " + CheatConsole.TimeSinceLastKeyPress.ToString()));
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

  public void PlayVideo()
  {
    AudioManager.Instance.StopCurrentAtmos();
    AudioManager.Instance.StopCurrentMusic();
    AudioManager.Instance.PauseActiveLoopsAndSFX();
    MMVideoPlayer.Play("Trailer", new System.Action(this.VideoComplete), MMVideoPlayer.Options.ENABLE, MMVideoPlayer.Options.DISABLE, false);
    this.loopedSound = AudioManager.Instance.CreateLoop("event:/music/trailer/trailer_video", true);
    MMTransition.ResumePlay();
  }

  public void VideoComplete()
  {
    MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Main Menu", 1f, "", (System.Action) (() =>
    {
      AudioManager.Instance.StopLoop(this.loopedSound);
      MMVideoPlayer.Hide();
      Debug.Log((object) "VIDEO COMPLETE!");
    }));
  }

  public void ShowBlueTheme(float dur = 3f, float delay = 1f)
  {
    if (!this.isMajorDLC)
      return;
    if (this.tween != null)
      this.tween.Kill();
    float lerp = 0.0f;
    this.tween = DOTween.To((DOGetter<float>) (() => lerp), (DOSetter<float>) (x => lerp = x), 1f, dur).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutBack).SetDelay<TweenerCore<float, float, FloatOptions>>(delay).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this._stylizer.LerpPalette = lerp));
    DOTweenModuleUI.DOColor(this._versionText, Color.blue, 1f);
    ShortcutExtensionsTMPText.DOColor(this._editionText, Color.blue, 1f);
  }

  public void HideBlueTheme(float dur = 3f, float delay = 1f)
  {
    if (!this.isMajorDLC)
      return;
    if (this.tween != null)
      this.tween.Kill();
    float lerp = 1f;
    this.tween = DOTween.To((DOGetter<float>) (() => lerp), (DOSetter<float>) (x => lerp = x), 0.0f, dur).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutBack).SetDelay<TweenerCore<float, float, FloatOptions>>(delay).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this._stylizer.LerpPalette = lerp));
    DOTweenModuleUI.DOColor(this._versionText, Color.red, 1f);
    ShortcutExtensionsTMPText.DOColor(this._editionText, Color.red, 1f);
  }

  public IEnumerator DoIntroSequence()
  {
    MainMenuController coroutineSupport = this;
    Engagement.GlobalAllowEngagement = true;
    while (true)
    {
      if (!MainMenuController.EngagementStarted && SessionManager.instance.State == SessionManager.SessionState.Loading)
      {
        DeviceLightingManager.TransitionLighting(Color.red, new Color(0.7f, 0.65f, 0.1f, 1f), 2f, Ease.OutBounce);
        MainMenuController.EngagementStarted = true;
        coroutineSupport.StartTextAnimator.SetTrigger("Active");
        if (SettingsManager.Settings != null && SettingsManager.Settings.Accessibility.FlashingLights)
        {
          coroutineSupport.RedFlash.color = new Color(coroutineSupport.RedFlash.color.r, coroutineSupport.RedFlash.color.g, coroutineSupport.RedFlash.color.b, 1f);
          DOTweenModuleUI.DOFade(coroutineSupport.RedFlash, 0.0f, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        }
        UIManager.PlayAudio("event:/ui/Sermon Speech Bubble");
        UIManager.PlayAudio("event:/sermon/select_upgrade");
      }
      if (!SessionManager.instance.HasStarted)
        yield return (object) null;
      else
        break;
    }
    if (UnifyManager.platform != UnifyManager.Platform.Standalone)
    {
      Singleton<SettingsManager>.Instance.LoadAndApply(true);
      Singleton<PersistenceManager>.Instance.Load();
    }
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact, coroutineSupport: (MonoBehaviour) coroutineSupport);
    UIManager.PlayAudio("event:/ui/start_game");
    coroutineSupport.StartTextAnimator.SetTrigger("End");
    if (PersistenceManager.PersistentData.PlayedTrailerIndex == 0)
    {
      PersistenceManager.PersistentData.PlayedTrailerIndex = 1;
      PersistenceManager.Save();
      AudioManager.Instance.StopCurrentMusic();
      bool playingVideo = true;
      MMVideoPlayer.Play("Update_Video", (System.Action) (() => playingVideo = false), MMVideoPlayer.Options.ENABLE, MMVideoPlayer.Options.DISABLE);
      yield return (object) new WaitForSeconds(0.5f);
      EventInstance instance = AudioManager.Instance.PlayOneShotWithInstance("event:/music/trailer/update_video");
      while (playingVideo)
      {
        MonoSingleton<UINavigatorNew>.Instance.LockInput = MonoSingleton<UINavigatorNew>.Instance.LockNavigation = true;
        yield return (object) null;
      }
      AudioManager.Instance.StopOneShotInstanceEarly(instance, STOP_MODE.IMMEDIATE);
      AudioManager.Instance.PlayMusic("event:/music/menu/menu_title");
      yield return (object) new WaitForSeconds(0.5f);
      instance = new EventInstance();
    }
    coroutineSupport.StartCoroutine(coroutineSupport._uiMainMenu.PreloadMetadata());
    MonoSingleton<UINavigatorNew>.Instance.LockInput = MonoSingleton<UINavigatorNew>.Instance.LockNavigation = true;
    if (GameManager.AuthenticateMajorDLC())
    {
      coroutineSupport.isMajorDLC = true;
      coroutineSupport.ShowBlueTheme();
      coroutineSupport._snowEffect.gameObject.SetActive(true);
    }
    coroutineSupport._animator.Play("Intro");
    yield return (object) new WaitForSeconds(3.5f);
    coroutineSupport._cameraSubtle.enabled = true;
    if (CheatConsole.IN_DEMO)
    {
      CheatConsole.DemoBeginTime = 0.0f;
      coroutineSupport.AttractMode();
    }
    if (GameManager.AuthenticatePilgrimDLC())
    {
      coroutineSupport.StartCoroutine(coroutineSupport._uiMainMenu.WaitForDLCCheck());
      if (!PersistenceManager.PersistentData.RevealedComic)
      {
        coroutineSupport._uiMainMenu.FocusOnComicButton();
        PersistenceManager.PersistentData.RevealedComic = true;
        PersistenceManager.Save();
      }
    }
    UnifyManager.instance.OnPaused += new UnifyManager.PauseDelegate(coroutineSupport.CheckDLCAdded);
  }

  public void CheckDLCAdded(bool Suspended)
  {
    if (Suspended)
      return;
    Debug.Log((object) "Check DLC");
    if (!GameManager.AuthenticatePilgrimDLC())
      return;
    this.StartCoroutine(this._uiMainMenu.WaitForDLCCheck());
    if (PersistenceManager.PersistentData.RevealedComic)
      return;
    this._uiMainMenu.FocusOnComicButton();
    PersistenceManager.PersistentData.RevealedComic = true;
    PersistenceManager.Save();
  }

  public void OnDestroy()
  {
    UnifyManager.instance.OnPaused -= new UnifyManager.PauseDelegate(this.CheckDLCAdded);
    MainMenuController.EngagementStarted = false;
  }

  public void StartInput()
  {
    MonoSingleton<UINavigatorNew>.Instance.LockInput = MonoSingleton<UINavigatorNew>.Instance.LockNavigation = false;
    Debug.Log((object) $"AutomationClient recived:{this.gameObject.name}_SHOWN");
    UnifyManager.Instance.AutomationClient.SendGameEvent(this.gameObject.name.Replace(" ", "_") + "_SHOWN");
    this._uiMainMenu.MainMenu.Show(true);
  }

  [CompilerGenerated]
  public void \u003CVideoComplete\u003Eb__33_0()
  {
    AudioManager.Instance.StopLoop(this.loopedSound);
    MMVideoPlayer.Hide();
    Debug.Log((object) "VIDEO COMPLETE!");
  }
}
