// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIFollowerIndoctrinationMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using src.Extensions;
using src.UI;
using src.UINavigator;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIFollowerIndoctrinationMenuController : UIMenuBase
{
  private const string kUpdateFollowerAnimationTrigger = "UpdateFollower";
  public System.Action OnIndoctrinationCompleted;
  [Header("Follower Indoctrination")]
  [SerializeField]
  private ButtonHighlightController _buttonHighlightController;
  [Header("Misc")]
  [SerializeField]
  private Material _followerUIMaterial;
  [SerializeField]
  private UIMenuControlPrompts _controlPrompts;
  [Header("Name")]
  [SerializeField]
  private MMInputField _nameInputField;
  [SerializeField]
  private MMButton _randomiseNameButton;
  [Header("Appearance")]
  [SerializeField]
  private MMButton _formButton;
  [SerializeField]
  private MMButton _colourButton;
  [SerializeField]
  private MMButton _variantButton;
  [SerializeField]
  private MMButton _randomiseAppearanceButton;
  [Header("Traits")]
  [SerializeField]
  private RectTransform _traitsContent;
  [SerializeField]
  private IndoctrinationTraitItem _traitItemTemplate;
  [Header("Finalize")]
  [SerializeField]
  private MMButton _acceptButton;
  [SerializeField]
  private RectTransform _acceptButtonRectTransform;
  [SerializeField]
  private RectTransform _emptyTextTransform;
  [Header("Twitch")]
  [SerializeField]
  private MMButton _twitchButton;
  private Follower _targetFollower;
  private RendererMaterialSwap _renderMaterialSwap;
  private Material _cachedMaterial;
  private Renderer _followerRenderer;
  private Animator _spawnLocationAnimator;
  private bool _editing;
  private List<IndoctrinationTraitItem> _traitItems = new List<IndoctrinationTraitItem>();
  private string twitchFollowerViewerID = "";
  private string twitchFollowerID = "";
  private bool _createdTwitchFollower;

  public void Show(Follower follower, bool instant = false)
  {
    if (TwitchAuthentication.IsAuthenticated && !TwitchFollowers.Deactivated)
      TwitchFollowers.SetFollowerVariations(DataManager.Instance.FollowerSkinsUnlocked);
    follower.Spine.UseDeltaTime = false;
    UIManager.PlayAudio("event:/followers/appearance_menu_appear");
    this._targetFollower = follower;
    this._nameInputField.text = this._targetFollower.Brain.Info.Name;
    this._controlPrompts.HideCancelButton();
    this._followerRenderer = this._targetFollower.Spine.GetComponent<Renderer>();
    this._cachedMaterial = this._followerRenderer.sharedMaterial;
    this._targetFollower.Spine.CustomMaterialOverride.Add(this._cachedMaterial, this._followerUIMaterial);
    this._followerRenderer.sharedMaterial.SetColor("_Color", Color.white);
    this._twitchButton.gameObject.SetActive(TwitchAuthentication.IsAuthenticated && !TwitchFollowers.Deactivated);
    if ((UnityEngine.Object) BiomeBaseManager.Instance != (UnityEngine.Object) null)
    {
      this._spawnLocationAnimator = BiomeBaseManager.Instance.RecruitSpawnLocation.transform.parent.GetComponent<Animator>();
      if ((UnityEngine.Object) this._spawnLocationAnimator != (UnityEngine.Object) null)
        this._spawnLocationAnimator.Play("Init");
    }
    this._nameInputField.OnStartedEditing += new System.Action(this.OnNameStartedEditing);
    this._nameInputField.OnEndedEditing += new Action<string>(this.OnNameEndedEditing);
    this._acceptButton.OnConfirmDenied += new System.Action(this.ShakeAcceptButton);
    this._acceptButton.OnSelected += new System.Action(this.OnAcceptButtonSelected);
    this._randomiseNameButton.onClick.AddListener(new UnityAction(this.RandomiseName));
    this._emptyTextTransform.gameObject.SetActive(false);
    this._formButton.onClick.AddListener((UnityAction) (() =>
    {
      UIAppearanceMenuController_Form menu = MonoSingleton<UIManager>.Instance.AppearanceMenuFormTemplate.Instantiate<UIAppearanceMenuController_Form>();
      menu.Show(this._targetFollower);
      menu.OnFormChanged += new Action<int>(this.OnFormChanged);
      this.PushInstance<UIAppearanceMenuController_Form>(menu);
    }));
    this._colourButton.onClick.AddListener((UnityAction) (() =>
    {
      UIAppearanceMenuController_Colour menu = MonoSingleton<UIManager>.Instance.AppearanceMenuColourTemplate.Instantiate<UIAppearanceMenuController_Colour>();
      menu.Show(this._targetFollower);
      menu.OnColourChanged += new Action<int>(this.OnColourChanged);
      this.PushInstance<UIAppearanceMenuController_Colour>(menu);
    }));
    this._variantButton.onClick.AddListener((UnityAction) (() =>
    {
      UIAppearanceMenuController_Variant menu = MonoSingleton<UIManager>.Instance.AppearanceMenuVariantTemplate.Instantiate<UIAppearanceMenuController_Variant>();
      menu.Show(this._targetFollower);
      menu.OnVariantChanged += new Action<int>(this.OnVariantChanged);
      this.PushInstance<UIAppearanceMenuController_Variant>(menu);
    }));
    this._randomiseAppearanceButton.onClick.AddListener((UnityAction) (() =>
    {
      this._targetFollower.Brain.Info.SkinColour = WorshipperData.Instance.Characters[this._targetFollower.Brain.Info.SkinCharacter].SlotAndColours.RandomIndex<WorshipperData.SlotsAndColours>();
      this._targetFollower.Brain.Info.SkinCharacter = WorshipperData.Instance.GetRandomAvailableSkin(true, true);
      this._targetFollower.Brain.Info.SkinVariation = WorshipperData.Instance.GetColourData(WorshipperData.Instance.GetSkinNameFromIndex(this._targetFollower.Brain.Info.SkinCharacter)).Skin.RandomIndex<WorshipperData.CharacterSkin>();
      this.UpdateFollower();
      this.UpdateButtons();
    }));
    this._twitchButton.onClick.AddListener((UnityAction) (() =>
    {
      UITwitchFollowerSelectOverlayController menu = MonoSingleton<UIManager>.Instance.TwitchFollowerSelectOverlayController.Instantiate<UITwitchFollowerSelectOverlayController>();
      menu.Show(this);
      this.PushInstance<UITwitchFollowerSelectOverlayController>(menu);
    }));
    this._acceptButton.onClick.AddListener((UnityAction) (() =>
    {
      this._targetFollower.Brain.Info.Name = this._nameInputField.text;
      if (!string.IsNullOrEmpty(this.twitchFollowerViewerID))
      {
        DataManager.Instance.TwitchFollowerViewerIDs.Insert(0, this.twitchFollowerViewerID);
        DataManager.Instance.TwitchFollowerIDs.Insert(0, this.twitchFollowerID);
        this.twitchFollowerViewerID = "";
        this.twitchFollowerID = "";
      }
      this.Hide();
    }));
    if (this._traitItems.Count == 0)
    {
      foreach (FollowerTrait.TraitType trait in this._targetFollower.Brain.Info.Traits)
      {
        IndoctrinationTraitItem indoctrinationTraitItem = this._traitItemTemplate.Instantiate<IndoctrinationTraitItem>((Transform) this._traitsContent);
        indoctrinationTraitItem.Configure(trait);
        this._traitItems.Add(indoctrinationTraitItem);
      }
    }
    double num = (double) this._targetFollower.SetBodyAnimation("Indoctrinate/indoctrinate-start", false);
    this._targetFollower.AddBodyAnimation("Indoctrinate/indoctrinate-loop", true, 0.0f);
    this.Show(instant);
  }

  protected override void OnShowStarted() => this._buttonHighlightController.SetAsRed();

  protected override void SetActiveStateForMenu(bool state)
  {
    base.SetActiveStateForMenu(state);
    this.UpdateButtons();
  }

  private void UpdateButtons()
  {
    WorshipperData.SkinAndData skinAndData = this.GetSkinAndData(this._targetFollower.Brain._directInfoAccess);
    this._formButton.Interactable = !skinAndData.Invariant;
    this._randomiseAppearanceButton.Interactable = !skinAndData.Invariant;
    this._variantButton.Interactable = skinAndData.Skin.Count > 1;
  }

  private void OnNameStartedEditing()
  {
    this._editing = true;
    this._emptyTextTransform.gameObject.SetActive(false);
  }

  private void OnNameEndedEditing(string text)
  {
    this._targetFollower.Brain.Info.Name = text;
    if (text != this._targetFollower.Brain.Info.Name)
    {
      double num = (double) this._targetFollower.SetBodyAnimation("Indoctrinate/indoctrinate-react", false);
      this._targetFollower.AddBodyAnimation("pray", true, 0.0f);
    }
    this._acceptButton.Confirmable = !string.IsNullOrWhiteSpace(text);
    this._emptyTextTransform.gameObject.SetActive(!this._acceptButton.Confirmable);
    this._editing = false;
  }

  private void RandomiseName()
  {
    this._nameInputField.text = FollowerInfo.GenerateName();
    this.OnNameEndedEditing(this._nameInputField.text);
  }

  private void OnFormChanged(int formIndex)
  {
    if (formIndex == this._targetFollower.Brain.Info.SkinCharacter)
      return;
    this._targetFollower.Brain.Info.SkinCharacter = formIndex;
    this._targetFollower.Brain.Info.SkinColour = 0;
    this._targetFollower.Brain.Info.SkinVariation = 0;
    this.UpdateFollower();
  }

  private void OnVariantChanged(int variantIndex)
  {
    if (variantIndex == this._targetFollower.Brain.Info.SkinVariation)
      return;
    this._targetFollower.Brain.Info.SkinVariation = variantIndex;
    this.UpdateFollower();
  }

  private void OnColourChanged(int colourIndex)
  {
    if (colourIndex == this._targetFollower.Brain.Info.SkinColour)
      return;
    this._targetFollower.Brain.Info.SkinColour = colourIndex;
    this.UpdateFollower();
  }

  private void UpdateFollower()
  {
    WorshipperData.SkinAndData skinAndData = this.GetSkinAndData(this._targetFollower.Brain._directInfoAccess);
    this._targetFollower.Brain.Info.SkinName = skinAndData.Skin[this._targetFollower.Brain.Info.SkinVariation].Skin;
    this._targetFollower.SetOutfit(this._targetFollower.Outfit.CurrentOutfit, false);
    if (!DataManager.GetFollowerSkinUnlocked(skinAndData.Skin[0].Skin) && !skinAndData.Invariant)
    {
      this._followerRenderer.sharedMaterial.DOKill();
      this._followerRenderer.sharedMaterial.SetColor("_Color", Color.black);
    }
    else
    {
      this._followerRenderer.sharedMaterial.SetColor("_Color", StaticColors.RedColor);
      this._followerRenderer.sharedMaterial.DOKill();
      this._followerRenderer.sharedMaterial.DOColor(Color.white, "_Color", 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutSine).SetDelay<TweenerCore<Color, Color, ColorOptions>>(0.2f);
      if (!((UnityEngine.Object) this._spawnLocationAnimator != (UnityEngine.Object) null))
        return;
      this._spawnLocationAnimator.SetTrigger(nameof (UpdateFollower));
    }
  }

  private WorshipperData.SkinAndData GetSkinAndData(FollowerInfo info)
  {
    if (info.SkinCharacter == -1)
      info.SkinCharacter = WorshipperData.Instance.GetSkinIndexFromName(info.SkinName);
    return WorshipperData.Instance.Characters[info.SkinCharacter];
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable || this._editing)
      return;
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._acceptButton);
  }

  protected override void OnHideStarted()
  {
    this._spawnLocationAnimator.Play("Hidden");
    UIManager.PlayAudio("event:/followers/appearance_accept");
    this._targetFollower.Spine.CustomMaterialOverride.Remove(this._cachedMaterial);
  }

  protected override void OnHideCompleted()
  {
    this._targetFollower.Spine.UseDeltaTime = true;
    System.Action indoctrinationCompleted = this.OnIndoctrinationCompleted;
    if (indoctrinationCompleted != null)
      indoctrinationCompleted();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void CreatedTwitchFollower(
    FollowerInfoSnapshot follower,
    string twitchViewerID,
    string twitchFollowerID,
    string viewerID)
  {
    if (!string.IsNullOrEmpty(this.twitchFollowerViewerID))
      return;
    this.twitchFollowerViewerID = twitchViewerID;
    this.twitchFollowerID = twitchFollowerID;
    this._nameInputField.text = follower.Name;
    this._targetFollower.Brain.Info.Name = follower.Name;
    this._targetFollower.Brain.Info.SkinName = follower.SkinName;
    this._targetFollower.Brain.Info.SkinVariation = follower.SkinVariation;
    this._targetFollower.Brain.Info.SkinColour = follower.SkinColour;
    this._targetFollower.Brain.Info.SkinCharacter = follower.SkinCharacter;
    this._targetFollower.Brain.Info.ViewerID = viewerID;
    this._createdTwitchFollower = true;
    this.OverrideDefaultOnce((Selectable) this._acceptButton);
    this.UpdateFollower();
  }

  private void ShakeAcceptButton()
  {
    this._emptyTextTransform.DOKill();
    this._acceptButtonRectTransform.DOKill();
    this._acceptButtonRectTransform.localPosition = Vector3.zero;
    this._emptyTextTransform.localPosition = Vector3.zero;
    this._acceptButtonRectTransform.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
    this._emptyTextTransform.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }

  private void OnAcceptButtonSelected()
  {
    this._acceptButton.Confirmable = !string.IsNullOrWhiteSpace(this._nameInputField.text);
  }

  protected override void OnPush() => this._buttonHighlightController.enabled = false;

  protected override void DoRelease()
  {
    base.DoRelease();
    this._buttonHighlightController.enabled = true;
    if (!this._createdTwitchFollower)
      return;
    this._formButton.Interactable = false;
    this._colourButton.Interactable = false;
    this._variantButton.Interactable = false;
    this._randomiseAppearanceButton.Interactable = false;
    this._randomiseNameButton.Interactable = false;
    this._twitchButton.Interactable = false;
    this._nameInputField.Interactable = false;
  }
}
