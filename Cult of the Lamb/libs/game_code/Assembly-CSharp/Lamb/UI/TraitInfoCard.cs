// Decompiled with JetBrains decompiler
// Type: Lamb.UI.TraitInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class TraitInfoCard : UIInfoCardBase<FollowerTrait.TraitType>
{
  [SerializeField]
  public TextMeshProUGUI _traitTitle;
  [SerializeField]
  public TextMeshProUGUI _traitDescription;
  [SerializeField]
  public IndoctrinationTraitItem _traitItem;
  [CompilerGenerated]
  public FollowerTrait.TraitType \u003CTrait\u003Ek__BackingField;
  [CompilerGenerated]
  public FollowerBrain \u003CFollowerBrain\u003Ek__BackingField;

  public FollowerTrait.TraitType Trait
  {
    get => this.\u003CTrait\u003Ek__BackingField;
    set => this.\u003CTrait\u003Ek__BackingField = value;
  }

  public FollowerBrain FollowerBrain
  {
    get => this.\u003CFollowerBrain\u003Ek__BackingField;
    set => this.\u003CFollowerBrain\u003Ek__BackingField = value;
  }

  public override void Configure(FollowerTrait.TraitType trait)
  {
    this.Trait = trait;
    this._traitTitle.text = FollowerTrait.GetLocalizedTitle(trait);
    this._traitDescription.text = FollowerTrait.GetLocalizedDescription(trait, this.FollowerBrain);
    this._traitItem.Configure(trait);
  }
}
