// Decompiled with JetBrains decompiler
// Type: FollowerCommandItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.FollowerInteractionWheel;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public sealed class FollowerCommandItems
{
  public static CommandItem CutTrees()
  {
    FollowerCommandItems.FollowerRoleCommandItem followerRoleCommandItem = new FollowerCommandItems.FollowerRoleCommandItem();
    followerRoleCommandItem.Command = FollowerCommands.CutTrees;
    followerRoleCommandItem.FollowerTaskType = FollowerTaskType.ChopTrees;
    return (CommandItem) followerRoleCommandItem;
  }

  public static CommandItem ClearRubble()
  {
    FollowerCommandItems.FollowerRoleCommandItem followerRoleCommandItem = new FollowerCommandItems.FollowerRoleCommandItem();
    followerRoleCommandItem.Command = FollowerCommands.ClearRubble;
    followerRoleCommandItem.FollowerTaskType = FollowerTaskType.ClearRubble;
    return (CommandItem) followerRoleCommandItem;
  }

  public static CommandItem MineRotstone()
  {
    FollowerCommandItems.FollowerRoleCommandItem followerRoleCommandItem = new FollowerCommandItems.FollowerRoleCommandItem();
    followerRoleCommandItem.Command = FollowerCommands.MineRotstone;
    followerRoleCommandItem.FollowerTaskType = FollowerTaskType.MineRotstone;
    return (CommandItem) followerRoleCommandItem;
  }

  public static CommandItem Farmer()
  {
    FollowerCommandItems.FollowerRoleCommandItem followerRoleCommandItem = new FollowerCommandItems.FollowerRoleCommandItem();
    followerRoleCommandItem.Command = FollowerCommands.Farmer_2;
    followerRoleCommandItem.FollowerTaskType = FollowerTaskType.Farm;
    return (CommandItem) followerRoleCommandItem;
  }

  public static CommandItem Builder()
  {
    FollowerCommandItems.FollowerRoleCommandItem followerRoleCommandItem = new FollowerCommandItems.FollowerRoleCommandItem();
    followerRoleCommandItem.Command = FollowerCommands.Build;
    followerRoleCommandItem.FollowerTaskType = FollowerTaskType.Build;
    return (CommandItem) followerRoleCommandItem;
  }

  public static CommandItem Handyman()
  {
    FollowerCommandItems.FollowerRoleCommandItem followerRoleCommandItem = new FollowerCommandItems.FollowerRoleCommandItem();
    followerRoleCommandItem.Command = FollowerCommands.Handyman;
    followerRoleCommandItem.FollowerTaskType = FollowerTaskType.Handyman;
    return (CommandItem) followerRoleCommandItem;
  }

  public static CommandItem TraitManipulator()
  {
    FollowerCommandItems.FollowerRoleCommandItem followerRoleCommandItem = new FollowerCommandItems.FollowerRoleCommandItem();
    followerRoleCommandItem.Command = FollowerCommands.TraitManipulator;
    followerRoleCommandItem.FollowerTaskType = FollowerTaskType.TraitManipulator;
    return (CommandItem) followerRoleCommandItem;
  }

  public static CommandItem Refiner()
  {
    FollowerCommandItems.FollowerRoleCommandItem followerRoleCommandItem = new FollowerCommandItems.FollowerRoleCommandItem();
    followerRoleCommandItem.Command = FollowerCommands.Refiner_2;
    followerRoleCommandItem.FollowerTaskType = FollowerTaskType.Refinery;
    return (CommandItem) followerRoleCommandItem;
  }

  public static CommandItem CollectTax()
  {
    FollowerCommandItems.CollectTaxCommandItem collectTaxCommandItem = new FollowerCommandItems.CollectTaxCommandItem();
    collectTaxCommandItem.Command = FollowerCommands.CollectTax;
    return (CommandItem) collectTaxCommandItem;
  }

  public static CommandItem Worship()
  {
    FollowerCommandItems.FollowerRoleCommandItem followerRoleCommandItem = new FollowerCommandItems.FollowerRoleCommandItem();
    followerRoleCommandItem.Command = FollowerCommands.WorshipAtShrine;
    followerRoleCommandItem.FollowerTaskType = FollowerTaskType.Pray;
    return (CommandItem) followerRoleCommandItem;
  }

  public static CommandItem Kiss()
  {
    FollowerCommandItems.KissCommandItem kissCommandItem = new FollowerCommandItems.KissCommandItem();
    kissCommandItem.Command = FollowerCommands.Romance;
    return (CommandItem) kissCommandItem;
  }

  public static CommandItem PetDog()
  {
    FollowerCommandItems.PetDogCommandItem petDogCommandItem = new FollowerCommandItems.PetDogCommandItem();
    petDogCommandItem.Command = FollowerCommands.PetDog;
    return (CommandItem) petDogCommandItem;
  }

  public static CommandItem PetFollower()
  {
    FollowerCommandItems.PetDogCommandItem petDogCommandItem = new FollowerCommandItems.PetDogCommandItem();
    petDogCommandItem.Command = FollowerCommands.PetFollower;
    return (CommandItem) petDogCommandItem;
  }

  public static CommandItem CuddleBaby()
  {
    FollowerCommandItems.CuddleCommandItem cuddleCommandItem = new FollowerCommandItems.CuddleCommandItem();
    cuddleCommandItem.Command = FollowerCommands.CuddleBaby;
    return (CommandItem) cuddleCommandItem;
  }

  public static CommandItem Bully()
  {
    FollowerCommandItems.ScaredTraitCommandItem traitCommandItem = new FollowerCommandItems.ScaredTraitCommandItem();
    traitCommandItem.Command = FollowerCommands.Bully;
    return (CommandItem) traitCommandItem;
  }

  public static CommandItem Reassure()
  {
    FollowerCommandItems.ScaredTraitCommandItem traitCommandItem = new FollowerCommandItems.ScaredTraitCommandItem();
    traitCommandItem.Command = FollowerCommands.Reassure;
    return (CommandItem) traitCommandItem;
  }

  public static CommandItem Janitor()
  {
    FollowerCommandItems.FollowerRoleCommandItem followerRoleCommandItem = new FollowerCommandItems.FollowerRoleCommandItem();
    followerRoleCommandItem.Command = FollowerCommands.Janitor_2;
    followerRoleCommandItem.FollowerTaskType = FollowerTaskType.Janitor;
    return (CommandItem) followerRoleCommandItem;
  }

  public static CommandItem Medic()
  {
    FollowerCommandItems.FollowerRoleCommandItem followerRoleCommandItem = new FollowerCommandItems.FollowerRoleCommandItem();
    followerRoleCommandItem.Command = FollowerCommands.Medic;
    followerRoleCommandItem.FollowerTaskType = FollowerTaskType.Medic;
    return (CommandItem) followerRoleCommandItem;
  }

  public static CommandItem Logistics()
  {
    FollowerCommandItems.FollowerRoleCommandItem followerRoleCommandItem = new FollowerCommandItems.FollowerRoleCommandItem();
    followerRoleCommandItem.Command = FollowerCommands.Logistics;
    followerRoleCommandItem.FollowerTaskType = FollowerTaskType.Logistics;
    return (CommandItem) followerRoleCommandItem;
  }

  public static CommandItem Rancher()
  {
    FollowerCommandItems.RancherRoleCommandItem rancherRoleCommandItem = new FollowerCommandItems.RancherRoleCommandItem();
    rancherRoleCommandItem.Command = FollowerCommands.Rancher;
    rancherRoleCommandItem.FollowerTaskType = FollowerTaskType.Rancher;
    return (CommandItem) rancherRoleCommandItem;
  }

  public static CommandItem Undertaker()
  {
    FollowerCommandItems.FollowerRoleCommandItem followerRoleCommandItem = new FollowerCommandItems.FollowerRoleCommandItem();
    followerRoleCommandItem.Command = FollowerCommands.Undertaker;
    followerRoleCommandItem.FollowerTaskType = FollowerTaskType.Undertaker;
    return (CommandItem) followerRoleCommandItem;
  }

  public static CommandItem BedRest()
  {
    return new CommandItem()
    {
      Command = FollowerCommands.BedRest
    };
  }

  public static CommandItem Sleep()
  {
    FollowerCommandItems.SleepCommandItem sleepCommandItem = new FollowerCommandItems.SleepCommandItem();
    sleepCommandItem.Command = FollowerCommands.Sleep;
    return (CommandItem) sleepCommandItem;
  }

  public static CommandItem Bless()
  {
    FollowerCommandItems.BlessCommandItem blessCommandItem = new FollowerCommandItems.BlessCommandItem();
    blessCommandItem.Command = FollowerCommands.Bless;
    return (CommandItem) blessCommandItem;
  }

  public static CommandItem Murder()
  {
    FollowerCommandItems.MurderCommandItem murderCommandItem = new FollowerCommandItems.MurderCommandItem();
    murderCommandItem.Command = FollowerCommands.Murder;
    murderCommandItem.SubCommands = FollowerCommandGroups.AreYouSureCommands();
    return (CommandItem) murderCommandItem;
  }

  public static CommandItem Surveillance()
  {
    return new CommandItem()
    {
      Command = FollowerCommands.Surveillance
    };
  }

  public static CommandItem Extort()
  {
    FollowerCommandItems.ExtortCommandItem extortCommandItem = new FollowerCommandItems.ExtortCommandItem();
    extortCommandItem.Command = FollowerCommands.ExtortMoney;
    return (CommandItem) extortCommandItem;
  }

  public static CommandItem Bribe()
  {
    FollowerCommandItems.BribeCommandItem bribeCommandItem = new FollowerCommandItems.BribeCommandItem();
    bribeCommandItem.Command = FollowerCommands.Bribe;
    return (CommandItem) bribeCommandItem;
  }

  public static CommandItem Dance()
  {
    FollowerCommandItems.DanceCommandItem danceCommandItem = new FollowerCommandItems.DanceCommandItem();
    danceCommandItem.Command = FollowerCommands.Dance;
    return (CommandItem) danceCommandItem;
  }

  public static CommandItem Intimidate()
  {
    FollowerCommandItems.IntimidateCommandItem intimidateCommandItem = new FollowerCommandItems.IntimidateCommandItem();
    intimidateCommandItem.Command = FollowerCommands.Intimidate;
    return (CommandItem) intimidateCommandItem;
  }

  public static CommandItem EatSomething(Follower follower)
  {
    CommandItem commandItem = new CommandItem()
    {
      Command = FollowerCommands.EatSomething
    };
    if (!FollowerBrainStats.Fasting)
    {
      List<InventoryItem.ITEM_TYPE> availableMeals = new List<InventoryItem.ITEM_TYPE>();
      foreach (StructureBrain structureBrain in StructureManager.GetAllStructuresOfType<Structures_Meal>(in PlayerFarming.Location))
      {
        InventoryItem.ITEM_TYPE mealType = StructuresData.GetMealType(structureBrain.Data.Type);
        if (!availableMeals.Contains(mealType))
          availableMeals.Add(mealType);
      }
      foreach (Structures_Kitchen structuresKitchen in StructureManager.GetAllStructuresOfType<Structures_Kitchen>(in PlayerFarming.Location))
      {
        foreach (InventoryItem inventoryItem in structuresKitchen.FoodStorage.Data.Inventory)
        {
          if (!availableMeals.Contains((InventoryItem.ITEM_TYPE) inventoryItem.type))
            availableMeals.Add((InventoryItem.ITEM_TYPE) inventoryItem.type);
        }
      }
      if (availableMeals.Count > 0)
        commandItem.SubCommands = FollowerCommandGroups.MealCommands(availableMeals);
    }
    return commandItem;
  }

  public static CommandItem DrinkSomething(Follower follower)
  {
    CommandItem commandItem = new CommandItem()
    {
      Command = FollowerCommands.DrinkSomething
    };
    List<InventoryItem.ITEM_TYPE> availableDrinks = new List<InventoryItem.ITEM_TYPE>();
    foreach (Structures_Pub structuresPub in StructureManager.GetAllStructuresOfType<Structures_Pub>())
    {
      foreach (InventoryItem inventoryItem in structuresPub.FoodStorage.Data.Inventory)
      {
        if (inventoryItem != null && !availableDrinks.Contains((InventoryItem.ITEM_TYPE) inventoryItem.type) && inventoryItem.QuantityReserved <= 0)
          availableDrinks.Add((InventoryItem.ITEM_TYPE) inventoryItem.type);
      }
    }
    if (availableDrinks.Count > 0)
      commandItem.SubCommands = FollowerCommandGroups.DrinkCommands(availableDrinks);
    return commandItem;
  }

  public static CommandItem Gift(Follower follower)
  {
    return new CommandItem()
    {
      Command = FollowerCommands.Gift,
      SubCommands = FollowerCommandGroups.GiftCommands(follower)
    };
  }

  public static CommandItem Gift_Small()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.GIFT_SMALL);
    giftCommandItem.Command = FollowerCommands.Gift_Small;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem Gift_Medium()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.GIFT_MEDIUM);
    giftCommandItem.Command = FollowerCommands.Gift_Medium;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem Gift_Necklace1()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.Necklace_1);
    giftCommandItem.Command = FollowerCommands.Gift_Necklace1;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem Gift_Necklace2()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.Necklace_2);
    giftCommandItem.Command = FollowerCommands.Gift_Necklace2;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem Gift_Necklace3()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.Necklace_3);
    giftCommandItem.Command = FollowerCommands.Gift_Necklace3;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem Gift_Necklace4()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.Necklace_4);
    giftCommandItem.Command = FollowerCommands.Gift_Necklace4;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem Gift_Necklace5()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.Necklace_5);
    giftCommandItem.Command = FollowerCommands.Gift_Necklace5;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem Gift_Necklace_Dark()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.Necklace_Dark);
    giftCommandItem.Command = FollowerCommands.Gift_Necklace_Dark;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem Gift_Necklace_Light()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.Necklace_Light);
    giftCommandItem.Command = FollowerCommands.Gift_Necklace_Light;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem Gift_Necklace_Missionary()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.Necklace_Missionary);
    giftCommandItem.Command = FollowerCommands.Gift_Necklace_Missionary;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem Gift_Necklace_Demonic()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.Necklace_Demonic);
    giftCommandItem.Command = FollowerCommands.Gift_Necklace_Demonic;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem Gift_Necklace_Loyalty()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.Necklace_Loyalty);
    giftCommandItem.Command = FollowerCommands.Gift_Necklace_Loyalty;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem Gift_Necklace_Gold_Skull()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.Necklace_Gold_Skull);
    giftCommandItem.Command = FollowerCommands.Gift_Necklace_Gold_Skull;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem Gift_Necklace_Deaths_Door()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.Necklace_Deaths_Door);
    giftCommandItem.Command = FollowerCommands.Gift_Necklace_Deaths_Door;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem Gift_Necklace_Winter()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.Necklace_Winter);
    giftCommandItem.Command = FollowerCommands.Gift_Necklace_Winter;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem Gift_Necklace_Frozen()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.Necklace_Frozen);
    giftCommandItem.Command = FollowerCommands.Gift_Necklace_Frozen;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem Gift_Necklace_Weird()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.Necklace_Weird);
    giftCommandItem.Command = FollowerCommands.Gift_Necklace_Weird;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem Gift_Necklace_Targeted()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.Necklace_Targeted);
    giftCommandItem.Command = FollowerCommands.Gift_Necklace_Targeted;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem GiveAnimalPet(Follower follower)
  {
    return new CommandItem()
    {
      Command = FollowerCommands.Give_Animal_Pet,
      SubCommands = FollowerCommandGroups.GiveAnimalPetCommands(follower)
    };
  }

  public static CommandItem Give_Goat_Pet()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.ANIMAL_GOAT);
    giftCommandItem.Command = FollowerCommands.Give_Goat_Pet;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem Give_Cow_Pet()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.ANIMAL_COW);
    giftCommandItem.Command = FollowerCommands.Give_Cow_Pet;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem Give_Spider_Pet()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.ANIMAL_SPIDER);
    giftCommandItem.Command = FollowerCommands.Give_Spider_Pet;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem Give_Llama_Pet()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.ANIMAL_LLAMA);
    giftCommandItem.Command = FollowerCommands.Give_Llama_Pet;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem Give_Snail_Pet()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.ANIMAL_SNAIL);
    giftCommandItem.Command = FollowerCommands.Give_Snail_Pet;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem Give_Crab_Pet()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.ANIMAL_CRAB);
    giftCommandItem.Command = FollowerCommands.Give_Crab_Pet;
    return (CommandItem) giftCommandItem;
  }

  public static CommandItem Give_Turtle_Pet()
  {
    FollowerCommandItems.GiftCommandItem giftCommandItem = new FollowerCommandItems.GiftCommandItem(InventoryItem.ITEM_TYPE.ANIMAL_TURTLE);
    giftCommandItem.Command = FollowerCommands.Give_Turtle_Pet;
    return (CommandItem) giftCommandItem;
  }

  public static FollowerCommandItems.HideNecklaceCommandItem HideNecklace(
    InventoryItem.ITEM_TYPE Necklace)
  {
    FollowerCommandItems.HideNecklaceCommandItem necklaceCommandItem = new FollowerCommandItems.HideNecklaceCommandItem();
    necklaceCommandItem.Command = FollowerCommands.HideNecklace;
    necklaceCommandItem.SubCommand = FollowerCommands.Hide;
    necklaceCommandItem.Necklace = Necklace;
    return necklaceCommandItem;
  }

  public static FollowerCommandItems.ShowNecklaceCommandItem ShowNecklace(
    InventoryItem.ITEM_TYPE Necklace)
  {
    FollowerCommandItems.ShowNecklaceCommandItem necklaceCommandItem = new FollowerCommandItems.ShowNecklaceCommandItem();
    necklaceCommandItem.Command = FollowerCommands.ShowNecklace;
    necklaceCommandItem.SubCommand = FollowerCommands.Show;
    necklaceCommandItem.Necklace = Necklace;
    return necklaceCommandItem;
  }

  public static FollowerCommandItems.RemoveNecklaceCommandItem RemoveNecklace(
    InventoryItem.ITEM_TYPE Necklace)
  {
    FollowerCommandItems.RemoveNecklaceCommandItem necklaceCommandItem = new FollowerCommandItems.RemoveNecklaceCommandItem();
    necklaceCommandItem.Command = FollowerCommands.RemoveNecklace;
    necklaceCommandItem.SubCommand = FollowerCommands.RemoveItem;
    necklaceCommandItem.Necklace = Necklace;
    return necklaceCommandItem;
  }

  public static CommandItem Imprison()
  {
    return new CommandItem()
    {
      Command = FollowerCommands.Imprison
    };
  }

  public static CommandItem SendToDaycare()
  {
    return new CommandItem()
    {
      Command = FollowerCommands.SendToDaycare
    };
  }

  public static CommandItem Reeducate()
  {
    FollowerCommandItems.ReeducateCommandItem reeducateCommandItem = new FollowerCommandItems.ReeducateCommandItem();
    reeducateCommandItem.Command = FollowerCommands.Reeducate;
    return (CommandItem) reeducateCommandItem;
  }

  public static CommandItem NoAvailablePrisons()
  {
    return new CommandItem()
    {
      Command = FollowerCommands.NoAvailablePrisons
    };
  }

  public static CommandItem Cook()
  {
    FollowerCommandItems.FollowerRoleCommandItem followerRoleCommandItem = new FollowerCommandItems.FollowerRoleCommandItem();
    followerRoleCommandItem.Command = FollowerCommands.Cook_2;
    followerRoleCommandItem.FollowerTaskType = FollowerTaskType.Cook;
    return (CommandItem) followerRoleCommandItem;
  }

  public static CommandItem Brew()
  {
    FollowerCommandItems.FollowerRoleCommandItem followerRoleCommandItem = new FollowerCommandItems.FollowerRoleCommandItem();
    followerRoleCommandItem.Command = FollowerCommands.Brew;
    followerRoleCommandItem.FollowerTaskType = FollowerTaskType.Brew;
    return (CommandItem) followerRoleCommandItem;
  }

  public static FollowerCommandItems.LeaderCommandItem GiveLeaderSecretItem(
    FollowerLocation location)
  {
    FollowerCommandItems.LeaderCommandItem leaderCommandItem = new FollowerCommandItems.LeaderCommandItem();
    leaderCommandItem.LeaderLocation = location;
    leaderCommandItem.Command = FollowerCommands.GiveLeaderItem;
    return leaderCommandItem;
  }

  public static CommandItem ForageBerries()
  {
    FollowerCommandItems.FollowerRoleCommandItem followerRoleCommandItem = new FollowerCommandItems.FollowerRoleCommandItem();
    followerRoleCommandItem.Command = FollowerCommands.ForageBerries;
    followerRoleCommandItem.FollowerTaskType = FollowerTaskType.Forage;
    return (CommandItem) followerRoleCommandItem;
  }

  public static CommandItem Meal()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.Meal;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealGrass()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealGrass;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealPoop()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealPoop;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealMushrooms()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealMushrooms;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealFollowerMeat()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealFollowerMeat;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealGoodFish()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealGoodFish;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealGreat()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealGreat;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealMeat()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealMeat;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealGreatFish()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealGreatFish;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealEgg()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealEgg;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealBadFish()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealBadFish;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealBerries()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealBerries;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealDeadly()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealDeadly;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealMediumVeg()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealMediumVeg;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealMeatLow()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealMeatLow;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealMeatHigh()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealMeatHigh;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealMixedLow()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealMixedLow;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealMixedMedium()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealMixedMedium;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealMixedHigh()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealMixedHigh;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealSpicy()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealSpicy;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealMilkBad()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealMilkBad;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealMilkGood()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealMilkGood;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealMilkGreat()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealMilkGreat;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealSnowFruit()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealSnowFruit;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem MealBurnt()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.MealBurnt;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem DrinkBeer()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.DrinkBeer;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem DrinkCocktail()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.DrinkCocktail;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem DrinkEggnog()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.DrinkEggnog;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem DrinkGin()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.DrinkGin;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem DrinkMushroomJuice()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.DrinkMushroomJuice;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem DrinkPoopJuice()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.DrinkPoopJuice;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem DrinkWine()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.DrinkWine;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem DrinkChilli()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.DrinkChilli;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem DrinkLightning()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.DrinkLightning;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem DrinkSin()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.DrinkSin;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem DrinkGrass()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.DrinkGrass;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem DrinkMilkshake()
  {
    FollowerCommandItems.FoodCommandItem foodCommandItem = new FollowerCommandItems.FoodCommandItem();
    foodCommandItem.Command = FollowerCommands.DrinkMilkshake;
    return (CommandItem) foodCommandItem;
  }

  public static CommandItem AreYouSureYes()
  {
    FollowerCommandItems.ConfirmationItem confirmationItem = new FollowerCommandItems.ConfirmationItem();
    confirmationItem.Command = FollowerCommands.AreYouSureYes;
    return (CommandItem) confirmationItem;
  }

  public static CommandItem AreYouSureNo()
  {
    FollowerCommandItems.ConfirmationItem confirmationItem = new FollowerCommandItems.ConfirmationItem();
    confirmationItem.Command = FollowerCommands.AreYouSureNo;
    return (CommandItem) confirmationItem;
  }

  public static CommandItem WakeUp()
  {
    return new CommandItem()
    {
      Command = FollowerCommands.WakeUp
    };
  }

  public static CommandItem MakeDemand(Follower follower)
  {
    return new CommandItem()
    {
      Command = FollowerCommands.MakeDemand,
      SubCommands = FollowerCommandGroups.MakeDemandCommands(follower)
    };
  }

  public static CommandItem GiveWorkerCommand(Follower follower)
  {
    FollowerCommandItems.GiverWorkerCommandItem workerCommandItem1 = new FollowerCommandItems.GiverWorkerCommandItem();
    workerCommandItem1.Command = FollowerCommands.GiveWorkerCommand_2;
    FollowerCommandItems.GiverWorkerCommandItem workerCommandItem2 = workerCommandItem1;
    if (follower.Brain.Info.CursedState == Thought.None && ((Object) follower == (Object) null || !follower.Brain.HasTrait(FollowerTrait.TraitType.JiltedLover)))
      workerCommandItem2.SubCommands = FollowerCommandGroups.GiveWorkerCommands(follower);
    return (CommandItem) workerCommandItem2;
  }

  public static FollowerCommandItems.NextPageCommandItem NextPage()
  {
    FollowerCommandItems.NextPageCommandItem nextPageCommandItem = new FollowerCommandItems.NextPageCommandItem();
    nextPageCommandItem.Command = FollowerCommands.NextPage;
    nextPageCommandItem.SubCommands = new List<CommandItem>();
    return nextPageCommandItem;
  }

  public static FollowerCommandItems.PrevPageCommandItem PrevPage()
  {
    FollowerCommandItems.PrevPageCommandItem prevPageCommandItem = new FollowerCommandItems.PrevPageCommandItem();
    prevPageCommandItem.Command = FollowerCommands.PrevPage;
    prevPageCommandItem.SubCommands = new List<CommandItem>();
    return prevPageCommandItem;
  }

  public class FollowerRoleCommandItem : CommandItem
  {
    public FollowerTaskType FollowerTaskType;

    public override bool IsAvailable(Follower follower)
    {
      return FollowerBrain.IsTaskAvailable(this.FollowerTaskType) || this.FollowerTaskType == FollowerTaskType.Pray;
    }

    public override string GetLockedDescription(Follower follower)
    {
      return LocalizationManager.GetTranslation("FollowerInteractions/TaskNotAvailable");
    }
  }

  public class CollectTaxCommandItem : CommandItem
  {
    public override string GetTitle(Follower follower)
    {
      return follower.Brain._directInfoAccess.TaxCollected <= 0 ? LocalizationManager.GetTranslation("FollowerInteractions/NoTax") : $"{base.GetTitle(follower)} <sprite name=\"icon_blackgold\"> x{follower.Brain._directInfoAccess.TaxCollected.ToString()}";
    }
  }

  public class KissCommandItem : CommandItem
  {
    public override bool IsAvailable(Follower follower) => !follower.Brain.Stats.KissedAction;
  }

  public class PetDogCommandItem : CommandItem
  {
    public override bool IsAvailable(Follower follower) => !follower.Brain.Stats.PetDog;
  }

  public class CuddleCommandItem : CommandItem
  {
    public override bool IsAvailable(Follower follower)
    {
      return !follower.Brain.Stats.Cuddled && follower.Brain.Info.Age < 18;
    }

    public override string GetLockedDescription(Follower follower)
    {
      return follower.Brain.Info.Age >= 18 ? LocalizationManager.GetTranslation("UI/FollowerSelect/UnavailableTooOldForDaycare") : LocalizationManager.GetTranslation($"FollowerInteractions/{this.Command}/NotAvailable");
    }
  }

  public class ScaredTraitCommandItem : CommandItem
  {
    public override bool IsAvailable(Follower follower)
    {
      return !follower.Brain.Stats.ScaredTraitInteracted;
    }

    public override string GetDescription(Follower follower)
    {
      return follower.Brain.HasTrait(FollowerTrait.TraitType.Bastard) ? LocalizationManager.GetTranslation($"FollowerInteractions/{this.Command}/Description/Bastard") : base.GetDescription(follower);
    }
  }

  public class RancherRoleCommandItem : FollowerCommandItems.FollowerRoleCommandItem
  {
    public bool isAnyRanchOvercrowded;

    public override bool IsAvailable(Follower follower)
    {
      bool flag = FollowerBrain.IsTaskAvailable(this.FollowerTaskType);
      this.isAnyRanchOvercrowded = false;
      if (!flag)
      {
        foreach (Interaction_Ranch ranch in Interaction_Ranch.Ranches)
        {
          if (ranch.IsOvercrowded)
          {
            this.isAnyRanchOvercrowded = true;
            return flag;
          }
        }
      }
      return flag;
    }

    public override string GetLockedDescription(Follower follower)
    {
      return this.isAnyRanchOvercrowded ? LocalizationManager.GetTranslation("UI/RanchMenu/RequiresBiggerRanch") : LocalizationManager.GetTranslation("FollowerInteractions/TaskNotAvailable");
    }
  }

  public class SleepCommandItem : CommandItem
  {
    public override string GetTitle(Follower follower)
    {
      return follower.Brain.Info.IsDrunk ? LocalizationManager.GetTranslation("FollowerInteractions/SleepDrunk") : base.GetTitle(follower);
    }

    public override string GetDescription(Follower follower)
    {
      return follower.Brain.Info.IsDrunk ? $"{LocalizationManager.GetTranslation("FollowerInteractions/SleepDrunk/Description")}<br><sprite name=\"icon_Faith\">{" -25".Colour(StaticColors.RedColor)}" : base.GetDescription(follower);
    }
  }

  public class BlessCommandItem : CommandItem
  {
    public override string GetDescription(Follower follower)
    {
      return $"{base.GetDescription(follower)}<br><sprite name=\"icon_Faith\">{(" +" + FollowerThoughts.GetData(Thought.Cult_Bless).Modifier.ToString()).Colour(StaticColors.GreenColor)}";
    }

    public override bool IsAvailable(Follower follower) => !follower.Brain.Stats.ReceivedBlessing;
  }

  public class MurderCommandItem : CommandItem
  {
    public override string GetDescription(Follower follower)
    {
      return $"{base.GetDescription(follower)}<br><sprite name=\"icon_Faith\">{FollowerThoughts.GetData(Thought.Cult_Murder).Modifier.ToString().Colour(StaticColors.RedColor)}";
    }
  }

  public class ExtortCommandItem : CommandItem
  {
    public override bool IsAvailable(Follower follower) => !follower.Brain.Stats.PaidTithes;
  }

  public class BribeCommandItem : CommandItem
  {
    public override bool IsAvailable(Follower follower) => !follower.Brain.Stats.Bribed;
  }

  public class DanceCommandItem : CommandItem
  {
    public override string GetDescription(Follower follower)
    {
      return $"{base.GetDescription(follower)}<br><sprite name=\"icon_Faith\">{(" +" + FollowerThoughts.GetData(Thought.Cult_Inspire).Modifier.ToString()).Colour(StaticColors.GreenColor)}";
    }

    public override bool IsAvailable(Follower follower) => !follower.Brain.Stats.Inspired;
  }

  public class IntimidateCommandItem : CommandItem
  {
    public override bool IsAvailable(Follower follower) => !follower.Brain.Stats.Intimidated;
  }

  public class GiftCommandItem : CommandItem
  {
    public InventoryItem.ITEM_TYPE _itemType;

    public GiftCommandItem(InventoryItem.ITEM_TYPE itemType) => this._itemType = itemType;

    public override string GetTitle(Follower follower)
    {
      return $"{LocalizationManager.GetTranslation($"Inventory/{this._itemType}")} ({Inventory.GetItemQuantity(this._itemType)})";
    }
  }

  public class RemoveNecklaceCommandItem : CommandItem
  {
    public InventoryItem.ITEM_TYPE Necklace;

    public override string GetDescription(Follower follower)
    {
      return string.Format(base.GetDescription(follower), (object) InventoryItem.LocalizedName(this.Necklace));
    }

    public override string GetIcon() => FontImageNames.GetIconByType(this.Necklace);
  }

  public class HideNecklaceCommandItem : CommandItem
  {
    public InventoryItem.ITEM_TYPE Necklace;

    public override string GetDescription(Follower follower)
    {
      return $"{LocalizationManager.GetTranslation($"FollowerInteractions/{this.SubCommand}")} <color=#FFD201>{InventoryItem.LocalizedName(this.Necklace)}";
    }

    public override string GetIcon() => FontImageNames.GetIconByType(this.Necklace);
  }

  public class ShowNecklaceCommandItem : CommandItem
  {
    public InventoryItem.ITEM_TYPE Necklace;

    public override string GetDescription(Follower follower)
    {
      return $"{LocalizationManager.GetTranslation($"FollowerInteractions/{this.SubCommand}")} <color=#FFD201>{InventoryItem.LocalizedName(this.Necklace)}";
    }

    public override string GetIcon() => FontImageNames.GetIconByType(this.Necklace);
  }

  public class ReeducateCommandItem : CommandItem
  {
    public override bool IsAvailable(Follower follower) => !follower.Brain.Stats.ReeducatedAction;
  }

  public class FoodCommandItem : CommandItem
  {
    public override string GetTitle(Follower follower)
    {
      return LocalizationManager.GetTranslation($"CookingData/{CookingData.RecipeTypeFromFollowerCommand(this.Command)}/Name");
    }

    public override string GetDescription(Follower follower)
    {
      return LocalizationManager.GetTranslation($"CookingData/{CookingData.RecipeTypeFromFollowerCommand(this.Command)}/Description");
    }
  }

  public class LeaderCommandItem : CommandItem
  {
    public FollowerLocation LeaderLocation;

    public override string GetTitle(Follower follower)
    {
      return LocalizationManager.GetTranslation($"FollowerInteractions/{this.LeaderLocation}/SecretInteraction/Name");
    }

    public override string GetDescription(Follower follower)
    {
      return LocalizationManager.GetTranslation($"FollowerInteractions/{this.LeaderLocation}/SecretInteraction/Description");
    }
  }

  public class ConfirmationItem : CommandItem
  {
    public override string GetDescription(Follower follower) => "";
  }

  public class GiverWorkerCommandItem : CommandItem
  {
    public override bool IsAvailable(Follower follower)
    {
      if (follower?.Brain?.Info?.IsSnowman.GetValueOrDefault())
        return true;
      if (!FollowerBrainStats.ShouldWork)
        return false;
      if ((Object) follower == (Object) null)
        return true;
      return !follower.Brain.HasTrait(FollowerTrait.TraitType.JiltedLover) && follower.Brain.CanWork;
    }
  }

  public class NextPageCommandItem : CommandItem
  {
    public int PageNumber;
    public int TotalPageNumbers;

    public override string GetDescription(Follower follower)
    {
      return $"{this.PageNumber}/{this.TotalPageNumbers}";
    }
  }

  public class PrevPageCommandItem : CommandItem
  {
    public int PageNumber;
    public int TotalPageNumbers;

    public override bool IsAvailable(Follower follower) => true;

    public override string GetDescription(Follower follower)
    {
      return $"{this.PageNumber}/{this.TotalPageNumbers}";
    }
  }
}
