// Decompiled with JetBrains decompiler
// Type: Lamb.UI.InventoryMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class InventoryMenu : UISubmenuBase
{
  [Header("Inventory Menu")]
  [SerializeField]
  public MMScrollRect _scrollRect;
  [Header("Currencies")]
  [SerializeField]
  public RectTransform _currenciesContainer;
  [SerializeField]
  public GameObject _noCurrencyText;
  [Header("Food")]
  [SerializeField]
  public RectTransform _foodContainer;
  [SerializeField]
  public GameObject _noFoodText;
  [Header("Items")]
  [SerializeField]
  public RectTransform _itemsContainer;
  [SerializeField]
  public GameObject _noItemsText;
  [Header("Templates")]
  [SerializeField]
  public GenericInventoryItem _inventoryItemTemplate;
  public List<GenericInventoryItem> _currencies = new List<GenericInventoryItem>();
  public List<GenericInventoryItem> _food = new List<GenericInventoryItem>();
  public List<GenericInventoryItem> _items = new List<GenericInventoryItem>();
  public List<InventoryItem.ITEM_TYPE> _currencyFilter = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.GOLD_NUGGET,
    InventoryItem.ITEM_TYPE.BLACK_GOLD,
    InventoryItem.ITEM_TYPE.GOLD_REFINED,
    InventoryItem.ITEM_TYPE.LOG,
    InventoryItem.ITEM_TYPE.LOG_REFINED,
    InventoryItem.ITEM_TYPE.STONE,
    InventoryItem.ITEM_TYPE.STONE_REFINED,
    InventoryItem.ITEM_TYPE.BONE,
    InventoryItem.ITEM_TYPE.SHELL,
    InventoryItem.ITEM_TYPE.MONSTER_HEART,
    InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE,
    InventoryItem.ITEM_TYPE.TALISMAN,
    InventoryItem.ITEM_TYPE.BEHOLDER_EYE,
    InventoryItem.ITEM_TYPE.BEHOLDER_EYE_ROT,
    InventoryItem.ITEM_TYPE.GOD_TEAR_FRAGMENT,
    InventoryItem.ITEM_TYPE.GOD_TEAR,
    InventoryItem.ITEM_TYPE.PLEASURE_POINT,
    InventoryItem.ITEM_TYPE.LIGHTNING_SHARD,
    InventoryItem.ITEM_TYPE.MAGMA_STONE,
    InventoryItem.ITEM_TYPE.SOOT,
    InventoryItem.ITEM_TYPE.YEW_CURSED,
    InventoryItem.ITEM_TYPE.YEW_HOLY,
    InventoryItem.ITEM_TYPE.WOOL,
    InventoryItem.ITEM_TYPE.FORGE_FLAME,
    InventoryItem.ITEM_TYPE.YNGYA_GHOST,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_RANCHER,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_LAMBWAR,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_BLACKSMITH,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_TAROT,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_DECORATION,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_6,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_7,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_8,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_9,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_10,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_11
  };
  public List<InventoryItem.ITEM_TYPE> _foodSorter = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.GRASS,
    InventoryItem.ITEM_TYPE.SEED,
    InventoryItem.ITEM_TYPE.BERRY,
    InventoryItem.ITEM_TYPE.SEED_PUMPKIN,
    InventoryItem.ITEM_TYPE.PUMPKIN,
    InventoryItem.ITEM_TYPE.SEED_CAULIFLOWER,
    InventoryItem.ITEM_TYPE.CAULIFLOWER,
    InventoryItem.ITEM_TYPE.SEED_BEETROOT,
    InventoryItem.ITEM_TYPE.BEETROOT,
    InventoryItem.ITEM_TYPE.SEED_COTTON,
    InventoryItem.ITEM_TYPE.SEED_HOPS,
    InventoryItem.ITEM_TYPE.SEED_GRAPES,
    InventoryItem.ITEM_TYPE.SEED_SNOW_FRUIT,
    InventoryItem.ITEM_TYPE.SEED_SOZO,
    InventoryItem.ITEM_TYPE.SEED_CHILLI,
    InventoryItem.ITEM_TYPE.CHILLI,
    InventoryItem.ITEM_TYPE.POOP,
    InventoryItem.ITEM_TYPE.POOP_GLOW,
    InventoryItem.ITEM_TYPE.POOP_GOLD,
    InventoryItem.ITEM_TYPE.POOP_RAINBOW,
    InventoryItem.ITEM_TYPE.POOP_DEVOTION,
    InventoryItem.ITEM_TYPE.POOP_ROTSTONE,
    InventoryItem.ITEM_TYPE.MEAT_MORSEL,
    InventoryItem.ITEM_TYPE.MEAT,
    InventoryItem.ITEM_TYPE.FOLLOWER_MEAT,
    InventoryItem.ITEM_TYPE.YOLK,
    InventoryItem.ITEM_TYPE.MILK,
    InventoryItem.ITEM_TYPE.HOPS,
    InventoryItem.ITEM_TYPE.GRAPES,
    InventoryItem.ITEM_TYPE.SNOW_FRUIT,
    InventoryItem.ITEM_TYPE.FISH,
    InventoryItem.ITEM_TYPE.FISH_SMALL,
    InventoryItem.ITEM_TYPE.FISH_BIG,
    InventoryItem.ITEM_TYPE.FISH_CRAB,
    InventoryItem.ITEM_TYPE.FISH_LOBSTER,
    InventoryItem.ITEM_TYPE.FISH_OCTOPUS,
    InventoryItem.ITEM_TYPE.FISH_SWORDFISH,
    InventoryItem.ITEM_TYPE.FISH_BLOWFISH,
    InventoryItem.ITEM_TYPE.FISH_SQUID,
    InventoryItem.ITEM_TYPE.FISH_CATFISH,
    InventoryItem.ITEM_TYPE.FISH_PIKE,
    InventoryItem.ITEM_TYPE.FISH_COD,
    InventoryItem.ITEM_TYPE.ANIMAL_GOAT,
    InventoryItem.ITEM_TYPE.ANIMAL_COW,
    InventoryItem.ITEM_TYPE.ANIMAL_LLAMA,
    InventoryItem.ITEM_TYPE.ANIMAL_CRAB,
    InventoryItem.ITEM_TYPE.ANIMAL_SNAIL,
    InventoryItem.ITEM_TYPE.ANIMAL_SPIDER,
    InventoryItem.ITEM_TYPE.ANIMAL_TURTLE
  };
  public List<InventoryItem.ITEM_TYPE> _itemsSorter = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.SEED_FLOWER_RED,
    InventoryItem.ITEM_TYPE.FLOWER_RED,
    InventoryItem.ITEM_TYPE.FLOWER_WHITE,
    InventoryItem.ITEM_TYPE.FLOWER_PURPLE,
    InventoryItem.ITEM_TYPE.SEED_MUSHROOM,
    InventoryItem.ITEM_TYPE.MUSHROOM_SMALL,
    InventoryItem.ITEM_TYPE.CRYSTAL,
    InventoryItem.ITEM_TYPE.SPIDER_WEB,
    InventoryItem.ITEM_TYPE.SNOW_CHUNK,
    InventoryItem.ITEM_TYPE.SILK_THREAD,
    InventoryItem.ITEM_TYPE.COTTON,
    InventoryItem.ITEM_TYPE.BROKEN_WEAPON_HAMMER,
    InventoryItem.ITEM_TYPE.BROKEN_WEAPON_SWORD,
    InventoryItem.ITEM_TYPE.BROKEN_WEAPON_DAGGER,
    InventoryItem.ITEM_TYPE.BROKEN_WEAPON_AXE,
    InventoryItem.ITEM_TYPE.BROKEN_WEAPON_GAUNTLETS,
    InventoryItem.ITEM_TYPE.BROKEN_WEAPON_BLUNDERBUSS,
    InventoryItem.ITEM_TYPE.BROKEN_WEAPON_CHAIN,
    InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_HAMMER,
    InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_SWORD,
    InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_DAGGER,
    InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_GAUNTLETS,
    InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_BLUNDERBUSS,
    InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_CHAIN,
    InventoryItem.ITEM_TYPE.ILLEGIBLE_LETTER_SCYLLA,
    InventoryItem.ITEM_TYPE.ILLEGIBLE_LETTER_CHARYBDIS,
    InventoryItem.ITEM_TYPE.FISHING_ROD,
    InventoryItem.ITEM_TYPE.BOP,
    InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT,
    InventoryItem.ITEM_TYPE.Necklace_1,
    InventoryItem.ITEM_TYPE.Necklace_2,
    InventoryItem.ITEM_TYPE.Necklace_3,
    InventoryItem.ITEM_TYPE.Necklace_4,
    InventoryItem.ITEM_TYPE.Necklace_5,
    InventoryItem.ITEM_TYPE.Necklace_Demonic,
    InventoryItem.ITEM_TYPE.Necklace_Loyalty,
    InventoryItem.ITEM_TYPE.Necklace_Missionary,
    InventoryItem.ITEM_TYPE.Necklace_Gold_Skull,
    InventoryItem.ITEM_TYPE.Necklace_Light,
    InventoryItem.ITEM_TYPE.Necklace_Dark,
    InventoryItem.ITEM_TYPE.Necklace_Deaths_Door,
    InventoryItem.ITEM_TYPE.Necklace_Winter,
    InventoryItem.ITEM_TYPE.Necklace_Frozen,
    InventoryItem.ITEM_TYPE.Necklace_Weird,
    InventoryItem.ITEM_TYPE.Necklace_Targeted,
    InventoryItem.ITEM_TYPE.GIFT_SMALL,
    InventoryItem.ITEM_TYPE.GIFT_MEDIUM
  };

  public override void OnShowStarted()
  {
    this._scrollRect.enabled = false;
    if (this._items.Count + this._food.Count + this._currencies.Count == 0)
    {
      this.Populate(Inventory.items, this._currencies, this._currenciesContainer, this._noCurrencyText, this._currencyFilter, sorting: this._currencyFilter);
      this.Populate(Inventory.items, this._food, this._foodContainer, this._noFoodText, this._foodSorter, sorting: this._foodSorter);
      this.Populate(Inventory.items, this._items, this._itemsContainer, this._noItemsText, this._itemsSorter, sorting: this._itemsSorter);
      if (this._currencies.Count > 0)
        this.OverrideDefault((Selectable) this._currencies[0].Button);
      else if (this._food.Count > 0)
        this.OverrideDefault((Selectable) this._food[0].Button);
      else if (this._items.Count > 0)
        this.OverrideDefault((Selectable) this._items[0].Button);
      this.ActivateNavigation();
    }
    this._scrollRect.enabled = true;
    this._scrollRect.normalizedPosition = Vector2.one;
  }

  public void Populate(
    List<InventoryItem> items,
    List<GenericInventoryItem> destination,
    RectTransform container,
    GameObject noText,
    List<InventoryItem.ITEM_TYPE> filter = null,
    List<InventoryItem.ITEM_TYPE> ignore = null,
    List<InventoryItem.ITEM_TYPE> sorting = null)
  {
    List<InventoryItem.ITEM_TYPE> source = new List<InventoryItem.ITEM_TYPE>();
    foreach (InventoryItem inventoryItem in items)
    {
      if ((filter == null || filter.Contains((InventoryItem.ITEM_TYPE) inventoryItem.type)) && (ignore == null || !ignore.Contains((InventoryItem.ITEM_TYPE) inventoryItem.type)))
        source.Add((InventoryItem.ITEM_TYPE) inventoryItem.type);
    }
    if (sorting != null)
      source = source.OrderBy<InventoryItem.ITEM_TYPE, int>((Func<InventoryItem.ITEM_TYPE, int>) (x => sorting.IndexOf(x))).ToList<InventoryItem.ITEM_TYPE>();
    foreach (InventoryItem.ITEM_TYPE itemType in source)
      this.MakeItem(itemType, (Transform) container, destination);
    noText.SetActive(destination.Count == 0);
  }

  public GenericInventoryItem MakeItem(
    InventoryItem.ITEM_TYPE itemType,
    Transform container,
    List<GenericInventoryItem> destination)
  {
    GenericInventoryItem genericInventoryItem = this._inventoryItemTemplate.Instantiate<GenericInventoryItem>(container);
    genericInventoryItem.Button.Confirmable = false;
    genericInventoryItem.Configure(itemType);
    destination.Add(genericInventoryItem);
    return genericInventoryItem;
  }
}
