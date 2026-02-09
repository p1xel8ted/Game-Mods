// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.TailorInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Spine.Unity;
using TMPro;
using UnityEngine;
using WebSocketSharp;

#nullable disable
namespace src.UI.InfoCards;

public class TailorInfoCard : UIInfoCardBase<ClothingData>
{
  [SerializeField]
  public SkeletonGraphic _spine;
  [SerializeField]
  public TextMeshProUGUI _headerText;
  [SerializeField]
  public TextMeshProUGUI _descriptionText;
  [SerializeField]
  public TextMeshProUGUI _mealEffects;
  [SerializeField]
  public GameObject _ingredientsObject;
  [SerializeField]
  public TextMeshProUGUI _ingredients;
  [SerializeField]
  public GameObject _singleBuild;
  [SerializeField]
  public GameObject _deafultRobe;
  [SerializeField]
  public GameObject _craftBeforeCustomise;
  [SerializeField]
  public GameObject _craftBeforeAssign;
  [SerializeField]
  public GameObject _alreadyAssigned;
  [SerializeField]
  public GameObject _lockedObject;
  [SerializeField]
  public GameObject _requiresDLCObject;
  [SerializeField]
  public TextMeshProUGUI _requiresDLC;
  [SerializeField]
  public GameObject _requiresDLCAlert;
  public TailorItem.InMenu _inMenu;
  public FollowerBrain targetFollower;
  public ClothingData clothingData;
  public string variant;
  public Color startColorDLC = Color.magenta;
  public bool hasCrafted;
  public bool _checkIfCrafted;
  public string skin;

  public void SetMenu(TailorItem.InMenu inMenu)
  {
    Debug.Log((object) ("Set Menu: " + inMenu.ToString()));
    this._inMenu = inMenu;
    if (this._inMenu == TailorItem.InMenu.Customise)
    {
      this.HideDetails();
      this.CheckIfCrafted(true);
      this._craftBeforeAssign.gameObject.SetActive(false);
    }
    else if (this._inMenu == TailorItem.InMenu.Assign)
    {
      this.HideDetails();
      this.CheckIfCrafted(false);
      if (!TailorManager.GetClothingAvailable(this.clothingData.ClothingType))
      {
        this._craftBeforeAssign.gameObject.SetActive(false);
      }
      else
      {
        this.hasCrafted = false;
        if (this.clothingData.ClothingType == FollowerClothingType.None)
        {
          this.hasCrafted = true;
        }
        else
        {
          foreach (FollowerClothingType followerClothingType in DataManager.Instance.clothesCrafted)
          {
            if (followerClothingType == this.clothingData.ClothingType)
            {
              this.hasCrafted = true;
              return;
            }
          }
        }
        if (!this.hasCrafted)
          this._craftBeforeAssign.gameObject.SetActive(true);
        else
          this._craftBeforeAssign.gameObject.SetActive(false);
      }
    }
    else
    {
      this.CheckIfCrafted(false);
      this.ShowDetails();
      this._craftBeforeAssign.gameObject.SetActive(false);
    }
  }

  public void Configure(
    ClothingData clothingData,
    Structures_Tailor tailor,
    string variant,
    TailorItem.InMenu inMenu = TailorItem.InMenu.Craft,
    bool showSpecialOutfitFollower = true)
  {
    this._alreadyAssigned.gameObject.SetActive(false);
    this._lockedObject.gameObject.SetActive(false);
    this._inMenu = inMenu;
    this._headerText.text = TailorManager.LocalizedName(clothingData.ClothingType);
    this._descriptionText.text = TailorManager.LocalizedDescription(clothingData.ClothingType);
    this._singleBuild.gameObject.SetActive(clothingData.SpecialClothing);
    this._deafultRobe.gameObject.SetActive(clothingData.ClothingType == FollowerClothingType.None && inMenu == TailorItem.InMenu.Craft);
    this._craftBeforeCustomise.gameObject.SetActive(false);
    this._craftBeforeAssign.gameObject.SetActive(false);
    this._ingredients.gameObject.SetActive(true);
    this._mealEffects.gameObject.SetActive(true);
    this._mealEffects.transform.parent.gameObject.SetActive(TailorManager.GetEffectTypes(clothingData.ClothingType).Length != 0 || clothingData.ProtectionType != 0);
    this._mealEffects.text = "";
    foreach (CookingData.MealEffect effectType in TailorManager.GetEffectTypes(clothingData.ClothingType))
    {
      this._mealEffects.text += string.Format(LocalizationManager.GetTranslation($"CookingData/{effectType.MealEffectType}/Description"), (object) effectType.Chance);
      this._mealEffects.text += "\n";
    }
    if (clothingData.ProtectionType != FollowerProtectionType.None)
    {
      this._mealEffects.text += LocalizationManager.GetTranslation($"Clothing/FollowerProtectionType/{clothingData.ProtectionType}").Colour(StaticColors.YellowColorHex);
      this._mealEffects.text += "\n";
    }
    StructuresData.ItemCost[] itemCostArray = new StructuresData.ItemCost[clothingData.Cost.Length];
    for (int index = 0; index < itemCostArray.Length; ++index)
      itemCostArray[index] = new StructuresData.ItemCost(clothingData.Cost[index].ItemType, clothingData.Cost[index].Cost);
    this._ingredients.text = itemCostArray.Length != 0 ? $"<color=#FD1D03><i>{ScriptLocalization.Interactions.Ingredients}</i></color><br><br>{StructuresData.ItemCost.GetCostStringWithQuantity(itemCostArray)}" : "";
    this.variant = variant;
    this.clothingData = clothingData;
    this.SetCostume(showSpecialOutfitFollower);
    if (!TailorManager.GetClothingAvailable(clothingData.ClothingType))
      this.setLocked();
    if (clothingData.SpecialClothing && !TailorManager.IsClothingAvailable(clothingData.ClothingType))
      this._ingredientsObject.gameObject.SetActive(false);
    this.CheckIfCrafted(this._checkIfCrafted);
    this.SetMenu(this._inMenu);
    this.CheckDLC();
  }

  public void CheckDLC()
  {
    CanvasGroup component = this._lockedObject.gameObject.GetComponent<CanvasGroup>();
    if (this.startColorDLC == Color.magenta)
      this.startColorDLC = this._requiresDLC.color;
    if (this.clothingData.IsDLC)
    {
      this._requiresDLC.color = this.startColorDLC;
      this._requiresDLCAlert.gameObject.SetActive(true);
      this._requiresDLCObject.gameObject.SetActive(true);
      string str1 = ScriptLocalization.Interactions.Requires + " ";
      string str2 = "";
      if (this.clothingData.IsCultist)
      {
        str2 = ScriptLocalization.UI_DLC.CultistEdition;
        if (DataManager.Instance.DLC_Cultist_Pack)
          str1 = "";
      }
      else if (this.clothingData.IsHeretic)
      {
        str2 = ScriptLocalization.UI_DLC.HereticEdition;
        if (DataManager.Instance.DLC_Heretic_Pack)
          str1 = "";
      }
      else if (this.clothingData.IsSinful)
      {
        str2 = ScriptLocalization.UI_DLC.SinfulEdition;
        if (DataManager.Instance.DLC_Sinful_Pack)
          str1 = "";
      }
      else if (this.clothingData.IsPilgrim)
      {
        str2 = ScriptLocalization.UI_DLC.PilgrimPack;
        if (DataManager.Instance.DLC_Pilgrim_Pack)
          str1 = "";
      }
      if (str1 == "")
      {
        this._requiresDLC.color = StaticColors.GreenColor;
        this._requiresDLCAlert.gameObject.SetActive(false);
      }
      this._requiresDLC.text = str1 + str2;
      component.DOKill();
      component.DOFade(0.8f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    }
    else
    {
      this._requiresDLCObject.gameObject.SetActive(false);
      if ((double) component.alpha == 1.0)
        return;
      component.DOKill();
      component.DOFade(0.8f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    }
  }

  public void CheckIfCrafted(bool checkIfCrafted)
  {
    this._checkIfCrafted = checkIfCrafted;
    if (this.clothingData.ClothingType == FollowerClothingType.None)
      return;
    if (this._checkIfCrafted)
    {
      this.hasCrafted = false;
      foreach (FollowerClothingType followerClothingType in DataManager.Instance.clothesCrafted)
      {
        if (followerClothingType == this.clothingData.ClothingType)
        {
          this.hasCrafted = true;
          return;
        }
      }
      if (!this.hasCrafted)
        this._craftBeforeCustomise.gameObject.SetActive(true);
      else
        this._craftBeforeCustomise.gameObject.SetActive(false);
    }
    else
      this._craftBeforeCustomise.gameObject.SetActive(false);
  }

  public void HideDetails()
  {
    Debug.Log((object) "Hide Details");
    this._ingredientsObject.gameObject.SetActive(false);
    this._singleBuild.gameObject.SetActive(false);
    this._deafultRobe.gameObject.SetActive(false);
  }

  public void ShowDetails()
  {
    Debug.Log((object) "Show Details");
    if (this.clothingData.ClothingType != FollowerClothingType.None && this._inMenu == TailorItem.InMenu.Craft)
      this._ingredientsObject.gameObject.SetActive(true);
    else
      this._ingredientsObject.gameObject.SetActive(false);
    if (!TailorManager.IsSpecialClothing(this.clothingData.ClothingType))
      return;
    this._singleBuild.gameObject.SetActive(true);
  }

  public void setLocked() => this._lockedObject.gameObject.SetActive(true);

  public void ShakeInfo()
  {
    CanvasGroup component1 = this._deafultRobe.GetComponent<CanvasGroup>();
    component1.DOKill();
    component1.alpha = 0.0f;
    component1.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    if (this._inMenu == TailorItem.InMenu.Craft)
      return;
    CanvasGroup component2 = this._singleBuild.GetComponent<CanvasGroup>();
    component2.DOKill();
    component2.alpha = 0.0f;
    component2.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
  }

  public override void Configure(ClothingData clothingData)
  {
    this.clothingData = clothingData;
    this.SetCostume();
    this._headerText.text = TailorManager.LocalizedName(clothingData.ClothingType);
    this._descriptionText.text = TailorManager.LocalizedDescription(clothingData.ClothingType);
    this._singleBuild.gameObject.SetActive(false);
    this._deafultRobe.gameObject.SetActive(false);
    this._mealEffects.gameObject.SetActive(false);
    this._ingredients.gameObject.SetActive(false);
  }

  public void SetCostume(bool showSpecialOutfitFollower = true)
  {
    this.skin = "";
    int skinColor = 0;
    FollowerOutfitType outfit = FollowerOutfitType.None;
    FollowerSpecialType special = FollowerSpecialType.None;
    FollowerHatType hat = FollowerHatType.None;
    InventoryItem.ITEM_TYPE necklace = InventoryItem.ITEM_TYPE.NONE;
    if (((this.clothingData.ClothingType == FollowerClothingType.None ? 0 : (this.clothingData.SpecialClothing ? 1 : 0)) & (showSpecialOutfitFollower ? 1 : 0)) != 0)
    {
      FollowerBrain followerWearingOutfit = TailorManager.GetFollowerWearingOutfit(this.clothingData.ClothingType);
      if (followerWearingOutfit != null)
      {
        this.skin = followerWearingOutfit.Info.SkinName;
        skinColor = followerWearingOutfit.Info.SkinColour;
        hat = followerWearingOutfit.Info.Hat;
        string str = string.Format(LocalizationManager.GetTranslation("UI/OutfitMenu/AssignedTo"), (object) followerWearingOutfit.Info.Name);
        TextMeshProUGUI descriptionText = this._descriptionText;
        descriptionText.text = $"{descriptionText.text}\n{str}";
        outfit = FollowerBrain.GetOutfitFromCursedState(followerWearingOutfit._directInfoAccess);
      }
    }
    if (this.skin.IsNullOrEmpty() && this.targetFollower == null && DataManager.Instance.Followers.Count > 0)
    {
      FollowerBrain brain = FollowerBrain.GetOrCreateBrain(DataManager.Instance.Followers[0]);
      this.skin = brain.Info.SkinName;
      skinColor = brain.Info.SkinColour;
      special = brain.Info.Special;
      hat = brain.Info.Hat;
      outfit = FollowerBrain.GetOutfitFromCursedState(brain._directInfoAccess);
    }
    if (this.targetFollower != null && this.skin.IsNullOrEmpty())
    {
      this.skin = this.targetFollower.Info.SkinName;
      skinColor = this.targetFollower.Info.SkinColour;
      special = this.targetFollower.Info.Special;
      hat = this.targetFollower.Info.Hat;
      outfit = FollowerBrain.GetOutfitFromCursedState(this.targetFollower._directInfoAccess);
    }
    this._spine.transform.localScale = Vector3.one * 1.2785f;
    if (this.targetFollower != null && this.targetFollower.Info.CursedState == Thought.Child)
      this._spine.transform.localScale = Vector3.one;
    FollowerBrain.SetFollowerCostume(this._spine.Skeleton, 0, this.skin, skinColor, outfit, hat, this.clothingData.ClothingType, FollowerCustomisationType.None, special, necklace, this.variant);
    this._deafultRobe.gameObject.SetActive(this.clothingData.ClothingType == FollowerClothingType.None && this._inMenu == TailorItem.InMenu.Craft);
  }

  public bool CheckIfAlreadyAssigned(FollowerInfo follower)
  {
    if (follower.Clothing == this.clothingData.ClothingType && follower.ClothingVariant == this.variant)
    {
      this._alreadyAssigned.gameObject.SetActive(true);
      return true;
    }
    this._alreadyAssigned.gameObject.SetActive(false);
    return false;
  }

  public void SetTargetFollower(FollowerBrain follower)
  {
    this.targetFollower = follower;
    this.SetCostume();
  }

  public void ResetCostume()
  {
    this.targetFollower = (FollowerBrain) null;
    this.SetCostume();
  }
}
