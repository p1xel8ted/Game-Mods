// Decompiled with JetBrains decompiler
// Type: AnimalInteractionModel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.FollowerInteractionWheel;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class AnimalInteractionModel
{
  public static CommandItem Kill()
  {
    AnimalInteractionModel.AnimalCommandItem animalCommandItem = new AnimalInteractionModel.AnimalCommandItem();
    animalCommandItem.Command = FollowerCommands.KillAnimal;
    animalCommandItem.SubCommands = new List<CommandItem>()
    {
      AnimalInteractionModel.Slaughter(),
      AnimalInteractionModel.Sacrifice()
    };
    return (CommandItem) animalCommandItem;
  }

  public static CommandItem Interact()
  {
    AnimalInteractionModel.AnimalCommandItem animalCommandItem = new AnimalInteractionModel.AnimalCommandItem();
    animalCommandItem.Command = FollowerCommands.InteractAnimal;
    animalCommandItem.SubCommands = new List<CommandItem>();
    return (CommandItem) animalCommandItem;
  }

  public static CommandItem Slaughter()
  {
    AnimalInteractionModel.SlaughterItem slaughterItem = new AnimalInteractionModel.SlaughterItem();
    slaughterItem.Command = FollowerCommands.Slaughter;
    slaughterItem.SubCommands = AnimalInteractionModel.AreYouSureCommands();
    return (CommandItem) slaughterItem;
  }

  public static CommandItem Sacrifice()
  {
    AnimalInteractionModel.SacrificeItem sacrificeItem = new AnimalInteractionModel.SacrificeItem();
    sacrificeItem.Command = FollowerCommands.Sacrifice;
    sacrificeItem.SubCommands = AnimalInteractionModel.AreYouSureCommands();
    return (CommandItem) sacrificeItem;
  }

  public static List<CommandItem> AreYouSureCommands()
  {
    return new List<CommandItem>()
    {
      AnimalInteractionModel.AreYouSureYes(),
      AnimalInteractionModel.AreYouSureNo()
    };
  }

  public static CommandItem AreYouSureYes()
  {
    AnimalInteractionModel.AnimalCommandItem animalCommandItem = new AnimalInteractionModel.AnimalCommandItem();
    animalCommandItem.Command = FollowerCommands.AreYouSureYes;
    return (CommandItem) animalCommandItem;
  }

  public static CommandItem AreYouSureNo()
  {
    AnimalInteractionModel.AnimalCommandItem animalCommandItem = new AnimalInteractionModel.AnimalCommandItem();
    animalCommandItem.Command = FollowerCommands.AreYouSureNo;
    return (CommandItem) animalCommandItem;
  }

  public static CommandItem Clean()
  {
    AnimalInteractionModel.AnimalCommandItem animalCommandItem = new AnimalInteractionModel.AnimalCommandItem();
    animalCommandItem.Command = FollowerCommands.Clean;
    return (CommandItem) animalCommandItem;
  }

  public static CommandItem Calm()
  {
    AnimalInteractionModel.CalmItem calmItem = new AnimalInteractionModel.CalmItem();
    calmItem.Command = FollowerCommands.Calm;
    return (CommandItem) calmItem;
  }

  public static AnimalInteractionModel.AnimalCommandItem Heal(InventoryItem.ITEM_TYPE FlowerType)
  {
    AnimalInteractionModel.HealItem healItem = new AnimalInteractionModel.HealItem();
    healItem.Command = FollowerCommands.Heal;
    healItem.Resource = FlowerType;
    healItem.Count = 3;
    return (AnimalInteractionModel.AnimalCommandItem) healItem;
  }

  public static CommandItem Pet()
  {
    AnimalInteractionModel.PetItem petItem = new AnimalInteractionModel.PetItem();
    petItem.Command = FollowerCommands.PetAnimal;
    return (CommandItem) petItem;
  }

  public static CommandItem Harvest()
  {
    AnimalInteractionModel.HarvestItem harvestItem = new AnimalInteractionModel.HarvestItem();
    harvestItem.Command = FollowerCommands.Harvest;
    return (CommandItem) harvestItem;
  }

  public static CommandItem MilkAnimal()
  {
    AnimalInteractionModel.MilkItem milkItem = new AnimalInteractionModel.MilkItem();
    milkItem.Command = FollowerCommands.MilkAnimal;
    return (CommandItem) milkItem;
  }

  public static CommandItem Walk()
  {
    AnimalInteractionModel.AnimalCommandItem animalCommandItem = new AnimalInteractionModel.AnimalCommandItem();
    animalCommandItem.Command = FollowerCommands.Walk;
    return (CommandItem) animalCommandItem;
  }

  public static CommandItem Name()
  {
    AnimalInteractionModel.AnimalCommandItem animalCommandItem = new AnimalInteractionModel.AnimalCommandItem();
    animalCommandItem.Command = FollowerCommands.NameAnimal;
    return (CommandItem) animalCommandItem;
  }

  public static CommandItem Ride()
  {
    AnimalInteractionModel.RideItem rideItem = new AnimalInteractionModel.RideItem();
    rideItem.Command = FollowerCommands.Ride;
    return (CommandItem) rideItem;
  }

  public static CommandItem Ascend()
  {
    AnimalInteractionModel.AscendItem ascendItem = new AnimalInteractionModel.AscendItem();
    ascendItem.Command = FollowerCommands.Ascend;
    ascendItem.SubCommands = AnimalInteractionModel.AreYouSureCommands();
    return (CommandItem) ascendItem;
  }

  public static CommandItem FollowPlayer()
  {
    AnimalInteractionModel.FollowPlayerItem followPlayerItem = new AnimalInteractionModel.FollowPlayerItem();
    followPlayerItem.Command = FollowerCommands.FollowPlayer;
    return (CommandItem) followPlayerItem;
  }

  public static CommandItem StopFollowingPlayer()
  {
    AnimalInteractionModel.StopFollowingPlayerItem followingPlayerItem = new AnimalInteractionModel.StopFollowingPlayerItem();
    followingPlayerItem.Command = FollowerCommands.StopFollowingPlayer;
    return (CommandItem) followingPlayerItem;
  }

  public static CommandItem FeedGrass(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedGrass;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.GRASS;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.GRASS == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedPoop(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedPoop;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.POOP;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.POOP == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedFollowerMeat(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedFollowerMeat;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.FOLLOWER_MEAT;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.FOLLOWER_MEAT == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedBerry(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedBerry;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.BERRY;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.BERRY == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedFlowerRed(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedFlowerRed;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.FLOWER_RED;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.FLOWER_RED == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedMushroom(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedMushroom;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.MUSHROOM_SMALL;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.MUSHROOM_SMALL == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedGrapes(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedGrapes;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.GRAPES;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.GRAPES == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedHops(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedHops;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.HOPS;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.HOPS == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedPumpkin(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedPumpkin;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.PUMPKIN;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.PUMPKIN == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedBeetroot(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedBeetroot;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.BEETROOT;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.BEETROOT == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedCauliflower(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedCauliflower;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.CAULIFLOWER;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.CAULIFLOWER == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedFish(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedFish;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.FISH;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.FISH == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedFishBig(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedFishBig;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.FISH_BIG;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.FISH_BIG == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedFishBlowfish(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedFishBlowfish;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.FISH_BLOWFISH;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.FISH_BLOWFISH == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedFishCrab(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedFishCrab;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.FISH_CRAB;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.FISH_CRAB == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedFishLobster(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedFishLobster;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.FISH_LOBSTER;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.FISH_LOBSTER == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedFishOctopus(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedFishOctopus;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.FISH_OCTOPUS;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.FISH_OCTOPUS == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedFishSmall(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedFishSmall;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.FISH_SMALL;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.FISH_SMALL == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedFishSquid(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedFishSquid;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.FISH_SQUID;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.FISH_SQUID == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedFishSwordfish(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedFishSwordfish;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.FISH_SWORDFISH;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.FISH_SWORDFISH == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedFishCod(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedFishCod;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.FISH_COD;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.FISH_COD == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedFishCatfish(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedFishCatfish;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.FISH_CATFISH;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.FISH_CATFISH == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedFishPike(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedFishPike;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.FISH_PIKE;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.FISH_PIKE == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedMeat(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedMeat;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.MEAT;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.MEAT == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedMeatMorsel(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedMeatMorsel;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.MEAT_MORSEL;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.MEAT_MORSEL == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedYolk(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedYolk;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.YOLK;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.YOLK == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static CommandItem FeedChilli(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem = new AnimalInteractionModel.FeedItem();
    feedItem.Command = FollowerCommands.FeedChilli;
    feedItem.FoodType = InventoryItem.ITEM_TYPE.CHILLI;
    feedItem.IsFavouriteFood = InventoryItem.ITEM_TYPE.CHILLI == animal.FavouriteFood;
    feedItem.IsFavouriteFoodRevealed = animal.IsFavouriteFoodRevealed;
    return (CommandItem) feedItem;
  }

  public static AnimalInteractionModel.FeedItem EatSomething(StructuresData.Ranchable_Animal animal)
  {
    AnimalInteractionModel.FeedItem feedItem1 = new AnimalInteractionModel.FeedItem();
    feedItem1.Command = FollowerCommands.FeedAnimal;
    AnimalInteractionModel.FeedItem feedItem2 = feedItem1;
    if (animal.EatenToday)
      return feedItem2;
    List<CommandItem> commandItemList = new List<CommandItem>();
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.GRASS) > 0)
      commandItemList.Add(AnimalInteractionModel.FeedGrass(animal));
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BERRY) > 0)
      commandItemList.Add(AnimalInteractionModel.FeedBerry(animal));
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.PUMPKIN) > 0)
      commandItemList.Add(AnimalInteractionModel.FeedPumpkin(animal));
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BEETROOT) > 0)
      commandItemList.Add(AnimalInteractionModel.FeedBeetroot(animal));
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.CAULIFLOWER) > 0)
      commandItemList.Add(AnimalInteractionModel.FeedCauliflower(animal));
    if (animal.Ailment == Interaction_Ranchable.Ailment.None && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.FOLLOWER_MEAT) > 0)
      commandItemList.Add(AnimalInteractionModel.FeedFollowerMeat(animal));
    feedItem2.SubCommands = commandItemList;
    return feedItem2;
  }

  public class AnimalCommandItem : CommandItem
  {
    public virtual string GetTitle(StructuresData.Ranchable_Animal animal)
    {
      return LocalizationManager.GetTranslation($"FollowerInteractions/{this.Command}");
    }

    public virtual string GetDescription(StructuresData.Ranchable_Animal animal)
    {
      return LocalizationManager.GetTranslation($"FollowerInteractions/{this.Command}/Description");
    }

    public virtual string GetLockedDescription(StructuresData.Ranchable_Animal animal)
    {
      return LocalizationManager.GetTranslation($"FollowerInteractions/{this.Command}/NotAvailable");
    }

    public virtual bool IsAvailable(StructuresData.Ranchable_Animal animal) => true;
  }

  public class SlaughterItem : AnimalInteractionModel.AnimalCommandItem
  {
    public override string GetDescription(StructuresData.Ranchable_Animal animal)
    {
      int meatCount = animal.GetMeatCount();
      string str1 = "Meat";
      string str2 = "<sprite name=\"icon_meat\">";
      if (animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_CRAB)
      {
        str1 = "Fish";
        str2 = "<sprite name=\"icon_Fish\">";
      }
      if (animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SNAIL)
      {
        str1 = "Veggies";
        str2 = "<sprite name=\"icon_Pumpkin\"><sprite name=\"icon_Beetroot\"><sprite name=\"icon_Cauliflower\">";
      }
      string translation = LocalizationManager.GetTranslation($"FollowerInteractions/Slaughter_{str1}/Description");
      string description;
      if (animal.Ailment == Interaction_Ranchable.Ailment.Feral)
        description = string.Format(translation, (object) $"{str2}<s>x{(meatCount / 2).ToString()}</s> {("x" + meatCount.ToString()).Colour(StaticColors.YellowColorHex)}");
      else
        description = string.Format(translation, (object) (str2 + ("x" + meatCount.ToString()).Colour(StaticColors.YellowColorHex)));
      return description;
    }
  }

  public class HarvestItem : AnimalInteractionModel.AnimalCommandItem
  {
    public override string GetTitle(StructuresData.Ranchable_Animal animal)
    {
      return animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_GOAT || animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_LLAMA ? LocalizationManager.GetTranslation("FollowerInteractions/Shear") : base.GetTitle(animal);
    }

    public override string GetDescription(StructuresData.Ranchable_Animal animal)
    {
      return Interaction_Ranchable.GetWorkDescription(animal);
    }

    public override string GetLockedDescription(StructuresData.Ranchable_Animal animal)
    {
      if (!animal.WorkedToday)
        return LocalizationManager.GetTranslation($"FollowerInteractions/{(animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_GOAT || animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_LLAMA ? "Shear" : "Harvest")}/NotReady");
      return animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_GOAT || animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_LLAMA ? LocalizationManager.GetTranslation("FollowerInteractions/Shear/NotAvailable") : base.GetLockedDescription(animal);
    }

    public override bool IsAvailable(StructuresData.Ranchable_Animal animal)
    {
      return !animal.WorkedToday && animal.WorkedReady;
    }
  }

  public class MilkItem : AnimalInteractionModel.AnimalCommandItem
  {
    public override string GetDescription(StructuresData.Ranchable_Animal animal)
    {
      int num = 1;
      string str = "<sprite name=\"icon_Milk\">";
      return string.Format(LocalizationManager.GetTranslation("FollowerInteractions/Milk/Description"), (object) (str + ("x" + num.ToString()).Colour(StaticColors.YellowColorHex)));
    }

    public override string GetLockedDescription(StructuresData.Ranchable_Animal animal)
    {
      return animal.MilkedToday ? base.GetLockedDescription(animal) : LocalizationManager.GetTranslation("FollowerInteractions/Milk/NotReady");
    }

    public override bool IsAvailable(StructuresData.Ranchable_Animal animal)
    {
      return !animal.MilkedToday && animal.MilkedReady;
    }
  }

  public class CalmItem : AnimalInteractionModel.AnimalCommandItem
  {
    public override string GetLockedDescription(StructuresData.Ranchable_Animal animal)
    {
      return LocalizationManager.GetTranslation("FollowerInteractions/Feral/AlreadyCalmed");
    }

    public override bool IsAvailable(StructuresData.Ranchable_Animal animal)
    {
      return (double) TimeManager.TotalElapsedGameTime > (double) animal.AilmentGameTime + 600.0;
    }

    public override string GetDescription(StructuresData.Ranchable_Animal animal)
    {
      return LocalizationManager.GetTranslation($"FollowerInteractions/{this.Command}/Description");
    }
  }

  public class HealItem : AnimalInteractionModel.RequiresResourceItem
  {
    public override string GetTitle(StructuresData.Ranchable_Animal animal)
    {
      return ScriptLocalization.Interactions.HealingStatue;
    }

    public override string GetDescription(StructuresData.Ranchable_Animal animal)
    {
      return $"{ScriptLocalization.Interactions.HealingStatue} {CostFormatter.FormatCost(this.Resource, 3)}";
    }

    public override string GetLockedDescription(StructuresData.Ranchable_Animal animal)
    {
      return $"{ScriptLocalization.Interactions.HealingStatue} {CostFormatter.FormatCost(this.Resource, 3)}";
    }
  }

  public class RequiresResourceItem : AnimalInteractionModel.AnimalCommandItem
  {
    public InventoryItem.ITEM_TYPE Resource;
    public int Count;

    public override string GetLockedDescription(StructuresData.Ranchable_Animal animal)
    {
      string lockedDescription;
      if (LocalizeIntegration.IsArabic())
        lockedDescription = $"{ScriptLocalization.Interactions.Requires}{this.Count.ToString()} / <color=red> {Inventory.GetItemQuantity(this.Resource).ToString()}</color>{FontImageNames.GetIconByType(this.Resource)}";
      else
        lockedDescription = $"{ScriptLocalization.Interactions.Requires}<color=red> {Inventory.GetItemQuantity(this.Resource).ToString()}</color> / {this.Count.ToString()}{FontImageNames.GetIconByType(this.Resource)}";
      return lockedDescription;
    }

    public override bool IsAvailable(StructuresData.Ranchable_Animal animal)
    {
      return Inventory.GetItemQuantity(this.Resource) >= this.Count;
    }

    public override string GetDescription(StructuresData.Ranchable_Animal animal)
    {
      return LocalizationManager.GetTranslation($"FollowerInteractions/{this.Command}/Description");
    }
  }

  public class SacrificeItem : AnimalInteractionModel.AnimalCommandItem
  {
    public override string GetDescription(StructuresData.Ranchable_Animal animal)
    {
      int meatCount = animal.GetMeatCount();
      return string.Format(LocalizationManager.GetTranslation($"FollowerInteractions/{this.Command}/Description"), (object) ("<sprite name=\"icon_bones\">" + ("x" + meatCount.ToString()).Colour(StaticColors.YellowColorHex)));
    }
  }

  public class PetItem : AnimalInteractionModel.AnimalCommandItem
  {
    public override bool IsAvailable(StructuresData.Ranchable_Animal animal)
    {
      return !animal.PetToday && animal.State != Interaction_Ranchable.State.EnteringHutch && animal.State != Interaction_Ranchable.State.InsideHutch;
    }
  }

  public class AscendItem : AnimalInteractionModel.AnimalCommandItem
  {
    public static float MULTIPLIER = 1.3f;
    public static float MULTIPLIER_RESOURCES = 2f;
    public static float REQUIRED_LEVEL = 5f;

    public override bool IsAvailable(StructuresData.Ranchable_Animal animal) => true;

    public override string GetLockedDescription(StructuresData.Ranchable_Animal animal)
    {
      string translation = LocalizationManager.GetTranslation("UI/UpgradeTree/Requires");
      string.Format(LocalizationManager.GetTranslation("UI/RanchAssignMenu/Old"), (object) "");
      string str = $"{LocalizationManager.GetTranslation("Interactions/Level")} {Mathf.RoundToInt(AnimalInteractionModel.AscendItem.REQUIRED_LEVEL).ToNumeral()}";
      return string.Format(translation, (object) str);
    }

    public override string GetDescription(StructuresData.Ranchable_Animal animal)
    {
      string str1 = "<sprite name=\"icon_meat\">";
      if (animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_CRAB)
        str1 = "<sprite name=\"icon_Fish\">";
      if (animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SNAIL)
        str1 = "<sprite name=\"icon_Pumpkin\"><sprite name=\"icon_Beetroot\"><sprite name=\"icon_Cauliflower\">";
      string str2 = "<sprite name=\"icon_Wool\">";
      if (animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_CRAB)
        str2 = "<sprite name=\"icon_Crystal\">";
      else if (animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_TURTLE)
        str2 = "<sprite name=\"icon_MagmaStone\">";
      else if (animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SNAIL)
        str2 = "<sprite name=\"icon_seed\">";
      else if (animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SPIDER)
        str2 = "<sprite name=\"icon_SpiderWeb\">";
      return string.Format(LocalizationManager.GetTranslation($"FollowerInteractions/{this.Command}/Description"), (object) $"{AnimalInteractionModel.AscendItem.MULTIPLIER.ToString()}x {str1}", (object) $"{AnimalInteractionModel.AscendItem.MULTIPLIER_RESOURCES.ToString()}x {str2}");
    }
  }

  public class RideItem : AnimalInteractionModel.AnimalCommandItem
  {
    public static float REQUIRED_LEVEL = 3f;

    public override bool IsAvailable(StructuresData.Ranchable_Animal animal)
    {
      return (double) animal.Level >= (double) AnimalInteractionModel.RideItem.REQUIRED_LEVEL;
    }

    public override string GetLockedDescription(StructuresData.Ranchable_Animal animal)
    {
      return string.Format(LocalizationManager.GetTranslation("UI/UpgradeTree/Requires"), (object) $"{LocalizationManager.GetTranslation("Interactions/Level")} {Mathf.RoundToInt(AnimalInteractionModel.RideItem.REQUIRED_LEVEL).ToNumeral()}");
    }
  }

  public class FeedItem : AnimalInteractionModel.AnimalCommandItem
  {
    public InventoryItem.ITEM_TYPE FoodType;
    public bool IsFavouriteFood;
    public bool IsFavouriteFoodRevealed;

    public override bool IsAvailable(StructuresData.Ranchable_Animal animal)
    {
      return !animal.EatenToday && (this.FoodType != InventoryItem.ITEM_TYPE.NONE || this.SubCommands != null && this.SubCommands.Count != 0);
    }

    public override string GetTitle(StructuresData.Ranchable_Animal animal)
    {
      return this.FoodType != InventoryItem.ITEM_TYPE.NONE ? string.Format(LocalizationManager.GetTranslation("FollowerInteractions/FeedAnimalTitle"), (object) InventoryItem.LocalizedName(this.FoodType)) : base.GetTitle(animal);
    }

    public override string GetDescription(StructuresData.Ranchable_Animal animal)
    {
      if (this.FoodType == InventoryItem.ITEM_TYPE.NONE)
        return base.GetDescription(animal);
      string description = string.Format(LocalizationManager.GetTranslation("FollowerInteractions/FeedAnimalDescription"), (object) InventoryItem.LocalizedName(this.FoodType), (object) FontImageNames.GetIconByType(this.FoodType));
      if (this.FoodType == InventoryItem.ITEM_TYPE.POOP)
        description = $"{description}<br>{string.Format(LocalizationManager.GetTranslation("FollowerInteractions/ChanceToCauseStinky"), (object) 25.ToString()).Colour(StaticColors.YellowColorHex)}";
      else if (this.FoodType == InventoryItem.ITEM_TYPE.FOLLOWER_MEAT)
        description = $"{description}<br>{string.Format(LocalizationManager.GetTranslation("FollowerInteractions/ChanceToCauseFeral"), (object) 100.ToString()).Colour(StaticColors.YellowColorHex)}";
      return description;
    }

    public override string GetSubIcon()
    {
      return !this.IsFavouriteFood || !this.IsFavouriteFoodRevealed ? base.GetSubIcon() : "\uF004";
    }

    public override string GetLockedDescription(StructuresData.Ranchable_Animal animal)
    {
      if (animal.EatenToday)
        return LocalizationManager.GetTranslation($"FollowerInteractions/{this.Command}/NotAvailable");
      return this.FoodType == InventoryItem.ITEM_TYPE.NONE && (this.SubCommands == null || this.SubCommands.Count == 0) ? LocalizationManager.GetTranslation($"FollowerInteractions/{this.Command}/Description") : LocalizationManager.GetTranslation($"FollowerInteractions/{this.Command}/Description");
    }
  }

  public class FollowPlayerItem : AnimalInteractionModel.AnimalCommandItem
  {
    public override bool IsAvailable(StructuresData.Ranchable_Animal animal)
    {
      for (int index = 0; index < DataManager.Instance.FollowingPlayerAnimals.Length; ++index)
      {
        if (DataManager.Instance.FollowingPlayerAnimals[index] == animal)
          return false;
      }
      return animal.Age >= 2;
    }
  }

  public class StopFollowingPlayerItem : AnimalInteractionModel.AnimalCommandItem
  {
    public override bool IsAvailable(StructuresData.Ranchable_Animal animal)
    {
      for (int index = 0; index < DataManager.Instance.FollowingPlayerAnimals.Length; ++index)
      {
        if (DataManager.Instance.FollowingPlayerAnimals[index] == animal)
          return true;
      }
      return false;
    }
  }
}
