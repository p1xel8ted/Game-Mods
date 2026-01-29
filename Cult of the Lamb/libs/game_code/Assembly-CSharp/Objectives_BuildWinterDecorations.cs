// Decompiled with JetBrains decompiler
// Type: Objectives_BuildWinterDecorations
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_BuildWinterDecorations : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public int Target;
  [Key(17)]
  public int Count;

  public override string Text
  {
    get
    {
      return string.Format(ScriptLocalization.Objectives_BuildStructure.WinterDecorations, (object) LocalizeIntegration.ReverseText(this.Count.ToString()), (object) LocalizeIntegration.ReverseText(this.Target.ToString()));
    }
  }

  public Objectives_BuildWinterDecorations()
  {
  }

  public Objectives_BuildWinterDecorations(string groupId, int target = 1, float expireTimestamp = -1f)
    : base(groupId, expireTimestamp)
  {
    this.Type = Objectives.TYPES.BUILD_WINTER_DECORATION;
    this.Target = target;
    this.Count = 0;
    this.IsWinterObjective = true;
  }

  public override void Init(bool initialAssigning)
  {
    if (!this.initialised)
      StructureManager.OnStructureAdded += new StructureManager.StructureChanged(this.OnStructureAdded);
    if (initialAssigning)
      this.Count = 0;
    base.Init(initialAssigning);
  }

  public override void Complete()
  {
    base.Complete();
    StructureManager.OnStructureAdded -= new StructureManager.StructureChanged(this.OnStructureAdded);
  }

  public override void Failed()
  {
    base.Failed();
    StructureManager.OnStructureAdded -= new StructureManager.StructureChanged(this.OnStructureAdded);
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_BuildWinterDecorations.FinalizedData_BuildWinterDecorations finalizedData = new Objectives_BuildWinterDecorations.FinalizedData_BuildWinterDecorations();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.Target = this.Target;
    finalizedData.Count = this.Count;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public void OnStructureAdded(StructuresData structure)
  {
    if (!DataManager.DecorationsForType(DataManager.DecorationType.Major_DLC).Contains(structure.Type))
      return;
    ++this.Count;
    ObjectiveManager.CheckObjectives(Objectives.TYPES.BUILD_WINTER_DECORATION);
  }

  public override bool CheckComplete() => this.Count >= this.Target;

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_BuildWinterDecorations : ObjectivesDataFinalized
  {
    [Key(3)]
    public int Target;
    [Key(4)]
    public int Count;

    public override string GetText()
    {
      return string.Format(ScriptLocalization.Objectives_BuildStructure.WinterDecorations, (object) LocalizeIntegration.ReverseText(this.Count.ToString()), (object) LocalizeIntegration.ReverseText(this.Target.ToString()));
    }
  }
}
