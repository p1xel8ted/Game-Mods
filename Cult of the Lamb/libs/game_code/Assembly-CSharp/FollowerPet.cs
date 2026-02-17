// Decompiled with JetBrains decompiler
// Type: FollowerPet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class FollowerPet : MonoBehaviour
{
  public static List<FollowerPet> FollowerPets = new List<FollowerPet>();
  public static List<FollowerPet.FollowerPetType> dlcPets = new List<FollowerPet.FollowerPetType>()
  {
    FollowerPet.FollowerPetType.Goat,
    FollowerPet.FollowerPetType.Cow,
    FollowerPet.FollowerPetType.Crab,
    FollowerPet.FollowerPetType.Llama,
    FollowerPet.FollowerPetType.Snail,
    FollowerPet.FollowerPetType.BigSpider,
    FollowerPet.FollowerPetType.Turtle
  };
  public static Dictionary<FollowerPet.FollowerPetType, int> pets = new Dictionary<FollowerPet.FollowerPetType, int>();
  [SerializeField]
  public SkeletonAnimation spine;
  public FollowerPet.FollowerPetType petType;
  public Follower follower;
  public FollowerPet.DLCPet dlcPetData;
  public CritterSpider critter;

  public Follower Follower => this.follower;

  public FollowerPet.DLCPet DLCPetData => this.dlcPetData;

  public static void Create(
    FollowerPet.DLCPet petData,
    Follower target,
    Vector3 pos,
    Action<FollowerPet> callback = null)
  {
    FollowerPet.Create(petData.PetType, target, pos, (Action<FollowerPet>) (pet =>
    {
      string dlcPetSkin = FollowerPet.GetDLCPetSkin(petData.PetType);
      pet.spine.initialSkinName = $"{dlcPetSkin}/{dlcPetSkin}";
      pet.spine.Initialize(true);
      pet.dlcPetData = petData;
      pet.UpdateSkin();
      Action<FollowerPet> action = callback;
      if (action == null)
        return;
      action(pet);
    }));
  }

  public static void Create(
    InventoryItem.ITEM_TYPE petItem,
    Follower target,
    Vector3 pos,
    Action<FollowerPet> callback = null)
  {
    FollowerPet.DLCPet newPetData;
    switch (petItem)
    {
      case InventoryItem.ITEM_TYPE.ANIMAL_GOAT:
        newPetData = FollowerPet.GetNewPetData(petItem, FollowerPet.FollowerPetType.Goat);
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_TURTLE:
        newPetData = FollowerPet.GetNewPetData(petItem, FollowerPet.FollowerPetType.Turtle);
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_CRAB:
        newPetData = FollowerPet.GetNewPetData(petItem, FollowerPet.FollowerPetType.Crab);
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_SPIDER:
        newPetData = FollowerPet.GetNewPetData(petItem, FollowerPet.FollowerPetType.BigSpider);
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_SNAIL:
        newPetData = FollowerPet.GetNewPetData(petItem, FollowerPet.FollowerPetType.Snail);
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_COW:
        newPetData = FollowerPet.GetNewPetData(petItem, FollowerPet.FollowerPetType.Cow);
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_LLAMA:
        newPetData = FollowerPet.GetNewPetData(petItem, FollowerPet.FollowerPetType.Llama);
        break;
      default:
        newPetData = FollowerPet.GetNewPetData(InventoryItem.ITEM_TYPE.ANIMAL_GOAT, FollowerPet.FollowerPetType.Goat);
        break;
    }
    FollowerPet.Create(newPetData, target, pos, callback);
  }

  public static void Create(
    FollowerPet.FollowerPetType petType,
    Follower target,
    Vector3 pos,
    Action<FollowerPet> callback = null)
  {
    string key = "";
    switch (petType)
    {
      case FollowerPet.FollowerPetType.Spider:
        key = "Assets/Prefabs/Follower Pets/Spider Pet.prefab";
        break;
      case FollowerPet.FollowerPetType.Poop:
        key = "Assets/Prefabs/Follower Pets/Poop Pet.prefab";
        break;
      case FollowerPet.FollowerPetType.Goat:
      case FollowerPet.FollowerPetType.Cow:
      case FollowerPet.FollowerPetType.Crab:
      case FollowerPet.FollowerPetType.Llama:
      case FollowerPet.FollowerPetType.Snail:
      case FollowerPet.FollowerPetType.BigSpider:
      case FollowerPet.FollowerPetType.Turtle:
        key = "Assets/Prefabs/DLC Ranchable Pet.prefab";
        break;
    }
    Addressables_wrapper.InstantiateAsync((object) key, target.transform.parent, (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      FollowerPet component = obj.Result.GetComponent<FollowerPet>();
      obj.Result.transform.position = pos;
      component.follower = target;
      component.petType = petType;
      if (FollowerPet.pets.ContainsKey(petType))
        FollowerPet.pets[petType]++;
      else
        FollowerPet.pets.Add(petType, 1);
      component.critter = obj.Result.GetComponent<CritterSpider>();
      if ((UnityEngine.Object) component.critter != (UnityEngine.Object) null)
      {
        component.critter.TargetHost = target.gameObject;
        component.critter.IsPet = true;
        component.critter.CanTeleport = false;
        component.critter.Initialise();
      }
      if (FollowerPet.dlcPets.Contains(petType))
        target.DLCFollowerPets.Add(component);
      Action<FollowerPet> action = callback;
      if (action == null)
        return;
      action(component);
    }));
  }

  public void Awake() => FollowerPet.FollowerPets.Add(this);

  public void OnDestroy()
  {
    FollowerPet.FollowerPets.Remove(this);
    if (!FollowerPet.pets.ContainsKey(this.petType))
      return;
    FollowerPet.pets[this.petType]--;
  }

  public void Update()
  {
    if (!FollowerPet.dlcPets.Contains(this.petType))
      return;
    this.UpdateSkin();
  }

  public static int GetPetCount(FollowerPet.FollowerPetType petType)
  {
    return !FollowerPet.pets.ContainsKey(petType) ? 0 : FollowerPet.pets[petType];
  }

  public static string GetDLCPetSkin(FollowerPet.FollowerPetType petType)
  {
    switch (petType)
    {
      case FollowerPet.FollowerPetType.Goat:
        return "Goat";
      case FollowerPet.FollowerPetType.Cow:
        return "Cow";
      case FollowerPet.FollowerPetType.Crab:
        return "Crab";
      case FollowerPet.FollowerPetType.Llama:
        return "Llama";
      case FollowerPet.FollowerPetType.Snail:
        return "Snail";
      case FollowerPet.FollowerPetType.BigSpider:
        return "Spider";
      case FollowerPet.FollowerPetType.Turtle:
        return "Turtle";
      default:
        return (string) null;
    }
  }

  public void Sleep() => this.critter.IsSleeping = true;

  public void StopSleeping() => this.critter.IsSleeping = false;

  public static FollowerPet.DLCPet GetNewPetData(
    InventoryItem.ITEM_TYPE itemType,
    FollowerPet.FollowerPetType petType)
  {
    return new FollowerPet.DLCPet()
    {
      ItemType = itemType,
      PetType = petType,
      Horns = UnityEngine.Random.Range(1, 6),
      Ears = UnityEngine.Random.Range(1, 6),
      Head = UnityEngine.Random.Range(1, 6),
      Colour = UnityEngine.Random.Range(0, 10)
    };
  }

  public void UpdateSkin()
  {
    Skin newSkin = new Skin("Skin");
    string dlcPetSkin = FollowerPet.GetDLCPetSkin(this.dlcPetData.PetType);
    newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{dlcPetSkin}/{dlcPetSkin}"));
    newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin("State/Normal"));
    if (this.dlcPetData.PetType == FollowerPet.FollowerPetType.Goat)
    {
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{dlcPetSkin}/Horns/{this.dlcPetData.Horns}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{dlcPetSkin}/Ears/{this.dlcPetData.Ears}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{dlcPetSkin}/Head/{this.dlcPetData.Head}"));
    }
    else if (this.dlcPetData.PetType == FollowerPet.FollowerPetType.Cow)
    {
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{dlcPetSkin}/Horns/{this.dlcPetData.Horns}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{dlcPetSkin}/Ears/{this.dlcPetData.Ears}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{dlcPetSkin}/Head/{this.dlcPetData.Head}"));
    }
    else if (this.dlcPetData.PetType == FollowerPet.FollowerPetType.Llama)
    {
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{dlcPetSkin}/Tail/{this.dlcPetData.Horns}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{dlcPetSkin}/Ears/{this.dlcPetData.Ears}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{dlcPetSkin}/Head/{this.dlcPetData.Head}"));
    }
    else if (this.dlcPetData.PetType == FollowerPet.FollowerPetType.Crab)
    {
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{dlcPetSkin}/Claws/{this.dlcPetData.Horns}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{dlcPetSkin}/Shell/{this.dlcPetData.Ears}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{dlcPetSkin}/Head/{this.dlcPetData.Head}"));
    }
    else if (this.dlcPetData.PetType == FollowerPet.FollowerPetType.Snail)
    {
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{dlcPetSkin}/Body/{this.dlcPetData.Horns}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{dlcPetSkin}/Face/{this.dlcPetData.Ears}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{dlcPetSkin}/Shell/{this.dlcPetData.Head}"));
    }
    else if (this.dlcPetData.PetType == FollowerPet.FollowerPetType.BigSpider)
    {
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{dlcPetSkin}/Body/{this.dlcPetData.Horns}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{dlcPetSkin}/Eyes/{this.dlcPetData.Ears}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{dlcPetSkin}/Head/{this.dlcPetData.Head}"));
    }
    else if (this.dlcPetData.PetType == FollowerPet.FollowerPetType.Turtle)
    {
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{dlcPetSkin}/Body/{this.dlcPetData.Horns}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{dlcPetSkin}/Face/{this.dlcPetData.Ears}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{dlcPetSkin}/Head/{this.dlcPetData.Head}"));
    }
    this.spine.Skeleton.SetSkin(newSkin);
    if (!((UnityEngine.Object) AnimalData.Instance != (UnityEngine.Object) null))
      return;
    WorshipperData.SlotsAndColours[] animalColors = AnimalData.Instance.GetAnimalColors(this.dlcPetData.ItemType);
    foreach (WorshipperData.SlotAndColor slotAndColour in animalColors[Mathf.Clamp(this.dlcPetData.Colour, 0, animalColors.Length - 1)].SlotAndColours)
    {
      Slot slot = this.spine.Skeleton.FindSlot(slotAndColour.Slot);
      if (slot != null)
        slot.SetColor(slotAndColour.color);
    }
  }

  [MessagePackObject(false)]
  [Serializable]
  public struct DLCPet
  {
    [Key(0)]
    public InventoryItem.ITEM_TYPE ItemType;
    [Key(1)]
    public FollowerPet.FollowerPetType PetType;
    [Key(2)]
    public int Horns;
    [Key(3)]
    public int Ears;
    [Key(4)]
    public int Head;
    [Key(5)]
    public int Colour;
  }

  public enum FollowerPetType
  {
    Spider,
    Poop,
    Goat,
    Cow,
    Crab,
    Llama,
    Snail,
    BigSpider,
    Turtle,
  }
}
