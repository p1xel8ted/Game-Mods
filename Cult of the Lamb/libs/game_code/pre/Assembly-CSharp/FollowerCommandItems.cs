// Decompiled with JetBrains decompiler
// Type: FollowerCommandItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.FollowerInteractionWheel;
using System.Collections.Generic;

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

  public static CommandItem Janitor()
  {
    FollowerCommandItems.FollowerRoleCommandItem followerRoleCommandItem = new FollowerCommandItems.FollowerRoleCommandItem();
    followerRoleCommandItem.Command = FollowerCommands.Janitor_2;
    followerRoleCommandItem.FollowerTaskType = FollowerTaskType.Janitor;
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
    return new CommandItem()
    {
      Command = FollowerCommands.Sleep
    };
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
      foreach (StructureBrain structureBrain in StructureManager.GetAllStructuresOfType<Structures_Meal>(PlayerFarming.Location))
      {
        InventoryItem.ITEM_TYPE mealType = StructuresData.GetMealType(structureBrain.Data.Type);
        if (!availableMeals.Contains(mealType))
          availableMeals.Add(mealType);
      }
      foreach (StructureBrain structureBrain in StructureManager.GetAllStructuresOfType<Structures_FoodStorage>(PlayerFarming.Location))
      {
        foreach (InventoryItem inventoryItem in structureBrain.Data.Inventory)
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

  public static CommandItem Gift()
  {
    return new CommandItem()
    {
      Command = FollowerCommands.Gift,
      SubCommands = FollowerCommandGroups.GiftCommands()
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

  public static CommandItem RemoveNecklace()
  {
    return new CommandItem()
    {
      Command = FollowerCommands.RemoveNecklace
    };
  }

  public static CommandItem Imprison()
  {
    return new CommandItem()
    {
      Command = FollowerCommands.Imprison
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

  public static CommandItem ForageBerries()
  {
    FollowerCommandItems.FollowerRoleCommandItem followerRoleCommandItem = new FollowerCommandItems.FollowerRoleCommandItem();
    followerRoleCommandItem.Command = FollowerCommands.ForageBerries;
    followerRoleCommandItem.FollowerTaskType = FollowerTaskType.Forage;
    return (CommandItem) followerRoleCommandItem;
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
    if (follower.Brain.Info.CursedState == Thought.None)
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

  private class FollowerRoleCommandItem : CommandItem
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

  private class CollectTaxCommandItem : CommandItem
  {
    public override string GetTitle(Follower follower)
    {
      return follower.Brain._directInfoAccess.TaxCollected <= 0 ? LocalizationManager.GetTranslation("FollowerInteractions/NoTax") : $"{base.GetTitle(follower)} <sprite name=\"icon_blackgold\"> x{(object) follower.Brain._directInfoAccess.TaxCollected}";
    }
  }

  private class KissCommandItem : CommandItem
  {
    public override bool IsAvailable(Follower follower) => !follower.Brain.Stats.KissedAction;
  }

  private class PetDogCommandItem : CommandItem
  {
    public override bool IsAvailable(Follower follower) => !follower.Brain.Stats.PetDog;
  }

  private class BlessCommandItem : CommandItem
  {
    public override string GetDescription(Follower follower)
    {
      return $"{base.GetDescription(follower)}<br><sprite name=\"icon_Faith\">{(" +" + (object) FollowerThoughts.GetData(Thought.Cult_Bless).Modifier).Colour(StaticColors.GreenColor)}";
    }

    public override bool IsAvailable(Follower follower) => !follower.Brain.Stats.ReceivedBlessing;
  }

  private class MurderCommandItem : CommandItem
  {
    public override string GetDescription(Follower follower)
    {
      return $"{base.GetDescription(follower)}<br><sprite name=\"icon_Faith\">{FollowerThoughts.GetData(Thought.Cult_Murder).Modifier.ToString().Colour(StaticColors.RedColor)}";
    }
  }

  private class ExtortCommandItem : CommandItem
  {
    public override bool IsAvailable(Follower follower) => !follower.Brain.Stats.PaidTithes;
  }

  private class BribeCommandItem : CommandItem
  {
    public override bool IsAvailable(Follower follower) => !follower.Brain.Stats.Bribed;
  }

  private class DanceCommandItem : CommandItem
  {
    public override string GetDescription(Follower follower)
    {
      return $"{base.GetDescription(follower)}<br><sprite name=\"icon_Faith\">{(" +" + (object) FollowerThoughts.GetData(Thought.Cult_Inspire).Modifier).Colour(StaticColors.GreenColor)}";
    }

    public override bool IsAvailable(Follower follower) => !follower.Brain.Stats.Inspired;
  }

  private class IntimidateCommandItem : CommandItem
  {
    public override bool IsAvailable(Follower follower) => !follower.Brain.Stats.Intimidated;
  }

  public class GiftCommandItem : CommandItem
  {
    private InventoryItem.ITEM_TYPE _itemType;

    public GiftCommandItem(InventoryItem.ITEM_TYPE itemType) => this._itemType = itemType;

    public override string GetTitle(Follower follower)
    {
      return $"{LocalizationManager.GetTranslation($"Inventory/{this._itemType}")} ({Inventory.GetItemQuantity(this._itemType)})";
    }
  }

  private class ReeducateCommandItem : CommandItem
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

  public class ConfirmationItem : CommandItem
  {
    public override string GetDescription(Follower follower) => "";
  }

  private class GiverWorkerCommandItem : CommandItem
  {
    public override bool IsAvailable(Follower follower) => !FollowerBrainStats.IsHoliday;
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
}
