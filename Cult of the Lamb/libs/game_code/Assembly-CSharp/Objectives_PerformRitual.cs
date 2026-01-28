// Decompiled with JetBrains decompiler
// Type: Objectives_PerformRitual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_PerformRitual : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public UpgradeSystem.Type Ritual;
  [Key(17)]
  public int TargetFollowerID_1 = -1;
  [Key(18)]
  public int TargetFollowerID_2 = -1;
  [Key(19)]
  public int RequiredFollowers;
  [IgnoreMember]
  public bool complete;

  [IgnoreMember]
  public override string Text
  {
    get
    {
      string translation = LocalizationManager.GetTranslation("Objectives/PerformRitual/" + this.Ritual.ToString());
      string name1 = this.TargetFollowerID_1 != -1 ? FollowerInfo.GetInfoByID(this.TargetFollowerID_1, true)?.Name : "";
      string name2 = this.TargetFollowerID_2 != -1 ? FollowerInfo.GetInfoByID(this.TargetFollowerID_2, true)?.Name : "";
      string str1 = name1;
      string str2 = name2;
      return string.Format(translation, (object) str1, (object) str2);
    }
  }

  public Objectives_PerformRitual()
  {
  }

  public Objectives_PerformRitual(
    string groupId,
    UpgradeSystem.Type ritual,
    int targetFollowerID = -1,
    int requiredFollowers = 0,
    float questExpireDuration = -1f)
    : base(groupId, questExpireDuration)
  {
    this.Type = Objectives.TYPES.PERFORM_RITUAL;
    this.Ritual = ritual;
    this.RequiredFollowers = requiredFollowers;
    this.TargetFollowerID_1 = targetFollowerID;
  }

  public override void Init(bool initialAssigning)
  {
    base.Init(initialAssigning);
    this.complete = false;
    if (UpgradeSystem.GetUnlocked(this.Ritual) || this.FailLocked)
      return;
    this.Failed();
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_PerformRitual.FinalizedData_PerformRitual finalizedData = new Objectives_PerformRitual.FinalizedData_PerformRitual();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.Ritual = this.Ritual;
    finalizedData.TargetFollowerName_1 = FollowerInfo.GetInfoByID(this.TargetFollowerID_1, true)?.Name;
    finalizedData.TargetFollowerName_2 = FollowerInfo.GetInfoByID(this.TargetFollowerID_2, true)?.Name;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public void Init(int targetFollowerID)
  {
    this.TargetFollowerID_1 = targetFollowerID;
    this.Init(true);
  }

  public override bool CheckComplete()
  {
    if (!this.IsFailed && !this.complete && !this.FailLocked)
    {
      if (this.TargetFollowerID_1 != -1 && FollowerInfo.GetInfoByID(this.TargetFollowerID_1) == null)
        this.Failed();
      else if (this.TargetFollowerID_2 != -1 && FollowerInfo.GetInfoByID(this.TargetFollowerID_2) == null)
        this.Failed();
      else if (this.TargetFollowerID_1 == -1 && this.TargetFollowerID_2 == -1 && this.Follower != -1 && FollowerInfo.GetInfoByID(this.Follower) == null)
        this.Failed();
      else if (!UpgradeSystem.GetUnlocked(this.Ritual))
        this.Failed();
    }
    return !this.IsFailed && this.complete;
  }

  public void CheckComplete(int targetFollowerID_1, int targetFollowerID_2)
  {
    if (this.RequiredFollowers == 1 && this.TargetFollowerID_1 == targetFollowerID_1)
      this.complete = true;
    else if (this.RequiredFollowers == 2 && ((this.TargetFollowerID_1 == targetFollowerID_1 || this.TargetFollowerID_1 == -1) && this.TargetFollowerID_2 == targetFollowerID_2 || this.TargetFollowerID_2 == -1))
      this.complete = true;
    else if (this.RequiredFollowers == 2 && ((this.TargetFollowerID_1 == targetFollowerID_2 || this.TargetFollowerID_1 == -1) && this.TargetFollowerID_2 == targetFollowerID_1 || this.TargetFollowerID_2 == -1))
      this.complete = true;
    else if (this.RequiredFollowers <= 0)
      this.complete = true;
    if (this.Ritual != UpgradeSystem.Type.Ritual_FollowerWedding)
      return;
    if (targetFollowerID_1 == this.TargetFollowerID_1 && targetFollowerID_2 != this.TargetFollowerID_2)
    {
      this.Failed();
      this.complete = false;
    }
    else
    {
      if (targetFollowerID_1 == this.TargetFollowerID_1 || targetFollowerID_2 != this.TargetFollowerID_2)
        return;
      this.Failed();
      this.complete = false;
    }
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_PerformRitual : ObjectivesDataFinalized
  {
    [Key(3)]
    public UpgradeSystem.Type Ritual;
    [Key(4)]
    public string TargetFollowerName_1;
    [Key(5)]
    public string TargetFollowerName_2;

    public override string GetText()
    {
      return string.Format(LocalizationManager.GetTranslation("Objectives/PerformRitual/" + this.Ritual.ToString()), (object) this.TargetFollowerName_1, (object) this.TargetFollowerName_2);
    }
  }
}
