// Decompiled with JetBrains decompiler
// Type: Objectives_RemoveStructure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;

#nullable disable
[Serializable]
public class Objectives_RemoveStructure : ObjectivesData
{
  public StructureBrain.TYPES StructureType;
  public int Target = -1;
  public int Count;

  public override string Text
  {
    get
    {
      return $"{ScriptLocalization.Interactions.Clean} {StructuresData.GetInfoByType(this.StructureType, 0).GetLocalizedName()} ({(object) this.Count}/{(object) this.Target})";
    }
  }

  public Objectives_RemoveStructure()
  {
  }

  public Objectives_RemoveStructure(string groupId, StructureBrain.TYPES structureType)
    : base(groupId)
  {
    this.Type = Objectives.TYPES.REMOVE_STRUCTURES;
    this.StructureType = structureType;
  }

  public override void Init(bool initialAssigning)
  {
    if (!this.initialised)
      StructureManager.OnStructureRemoved += new StructureManager.StructureChanged(this.StructureRemoved);
    base.Init(initialAssigning);
    if (this.Target != -1)
      return;
    this.Target = StructureManager.GetAllStructuresOfType(this.StructureType).Count;
    this.Count = 0;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_RemoveStructure.FinalizedData_RemoveStructure finalizedData = new Objectives_RemoveStructure.FinalizedData_RemoveStructure();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.StructureType = this.StructureType;
    finalizedData.Target = this.Target;
    finalizedData.Count = this.Count;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  private void StructureRemoved(StructuresData structure)
  {
    if (structure.Type != this.StructureType)
      return;
    ++this.Count;
  }

  protected override bool CheckComplete() => this.Count >= this.Target;

  public override void Complete()
  {
    base.Complete();
    StructureManager.OnStructureRemoved -= new StructureManager.StructureChanged(this.StructureRemoved);
  }

  [Serializable]
  public class FinalizedData_RemoveStructure : ObjectivesDataFinalized
  {
    public StructureBrain.TYPES StructureType;
    public int Target;
    public int Count;

    public override string GetText()
    {
      return $"{ScriptLocalization.Interactions.Clean} {StructuresData.GetInfoByType(this.StructureType, 0).GetLocalizedName()} ({(object) this.Count}/{(object) this.Target})";
    }
  }
}
