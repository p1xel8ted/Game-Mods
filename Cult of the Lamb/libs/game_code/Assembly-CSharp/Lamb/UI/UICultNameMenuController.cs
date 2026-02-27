// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UICultNameMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using src.UI;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public class UICultNameMenuController : UIMenuBase
{
  public Action<string> OnNameConfirmed;
  [Header("Cult Name Menu")]
  [SerializeField]
  public MMInputField _nameInputField;
  [SerializeField]
  public MMButton _confirmButton;
  [SerializeField]
  public RectTransform _confirmButtonRectTransform;
  [SerializeField]
  public RectTransform _emptyTextTransform;
  [SerializeField]
  public ButtonHighlightController _buttonHighlight;
  [SerializeField]
  public RectTransform _buttonHighlightRectTransform;
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  public GameObject _renameDisclaimer;
  [SerializeField]
  public TMP_Text _title;
  public bool _editing;
  public Vector2 _buttonHighlightSizeDelta;
  public Vector3 _confirmButtonOrigin;
  public Vector3 _emptyTextOrigin;
  public bool _cancellable;
  [CompilerGenerated]
  public bool \u003CRequiresName\u003Ek__BackingField = true;

  public bool RequiresName
  {
    get => this.\u003CRequiresName\u003Ek__BackingField;
    set => this.\u003CRequiresName\u003Ek__BackingField = value;
  }

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

  public void Show(string prefillText, bool cancellable, bool showDisclaimer)
  {
    this._nameInputField.text = prefillText;
    this.Show(cancellable, showDisclaimer);
  }

  public void ShowEmptyName() => this._emptyTextTransform.gameObject.SetActive(true);

  public void Show(bool cancellable, bool showDisclaimer)
  {
    this._cancellable = cancellable;
    if (!this._cancellable)
      this._controlPrompts.HideCancelButton();
    this._renameDisclaimer.SetActive(showDisclaimer);
    this.Show();
  }

  public override void OnShowCompleted()
  {
    base.OnShowCompleted();
    Time.timeScale = 0.0f;
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    Time.timeScale = 1f;
  }

  public void OnNameInputSelected()
  {
    this._buttonHighlightRectTransform.sizeDelta = this._buttonHighlightSizeDelta;
  }

  public void OnStartedEditing() => this._emptyTextTransform.gameObject.SetActive(false);

  public void OnEndedEditing(string text)
  {
    this._confirmButton.Confirmable = !this.RequiresName || !string.IsNullOrWhiteSpace(text);
    this._emptyTextTransform.gameObject.SetActive(!this._confirmButton.Confirmable);
  }

  public void OnConfirmButtonSelected()
  {
    this._confirmButton.Confirmable = !this.RequiresName || !string.IsNullOrWhiteSpace(this._nameInputField.text);
    this._buttonHighlightRectTransform.sizeDelta = new Vector2(320f, 72f);
  }

  public void OnConfirmButtonClicked()
  {
    Action<string> onNameConfirmed = this.OnNameConfirmed;
    if (onNameConfirmed != null)
      onNameConfirmed(this._nameInputField.text);
    this.Hide();
  }

  public void ShakeConfirmButton()
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

  public override void OnHideCompleted()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    MonoSingleton<UIManager>.Instance.UnloadCultNameAssets();
  }

  public void SetTitle(string title) => this._title.text = title;
}
