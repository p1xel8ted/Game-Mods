// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Assets.UpgradeCategoryIconMapping
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Lamb.UI.Assets;

[CreateAssetMenu(fileName = "Upgrade Category Icon Mapping", menuName = "Massive Monster/Upgrade Category Icon Mapping", order = 1)]
public class UpgradeCategoryIconMapping : ScriptableObject
{
  [SerializeField]
  private List<UpgradeCategoryIconMapping.UpgradeCategoryColour> _itemImages;
  private Dictionary<UpgradeSystem.Category, Color> _itemMap;

  private void Initialise()
  {
    this._itemMap = new Dictionary<UpgradeSystem.Category, Color>();
    foreach (UpgradeCategoryIconMapping.UpgradeCategoryColour itemImage in this._itemImages)
    {
      if (!this._itemMap.ContainsKey(itemImage.Category))
        this._itemMap.Add(itemImage.Category, itemImage.Colour);
    }
  }

  public Color GetColor(UpgradeSystem.Category type)
  {
    if (this._itemMap == null)
      this.Initialise();
    Color color;
    return this._itemMap.TryGetValue(type, out color) ? color : StaticColors.OffWhiteColor;
  }

  public static string GetIcon(UpgradeSystem.Category category)
  {
    switch (category)
    {
      case UpgradeSystem.Category.ECONOMY:
        return "\uF81D";
      case UpgradeSystem.Category.FOLLOWERS:
        return "\uF683";
      case UpgradeSystem.Category.COMBAT:
      case UpgradeSystem.Category.P_WEAPON:
        return "\uF71C";
      case UpgradeSystem.Category.FAITH:
        return "\uF684";
      case UpgradeSystem.Category.ASTHETIC:
        return "\uF890";
      case UpgradeSystem.Category.DEATH:
        return "\uF714";
      case UpgradeSystem.Category.POOP:
        return "\uF619";
      case UpgradeSystem.Category.FARMING:
        return "\uF864";
      case UpgradeSystem.Category.SLEEP:
        return "\uF236";
      case UpgradeSystem.Category.ILLNESS:
        return "\uE074";
      case UpgradeSystem.Category.P_HEALTH:
        return "\uF004";
      case UpgradeSystem.Category.P_STRENGTH:
        return "\uF6DE";
      case UpgradeSystem.Category.P_CURSE:
        return "\uF6B8";
      case UpgradeSystem.Category.P_FERVOR:
        return "\uF7E4";
      default:
        return "";
    }
  }

  [Serializable]
  public class UpgradeCategoryColour
  {
    public UpgradeSystem.Category Category;
    public Color Colour;
  }
}
