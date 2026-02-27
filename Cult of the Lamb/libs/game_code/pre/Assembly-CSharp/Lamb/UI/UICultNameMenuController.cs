// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UICultNameMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using src.UI;
using System;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public class UICultNameMenuController : UIMenuBase
{
  public Action<string> OnNameConfirmed;
  [Header("Cult Name Menu")]
  [SerializeField]
  private MMInputField _nameInputField;
  [SerializeField]
  private MMButton _confirmButton;
  [SerializeField]
  private RectTransform _confirmButtonRectTransform;
  [SerializeField]
  private RectTransform _emptyTextTransform;
  [SerializeField]
  private ButtonHighlightController _buttonHighlight;
  [SerializeField]
  private RectTransform _buttonHighlightRectTransform;
  [SerializeField]
  private UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  private GameObject _renameDisclaimer;
  private bool _editing;
  private Vector2 _buttonHighlightSizeDelta;
  private Vector3 _confirmButtonOrigin;
  private Vector3 _emptyTextOrigin;
  private bool _cancellable;

  public override void Awake()
  {
    base.Awake();
    this._buttonHighlight.SetAsRed();
    this._nameInputField.characterLimit = 16 /*0x10*/;
    this._buttonHighlightSizeDelta = this._buttonHighlightRectTransform.sizeDelta;
    this._nameInputField.text = LocalizationManager.GetTranslation("NAMES/Place/Cult");
    this._nameInputField.OnEndedEditing += new Action<string>(this.OnEndedEditing);
    this._nameInputField.OnStartedEditing += new System.Action(this.OnStartedEditing);
    this._nameInputField.OnSelected += new System.Action(this.OnNameInputSelected);
    this._confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
    this._confirmButton.OnConfirmDenied += new System.Action(this.ShakeConfirmButton);
    this._confirmButton.OnSelected += new System.Action(this.OnConfirmButtonSelected);
    this._confirmButtonOrigin = (Vector3) this._confirmButtonRectTransform.anchoredPosition;
    this._emptyTextTransform.gameObject.SetActive(false);
    this._emptyTextOrigin = (Vector3) this._emptyTextTransform.anchoredPosition;
  }

  public void Show(bool cancellable, bool showDisclaimer)
  {
    this._cancellable = cancellable;
    if (!this._cancellable)
      this._controlPrompts.HideCancelButton();
    this._renameDisclaimer.SetActive(showDisclaimer);
    this.Show();
  }

  private void OnNameInputSelected()
  {
    this._buttonHighlightRectTransform.sizeDelta = this._buttonHighlightSizeDelta;
  }

  private void OnStartedEditing() => this._emptyTextTransform.gameObject.SetActive(false);

  private void OnEndedEditing(string text)
  {
    this._confirmButton.Confirmable = !string.IsNullOrWhiteSpace(text);
    this._emptyTextTransform.gameObject.SetActive(!this._confirmButton.Confirmable);
  }

  private void OnConfirmButtonSelected()
  {
    this._confirmButton.Confirmable = !string.IsNullOrWhiteSpace(this._nameInputField.text);
    this._buttonHighlightRectTransform.sizeDelta = new Vector2(320f, 72f);
  }

  private void OnConfirmButtonClicked()
  {
    Action<string> onNameConfirmed = this.OnNameConfirmed;
    if (onNameConfirmed != null)
      onNameConfirmed(this._nameInputField.text);
    this.Hide();
  }

  private void ShakeConfirmButton()
  {
    this._emptyTextTransform.DOKill();
    this._confirmButtonRectTransform.DOKill();
    this._confirmButtonRectTransform.anchoredPosition = (Vector2) this._confirmButtonOrigin;
    this._emptyTextTransform.anchoredPosition = (Vector2) this._emptyTextOrigin;
    this._confirmButtonRectTransform.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
    this._emptyTextTransform.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }

  public override void OnCancelButtonInput()
  {
    base.OnCancelButtonInput();
    if (!this._canvasGroup.interactable || !this._cancellable || this._nameInputField.isFocused)
      return;
    this.Hide();
  }

  protected override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
