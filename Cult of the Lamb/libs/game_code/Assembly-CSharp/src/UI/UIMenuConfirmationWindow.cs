// Decompiled with JetBrains decompiler
// Type: src.UI.UIMenuConfirmationWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using src.UINavigator;
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
  public MMButton _cancelButton;
  [SerializeField]
  public MMButton _confirmButton;
  [Header("Text")]
  [SerializeField]
  public TextMeshProUGUI _headerText;
  [SerializeField]
  public TextMeshProUGUI _bodyText;
  [Header("Misc")]
  [SerializeField]
  public UINavigatorFollowElement _buttonHighlight;

  public TextAlignmentOptions HeaderAlignment
  {
    get => this._headerText.alignment;
    set => this._headerText.alignment = value;
  }

  public TextAlignmentOptions BodyAlignment
  {
    get => this._bodyText.alignment;
    set => this._bodyText.alignment = value;
  }

  public override void Awake()
  {
    base.Awake();
    this.SetActiveStateForMenu(false);
  }

  public void Configure(string header, string body, bool acceptOnly = false)
  {
    this._headerText.text = header;
    this._bodyText.text = body;
    MonoSingleton<UINavigatorNew>.Instance.LockNavigation = false;
    MonoSingleton<UINavigatorNew>.Instance.LockInput = false;
    if (!acceptOnly)
      return;
    this._cancelButton.gameObject.SetActive(false);
    this.OverrideDefault((Selectable) this._confirmButton);
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._confirmButton);
  }

  public void Start()
  {
    this._cancelButton.onClick.AddListener(new UnityAction(this.OnCancelClicked));
    this._confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmClicked));
  }

  public virtual void OnCancelClicked()
  {
    System.Action onCancel = this.OnCancel;
    if (onCancel != null)
      onCancel();
    this.Hide();
  }

  public virtual void OnConfirmClicked()
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

  public override void OnShowStarted() => this._buttonHighlight.enabled = true;

  public override void OnHideStarted() => this._buttonHighlight.enabled = false;

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
