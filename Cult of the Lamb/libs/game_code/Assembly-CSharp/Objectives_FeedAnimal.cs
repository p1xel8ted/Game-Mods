// Decompiled with JetBrains decompiler
// Type: Objectives_FeedAnimal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;
using UnityEngine;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_FeedAnimal : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public int TargetAnimal;
  [Key(17)]
  public InventoryItem.ITEM_TYPE Food;
  [IgnoreMember]
  public string initialTargetAnimalName;
  [IgnoreMember]
  public bool complete;

  public override string Text
  {
    get
    {
      StructuresData.Ranchable_Animal animalById = StructureManager.GetAnimalByID(this.TargetAnimal);
      string str = "";
      if (animalById != null)
        str = string.IsNullOrEmpty(animalById.GivenName) ? InventoryItem.LocalizedName(animalById.Type) : animalById.GivenName;
      else
        Debug.LogError((object) "Why animal is null?");
      return string.Format(LocalizationManager.GetTranslation("Objectives/FeedAnimal"), (object) str, (object) FontImageNames.GetIconByType(this.Food));
    }
  }

  public Objectives_FeedAnimal()
  {
  }

  public Objectives_FeedAnimal(
    string groupId,
    int targetAnimal,
    InventoryItem.ITEM_TYPE food,
    float questExpireDuration = -1f)
    : base(groupId, questExpireDuration)
  {
    this.Type = Objectives.TYPES.FEED_ANIMAL;
    this.TargetAnimal = targetAnimal;
    this.Food = food;
    this.IsWinterObjective = true;
  }

  public override void Init(bool initialAssigning)
  {
    base.Init(initialAssigning);
    this.complete = false;
    StructuresData.Ranchable_Animal animalById = StructureManager.GetAnimalByID(this.TargetAnimal);
    if (!string.IsNullOrEmpty(animalById.GivenName))
      this.initialTargetAnimalName = animalById.GivenName;
    else
      this.initialTargetAnimalName = InventoryItem.LocalizedName(animalById.Type);
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    StructuresData.Ranchable_Animal animalById = StructureManager.GetAnimalByID(this.TargetAnimal);
    string str = animalById != null ? (string.IsNullOrEmpty(animalById.GivenName) ? InventoryItem.LocalizedName(animalById.Type) : animalById.GivenName) : this.initialTargetAnimalName;
    Objectives_FeedAnimal.FinalizedData_FeedAnimal finalizedData = new Objectives_FeedAnimal.FinalizedData_FeedAnimal();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.TargetAnimalName = str;
    finalizedData.Food = this.Food;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public override bool CheckComplete()
  {
    base.CheckComplete();
    return this.complete;
  }

  public override void Update()
  {
    base.Update();
    if (this.IsFailed || this.TargetAnimal == -1 || StructureManager.GetAnimalByID(this.TargetAnimal) != null && StructureManager.GetAnimalByID(this.TargetAnimal).State != Interaction_Ranchable.State.Dead)
      return;
    this.Failed();
  }

  public void CheckComplete(int animalID, InventoryItem.ITEM_TYPE food)
  {
    if (animalID == this.TargetAnimal && this.Food == food)
      this.complete = true;
    else if (StructureManager.GetAnimalByID(this.TargetAnimal) == null || StructureManager.GetAnimalByID(this.TargetAnimal).State == Interaction_Ranchable.State.Dead)
      this.Failed();
    this.CheckComplete();
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_FeedAnimal : ObjectivesDataFinalized
  {
    [Key(3)]
    public string TargetAnimalName;
    [Key(4)]
    public InventoryItem.ITEM_TYPE Food;

    public override string GetText()
    {
      return string.Format(LocalizationManager.GetTranslation("Objectives/FeedAnimal"), (object) this.TargetAnimalName, (object) FontImageNames.GetIconByType(this.Food));
    }
  }
}
