// Decompiled with JetBrains decompiler
// Type: Lamb.UI.TraitsMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using src.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class TraitsMenu : UISubmenuBase
{
  [SerializeField]
  private MMScrollRect _scrollRect;
  [Header("Character Traits")]
  [SerializeField]
  private RectTransform _characterTraitContent;
  [Header("Cult Traits")]
  [SerializeField]
  private RectTransform _cultTraitContent;
  [Header("Template")]
  [SerializeField]
  private IndoctrinationTraitItem _traitItemTemplate;
  private List<IndoctrinationTraitItem> _traitItems = new List<IndoctrinationTraitItem>();
  private List<IndoctrinationTraitItem> _cultItems = new List<IndoctrinationTraitItem>();
  private Follower _follower;

  public void Configure(Follower follower) => this._follower = follower;

  protected override void OnShowStarted()
  {
    this._scrollRect.normalizedPosition = Vector2.one;
    this._scrollRect.enabled = false;
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
    }
    this.OverrideDefaultOnce((Selectable) this._traitItems[0].Selectable);
    this.ActivateNavigation();
    this._scrollRect.enabled = true;
  }
}
