// Decompiled with JetBrains decompiler
// Type: Lamb.UI.IndoctrinationTraitItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class IndoctrinationTraitItem : MonoBehaviour
{
  [SerializeField]
  private MMSelectable _selectable;
  [SerializeField]
  private Image _icon;
  [SerializeField]
  private Image _arrow;
  [Header("Arrows")]
  [SerializeField]
  private Sprite _positiveArrow;
  [SerializeField]
  private Sprite _negativeArrow;
  private FollowerTrait.TraitType _traitType;

  public MMSelectable Selectable => this._selectable;

  public FollowerTrait.TraitType TraitType => this._traitType;

  public void Configure(FollowerTrait.TraitType traitType)
  {
    this._traitType = traitType;
    this._icon.sprite = FollowerTrait.GetIcon(this._traitType);
    this._arrow.sprite = FollowerTrait.IsPositiveTrait(this._traitType) ? this._positiveArrow : this._negativeArrow;
  }
}
