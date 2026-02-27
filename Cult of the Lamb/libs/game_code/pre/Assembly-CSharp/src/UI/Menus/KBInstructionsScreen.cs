// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.KBInstructionsScreen
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

public class KBInstructionsScreen : UISubmenuBase
{
  public System.Action OnContinueButtonPressed;
  public System.Action OnBackButtonPressed;
  [Header("Instructions Menu")]
  [SerializeField]
  private Button _continueButton;
  [Header("Misc")]
  [SerializeField]
  private UINavigatorFollowElement _buttonHighlight;

  private void Start()
  {
    this._continueButton.onClick.AddListener(new UnityAction(this.OnContinueButtonClicked));
  }

  protected override void OnShowStarted() => DataManager.Instance.ShownKnuckleboneTutorial = true;

  private void OnContinueButtonClicked()
  {
    System.Action continueButtonPressed = this.OnContinueButtonPressed;
    if (continueButtonPressed == null)
      return;
    continueButtonPressed();
  }

  private void OnBackButtonClicked()
  {
    System.Action backButtonPressed = this.OnBackButtonPressed;
    if (backButtonPressed == null)
      return;
    backButtonPressed();
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.OnBackButtonClicked();
  }

  protected override void SetActiveStateForMenu(bool state)
  {
    this._buttonHighlight.enabled = state;
    base.SetActiveStateForMenu(state);
  }

  protected override IEnumerator DoShowAnimation()
  {
    KBInstructionsScreen instructionsScreen = this;
    while ((double) instructionsScreen._canvasGroup.alpha < 1.0)
    {
      instructionsScreen._canvasGroup.alpha += Time.deltaTime;
      yield return (object) null;
    }
  }

  protected override IEnumerator DoHideAnimation()
  {
    KBInstructionsScreen instructionsScreen = this;
    while ((double) instructionsScreen._canvasGroup.alpha > 0.0)
    {
      instructionsScreen._canvasGroup.alpha -= Time.deltaTime;
      yield return (object) null;
    }
  }
}
