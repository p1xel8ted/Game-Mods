// Decompiled with JetBrains decompiler
// Type: FollowerInteractionAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class FollowerInteractionAlerts : AlertCategory<FollowerCommands>
{
  public FollowerInteractionAlerts()
  {
    Inventory.OnItemAddedToInventory += new Inventory.ItemAddedToInventory(this.OnInventoryItemAdded);
    StructureManager.OnStructureAdded += new StructureManager.StructureChanged(this.OnStructureAdded);
    DoctrineUpgradeSystem.OnDoctrineUnlocked += new Action<DoctrineUpgradeSystem.DoctrineType>(this.OnDoctrineUnlocked);
  }

  void object.Finalize()
  {
    try
    {
      Inventory.OnItemAddedToInventory -= new Inventory.ItemAddedToInventory(this.OnInventoryItemAdded);
      StructureManager.OnStructureAdded -= new StructureManager.StructureChanged(this.OnStructureAdded);
      DoctrineUpgradeSystem.OnDoctrineUnlocked -= new Action<DoctrineUpgradeSystem.DoctrineType>(this.OnDoctrineUnlocked);
    }
    finally
    {
      // ISSUE: explicit finalizer call
      base.Finalize();
    }
  }

  public void OnInventoryItemAdded(InventoryItem.ITEM_TYPE itemType, int delta)
  {
    if (!InventoryItem.IsGiftOrNecklace(itemType) || !this.AddOnce(this.CommandForGift(itemType)))
      return;
    this.Add(FollowerCommands.Gift);
  }

  public FollowerCommands CommandForGift(InventoryItem.ITEM_TYPE gift)
  {
    switch (gift)
    {
      case InventoryItem.ITEM_TYPE.GIFT_SMALL:
        return FollowerCommands.Gift_Small;
      case InventoryItem.ITEM_TYPE.GIFT_MEDIUM:
        return FollowerCommands.Gift_Medium;
      case InventoryItem.ITEM_TYPE.Necklace_1:
        return FollowerCommands.Gift_Necklace1;
      case InventoryItem.ITEM_TYPE.Necklace_2:
        return FollowerCommands.Gift_Necklace2;
      case InventoryItem.ITEM_TYPE.Necklace_3:
        return FollowerCommands.Gift_Necklace3;
      case InventoryItem.ITEM_TYPE.Necklace_4:
        return FollowerCommands.Gift_Necklace4;
      case InventoryItem.ITEM_TYPE.Necklace_5:
        return FollowerCommands.Gift_Necklace5;
      case InventoryItem.ITEM_TYPE.Necklace_Loyalty:
        return FollowerCommands.Gift_Necklace_Loyalty;
      case InventoryItem.ITEM_TYPE.Necklace_Demonic:
        return FollowerCommands.Gift_Necklace_Demonic;
      case InventoryItem.ITEM_TYPE.Necklace_Dark:
        return FollowerCommands.Gift_Necklace_Dark;
      case InventoryItem.ITEM_TYPE.Necklace_Light:
        return FollowerCommands.Gift_Necklace_Light;
      case InventoryItem.ITEM_TYPE.Necklace_Missionary:
        return FollowerCommands.Gift_Necklace_Missionary;
      case InventoryItem.ITEM_TYPE.Necklace_Gold_Skull:
        return FollowerCommands.Gift_Necklace_Gold_Skull;
      case InventoryItem.ITEM_TYPE.ANIMAL_GOAT:
        return FollowerCommands.Give_Goat_Pet;
      case InventoryItem.ITEM_TYPE.ANIMAL_TURTLE:
        return FollowerCommands.Give_Turtle_Pet;
      case InventoryItem.ITEM_TYPE.ANIMAL_CRAB:
        return FollowerCommands.Give_Crab_Pet;
      case InventoryItem.ITEM_TYPE.ANIMAL_SPIDER:
        return FollowerCommands.Give_Spider_Pet;
      case InventoryItem.ITEM_TYPE.ANIMAL_SNAIL:
        return FollowerCommands.Give_Snail_Pet;
      case InventoryItem.ITEM_TYPE.Necklace_Deaths_Door:
        return FollowerCommands.Gift_Necklace_Deaths_Door;
      case InventoryItem.ITEM_TYPE.Necklace_Winter:
        return FollowerCommands.Gift_Necklace_Winter;
      case InventoryItem.ITEM_TYPE.Necklace_Frozen:
        return FollowerCommands.Gift_Necklace_Frozen;
      case InventoryItem.ITEM_TYPE.Necklace_Weird:
        return FollowerCommands.Gift_Necklace_Weird;
      case InventoryItem.ITEM_TYPE.Necklace_Targeted:
        return FollowerCommands.Gift_Necklace_Targeted;
      case InventoryItem.ITEM_TYPE.ANIMAL_COW:
        return FollowerCommands.Give_Cow_Pet;
      case InventoryItem.ITEM_TYPE.ANIMAL_LLAMA:
        return FollowerCommands.Give_Llama_Pet;
      default:
        return FollowerCommands.None;
    }
  }

  public void OnStructureAdded(StructuresData structuresData)
  {
    if (structuresData.Type == StructureBrain.TYPES.PRISON)
    {
      this.AddOnce(FollowerCommands.NoAvailablePrisons);
      this.AddOnce(FollowerCommands.Imprison);
    }
    if (structuresData.Type == StructureBrain.TYPES.SURVEILLANCE)
      this.AddOnce(FollowerCommands.Surveillance);
    if (structuresData.Type != StructureBrain.TYPES.KITCHEN && structuresData.Type != StructureBrain.TYPES.KITCHEN_II)
      return;
    this.AddOnce(FollowerCommands.Cook_2);
  }

  public void OnDoctrineUnlocked(DoctrineUpgradeSystem.DoctrineType doctrineType)
  {
    switch (doctrineType)
    {
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_Inspire:
        this.AddOnce(FollowerCommands.Dance);
        break;
      case DoctrineUpgradeSystem.DoctrineType.WorkWorship_Intimidate:
        this.AddOnce(FollowerCommands.Intimidate);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Possessions_ExtortTithes:
        this.AddOnce(FollowerCommands.ExtortMoney);
        break;
      case DoctrineUpgradeSystem.DoctrineType.Possessions_Bribe:
        this.AddOnce(FollowerCommands.Bribe);
        break;
      case DoctrineUpgradeSystem.DoctrineType.LawOrder_MurderFollower:
        this.AddOnce(FollowerCommands.Murder);
        break;
    }
  }
}
