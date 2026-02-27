// Decompiled with JetBrains decompiler
// Type: Lamb.UI.TraitInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class TraitInfoCardController : UIInfoCardController<TraitInfoCard, FollowerTrait.TraitType>
{
  public FollowerBrain followerBrain;

  public FollowerBrain FollowerBrain
  {
    get => this.followerBrain;
    set
    {
      this.followerBrain = value;
      this.Card1.FollowerBrain = this.followerBrain;
      this.Card2.FollowerBrain = this.followerBrain;
    }
  }

  public override bool IsSelectionValid(
    Selectable selectable,
    out FollowerTrait.TraitType showParam)
  {
    showParam = FollowerTrait.TraitType.None;
    IndoctrinationTraitItem component;
    if (!selectable.TryGetComponent<IndoctrinationTraitItem>(out component))
      return false;
    showParam = component.TraitType;
    return true;
  }
}
