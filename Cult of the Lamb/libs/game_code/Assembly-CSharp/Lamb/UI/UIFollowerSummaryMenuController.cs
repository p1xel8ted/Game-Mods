// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIFollowerSummaryMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.Extensions;
using src.UI.Items;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIFollowerSummaryMenuController : UIMenuBase
{
  [Header("Content")]
  [SerializeField]
  public FollowerInformationBox _infoBox;
  [SerializeField]
  public GridLayoutGroup _characterTraitLayoutGroup;
  [SerializeField]
  public RectTransform _characterTraitContent;
  [SerializeField]
  public GameObject _cultTraitHeader;
  [SerializeField]
  public GridLayoutGroup _cultTraitLayoutGroup;
  [SerializeField]
  public RectTransform _cultTraitContent;
  [SerializeField]
  public RectTransform _followerThoughtContent;
  [SerializeField]
  public MMScrollRect _scrollRect;
  [SerializeField]
  public TextMeshProUGUI Necklace;
  [SerializeField]
  public TraitInfoCardController _traitInfoCardController;
  [Header("Templates")]
  [SerializeField]
  public IndoctrinationTraitItem _traitItemTemplate;
  [SerializeField]
  public FollowerThoughtItem _followerThoughtItemTemplate;
  public List<IndoctrinationTraitItem> _traitItems = new List<IndoctrinationTraitItem>();
  public List<IndoctrinationTraitItem> _cultItems = new List<IndoctrinationTraitItem>();
  public List<FollowerThoughtItem> _thoughtItems = new List<FollowerThoughtItem>();
  public Follower _follower;

  public void Show(Follower follower, bool instant = false)
  {
    this._follower = follower;
    this._traitInfoCardController.FollowerBrain = follower.Brain;
    this.Show(instant);
  }

  public override void OnShowStarted()
  {
    this._scrollRect.normalizedPosition = Vector2.one;
    this._scrollRect.enabled = false;
    this._infoBox.Configure(this._follower.Brain._directInfoAccess);
    if (this._traitItems.Count == 0)
    {
      foreach (FollowerTrait.TraitType trait in this._follower.Brain.Info.Traits)
      {
        IndoctrinationTraitItem indoctrinationTraitItem = this._traitItemTemplate.Instantiate<IndoctrinationTraitItem>((Transform) this._characterTraitContent);
        indoctrinationTraitItem.Configure(trait);
        this._traitItems.Add(indoctrinationTraitItem);
      }
      if (this._traitItems.Count > this._characterTraitLayoutGroup.constraintCount)
        this._characterTraitLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
      else
        this._characterTraitLayoutGroup.childAlignment = TextAnchor.UpperLeft;
    }
    if (this._cultItems.Count == 0)
    {
      foreach (FollowerTrait.TraitType cultTrait in DataManager.Instance.CultTraits)
      {
        IndoctrinationTraitItem indoctrinationTraitItem = this._traitItemTemplate.Instantiate<IndoctrinationTraitItem>((Transform) this._cultTraitContent);
        indoctrinationTraitItem.Configure(cultTrait);
        this._cultItems.Add(indoctrinationTraitItem);
      }
      if (this._cultItems.Count > this._cultTraitLayoutGroup.constraintCount)
        this._cultTraitLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
      else
        this._cultTraitLayoutGroup.childAlignment = TextAnchor.UpperLeft;
      this._cultTraitHeader.SetActive(this._cultItems.Count > 0);
    }
    if (this._thoughtItems.Count == 0)
    {
      List<ThoughtData> thoughtDataList = new List<ThoughtData>((IEnumerable<ThoughtData>) this._follower.Brain.Stats.Thoughts);
      if (this._follower.Brain.Info.CursedState == Thought.OldAge)
      {
        foreach (ThoughtData thoughtData in thoughtDataList)
        {
          if (thoughtData.ThoughtType == Thought.OldAge)
          {
            thoughtDataList.Remove(thoughtData);
            break;
          }
        }
        thoughtDataList.Add(new ThoughtData(Thought.OldAge));
      }
      else if (this._follower.Brain.Info.CursedState == Thought.Dissenter)
        thoughtDataList.Add(new ThoughtData(Thought.BiggestNeed_Dissenter));
      else if (this._follower.Brain.Info.CursedState == Thought.Ill)
        thoughtDataList.Add(new ThoughtData(Thought.BiggestNeed_Sick));
      else if (this._follower.Brain.Info.CursedState == Thought.BecomeStarving)
        thoughtDataList.Add(new ThoughtData(Thought.BiggestNeed_Hungry));
      else if ((double) this._follower.Brain.Stats.Exhaustion > 0.0)
        thoughtDataList.Add(new ThoughtData(Thought.BiggestNeed_Exhausted));
      else if (!this._follower.Brain.HasHome)
      {
        thoughtDataList.Add(new ThoughtData(Thought.BiggestNeed_Homeless));
      }
      else
      {
        Structures_Bed structureById = StructureManager.GetStructureByID<Structures_Bed>(this._follower.Brain._directInfoAccess.DwellingID);
        if (structureById != null && structureById.IsCollapsed)
          thoughtDataList.Add(new ThoughtData(Thought.BiggestNeed_BrokenBed));
      }
      thoughtDataList.Reverse();
      for (int index = 0; index < thoughtDataList.Count; ++index)
      {
        FollowerThoughtItem followerThoughtItem = this._followerThoughtItemTemplate.Instantiate<FollowerThoughtItem>((Transform) this._followerThoughtContent);
        thoughtDataList[index].FollowerID = this._follower.Brain.Info.ID;
        followerThoughtItem.Configure(thoughtDataList[index]);
        this._thoughtItems.Add(followerThoughtItem);
      }
    }
    Navigation navigation1 = this._infoBox.Button.navigation with
    {
      mode = Navigation.Mode.Explicit
    };
    if (this._traitItems.Count > 0)
      navigation1.selectOnDown = (Selectable) this._traitItems[0].Selectable;
    else if (this._cultItems.Count > 0)
      navigation1.selectOnDown = (Selectable) this._cultItems[0].Selectable;
    else if (this._thoughtItems.Count > 0)
      navigation1.selectOnDown = (Selectable) this._thoughtItems[0].Selectable;
    this._infoBox.Button.navigation = navigation1;
    int constraintCount1 = this._characterTraitLayoutGroup.constraintCount;
    int num1 = Mathf.FloorToInt((float) this._traitItems.Count / (float) constraintCount1);
    if (this._traitItems.Count > 0)
    {
      for (int index1 = 0; index1 < this._traitItems.Count; ++index1)
      {
        Navigation navigation2 = this._traitItems[index1].Selectable.navigation with
        {
          mode = Navigation.Mode.Explicit
        };
        int num2 = Mathf.FloorToInt((float) index1 / (float) constraintCount1);
        int index2 = index1 - num2 * constraintCount1;
        if (num2 == 0)
        {
          navigation2.selectOnUp = (Selectable) this._infoBox.Button;
          if (index1 + constraintCount1 < this._traitItems.Count)
            navigation2.selectOnDown = (Selectable) this._traitItems[index1 + constraintCount1].Selectable;
          else if (this._cultItems.Count > 0)
            navigation2.selectOnDown = index2 >= this._cultItems.Count ? (Selectable) this._cultItems.LastElement<IndoctrinationTraitItem>().Selectable : (Selectable) this._cultItems[index2].Selectable;
          else if (this._thoughtItems.Count > 0)
            navigation2.selectOnDown = (Selectable) this._thoughtItems[0].Selectable;
        }
        else if (num2 == num1)
        {
          navigation2.selectOnUp = (Selectable) this._traitItems[index1 - constraintCount1].Selectable;
          if (this._cultItems.Count > 0)
            navigation2.selectOnDown = index2 >= this._cultItems.Count ? (Selectable) this._cultItems.LastElement<IndoctrinationTraitItem>().Selectable : (Selectable) this._cultItems[index2].Selectable;
          else if (this._thoughtItems.Count > 0)
            navigation2.selectOnDown = (Selectable) this._thoughtItems[0].Selectable;
        }
        else
        {
          navigation2.selectOnUp = (Selectable) this._traitItems[index1 - constraintCount1].Selectable;
          navigation2.selectOnDown = index1 + constraintCount1 >= this._traitItems.Count ? (Selectable) this._traitItems.LastElement<IndoctrinationTraitItem>().Selectable : (Selectable) this._traitItems[index1 + constraintCount1].Selectable;
        }
        if (index2 > 0)
          navigation2.selectOnLeft = (Selectable) this._traitItems[index1 - 1].Selectable;
        if (index2 < constraintCount1 && index1 + 1 < this._traitItems.Count)
          navigation2.selectOnRight = (Selectable) this._traitItems[index1 + 1].Selectable;
        this._traitItems[index1].Selectable.navigation = navigation2;
      }
    }
    int constraintCount2 = this._characterTraitLayoutGroup.constraintCount;
    int num3 = Mathf.CeilToInt((float) this._cultItems.Count / (float) constraintCount2);
    if (this._cultItems.Count > 0)
    {
      for (int index = 0; index < this._cultItems.Count; ++index)
      {
        Navigation navigation3 = this._cultItems[index].Selectable.navigation with
        {
          mode = Navigation.Mode.Explicit
        };
        int num4 = Mathf.FloorToInt((float) index / (float) constraintCount2);
        int num5 = index - num4 * constraintCount2;
        if (num4 == 0)
        {
          if (this._traitItems.Count > 0)
            navigation3.selectOnUp = constraintCount1 * num1 + num5 >= this._traitItems.Count ? (Selectable) this._traitItems.LastElement<IndoctrinationTraitItem>().Selectable : (Selectable) this._traitItems[constraintCount1 * num1 + num5].Selectable;
          if (num5 + constraintCount2 < this._cultItems.Count)
            navigation3.selectOnDown = (Selectable) this._cultItems[num5 + constraintCount2].Selectable;
          else if (this._thoughtItems.Count > 0)
            navigation3.selectOnDown = (Selectable) this._thoughtItems[0].Selectable;
        }
        else if (num4 == num3 - 1)
        {
          navigation3.selectOnUp = (Selectable) this._cultItems[index - constraintCount2].Selectable;
          navigation3.selectOnDown = this._thoughtItems.Count <= 0 ? (Selectable) this._cultItems.LastElement<IndoctrinationTraitItem>().Selectable : (Selectable) this._thoughtItems[0].Selectable;
        }
        else
        {
          navigation3.selectOnUp = (Selectable) this._cultItems[index - constraintCount2].Selectable;
          navigation3.selectOnDown = index + constraintCount2 >= this._cultItems.Count ? (Selectable) this._cultItems.LastElement<IndoctrinationTraitItem>().Selectable : (Selectable) this._cultItems[index + constraintCount2].Selectable;
        }
        if (num5 > 0)
          navigation3.selectOnLeft = (Selectable) this._cultItems[index - 1].Selectable;
        if (num5 < constraintCount2 && index + 1 < this._cultItems.Count)
          navigation3.selectOnRight = (Selectable) this._cultItems[index + 1].Selectable;
        this._cultItems[index].Selectable.navigation = navigation3;
      }
    }
    if (this._thoughtItems.Count > 0)
    {
      Navigation navigation4 = this._thoughtItems[0].Selectable.navigation with
      {
        mode = Navigation.Mode.Explicit
      };
      if (this._cultItems.Count > 0)
        navigation4.selectOnUp = (Selectable) this._cultItems[constraintCount2 * Mathf.Clamp(num3 - 1, 0, int.MaxValue)].Selectable;
      else if (this._traitItems.Count > 0)
        navigation4.selectOnUp = (Selectable) this._traitItems[constraintCount1 * num1].Selectable;
      if (this._thoughtItems.Count > 1)
        navigation4.selectOnDown = (Selectable) this._thoughtItems[1].Selectable;
      this._thoughtItems[0].Selectable.navigation = navigation4;
    }
    if (this._follower.Brain.Info.Necklace == InventoryItem.ITEM_TYPE.NONE)
      this.Necklace.gameObject.SetActive(false);
    else
      this.Necklace.text = $"{FontImageNames.GetIconByType(this._follower.Brain.Info.Necklace)} {InventoryItem.LocalizedName(this._follower.Brain.Info.Necklace)}: {InventoryItem.LocalizedDescription(this._follower.Brain.Info.Necklace)}";
    this._scrollRect.enabled = true;
    if (!DataManager.Instance.PleasureEnabled)
      return;
    this._infoBox.ShowPleasure(0, false);
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  public override void OnHideCompleted() => Object.Destroy((Object) this.gameObject);
}
