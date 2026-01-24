// Decompiled with JetBrains decompiler
// Type: Objectives_LegendarySwordReturn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_LegendarySwordReturn : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public string TargetFollowerName = "";

  public override string Text
  {
    get
    {
      int num = TimeManager.CurrentDay - DataManager.Instance.ChosenChildMeditationQuestDay;
      return num >= 3 ? string.Format(LocalizationManager.GetTranslation("Objectives/LegendarySwordReturn/Finish"), (object) this.TargetFollowerName) : string.Format(LocalizationManager.GetTranslation("Objectives/LegendarySwordReturn"), (object) (3 - num));
    }
  }

  public Objectives_LegendarySwordReturn()
  {
  }

  public Objectives_LegendarySwordReturn(
    string groupId,
    string followerName,
    float questExpireDuration = -1f)
    : base(groupId, questExpireDuration)
  {
    this.Type = Objectives.TYPES.LEGENDARY_SWORD_RETURN;
    this.TargetFollowerName = followerName;
    this.IsWinterObjective = true;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_LegendarySwordReturn.FinalizedData_LegendarySwordReturn finalizedData = new Objectives_LegendarySwordReturn.FinalizedData_LegendarySwordReturn();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.TargetFollowerName = this.TargetFollowerName;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public override bool CheckComplete() => this.IsComplete;

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_LegendarySwordReturn : ObjectivesDataFinalized
  {
    [Key(3)]
    public string TargetFollowerName = "";

    public override string GetText()
    {
      return string.Format(LocalizationManager.GetTranslation("Objectives/LegendarySwordReturn/Finish"), (object) this.TargetFollowerName);
    }
  }
}
