// Decompiled with JetBrains decompiler
// Type: Objectives_BedRest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;

#nullable disable
[Serializable]
public class Objectives_BedRest : ObjectivesData
{
  public string FollowerName;

  public override string Text
  {
    get
    {
      return string.Format(LocalizationManager.GetTranslation("Objectives/SendFollowerToBedRest"), (object) this.FollowerName);
    }
  }

  public Objectives_BedRest()
  {
  }

  public Objectives_BedRest(string groupId, string followerName)
    : base(groupId)
  {
    this.Type = Objectives.TYPES.SEND_FOLLOWER_BED_REST;
    this.FollowerName = followerName;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_BedRest.FinalizedData_BedRest finalizedData = new Objectives_BedRest.FinalizedData_BedRest();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.FollowerName = this.FollowerName;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  protected override bool CheckComplete() => true;

  [Serializable]
  public class FinalizedData_BedRest : ObjectivesDataFinalized
  {
    public string FollowerName;

    public override string GetText()
    {
      return string.Format(LocalizationManager.GetTranslation("Objectives/SendFollowerToBedRest"), (object) this.FollowerName);
    }
  }
}
