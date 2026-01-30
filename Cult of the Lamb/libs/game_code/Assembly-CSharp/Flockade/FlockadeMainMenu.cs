// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeMainMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using src.UI;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

public class FlockadeMainMenu : UISubmenuBase
{
  [Header("Main Menu")]
  [SerializeField]
  public MMButton _playButton;
  [SerializeField]
  public Localize _playButtonText;
  [SerializeField]
  public MMButton _playHardModeButton;
  [SerializeField]
  public MMButton _instructionsButton;
  [SerializeField]
  public MMButton _quitButton;
  [SerializeField]
  [TermsPopup("")]
  public string _playButtonTextWhenHardModeIsUnlocked;
  [SerializeField]
  public DataManager.Variables[] _variablesToUnlockHardMode;
  [SerializeField]
  public GameObject _shepherdIcon;
  [Header("Credits")]
  [SerializeField]
  [TermsPopup("")]
  public string _creditsTermLine1_0;
  [SerializeField]
  [TermsPopup("")]
  public string _creditsTermLine1_1;
  [SerializeField]
  [TermsPopup("")]
  public string _creditsTermLine2_0;
  [SerializeField]
  [TermsPopup("")]
  public string _creditsTermLine2_1;
  [SerializeField]
  [TermsPopup("")]
  public string _creditsTermLine3_0;
  [SerializeField]
  public Localize _creditsTextLine1_0;
  [SerializeField]
  public Localize _creditsTextLine1_1;
  [SerializeField]
  public Localize _creditsTextLine2_0;
  [SerializeField]
  public Localize _creditsTextLine2_1;
  [SerializeField]
  public Localize _creditsTextLine3_0;
  [SerializeField]
  public TMP_Text _creditsText2Line_0;
  [SerializeField]
  public TMP_Text _creditsText2Line_1;
  [Header("Misc")]
  [SerializeField]
  public UINavigatorFollowElement _buttonHighlight;
  public bool hardModeDisabled;

  public event System.Action PlayButtonPressed;

  public event System.Action PlayHardModeButtonPressed;

  public event System.Action InstructionsButtonPressed;

  public event System.Action QuitButtonPressed;

  public override IEnumerator DoShowAnimation()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    FlockadeMainMenu flockadeMainMenu = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) flockadeMainMenu._canvasGroup.DOFade(1f, 0.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InCubic).WaitForCompletion();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public virtual void Start()
  {
    this._playButton.onClick.AddListener(new UnityAction(this.EmitPlayButtonPressed));
    this._instructionsButton.onClick.AddListener(new UnityAction(this.EmitInstructionsButtonPressed));
    this._quitButton.onClick.AddListener(new UnityAction(this.EmitQuitButtonPressed));
    if (FlockadePieceManager.IsAnyPieceOfSameKindUnlocked(FlockadePieceType.Shepherd))
      this._shepherdIcon.SetActive(true);
    if (((IEnumerable<DataManager.Variables>) this._variablesToUnlockHardMode).All<DataManager.Variables>((Func<DataManager.Variables, bool>) (variable => DataManager.Instance.GetVariable(variable))) && !this.hardModeDisabled)
    {
      this._playHardModeButton.gameObject.SetActive(true);
      this._playHardModeButton.onClick.AddListener(new UnityAction(this.EmitPlayHardModeButtonPressed));
      if (!string.IsNullOrEmpty(this._playButtonTextWhenHardModeIsUnlocked))
        this._playButtonText.Term = this._playButtonTextWhenHardModeIsUnlocked;
    }
    this.SetCreditsLocalisation();
  }

  public void SetCreditsLocalisation()
  {
    if ((UnityEngine.Object) this._creditsTextLine1_0 != (UnityEngine.Object) null)
      this._creditsTextLine1_0.Term = this._creditsTermLine1_0;
    if ((UnityEngine.Object) this._creditsTextLine1_1 != (UnityEngine.Object) null)
      this._creditsTextLine1_1.Term = this._creditsTermLine1_1;
    if ((UnityEngine.Object) this._creditsTextLine2_0 != (UnityEngine.Object) null)
      this._creditsTextLine2_0.Term = this._creditsTermLine2_0;
    if ((UnityEngine.Object) this._creditsTextLine2_1 != (UnityEngine.Object) null)
      this._creditsTextLine2_1.Term = this._creditsTermLine2_1;
    if ((UnityEngine.Object) this._creditsTextLine3_0 != (UnityEngine.Object) null)
      this._creditsTextLine3_0.Term = this._creditsTermLine3_0;
    if ((UnityEngine.Object) this._creditsText2Line_0 != (UnityEngine.Object) null)
      this._creditsText2Line_0.text = $"{LocalizationManager.GetTranslation(this._creditsTermLine1_0)} {LocalizationManager.GetTranslation(this._creditsTermLine1_1)}";
    if (!((UnityEngine.Object) this._creditsText2Line_1 != (UnityEngine.Object) null))
      return;
    this._creditsText2Line_1.text = $"{LocalizationManager.GetTranslation(this._creditsTermLine2_0)} {LocalizationManager.GetTranslation(this._creditsTermLine2_1)} {LocalizationManager.GetTranslation(this._creditsTermLine3_0)}";
  }

  public void EmitPlayButtonPressed()
  {
    System.Action playButtonPressed = this.PlayButtonPressed;
    if (playButtonPressed == null)
      return;
    playButtonPressed();
  }

  public void EmitPlayHardModeButtonPressed()
  {
    System.Action modeButtonPressed = this.PlayHardModeButtonPressed;
    if (modeButtonPressed == null)
      return;
    modeButtonPressed();
  }

  public void EmitInstructionsButtonPressed()
  {
    this.OverrideDefaultOnce((Selectable) this._instructionsButton);
    System.Action instructionsButtonPressed = this.InstructionsButtonPressed;
    if (instructionsButtonPressed == null)
      return;
    instructionsButtonPressed();
  }

  public void EmitQuitButtonPressed()
  {
    this._buttonHighlight.enabled = false;
    UIMenuConfirmationWindow confirmationWindow = this.Push<UIMenuConfirmationWindow>(MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate);
    confirmationWindow.Configure(ScriptLocalization.UI_Flockade.Exit, ScriptLocalization.UI_Flockade.AreYouSure);
    confirmationWindow.OnShow = confirmationWindow.OnShow + (System.Action) (() => UIManager.PlayAudio("event:/ui/open_menu"));
    confirmationWindow.OnConfirm += (System.Action) (() =>
    {
      System.Action quitButtonPressed = this.QuitButtonPressed;
      if (quitButtonPressed == null)
        return;
      quitButtonPressed();
    });
    confirmationWindow.OnCancel += new System.Action(MonoSingleton<UINavigatorNew>.Instance.Clear);
    confirmationWindow.OnHide = confirmationWindow.OnHide + (System.Action) (() =>
    {
      this._buttonHighlight.enabled = true;
      UIManager.PlayAudio("event:/ui/close_menu");
    });
  }

  public override void OnCancelButtonInput()
  {
    if (!this._parent.CanvasGroup.interactable || !this._canvasGroup.interactable)
      return;
    this.EmitQuitButtonPressed();
  }

  public override IEnumerator DoHideAnimation()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    FlockadeMainMenu flockadeMainMenu = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) flockadeMainMenu._canvasGroup.DOFade(0.0f, 0.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutCubic).WaitForCompletion();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void HideHardMode()
  {
    this._playHardModeButton.gameObject.SetActive(false);
    this.hardModeDisabled = true;
  }

  [CompilerGenerated]
  public void \u003CEmitQuitButtonPressed\u003Eb__40_1()
  {
    System.Action quitButtonPressed = this.QuitButtonPressed;
    if (quitButtonPressed == null)
      return;
    quitButtonPressed();
  }

  [CompilerGenerated]
  public void \u003CEmitQuitButtonPressed\u003Eb__40_2()
  {
    this._buttonHighlight.enabled = true;
    UIManager.PlayAudio("event:/ui/close_menu");
  }
}
