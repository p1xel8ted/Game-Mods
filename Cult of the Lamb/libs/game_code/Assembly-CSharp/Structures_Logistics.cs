// Decompiled with JetBrains decompiler
// Type: Structures_Logistics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.KitchenMenu;
using System.Collections.Generic;
using System.Linq;

#nullable disable
public class Structures_Logistics : StructureBrain, ITaskProvider
{
  public bool CheckOverrideComplete() => false;

  public void GetAvailableTasks(
    ScheduledActivity activity,
    SortedList<float, FollowerTask> sortedTasks)
  {
    if (PlayerFarming.Location == FollowerLocation.Church || activity != ScheduledActivity.Work)
      return;
    SortedList<float, FollowerTask> source = new SortedList<float, FollowerTask>();
    List<StructuresData.LogisticsSlot> logisticsSlotList = new List<StructuresData.LogisticsSlot>((IEnumerable<StructuresData.LogisticsSlot>) this.Data.LogisticSlots);
    for (int slot = 0; slot < logisticsSlotList.Count; ++slot)
    {
      FollowerTask task;
      if (this.HasTaskAvailable(slot, out int _, out int _, out task))
        source.Add((float) source.Count, task);
    }
    for (int index = source.Count - 1; index >= 0; --index)
    {
      KeyValuePair<float, FollowerTask> keyValuePair = source.ElementAt<KeyValuePair<float, FollowerTask>>(index);
      bool flag = true;
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (allBrain.CurrentTask != null && allBrain.CurrentTask is FollowerTask_LogisticsPorter currentTask && currentTask.RootStructureType == ((FollowerTask_LogisticsPorter) keyValuePair.Value).RootStructureType)
        {
          flag = false;
          break;
        }
      }
      if (flag)
        sortedTasks.Add(keyValuePair.Key, keyValuePair.Value);
    }
  }

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool HasTaskAvailable(
    int slot,
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.FARM_STATION && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.KITCHEN)
      return this.HasTaskAvailable_Farm_Kitchen(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.MORGUE_1 && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.BODY_PIT)
      return this.HasTaskAvailable_Morgue_BodyPit(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.OUTHOUSE && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.POOP_BUCKET)
      return this.HasTaskAvailable_Outhouse_SiloFertiliser(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.MORGUE_1 && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.CRYPT_1)
      return this.HasTaskAvailable_Morgue_Crypt(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.MORGUE_1 && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.COMPOST_BIN_DEAD_BODY)
      return this.HasTaskAvailable_Morgue_Compost(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.FARM_STATION && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.PUB)
      return this.HasTaskAvailable_Farm_Pub(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.FARM_STATION && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.SEED_BUCKET)
      return this.HasTaskAvailable_Farm_SiloSeeder(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.FARM_STATION && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.RANCH_TROUGH)
      return this.HasTaskAvailable_Farm_Trough(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.FARM_STATION && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.COMPOST_BIN)
      return this.HasTaskAvailable_Farm_Compost(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.FARM_STATION && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.MEDIC)
      return this.HasTaskAvailable_Farm_Medic(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.RANCH && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.WOOLY_SHACK)
      return this.HasTaskAvailable_Ranch_WoolyShack(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.COMPOST_BIN && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.POOP_BUCKET)
      return this.HasTaskAvailable_Compost_SiloFertiliser(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.COMPOST_BIN_DEAD_BODY && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.POOP_BUCKET)
      return this.HasTaskAvailable_DeadCompost_SiloFertiliser(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.LUMBERJACK_STATION && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.REFINERY)
      return this.HasTaskAvailable_Lumberjack_Refinery(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.BLOODSTONE_MINE && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.REFINERY)
      return this.HasTaskAvailable_StoneMine_Refinery(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.ROTSTONE_MINE && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.FURNACE_1)
      return this.HasTaskAvailable_RotstoneMine_Furnace(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.SCARECROW_2 && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.KITCHEN)
      return this.HasTaskAvailable_Scarecrow_Kitchen(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.REFINERY && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.PROPAGANDA_SPEAKER)
      return this.HasTaskAvailable_Refinery_PropagandaSpeaker(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.RANCH && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.REFINERY)
      return this.HasTaskAvailable_Ranch_Refinery(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.RANCH && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.SEED_BUCKET)
      return this.HasTaskAvailable_Ranch_SiloSeeder(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.WOLF_TRAP && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.KITCHEN)
      return this.HasTaskAvailable_WolfTrap_Kitchen(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.FURNACE_3 && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.REFINERY)
      return this.HasTaskAvailable_Furnace_Refinery(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.REFINERY && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.FURNACE_1)
      return this.HasTaskAvailable_Refinery_Furnace(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.LIGHTNING_ROD_2 && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.FARM_CROP_GROWER)
      return this.HasTaskAvailable_LightningRod_FarmCropGrower(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.LUMBERJACK_STATION && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.SHRINE && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Shrine_Flame))
      return this.HasTaskAvailable_Lumberjack_Shrine(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.LUMBERJACK_STATION && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.SHRINE_PASSIVE && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Shrine_PassiveShrinesFlames))
      return this.HasTaskAvailable_Lumberjack_PassiveShrine(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.LUMBERJACK_STATION && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.TOOLSHED)
      return this.HasTaskAvailable_Lumberjack_Toolshed(out rootStructure, out targetStructure, out task);
    if (this.Data.LogisticSlots[slot].RootStructureType == StructureBrain.TYPES.BLOODSTONE_MINE && this.Data.LogisticSlots[slot].TargetStructureType == StructureBrain.TYPES.TOOLSHED)
      return this.HasTaskAvailable_StoneMine_Toolshed(out rootStructure, out targetStructure, out task);
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    return false;
  }

  public bool HasTaskAvailable_Farm_Kitchen(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<StructureBrain> structuresOfType1 = StructureManager.GetAllStructuresOfType(FollowerLocation.Base, StructureBrain.TYPES.FARM_STATION_II);
    List<Structures_Kitchen> structuresOfType2 = StructureManager.GetAllStructuresOfType<Structures_Kitchen>();
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfType1.Count > 0 && structuresOfType2.Count > 0)
    {
      bool flag1 = false;
      foreach (StructureBrain structureBrain in structuresOfType1)
      {
        if (CookingData.CanMakeMeals(structureBrain.Data.Inventory))
        {
          rootStructure = structureBrain.Data.ID;
          flag1 = true;
          break;
        }
      }
      if (flag1)
      {
        bool flag2 = false;
        foreach (Structures_Kitchen structuresKitchen in structuresOfType2)
        {
          if (structuresKitchen.Data.QueuedMeals.Count < UIFollowerKitchenMenuController.kMaxItems)
          {
            targetStructure = structuresKitchen.Data.ID;
            flag2 = true;
            break;
          }
        }
        if (flag2)
        {
          task = (FollowerTask) new FollowerTask_KitchenPorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_Farm_Pub(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<StructureBrain> structuresOfType1 = StructureManager.GetAllStructuresOfType(FollowerLocation.Base, StructureBrain.TYPES.FARM_STATION_II);
    List<Structures_Pub> structuresOfType2 = StructureManager.GetAllStructuresOfType<Structures_Pub>();
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfType1.Count > 0 && structuresOfType2.Count > 0)
    {
      bool flag1 = false;
      foreach (StructureBrain structureBrain in structuresOfType1)
      {
        if (CookingData.CanMakeDrinks(structureBrain.Data.Inventory))
        {
          rootStructure = structureBrain.Data.ID;
          flag1 = true;
          break;
        }
      }
      if (flag1)
      {
        bool flag2 = false;
        foreach (Structures_Pub structuresPub in structuresOfType2)
        {
          if (structuresPub.Data.QueuedMeals.Count < structuresPub.MaxQueue)
          {
            targetStructure = structuresPub.Data.ID;
            flag2 = true;
            break;
          }
        }
        if (flag2)
        {
          task = (FollowerTask) new FollowerTask_PubPorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_Farm_SiloSeeder(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<StructureBrain> structuresOfType1 = StructureManager.GetAllStructuresOfType(FollowerLocation.Base, StructureBrain.TYPES.FARM_STATION_II);
    List<StructureBrain> structuresOfType2 = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.SEED_BUCKET);
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfType1.Count > 0 && structuresOfType2.Count > 0)
    {
      bool flag1 = false;
      foreach (StructureBrain structureBrain in structuresOfType1)
      {
        for (int index = 0; index < structureBrain.Data.Inventory.Count; ++index)
        {
          if (InventoryItem.AllSeeds.Contains((InventoryItem.ITEM_TYPE) structureBrain.Data.Inventory[index].type) && structureBrain.Data.Inventory[index].type != 35)
          {
            rootStructure = structureBrain.Data.ID;
            flag1 = true;
            break;
          }
        }
      }
      if (flag1)
      {
        bool flag2 = false;
        foreach (StructureBrain structureBrain in structuresOfType2)
        {
          if ((double) structureBrain.Data.Inventory.Count < (double) ((Structures_SiloSeed) structureBrain).Capacity)
          {
            targetStructure = structureBrain.Data.ID;
            flag2 = true;
            break;
          }
        }
        if (flag2)
        {
          task = (FollowerTask) new FollowerTask_SiloSeedPorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_Farm_Trough(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<StructureBrain> structuresOfType1 = StructureManager.GetAllStructuresOfType(FollowerLocation.Base, StructureBrain.TYPES.FARM_STATION_II);
    List<StructureBrain> structuresOfType2 = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.RANCH_TROUGH);
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfType1.Count > 0 && structuresOfType2.Count > 0)
    {
      bool flag1 = false;
      foreach (StructureBrain structureBrain in structuresOfType1)
      {
        for (int index = 0; index < structureBrain.Data.Inventory.Count; ++index)
        {
          if (structureBrain.Data.Inventory[index].type == 35)
          {
            rootStructure = structureBrain.Data.ID;
            flag1 = true;
            break;
          }
        }
      }
      if (flag1)
      {
        bool flag2 = false;
        foreach (StructureBrain structureBrain in structuresOfType2)
        {
          if (structureBrain.Data.Inventory.Count < ((Structures_RanchTrough) structureBrain).Capacity)
          {
            targetStructure = structureBrain.Data.ID;
            flag2 = true;
            break;
          }
        }
        if (flag2)
        {
          task = (FollowerTask) new FollowerTask_TroughPorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_Farm_Compost(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<StructureBrain> structuresOfType1 = StructureManager.GetAllStructuresOfType(FollowerLocation.Base, StructureBrain.TYPES.FARM_STATION_II);
    List<StructureBrain> structuresOfType2 = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.COMPOST_BIN);
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfType1.Count > 0 && structuresOfType2.Count > 0)
    {
      int num = 0;
      foreach (StructureBrain structureBrain in structuresOfType1)
      {
        for (int index = 0; index < structureBrain.Data.Inventory.Count; ++index)
        {
          if (structureBrain.Data.Inventory[index].type == 35)
          {
            rootStructure = structureBrain.Data.ID;
            num += structureBrain.Data.Inventory[index].quantity;
          }
        }
      }
      if (num >= 50)
      {
        bool flag = false;
        foreach (StructureBrain structureBrain in structuresOfType2)
        {
          if (((Structures_CompostBin) structureBrain).CompostCount <= 0 && ((Structures_CompostBin) structureBrain).PoopCount <= 0)
          {
            targetStructure = structureBrain.Data.ID;
            flag = true;
            break;
          }
        }
        if (flag)
        {
          task = (FollowerTask) new FollowerTask_CompostPorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_Farm_Medic(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<StructureBrain> structuresOfType1 = StructureManager.GetAllStructuresOfType(FollowerLocation.Base, StructureBrain.TYPES.FARM_STATION_II);
    List<StructureBrain> structuresOfType2 = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.MEDIC);
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfType1.Count > 0 && structuresOfType2.Count > 0)
    {
      bool flag1 = false;
      foreach (StructureBrain structureBrain in structuresOfType1)
      {
        for (int index = 0; index < structureBrain.Data.Inventory.Count; ++index)
        {
          if (structureBrain.Data.Inventory[index].type == 55)
          {
            rootStructure = structureBrain.Data.ID;
            flag1 = true;
            break;
          }
        }
      }
      if (flag1)
      {
        bool flag2 = false;
        foreach (StructureBrain structureBrain in structuresOfType2)
        {
          if (structureBrain.Data.Inventory.Count < ((Structures_Medic) structureBrain).Capacity)
          {
            targetStructure = structureBrain.Data.ID;
            flag2 = true;
            break;
          }
        }
        if (flag2)
        {
          task = (FollowerTask) new FollowerTask_MedicPorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_Ranch_WoolyShack(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<StructureBrain> structuresOfType1 = StructureManager.GetAllStructuresOfType(FollowerLocation.Base, StructureBrain.TYPES.RANCH_2);
    List<StructureBrain> structuresOfType2 = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.WOOLY_SHACK);
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfType1.Count > 0 && structuresOfType2.Count > 0)
    {
      int num = 0;
      foreach (StructureBrain structureBrain in structuresOfType1)
      {
        for (int index = 0; index < structureBrain.Data.Inventory.Count; ++index)
        {
          if (structureBrain.Data.Inventory[index].type == 165)
          {
            rootStructure = structureBrain.Data.ID;
            num += structureBrain.Data.Inventory[index].quantity;
          }
        }
      }
      if (num >= 5)
      {
        targetStructure = structuresOfType2[0].Data.ID;
        task = (FollowerTask) new FollowerTask_WoolyShackPorter(this.Data.ID, rootStructure, targetStructure);
        return true;
      }
    }
    return false;
  }

  public bool HasTaskAvailable_Outhouse_SiloFertiliser(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<Structures_Outhouse> structuresOfType1 = StructureManager.GetAllStructuresOfType<Structures_Outhouse>();
    List<StructureBrain> structuresOfType2 = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.POOP_BUCKET);
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfType1.Count > 0 && structuresOfType2.Count > 0)
    {
      bool flag1 = false;
      foreach (Structures_Outhouse structuresOuthouse in structuresOfType1)
      {
        for (int index = 0; index < structuresOuthouse.Data.Inventory.Count; ++index)
        {
          if (InventoryItem.AllPoops.Contains((InventoryItem.ITEM_TYPE) structuresOuthouse.Data.Inventory[index].type))
          {
            rootStructure = structuresOuthouse.Data.ID;
            flag1 = true;
            break;
          }
        }
      }
      if (flag1)
      {
        bool flag2 = false;
        foreach (StructureBrain structureBrain in structuresOfType2)
        {
          if ((double) structureBrain.Data.Inventory.Count < (double) ((Structures_SiloFertiliser) structureBrain).Capacity)
          {
            targetStructure = structureBrain.Data.ID;
            flag2 = true;
            break;
          }
        }
        if (flag2)
        {
          task = (FollowerTask) new FollowerTask_SiloPoopPorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_Compost_SiloFertiliser(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<Structures_CompostBin> structuresOfType1 = StructureManager.GetAllStructuresOfType<Structures_CompostBin>();
    List<StructureBrain> structuresOfType2 = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.POOP_BUCKET);
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfType1.Count > 0 && structuresOfType2.Count > 0)
    {
      bool flag1 = false;
      foreach (Structures_CompostBin structuresCompostBin in structuresOfType1)
      {
        for (int index = 0; index < structuresCompostBin.Data.Inventory.Count; ++index)
        {
          if (InventoryItem.AllPoops.Contains((InventoryItem.ITEM_TYPE) structuresCompostBin.Data.Inventory[index].type))
          {
            rootStructure = structuresCompostBin.Data.ID;
            flag1 = true;
            break;
          }
        }
      }
      if (flag1)
      {
        bool flag2 = false;
        foreach (StructureBrain structureBrain in structuresOfType2)
        {
          if ((double) structureBrain.Data.Inventory.Count < (double) ((Structures_SiloFertiliser) structureBrain).Capacity)
          {
            targetStructure = structureBrain.Data.ID;
            flag2 = true;
            break;
          }
        }
        if (flag2)
        {
          task = (FollowerTask) new FollowerTask_SiloPoopPorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_DeadCompost_SiloFertiliser(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<Structures_DeadBodyCompost> structuresOfType1 = StructureManager.GetAllStructuresOfType<Structures_DeadBodyCompost>();
    List<StructureBrain> structuresOfType2 = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.POOP_BUCKET);
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfType1.Count > 0 && structuresOfType2.Count > 0)
    {
      bool flag1 = false;
      foreach (Structures_DeadBodyCompost structuresDeadBodyCompost in structuresOfType1)
      {
        for (int index = 0; index < structuresDeadBodyCompost.Data.Inventory.Count; ++index)
        {
          if (InventoryItem.AllPoops.Contains((InventoryItem.ITEM_TYPE) structuresDeadBodyCompost.Data.Inventory[index].type))
          {
            rootStructure = structuresDeadBodyCompost.Data.ID;
            flag1 = true;
            break;
          }
        }
      }
      if (flag1)
      {
        bool flag2 = false;
        foreach (StructureBrain structureBrain in structuresOfType2)
        {
          if ((double) structureBrain.Data.Inventory.Count < (double) ((Structures_SiloFertiliser) structureBrain).Capacity)
          {
            targetStructure = structureBrain.Data.ID;
            flag2 = true;
            break;
          }
        }
        if (flag2)
        {
          task = (FollowerTask) new FollowerTask_SiloPoopPorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_Lumberjack_Refinery(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<StructureBrain> structuresOfTypes = StructureManager.GetAllStructuresOfTypes(StructureBrain.TYPES.LUMBERJACK_STATION, StructureBrain.TYPES.LUMBERJACK_STATION_2);
    List<Structures_Refinery> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Refinery>();
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfTypes.Count > 0 && structuresOfType.Count > 0)
    {
      bool flag1 = false;
      foreach (StructureBrain structureBrain in structuresOfTypes)
      {
        int num = 0;
        for (int index = 0; index < structureBrain.Data.Inventory.Count; ++index)
          num += structureBrain.Data.Inventory[index].quantity;
        if (num >= Structures_Refinery.GetCost(InventoryItem.ITEM_TYPE.LOG_REFINED)[0].CostValue)
        {
          rootStructure = structureBrain.Data.ID;
          flag1 = true;
          break;
        }
      }
      if (flag1)
      {
        bool flag2 = false;
        foreach (Structures_Refinery structuresRefinery in structuresOfType)
        {
          if (structuresRefinery.Data.QueuedResources.Count < (structuresRefinery.Data.Type == StructureBrain.TYPES.REFINERY ? 5 : 10))
          {
            targetStructure = structuresRefinery.Data.ID;
            flag2 = true;
            break;
          }
        }
        if (flag2)
        {
          task = (FollowerTask) new FollowerTask_RefineryPorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_StoneMine_Refinery(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<StructureBrain> structuresOfTypes = StructureManager.GetAllStructuresOfTypes(StructureBrain.TYPES.BLOODSTONE_MINE, StructureBrain.TYPES.BLOODSTONE_MINE_2);
    List<Structures_Refinery> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Refinery>();
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfTypes.Count > 0 && structuresOfType.Count > 0)
    {
      bool flag1 = false;
      foreach (StructureBrain structureBrain in structuresOfTypes)
      {
        int num = 0;
        for (int index = 0; index < structureBrain.Data.Inventory.Count; ++index)
          num += structureBrain.Data.Inventory[index].quantity;
        if (num >= Structures_Refinery.GetCost(InventoryItem.ITEM_TYPE.STONE_REFINED)[0].CostValue)
        {
          rootStructure = structureBrain.Data.ID;
          flag1 = true;
          break;
        }
      }
      if (flag1)
      {
        bool flag2 = false;
        foreach (Structures_Refinery structuresRefinery in structuresOfType)
        {
          if (structuresRefinery.Data.QueuedResources.Count < (structuresRefinery.Data.Type == StructureBrain.TYPES.REFINERY ? 5 : 10))
          {
            targetStructure = structuresRefinery.Data.ID;
            flag2 = true;
            break;
          }
        }
        if (flag2)
        {
          task = (FollowerTask) new FollowerTask_RefineryPorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_RotstoneMine_Furnace(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<StructureBrain> structuresOfTypes = StructureManager.GetAllStructuresOfTypes(StructureBrain.TYPES.ROTSTONE_MINE, StructureBrain.TYPES.ROTSTONE_MINE_2);
    List<Structures_Furnace> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Furnace>();
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfTypes.Count > 0 && structuresOfType.Count > 0)
    {
      bool flag1 = false;
      foreach (StructureBrain structureBrain in structuresOfTypes)
      {
        int num = 0;
        for (int index = 0; index < structureBrain.Data.Inventory.Count; ++index)
          num += structureBrain.Data.Inventory[index].quantity;
        if (num > 4)
        {
          rootStructure = structureBrain.Data.ID;
          flag1 = true;
          break;
        }
      }
      if (flag1)
      {
        bool flag2 = false;
        foreach (Structures_Furnace structuresFurnace in structuresOfType)
        {
          if (structuresFurnace.Data.Fuel < structuresFurnace.Data.MaxFuel)
          {
            targetStructure = structuresFurnace.Data.ID;
            flag2 = true;
            break;
          }
        }
        if (flag2)
        {
          task = (FollowerTask) new FollowerTask_FurnacePorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_Morgue_Crypt(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<Structures_Morgue> structuresOfType1 = StructureManager.GetAllStructuresOfType<Structures_Morgue>();
    List<Structures_Crypt> structuresOfType2 = StructureManager.GetAllStructuresOfType<Structures_Crypt>();
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfType1.Count > 0 && structuresOfType2.Count > 0)
    {
      bool flag1 = false;
      foreach (Structures_Morgue structuresMorgue in structuresOfType1)
      {
        if (structuresMorgue.Data.MultipleFollowerIDs.Count > 0)
        {
          rootStructure = structuresMorgue.Data.ID;
          flag1 = true;
          break;
        }
      }
      if (flag1)
      {
        bool flag2 = false;
        foreach (Structures_Crypt structuresCrypt in structuresOfType2)
        {
          if (structuresCrypt.Data.MultipleFollowerIDs.Count < structuresCrypt.DEAD_BODY_SLOTS)
          {
            targetStructure = structuresCrypt.Data.ID;
            flag2 = true;
            break;
          }
        }
        if (flag2)
        {
          task = (FollowerTask) new FollowerTask_CryptPorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_Morgue_Compost(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<Structures_Morgue> structuresOfType1 = StructureManager.GetAllStructuresOfType<Structures_Morgue>();
    List<Structures_DeadBodyCompost> structuresOfType2 = StructureManager.GetAllStructuresOfType<Structures_DeadBodyCompost>();
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfType1.Count > 0 && structuresOfType2.Count > 0)
    {
      bool flag1 = false;
      foreach (Structures_Morgue structuresMorgue in structuresOfType1)
      {
        if (structuresMorgue.Data.MultipleFollowerIDs.Count > 0)
        {
          rootStructure = structuresMorgue.Data.ID;
          flag1 = true;
          break;
        }
      }
      if (flag1)
      {
        bool flag2 = false;
        foreach (Structures_DeadBodyCompost structuresDeadBodyCompost in structuresOfType2)
        {
          if (structuresDeadBodyCompost.CompostCount <= 0 && structuresDeadBodyCompost.PoopCount <= 0)
          {
            targetStructure = structuresDeadBodyCompost.Data.ID;
            flag2 = true;
            break;
          }
        }
        if (flag2)
        {
          task = (FollowerTask) new FollowerTask_BodyCompostPorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_Morgue_BodyPit(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<Structures_Morgue> structuresOfType1 = StructureManager.GetAllStructuresOfType<Structures_Morgue>();
    List<Structures_Grave> structuresOfType2 = StructureManager.GetAllStructuresOfType<Structures_Grave>();
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfType1.Count > 0 && structuresOfType2.Count > 0)
    {
      bool flag1 = false;
      foreach (Structures_Morgue structuresMorgue in structuresOfType1)
      {
        if (structuresMorgue.Data.MultipleFollowerIDs.Count > 0)
        {
          rootStructure = structuresMorgue.Data.ID;
          flag1 = true;
          break;
        }
      }
      if (flag1)
      {
        bool flag2 = false;
        foreach (Structures_Grave structuresGrave in structuresOfType2)
        {
          if (structuresGrave.Data.FollowerID == -1)
          {
            targetStructure = structuresGrave.Data.ID;
            flag2 = true;
            break;
          }
        }
        if (flag2)
        {
          task = (FollowerTask) new FollowerTask_GravePorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_Scarecrow_Kitchen(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<Structures_Scarecrow> structuresOfType1 = StructureManager.GetAllStructuresOfType<Structures_Scarecrow>();
    List<Structures_Kitchen> structuresOfType2 = StructureManager.GetAllStructuresOfType<Structures_Kitchen>();
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfType1.Count > 0 && structuresOfType2.Count > 0)
    {
      bool flag1 = false;
      foreach (Structures_Scarecrow structuresScarecrow in structuresOfType1)
      {
        if (structuresScarecrow.HasBird)
        {
          rootStructure = structuresScarecrow.Data.ID;
          flag1 = true;
          break;
        }
      }
      if (flag1)
      {
        bool flag2 = false;
        foreach (Structures_Kitchen structuresKitchen in structuresOfType2)
        {
          if (structuresKitchen.Data.QueuedMeals.Count < UIFollowerKitchenMenuController.kMaxItems)
          {
            targetStructure = structuresKitchen.Data.ID;
            flag2 = true;
            break;
          }
        }
        if (flag2)
        {
          task = (FollowerTask) new FollowerTask_KitchenPorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_Refinery_PropagandaSpeaker(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<Structures_Refinery> structuresOfType1 = StructureManager.GetAllStructuresOfType<Structures_Refinery>();
    List<Structures_PropagandaSpeaker> structuresOfType2 = StructureManager.GetAllStructuresOfType<Structures_PropagandaSpeaker>();
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfType1.Count > 0 && structuresOfType2.Count > 0)
    {
      bool flag1 = false;
      foreach (Structures_Refinery structuresRefinery in structuresOfType1)
      {
        for (int index = 0; index < structuresRefinery.Data.Inventory.Count; ++index)
        {
          if (structuresRefinery.Data.Inventory[index].type == 86)
          {
            rootStructure = structuresRefinery.Data.ID;
            flag1 = true;
            break;
          }
        }
      }
      if (flag1)
      {
        bool flag2 = false;
        foreach (Structures_PropagandaSpeaker propagandaSpeaker in structuresOfType2)
        {
          if (propagandaSpeaker.Data.Fuel <= 0)
          {
            targetStructure = propagandaSpeaker.Data.ID;
            flag2 = true;
            break;
          }
        }
        if (flag2)
        {
          task = (FollowerTask) new FollowerTask_PropagandaPorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_Ranch_Refinery(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<StructureBrain> structuresOfType1 = StructureManager.GetAllStructuresOfType(FollowerLocation.Base, StructureBrain.TYPES.RANCH_2);
    List<Structures_Refinery> structuresOfType2 = StructureManager.GetAllStructuresOfType<Structures_Refinery>();
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfType1.Count > 0 && structuresOfType2.Count > 0)
    {
      foreach (StructureBrain structureBrain in structuresOfType1)
      {
        int num1 = 0;
        int num2 = 0;
        for (int index = 0; index < structureBrain.Data.Inventory.Count; ++index)
        {
          if (structureBrain.Data.Inventory[index].type == 90)
            num1 += structureBrain.Data.Inventory[index].quantity;
          if (structureBrain.Data.Inventory[index].type == 1)
            num2 += structureBrain.Data.Inventory[index].quantity;
        }
        if (num2 >= Structures_Refinery.GetCost(InventoryItem.ITEM_TYPE.LOG_REFINED)[0].CostValue || num1 >= Structures_Refinery.GetCost(InventoryItem.ITEM_TYPE.SILK_THREAD)[0].CostValue)
        {
          rootStructure = structureBrain.Data.ID;
          break;
        }
      }
      if (rootStructure != 0)
      {
        bool flag = false;
        foreach (Structures_Refinery structuresRefinery in structuresOfType2)
        {
          if (structuresRefinery.Data.QueuedResources.Count < (structuresRefinery.Data.Type == StructureBrain.TYPES.REFINERY ? 5 : 10))
          {
            targetStructure = structuresRefinery.Data.ID;
            flag = true;
            break;
          }
        }
        if (flag)
        {
          task = (FollowerTask) new FollowerTask_RefineryPorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_Ranch_SiloSeeder(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<StructureBrain> structuresOfType1 = StructureManager.GetAllStructuresOfType(FollowerLocation.Base, StructureBrain.TYPES.RANCH_2);
    List<StructureBrain> structuresOfType2 = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.SEED_BUCKET);
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfType1.Count > 0 && structuresOfType2.Count > 0)
    {
      bool flag1 = false;
      foreach (StructureBrain structureBrain in structuresOfType1)
      {
        for (int index = 0; index < structureBrain.Data.Inventory.Count; ++index)
        {
          if (InventoryItem.AllSeeds.Contains((InventoryItem.ITEM_TYPE) structureBrain.Data.Inventory[index].type))
          {
            rootStructure = structureBrain.Data.ID;
            flag1 = true;
            break;
          }
        }
      }
      if (flag1)
      {
        bool flag2 = false;
        foreach (StructureBrain structureBrain in structuresOfType2)
        {
          if ((double) structureBrain.Data.Inventory.Count < (double) ((Structures_SiloSeed) structureBrain).Capacity)
          {
            targetStructure = structureBrain.Data.ID;
            flag2 = true;
            break;
          }
        }
        if (flag2)
        {
          task = (FollowerTask) new FollowerTask_SiloSeedPorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_WolfTrap_Kitchen(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<StructureBrain> structuresOfType1 = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.WOLF_TRAP);
    List<Structures_Kitchen> structuresOfType2 = StructureManager.GetAllStructuresOfType<Structures_Kitchen>();
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfType1.Count > 0 && structuresOfType2.Count > 0)
    {
      bool flag1 = false;
      foreach (StructureBrain structureBrain in structuresOfType1)
      {
        if (structureBrain.Data.HasBird)
        {
          rootStructure = structureBrain.Data.ID;
          flag1 = true;
          break;
        }
      }
      if (flag1)
      {
        bool flag2 = false;
        foreach (Structures_Kitchen structuresKitchen in structuresOfType2)
        {
          if (structuresKitchen.Data.QueuedMeals.Count < UIFollowerKitchenMenuController.kMaxItems)
          {
            targetStructure = structuresKitchen.Data.ID;
            flag2 = true;
            break;
          }
        }
        if (flag2)
        {
          task = (FollowerTask) new FollowerTask_KitchenPorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_Furnace_Refinery(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<StructureBrain> structuresOfType1 = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.FURNACE_3);
    List<Structures_Refinery> structuresOfType2 = StructureManager.GetAllStructuresOfType<Structures_Refinery>();
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfType1.Count > 0 && structuresOfType2.Count > 0)
    {
      foreach (StructureBrain structureBrain in structuresOfType1)
      {
        int num = 0;
        for (int index = 0; index < structureBrain.Data.Inventory.Count; ++index)
        {
          if (structureBrain.Data.Inventory[index].type == 186)
          {
            num += structureBrain.Data.Inventory[index].quantity;
            if (num >= Structures_Refinery.GetCost(InventoryItem.ITEM_TYPE.MAGMA_STONE)[0].CostValue)
            {
              rootStructure = structureBrain.Data.ID;
              break;
            }
          }
        }
      }
      if (rootStructure != 0)
      {
        bool flag = false;
        foreach (Structures_Refinery structuresRefinery in structuresOfType2)
        {
          if (structuresRefinery.Data.QueuedResources.Count < (structuresRefinery.Data.Type == StructureBrain.TYPES.REFINERY ? 5 : 10))
          {
            targetStructure = structuresRefinery.Data.ID;
            flag = true;
            break;
          }
        }
        if (flag)
        {
          task = (FollowerTask) new FollowerTask_RefineryPorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_Refinery_Furnace(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<Structures_Refinery> structuresOfType1 = StructureManager.GetAllStructuresOfType<Structures_Refinery>();
    List<Structures_Furnace> structuresOfType2 = StructureManager.GetAllStructuresOfType<Structures_Furnace>();
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfType1.Count > 0 && structuresOfType2.Count > 0)
    {
      bool flag1 = false;
      foreach (Structures_Refinery structuresRefinery in structuresOfType1)
      {
        for (int index = 0; index < structuresRefinery.Data.Inventory.Count; ++index)
        {
          if (structuresRefinery.Data.Inventory[index].type == 172)
          {
            rootStructure = structuresRefinery.Data.ID;
            flag1 = true;
            break;
          }
        }
      }
      if (flag1)
      {
        bool flag2 = false;
        foreach (Structures_Furnace structuresFurnace in structuresOfType2)
        {
          if (structuresFurnace.Data.MaxFuel - structuresFurnace.Data.Fuel >= InventoryItem.FuelWeight(InventoryItem.ITEM_TYPE.MAGMA_STONE))
          {
            flag2 = true;
            targetStructure = structuresFurnace.Data.ID;
            break;
          }
        }
        if (flag2)
        {
          task = (FollowerTask) new FollowerTask_FurnacePorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_LightningRod_FarmCropGrower(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<StructureBrain> structuresOfType1 = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.LIGHTNING_ROD_2);
    List<Structures_FarmCropGrower> structuresOfType2 = StructureManager.GetAllStructuresOfType<Structures_FarmCropGrower>();
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfType1.Count > 0 && structuresOfType2.Count > 0)
    {
      foreach (StructureBrain structureBrain in structuresOfType1)
      {
        for (int index = 0; index < structureBrain.Data.Inventory.Count; ++index)
        {
          if (structureBrain.Data.Inventory[index].type == 139)
          {
            rootStructure = structureBrain.Data.ID;
            break;
          }
        }
      }
      if (rootStructure != 0)
      {
        bool flag = false;
        foreach (Structures_FarmCropGrower structuresFarmCropGrower in structuresOfType2)
        {
          if (!structuresFarmCropGrower.Data.FullyFueled)
          {
            flag = true;
            targetStructure = structuresFarmCropGrower.Data.ID;
            break;
          }
        }
        if (flag)
        {
          task = (FollowerTask) new FollowerTask_FarmCropGrowerPorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_Lumberjack_Shrine(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<StructureBrain> structuresOfTypes = StructureManager.GetAllStructuresOfTypes(StructureBrain.TYPES.LUMBERJACK_STATION, StructureBrain.TYPES.LUMBERJACK_STATION_2);
    List<Structures_Shrine> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Shrine>();
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfTypes.Count > 0 && structuresOfType.Count > 0)
    {
      foreach (StructureBrain structureBrain in structuresOfTypes)
      {
        for (int index = 0; index < structureBrain.Data.Inventory.Count; ++index)
        {
          if (structureBrain.Data.Inventory[index].type == 1)
          {
            rootStructure = structureBrain.Data.ID;
            break;
          }
        }
      }
      if (rootStructure != 0)
      {
        bool flag = false;
        foreach (Structures_Shrine structuresShrine in structuresOfType)
        {
          if (!structuresShrine.Data.FullyFueled)
          {
            flag = true;
            targetStructure = structuresShrine.Data.ID;
            break;
          }
        }
        if (flag)
        {
          task = (FollowerTask) new FollowerTask_ShrinePorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_Lumberjack_PassiveShrine(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<StructureBrain> structuresOfTypes1 = StructureManager.GetAllStructuresOfTypes(false, true, StructureBrain.TYPES.LUMBERJACK_STATION, StructureBrain.TYPES.LUMBERJACK_STATION_2);
    List<StructureBrain> structuresOfTypes2 = StructureManager.GetAllStructuresOfTypes(StructureBrain.TYPES.SHRINE_PASSIVE, StructureBrain.TYPES.SHRINE_PASSIVE_II, StructureBrain.TYPES.SHRINE_PASSIVE_III);
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfTypes1.Count > 0 && structuresOfTypes2.Count > 0)
    {
      foreach (StructureBrain structureBrain in structuresOfTypes1)
      {
        for (int index = 0; index < structureBrain.Data.Inventory.Count; ++index)
        {
          if (structureBrain.Data.Inventory[index].type == 1)
          {
            rootStructure = structureBrain.Data.ID;
            break;
          }
        }
      }
      if (rootStructure != 0)
      {
        bool flag = false;
        foreach (StructureBrain structureBrain in structuresOfTypes2)
        {
          if (!structureBrain.Data.FullyFueled)
          {
            flag = true;
            targetStructure = structureBrain.Data.ID;
            break;
          }
        }
        if (flag)
        {
          task = (FollowerTask) new FollowerTask_ShrinePorter(this.Data.ID, rootStructure, targetStructure);
          return true;
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_Lumberjack_Toolshed(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<StructureBrain> structuresOfTypes = StructureManager.GetAllStructuresOfTypes(StructureBrain.TYPES.LUMBERJACK_STATION, StructureBrain.TYPES.LUMBERJACK_STATION_2);
    List<Structures_Toolshed> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Toolshed>();
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfTypes.Count > 0 && structuresOfType.Count > 0)
    {
      foreach (StructureBrain structureBrain in structuresOfTypes)
      {
        for (int index = 0; index < structureBrain.Data.Inventory.Count; ++index)
        {
          if (structureBrain.Data.Inventory[index].type == 1)
          {
            rootStructure = structureBrain.Data.ID;
            break;
          }
        }
      }
      if (rootStructure != 0)
      {
        foreach (Structures_Toolshed structuresToolshed in structuresOfType)
        {
          if (structuresToolshed.Data.Inventory.Count < structuresToolshed.Capacity)
          {
            targetStructure = structuresToolshed.Data.ID;
            task = (FollowerTask) new FollowerTask_ToolshedPorter(this.Data.ID, rootStructure, targetStructure);
            return true;
          }
        }
      }
    }
    return false;
  }

  public bool HasTaskAvailable_StoneMine_Toolshed(
    out int rootStructure,
    out int targetStructure,
    out FollowerTask task)
  {
    List<StructureBrain> structuresOfTypes = StructureManager.GetAllStructuresOfTypes(StructureBrain.TYPES.BLOODSTONE_MINE, StructureBrain.TYPES.BLOODSTONE_MINE_2);
    List<Structures_Toolshed> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Toolshed>();
    task = (FollowerTask) null;
    rootStructure = 0;
    targetStructure = 0;
    if (structuresOfTypes.Count > 0 && structuresOfType.Count > 0)
    {
      foreach (StructureBrain structureBrain in structuresOfTypes)
      {
        for (int index = 0; index < structureBrain.Data.Inventory.Count; ++index)
        {
          if (structureBrain.Data.Inventory[index].type == 2)
          {
            rootStructure = structureBrain.Data.ID;
            break;
          }
        }
      }
      if (rootStructure != 0)
      {
        foreach (Structures_Toolshed structuresToolshed in structuresOfType)
        {
          if (structuresToolshed.Data.Inventory.Count < structuresToolshed.Capacity)
          {
            targetStructure = structuresToolshed.Data.ID;
            task = (FollowerTask) new FollowerTask_ToolshedPorter(this.Data.ID, rootStructure, targetStructure);
            return true;
          }
        }
      }
    }
    return false;
  }
}
