// Decompiled with JetBrains decompiler
// Type: AnimalCommandGroups
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.FollowerInteractionWheel;
using System.Collections.Generic;

#nullable disable
public sealed class AnimalCommandGroups
{
  public static List<CommandItem> DefaultCommands(StructuresData.Ranchable_Animal animal)
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    CommandItem commandItem = AnimalInteractionModel.Interact();
    if (animal.Age >= 2)
      commandItem.SubCommands.Add(AnimalInteractionModel.Ride());
    commandItem.SubCommands.Add(AnimalInteractionModel.Walk());
    commandItem.SubCommands.Add(AnimalInteractionModel.Pet());
    commandItem.SubCommands.Add(AnimalInteractionModel.Name());
    commandItemList.Add(commandItem);
    commandItemList.Add(AnimalInteractionModel.Harvest());
    if (animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_COW)
      commandItemList.Add(AnimalInteractionModel.MilkAnimal());
    commandItemList.Add((CommandItem) AnimalInteractionModel.EatSomething(animal));
    if (animal.Age >= 15)
      commandItemList.Add(AnimalInteractionModel.Ascend());
    return commandItemList;
  }

  public static List<CommandItem> OvercrowdedCommands(StructuresData.Ranchable_Animal animal)
  {
    return new List<CommandItem>()
    {
      AnimalInteractionModel.Walk(),
      (CommandItem) AnimalInteractionModel.EatSomething(animal)
    };
  }

  public static List<CommandItem> FeralCommands(StructuresData.Ranchable_Animal animal)
  {
    return new List<CommandItem>()
    {
      AnimalInteractionModel.Calm(),
      AnimalInteractionModel.Walk(),
      (CommandItem) AnimalInteractionModel.EatSomething(animal)
    };
  }

  public static List<CommandItem> InjuredCommands(
    StructuresData.Ranchable_Animal animal,
    InventoryItem.ITEM_TYPE FlowerType)
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    commandItemList.Add((CommandItem) AnimalInteractionModel.Heal(FlowerType));
    commandItemList.Add((CommandItem) AnimalInteractionModel.EatSomething(animal));
    commandItemList.Add(AnimalInteractionModel.Walk());
    commandItemList.Add(AnimalInteractionModel.Harvest());
    if (animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_COW)
      commandItemList.Add(AnimalInteractionModel.MilkAnimal());
    return commandItemList;
  }

  public static List<CommandItem> StinkyCommands(StructuresData.Ranchable_Animal animal)
  {
    return new List<CommandItem>()
    {
      AnimalInteractionModel.Clean()
    };
  }
}
