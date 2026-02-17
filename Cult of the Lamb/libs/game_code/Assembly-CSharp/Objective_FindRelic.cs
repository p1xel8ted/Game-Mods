// Decompiled with JetBrains decompiler
// Type: Objective_FindRelic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objective_FindRelic : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public FollowerLocation TargetLocation;
  [Key(17)]
  public RelicType RelicType;

  public override string Text
  {
    get
    {
      return string.Format(ScriptLocalization.Objectives.FindRelic, (object) RelicData.GetTitleLocalisation(this.RelicType), (object) LocalizationManager.GetTranslation($"NAMES/Places/{this.TargetLocation}"));
    }
  }

  public Objective_FindRelic()
  {
  }

  public Objective_FindRelic(
    string groupId,
    FollowerLocation targetLocation,
    RelicType relicType,
    float expireTimestamp = -1f)
    : base(groupId, expireTimestamp)
  {
    this.Type = Objectives.TYPES.FIND_RELIC;
    this.TargetLocation = targetLocation;
    this.RelicType = relicType;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objective_FindRelic.FinalizedData_FindRelic finalizedData = new Objective_FindRelic.FinalizedData_FindRelic();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.TargetLocation = this.TargetLocation;
    finalizedData.RelicType = this.RelicType;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public override bool CheckComplete()
  {
    base.CheckComplete();
    return this.IsComplete;
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_FindRelic : ObjectivesDataFinalized
  {
    [Key(3)]
    public FollowerLocation TargetLocation;
    [Key(4)]
    public RelicType RelicType;

    public override string GetText()
    {
      return string.Format(ScriptLocalization.Objectives.FindRelic, (object) RelicData.GetTitleLocalisation(this.RelicType), (object) LocalizationManager.GetTranslation($"NAMES/Places/{this.TargetLocation}"));
    }
  }
}
