// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.KBInstructionsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public Button _continueButton;
  [Header("Misc")]
  [SerializeField]
  public UINavigatorFollowElement _buttonHighlight;

  public void Start()
  {
    this._continueButton.onClick.AddListener(new UnityAction(this.OnContinueButtonClicked));
  }

  public override void OnShowStarted() => DataManager.Instance.ShownKnuckleboneTutorial = true;

  public void OnContinueButtonClicked()
  {
    System.Action continueButtonPressed = this.OnContinueButtonPressed;
    if (continueButtonPressed == null)
      return;
    continueButtonPressed();
  }

  public void OnBackButtonClicked()
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

  public override void SetActiveStateForMenu(bool state)
  {
    this._buttonHighlight.enabled = state;
    base.SetActiveStateForMenu(state);
  }

  public override IEnumerator DoShowAnimation()
  {
    KBInstructionsScreen instructionsScreen = this;
    while ((double) instructionsScreen._canvasGroup.alpha < 1.0)
    {
      instructionsScreen._canvasGroup.alpha += Time.deltaTime;
      yield return (object) null;
    }
  }

  public override IEnumerator DoHideAnimation()
  {
    KBInstructionsScreen instructionsScreen = this;
    while ((double) instructionsScreen._canvasGroup.alpha > 0.0)
    {
      instructionsScreen._canvasGroup.alpha -= Time.deltaTime;
      yield return (object) null;
    }
  }
}
