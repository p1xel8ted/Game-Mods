// Decompiled with JetBrains decompiler
// Type: Objectives_RoomChallenge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public abstract class Objectives_RoomChallenge : ObjectivesData
{
  public override string Text => "";

  public override bool AutoTrack => true;

  public int RoomsRequired { get; set; }

  public int RoomsCompleted { get; set; }

  public Objectives_RoomChallenge()
  {
  }

  public Objectives_RoomChallenge(string groupId, int roomsRequired)
    : base(groupId)
  {
    this.RoomsRequired = roomsRequired;
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  public override void Init(bool initialAssigning)
  {
    base.Init(initialAssigning);
    this.RoomsCompleted = 0;
    this.AutoRemoveQuestOnceComplete = false;
  }

  private void RoomLockController_OnRoomCleared()
  {
    ++this.RoomsCompleted;
    this.TryComplete();
  }

  protected override bool CheckComplete()
  {
    return this.initialised && !this.IsFailed && this.RoomsCompleted >= this.RoomsRequired;
  }
}
