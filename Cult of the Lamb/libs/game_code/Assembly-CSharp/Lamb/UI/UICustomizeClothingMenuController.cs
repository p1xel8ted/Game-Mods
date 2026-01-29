// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UICustomizeClothingMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using src.Extensions;
using src.UI;
using src.UINavigator;
using System;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public class UICustomizeClothingMenuController : UIMenuBase
{
  public const string kUpdateFollowerAnimationTrigger = "UpdateFollower";
  public const string DisplayFollowerForm = "Pig";
  public System.Action OnIndoctrinationCompleted;
  [Header("Follower Indoctrination")]
  [SerializeField]
  public ButtonHighlightController _buttonHighlightController;
  [Header("Misc")]
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [Header("Appearance")]
  [SerializeField]
  public MMButton _outfitButton;
  [SerializeField]
  public MMButton _colourButton;
  [SerializeField]
  public MMButton _variantButton;
  [SerializeField]
  public MMButton _randomiseAppearanceButton;
  [Header("Finalize")]
  [SerializeField]
  public MMButton _acceptButton;
  [SerializeField]
  public RectTransform _acceptButtonRectTransform;
  public bool _editing;
  [Header("Follower")]
  [SerializeField]
  public CustomizeClothesFollowerDisplay _infoCardController;
  public FollowerClothingType ClothingType;
  public string Variant;

  public void Show(FollowerClothingType clothingType, string variant, bool instant = false)
  {
    Debug.Log((object) "A");
    this.ClothingType = clothingType;
    this.Variant = variant;
    UIManager.PlayAudio("event:/followers/appearance_menu_appear");
    this._controlPrompts.HideCancelButton();
    this._acceptButton.OnConfirmDenied += new System.Action(this.ShakeAcceptButton);
    this._acceptButton.OnSelected += new System.Action(this.OnAcceptButtonSelected);
    this._colourButton.onClick.AddListener((UnityAction) (() =>
    {
      UICustomizeClothesMenuController_Colour menu = MonoSingleton<UIManager>.Instance.CustomizeClothesColourTemplate.Instantiate<UICustomizeClothesMenuController_Colour>();
      menu.Show(this.ClothingType, variant);
      menu.OnClothingChanged += new Action<ClothingData, int, string>(this.OnColourChanged);
      this.PushInstance<UICustomizeClothesMenuController_Colour>(menu);
    }));
    this._randomiseAppearanceButton.onClick.AddListener((UnityAction) (() => { }));
    this._acceptButton.onClick.AddListener((UnityAction) (() => this.Hide()));
    this._infoCardController.Configure(TailorManager.GetClothingData(this.ClothingType));
    this.Show(instant);
  }

  public override void OnShowStarted() => this._buttonHighlightController.SetAsRed();

  public override void SetActiveStateForMenu(bool state)
  {
    base.SetActiveStateForMenu(state);
    this.UpdateButtons();
  }

  public void UpdateButtons()
  {
  }

  public void OnOutfitChanged(ClothingData clothes)
  {
    this.ClothingType = clothes.ClothingType;
    this.UpdateFollower();
  }

  public void OnVariantChanged(int variantIndex) => this.UpdateFollower();

  public void OnColourChanged(ClothingData data, int colourIndex, string variation)
  {
    this.UpdateFollower();
  }

  public void UpdateFollower()
  {
    this._infoCardController.Configure(TailorManager.GetClothingData(this.ClothingType));
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable || this._editing)
      return;
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._acceptButton);
  }

  public override void OnHideStarted() => UIManager.PlayAudio("event:/followers/appearance_accept");

  public override void OnHideCompleted()
  {
    System.Action indoctrinationCompleted = this.OnIndoctrinationCompleted;
    if (indoctrinationCompleted != null)
      indoctrinationCompleted();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void ShakeAcceptButton()
  {
    this._acceptButtonRectTransform.DOKill();
    this._acceptButtonRectTransform.localPosition = Vector3.zero;
    this._acceptButtonRectTransform.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }

  public void OnAcceptButtonSelected()
  {
  }

  public override void OnPush() => this._buttonHighlightController.enabled = false;

  public override void DoRelease()
  {
    base.DoRelease();
    this._buttonHighlightController.enabled = true;
  }
}
