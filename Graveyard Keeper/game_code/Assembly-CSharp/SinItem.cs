// Decompiled with JetBrains decompiler
// Type: SinItem
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SinItem : MonoBehaviour
{
  public const string GREED_KEY = "is_greed_unlocked";
  public const string WRATH_KEY = "is_wrath_unlocked";
  public const string ENVY_KEY = "is_envy_unlocked";
  public const string PRIDE_KEY = "is_pride_unlocked";
  public const string GLUTTONY_KEY = "is_gluttony_unlocked";
  public const string SLOTH_KEY = "is_sloth_unlocked";
  public const string LUST_KEY = "is_lust_unlocked";
  [SerializeField]
  public SinItem.ItemType _item_type;
  [SerializeField]
  public BaseItemCellGUI _item_cell_gui;
  [SerializeField]
  [Space]
  public SoulPanelSkullBarGUI _skull_bar;

  public SinItem.ItemType item_type => this._item_type;

  public BaseItemCellGUI item_cell_gui => this._item_cell_gui;

  public static string GetOrganIdBySin(SinItem sin_item)
  {
    string organIdBySin = string.Empty;
    switch (sin_item.item_type)
    {
      case SinItem.ItemType.Greed:
        organIdBySin = "flesh";
        break;
      case SinItem.ItemType.Sloth:
        organIdBySin = "brain";
        break;
      case SinItem.ItemType.Lust:
        organIdBySin = "heart";
        break;
      case SinItem.ItemType.Pride:
        organIdBySin = "skin";
        break;
      case SinItem.ItemType.Wrath:
        organIdBySin = "blood";
        break;
      case SinItem.ItemType.Envy:
        organIdBySin = "intestine";
        break;
      case SinItem.ItemType.Gluttony:
        organIdBySin = "fat";
        break;
    }
    return organIdBySin;
  }

  public static Item GetSinItemByTypeFromItem(SinItem.ItemType item_type, Item from_item)
  {
    string str = string.Empty;
    switch (item_type)
    {
      case SinItem.ItemType.Greed:
        str = "sin_greed";
        break;
      case SinItem.ItemType.Sloth:
        str = "sin_sloth";
        break;
      case SinItem.ItemType.Lust:
        str = "sin_lust";
        break;
      case SinItem.ItemType.Pride:
        str = "sin_pride";
        break;
      case SinItem.ItemType.Wrath:
        str = "sin_wrath";
        break;
      case SinItem.ItemType.Envy:
        str = "sin_envy";
        break;
      case SinItem.ItemType.Gluttony:
        str = "sin_gluttony";
        break;
    }
    foreach (Item itemByTypeFromItem in from_item.inventory)
    {
      string id = itemByTypeFromItem.id;
      if (itemByTypeFromItem.id.Contains(":"))
        id = id.Split(':')[0];
      if (str == id)
        return itemByTypeFromItem;
    }
    return (Item) null;
  }

  public void SetSinValuesInSkulls(
    bool is_sin_item_set = false,
    int red_skulls_sin = 0,
    int white_skulls_sin = 0,
    int red_skulls_organ = 0,
    int white_skulls_organ = 0)
  {
    this._skull_bar.SetSkullValues(red_skulls_sin, white_skulls_sin, red_skulls_organ, white_skulls_organ);
  }

  public float GetHealRate() => this._skull_bar.GetSkullsFillRate();

  public bool IsSinUnlocked(WorldGameObject wgo)
  {
    string param_name = string.Empty;
    switch (this.item_type)
    {
      case SinItem.ItemType.Greed:
        param_name = "is_greed_unlocked";
        break;
      case SinItem.ItemType.Sloth:
        param_name = "is_sloth_unlocked";
        break;
      case SinItem.ItemType.Lust:
        param_name = "is_lust_unlocked";
        break;
      case SinItem.ItemType.Pride:
        param_name = "is_pride_unlocked";
        break;
      case SinItem.ItemType.Wrath:
        param_name = "is_wrath_unlocked";
        break;
      case SinItem.ItemType.Envy:
        param_name = "is_envy_unlocked";
        break;
      case SinItem.ItemType.Gluttony:
        param_name = "is_gluttony_unlocked";
        break;
    }
    return wgo.GetParam(param_name).EqualsTo(1f);
  }

  public enum ItemType
  {
    Greed,
    Sloth,
    Lust,
    Pride,
    Wrath,
    Envy,
    Gluttony,
  }
}
