// Decompiled with JetBrains decompiler
// Type: Objectives_NoHealing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_NoHealing : Objectives_RoomChallenge
{
  public override string Text
  {
    get => string.Format("REMOVED", (object) this.RoomsCompleted, (object) this.RoomsRequired);
  }

  public Objectives_NoHealing()
  {
  }

  public Objectives_NoHealing(string groupId, int roomsRequired)
    : base(groupId, roomsRequired)
  {
    this.Type = Objectives.TYPES.NO_HEALING;
  }

  public override void Init(bool initialAssigning)
  {
    base.Init(initialAssigning);
    foreach (PlayerFarming player in PlayerFarming.players)
      player.health.OnHeal += new HealthPlayer.HPUpdated(this.Health_OnHeal);
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_NoHealing.FinalizedData_NoHealing finalizedData = new Objectives_NoHealing.FinalizedData_NoHealing();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public void Health_OnHeal(HealthPlayer player)
  {
    this.Failed();
    ObjectiveManager.CheckObjectives(this.Type);
    foreach (PlayerFarming player1 in PlayerFarming.players)
      player1.health.OnHeal -= new HealthPlayer.HPUpdated(this.Health_OnHeal);
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_NoHealing : ObjectivesDataFinalized
  {
    public override string GetText() => "";
  }
}
