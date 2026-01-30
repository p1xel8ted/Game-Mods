// Decompiled with JetBrains decompiler
// Type: Objectives_RoomChallenge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;
using System.Runtime.CompilerServices;

#nullable disable
[MessagePackObject(false)]
[Union(0, typeof (Objectives_NoDodge))]
[Union(1, typeof (Objectives_NoCurses))]
[Union(2, typeof (Objectives_NoDamage))]
[Union(3, typeof (Objectives_NoHealing))]
[Serializable]
public abstract class Objectives_RoomChallenge : ObjectivesData
{
  [CompilerGenerated]
  public int \u003CRoomsRequired\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CRoomsCompleted\u003Ek__BackingField;

  [IgnoreMember]
  public override string Text => "";

  public override bool AutoTrack => true;

  [Key(16 /*0x10*/)]
  public int RoomsRequired
  {
    get => this.\u003CRoomsRequired\u003Ek__BackingField;
    set => this.\u003CRoomsRequired\u003Ek__BackingField = value;
  }

  [Key(17)]
  public int RoomsCompleted
  {
    get => this.\u003CRoomsCompleted\u003Ek__BackingField;
    set => this.\u003CRoomsCompleted\u003Ek__BackingField = value;
  }

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

  public void RoomLockController_OnRoomCleared()
  {
    ++this.RoomsCompleted;
    this.TryComplete();
  }

  public override bool CheckComplete()
  {
    return this.initialised && !this.IsFailed && this.RoomsCompleted >= this.RoomsRequired;
  }
}
