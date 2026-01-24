// Decompiled with JetBrains decompiler
// Type: Objectives_WalkAnimal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_WalkAnimal : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public int TargetAnimal;
  [IgnoreMember]
  public bool complete;

  public override string Text
  {
    get
    {
      StructuresData.Ranchable_Animal animalById = StructureManager.GetAnimalByID(this.TargetAnimal);
      string str = string.IsNullOrEmpty(animalById.GivenName) ? InventoryItem.LocalizedName(animalById.Type) : animalById.GivenName;
      return string.Format(LocalizationManager.GetTranslation("Objectives/WalkAnimal"), (object) str);
    }
  }

  public Objectives_WalkAnimal()
  {
  }

  public Objectives_WalkAnimal(string groupId, int targetAnimal, float questExpireDuration = -1f)
    : base(groupId, questExpireDuration)
  {
    this.Type = Objectives.TYPES.WALK_ANIMAL;
    this.TargetAnimal = targetAnimal;
    this.IsWinterObjective = true;
  }

  public override void Init(bool initialAssigning)
  {
    base.Init(initialAssigning);
    this.complete = false;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    StructuresData.Ranchable_Animal animalById = StructureManager.GetAnimalByID(this.TargetAnimal);
    string str = string.IsNullOrEmpty(animalById.GivenName) ? InventoryItem.LocalizedName(animalById.Type) : animalById.GivenName;
    Objectives_WalkAnimal.FinalizedData_WalkAnimal finalizedData = new Objectives_WalkAnimal.FinalizedData_WalkAnimal();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.TargetAnimalName = str;
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

  public void CheckComplete(int animalID)
  {
    if (animalID == this.TargetAnimal)
      this.complete = true;
    else if (StructureManager.GetAnimalByID(this.TargetAnimal) == null)
      this.Failed();
    this.CheckComplete();
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_WalkAnimal : ObjectivesDataFinalized
  {
    [Key(3)]
    public string TargetAnimalName;

    public override string GetText()
    {
      return string.Format(LocalizationManager.GetTranslation("Objectives/WalkAnimal"), (object) this.TargetAnimalName);
    }
  }
}
