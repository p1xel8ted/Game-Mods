// Decompiled with JetBrains decompiler
// Type: Objectives_AssignClothing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_AssignClothing : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public FollowerClothingType ClothingType;
  [Key(17)]
  public int TargetFollower;
  [IgnoreMember]
  public bool complete;

  public override string Text
  {
    get
    {
      string name = FollowerInfo.GetInfoByID(this.TargetFollower, true)?.Name;
      string str = TailorManager.LocalizedName(this.ClothingType);
      return string.Format(LocalizationManager.GetTranslation("Objectives/AssignClothing"), (object) name, (object) str);
    }
  }

  public Objectives_AssignClothing()
  {
  }

  public Objectives_AssignClothing(
    string groupId,
    FollowerClothingType clothingType,
    float questExpireDuration = -1f)
    : base(groupId, questExpireDuration)
  {
    this.Type = Objectives.TYPES.ASSIGN_CLOTHING;
    this.ClothingType = clothingType;
  }

  public override void Init(bool initialAssigning)
  {
    base.Init(initialAssigning);
    this.complete = false;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_AssignClothing.FinalizedData_AssignClothing finalizedData = new Objectives_AssignClothing.FinalizedData_AssignClothing();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.ClothingType = this.ClothingType;
    finalizedData.TargetFollowerName = FollowerInfo.GetInfoByID(this.TargetFollower, true)?.Name;
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
    if (this.IsFailed || this.TargetFollower == -1 || FollowerInfo.GetInfoByID(this.TargetFollower) != null)
      return;
    this.Failed();
  }

  public void CheckComplete(FollowerClothingType clothingType, int targetFollowerID_1)
  {
    if (clothingType == this.ClothingType && this.TargetFollower == targetFollowerID_1)
      this.complete = true;
    else if (FollowerInfo.GetInfoByID(this.TargetFollower) == null)
      this.Failed();
    this.CheckComplete();
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_AssignClothing : ObjectivesDataFinalized
  {
    [Key(3)]
    public FollowerClothingType ClothingType;
    [Key(4)]
    public string TargetFollowerName;

    public override string GetText()
    {
      string str = TailorManager.LocalizedName(this.ClothingType);
      return string.Format(LocalizationManager.GetTranslation("Objectives/AssignClothing"), (object) this.TargetFollowerName, (object) str);
    }
  }
}
