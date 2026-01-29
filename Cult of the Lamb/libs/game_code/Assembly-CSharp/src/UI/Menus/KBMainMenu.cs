// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.KBMainMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace src.UI.Menus;

public class KBMainMenu : UISubmenuBase
{
  public System.Action OnPlayButtonPressed;
  public System.Action OnInstructionsButtonPressed;
  public System.Action OnQuitButtonPressed;
  [Header("Main Menu")]
  [SerializeField]
  public MMButton _playButton;
  [SerializeField]
  public MMButton _instructionsButton;
  [SerializeField]
  public MMButton _quitButton;
  [Header("Misc")]
  [SerializeField]
  public UINavigatorFollowElement _buttonHighlight;

  public void Start()
  {
    this._playButton.onClick.AddListener(new UnityAction(this.OnPlayButtonClicked));
    this._instructionsButton.onClick.AddListener(new UnityAction(this.OnInstructionsButtonClicked));
    this._quitButton.onClick.AddListener(new UnityAction(this.OnQuitButtonClicked));
  }

  public void OnPlayButtonClicked()
  {
    System.Action playButtonPressed = this.OnPlayButtonPressed;
    if (playButtonPressed == null)
      return;
    playButtonPressed();
  }

  public void OnInstructionsButtonClicked()
  {
    this.OverrideDefaultOnce((Selectable) this._instructionsButton);
    System.Action instructionsButtonPressed = this.OnInstructionsButtonPressed;
    if (instructionsButtonPressed == null)
      return;
    instructionsButtonPressed();
  }

  public override void OnCancelButtonInput()
  {
    if (!this._parent.CanvasGroup.interactable || !this.CanvasGroup.interactable)
      return;
    this.OnQuitButtonClicked();
  }

  public void OnQuitButtonClicked()
  {
    this._buttonHighlight.enabled = false;
    UIMenuConfirmationWindow confirmationWindow = this.Push<UIMenuConfirmationWindow>(MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate);
    confirmationWindow.Configure(KnucklebonesModel.GetLocalizedString("Exit"), KnucklebonesModel.GetLocalizedString("AreYouSure"));
    confirmationWindow.OnHide = confirmationWindow.OnHide + (System.Action) (() => this._buttonHighlight.enabled = true);
    confirmationWindow.OnConfirm += (System.Action) (() =>
    {
      System.Action quitButtonPressed = this.OnQuitButtonPressed;
      if (quitButtonPressed == null)
        return;
      quitButtonPressed();
    });
  }

  public override IEnumerator DoShowAnimation()
  {
    KBMainMenu kbMainMenu = this;
    while ((double) kbMainMenu._canvasGroup.alpha < 1.0)
    {
      kbMainMenu._canvasGroup.alpha += Time.deltaTime;
      yield return (object) null;
    }
  }

  public override IEnumerator DoHideAnimation()
  {
    KBMainMenu kbMainMenu = this;
    while ((double) kbMainMenu._canvasGroup.alpha > 0.0)
    {
      kbMainMenu._canvasGroup.alpha -= Time.deltaTime;
      yield return (object) null;
    }
  }

  [CompilerGenerated]
  public void \u003COnQuitButtonClicked\u003Eb__11_0() => this._buttonHighlight.enabled = true;

  [CompilerGenerated]
  public void \u003COnQuitButtonClicked\u003Eb__11_1()
  {
    System.Action quitButtonPressed = this.OnQuitButtonPressed;
    if (quitButtonPressed == null)
      return;
    quitButtonPressed();
  }
}
