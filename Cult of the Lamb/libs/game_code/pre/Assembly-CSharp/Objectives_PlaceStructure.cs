// Decompiled with JetBrains decompiler
// Type: Objectives_PlaceStructure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;

#nullable disable
[Serializable]
public class Objectives_PlaceStructure : ObjectivesData
{
  public StructureBrain.Categories category;
  public int Count;
  public int Target;
  public string Term;

  public override string Text
  {
    get
    {
      return string.Format(ScriptLocalization.Objectives.PlaceDecorations, (object) this.Count, (object) this.Target);
    }
  }

  public Objectives_PlaceStructure()
  {
  }

  public Objectives_PlaceStructure(
    string groupId,
    StructureBrain.Categories category,
    int target,
    float expireDuration)
    : base(groupId, expireDuration)
  {
    this.Type = Objectives.TYPES.PLACE_STRUCTURES;
    this.Target = target;
    this.category = category;
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
    Objectives_PlaceStructure.FinalizedData_PlaceStructure finalizedData = new Objectives_PlaceStructure.FinalizedData_PlaceStructure();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.Target = this.Target;
    finalizedData.Count = this.Count;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  private void OnStructureAdded(StructuresData structure)
  {
    if (StructuresData.GetCategory(structure.Type) != this.category)
      return;
    ++this.Count;
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

  protected override bool CheckComplete() => this.Count >= this.Target;

  [Serializable]
  public class FinalizedData_PlaceStructure : ObjectivesDataFinalized
  {
    public int Target;
    public int Count;

    public override string GetText()
    {
      return string.Format(ScriptLocalization.Objectives.PlaceDecorations, (object) this.Count, (object) this.Target);
    }
  }
}
