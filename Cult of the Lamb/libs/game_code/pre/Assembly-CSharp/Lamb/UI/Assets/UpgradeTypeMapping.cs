// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Assets.UpgradeTypeMapping
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Lamb.UI.Assets;

[CreateAssetMenu(fileName = "Upgrade Type Icon Mapping", menuName = "Massive Monster/Upgrade Type Icon Mapping", order = 1)]
public class UpgradeTypeMapping : ScriptableObject
{
  [SerializeField]
  private List<UpgradeTypeMapping.SpriteItem> _upgradeImage;
  private Dictionary<UpgradeSystem.Type, UpgradeTypeMapping.SpriteItem> _upgradeMap;

  private void Initialise()
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
    private UpgradeSystem.Type _ugpradeType;
    [SerializeField]
    private UpgradeSystem.Category _category;
    [SerializeField]
    private Sprite _sprite;

    public UpgradeSystem.Type UpgradeType => this._ugpradeType;

    public UpgradeSystem.Category Category => this._category;

    public Sprite Sprite => this._sprite;
  }
}
