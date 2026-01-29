// Decompiled with JetBrains decompiler
// Type: Objectives_RecruitFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_RecruitFollower : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public int Count;

  public override string Text
  {
    get
    {
      return this.Count == 1 ? ScriptLocalization.Objectives_Custom.GetNewFollowersFromDungeon : string.Format(ScriptLocalization.Objectives_RecruitFollower.Plural, (object) this.Count, (object) LocalizeIntegration.ReverseText(DataManager.Instance.Followers.Count.ToString()), (object) this.Count);
    }
  }

  public Objectives_RecruitFollower()
  {
  }

  public Objectives_RecruitFollower(string groupId, int count = 1)
    : base(groupId)
  {
    this.Type = Objectives.TYPES.RECRUIT_FOLLOWER;
    this.Count = count;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_RecruitFollower.FinalizedData_RecruitFollower finalizedData = new Objectives_RecruitFollower.FinalizedData_RecruitFollower();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.Count = this.Count;
    finalizedData.FollowerCount = DataManager.Instance.Followers.Count;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public override bool CheckComplete() => DataManager.Instance.Followers.Count >= this.Count;

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_RecruitFollower : ObjectivesDataFinalized
  {
    [Key(3)]
    public int Count;
    [Key(4)]
    public int FollowerCount;

    public override string GetText()
    {
      return this.Count == 1 ? ScriptLocalization.Objectives_Custom.GetNewFollowersFromDungeon : string.Format(ScriptLocalization.Objectives_RecruitFollower.Plural, (object) this.Count, (object) LocalizeIntegration.ReverseText(this.FollowerCount.ToString()), (object) this.Count);
    }
  }
}
