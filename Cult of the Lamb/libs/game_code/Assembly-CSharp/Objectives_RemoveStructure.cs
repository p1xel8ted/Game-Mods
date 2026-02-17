// Decompiled with JetBrains decompiler
// Type: Objectives_RemoveStructure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_RemoveStructure : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public StructureBrain.TYPES StructureType;
  [Key(17)]
  public int Target = -1;
  [Key(18)]
  public int Count;

  [IgnoreMember]
  public override string Text
  {
    get
    {
      return $"{ScriptLocalization.Interactions.Clean} {StructuresData.GetInfoByType(this.StructureType, 0).GetLocalizedName()} ({this.Count.ToString()}/{this.Target.ToString()})";
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

  public void StructureRemoved(StructuresData structure)
  {
    if (structure.Type != this.StructureType)
      return;
    ++this.Count;
  }

  public override bool CheckComplete() => this.Count >= this.Target;

  public override void Complete()
  {
    base.Complete();
    StructureManager.OnStructureRemoved -= new StructureManager.StructureChanged(this.StructureRemoved);
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_RemoveStructure : ObjectivesDataFinalized
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
      return LocalizeIntegration.IsArabic() ? $"{ScriptLocalization.Interactions.Clean} {infoByType.GetLocalizedName()} ){LocalizeIntegration.FormatCurrentMax(this.Count.ToString(), this.Target.ToString())}(" : $"{ScriptLocalization.Interactions.Clean} {infoByType.GetLocalizedName()} ({LocalizeIntegration.FormatCurrentMax(this.Count.ToString(), this.Target.ToString())})";
    }
  }
}
