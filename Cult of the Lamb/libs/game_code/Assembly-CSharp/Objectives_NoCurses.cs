// Decompiled with JetBrains decompiler
// Type: Objectives_NoCurses
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_NoCurses : Objectives_RoomChallenge
{
  public override string Text
  {
    get
    {
      return string.Format(ScriptLocalization.Objectives.NoCurses, (object) this.RoomsCompleted, (object) this.RoomsRequired);
    }
  }

  public Objectives_NoCurses()
  {
  }

  public Objectives_NoCurses(string groupId, int roomsRequired)
    : base(groupId, roomsRequired)
  {
    this.Type = Objectives.TYPES.NO_CURSES;
  }

  public override void Init(bool initialAssigning)
  {
    base.Init(initialAssigning);
    PlayerSpells.OnSpellCast += new PlayerSpells.CastEvent(this.OnSpellCast);
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_NoCurses.FinalizedData_NoCurses finalizedData = new Objectives_NoCurses.FinalizedData_NoCurses();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.RoomsRequired = this.RoomsRequired;
    finalizedData.RoomsCompleted = this.RoomsCompleted;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public void OnSpellCast(EquipmentType curse)
  {
    if (Health.team2.Count <= 0 || RoomLockController.DoorsOpen || MMBiomeGeneration.BiomeGenerator.Instance.RoomEntrance == MMBiomeGeneration.BiomeGenerator.Instance.CurrentRoom)
      return;
    this.Failed();
    ObjectiveManager.CheckObjectives(this.Type);
    PlayerSpells.OnSpellCast -= new PlayerSpells.CastEvent(this.OnSpellCast);
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_NoCurses : ObjectivesDataFinalized
  {
    [Key(3)]
    public int RoomsRequired;
    [Key(4)]
    public int RoomsCompleted;

    public override string GetText()
    {
      return string.Format(ScriptLocalization.Objectives.NoCurses, (object) LocalizeIntegration.ReverseText(this.RoomsCompleted.ToString()), (object) LocalizeIntegration.ReverseText(this.RoomsRequired.ToString()));
    }
  }
}
