// Decompiled with JetBrains decompiler
// Type: Objectives_GetAnimal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_GetAnimal : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public InventoryItem.ITEM_TYPE AnimalType;
  [Key(17)]
  public int Level;
  [IgnoreMember]
  public bool complete;

  public override string Text
  {
    get
    {
      return string.Format(LocalizationManager.GetTranslation($"Objectives/GetAnimal/{this.AnimalType}"), (object) this.Level.ToNumeral());
    }
  }

  public Objectives_GetAnimal()
  {
  }

  public Objectives_GetAnimal(
    string groupId,
    InventoryItem.ITEM_TYPE animalType,
    int level,
    float questExpireDuration = -1f)
    : base(groupId, questExpireDuration)
  {
    this.Type = Objectives.TYPES.GET_ANIMAL;
    this.AnimalType = animalType;
    this.Level = level;
  }

  public override void Init(bool initialAssigning)
  {
    base.Init(initialAssigning);
    this.complete = false;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_GetAnimal.FinalizedData_GetAnimal finalizedData = new Objectives_GetAnimal.FinalizedData_GetAnimal();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.AnimalType = this.AnimalType;
    finalizedData.Level = this.Level;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public override bool CheckComplete() => this.IsComplete;

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_GetAnimal : ObjectivesDataFinalized
  {
    [Key(3)]
    public InventoryItem.ITEM_TYPE AnimalType;
    [Key(4)]
    public int Level;

    public override string GetText()
    {
      return string.Format(LocalizationManager.GetTranslation($"Objectives/GetAnimal/{this.AnimalType}"), (object) this.Level.ToNumeral());
    }
  }
}
