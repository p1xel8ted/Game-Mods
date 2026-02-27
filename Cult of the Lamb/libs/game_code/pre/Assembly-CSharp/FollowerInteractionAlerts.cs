// Decompiled with JetBrains decompiler
// Type: FollowerInteractionAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class FollowerInteractionAlerts : AlertCategory<FollowerCommands>
{
  public FollowerInteractionAlerts()
  {
    Inventory.OnItemAddedToInventory += new Inventory.ItemAddedToInventory(this.OnInventoryItemAdded);
    StructureManager.OnStructureAdded += new StructureManager.StructureChanged(this.OnStructureAdded);
    DoctrineUpgradeSystem.OnDoctrineUnlocked += new Action<DoctrineUpgradeSystem.DoctrineType>(this.OnDoctrineUnlocked);
  }

  ~FollowerInteractionAlerts()
  {
    Inventory.OnItemAddedToInventory -= new Inventory.ItemAddedToInventory(this.OnInventoryItemAdded);
    StructureManager.OnStructureAdded -= new StructureManager.StructureChanged(this.OnStructureAdded);
    DoctrineUpgradeSystem.OnDoctrineUnlocked -= new Action<DoctrineUpgradeSystem.DoctrineType>(this.OnDoctrineUnlocked);
  }

  private void OnInventoryItemAdded(InventoryItem.ITEM_TYPE itemType, int Delta)
  {
    if (!InventoryItem.IsGift(itemType))
      return;
    this.AddOnce(FollowerCommands.Gift);
  }

  private void OnStructureAdded(StructuresData structuresData)
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

  private void OnDoctrineUnlocked(DoctrineUpgradeSystem.DoctrineType doctrineType)
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
