// Decompiled with JetBrains decompiler
// Type: AnimalData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.RanchSelect;
using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AnimalData : BaseMonoBehaviour
{
  public SkeletonAnimation SkeletonData;
  public List<AnimalColors> animalColors;
  public static AnimalData _Instance;

  public static AnimalData Instance
  {
    get
    {
      if (!Application.isPlaying)
        return AnimalData._Instance = Object.FindObjectOfType<AnimalData>();
      if ((Object) AnimalData._Instance == (Object) null)
        AnimalData._Instance = (Object.Instantiate(Resources.Load("Prefabs/Animal Data")) as GameObject).GetComponent<AnimalData>();
      return AnimalData._Instance;
    }
    set => AnimalData._Instance = value;
  }

  public WorshipperData.SlotsAndColours[] GetAnimalColors(InventoryItem.ITEM_TYPE type)
  {
    foreach (AnimalColors animalColor in this.animalColors)
    {
      if (animalColor.Type == type)
        return animalColor.colours;
    }
    return (WorshipperData.SlotsAndColours[]) null;
  }

  public bool RemoveAnimalByID(int animalInfoID)
  {
    List<RanchSelectEntry> ranchSelectEntryList = new List<RanchSelectEntry>();
    foreach (Interaction_Ranch ranch in Interaction_Ranch.Ranches)
    {
      foreach (StructuresData.Ranchable_Animal animal in ranch.Brain.Data.Animals)
      {
        if (animalInfoID == animal.ID)
        {
          DataManager.Instance.DeadAnimalsTemporaryList.Add(animal);
          ranch.Brain.RemoveAnimal(animal);
          return true;
        }
      }
    }
    return false;
  }

  public static List<StructuresData.Ranchable_Animal> GetAnimals()
  {
    List<StructuresData.Ranchable_Animal> animals = new List<StructuresData.Ranchable_Animal>();
    foreach (Interaction_Ranch ranch in Interaction_Ranch.Ranches)
    {
      int num = ranch.IsOvercrowded ? 1 : 0;
      foreach (StructuresData.Ranchable_Animal animal in ranch.Brain.Data.Animals)
        animals.Add(animal);
    }
    return animals;
  }

  public static void RemoveAnimal(
    StructuresData.Ranchable_Animal animal,
    List<StructuresData.Ranchable_Animal> animalinfo)
  {
    if (!animalinfo.Contains(animal))
      return;
    Debug.Log((object) "Animal Removed");
    animalinfo.Remove(animal);
  }

  public static void AddAnimal(
    StructuresData.Ranchable_Animal animal,
    List<StructuresData.Ranchable_Animal> animalinfo)
  {
    if (animalinfo.Contains(animal))
      return;
    Debug.Log((object) "Animal added");
    animalinfo.Add(animal);
  }

  public static void RemoveAnimalAt(int index, List<StructuresData.Ranchable_Animal> animalinfo)
  {
    Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.ANIMALS, Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.ANIMALS) - 1);
    animalinfo.RemoveAt(index);
  }

  public static void GetRanchSpineSkin(
    StructuresData.Ranchable_Animal info,
    SkeletonAnimation skeletonGraphic)
  {
    int growthState = info.GrowthState;
    skeletonGraphic.Skeleton.SetSkin((Skin) null);
    Skin newSkin = new Skin("Skin");
    string lower = info.Type.ToString().Replace("ANIMAL_", "").ToLower();
    string str1 = char.ToUpper(lower[0]).ToString() + lower.Substring(1);
    string str2 = str1;
    string skinName = !info.WorkedToday || !Interaction_Ranchable.shearables.Contains(info.Type) ? (!info.WorkedReady ? $"{str2}/{str1}" : $"{str2}/{str1}_Ready") : $"{str2}/{str1}_Sheared";
    newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin(skinName));
    if (info.Age < 2)
      newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin("State/Baby"));
    else if ((double) info.Satiation <= 25.0)
      newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin("State/Starving"));
    else if (growthState >= 6)
      newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin("State/Fat"));
    else
      newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin("State/Normal"));
    if (info.Ailment == Interaction_Ranchable.Ailment.Feral)
      newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin("State/Feral"));
    else if (info.Ailment == Interaction_Ranchable.Ailment.Stinky)
      newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin("State/Stinky"));
    switch (info.Type)
    {
      case InventoryItem.ITEM_TYPE.ANIMAL_GOAT:
      case InventoryItem.ITEM_TYPE.ANIMAL_COW:
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{str1}/Horns/{info.Horns}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{str1}/Ears/{info.Ears}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{str1}/Head/{info.Head}"));
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_TURTLE:
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{str1}/Body/{info.Horns}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{str1}/Face/{info.Ears}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{str1}/Head/{info.Head}"));
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_CRAB:
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{str1}/Claws/{info.Horns}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{str1}/Shell/{info.Ears}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{str1}/Head/{info.Head}"));
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_SPIDER:
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{str1}/Body/{info.Horns}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{str1}/Eyes/{info.Ears}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{str1}/Head/{info.Head}"));
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_SNAIL:
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{str1}/Body/{info.Horns}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{str1}/Face/{info.Ears}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{str1}/Shell/{info.Head}"));
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_LLAMA:
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{str1}/Tail/{info.Horns}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{str1}/Ears/{info.Ears}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{str1}/Head/{info.Head}"));
        break;
    }
    skeletonGraphic.Skeleton.SetSkin(newSkin);
    skeletonGraphic.Skeleton.SetSlotsToSetupPose();
    WorshipperData.SlotsAndColours[] animalColors = AnimalData.Instance.GetAnimalColors(info.Type);
    foreach (WorshipperData.SlotAndColor slotAndColour in animalColors[Mathf.Clamp(info.Colour, 0, animalColors.Length - 1)].SlotAndColours)
    {
      Slot slot = skeletonGraphic.Skeleton.FindSlot(slotAndColour.Slot);
      if (slot != null)
        slot.SetColor(slotAndColour.color);
    }
  }

  public static bool HasDiscoveredAnimal(InventoryItem.ITEM_TYPE animal)
  {
    return DataManager.Instance.DisoveredAnimals.Contains(animal);
  }

  public static bool TryDiscoverAnimal(InventoryItem.ITEM_TYPE animal)
  {
    if (DataManager.Instance.DisoveredAnimals.Contains(animal))
      return false;
    DataManager.Instance.DisoveredAnimals.Add(animal);
    return true;
  }
}
