// Decompiled with JetBrains decompiler
// Type: TraitManipulatorInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

#nullable disable
public class TraitManipulatorInfoCard : UIInfoCardBase<FollowerInfo>
{
  [Header("Prison Info Card")]
  [SerializeField]
  public TextMeshProUGUI _followerNameText;
  [SerializeField]
  public SkeletonGraphic _followerSpine;
  [SerializeField]
  public RectTransform _redOutline;
  [SerializeField]
  public IndoctrinationTraitItem traitItem;
  [Header("Shuffle")]
  [SerializeField]
  public GameObject shuffleParent;
  [SerializeField]
  public List<GameObject> traitSlots;
  [SerializeField]
  public List<GameObject> targetTraitSlots;
  [Header("Add/Remove")]
  [SerializeField]
  public GameObject addRemoveParent;
  [SerializeField]
  public List<GameObject> addRemoveTraitSlots;
  [SerializeField]
  public GameObject addParent;
  [SerializeField]
  public GameObject addSlot;
  public FollowerInfo _followerInfo;
  [CompilerGenerated]
  public List<IndoctrinationTraitItem> \u003CTraits\u003Ek__BackingField = new List<IndoctrinationTraitItem>();

  public SkeletonGraphic FollowerSpine => this._followerSpine;

  public RectTransform RedOutline => this._redOutline;

  public List<IndoctrinationTraitItem> Traits
  {
    get => this.\u003CTraits\u003Ek__BackingField;
    set => this.\u003CTraits\u003Ek__BackingField = value;
  }

  public override void Configure(FollowerInfo config)
  {
    this._followerInfo = config;
    this._followerNameText.text = config.Name;
    this._followerSpine.ConfigureFollower(config);
    for (int index1 = this.addRemoveTraitSlots.Count - 1; index1 >= 0; --index1)
    {
      if (this.addRemoveTraitSlots[index1].transform.childCount > 0)
      {
        for (int index2 = this.addRemoveTraitSlots[index1].transform.childCount - 1; index2 >= 0; --index2)
          UnityEngine.Object.Destroy((UnityEngine.Object) this.addRemoveTraitSlots[index1].transform.GetChild(index2).gameObject);
      }
    }
    for (int index3 = this.traitSlots.Count - 1; index3 >= 0; --index3)
    {
      if (this.traitSlots[index3].transform.childCount > 0)
      {
        for (int index4 = this.traitSlots[index3].transform.childCount - 1; index4 >= 0; --index4)
          UnityEngine.Object.Destroy((UnityEngine.Object) this.traitSlots[index3].transform.GetChild(index4).gameObject);
      }
    }
    for (int index5 = this.targetTraitSlots.Count - 1; index5 >= 0; --index5)
    {
      if (this.targetTraitSlots[index5].transform.childCount > 0)
      {
        for (int index6 = this.targetTraitSlots[index5].transform.childCount - 1; index6 >= 0; --index6)
          UnityEngine.Object.Destroy((UnityEngine.Object) this.targetTraitSlots[index5].transform.GetChild(index6).gameObject);
      }
    }
    if (this.addSlot.transform.childCount > 0)
    {
      for (int index = this.addSlot.transform.childCount - 1; index >= 0; --index)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.addSlot.transform.GetChild(index).gameObject);
    }
    UITraitManipulatorMenuController.Type selectionType = this.GetComponentInParent<UITraitManipulatorMenuController>().SelectionType;
    this.shuffleParent.gameObject.SetActive(selectionType == UITraitManipulatorMenuController.Type.Shuffle);
    this.addRemoveParent.gameObject.SetActive(selectionType != 0);
    this.addParent.gameObject.SetActive(selectionType == UITraitManipulatorMenuController.Type.Add);
    this.Traits.Clear();
    int index7 = 0;
    List<GameObject> gameObjectList = this.traitSlots;
    if (selectionType != UITraitManipulatorMenuController.Type.Shuffle)
      gameObjectList = this.addRemoveTraitSlots;
    List<FollowerTrait.TraitType> traitTypeList1 = new List<FollowerTrait.TraitType>((IEnumerable<FollowerTrait.TraitType>) config.Traits);
    if (selectionType == UITraitManipulatorMenuController.Type.Add)
    {
      for (int index8 = traitTypeList1.Count - 1; index8 >= 0; --index8)
      {
        if (config.HasTraitFromNecklace(traitTypeList1[index8]))
          traitTypeList1.RemoveAt(index8);
      }
    }
    traitTypeList1.Sort(new Comparison<FollowerTrait.TraitType>(this.SortCompareTraits));
    for (int index9 = 0; index9 < traitTypeList1.Count && index7 < gameObjectList.Count; ++index9)
    {
      IndoctrinationTraitItem indoctrinationTraitItem = UnityEngine.Object.Instantiate<IndoctrinationTraitItem>(this.traitItem, gameObjectList[index7].transform);
      ((RectTransform) indoctrinationTraitItem.transform).anchoredPosition = new Vector2(30f, 30f);
      indoctrinationTraitItem.transform.localScale = Vector3.one * 0.85f;
      indoctrinationTraitItem.Configure(traitTypeList1[index9]);
      indoctrinationTraitItem.Selectable.Interactable = false;
      if ((selectionType == UITraitManipulatorMenuController.Type.Remove || selectionType == UITraitManipulatorMenuController.Type.Shuffle) && FollowerTrait.UniqueTraits.Contains(indoctrinationTraitItem.TraitType))
        indoctrinationTraitItem.SetDeactivated(true);
      this.Traits.Add(indoctrinationTraitItem);
      ++index7;
    }
    switch (selectionType)
    {
      case UITraitManipulatorMenuController.Type.Shuffle:
        int index10 = 0;
        List<FollowerTrait.TraitType> traitTypeList2 = config.RandomisedTraits(config.ID + TimeManager.CurrentDay);
        traitTypeList2.Sort(new Comparison<FollowerTrait.TraitType>(this.SortCompareTraits));
        for (int index11 = 0; index11 < traitTypeList2.Count && index10 < this.targetTraitSlots.Count; ++index11)
        {
          IndoctrinationTraitItem indoctrinationTraitItem = UnityEngine.Object.Instantiate<IndoctrinationTraitItem>(this.traitItem, this.targetTraitSlots[index10].transform);
          ((RectTransform) indoctrinationTraitItem.transform).anchoredPosition = new Vector2(30f, 30f);
          indoctrinationTraitItem.transform.localScale = Vector3.one * 0.85f;
          if (FollowerTrait.UniqueTraits.Contains(traitTypeList2[index11]) && traitTypeList1.Contains(traitTypeList2[index11]))
          {
            indoctrinationTraitItem.Configure(traitTypeList2[index11]);
            indoctrinationTraitItem.SetDeactivated(true);
          }
          else
            indoctrinationTraitItem.Configure(FollowerTrait.TraitType.None);
          indoctrinationTraitItem.Selectable.Interactable = false;
          ++index10;
        }
        break;
      case UITraitManipulatorMenuController.Type.Add:
        IndoctrinationTraitItem indoctrinationTraitItem1 = UnityEngine.Object.Instantiate<IndoctrinationTraitItem>(this.traitItem, this.addSlot.transform);
        ((RectTransform) indoctrinationTraitItem1.transform).anchoredPosition = new Vector2(30f, 30f);
        indoctrinationTraitItem1.transform.localScale = Vector3.one * 0.85f;
        indoctrinationTraitItem1.Configure(config.CompletelyNewRandomisedTraits(config.ID + TimeManager.CurrentDay, 1)[0]);
        indoctrinationTraitItem1.Selectable.Interactable = false;
        this.Traits.Add(indoctrinationTraitItem1);
        break;
    }
  }

  public void TraitsSelectable(bool selectable)
  {
    foreach (IndoctrinationTraitItem trait in this.Traits)
    {
      UITraitManipulatorMenuController.Type selectionType = this.GetComponentInParent<UITraitManipulatorMenuController>().SelectionType;
      if (selectable && selectionType == UITraitManipulatorMenuController.Type.Remove && FollowerTrait.UniqueTraits.Contains(trait.TraitType))
      {
        trait.Selectable.Interactable = false;
        trait.SetDeactivated(true);
      }
      else
        trait.Selectable.Interactable = selectable;
    }
  }

  public int SortCompareTraits(FollowerTrait.TraitType trait1, FollowerTrait.TraitType trait2)
  {
    if (FollowerTrait.UniqueTraits.Contains(trait1) && FollowerTrait.UniqueTraits.Contains(trait2))
      return 0;
    if (FollowerTrait.UniqueTraits.Contains(trait1))
      return 1;
    return FollowerTrait.UniqueTraits.Contains(trait2) ? -1 : 0;
  }
}
