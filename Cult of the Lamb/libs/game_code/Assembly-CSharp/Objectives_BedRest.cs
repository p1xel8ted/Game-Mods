// Decompiled with JetBrains decompiler
// Type: Objectives_BedRest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_BedRest : ObjectivesData
{
  [Key(16 /*0x10*/)]
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

  public override bool CheckComplete() => true;

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_BedRest : ObjectivesDataFinalized
  {
    [Key(3)]
    public string FollowerName;

    public override string GetText()
    {
      return string.Format(LocalizationManager.GetTranslation("Objectives/SendFollowerToBedRest"), (object) this.FollowerName);
    }
  }
}
