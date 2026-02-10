// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UICustomizeClothesController_Outfit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UICustomizeClothesController_Outfit : UIMenuBase
{
  public Action<ClothingData> OnOutfitChanged;
  [SerializeField]
  public RectTransform _outfitContent;
  public List<IndoctrinationOutfitItem> _outfitItems = new List<IndoctrinationOutfitItem>();
  public int _cacheOutfit;
  public FollowerClothingType cachedClothingType;
  public FollowerClothingType currentClothingType;

  public override void OnShowStarted()
  {
    this._outfitItems.AddRange((IEnumerable<IndoctrinationOutfitItem>) this.Populate(TailorManager.GetAvailableWeatherClothing(), this._outfitContent, (Follower) null));
  }

  public void Show(FollowerClothingType currentClothing)
  {
    this.cachedClothingType = currentClothing;
    this.Show();
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  public void RevertOutfit()
  {
    Action<ClothingData> onOutfitChanged = this.OnOutfitChanged;
    if (onOutfitChanged == null)
      return;
    onOutfitChanged(TailorManager.GetClothingData(this.cachedClothingType));
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  public List<IndoctrinationOutfitItem> Populate(
    List<ClothingData> types,
    RectTransform contentContainer,
    Follower follower,
    string[] order = null)
  {
    IndoctrinationOutfitItem indoctrinationOutfitItem1 = (IndoctrinationOutfitItem) null;
    List<IndoctrinationOutfitItem> indoctrinationOutfitItemList = new List<IndoctrinationOutfitItem>();
    foreach (ClothingData type in types)
    {
      if (type.ClothingType != FollowerClothingType.None)
      {
        IndoctrinationOutfitItem indoctrinationOutfitItem2 = MonoSingleton<UIManager>.Instance.FollowerFormOutfitTemplate.Spawn<IndoctrinationOutfitItem>((Transform) contentContainer);
        indoctrinationOutfitItem2.transform.localScale = Vector3.one;
        indoctrinationOutfitItem2.Configure(type, follower);
        IndoctrinationOutfitItem indoctrinationOutfitItem3 = indoctrinationOutfitItem2;
        indoctrinationOutfitItem3.OnItemHighlighted = indoctrinationOutfitItem3.OnItemHighlighted + new Action<IndoctrinationOutfitItem>(this.OnClothingHighlighted);
        IndoctrinationOutfitItem indoctrinationOutfitItem4 = indoctrinationOutfitItem2;
        indoctrinationOutfitItem4.OnItemSelected = indoctrinationOutfitItem4.OnItemSelected + new Action<IndoctrinationOutfitItem>(this.OnClothingSelected);
        this._outfitItems.Add(indoctrinationOutfitItem2);
        if (type.ClothingType == this.cachedClothingType)
          indoctrinationOutfitItem1 = indoctrinationOutfitItem2;
      }
    }
    this._outfitItems = this._outfitItems.OrderBy<IndoctrinationOutfitItem, int>((Func<IndoctrinationOutfitItem, int>) (x => x.transform.GetSiblingIndex())).ToList<IndoctrinationOutfitItem>();
    if ((UnityEngine.Object) indoctrinationOutfitItem1 == (UnityEngine.Object) null && this._outfitItems.Count > 0)
      indoctrinationOutfitItem1 = this._outfitItems[0];
    if ((UnityEngine.Object) indoctrinationOutfitItem1 != (UnityEngine.Object) null)
    {
      this.OverrideDefaultOnce((Selectable) indoctrinationOutfitItem1.Button);
      this.ActivateNavigation();
      indoctrinationOutfitItem1.SetAsSelected();
    }
    return indoctrinationOutfitItemList;
  }

  public void OnClothingSelected(IndoctrinationOutfitItem formItem)
  {
    this.UpdateSelection(formItem);
    if (DataManager.Instance.ClothesUnlocked(this.currentClothingType) || this.currentClothingType == FollowerClothingType.None)
    {
      this.currentClothingType = (UnityEngine.Object) formItem.returnOutfitData() == (UnityEngine.Object) null ? FollowerClothingType.None : formItem.returnOutfitData().ClothingType;
      this.cachedClothingType = this.currentClothingType;
      Action<ClothingData> onOutfitChanged = this.OnOutfitChanged;
      if (onOutfitChanged != null)
        onOutfitChanged(formItem.returnOutfitData());
    }
    formItem.SetAsSelected();
  }

  public void OnClothingHighlighted(IndoctrinationOutfitItem formItem)
  {
    this.currentClothingType = (UnityEngine.Object) formItem.returnOutfitData() == (UnityEngine.Object) null ? FollowerClothingType.None : formItem.returnOutfitData().ClothingType;
    if (!DataManager.Instance.ClothesUnlocked(this.currentClothingType))
      return;
    Action<ClothingData> onOutfitChanged = this.OnOutfitChanged;
    if (onOutfitChanged == null)
      return;
    onOutfitChanged(formItem.returnOutfitData());
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
  }
}
