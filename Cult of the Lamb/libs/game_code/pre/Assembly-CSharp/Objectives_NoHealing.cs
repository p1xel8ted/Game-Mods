// Decompiled with JetBrains decompiler
// Type: Objectives_NoHealing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
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
    HealthPlayer.OnHeal += new HealthPlayer.HPUpdated(this.Health_OnHeal);
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_NoHealing.FinalizedData_NoHealing finalizedData = new Objectives_NoHealing.FinalizedData_NoHealing();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  private void Health_OnHeal(HealthPlayer player)
  {
    this.Failed();
    ObjectiveManager.CheckObjectives(this.Type);
    HealthPlayer.OnHeal -= new HealthPlayer.HPUpdated(this.Health_OnHeal);
  }

  [Serializable]
  public class FinalizedData_NoHealing : ObjectivesDataFinalized
  {
    public override string GetText() => "";
  }
}
