// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIWeaponsNameChangeMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using src.UI;
using src.UINavigator;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIWeaponsNameChangeMenu : UIMenuBase
{
  public const string showAnimationState = "Show";
  public Action<string> OnNameConfirmed;
  [Header("Weapons Menu")]
  [SerializeField]
  public WeaponItem _weaponItem;
  [SerializeField]
  public RectTransform _iconContainer;
  [SerializeField]
  public Image _iconImage;
  [SerializeField]
  public MMInputField _nameInputField;
  [SerializeField]
  public TextMeshProUGUI _itemLore;
  [SerializeField]
  public TextMeshProUGUI _itemDescription;
  [SerializeField]
  public CanvasGroup _weaponCardContainer;
  [SerializeField]
  public MMButton _confirmButton;
  [SerializeField]
  public RectTransform _confirmButtonRectTransform;
  [SerializeField]
  public ButtonHighlightController _buttonHighlight;
  [SerializeField]
  public RectTransform _buttonHighlightRectTransform;
  [SerializeField]
  public RectTransform _frontContainerRectTransform;
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  public Vector2 _buttonHighlightSizeDelta;
  public Vector3 _confirmButtonOrigin;
  public bool _cancellable;

  public override void Awake()
  {
    base.Awake();
    this._buttonHighlight.SetAsRed();
    this._buttonHighlightSizeDelta = this._buttonHighlightRectTransform.sizeDelta;
    this._nameInputField.OnEndedEditing += new Action<string>(this.OnEndedEditing);
    this._nameInputField.OnSelected += new System.Action(this.OnNameInputSelected);
    this._confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
    this._confirmButton.OnConfirmDenied += new System.Action(this.ShakeConfirmButton);
    this._confirmButton.OnSelected += new System.Action(this.OnConfirmButtonSelected);
    this._confirmButtonOrigin = (Vector3) this._confirmButtonRectTransform.anchoredPosition;
  }

  public void Show(EquipmentType equipmentType, bool cancellable)
  {
    this._weaponItem.Configure(equipmentType);
    this._nameInputField.text = LocalizationManager.GetTranslation($"UpgradeSystem/{equipmentType}/Name");
    this._itemLore.text = LocalizationManager.GetTranslation($"UpgradeSystem/{equipmentType}/Lore");
    this._itemDescription.text = LocalizationManager.GetTranslation($"UpgradeSystem/{equipmentType}/Description");
    this._cancellable = cancellable;
    if (!this._cancellable)
      this._controlPrompts.HideCancelButton();
    this.Show();
  }

  public override IEnumerator DoShowAnimation()
  {
    UIWeaponsNameChangeMenu weaponsNameChangeMenu = this;
    weaponsNameChangeMenu._canvasGroup.interactable = false;
    weaponsNameChangeMenu._controlPrompts.HideCancelButton();
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    weaponsNameChangeMenu.SetActiveStateForMenu(false);
    Transform originalParent = weaponsNameChangeMenu._iconContainer.parent;
    Vector2 originalScale = (Vector2) weaponsNameChangeMenu._iconContainer.localScale;
    Vector2 originalPosition = (Vector2) weaponsNameChangeMenu._iconContainer.localPosition;
    weaponsNameChangeMenu._iconContainer.SetParent((Transform) weaponsNameChangeMenu._frontContainerRectTransform);
    DOTweenModuleUI.DOFade(weaponsNameChangeMenu._iconImage, 0.0f, 0.0f);
    weaponsNameChangeMenu._iconContainer.localScale = (Vector3) (originalScale * 3f);
    weaponsNameChangeMenu._iconContainer.localPosition = (Vector3) Vector2.zero;
    yield return (object) new WaitForSecondsRealtime(0.15f);
    weaponsNameChangeMenu._iconContainer.DOScale((Vector3) (originalScale * 5f), 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    DOTweenModuleUI.DOFade(weaponsNameChangeMenu._iconImage, 1f, 0.5f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.75f);
    yield return (object) weaponsNameChangeMenu._animator.YieldForAnimation("Show");
    Vector2 endValue = (Vector2) weaponsNameChangeMenu._frontContainerRectTransform.InverseTransformPoint(originalParent.TransformPoint((Vector3) originalPosition));
    weaponsNameChangeMenu._iconContainer.DOLocalMove((Vector3) endValue, 0.65f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    weaponsNameChangeMenu._iconContainer.DOScale((Vector3) originalScale, 0.65f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    yield return (object) new WaitForSecondsRealtime(0.15f);
    weaponsNameChangeMenu._weaponCardContainer.DOFade(1f, 0.75f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.55f);
    weaponsNameChangeMenu._iconContainer.SetParent(originalParent);
    weaponsNameChangeMenu._controlPrompts.ShowAcceptButton();
    weaponsNameChangeMenu.SetActiveStateForMenu(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    weaponsNameChangeMenu.OverrideDefaultOnce(weaponsNameChangeMenu._nameInputField.Selectable);
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

  public void OnEndedEditing(string text)
  {
    this._confirmButton.Confirmable = !string.IsNullOrWhiteSpace(text);
  }

  public void OnConfirmButtonSelected()
  {
    this._confirmButton.Confirmable = !string.IsNullOrWhiteSpace(this._nameInputField.text);
    this._buttonHighlightRectTransform.sizeDelta = new Vector2(320f, 72f);
  }

  public void OnConfirmButtonClicked()
  {
    AudioManager.Instance.PlayOneShot("event:/dlc/env/legendary_weapon_repair/naming_accept", this.transform.position);
    this.Hide();
  }

  public void ShakeConfirmButton()
  {
    this._confirmButtonRectTransform.DOKill();
    this._confirmButtonRectTransform.anchoredPosition = (Vector2) this._confirmButtonOrigin;
    this._confirmButtonRectTransform.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
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
    Action<string> onNameConfirmed = this.OnNameConfirmed;
    if (onNameConfirmed != null)
      onNameConfirmed(this._nameInputField.text);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
