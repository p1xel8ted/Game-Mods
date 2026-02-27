// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIFollowerSummaryMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private FollowerInformationBox _infoBox;
  [SerializeField]
  private RectTransform _characterTraitContent;
  [SerializeField]
  private GameObject _cultTraitHeader;
  [SerializeField]
  private RectTransform _cultTraitContent;
  [SerializeField]
  private RectTransform _followerThoughtContent;
  [SerializeField]
  private MMScrollRect _scrollRect;
  [SerializeField]
  private TextMeshProUGUI Necklace;
  [Header("Templates")]
  [SerializeField]
  private IndoctrinationTraitItem _traitItemTemplate;
  [SerializeField]
  private FollowerThoughtItem _followerThoughtItemTemplate;
  private List<IndoctrinationTraitItem> _traitItems = new List<IndoctrinationTraitItem>();
  private List<IndoctrinationTraitItem> _cultItems = new List<IndoctrinationTraitItem>();
  private List<FollowerThoughtItem> _thoughtItems = new List<FollowerThoughtItem>();
  private Follower _follower;

  public void Show(Follower follower, bool instant = false)
  {
    this._follower = follower;
    this.Show(instant);
  }

  protected override void OnShowStarted()
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
    }
    if (this._cultItems.Count == 0)
    {
      foreach (FollowerTrait.TraitType cultTrait in DataManager.Instance.CultTraits)
      {
        IndoctrinationTraitItem indoctrinationTraitItem = this._traitItemTemplate.Instantiate<IndoctrinationTraitItem>((Transform) this._cultTraitContent);
        indoctrinationTraitItem.Configure(cultTrait);
        this._cultItems.Add(indoctrinationTraitItem);
      }
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
      foreach (ThoughtData thoughtData in thoughtDataList)
      {
        FollowerThoughtItem followerThoughtItem = this._followerThoughtItemTemplate.Instantiate<FollowerThoughtItem>((Transform) this._followerThoughtContent);
        followerThoughtItem.Configure(thoughtData);
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
    if (this._traitItems.Count > 0)
    {
      for (int index = 0; index < this._traitItems.Count; ++index)
      {
        Navigation navigation2 = this._traitItems[index].Selectable.navigation with
        {
          mode = Navigation.Mode.Explicit
        };
        if (this._cultItems.Count > 0 && index < this._cultItems.Count)
          navigation2.selectOnDown = (Selectable) this._cultItems[index].Selectable;
        else if (this._thoughtItems.Count > 0)
          navigation2.selectOnDown = (Selectable) this._thoughtItems[0].Selectable;
        if (index > 0)
          navigation2.selectOnLeft = (Selectable) this._traitItems[index - 1].Selectable;
        if (index < this._traitItems.Count - 1)
          navigation2.selectOnRight = (Selectable) this._traitItems[index + 1].Selectable;
        navigation2.selectOnUp = (Selectable) this._infoBox.Button;
        this._traitItems[index].Selectable.navigation = navigation2;
      }
    }
    if (this._cultItems.Count > 0)
    {
      for (int index = 0; index < this._cultItems.Count; ++index)
      {
        Navigation navigation3 = this._cultItems[index].Selectable.navigation with
        {
          mode = Navigation.Mode.Explicit,
          selectOnUp = index >= this._traitItems.Count ? (Selectable) this._infoBox.Button : (Selectable) this._traitItems[index].Selectable
        };
        if (this._thoughtItems.Count > 0)
          navigation3.selectOnDown = (Selectable) this._thoughtItems[0].Selectable;
        if (index > 0)
          navigation3.selectOnLeft = (Selectable) this._cultItems[index - 1].Selectable;
        if (index < this._cultItems.Count - 1)
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
        navigation4.selectOnUp = (Selectable) this._cultItems[0].Selectable;
      else if (this._traitItems.Count > 0)
        navigation4.selectOnUp = (Selectable) this._traitItems[0].Selectable;
      if (this._thoughtItems.Count > 1)
        navigation4.selectOnDown = (Selectable) this._thoughtItems[1].Selectable;
      this._thoughtItems[0].Selectable.navigation = navigation4;
    }
    if (this._follower.Brain.Info.Necklace == InventoryItem.ITEM_TYPE.NONE)
      this.Necklace.gameObject.SetActive(false);
    else
      this.Necklace.text = $"{FontImageNames.GetIconByType(this._follower.Brain.Info.Necklace)} {InventoryItem.LocalizedName(this._follower.Brain.Info.Necklace)}: {InventoryItem.LocalizedDescription(this._follower.Brain.Info.Necklace)}";
    this._scrollRect.enabled = true;
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  protected override void OnHideCompleted() => Object.Destroy((Object) this.gameObject);
}
