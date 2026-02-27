// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Assets.CultUpgradeIconMapping
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Lamb.UI.Assets;

[CreateAssetMenu(fileName = "Cult Upgrade Icon Mapping", menuName = "Massive Monster/Cult Upgrade Icon Mapping", order = 1)]
public class CultUpgradeIconMapping : ScriptableObject
{
  [SerializeField]
  public List<CultUpgradeIconMapping.CultUpgradeSprite> _items;
  public Dictionary<CultUpgradeData.TYPE, Sprite> _itemMap;

  public void Initialise()
  {
    this._itemMap = new Dictionary<CultUpgradeData.TYPE, Sprite>();
    foreach (CultUpgradeIconMapping.CultUpgradeSprite cultUpgradeSprite in this._items)
      this._itemMap.Add(cultUpgradeSprite.CultUpgradeType, cultUpgradeSprite.Sprite);
  }

  public Sprite GetImage(CultUpgradeData.TYPE type)
  {
    if (type == CultUpgradeData.TYPE.Count)
      return (Sprite) null;
    if (this._itemMap == null || this._items.Count != this._itemMap.Keys.Count)
      this.Initialise();
    if (type == (CultUpgradeData.TYPE.Cult3 | CultUpgradeData.TYPE.Cult8))
      type = CultUpgradeData.TYPE.Cult10;
    Sprite sprite;
    return this._itemMap.TryGetValue(type, out sprite) ? sprite : (Sprite) null;
  }

  [Serializable]
  public class CultUpgradeSprite
  {
    public CultUpgradeData.TYPE CultUpgradeType;
    public Sprite Sprite;
  }
}
