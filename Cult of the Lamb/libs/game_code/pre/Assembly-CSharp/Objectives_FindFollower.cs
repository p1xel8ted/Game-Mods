// Decompiled with JetBrains decompiler
// Type: Objectives_FindFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;

#nullable disable
[Serializable]
public class Objectives_FindFollower : ObjectivesData
{
  public FollowerLocation TargetLocation;
  public string FollowerSkin;
  public int FollowerColour;
  public int FollowerVariant;
  public string TargetFollowerName;
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

  protected override bool CheckComplete()
  {
    base.CheckComplete();
    return this.IsComplete;
  }

  [Serializable]
  public class FinalizedData_FindFollower : ObjectivesDataFinalized
  {
    public FollowerLocation TargetLocation;
    public string TargetFollowerName;

    public override string GetText()
    {
      return string.Format(ScriptLocalization.Objectives.FindFollower, (object) this.TargetFollowerName, (object) LocalizationManager.GetTranslation($"NAMES/Places/{this.TargetLocation}"));
    }
  }
}
