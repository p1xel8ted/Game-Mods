// Decompiled with JetBrains decompiler
// Type: Objectives_NoCurses
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMBiomeGeneration;
using System;

#nullable disable
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

  private void OnSpellCast(EquipmentType curse)
  {
    if (Health.team2.Count <= 0 || RoomLockController.DoorsOpen || BiomeGenerator.Instance.RoomEntrance == BiomeGenerator.Instance.CurrentRoom)
      return;
    this.Failed();
    ObjectiveManager.CheckObjectives(this.Type);
    PlayerSpells.OnSpellCast -= new PlayerSpells.CastEvent(this.OnSpellCast);
  }

  [Serializable]
  public class FinalizedData_NoCurses : ObjectivesDataFinalized
  {
    public int RoomsRequired;
    public int RoomsCompleted;

    public override string GetText()
    {
      return string.Format(ScriptLocalization.Objectives.NoCurses, (object) this.RoomsCompleted, (object) this.RoomsRequired);
    }
  }
}
