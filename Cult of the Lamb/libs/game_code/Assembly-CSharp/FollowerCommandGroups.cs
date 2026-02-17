// Decompiled with JetBrains decompiler
// Type: FollowerCommandGroups
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.FollowerInteractionWheel;
using System.Collections.Generic;

#nullable disable
public sealed class FollowerCommandGroups
{
  public static List<CommandItem> DefaultCommands(Follower follower)
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    if (follower.Brain._directInfoAccess.IsSnowman && follower.Brain.Info.CursedState == Thought.Child)
      return FollowerCommandGroups.SnowmanChildCommands(follower);
    if (follower.Brain._directInfoAccess.IsSnowman)
      return FollowerCommandGroups.SnowmanCommands(follower);
    if (follower.Brain.HasTrait(FollowerTrait.TraitType.Hibernation) && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && !follower.Brain._directInfoAccess.WorkThroughNight && follower.Brain.CurrentTaskType == FollowerTaskType.Sleep && follower.Brain.CurrentTask.State == FollowerTaskState.Doing || follower.Brain.HasTrait(FollowerTrait.TraitType.Aestivation) && SeasonsManager.CurrentSeason == SeasonsManager.Season.Spring && !follower.Brain._directInfoAccess.WorkThroughNight && follower.Brain.CurrentTaskType == FollowerTaskType.Sleep && follower.Brain.CurrentTask.State == FollowerTaskState.Doing)
      return FollowerCommandGroups.WakeUpCommands();
    if (follower.Brain.Info.CursedState == Thought.Child)
      return FollowerCommandGroups.BabyCommands(follower);
    if (follower.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
      return FollowerCommandGroups.ZombieCommands(follower);
    if ((follower.Brain.CurrentTaskType == FollowerTaskType.Sleep || follower.Brain.CurrentTaskType == FollowerTaskType.SleepBedRest) && follower.Brain.CurrentTask.State == FollowerTaskState.Doing)
      return follower.Brain.CurrentTaskType == FollowerTaskType.Sleep && ((FollowerTask_Sleep) follower.Brain.CurrentTask).isAwake ? FollowerCommandGroups.NormalCommands(follower) : FollowerCommandGroups.WakeUpCommands();
    if (follower.Brain.Info.CursedState == Thought.Dissenter)
      return FollowerCommandGroups.DissenterCommands(follower);
    if (follower.Brain.Info.CursedState == Thought.Ill)
      return FollowerCommandGroups.MakeDemandCommands(follower);
    if (follower.Brain.Info.IsDrunk)
      return FollowerCommandGroups.DrunkCommands(follower);
    if (follower.Brain.HasTrait(FollowerTrait.TraitType.ExistentialDread) || follower.Brain.HasTrait(FollowerTrait.TraitType.MissionaryTerrified))
      return FollowerCommandGroups.ExistentialDreadCommands(follower);
    if (follower.Brain.Info.CursedState == Thought.OldAge)
      return FollowerCommandGroups.OldAgeCommands(follower);
    if (follower.Brain.HasTrait(FollowerTrait.TraitType.Scared) || follower.Brain.HasTrait(FollowerTrait.TraitType.Terrified) || follower.Brain.Info.HasTrait(FollowerTrait.TraitType.CriminalScarred))
      return FollowerCommandGroups.ScaredCommands(follower);
    if (follower.Brain.HasTrait(FollowerTrait.TraitType.Poet) || follower.Brain.HasTrait(FollowerTrait.TraitType.FluteLover))
      return FollowerCommandGroups.PoetCommands(follower);
    if (follower.Brain.HasTrait(FollowerTrait.TraitType.Spy))
      return FollowerCommandGroups.SpyCommands(follower);
    return follower.Brain.HasTrait(FollowerTrait.TraitType.Mutated) ? FollowerCommandGroups.MutatedCommands(follower) : FollowerCommandGroups.NormalCommands(follower);
  }

  public static List<CommandItem> NormalCommands(Follower follower)
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    bool flag = true;
    if (follower.Brain.Info.ID == 99996 && !DataManager.Instance.SozoNoLongerBrainwashed)
      flag = false;
    if (flag)
      commandItemList.Add(FollowerCommandItems.GiveWorkerCommand(follower));
    if (DataManager.Instance.InTutorial)
      commandItemList.Add(FollowerCommandItems.MakeDemand(follower));
    if (follower.Brain.Info.TaxEnforcer)
      commandItemList.Add(FollowerCommandItems.CollectTax());
    if (DataManager.Instance.CanReadMinds)
      commandItemList.Add(FollowerCommandItems.Surveillance());
    if (follower.Brain.HasTrait(FollowerTrait.TraitType.Bastard) || follower.Brain.HasTrait(FollowerTrait.TraitType.CriminalHardened))
    {
      commandItemList.Add(FollowerCommandItems.Bully());
      commandItemList.Add(FollowerCommandItems.Reassure());
    }
    return commandItemList;
  }

  public static List<CommandItem> GiveWorkerCommands(Follower follower)
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    switch (Onboarding.CurrentPhase)
    {
      case DataManager.OnboardingPhase.Indoctrinate:
        commandItemList.Add(FollowerCommandItems.CutTrees());
        commandItemList.Add(FollowerCommandItems.ClearRubble());
        break;
      case DataManager.OnboardingPhase.Devotion:
        commandItemList.Add(FollowerCommandItems.Worship());
        break;
      case DataManager.OnboardingPhase.IndoctrinateBerriesAllowed:
        commandItemList.Add(FollowerCommandItems.CutTrees());
        commandItemList.Add(FollowerCommandItems.ClearRubble());
        if (FarmStation.FarmStations.Count > 0)
        {
          commandItemList.Add(FollowerCommandItems.Farmer());
          break;
        }
        break;
      default:
        if (DataManager.Instance.HasBuiltShrine1 && !follower.Brain.Info.HasTrait(FollowerTrait.TraitType.Mutated))
          commandItemList.Add(FollowerCommandItems.Worship());
        commandItemList.Add(FollowerCommandItems.CutTrees());
        commandItemList.Add(FollowerCommandItems.ClearRubble());
        commandItemList.Add(FollowerCommandItems.Builder());
        if (StructureManager.GetAllStructuresOfType<Structures_RotstoneStation>().Count > 0)
          commandItemList.Add(FollowerCommandItems.MineRotstone());
        if (FarmStation.FarmStations.Count > 0)
          commandItemList.Add(FollowerCommandItems.Farmer());
        if (Interaction_FollowerKitchen.FollowerKitchens.Count > 0)
          commandItemList.Add(FollowerCommandItems.Cook());
        if (JanitorStation.JanitorStations.Count > 0)
          commandItemList.Add(FollowerCommandItems.Janitor());
        if (Interaction_Medic.Medics.Count > 0)
          commandItemList.Add(FollowerCommandItems.Medic());
        if (StructureManager.GetAllStructuresOfType<Structures_Logistics>().Count > 0)
          commandItemList.Add(FollowerCommandItems.Logistics());
        if (StructureManager.GetAllStructuresOfType<Structures_Toolshed>().Count > 0)
          commandItemList.Add(FollowerCommandItems.Handyman());
        if (StructureManager.GetAllStructuresOfType<Structures_TraitManipulator>().Count > 0)
          commandItemList.Add(FollowerCommandItems.TraitManipulator());
        if (Interaction_Ranch.Ranches.Count > 0)
        {
          for (int index = 0; index < Interaction_Ranch.Ranches.Count; ++index)
          {
            if (Interaction_Ranch.Ranches[index].Brain.Data.Type == StructureBrain.TYPES.RANCH_2)
            {
              commandItemList.Add(FollowerCommandItems.Rancher());
              break;
            }
          }
        }
        if (Interaction_Refinery.Refineries.Count > 0)
          commandItemList.Add(FollowerCommandItems.Refiner());
        if (Interaction_Morgue.Morgues.Count > 0)
          commandItemList.Add(FollowerCommandItems.Undertaker());
        if (Interaction_Pub.Pubs.Count > 0)
        {
          commandItemList.Add(FollowerCommandItems.Brew());
          break;
        }
        break;
    }
    return commandItemList;
  }

  public static List<CommandItem> MealCommands(List<InventoryItem.ITEM_TYPE> availableMeals)
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL))
      commandItemList.Add(FollowerCommandItems.Meal());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_GRASS))
      commandItemList.Add(FollowerCommandItems.MealGrass());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_POOP))
      commandItemList.Add(FollowerCommandItems.MealPoop());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_MUSHROOMS))
      commandItemList.Add(FollowerCommandItems.MealMushrooms());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT))
      commandItemList.Add(FollowerCommandItems.MealFollowerMeat());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH))
      commandItemList.Add(FollowerCommandItems.MealGoodFish());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_GREAT))
      commandItemList.Add(FollowerCommandItems.MealGreat());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_MILK_BAD))
      commandItemList.Add(FollowerCommandItems.MealMilkBad());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_MILK_GOOD))
      commandItemList.Add(FollowerCommandItems.MealMilkGood());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_MILK_GREAT))
      commandItemList.Add(FollowerCommandItems.MealMilkGreat());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_MEAT))
      commandItemList.Add(FollowerCommandItems.MealMeat());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH))
      commandItemList.Add(FollowerCommandItems.MealGreatFish());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_EGG))
      commandItemList.Add(FollowerCommandItems.MealEgg());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_BAD_FISH))
      commandItemList.Add(FollowerCommandItems.MealBadFish());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_BERRIES))
      commandItemList.Add(FollowerCommandItems.MealBerries());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_MEDIUM_VEG))
      commandItemList.Add(FollowerCommandItems.MealMediumVeg());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_BAD_MEAT))
      commandItemList.Add(FollowerCommandItems.MealMeatLow());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT))
      commandItemList.Add(FollowerCommandItems.MealMeatHigh());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_DEADLY))
      commandItemList.Add(FollowerCommandItems.MealDeadly());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_BAD_MIXED))
      commandItemList.Add(FollowerCommandItems.MealMixedLow());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED))
      commandItemList.Add(FollowerCommandItems.MealMixedMedium());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED))
      commandItemList.Add(FollowerCommandItems.MealMixedHigh());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_SPICY))
      commandItemList.Add(FollowerCommandItems.MealSpicy());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_SNOW_FRUIT))
      commandItemList.Add(FollowerCommandItems.MealSnowFruit());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_BURNED))
      commandItemList.Add(FollowerCommandItems.MealBurnt());
    return commandItemList;
  }

  public static List<CommandItem> DrinkCommands(List<InventoryItem.ITEM_TYPE> availableDrinks)
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    if (availableDrinks.Contains(InventoryItem.ITEM_TYPE.DRINK_BEER))
      commandItemList.Add(FollowerCommandItems.DrinkBeer());
    if (availableDrinks.Contains(InventoryItem.ITEM_TYPE.DRINK_COCKTAIL))
      commandItemList.Add(FollowerCommandItems.DrinkCocktail());
    if (availableDrinks.Contains(InventoryItem.ITEM_TYPE.DRINK_EGGNOG))
      commandItemList.Add(FollowerCommandItems.DrinkEggnog());
    if (availableDrinks.Contains(InventoryItem.ITEM_TYPE.DRINK_GIN))
      commandItemList.Add(FollowerCommandItems.DrinkGin());
    if (availableDrinks.Contains(InventoryItem.ITEM_TYPE.DRINK_MUSHROOM_JUICE))
      commandItemList.Add(FollowerCommandItems.DrinkMushroomJuice());
    if (availableDrinks.Contains(InventoryItem.ITEM_TYPE.DRINK_POOP_JUICE))
      commandItemList.Add(FollowerCommandItems.DrinkPoopJuice());
    if (availableDrinks.Contains(InventoryItem.ITEM_TYPE.DRINK_WINE))
      commandItemList.Add(FollowerCommandItems.DrinkWine());
    if (availableDrinks.Contains(InventoryItem.ITEM_TYPE.DRINK_CHILLI))
      commandItemList.Add(FollowerCommandItems.DrinkChilli());
    if (availableDrinks.Contains(InventoryItem.ITEM_TYPE.DRINK_LIGHTNING))
      commandItemList.Add(FollowerCommandItems.DrinkLightning());
    if (availableDrinks.Contains(InventoryItem.ITEM_TYPE.DRINK_SIN))
      commandItemList.Add(FollowerCommandItems.DrinkSin());
    if (availableDrinks.Contains(InventoryItem.ITEM_TYPE.DRINK_GRASS))
      commandItemList.Add(FollowerCommandItems.DrinkGrass());
    if (availableDrinks.Contains(InventoryItem.ITEM_TYPE.DRINK_MILKSHAKE))
      commandItemList.Add(FollowerCommandItems.DrinkMilkshake());
    return commandItemList;
  }

  public static List<CommandItem> AreYouSureCommands()
  {
    return new List<CommandItem>()
    {
      FollowerCommandItems.AreYouSureYes(),
      FollowerCommandItems.AreYouSureNo()
    };
  }

  public static List<CommandItem> MakeDemandCommands(Follower follower)
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    if (follower.Brain.Info.ID == 99990 && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.FLOWER_RED) > 0 && !DataManager.Instance.SecretItemsGivenToFollower.Contains(FollowerLocation.Dungeon1_1))
      commandItemList.Add((CommandItem) FollowerCommandItems.GiveLeaderSecretItem(FollowerLocation.Dungeon1_1));
    else if (follower.Brain.Info.ID == 99991 && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL) > 0 && !DataManager.Instance.SecretItemsGivenToFollower.Contains(FollowerLocation.Dungeon1_2))
      commandItemList.Add((CommandItem) FollowerCommandItems.GiveLeaderSecretItem(FollowerLocation.Dungeon1_2));
    else if (follower.Brain.Info.ID == 99992 && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.CRYSTAL) > 0 && !DataManager.Instance.SecretItemsGivenToFollower.Contains(FollowerLocation.Dungeon1_3))
      commandItemList.Add((CommandItem) FollowerCommandItems.GiveLeaderSecretItem(FollowerLocation.Dungeon1_3));
    else if (follower.Brain.Info.ID == 99993 && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SPIDER_WEB) > 0 && !DataManager.Instance.SecretItemsGivenToFollower.Contains(FollowerLocation.Dungeon1_4))
      commandItemList.Add((CommandItem) FollowerCommandItems.GiveLeaderSecretItem(FollowerLocation.Dungeon1_4));
    else if (follower.Brain.Info.ID == 100007 && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.WOOL) > 0 && !DataManager.Instance.SecretItemsGivenToFollower.Contains(FollowerLocation.Dungeon1_5))
      commandItemList.Add((CommandItem) FollowerCommandItems.GiveLeaderSecretItem(FollowerLocation.Dungeon1_5));
    if (follower.Brain.CurrentTaskType != FollowerTaskType.Sleep && follower.Brain.CurrentTaskType != FollowerTaskType.SleepBedRest)
    {
      if (follower.Brain.Info.CursedState == Thought.Ill)
        commandItemList.Add(FollowerCommandItems.BedRest());
      else
        commandItemList.Add(FollowerCommandItems.Sleep());
    }
    if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.LawOrder_MurderFollower))
      commandItemList.Add(FollowerCommandItems.Murder());
    if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.Possessions_ExtortTithes))
      commandItemList.Add(FollowerCommandItems.Extort());
    if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.Possessions_Bribe))
      commandItemList.Add(FollowerCommandItems.Bribe());
    if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.WorkWorship_Inspire))
      commandItemList.Add(FollowerCommandItems.Dance());
    if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.WorkWorship_Intimidate))
      commandItemList.Add(FollowerCommandItems.Intimidate());
    if (!DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.WorkWorship_Inspire) && !DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.WorkWorship_Intimidate) && DataManager.Instance.ShowLoyaltyBars || CheatConsole.ForceBlessEnabled)
      commandItemList.Add(FollowerCommandItems.Bless());
    commandItemList.Add(FollowerCommandItems.EatSomething(follower));
    if (StructureManager.GetAllStructuresOfType<Structures_Pub>().Count > 0 && !Structures_Pub.IsDrinking)
      commandItemList.Add(FollowerCommandItems.DrinkSomething(follower));
    if (Inventory.HasGift())
      commandItemList.Add(FollowerCommandItems.Gift(follower));
    if (Inventory.HasAnimal() && DataManager.Instance.OnboardedWool)
      commandItemList.Add(FollowerCommandItems.GiveAnimalPet(follower));
    if (Prison.Prisons.Count > 0)
    {
      if (Prison.HasAvailablePrisons())
        commandItemList.Add(FollowerCommandItems.Imprison());
      else
        commandItemList.Add(FollowerCommandItems.NoAvailablePrisons());
    }
    if (follower.Brain.Info.MarriedToLeader || CheatConsole.ForceSmoochEnabled)
      commandItemList.Add(FollowerCommandItems.Kiss());
    if (follower.Brain.Info.SkinName.Contains("Dog") || follower.Brain.Info.SkinName.Contains("Poppy"))
      commandItemList.Add(FollowerCommandItems.PetDog());
    if (follower.Brain.HasTrait(FollowerTrait.TraitType.Pettable) && !follower.Brain.Info.SkinName.Contains("Dog") && !follower.Brain.Info.SkinName.Contains("Poppy"))
      commandItemList.Add(FollowerCommandItems.PetFollower());
    if (follower.Brain.Info.Necklace != InventoryItem.ITEM_TYPE.NONE)
    {
      commandItemList.Add((CommandItem) FollowerCommandItems.RemoveNecklace(follower.Brain.Info.Necklace));
      if (follower.Brain.Info.ShowingNecklace)
        commandItemList.Add((CommandItem) FollowerCommandItems.HideNecklace(follower.Brain.Info.Necklace));
      else
        commandItemList.Add((CommandItem) FollowerCommandItems.ShowNecklace(follower.Brain.Info.Necklace));
    }
    return commandItemList;
  }

  public static List<CommandItem> WakeUpCommands()
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    commandItemList.Add(FollowerCommandItems.WakeUp());
    if (DataManager.Instance.CanReadMinds)
      commandItemList.Add(FollowerCommandItems.Surveillance());
    return commandItemList;
  }

  public static List<CommandItem> OldAgeCommands(Follower follower)
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    commandItemList.Add(FollowerCommandItems.Sleep());
    if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.LawOrder_MurderFollower))
      commandItemList.Add(FollowerCommandItems.Murder());
    if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.WorkWorship_Inspire))
      commandItemList.Add(FollowerCommandItems.Dance());
    else if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.WorkWorship_Intimidate))
      commandItemList.Add(FollowerCommandItems.Intimidate());
    else if (DataManager.Instance.ShowLoyaltyBars || CheatConsole.ForceBlessEnabled)
      commandItemList.Add(FollowerCommandItems.Bless());
    if (DataManager.Instance.CanReadMinds)
      commandItemList.Add(FollowerCommandItems.Surveillance());
    if (follower.Brain.Info.HasTrait(FollowerTrait.TraitType.Scared) || follower.Brain.Info.HasTrait(FollowerTrait.TraitType.Terrified) || follower.Brain.HasTrait(FollowerTrait.TraitType.CriminalScarred) || follower.Brain.HasTrait(FollowerTrait.TraitType.Bastard) || follower.Brain.Info.HasTrait(FollowerTrait.TraitType.MissionaryTerrified))
    {
      commandItemList.Add(FollowerCommandItems.Bully());
      commandItemList.Add(FollowerCommandItems.Reassure());
    }
    commandItemList.Add(FollowerCommandItems.EatSomething(follower));
    if (StructureManager.GetAllStructuresOfType<Structures_Pub>().Count > 0 && !Structures_Pub.IsDrinking)
      commandItemList.Add(FollowerCommandItems.DrinkSomething(follower));
    if (follower.Brain.Info.MarriedToLeader || CheatConsole.ForceSmoochEnabled)
      commandItemList.Add(FollowerCommandItems.Kiss());
    if (follower.Brain.Info.SkinName.Contains("Dog") || follower.Brain.Info.SkinName.Contains("Poppy"))
      commandItemList.Add(FollowerCommandItems.PetDog());
    if (follower.Brain.HasTrait(FollowerTrait.TraitType.Pettable) && !follower.Brain.Info.SkinName.Contains("Dog") && !follower.Brain.Info.SkinName.Contains("Poppy"))
      commandItemList.Add(FollowerCommandItems.PetFollower());
    if (follower.Brain.Info.Necklace != InventoryItem.ITEM_TYPE.NONE)
    {
      if (follower.Brain.Info.ShowingNecklace)
        commandItemList.Add((CommandItem) FollowerCommandItems.HideNecklace(follower.Brain.Info.Necklace));
      else
        commandItemList.Add((CommandItem) FollowerCommandItems.ShowNecklace(follower.Brain.Info.Necklace));
    }
    return commandItemList;
  }

  public static List<CommandItem> DrunkCommands(Follower follower)
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    commandItemList.Add(FollowerCommandItems.Sleep());
    if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.LawOrder_MurderFollower))
      commandItemList.Add(FollowerCommandItems.Murder());
    if (follower.Brain.HasTrait(FollowerTrait.TraitType.Scared) || follower.Brain.HasTrait(FollowerTrait.TraitType.Terrified) || follower.Brain.HasTrait(FollowerTrait.TraitType.CriminalScarred) || follower.Brain.HasTrait(FollowerTrait.TraitType.Bastard) || follower.Brain.HasTrait(FollowerTrait.TraitType.MissionaryTerrified))
    {
      commandItemList.Add(FollowerCommandItems.Bully());
      commandItemList.Add(FollowerCommandItems.Reassure());
    }
    else if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.WorkWorship_Inspire))
      commandItemList.Add(FollowerCommandItems.Dance());
    else if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.WorkWorship_Intimidate))
      commandItemList.Add(FollowerCommandItems.Intimidate());
    else if (DataManager.Instance.ShowLoyaltyBars || CheatConsole.ForceBlessEnabled)
      commandItemList.Add(FollowerCommandItems.Bless());
    if (DataManager.Instance.CanReadMinds)
      commandItemList.Add(FollowerCommandItems.Surveillance());
    commandItemList.Add(FollowerCommandItems.EatSomething(follower));
    if (StructureManager.GetAllStructuresOfType<Structures_Pub>().Count > 0 && !Structures_Pub.IsDrinking)
      commandItemList.Add(FollowerCommandItems.DrinkSomething(follower));
    if (follower.Brain.Info.MarriedToLeader || CheatConsole.ForceSmoochEnabled)
      commandItemList.Add(FollowerCommandItems.Kiss());
    if (follower.Brain.Info.SkinName.Contains("Dog") || follower.Brain.Info.SkinName.Contains("Poppy"))
      commandItemList.Add(FollowerCommandItems.PetDog());
    return commandItemList;
  }

  public static List<CommandItem> SnowmanCommands(Follower follower)
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    commandItemList.Add(FollowerCommandItems.GiveWorkerCommand(follower));
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.Necklace_Winter) > 0 && follower.Brain.Info.Necklace == InventoryItem.ITEM_TYPE.NONE)
      commandItemList.Add(FollowerCommandItems.Gift_Necklace_Winter());
    return commandItemList;
  }

  public static List<CommandItem> SnowmanChildCommands(Follower follower)
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    commandItemList.Add(FollowerCommandItems.CuddleBaby());
    if (Interaction_Daycare.HasAvailableDaycare())
      commandItemList.Add(FollowerCommandItems.SendToDaycare());
    return commandItemList;
  }

  public static List<CommandItem> PoetCommands(Follower follower)
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    if (DataManager.Instance.InTutorial)
      commandItemList.Add(FollowerCommandItems.MakeDemand(follower));
    if (follower.Brain.Info.TaxEnforcer)
      commandItemList.Add(FollowerCommandItems.CollectTax());
    if (DataManager.Instance.CanReadMinds)
      commandItemList.Add(FollowerCommandItems.Surveillance());
    return commandItemList;
  }

  public static List<CommandItem> ScaredCommands(Follower follower)
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    commandItemList.Add(FollowerCommandItems.Sleep());
    if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.LawOrder_MurderFollower))
      commandItemList.Add(FollowerCommandItems.Murder());
    commandItemList.Add(FollowerCommandItems.Bully());
    commandItemList.Add(FollowerCommandItems.Reassure());
    if (DataManager.Instance.CanReadMinds)
      commandItemList.Add(FollowerCommandItems.Surveillance());
    return commandItemList;
  }

  public static List<CommandItem> ExistentialDreadCommands(Follower follower)
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.LawOrder_MurderFollower))
      commandItemList.Add(FollowerCommandItems.Murder());
    if (DataManager.Instance.CanReadMinds)
      commandItemList.Add(FollowerCommandItems.Surveillance());
    return commandItemList;
  }

  public static List<CommandItem> SpyCommands(Follower follower)
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    commandItemList.Add(FollowerCommandItems.Sleep());
    if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.LawOrder_MurderFollower))
      commandItemList.Add(FollowerCommandItems.Murder());
    if (DataManager.Instance.CanReadMinds)
      commandItemList.Add(FollowerCommandItems.Surveillance());
    if (Prison.Prisons.Count > 0)
    {
      if (Prison.HasAvailablePrisons())
        commandItemList.Add(FollowerCommandItems.Imprison());
      else
        commandItemList.Add(FollowerCommandItems.NoAvailablePrisons());
    }
    commandItemList.Add(FollowerCommandItems.EatSomething(follower));
    return commandItemList;
  }

  public static List<CommandItem> BabyCommands(Follower follower)
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    commandItemList.Add(FollowerCommandItems.CuddleBaby());
    if (DataManager.Instance.CanReadMinds)
      commandItemList.Add(FollowerCommandItems.Surveillance());
    if (Interaction_Daycare.HasAvailableDaycare())
      commandItemList.Add(FollowerCommandItems.SendToDaycare());
    return commandItemList;
  }

  public static List<CommandItem> ZombieCommands(Follower follower)
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    if (DataManager.Instance.CanReadMinds)
      commandItemList.Add(FollowerCommandItems.Surveillance());
    if (follower.Brain.Info.MarriedToLeader)
      commandItemList.Add(FollowerCommandItems.Kiss());
    return commandItemList;
  }

  public static List<CommandItem> MutatedCommands(Follower follower)
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    if (DataManager.Instance.CanReadMinds)
      commandItemList.Add(FollowerCommandItems.Surveillance());
    if (follower.Brain.Info.MarriedToLeader)
      commandItemList.Add(FollowerCommandItems.Kiss());
    if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.LawOrder_MurderFollower))
      commandItemList.Add(FollowerCommandItems.Murder());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.Necklace_Weird) > 0 && follower.Brain.Info.Necklace == InventoryItem.ITEM_TYPE.NONE)
      commandItemList.Add(FollowerCommandItems.Gift_Necklace_Weird());
    commandItemList.Add(FollowerCommandItems.GiveWorkerCommand(follower));
    return commandItemList;
  }

  public static List<CommandItem> DissenterCommands(Follower follower)
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    commandItemList.Add(FollowerCommandItems.Reeducate());
    if (Prison.Prisons.Count > 0)
    {
      if (Prison.HasAvailablePrisons())
        commandItemList.Add(FollowerCommandItems.Imprison());
      else
        commandItemList.Add(FollowerCommandItems.NoAvailablePrisons());
    }
    if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.LawOrder_MurderFollower))
      commandItemList.Add(FollowerCommandItems.Murder());
    if (DataManager.Instance.CanReadMinds)
      commandItemList.Add(FollowerCommandItems.Surveillance());
    commandItemList.Add(FollowerCommandItems.EatSomething(follower));
    return commandItemList;
  }

  public static List<CommandItem> GiftCommands(Follower follower)
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.GIFT_SMALL) > 0)
      commandItemList.Add(FollowerCommandItems.Gift_Small());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.GIFT_MEDIUM) > 0)
      commandItemList.Add(FollowerCommandItems.Gift_Medium());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.Necklace_1) > 0)
      commandItemList.Add(FollowerCommandItems.Gift_Necklace1());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.Necklace_2) > 0)
      commandItemList.Add(FollowerCommandItems.Gift_Necklace2());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.Necklace_3) > 0)
      commandItemList.Add(FollowerCommandItems.Gift_Necklace3());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.Necklace_4) > 0)
      commandItemList.Add(FollowerCommandItems.Gift_Necklace4());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.Necklace_5) > 0)
      commandItemList.Add(FollowerCommandItems.Gift_Necklace5());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.Necklace_Dark) > 0)
      commandItemList.Add(FollowerCommandItems.Gift_Necklace_Dark());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.Necklace_Gold_Skull) > 0)
      commandItemList.Add(FollowerCommandItems.Gift_Necklace_Gold_Skull());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.Necklace_Demonic) > 0)
      commandItemList.Add(FollowerCommandItems.Gift_Necklace_Demonic());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.Necklace_Light) > 0)
      commandItemList.Add(FollowerCommandItems.Gift_Necklace_Light());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.Necklace_Loyalty) > 0)
      commandItemList.Add(FollowerCommandItems.Gift_Necklace_Loyalty());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.Necklace_Missionary) > 0)
      commandItemList.Add(FollowerCommandItems.Gift_Necklace_Missionary());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.Necklace_Deaths_Door) > 0)
      commandItemList.Add(FollowerCommandItems.Gift_Necklace_Deaths_Door());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.Necklace_Winter) > 0)
      commandItemList.Add(FollowerCommandItems.Gift_Necklace_Winter());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.Necklace_Frozen) > 0)
      commandItemList.Add(FollowerCommandItems.Gift_Necklace_Frozen());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.Necklace_Weird) > 0)
      commandItemList.Add(FollowerCommandItems.Gift_Necklace_Weird());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.Necklace_Targeted) > 0)
      commandItemList.Add(FollowerCommandItems.Gift_Necklace_Targeted());
    return commandItemList;
  }

  public static List<CommandItem> GiveAnimalPetCommands(Follower follower)
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.ANIMAL_GOAT) > 0)
      commandItemList.Add(FollowerCommandItems.Give_Goat_Pet());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.ANIMAL_COW) > 0)
      commandItemList.Add(FollowerCommandItems.Give_Cow_Pet());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.ANIMAL_SPIDER) > 0)
      commandItemList.Add(FollowerCommandItems.Give_Spider_Pet());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.ANIMAL_LLAMA) > 0)
      commandItemList.Add(FollowerCommandItems.Give_Llama_Pet());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.ANIMAL_SNAIL) > 0)
      commandItemList.Add(FollowerCommandItems.Give_Snail_Pet());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.ANIMAL_TURTLE) > 0)
      commandItemList.Add(FollowerCommandItems.Give_Turtle_Pet());
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.ANIMAL_CRAB) > 0)
      commandItemList.Add(FollowerCommandItems.Give_Crab_Pet());
    return commandItemList;
  }
}
