// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.KBMainMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using System.Collections;
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
  private MMButton _playButton;
  [SerializeField]
  private MMButton _instructionsButton;
  [SerializeField]
  private MMButton _quitButton;
  [Header("Misc")]
  [SerializeField]
  private UINavigatorFollowElement _buttonHighlight;

  public void Start()
  {
    this._playButton.onClick.AddListener(new UnityAction(this.OnPlayButtonClicked));
    this._instructionsButton.onClick.AddListener(new UnityAction(this.OnInstructionsButtonClicked));
    this._quitButton.onClick.AddListener(new UnityAction(this.OnQuitButtonClicked));
  }

  private void OnPlayButtonClicked()
  {
    System.Action playButtonPressed = this.OnPlayButtonPressed;
    if (playButtonPressed == null)
      return;
    playButtonPressed();
  }

  private void OnInstructionsButtonClicked()
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

  private void OnQuitButtonClicked()
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

  protected override IEnumerator DoShowAnimation()
  {
    KBMainMenu kbMainMenu = this;
    while ((double) kbMainMenu._canvasGroup.alpha < 1.0)
    {
      kbMainMenu._canvasGroup.alpha += Time.deltaTime;
      yield return (object) null;
    }
  }

  protected override IEnumerator DoHideAnimation()
  {
    KBMainMenu kbMainMenu = this;
    while ((double) kbMainMenu._canvasGroup.alpha > 0.0)
    {
      kbMainMenu._canvasGroup.alpha -= Time.deltaTime;
      yield return (object) null;
    }
  }
}
