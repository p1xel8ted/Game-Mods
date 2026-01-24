// Decompiled with JetBrains decompiler
// Type: Objectives_Mating
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_Mating : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public int TargetFollower_1;
  [Key(17)]
  public int TargetFollower_2;
  [IgnoreMember]
  public bool complete;

  public override string Text
  {
    get
    {
      return string.Format(LocalizationManager.GetTranslation("Objectives/Mating"), (object) FollowerInfo.GetInfoByID(this.TargetFollower_1, true)?.Name, (object) FollowerInfo.GetInfoByID(this.TargetFollower_2, true)?.Name);
    }
  }

  public Objectives_Mating()
  {
  }

  public Objectives_Mating(
    string groupId,
    int follower1,
    int follower2,
    float questExpireDuration = -1f)
    : base(groupId, questExpireDuration)
  {
    this.Type = Objectives.TYPES.MATING;
    this.TargetFollower_1 = follower1;
    this.TargetFollower_2 = follower2;
  }

  public override void Init(bool initialAssigning)
  {
    base.Init(initialAssigning);
    this.complete = false;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_Mating.FinalizedData_Mating finalizedData = new Objectives_Mating.FinalizedData_Mating();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.TargetFollowerName_1 = FollowerInfo.GetInfoByID(this.TargetFollower_1, true)?.Name;
    finalizedData.TargetFollowerName_2 = FollowerInfo.GetInfoByID(this.TargetFollower_2, true)?.Name;
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
    if ((this.IsFailed || this.TargetFollower_1 == -1 || FollowerInfo.GetInfoByID(this.TargetFollower_1) != null) && (this.TargetFollower_2 == -1 || FollowerInfo.GetInfoByID(this.TargetFollower_2) != null))
      return;
    this.Failed();
  }

  public void CheckComplete(int targetFollowerID_1, int targetFollowerID_2)
  {
    if (this.TargetFollower_1 == targetFollowerID_1 && this.TargetFollower_2 == targetFollowerID_2 || this.TargetFollower_2 == targetFollowerID_1 && this.TargetFollower_1 == targetFollowerID_2)
      this.complete = true;
    else if (FollowerInfo.GetInfoByID(this.TargetFollower_1) == null || FollowerInfo.GetInfoByID(this.TargetFollower_2) == null)
      this.Failed();
    this.CheckComplete();
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_Mating : ObjectivesDataFinalized
  {
    [Key(3)]
    public string TargetFollowerName_1;
    [Key(4)]
    public string TargetFollowerName_2;

    public override string GetText()
    {
      return string.Format(LocalizationManager.GetTranslation("Objectives/Mating"), (object) this.TargetFollowerName_1, (object) this.TargetFollowerName_2);
    }
  }
}
