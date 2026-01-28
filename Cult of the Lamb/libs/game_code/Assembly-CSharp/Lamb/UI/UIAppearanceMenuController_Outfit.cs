// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIAppearanceMenuController_Outfit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI.Assets;
using src.Extensions;
using src.UINavigator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIAppearanceMenuController_Outfit : UIMenuBase
{
  public Action<ClothingData, string> OnOutfitChanged;
  [SerializeField]
  public RectTransform _outfitContent;
  [SerializeField]
  public RectTransform _specialOutfitContent;
  [SerializeField]
  public RectTransform _specialOutfitHeader;
  [SerializeField]
  public RectTransform _dlcOutfitContent;
  [SerializeField]
  public RectTransform _dlcOutfitHeader;
  [SerializeField]
  public RectTransform _majorDlcOutfitContent;
  [SerializeField]
  public RectTransform _majorDlcOutfitHeader;
  [SerializeField]
  public IndoctrinationOutfitItem _removeOutfitButton;
  [SerializeField]
  public CanvasGroup necklaceInfoCardCanvasGroup;
  [SerializeField]
  public TMP_Text necklaceInfoCardTitle;
  [SerializeField]
  public TMP_Text necklaceInfoCardDescription;
  [SerializeField]
  public Image necklaceInfoCardIcon;
  [SerializeField]
  public InventoryIconMapping iconMapping;
  [SerializeField]
  public GameObject customisePrompt;
  [SerializeField]
  public TextMeshProUGUI unlockedClothesText;
  [SerializeField]
  public TextMeshProUGUI unlockedSpecialClothesText;
  [SerializeField]
  public TextMeshProUGUI unlockedNecklacesText;
  public List<IndoctrinationOutfitItem> _outfitItems = new List<IndoctrinationOutfitItem>();
  public List<IndoctrinationOutfitItem> _specialOutfitItems = new List<IndoctrinationOutfitItem>();
  public List<IndoctrinationOutfitItem> _dlcOutfitItems = new List<IndoctrinationOutfitItem>();
  public List<IndoctrinationOutfitItem> _majorDlcOutfitItems = new List<IndoctrinationOutfitItem>();
  public int _cacheOutfit;
  public Action<InventoryItem.ITEM_TYPE> OnNecklaceChanged;
  [SerializeField]
  public RectTransform _necklaceHeader;
  [SerializeField]
  public RectTransform _necklaceContent;
  [SerializeField]
  public necklaceFormItem _removeNecklaceButton;
  public InventoryItem.ITEM_TYPE cacheNecklace;
  public InventoryItem.ITEM_TYPE currentNecklace;
  public StructuresData.ClothingStruct cachedClothingType;
  public StructuresData.ClothingStruct currentClothingType;
  public List<necklaceFormItem> _necklaceItems = new List<necklaceFormItem>();
  public Follower follower;
  public OriginalFollowerLookData originalFollowerLook;

  public override void OnShowStarted()
  {
    this.PopulateNecklaceSection();
    List<ClothingData> unlockedSpecialClothing = TailorManager.GetUnlockedSpecialClothing();
    if (unlockedSpecialClothing.Count > 0)
    {
      this._specialOutfitItems.AddRange((IEnumerable<IndoctrinationOutfitItem>) this.Populate(unlockedSpecialClothing, this._specialOutfitContent, this.follower, OutfitItemList: this._specialOutfitItems));
    }
    else
    {
      this._specialOutfitContent.gameObject.SetActive(false);
      this._specialOutfitHeader.gameObject.SetActive(false);
    }
    List<ClothingData> unlockedDlcClothing = TailorManager.GetUnlockedDLCClothing();
    if (unlockedDlcClothing.Count > 0)
    {
      this._dlcOutfitItems.AddRange((IEnumerable<IndoctrinationOutfitItem>) this.Populate(unlockedDlcClothing, this._dlcOutfitContent, this.follower, OutfitItemList: this._dlcOutfitItems));
    }
    else
    {
      this._dlcOutfitContent.gameObject.SetActive(false);
      this._dlcOutfitHeader.gameObject.SetActive(false);
    }
    List<ClothingData> majorDlcClothing = TailorManager.GetMajorDLCClothing();
    if (majorDlcClothing.Count > 0)
    {
      this._majorDlcOutfitItems.AddRange((IEnumerable<IndoctrinationOutfitItem>) this.Populate(majorDlcClothing, this._majorDlcOutfitContent, this.follower, OutfitItemList: this._majorDlcOutfitItems));
    }
    else
    {
      this._majorDlcOutfitContent.gameObject.SetActive(false);
      this._majorDlcOutfitHeader.gameObject.SetActive(false);
    }
    this._outfitItems.AddRange((IEnumerable<IndoctrinationOutfitItem>) this.Populate(TailorManager.GetAvailableWeatherClothing(), this._outfitContent, this.follower, OutfitItemList: this._outfitItems, isSpineVisible: false));
    this._outfitItems.Add(this._removeOutfitButton);
    TextMeshProUGUI unlockedClothesText = this.unlockedClothesText;
    int num1 = TailorManager.GetUnlockedClothingCount(true, false, false, false);
    string str1 = num1.ToString();
    num1 = TailorManager.GetClothingCount(true, false, false, false);
    string str2 = num1.ToString();
    string str3 = $"{str1}/{str2}";
    unlockedClothesText.text = str3;
    TextMeshProUGUI specialClothesText = this.unlockedSpecialClothesText;
    num1 = TailorManager.GetUnlockedClothingCount(false, true, false, false);
    string str4 = num1.ToString();
    num1 = TailorManager.GetClothingCount(false, true, false, false);
    string str5 = num1.ToString();
    string str6 = $"{str4}/{str5}";
    specialClothesText.text = str6;
    int num2 = 0;
    foreach (necklaceFormItem necklaceItem in this._necklaceItems)
    {
      if (necklaceItem.Button.Confirmable && (UnityEngine.Object) necklaceItem != (UnityEngine.Object) this._removeNecklaceButton)
        ++num2;
    }
    this.unlockedNecklacesText.text = $"{num2.ToString()}/{DataManager.AllNecklaces.Count.ToString()}";
  }

  public void PopulateNecklaceSection()
  {
    if (this.follower.Brain.Info.CursedState != Thought.OldAge)
    {
      this.necklaceInfoCardCanvasGroup.alpha = 0.0f;
      List<InventoryItem.ITEM_TYPE> allNecklaces = DataManager.AllNecklaces;
      if (!GameManager.AuthenticateMajorDLC())
      {
        allNecklaces.Remove(InventoryItem.ITEM_TYPE.Necklace_Deaths_Door);
        allNecklaces.Remove(InventoryItem.ITEM_TYPE.Necklace_Winter);
        allNecklaces.Remove(InventoryItem.ITEM_TYPE.Necklace_Frozen);
        allNecklaces.Remove(InventoryItem.ITEM_TYPE.Necklace_Weird);
        allNecklaces.Remove(InventoryItem.ITEM_TYPE.Necklace_Targeted);
      }
      this._necklaceItems.AddRange((IEnumerable<necklaceFormItem>) this.Populate(allNecklaces, this._necklaceContent));
    }
    else
    {
      this._necklaceContent.gameObject.SetActive(false);
      if (!((UnityEngine.Object) this._necklaceHeader != (UnityEngine.Object) null))
        return;
      this._necklaceHeader.gameObject.SetActive(false);
    }
  }

  public void Show(Follower follower, OriginalFollowerLookData originalFollowerLook)
  {
    this.follower = follower;
    this.originalFollowerLook = originalFollowerLook;
    this.cachedClothingType = new StructuresData.ClothingStruct(follower.Brain.Info.Clothing, follower.Brain.Info.ClothingVariant);
    this.cacheNecklace = follower.Brain.Info.Necklace;
    this.Show();
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.RevertOutfit();
    this.RevertNecklace();
    this.Hide();
  }

  public void RevertOutfit()
  {
    Action<ClothingData, string> onOutfitChanged = this.OnOutfitChanged;
    if (onOutfitChanged == null)
      return;
    onOutfitChanged(TailorManager.GetClothingData(this.cachedClothingType.ClothingType), this.cachedClothingType.Variant);
  }

  public void RevertNecklace()
  {
    Action<InventoryItem.ITEM_TYPE> onNecklaceChanged = this.OnNecklaceChanged;
    if (onNecklaceChanged == null)
      return;
    onNecklaceChanged(this.cacheNecklace);
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  public List<necklaceFormItem> Populate(
    List<InventoryItem.ITEM_TYPE> types,
    RectTransform contentContainer,
    string[] order = null)
  {
    necklaceFormItem necklaceFormItem1 = (necklaceFormItem) null;
    List<necklaceFormItem> necklaceFormItemList = new List<necklaceFormItem>();
    foreach (InventoryItem.ITEM_TYPE type in types)
    {
      necklaceFormItem necklaceFormItem2 = MonoSingleton<UIManager>.Instance.FollowerFormNecklaceTemplate.Spawn<necklaceFormItem>((Transform) contentContainer);
      necklaceFormItem2.transform.localScale = Vector3.one;
      necklaceFormItem2.Configure((WorshipperData.SkinAndData) null, type, this.originalFollowerLook.Necklace == type);
      necklaceFormItem necklaceFormItem3 = necklaceFormItem2;
      necklaceFormItem3.OnItemSelected = necklaceFormItem3.OnItemSelected + new Action<necklaceFormItem>(this.OnNecklaceSelected);
      necklaceFormItem necklaceFormItem4 = necklaceFormItem2;
      necklaceFormItem4.OnItemHighlighted = necklaceFormItem4.OnItemHighlighted + new Action<necklaceFormItem>(this.OnNecklaceHighlighted);
      this._necklaceItems.Add(necklaceFormItem2);
      if (type == this.cacheNecklace)
        necklaceFormItem1 = necklaceFormItem2;
    }
    necklaceFormItem removeNecklaceButton1 = this._removeNecklaceButton;
    removeNecklaceButton1.OnItemHighlighted = removeNecklaceButton1.OnItemHighlighted + new Action<necklaceFormItem>(this.OnNecklaceHighlighted);
    necklaceFormItem removeNecklaceButton2 = this._removeNecklaceButton;
    removeNecklaceButton2.OnItemSelected = removeNecklaceButton2.OnItemSelected + new Action<necklaceFormItem>(this.OnNecklaceSelected);
    this._necklaceItems.Add(this._removeNecklaceButton);
    if ((UnityEngine.Object) necklaceFormItem1 != (UnityEngine.Object) null)
      necklaceFormItem1.SetAsSelected();
    return necklaceFormItemList;
  }

  public List<IndoctrinationOutfitItem> Populate(
    List<ClothingData> types,
    RectTransform contentContainer,
    Follower follower,
    string[] order = null,
    List<IndoctrinationOutfitItem> OutfitItemList = null,
    bool isSpineVisible = true)
  {
    IndoctrinationOutfitItem indoctrinationOutfitItem1 = (IndoctrinationOutfitItem) null;
    List<IndoctrinationOutfitItem> indoctrinationOutfitItemList = new List<IndoctrinationOutfitItem>();
    foreach (ClothingData type in types)
    {
      if (type.ClothingType != FollowerClothingType.None)
      {
        IndoctrinationOutfitItem indoctrinationOutfitItem2 = MonoSingleton<UIManager>.Instance.FollowerFormOutfitTemplate.Spawn<IndoctrinationOutfitItem>((Transform) contentContainer);
        indoctrinationOutfitItem2.transform.localScale = Vector3.one;
        indoctrinationOutfitItem2.Configure(type, follower, isSpineVisible);
        IndoctrinationOutfitItem indoctrinationOutfitItem3 = indoctrinationOutfitItem2;
        indoctrinationOutfitItem3.OnItemHighlighted = indoctrinationOutfitItem3.OnItemHighlighted + new Action<IndoctrinationOutfitItem>(this.OnClothingHighlighted);
        IndoctrinationOutfitItem indoctrinationOutfitItem4 = indoctrinationOutfitItem2;
        indoctrinationOutfitItem4.OnItemSelected = indoctrinationOutfitItem4.OnItemSelected + new Action<IndoctrinationOutfitItem>(this.OnClothingSelected);
        OutfitItemList?.Add(indoctrinationOutfitItem2);
        if (type.ClothingType == this.cachedClothingType.ClothingType)
          indoctrinationOutfitItem1 = indoctrinationOutfitItem2;
      }
    }
    if (OutfitItemList != null)
      OutfitItemList = OutfitItemList.OrderBy<IndoctrinationOutfitItem, int>((Func<IndoctrinationOutfitItem, int>) (x => x.transform.GetSiblingIndex())).ToList<IndoctrinationOutfitItem>();
    if ((UnityEngine.Object) indoctrinationOutfitItem1 == (UnityEngine.Object) null)
      indoctrinationOutfitItem1 = this._removeOutfitButton;
    this._removeOutfitButton.Configure(TailorManager.GetClothingData(FollowerClothingType.None), follower);
    IndoctrinationOutfitItem removeOutfitButton1 = this._removeOutfitButton;
    removeOutfitButton1.OnItemHighlighted = removeOutfitButton1.OnItemHighlighted + new Action<IndoctrinationOutfitItem>(this.OnClothingHighlighted);
    IndoctrinationOutfitItem removeOutfitButton2 = this._removeOutfitButton;
    removeOutfitButton2.OnItemSelected = removeOutfitButton2.OnItemSelected + new Action<IndoctrinationOutfitItem>(this.OnClothingSelected);
    this._removeOutfitButton.transform.SetAsFirstSibling();
    this._removeOutfitButton.Button.Confirmable = true;
    OutfitItemList?.Add(this._removeOutfitButton);
    if ((UnityEngine.Object) indoctrinationOutfitItem1 != (UnityEngine.Object) null)
    {
      this.OverrideDefaultOnce((Selectable) indoctrinationOutfitItem1.Button);
      this.ActivateNavigation();
      indoctrinationOutfitItem1.SetAsSelected();
    }
    return indoctrinationOutfitItemList;
  }

  public void Update()
  {
    if (!this._canvasGroup.interactable)
      return;
    IndoctrinationOutfitItem component = MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable.GetComponent<IndoctrinationOutfitItem>();
    if (!InputManager.UI.GetCookButtonDown() || !((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.IsCustomizable)
      return;
    this.OnClothingSelected(component);
    UICustomizeClothesMenuController_Colour menu = MonoSingleton<UIManager>.Instance.CustomizeClothesColourTemplate.Instantiate<UICustomizeClothesMenuController_Colour>();
    menu.Show(this.follower, component.returnOutfitData().ClothingType, component.Variant);
    menu.OnClothingSelected += (Action<ClothingData, int, string>) ((data, i, v) =>
    {
      foreach (IndoctrinationOutfitItem outfitItem in this._outfitItems)
      {
        if ((UnityEngine.Object) outfitItem.returnOutfitData() == (UnityEngine.Object) data)
          outfitItem.UpdateIcon(v);
      }
      foreach (IndoctrinationOutfitItem specialOutfitItem in this._specialOutfitItems)
      {
        if ((UnityEngine.Object) specialOutfitItem.returnOutfitData() == (UnityEngine.Object) data)
          specialOutfitItem.UpdateIcon(v);
      }
      foreach (IndoctrinationOutfitItem dlcOutfitItem in this._dlcOutfitItems)
      {
        if ((UnityEngine.Object) dlcOutfitItem.returnOutfitData() == (UnityEngine.Object) data)
          dlcOutfitItem.UpdateIcon(v);
      }
      foreach (IndoctrinationOutfitItem majorDlcOutfitItem in this._majorDlcOutfitItems)
      {
        if ((UnityEngine.Object) majorDlcOutfitItem.returnOutfitData() == (UnityEngine.Object) data)
          majorDlcOutfitItem.UpdateIcon(v);
      }
      Action<ClothingData, string> onOutfitChanged = this.OnOutfitChanged;
      if (onOutfitChanged == null)
        return;
      onOutfitChanged(data, v);
    });
    menu.OnClothingChanged += (Action<ClothingData, int, string>) ((data, i, v) =>
    {
      Action<ClothingData, string> onOutfitChanged = this.OnOutfitChanged;
      if (onOutfitChanged == null)
        return;
      onOutfitChanged(data, v);
    });
    this.PushInstance<UICustomizeClothesMenuController_Colour>(menu);
  }

  public void OnNecklaceSelected(necklaceFormItem formItem)
  {
    this.cacheNecklace = formItem.ReturnNecklace();
    this.currentNecklace = formItem.ReturnNecklace();
    this.UpdateSelection(formItem);
    Action<InventoryItem.ITEM_TYPE> onNecklaceChanged = this.OnNecklaceChanged;
    if (onNecklaceChanged != null)
      onNecklaceChanged(this.cacheNecklace);
    formItem.SetAsSelected();
  }

  public void OnNecklaceHighlighted(necklaceFormItem formItem)
  {
    this.currentNecklace = formItem.ReturnNecklace();
    Action<InventoryItem.ITEM_TYPE> onNecklaceChanged = this.OnNecklaceChanged;
    if (onNecklaceChanged != null)
      onNecklaceChanged(formItem.ReturnNecklace());
    if (this.cachedClothingType.ClothingType != this.currentClothingType.ClothingType)
      this.RevertOutfit();
    if (formItem.Necklace != InventoryItem.ITEM_TYPE.NONE && !formItem.Locked)
      this.ShowNecklaceInfo(formItem.Necklace);
    else
      this.HideNecklaceInfo();
    this.customisePrompt.gameObject.SetActive(false);
  }

  public void OnClothingSelected(IndoctrinationOutfitItem formItem)
  {
    this.UpdateSelection(formItem);
    if (DataManager.Instance.ClothesUnlocked(this.currentClothingType.ClothingType) || this.currentClothingType.ClothingType == FollowerClothingType.None)
    {
      this.currentClothingType = new StructuresData.ClothingStruct((UnityEngine.Object) formItem.returnOutfitData() == (UnityEngine.Object) null ? FollowerClothingType.None : formItem.returnOutfitData().ClothingType, formItem.Variant);
      this.cachedClothingType = this.currentClothingType;
      Action<ClothingData, string> onOutfitChanged = this.OnOutfitChanged;
      if (onOutfitChanged != null)
        onOutfitChanged(formItem.returnOutfitData(), formItem.Variant);
    }
    formItem.SetAsSelected();
  }

  public void OnClothingHighlighted(IndoctrinationOutfitItem formItem)
  {
    this.currentClothingType = new StructuresData.ClothingStruct((UnityEngine.Object) formItem.returnOutfitData() == (UnityEngine.Object) null ? FollowerClothingType.None : formItem.returnOutfitData().ClothingType, formItem.Variant);
    if (DataManager.Instance.ClothesUnlocked(this.currentClothingType.ClothingType))
    {
      Action<ClothingData, string> onOutfitChanged = this.OnOutfitChanged;
      if (onOutfitChanged != null)
        onOutfitChanged(formItem.returnOutfitData(), formItem.Variant);
    }
    if (this.cacheNecklace != this.currentNecklace)
      this.RevertNecklace();
    this.HideNecklaceInfo();
    this.customisePrompt.gameObject.SetActive(true);
  }

  public void UpdateSelection(IndoctrinationOutfitItem formItem)
  {
    foreach (IndoctrinationOutfitItem outfitItem in this._outfitItems)
    {
      if ((UnityEngine.Object) outfitItem == (UnityEngine.Object) formItem)
        outfitItem.SetAsSelected();
      else
        outfitItem.CheckLocked();
    }
    foreach (IndoctrinationOutfitItem specialOutfitItem in this._specialOutfitItems)
    {
      if ((UnityEngine.Object) specialOutfitItem == (UnityEngine.Object) formItem)
        specialOutfitItem.SetAsSelected();
      else
        specialOutfitItem.CheckLocked();
    }
    foreach (IndoctrinationOutfitItem dlcOutfitItem in this._dlcOutfitItems)
    {
      if ((UnityEngine.Object) dlcOutfitItem == (UnityEngine.Object) formItem)
        dlcOutfitItem.SetAsSelected();
      else
        dlcOutfitItem.CheckLocked();
    }
    foreach (IndoctrinationOutfitItem majorDlcOutfitItem in this._majorDlcOutfitItems)
    {
      if ((UnityEngine.Object) majorDlcOutfitItem == (UnityEngine.Object) formItem)
        majorDlcOutfitItem.SetAsSelected();
      else
        majorDlcOutfitItem.CheckLocked();
    }
  }

  public void UpdateSelection(necklaceFormItem formItem)
  {
    foreach (necklaceFormItem necklaceItem in this._necklaceItems)
    {
      if ((UnityEngine.Object) necklaceItem == (UnityEngine.Object) formItem)
        necklaceItem.SetAsSelected();
      else
        necklaceItem.SetAsDefault();
    }
  }

  public void ShowNecklaceInfo(InventoryItem.ITEM_TYPE itemType)
  {
    if ((double) this.necklaceInfoCardCanvasGroup.alpha == 0.0)
    {
      this.necklaceInfoCardCanvasGroup.DOKill();
      this.necklaceInfoCardCanvasGroup.DOFade(1f, 0.25f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    }
    this.necklaceInfoCardTitle.text = InventoryItem.LocalizedName(itemType);
    this.necklaceInfoCardDescription.text = InventoryItem.LocalizedDescription(itemType);
    this.necklaceInfoCardIcon.sprite = this.iconMapping.GetImage(itemType);
  }

  public void HideNecklaceInfo()
  {
    this.necklaceInfoCardCanvasGroup.DOKill();
    this.necklaceInfoCardCanvasGroup.DOFade(0.0f, 0.25f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__43_0(ClothingData data, int i, string v)
  {
    foreach (IndoctrinationOutfitItem outfitItem in this._outfitItems)
    {
      if ((UnityEngine.Object) outfitItem.returnOutfitData() == (UnityEngine.Object) data)
        outfitItem.UpdateIcon(v);
    }
    foreach (IndoctrinationOutfitItem specialOutfitItem in this._specialOutfitItems)
    {
      if ((UnityEngine.Object) specialOutfitItem.returnOutfitData() == (UnityEngine.Object) data)
        specialOutfitItem.UpdateIcon(v);
    }
    foreach (IndoctrinationOutfitItem dlcOutfitItem in this._dlcOutfitItems)
    {
      if ((UnityEngine.Object) dlcOutfitItem.returnOutfitData() == (UnityEngine.Object) data)
        dlcOutfitItem.UpdateIcon(v);
    }
    foreach (IndoctrinationOutfitItem majorDlcOutfitItem in this._majorDlcOutfitItems)
    {
      if ((UnityEngine.Object) majorDlcOutfitItem.returnOutfitData() == (UnityEngine.Object) data)
        majorDlcOutfitItem.UpdateIcon(v);
    }
    Action<ClothingData, string> onOutfitChanged = this.OnOutfitChanged;
    if (onOutfitChanged == null)
      return;
    onOutfitChanged(data, v);
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__43_1(ClothingData data, int i, string v)
  {
    Action<ClothingData, string> onOutfitChanged = this.OnOutfitChanged;
    if (onOutfitChanged == null)
      return;
    onOutfitChanged(data, v);
  }
}
