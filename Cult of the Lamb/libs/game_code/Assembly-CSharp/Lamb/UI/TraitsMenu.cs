// Decompiled with JetBrains decompiler
// Type: Lamb.UI.TraitsMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class TraitsMenu : UISubmenuBase
{
  [SerializeField]
  public MMScrollRect _scrollRect;
  [Header("Character Traits")]
  [SerializeField]
  public RectTransform _characterTraitContent;
  [Header("Cult Traits")]
  [SerializeField]
  public RectTransform _cultTraitContent;
  [Header("Template")]
  [SerializeField]
  public IndoctrinationTraitItem _traitItemTemplate;
  public List<IndoctrinationTraitItem> _traitItems = new List<IndoctrinationTraitItem>();
  public List<IndoctrinationTraitItem> _cultItems = new List<IndoctrinationTraitItem>();
  public Follower _follower;

  public void Configure(Follower follower) => this._follower = follower;

  public override void OnShowStarted()
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
