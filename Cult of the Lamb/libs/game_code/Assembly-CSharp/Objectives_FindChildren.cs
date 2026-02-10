// Decompiled with JetBrains decompiler
// Type: Objectives_FindChildren
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_FindChildren : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public Objectives_FindChildren.ChildLocation Location;
  [IgnoreMember]
  public bool complete;

  public override string Text
  {
    get => ("Objectives/FindChildren/" + this.Location.ToString()).Localized();
  }

  public Objectives_FindChildren()
  {
  }

  public Objectives_FindChildren(
    string groupId,
    Objectives_FindChildren.ChildLocation location,
    float questExpireDuration = -1f)
    : base(groupId, questExpireDuration)
  {
    this.Type = Objectives.TYPES.FIND_CHILDREN;
    this.Location = location;
    this.subtitle = $"UI/JobBoard/Hint/Tarot/{(int) this.Location}";
  }

  public override void Init(bool initialAssigning)
  {
    base.Init(initialAssigning);
    this.complete = false;
    this.subtitle = $"UI/JobBoard/Hint/Tarot/{(int) this.Location}";
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_FindChildren.FinalizedData_FindChildren finalizedData = new Objectives_FindChildren.FinalizedData_FindChildren();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.Location = this.Location;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public override bool CheckComplete() => this.IsComplete;

  public enum ChildLocation
  {
    Woolhaven,
    Temple,
    PilgrimsPassage,
    SporeGrotto,
    SmugglersSanctuary,
    MidasCave,
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_FindChildren : ObjectivesDataFinalized
  {
    [Key(3)]
    public Objectives_FindChildren.ChildLocation Location;

    public override string GetText()
    {
      return ("Objectives/FindChildren/" + this.Location.ToString()).Localized();
    }
  }
}
