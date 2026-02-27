// Decompiled with JetBrains decompiler
// Type: FollowerCommandGroups
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI.FollowerInteractionWheel;
using System.Collections.Generic;

#nullable disable
public sealed class FollowerCommandGroups
{
  public static List<CommandItem> DefaultCommands(Follower follower)
  {
    List<CommandItem> commandItemList = new List<CommandItem>();
    if ((follower.Brain.CurrentTaskType == FollowerTaskType.Sleep || follower.Brain.CurrentTaskType == FollowerTaskType.SleepBedRest) && follower.Brain.CurrentTask.State == FollowerTaskState.Doing)
      return FollowerCommandGroups.WakeUpCommands();
    if (follower.Brain.Info.CursedState == Thought.Dissenter)
      return FollowerCommandGroups.DissenterCommands(follower);
    if (follower.Brain.Info.CursedState == Thought.Ill)
      return FollowerCommandGroups.MakeDemandCommands(follower);
    if (follower.Brain.Info.CursedState == Thought.OldAge)
      return FollowerCommandGroups.OldAgeCommands(follower);
    commandItemList.Add(FollowerCommandItems.GiveWorkerCommand(follower));
    if (DataManager.Instance.InTutorial)
      commandItemList.Add(FollowerCommandItems.MakeDemand(follower));
    if (follower.Brain.Info.TaxEnforcer)
      commandItemList.Add(FollowerCommandItems.CollectTax());
    if (DataManager.Instance.CanReadMinds)
      commandItemList.Add(FollowerCommandItems.Surveillance());
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
        commandItemList.Add(FollowerCommandItems.Worship());
        commandItemList.Add(FollowerCommandItems.CutTrees());
        commandItemList.Add(FollowerCommandItems.ClearRubble());
        commandItemList.Add(FollowerCommandItems.Builder());
        if (FarmStation.FarmStations.Count > 0)
          commandItemList.Add(FollowerCommandItems.Farmer());
        if (JanitorStation.JanitorStations.Count > 0)
          commandItemList.Add(FollowerCommandItems.Janitor());
        if (Interaction_Refinery.Refineries.Count > 0)
        {
          commandItemList.Add(FollowerCommandItems.Refiner());
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
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_MEAT))
      commandItemList.Add(FollowerCommandItems.MealMeat());
    if (availableMeals.Contains(InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH))
      commandItemList.Add(FollowerCommandItems.MealGreatFish());
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
    if (follower.Brain.Info.CursedState == Thought.Ill)
      commandItemList.Add(FollowerCommandItems.BedRest());
    else
      commandItemList.Add(FollowerCommandItems.Sleep());
    if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.LawOrder_MurderFollower))
      commandItemList.Add(FollowerCommandItems.Murder());
    if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.Possessions_ExtortTithes))
      commandItemList.Add(FollowerCommandItems.Extort());
    else if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.Possessions_Bribe))
      commandItemList.Add(FollowerCommandItems.Bribe());
    if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.WorkWorship_Inspire))
      commandItemList.Add(FollowerCommandItems.Dance());
    else if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.WorkWorship_Intimidate))
      commandItemList.Add(FollowerCommandItems.Intimidate());
    else if (DataManager.Instance.ShowLoyaltyBars || CheatConsole.ForceBlessEnabled)
      commandItemList.Add(FollowerCommandItems.Bless());
    commandItemList.Add(FollowerCommandItems.EatSomething(follower));
    if (Inventory.HasGift())
      commandItemList.Add(FollowerCommandItems.Gift());
    if (Prison.Prisons.Count > 0)
    {
      if (Prison.HasAvailablePrisons())
        commandItemList.Add(FollowerCommandItems.Imprison());
      else
        commandItemList.Add(FollowerCommandItems.NoAvailablePrisons());
    }
    if (follower.Brain.Info.MarriedToLeader || CheatConsole.ForceSmoochEnabled)
      commandItemList.Add(FollowerCommandItems.Kiss());
    if (WorshipperData.Instance.Characters[follower.Brain.Info.SkinCharacter].Title.Contains("Dog"))
      commandItemList.Add(FollowerCommandItems.PetDog());
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
    commandItemList.Add(FollowerCommandItems.EatSomething(follower));
    if (follower.Brain.Info.MarriedToLeader || CheatConsole.ForceSmoochEnabled)
      commandItemList.Add(FollowerCommandItems.Kiss());
    if (WorshipperData.Instance.Characters[follower.Brain.Info.SkinCharacter].Title.Contains("Dog"))
      commandItemList.Add(FollowerCommandItems.PetDog());
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

  public static List<CommandItem> GiftCommands()
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
    return commandItemList;
  }
}
