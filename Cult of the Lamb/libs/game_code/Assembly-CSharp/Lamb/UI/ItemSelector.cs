// Decompiled with JetBrains decompiler
// Type: Lamb.UI.ItemSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public sealed class ItemSelector
{
  public const string kFishermanKey = "fisherman";
  public const string kFishermanWinterKey = "fisherman_winter";
  public const string kSellItemsKey = "sell";
  public const string kPlantSeedsKey = "plant_seeds";
  public const string kPlantSeedsSozoKey = "plant_seeds_sozo";
  public const string kStoreSeedsKey = "store_seeds";
  public const string kStoreFoodsKey = "store_foods";
  public const string kAddFuelKey = "addfuel";
  public const string kTheNightFishKey = "thenight_fish";
  public const string kTheNightHeartKey = "thenight_heart";
  public const string kSeedShopKey = "seed_shop";
  public const string kFarmPlotKey = "farm_plot";
  public const string kFertiliserKey = "fertiliser";
  public const string kFertiliserIncludingRotburnKey = "fertiliser_including_rotburn";
  public const string kAnimalsKey = "animals";
  public const string kGrassKey = "grass";
  public const string kMedicKey = "medic";
  public const string kToolshedKey = "toolshed";
  public const string kTrapKey = "trap";
  public const string kPlantFlowersKey = "plant_flower";
  public const string kDLCGhostWool = "DLCGhost_Wool";
  public const string kGofernonRotburn = "Gofernon_Rotburn";

  public enum Context
  {
    Give,
    Sell,
    Buy,
    Plant,
    Add,
    SetLabel,
    AddFertiliser,
    Bury,
  }

  public struct Params
  {
    public string Key;
    public ItemSelector.Context Context;
    public bool HideOnSelection;
    public Vector2 Offset;
    public bool HideQuantity;
    public bool ShowEmpty;
    public bool RequiresDiscovery;
    public bool PreventCancellation;
    public bool ShowCoins;
    public bool ShowProgressText;
    public bool DisableForceClose;
    public bool DontCache;
    public PlayerFarming AllowInputOnlyFromPlayer;
  }

  [MessagePackObject(false)]
  [Serializable]
  public sealed class Category
  {
    [Key(0)]
    public string Key;
    [Key(1)]
    public List<InventoryItem.ITEM_TYPE> TrackedItems = new List<InventoryItem.ITEM_TYPE>();
    [Key(2)]
    public InventoryItem.ITEM_TYPE MostRecentItem;
  }
}
