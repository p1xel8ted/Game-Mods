// Decompiled with JetBrains decompiler
// Type: Objectives_NoDodge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_NoDodge : Objectives_RoomChallenge
{
  [IgnoreMember]
  public override string Text
  {
    get
    {
      return string.Format(ScriptLocalization.Objectives.NoDodge, (object) this.RoomsCompleted, (object) this.RoomsRequired);
    }
  }

  public Objectives_NoDodge()
  {
  }

  public Objectives_NoDodge(string groupId, int roomsRequired)
    : base(groupId, roomsRequired)
  {
    this.Type = Objectives.TYPES.NO_DODGE;
  }

  public override void Init(bool initialAssigning)
  {
    base.Init(initialAssigning);
    PlayerFarming.OnDodge += new PlayerFarming.PlayerEvent(this.PlayerFarming_OnDodge);
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_NoDodge.FinalizedData_NoDodge finalizedData = new Objectives_NoDodge.FinalizedData_NoDodge();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.RoomsRequired = this.RoomsRequired;
    finalizedData.RoomsCompleted = this.RoomsCompleted;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public void PlayerFarming_OnDodge()
  {
    if (Health.team2.Count <= 0 || RoomLockController.DoorsOpen || MMBiomeGeneration.BiomeGenerator.Instance.RoomEntrance == MMBiomeGeneration.BiomeGenerator.Instance.CurrentRoom)
      return;
    this.Failed();
    ObjectiveManager.CheckObjectives(this.Type);
    PlayerFarming.OnDodge -= new PlayerFarming.PlayerEvent(this.PlayerFarming_OnDodge);
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_NoDodge : ObjectivesDataFinalized
  {
    [Key(3)]
    public int RoomsRequired;
    [Key(4)]
    public int RoomsCompleted;

    public override string GetText()
    {
      return string.Format(ScriptLocalization.Objectives.NoDodge, (object) this.RoomsCompleted, (object) this.RoomsRequired);
    }
  }
}
