// Decompiled with JetBrains decompiler
// Type: Objectives_PlaceStructure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_PlaceStructure : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public StructureBrain.Categories category;
  [Key(17)]
  public Objectives_PlaceStructure.DecorationType DecoType;
  [Key(18)]
  public int Count;
  [Key(19)]
  public int Target;
  [Key(20)]
  public string Term;
  [Key(21)]
  public bool IncludeAlreadyBuilt;

  public static string GetLocaString(Objectives_PlaceStructure.DecorationType decoType)
  {
    switch (decoType)
    {
      case Objectives_PlaceStructure.DecorationType.WOOLHAVEN_DECO:
        return ScriptLocalization.Objectives.PlaceWoolhavenDecos;
      case Objectives_PlaceStructure.DecorationType.ROT_DECO:
        return ScriptLocalization.Objectives.PlaceRotDecos;
      case Objectives_PlaceStructure.DecorationType.EWEFALL_DECO:
        return ScriptLocalization.Objectives.PlaceEwefallDecos;
      case Objectives_PlaceStructure.DecorationType.WOOLHAVEN_MISC:
        return ScriptLocalization.Objectives.PlaceSteampunkDecos;
      default:
        return ScriptLocalization.Objectives.PlaceDecorations;
    }
  }

  public override string Text
  {
    get
    {
      return string.Format(Objectives_PlaceStructure.GetLocaString(this.DecoType), (object) this.Count, (object) this.Target);
    }
  }

  public Objectives_PlaceStructure()
  {
  }

  public Objectives_PlaceStructure(
    string groupId,
    StructureBrain.Categories category,
    Objectives_PlaceStructure.DecorationType decoType,
    int target,
    float expireDuration,
    bool includeAlreadyBuilt = false)
    : base(groupId, expireDuration)
  {
    this.Type = Objectives.TYPES.PLACE_STRUCTURES;
    this.Target = target;
    this.category = category;
    this.DecoType = decoType;
    this.IncludeAlreadyBuilt = includeAlreadyBuilt;
    this.Count = includeAlreadyBuilt ? this.CurrentMatching() : 0;
  }

  public override void Init(bool initialAssigning)
  {
    if (!this.initialised)
      StructureManager.OnStructureAdded += new StructureManager.StructureChanged(this.OnStructureAdded);
    if (initialAssigning)
      this.Count = this.IncludeAlreadyBuilt ? this.CurrentMatching() : 0;
    base.Init(initialAssigning);
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_PlaceStructure.FinalizedData_PlaceStructure finalizedData = new Objectives_PlaceStructure.FinalizedData_PlaceStructure();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.DecoType = this.DecoType;
    finalizedData.Target = this.Target;
    finalizedData.Count = this.Count;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public void OnStructureAdded(StructuresData structure)
  {
    Debug.Log((object) $"PlaceStructure has detected {structure.Type} being added");
    if (this.DecoType == Objectives_PlaceStructure.DecorationType.ANY)
    {
      if (StructuresData.GetCategory(structure.Type) != this.category)
        return;
      ++this.Count;
    }
    else
    {
      if (!this.MatchesDecoType(structure))
        return;
      ++this.Count;
    }
  }

  public bool MatchesDecoType(StructuresData structure)
  {
    if (this.DecoType == Objectives_PlaceStructure.DecorationType.ANY)
      return true;
    switch (this.DecoType)
    {
      case Objectives_PlaceStructure.DecorationType.WOOLHAVEN_DECO:
        return StructureBrain.WoolhavenDecos.Contains(structure.Type);
      case Objectives_PlaceStructure.DecorationType.ROT_DECO:
        return StructureBrain.RotDecos.Contains(structure.Type);
      case Objectives_PlaceStructure.DecorationType.EWEFALL_DECO:
        return StructureBrain.EwefallDecos.Contains(structure.Type);
      case Objectives_PlaceStructure.DecorationType.WOOLHAVEN_MISC:
        return StructureBrain.MiscWoolhavenDecos.Contains(structure.Type);
      default:
        return true;
    }
  }

  public override void Complete()
  {
    base.Complete();
    StructureManager.OnStructureAdded -= new StructureManager.StructureChanged(this.OnStructureAdded);
    Debug.Log((object) $"PlaceStructure complete...(GroupID={this.GroupId})");
  }

  public override void Failed()
  {
    base.Failed();
    StructureManager.OnStructureAdded -= new StructureManager.StructureChanged(this.OnStructureAdded);
  }

  public override bool CheckComplete() => this.Count >= this.Target;

  public int CurrentMatching()
  {
    return StructureManager.AccumulateOnAllStructures(FollowerLocation.Base, (Func<StructureBrain, int>) (s => !this.MatchesDecoType(s.Data) ? 0 : 1));
  }

  [CompilerGenerated]
  public int \u003CCurrentMatching\u003Eb__20_0(StructureBrain s)
  {
    return !this.MatchesDecoType(s.Data) ? 0 : 1;
  }

  public enum DecorationType
  {
    ANY,
    WOOLHAVEN_DECO,
    ROT_DECO,
    EWEFALL_DECO,
    WOOLHAVEN_MISC,
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_PlaceStructure : ObjectivesDataFinalized
  {
    [Key(3)]
    public Objectives_PlaceStructure.DecorationType DecoType;
    [Key(4)]
    public int Target;
    [Key(5)]
    public int Count;

    public override string GetText()
    {
      return string.Format(Objectives_PlaceStructure.GetLocaString(this.DecoType), (object) this.Count, (object) this.Target);
    }
  }
}
