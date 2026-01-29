// Decompiled with JetBrains decompiler
// Type: Objectives_NoDamage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_NoDamage : Objectives_RoomChallenge
{
  public override string Text
  {
    get
    {
      return string.Format(ScriptLocalization.Objectives.NoDamage, (object) this.RoomsCompleted, (object) this.RoomsRequired);
    }
  }

  public Objectives_NoDamage()
  {
  }

  public Objectives_NoDamage(string groupId, int roomsRequired)
    : base(groupId, roomsRequired)
  {
    this.Type = Objectives.TYPES.NO_DAMAGE;
  }

  public override void Init(bool initialAssigning)
  {
    base.Init(initialAssigning);
    foreach (PlayerFarming player in PlayerFarming.players)
      player.health.OnDamaged += new HealthPlayer.HPUpdated(this.HealthPlayer_OnDamaged);
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_NoDamage.FinalizedData_NoDamage finalizedData = new Objectives_NoDamage.FinalizedData_NoDamage();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.RoomsRequired = this.RoomsRequired;
    finalizedData.RoomsCompleted = this.RoomsCompleted;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public void HealthPlayer_OnDamaged(HealthPlayer Target)
  {
    if (Health.team2.Count <= 0 || RoomLockController.DoorsOpen || MMBiomeGeneration.BiomeGenerator.Instance.RoomEntrance == MMBiomeGeneration.BiomeGenerator.Instance.CurrentRoom)
      return;
    this.Failed();
    ObjectiveManager.CheckObjectives(this.Type);
    foreach (PlayerFarming player in PlayerFarming.players)
      player.health.OnDamaged -= new HealthPlayer.HPUpdated(this.HealthPlayer_OnDamaged);
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_NoDamage : ObjectivesDataFinalized
  {
    [Key(3)]
    public int RoomsRequired;
    [Key(4)]
    public int RoomsCompleted;

    public override string GetText()
    {
      return string.Format(ScriptLocalization.Objectives.NoDamage, (object) this.RoomsCompleted, (object) this.RoomsRequired);
    }
  }
}
