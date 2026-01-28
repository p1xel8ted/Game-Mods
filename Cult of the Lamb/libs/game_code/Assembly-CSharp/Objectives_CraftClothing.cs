// Decompiled with JetBrains decompiler
// Type: Objectives_CraftClothing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_CraftClothing : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public FollowerClothingType ClothingType;
  [IgnoreMember]
  public bool complete;

  public override string Text
  {
    get
    {
      string str = TailorManager.LocalizedName(this.ClothingType);
      return string.Format(LocalizationManager.GetTranslation("Objectives/CraftClothing"), (object) str);
    }
  }

  public Objectives_CraftClothing()
  {
  }

  public Objectives_CraftClothing(
    string groupId,
    FollowerClothingType clothingType,
    float questExpireDuration = -1f)
    : base(groupId, questExpireDuration)
  {
    this.Type = Objectives.TYPES.CRAFT_CLOTHING;
    this.ClothingType = clothingType;
  }

  public override void Init(bool initialAssigning)
  {
    base.Init(initialAssigning);
    this.complete = false;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_CraftClothing.FinalizedData_CraftClothing finalizedData = new Objectives_CraftClothing.FinalizedData_CraftClothing();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.ClothingType = this.ClothingType;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public override bool CheckComplete()
  {
    base.CheckComplete();
    return this.complete;
  }

  public void CheckComplete(FollowerClothingType clothingType)
  {
    if (clothingType == this.ClothingType)
      this.complete = true;
    this.CheckComplete();
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_CraftClothing : ObjectivesDataFinalized
  {
    [Key(3)]
    public FollowerClothingType ClothingType;

    public override string GetText()
    {
      string str = TailorManager.LocalizedName(this.ClothingType);
      return string.Format(LocalizationManager.GetTranslation("Objectives/CraftClothing"), (object) str);
    }
  }
}
