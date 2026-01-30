// Decompiled with JetBrains decompiler
// Type: Objectives_FindFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_FindFollower : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public FollowerLocation TargetLocation;
  [Key(17)]
  public string FollowerSkin;
  [Key(18)]
  public int FollowerColour;
  [Key(19)]
  public int FollowerVariant;
  [Key(20)]
  public string TargetFollowerName;
  [Key(21)]
  public int ObjectiveVariant;

  public override string Text
  {
    get
    {
      return string.Format(ScriptLocalization.Objectives.FindFollower, (object) this.TargetFollowerName, (object) LocalizationManager.GetTranslation($"NAMES/Places/{this.TargetLocation}"));
    }
  }

  public Objectives_FindFollower()
  {
  }

  public Objectives_FindFollower(
    string groupId,
    FollowerLocation targetLocation,
    string followerSkin,
    int followerColour,
    int followerVariant,
    string targetFollowerName,
    int objectiveVariant,
    float expireTimestamp = -1f)
    : base(groupId, expireTimestamp)
  {
    this.Type = Objectives.TYPES.FIND_FOLLOWER;
    this.TargetLocation = targetLocation;
    this.FollowerSkin = followerSkin;
    this.FollowerColour = followerColour;
    this.FollowerVariant = followerVariant;
    this.TargetFollowerName = targetFollowerName;
    this.ObjectiveVariant = objectiveVariant;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_FindFollower.FinalizedData_FindFollower finalizedData = new Objectives_FindFollower.FinalizedData_FindFollower();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.TargetLocation = this.TargetLocation;
    finalizedData.TargetFollowerName = this.TargetFollowerName;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public override bool CheckComplete()
  {
    base.CheckComplete();
    return this.IsComplete;
  }

  public override void Failed()
  {
    if (this.GroupId == "Objectives/GroupTitle/SozoStory")
      DataManager.Instance.SozoAteMushroomDay = 0;
    base.Failed();
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_FindFollower : ObjectivesDataFinalized
  {
    [Key(3)]
    public FollowerLocation TargetLocation;
    [Key(4)]
    public string TargetFollowerName;

    public override string GetText()
    {
      return string.Format(ScriptLocalization.Objectives.FindFollower, (object) this.TargetFollowerName, (object) LocalizationManager.GetTranslation($"NAMES/Places/{this.TargetLocation}"));
    }
  }
}
