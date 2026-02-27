// Decompiled with JetBrains decompiler
// Type: Objectives_RecruitFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;

#nullable disable
[Serializable]
public class Objectives_RecruitFollower : ObjectivesData
{
  public int Count;

  public override string Text
  {
    get
    {
      return this.Count == 1 ? ScriptLocalization.Objectives_Custom.GetNewFollowersFromDungeon : string.Format(ScriptLocalization.Objectives_RecruitFollower.Plural, (object) this.Count, (object) DataManager.Instance.Followers.Count, (object) this.Count);
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

  protected override bool CheckComplete() => DataManager.Instance.Followers.Count >= this.Count;

  [Serializable]
  public class FinalizedData_RecruitFollower : ObjectivesDataFinalized
  {
    public int Count;
    public int FollowerCount;

    public override string GetText()
    {
      return this.Count == 1 ? ScriptLocalization.Objectives_Custom.GetNewFollowersFromDungeon : string.Format(ScriptLocalization.Objectives_RecruitFollower.Plural, (object) this.Count, (object) this.FollowerCount, (object) this.Count);
    }
  }
}
