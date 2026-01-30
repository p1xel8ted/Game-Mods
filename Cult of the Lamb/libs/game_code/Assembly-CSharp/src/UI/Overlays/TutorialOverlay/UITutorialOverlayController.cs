// Decompiled with JetBrains decompiler
// Type: src.UI.Overlays.TutorialOverlay.UITutorialOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using src.UI.InfoCards;
using src.UINavigator;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace src.UI.Overlays.TutorialOverlay;

public class UITutorialOverlayController : UIMenuBase
{
  [SerializeField]
  public TutorialInfoCard _tutorialInfoCard;
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  public GameObject _prevPrompt;
  [SerializeField]
  public GameObject _nextPrompt;
  [SerializeField]
  public GameObject _blurBackground;
  public TutorialTopic _topic;
  public float _delay;
  public bool _leftDown;
  public bool _rightDown;

  public override void Awake()
  {
    base.Awake();
    this._canvasGroup.alpha = 0.0f;
  }

  public void Show(TutorialTopic topic, float delay = 0.0f, bool instant = false)
  {
    this._topic = topic;
    this._delay = delay;
    this.Show(instant);
  }

  public override void OnShowStarted()
  {
    if (!SettingsManager.Settings.Game.ShowTutorials)
    {
      this.Hide(true);
    }
    else
    {
      AudioManager.Instance.PauseActiveLoopsAndSFX();
      UIManager.PlayAudio("event:/ui/open_menu");
      UIManager.PlayAudio("event:/Stings/church_bell");
      this._controlPrompts.HideAcceptButton();
      this._controlPrompts.HideCancelButton();
      this._tutorialInfoCard.Show(this._topic, false);
      this._tutorialInfoCard.OnLeftArrowClicked += (System.Action) (() => this._leftDown = true);
      this._tutorialInfoCard.OnRightArrowClicked += (System.Action) (() => this._rightDown = true);
      this._prevPrompt.SetActive(false);
      this._nextPrompt.SetActive(true);
    }
  }

  public override IEnumerator DoShowAnimation()
  {
    yield return (object) new WaitForSecondsRealtime(this._delay);
    yield return (object) this.\u003C\u003En__0();
    yield return (object) new WaitForSecondsRealtime(0.5f);
  }

  public override void OnShowCompleted()
  {
    if (SaveGameCorruptionTool.s_RunningTest)
      this.Hide();
    else
      this.StartCoroutine((IEnumerator) this.RunOverlay());
  }

  public IEnumerator RunOverlay()
  {
    UITutorialOverlayController overlayController = this;
    bool seenAllPages = false;
    bool inputBuffer = false;
    overlayController._tutorialInfoCard.Animator.enabled = false;
    while (true)
    {
      if (!overlayController._leftDown)
        overlayController._leftDown = (double) InputManager.UI.GetHorizontalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) < -0.20000000298023224;
      if (!overlayController._rightDown)
        overlayController._rightDown = (double) InputManager.UI.GetHorizontalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) > 0.20000000298023224;
      if (overlayController._leftDown)
      {
        if (!inputBuffer)
        {
          inputBuffer = true;
          yield return (object) overlayController._tutorialInfoCard.PreviousPage();
        }
      }
      else if (overlayController._rightDown)
      {
        if (!inputBuffer)
        {
          inputBuffer = true;
          yield return (object) overlayController._tutorialInfoCard.NextPage();
          if (!seenAllPages && overlayController._tutorialInfoCard.Page == overlayController._tutorialInfoCard.NumPages)
          {
            seenAllPages = true;
            overlayController._controlPrompts.ShowAcceptButton();
          }
        }
      }
      else
        inputBuffer = false;
      overlayController._prevPrompt.SetActive(overlayController._tutorialInfoCard.Page > 0);
      overlayController._nextPrompt.SetActive(overlayController._tutorialInfoCard.Page < overlayController._tutorialInfoCard.NumPages);
      overlayController._leftDown = false;
      overlayController._rightDown = false;
      if (!seenAllPages || !InputManager.UI.GetCancelButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) && !InputManager.UI.GetAcceptButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
        yield return (object) null;
      else
        break;
    }
    overlayController.Hide();
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/close_menu");
  }

  public override void OnHideCompleted()
  {
    AudioManager.Instance.ResumePausedLoopsAndSFX();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void HideBlurBackground() => this._blurBackground.gameObject.SetActive(false);

  [CompilerGenerated]
  public void \u003COnShowStarted\u003Eb__11_0() => this._leftDown = true;

  [CompilerGenerated]
  public void \u003COnShowStarted\u003Eb__11_1() => this._rightDown = true;

  [CompilerGenerated]
  [DebuggerHidden]
  public IEnumerator \u003C\u003En__0() => base.DoShowAnimation();
}
