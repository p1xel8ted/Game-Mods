// Decompiled with JetBrains decompiler
// Type: Objectives_BuildStructure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;

#nullable disable
[Serializable]
public class Objectives_BuildStructure : ObjectivesData
{
  public StructureBrain.TYPES StructureType;
  public int Target;
  public int Count;

  public override string Text
  {
    get
    {
      StructuresData infoByType = StructuresData.GetInfoByType(this.StructureType, 0);
      return this.Target > 1 ? string.Format(ScriptLocalization.Objectives_BuildStructure.Plural, (object) infoByType.GetLocalizedName(), (object) this.Count.ToString(), (object) this.Target.ToString()) : string.Format(ScriptLocalization.Objectives.BuildStructure, (object) infoByType.GetLocalizedName());
    }
  }

  public Objectives_BuildStructure()
  {
  }

  public Objectives_BuildStructure(
    string groupId,
    StructureBrain.TYPES structureType,
    int target = 1,
    float expireTimestamp = -1f)
    : base(groupId, expireTimestamp)
  {
    this.Type = Objectives.TYPES.BUILD_STRUCTURE;
    this.StructureType = structureType;
    this.Target = target;
    this.Count = 0;
  }

  public override void Init(bool initialAssigning)
  {
    if (!this.initialised)
      StructureManager.OnStructureAdded += new StructureManager.StructureChanged(this.OnStructureAdded);
    if (initialAssigning)
      this.Count = 0;
    base.Init(initialAssigning);
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

  private void OnStructureAdded(StructuresData structure)
  {
    if (structure.Type != this.StructureType)
      return;
    ++this.Count;
    ObjectiveManager.CheckObjectives(Objectives.TYPES.BUILD_STRUCTURE);
  }

  protected override bool CheckComplete() => this.Count >= this.Target;

  [Serializable]
  public class FinalizedData_BuildStructure : ObjectivesDataFinalized
  {
    public StructureBrain.TYPES StructureType;
    public int Target;
    public int Count;

    public override string GetText()
    {
      StructuresData infoByType = StructuresData.GetInfoByType(this.StructureType, 0);
      return this.Target > 1 ? string.Format(ScriptLocalization.Objectives_BuildStructure.Plural, (object) infoByType.GetLocalizedName(), (object) this.Count.ToString(), (object) this.Target.ToString()) : string.Format(ScriptLocalization.Objectives.BuildStructure, (object) infoByType.GetLocalizedName());
    }
  }
}
