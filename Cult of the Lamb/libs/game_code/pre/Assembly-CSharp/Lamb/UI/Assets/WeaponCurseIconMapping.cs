// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Assets.WeaponCurseIconMapping
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Lamb.UI.Assets;

[CreateAssetMenu(fileName = "Weapon Curse Icon Mapping", menuName = "Massive Monster/Weapon Curse Icon Mapping", order = 1)]
public class WeaponCurseIconMapping : ScriptableObject
{
  [SerializeField]
  private WeaponCurseIconMapping.SpriteItem[] _weaponSprites;

  public Sprite GetSprite(TarotCards.Card card)
  {
    foreach (WeaponCurseIconMapping.SpriteItem weaponSprite in this._weaponSprites)
    {
      if (weaponSprite.ItemType == card)
        return weaponSprite.Sprite;
    }
    return (Sprite) null;
  }

  [Serializable]
  public class SpriteItem
  {
    [SerializeField]
    private TarotCards.Card _itemType;
    [SerializeField]
    private Sprite _sprite;

    public TarotCards.Card ItemType => this._itemType;

    public Sprite Sprite => this._sprite;
  }
}
