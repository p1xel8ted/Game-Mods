// Decompiled with JetBrains decompiler
// Type: src.UI.UIMenuConfirmationWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace src.UI;

public class UIMenuConfirmationWindow : UIMenuBase
{
  public new System.Action OnCancel;
  public System.Action OnConfirm;
  [Header("Buttons")]
  [SerializeField]
  private Button _cancelButton;
  [SerializeField]
  private Button _confirmButton;
  [Header("Text")]
  [SerializeField]
  protected TextMeshProUGUI _headerText;
  [SerializeField]
  protected TextMeshProUGUI _bodyText;
  [Header("Misc")]
  [SerializeField]
  private UINavigatorFollowElement _buttonHighlight;

  public override void Awake()
  {
    base.Awake();
    this.SetActiveStateForMenu(false);
  }

  public void Configure(string header, string body, bool acceptOnly = false)
  {
    this._headerText.text = header;
    this._bodyText.text = body;
    if (!acceptOnly)
      return;
    this._cancelButton.gameObject.SetActive(false);
    this.OverrideDefault((Selectable) this._confirmButton);
  }

  private void Start()
  {
    this._cancelButton.onClick.AddListener(new UnityAction(this.OnCancelClicked));
    this._confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmClicked));
  }

  protected virtual void OnCancelClicked()
  {
    System.Action onCancel = this.OnCancel;
    if (onCancel != null)
      onCancel();
    this.Hide();
  }

  protected virtual void OnConfirmClicked()
  {
    this.Hide();
    System.Action onConfirm = this.OnConfirm;
    if (onConfirm == null)
      return;
    onConfirm();
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.OnCancelClicked();
  }

  protected override void OnShowStarted() => this._buttonHighlight.enabled = true;

  protected override void OnHideStarted() => this._buttonHighlight.enabled = false;

  protected override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
