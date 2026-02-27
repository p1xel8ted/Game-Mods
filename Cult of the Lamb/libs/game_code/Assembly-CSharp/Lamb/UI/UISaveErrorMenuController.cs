// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UISaveErrorMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public class UISaveErrorMenuController : UIMenuBase
{
  public System.Action OnContinueButtonPressed;
  public System.Action OnRetryButtonPressed;
  [SerializeField]
  public MMButton _continueButton;
  [SerializeField]
  public MMButton _retryButton;
  [SerializeField]
  public UINavigatorFollowElement _buttonHighlight;

  public void Start()
  {
    this._continueButton.onClick.AddListener(new UnityAction(this.OnContinueButtonClicked));
    this._retryButton.onClick.AddListener(new UnityAction(this.OnRetryButtonClicked));
  }

  public void OnContinueButtonClicked()
  {
    System.Action continueButtonPressed = this.OnContinueButtonPressed;
    if (continueButtonPressed != null)
      continueButtonPressed();
    this.Hide();
  }

  public void OnRetryButtonClicked()
  {
    System.Action retryButtonPressed = this.OnRetryButtonPressed;
    if (retryButtonPressed != null)
      retryButtonPressed();
    this.Hide();
  }

  public override void OnShowStarted() => this._buttonHighlight.enabled = true;

  public override void OnHideStarted() => this._buttonHighlight.enabled = false;

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
