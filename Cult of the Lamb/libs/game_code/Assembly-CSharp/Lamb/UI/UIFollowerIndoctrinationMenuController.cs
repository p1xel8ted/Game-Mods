// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIFollowerIndoctrinationMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI.Assets;
using src.Extensions;
using src.UI;
using src.UINavigator;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIFollowerIndoctrinationMenuController : UIMenuBase
{
  public const string kUpdateFollowerAnimationTrigger = "UpdateFollower";
  public System.Action OnIndoctrinationCompleted;
  [Header("Follower Indoctrination")]
  [SerializeField]
  public ButtonHighlightController _buttonHighlightController;
  [Header("Misc")]
  [SerializeField]
  public TMP_Text _title;
  [SerializeField]
  public Material _followerUIMaterial;
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [Header("Name")]
  [SerializeField]
  public MMInputField _nameInputField;
  [SerializeField]
  public MMButton _randomiseNameButton;
  [SerializeField]
  public TextMeshProUGUI _parentText;
  [Header("Appearance")]
  [SerializeField]
  public GameObject _appearanceContainer;
  [SerializeField]
  public GameObject _appearanceHeader;
  [SerializeField]
  public MMButton _outfitButton;
  [SerializeField]
  public MMButton _formButton;
  [SerializeField]
  public MMButton _colourButton;
  [SerializeField]
  public MMButton _variantButton;
  [SerializeField]
  public MMButton _randomiseAppearanceButton;
  [Header("Traits")]
  [SerializeField]
  public RectTransform _traitsContent;
  [SerializeField]
  public IndoctrinationTraitItem _traitItemTemplate;
  [Header("Finalize")]
  [SerializeField]
  public MMButton _acceptButton;
  [SerializeField]
  public RectTransform _acceptButtonRectTransform;
  [SerializeField]
  public RectTransform _emptyTextTransform;
  [Header("Twitch")]
  [SerializeField]
  public MMButton _twitchButton;
  public Follower _targetFollower;
  public RendererMaterialSwap _renderMaterialSwap;
  public Material _cachedMaterial;
  public Renderer _followerRenderer;
  public Animator _spawnLocationAnimator;
  public bool _editing;
  public List<IndoctrinationTraitItem> _traitItems = new List<IndoctrinationTraitItem>();
  public string twitchFollowerViewerID = "";
  public string twitchFollowerID = "";
  public bool _createdTwitchFollower;
  public OriginalFollowerLookData originalFollowerLook;

  public void Show(Follower follower, OriginalFollowerLookData originalFollowerLook, bool instant = false)
  {
    if (TwitchAuthentication.IsAuthenticated && !TwitchFollowers.Deactivated)
      TwitchFollowers.SendEnabledSkins();
    GameManager.InMenu = true;
    this.originalFollowerLook = originalFollowerLook;
    follower.Spine.UseDeltaTime = false;
    UIManager.PlayAudio("event:/followers/appearance_menu_appear");
    this._targetFollower = follower;
    if (!string.IsNullOrWhiteSpace(this._targetFollower.Brain.Info.Name))
    {
      this._nameInputField.text = Regex.Replace(this._targetFollower.Brain.Info.Name, "<.*?>", string.Empty);
    }
    else
    {
      this._nameInputField.text = string.Empty;
      this._nameInputField.ForceLabelUpdate();
    }
    if (this._nameInputField.inputValidator is InputValidator inputValidator)
      inputValidator.MaxCharacters = Mathf.Max(this._targetFollower.Brain.Info.Name.Length, this._nameInputField.characterLimit);
    this._controlPrompts.HideCancelButton();
    this._followerRenderer = this._targetFollower.Spine.GetComponent<Renderer>();
    this._cachedMaterial = this._followerRenderer.sharedMaterial;
    this._targetFollower.Spine.CustomMaterialOverride.Add(this._cachedMaterial, this._followerUIMaterial);
    this._followerRenderer.sharedMaterial.SetColor("_Color", Color.white);
    this._followerUIMaterial.SetTexture("_MainTex", this._cachedMaterial.GetTexture("_MainTex"));
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
    this._parentText.gameObject.SetActive(false);
    this._formButton.onClick.AddListener((UnityAction) (() =>
    {
      UIAppearanceMenuController_Form menu = MonoSingleton<UIManager>.Instance.AppearanceMenuFormTemplate.Instantiate<UIAppearanceMenuController_Form>();
      menu.Show(this._targetFollower);
      menu.OnFormChanged += new Action<int>(this.OnFormChanged);
      this.PushInstance<UIAppearanceMenuController_Form>(menu);
    }));
    this._outfitButton.onClick.AddListener((UnityAction) (() =>
    {
      UIAppearanceMenuController_Outfit menu = MonoSingleton<UIManager>.Instance.AppearanceMenuOutfitTemplate.Instantiate<UIAppearanceMenuController_Outfit>();
      menu.Show(this._targetFollower, originalFollowerLook);
      menu.OnOutfitChanged += new Action<ClothingData, string>(this.OnOutfitChanged);
      menu.OnNecklaceChanged += new Action<InventoryItem.ITEM_TYPE>(this.OnNecklaceChanged);
      UIAppearanceMenuController_Outfit controllerOutfit = menu;
      double num1;
      controllerOutfit.OnHide = controllerOutfit.OnHide + (System.Action) (() => num1 = (double) follower.SetBodyAnimation("Indoctrinate/indoctrinate-loop", true));
      this.PushInstance<UIAppearanceMenuController_Outfit>(menu);
      this._targetFollower.Brain.Info.Outfit = FollowerOutfitType.Follower;
      double num2 = (double) follower.SetBodyAnimation("Indoctrinate/indoctrinate-clothing", true);
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
      this._targetFollower.Brain.Info.Name = LocalizeIntegration.Arabic_ReverseNonRTL(this._nameInputField.text);
      if (!string.IsNullOrEmpty(this.twitchFollowerViewerID))
      {
        DataManager.Instance.TwitchFollowerViewerIDs.Insert(0, this.twitchFollowerViewerID);
        DataManager.Instance.TwitchFollowerIDs.Insert(0, this.twitchFollowerID);
        this.twitchFollowerViewerID = "";
        this.twitchFollowerID = "";
      }
      this.SwapOutfit();
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
    if (this._targetFollower.Brain.Info.CursedState != Thought.Child || !this._targetFollower.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
    {
      double num = (double) this._targetFollower.SetBodyAnimation("Indoctrinate/indoctrinate-start", false);
      this._targetFollower.AddBodyAnimation("Indoctrinate/indoctrinate-loop", true, 0.0f);
    }
    this.OnHide = this.OnHide + new System.Action(this.ResetButtonsInteractions);
    TraitInfoCardController componentInChildren = this.GetComponentInChildren<TraitInfoCardController>();
    if ((bool) (UnityEngine.Object) componentInChildren)
      componentInChildren.FollowerBrain = follower.Brain;
    this.Show(instant);
  }

  public override void OnShowStarted()
  {
    this._buttonHighlightController.SetAsRed();
    if (string.IsNullOrEmpty(this._targetFollower.Brain._directInfoAccess.ViewerID))
      return;
    this._createdTwitchFollower = true;
    this.ResetButtonsInteractions();
  }

  public override void SetActiveStateForMenu(bool state)
  {
    base.SetActiveStateForMenu(state);
    this.UpdateButtons();
  }

  public void UpdateButtons()
  {
    WorshipperData.SkinAndData skinAndData = this.GetSkinAndData(this._targetFollower.Brain._directInfoAccess);
    bool flag1 = false;
    if (skinAndData.Skin[0].Skin == "Mushroom" && !DataManager.GetFollowerSkinUnlocked("Mushroom") || this._targetFollower.Brain._directInfoAccess.ID == 99998)
      flag1 = true;
    bool flag2 = false;
    bool flag3 = false;
    bool flag4 = false;
    if (this._targetFollower.Brain._directInfoAccess.Traits.Contains(FollowerTrait.TraitType.Mutated))
      flag1 = true;
    if (skinAndData.Skin[0].Skin == "Bug" && !DataManager.GetFollowerSkinUnlocked("Bug"))
    {
      flag2 = true;
      flag3 = true;
    }
    this._formButton.Interactable = !skinAndData.Invariant && !flag1 && !flag2;
    this._randomiseAppearanceButton.Interactable = !skinAndData.Invariant && !flag1 && !flag2;
    this._outfitButton.Interactable = !flag1;
    this._outfitButton.gameObject.SetActive(DataManager.Instance.TailorEnabled);
    this._variantButton.Interactable = skinAndData.Skin.Count > 1 && skinAndData.Skin[0].Skin != "Abomination" && !flag1 && !flag3;
    this._colourButton.Interactable = !skinAndData.LockColor && !flag1 && !flag4;
    if (this._targetFollower.Brain.Info.CursedState != Thought.Child || this._targetFollower.Brain.Info.Age >= 18)
      return;
    this._appearanceContainer.gameObject.SetActive(false);
    this._appearanceHeader.gameObject.SetActive(false);
    this._twitchButton.gameObject.SetActive(false);
    this._title.text = LocalizationManager.GetTranslation("UI/Generic/Follower");
    if (this._targetFollower.Brain._directInfoAccess.Parents.Count >= 2)
    {
      FollowerInfo directInfoAccess = this._targetFollower.Brain._directInfoAccess;
      string str1 = $"<color=#FFD201>{directInfoAccess.Parent1Name}</color>";
      string str2 = $"<color=#FFD201>{directInfoAccess.Parent2Name}</color>";
      string str3 = string.Format(LocalizationManager.GetTranslation("UI/MatingTent/IsChild"), (object) "", (object) $"{str1} + {str2}").Substring(30);
      string str4 = $"<color=#FFD201>{LocalizationManager.GetTranslation("UI/Generic/Follower")}</color>{str3}<br><br>";
      this._parentText.gameObject.SetActive(true);
      this._parentText.text = str4;
    }
    else
      this._parentText.gameObject.SetActive(false);
    this.UpdateFollower();
  }

  public void OnNameStartedEditing() => this._editing = true;

  public void OnNameEndedEditing(string text)
  {
    this._targetFollower.Brain.Info.Name = text;
    if (text != this._targetFollower.Brain.Info.Name)
    {
      double num = (double) this._targetFollower.SetBodyAnimation("Indoctrinate/indoctrinate-react", false);
      this._targetFollower.AddBodyAnimation("pray", true, 0.0f);
    }
    this._acceptButton.Confirmable = !string.IsNullOrWhiteSpace(text);
    this._editing = false;
  }

  public void RandomiseName()
  {
    this._nameInputField.text = FollowerInfo.GenerateName();
    this.OnNameEndedEditing(this._nameInputField.text);
  }

  public void OnFormChanged(int formIndex)
  {
    if (formIndex == this._targetFollower.Brain.Info.SkinCharacter)
      return;
    this._targetFollower.Brain.Info.SkinCharacter = formIndex;
    this._targetFollower.Brain.Info.SkinColour = 0;
    this._targetFollower.Brain.Info.SkinVariation = 0;
    this.UpdateFollower();
  }

  public void OnNecklaceChanged(InventoryItem.ITEM_TYPE necklace)
  {
    if (this._targetFollower.Brain.Info.Necklace == necklace)
      return;
    this._targetFollower.Brain.Info.Necklace = necklace;
    this.UpdateFollower();
  }

  public void OnOutfitChanged(ClothingData clothes, string variant)
  {
    this._targetFollower.Brain.AssignClothing((UnityEngine.Object) clothes != (UnityEngine.Object) null ? clothes.ClothingType : FollowerClothingType.None, variant);
    this.UpdateFollower();
  }

  public void OnVariantChanged(int variantIndex)
  {
    if (variantIndex == this._targetFollower.Brain.Info.SkinVariation)
      return;
    this._targetFollower.Brain.Info.SkinVariation = variantIndex;
    this.UpdateFollower();
  }

  public void OnColourChanged(int colourIndex)
  {
    if (colourIndex == this._targetFollower.Brain.Info.SkinColour)
      return;
    this._targetFollower.Brain.Info.SkinColour = colourIndex;
    this.UpdateFollower();
  }

  public void UpdateFollower()
  {
    WorshipperData.SkinAndData skinAndData = this.GetSkinAndData(this._targetFollower.Brain._directInfoAccess);
    this._targetFollower.Brain.Info.SkinName = skinAndData.Skin[this._targetFollower.Brain.Info.SkinVariation].Skin;
    FollowerOutfitType outfit = this._targetFollower.Brain.Info.Outfit;
    if (outfit != FollowerOutfitType.Old && this._targetFollower.Brain.Info.Special != FollowerSpecialType.SozoOld)
    {
      FollowerOutfitType outfitFromCursedState = FollowerBrain.GetOutfitFromCursedState(this._targetFollower.Brain._directInfoAccess);
      if (outfitFromCursedState != FollowerOutfitType.None)
        outfit = outfitFromCursedState;
    }
    if (outfit == FollowerOutfitType.Rags && FollowerBrainStats.IsNudism)
      outfit = FollowerOutfitType.Naked;
    if (this._targetFollower.Brain.Info.Clothing != FollowerClothingType.None && outfit == FollowerOutfitType.Rags)
    {
      outfit = FollowerOutfitType.Follower;
      this._targetFollower.Brain.Info.Outfit = FollowerOutfitType.Follower;
    }
    FollowerBrain.SetFollowerCostume(this._targetFollower.Spine.Skeleton, this._targetFollower.Brain.Info.XPLevel, this._targetFollower.Brain.Info.SkinName, this._targetFollower.Brain.Info.SkinColour, outfit, this._targetFollower.Brain.Info.Hat, this._targetFollower.Brain.Info.Clothing, this._targetFollower.Brain.Info.Customisation, this._targetFollower.Brain.Info.Special, this._targetFollower.Brain.Info.Necklace, this._targetFollower.Brain.Info.ClothingVariant, this._targetFollower.Brain._directInfoAccess);
    if (this._targetFollower.Brain.Info.CursedState == Thought.Child)
      return;
    if (!DataManager.GetFollowerSkinUnlocked(skinAndData.Skin[0].Skin) && !skinAndData.Invariant && !DataManager.Instance.AvailableBeforeUnlockSkins.Contains(skinAndData.Skin[0].Skin))
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

  public WorshipperData.SkinAndData GetSkinAndData(FollowerInfo info)
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

  public override void OnHideStarted()
  {
    this._spawnLocationAnimator.Play("Hidden");
    UIManager.PlayAudio("event:/followers/appearance_accept");
    this._targetFollower.Spine.CustomMaterialOverride.Remove(this._cachedMaterial);
  }

  public override void OnHideCompleted()
  {
    GameManager.InMenu = false;
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
    this._targetFollower.Brain.Info.Clothing = follower.Clothing;
    this._targetFollower.Brain.Info.ClothingVariant = follower.ClothingVariant;
    this._createdTwitchFollower = true;
    this.OverrideDefaultOnce((Selectable) this._acceptButton);
    this.UpdateFollower();
  }

  public void ShakeAcceptButton()
  {
    this._emptyTextTransform.DOKill();
    this._acceptButtonRectTransform.DOKill();
    this._acceptButtonRectTransform.localPosition = Vector3.zero;
    this._emptyTextTransform.localPosition = Vector3.zero;
    this._acceptButtonRectTransform.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
    this._emptyTextTransform.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }

  public void OnAcceptButtonSelected()
  {
    this._acceptButton.Confirmable = !string.IsNullOrWhiteSpace(this._nameInputField.text);
  }

  public override void OnPush() => this._buttonHighlightController.enabled = false;

  public void ResetButtonsInteractions()
  {
    this._buttonHighlightController.enabled = true;
    if (!this._createdTwitchFollower)
      return;
    this._formButton.Interactable = false;
    this._outfitButton.Interactable = false;
    this._colourButton.Interactable = false;
    this._variantButton.Interactable = false;
    this._randomiseAppearanceButton.Interactable = false;
    this._randomiseNameButton.Interactable = false;
    this._twitchButton.Interactable = false;
    this._nameInputField.Interactable = false;
  }

  public void SwapOutfit()
  {
    if (this._targetFollower.Brain.Info.Clothing != this.originalFollowerLook.Clothing)
    {
      ClothingData clothingData = TailorManager.GetClothingData(this._targetFollower.Brain.Info.Clothing);
      if (clothingData.SpecialClothing)
        TailorManager.RemoveClothingFromDeadFollower(clothingData.ClothingType);
      if (TailorManager.GetCraftedCount(this._targetFollower.Brain.Info.Clothing) > 0)
        TailorManager.RemoveClothingFromTailor(this._targetFollower.Brain.Info.Clothing);
      else
        TailorManager.RemoveClothingFromFollower(this._targetFollower.Brain.Info.Clothing, this._targetFollower.Brain);
      TailorManager.AddClothingToTailor(this.originalFollowerLook.Clothing, this._targetFollower.Brain.Info.ClothingPreviousVariant);
      foreach (Follower follower in Follower.Followers)
        FollowerBrain.SetFollowerCostume(follower.Spine.Skeleton, follower.Brain._directInfoAccess, forceUpdate: true);
    }
    if (this._targetFollower.Brain.Info.Necklace == this.originalFollowerLook.Necklace)
      return;
    if (this._targetFollower.Brain.Info.Necklace == InventoryItem.ITEM_TYPE.NONE)
    {
      if (this.originalFollowerLook.Necklace != InventoryItem.ITEM_TYPE.NONE)
        CultFaithManager.AddThought(Thought.Cult_RemovedFollowerNecklace, this._targetFollower.Brain.Info.ID);
    }
    else
      Inventory.ChangeItemQuantity(this._targetFollower.Brain.Info.Necklace, -1);
    if (this.originalFollowerLook.Necklace != InventoryItem.ITEM_TYPE.NONE)
      Inventory.ChangeItemQuantity(this.originalFollowerLook.Necklace, 1);
    if (this.originalFollowerLook.Necklace == InventoryItem.ITEM_TYPE.Necklace_Gold_Skull && 666 != this._targetFollower.Brain.Info.ID && this._targetFollower.Brain.HasTrait(FollowerTrait.TraitType.Immortal))
      this._targetFollower.Brain.RemoveTrait(FollowerTrait.TraitType.Immortal);
    if (this._targetFollower.Brain._directInfoAccess.Necklace != InventoryItem.ITEM_TYPE.Necklace_Gold_Skull || this._targetFollower.Brain.HasTrait(FollowerTrait.TraitType.Immortal))
      return;
    this._targetFollower.Brain.AddTrait(FollowerTrait.TraitType.Immortal);
  }
}
