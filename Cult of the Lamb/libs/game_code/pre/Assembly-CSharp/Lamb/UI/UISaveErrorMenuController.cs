// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UISaveErrorMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public class UISaveErrorMenuController : UIMenuBase
{
  public System.Action OnContinueButtonPressed;
  public System.Action OnRetryButtonPressed;
  [SerializeField]
  private MMButton _continueButton;
  [SerializeField]
  private MMButton _retryButton;
  [SerializeField]
  private UINavigatorFollowElement _buttonHighlight;

  private void Start()
  {
    this._continueButton.onClick.AddListener(new UnityAction(this.OnContinueButtonClicked));
    this._retryButton.onClick.AddListener(new UnityAction(this.OnRetryButtonClicked));
  }

  private void OnContinueButtonClicked()
  {
    System.Action continueButtonPressed = this.OnContinueButtonPressed;
    if (continueButtonPressed != null)
      continueButtonPressed();
    this.Hide();
  }

  private void OnRetryButtonClicked()
  {
    System.Action retryButtonPressed = this.OnRetryButtonPressed;
    if (retryButtonPressed != null)
      retryButtonPressed();
    this.Hide();
  }

  protected override void OnShowStarted() => this._buttonHighlight.enabled = true;

  protected override void OnHideStarted() => this._buttonHighlight.enabled = false;

  protected override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
