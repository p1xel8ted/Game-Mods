// Decompiled with JetBrains decompiler
// Type: Objectives_BuildStructure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;
using System.Runtime.CompilerServices;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_BuildStructure : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public StructureBrain.TYPES StructureType;
  [Key(17)]
  public int Target;
  [Key(18)]
  public int Count;
  [Key(19)]
  public bool IncludeAlreadyBuilt;

  public override string Text
  {
    get
    {
      string str = StructuresData.GetInfoByType(this.StructureType, 0)?.GetLocalizedName() ?? "Unknown";
      return this.Target > 1 ? string.Format(ScriptLocalization.Objectives_BuildStructure.Plural, (object) str, (object) LocalizeIntegration.ReverseText(this.Count.ToString()), (object) LocalizeIntegration.ReverseText(this.Target.ToString())) : string.Format(ScriptLocalization.Objectives.BuildStructure, (object) str);
    }
  }

  public Objectives_BuildStructure()
  {
  }

  public Objectives_BuildStructure(
    string groupId,
    StructureBrain.TYPES structureType,
    int target = 1,
    float expireTimestamp = -1f,
    bool includeAlreadyBuilt = false)
    : base(groupId, expireTimestamp)
  {
    this.Type = Objectives.TYPES.BUILD_STRUCTURE;
    this.StructureType = structureType;
    this.Target = target;
    this.IncludeAlreadyBuilt = includeAlreadyBuilt;
    this.Count = includeAlreadyBuilt ? StructureManager.AccumulateOnAllStructures(FollowerLocation.Base, (Func<StructureBrain, int>) (s => s.Data.Type != this.StructureType ? 0 : 1)) : 0;
  }

  public override void Init(bool initialAssigning)
  {
    if (!this.initialised)
      StructureManager.OnStructureAdded += new StructureManager.StructureChanged(this.OnStructureAdded);
    if (initialAssigning)
      this.Count = this.IncludeAlreadyBuilt ? StructureManager.AccumulateOnAllStructures(FollowerLocation.Base, (Func<StructureBrain, int>) (s => s.Data.Type != this.StructureType ? 0 : 1)) : 0;
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
    Objectives_BuildStructure.FinalizedData_BuildStructure finalizedData = new Objectives_BuildStructure.FinalizedData_BuildStructure();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.StructureType = this.StructureType;
    finalizedData.Target = this.Target;
    finalizedData.Count = this.Count;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public void OnStructureAdded(StructuresData structure)
  {
    bool flag = structure.Type == this.StructureType;
    if (this.StructureType == StructureBrain.TYPES.BED && structure.Type == StructureBrain.TYPES.SHARED_HOUSE)
      flag = true;
    if (!flag)
      return;
    ++this.Count;
    ObjectiveManager.CheckObjectives(Objectives.TYPES.BUILD_STRUCTURE);
  }

  public override bool CheckComplete() => this.Count >= this.Target;

  [CompilerGenerated]
  public int \u003C\u002Ector\u003Eb__8_0(StructureBrain s)
  {
    return s.Data.Type != this.StructureType ? 0 : 1;
  }

  [CompilerGenerated]
  public int \u003CInit\u003Eb__9_0(StructureBrain s) => s.Data.Type != this.StructureType ? 0 : 1;

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_BuildStructure : ObjectivesDataFinalized
  {
    [Key(3)]
    public StructureBrain.TYPES StructureType;
    [Key(4)]
    public int Target;
    [Key(5)]
    public int Count;

    public override string GetText()
    {
      StructuresData infoByType = StructuresData.GetInfoByType(this.StructureType, 0);
      return this.Target > 1 ? string.Format(ScriptLocalization.Objectives_BuildStructure.Plural, (object) infoByType.GetLocalizedName(), (object) LocalizeIntegration.ReverseText(this.Count.ToString()), (object) LocalizeIntegration.ReverseText(this.Target.ToString())) : string.Format(ScriptLocalization.Objectives.BuildStructure, (object) infoByType.GetLocalizedName());
    }
  }
}
