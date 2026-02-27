// Decompiled with JetBrains decompiler
// Type: Lamb.UI.TraitInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class TraitInfoCard : UIInfoCardBase<FollowerTrait.TraitType>
{
  [SerializeField]
  private TextMeshProUGUI _traitTitle;
  [SerializeField]
  private TextMeshProUGUI _traitDescription;
  [SerializeField]
  private IndoctrinationTraitItem _traitItem;

  public override void Configure(FollowerTrait.TraitType trait)
  {
    this._traitTitle.text = FollowerTrait.GetLocalizedTitle(trait);
    this._traitDescription.text = FollowerTrait.GetLocalizedDescription(trait).StripHtml();
    this._traitItem.Configure(trait);
  }
}
