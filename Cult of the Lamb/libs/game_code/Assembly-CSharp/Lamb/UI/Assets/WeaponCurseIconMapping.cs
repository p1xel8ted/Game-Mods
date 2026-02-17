// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Assets.WeaponCurseIconMapping
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Lamb.UI.Assets;

[CreateAssetMenu(fileName = "Weapon Curse Icon Mapping", menuName = "Massive Monster/Weapon Curse Icon Mapping", order = 1)]
public class WeaponCurseIconMapping : ScriptableObject
{
  [SerializeField]
  public WeaponCurseIconMapping.SpriteItem[] _weaponSprites;

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
    public TarotCards.Card _itemType;
    [SerializeField]
    public Sprite _sprite;

    public TarotCards.Card ItemType => this._itemType;

    public Sprite Sprite => this._sprite;
  }
}
