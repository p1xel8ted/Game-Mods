// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Assets.UpgradeTypeMapping
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Lamb.UI.Assets;

[CreateAssetMenu(fileName = "Upgrade Type Icon Mapping", menuName = "Massive Monster/Upgrade Type Icon Mapping", order = 1)]
public class UpgradeTypeMapping : ScriptableObject
{
  [SerializeField]
  public List<UpgradeTypeMapping.SpriteItem> _upgradeImage;
  public Dictionary<UpgradeSystem.Type, UpgradeTypeMapping.SpriteItem> _upgradeMap;

  public void Initialise()
  {
    this._upgradeMap = new Dictionary<UpgradeSystem.Type, UpgradeTypeMapping.SpriteItem>();
    foreach (UpgradeTypeMapping.SpriteItem spriteItem in this._upgradeImage)
    {
      if (!this._upgradeMap.ContainsKey(spriteItem.UpgradeType))
        this._upgradeMap.Add(spriteItem.UpgradeType, spriteItem);
    }
  }

  public UpgradeTypeMapping.SpriteItem GetItem(UpgradeSystem.Type type)
  {
    if (this._upgradeMap == null || this._upgradeMap.Keys.Count != this._upgradeImage.Count)
      this.Initialise();
    UpgradeTypeMapping.SpriteItem spriteItem;
    return this._upgradeMap.TryGetValue(type, out spriteItem) ? spriteItem : (UpgradeTypeMapping.SpriteItem) null;
  }

  [Serializable]
  public class SpriteItem
  {
    [SerializeField]
    public UpgradeSystem.Type _ugpradeType;
    [SerializeField]
    public UpgradeSystem.Category _category;
    [SerializeField]
    public Sprite _sprite;

    public UpgradeSystem.Type UpgradeType => this._ugpradeType;

    public UpgradeSystem.Category Category => this._category;

    public Sprite Sprite => this._sprite;
  }
}
